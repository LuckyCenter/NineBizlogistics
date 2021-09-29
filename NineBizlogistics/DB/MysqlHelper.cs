using Chloe;
using Chloe.Annotations;
using Chloe.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace NineBizlogistics.DB
{
    public class MysqlHelper : DBHelper
    {
        public MysqlHelper()
        {
            Dic_Map_Type = new Dictionary<Type, MapClass>()
        {
            {typeof (int ),new MapClass (){ MapType="INT NOT NULL", DefaultValue="0" } },  //常用类型与SQL数据类型映射
            {typeof (long  ),new MapClass (){ MapType="BIGINT NOT NULL", DefaultValue="0" } } ,
            {typeof (Guid  ), new MapClass (){ MapType="VARCHAR(36)", DefaultValue="{00000000-0000-0000-0000-000000000000}" }} ,
            {typeof (uint  ),new MapClass (){ MapType="INT UNSIGNED NOT NULL", DefaultValue="0" } } ,
            {typeof (ulong  ),  new MapClass (){ MapType="BIGINT UNSIGNED NOT NULL", DefaultValue="0" }} ,
            {typeof (bool  ),  new MapClass (){ MapType="BOOL NOT NULL", DefaultValue="false" }} ,
            {typeof (float  ), new MapClass (){ MapType="FLOAT NOT NULL", DefaultValue="0" } },
            {typeof (double  ), new MapClass (){ MapType="DOUBLE NOT NULL", DefaultValue="0" } },
            {typeof (DateTime  ),  new MapClass (){ MapType="DATETIME  NULL", DefaultValue="NULL" }},
            {typeof (string  ),  new MapClass (){ MapType="VARCHAR(1024) ", DefaultValue="" }},
            {typeof (byte  ), new MapClass (){ MapType="TINYINT UNSIGNED NOT NULL", DefaultValue="0" } } ,
            {typeof (sbyte  ),  new MapClass (){ MapType="TINYINT NULL", DefaultValue="0" }}  ,
            {typeof (short  ), new MapClass (){ MapType="SMALLINT NOT NULL", DefaultValue="0" } }  ,
            {typeof (ushort  ),  new MapClass (){ MapType="SMALLINT UNSIGNED NOT NULL", DefaultValue="0" } }  ,
        };
        }
        public static string DBName { get; set; }
        public override bool InitTable(IDbConnectionFactory factory, List<Type> mapClass, List<object> insertObj = null)
        {
            bool result = false;

            DBTableBase.InitContextFactory(factory, fac =>
            {
                return new Chloe.MySql.MySqlContext(fac);
            });
            using (var context = DBTableBase.GetCommomContext())
            {

                try
                {
                    MysqlConnectionFactory mcf = (MysqlConnectionFactory)factory;
                    if (!IsDataBaseExist(context, mcf.DataBase))
                    {
                        //创建数据库
                        CreatDatabase(context, mcf.DataBase);
                    }
                    DBName = mcf.DataBase;
                    //第一次创建数据库时，不能指定数据库，后面访问数据库时需要指定
                    factory = new MysqlConnectionFactory(mcf.Server, mcf.Port, mcf.UserID, mcf.Password, mcf.DataBase, true);
                    DBTableBase.InitContextFactory(factory, fac =>
                    {
                        return new Chloe.MySql.MySqlContext(fac);
                    });
                    if (context.Session.CurrentConnection.State != System.Data.ConnectionState.Open)
                    {
                        context.Session.CurrentConnection.Open();
                    }
                    context.Session.CurrentConnection.ChangeDatabase(mcf.DataBase);
                    List<Type> LsNewType = new List<Type>();
                    context.Session.BeginTransaction();

                    foreach (Type type in mapClass.Distinct())
                    {
                        if (IsTableExist(context, type.Name))
                        {
                            foreach (var p in type.GetProperties())
                            {
                                var atts = p.GetCustomAttributes(false);
                                if (atts.FirstOrDefault(zz => zz.GetType() == typeof(NotMappedAttribute)) != null) { continue; }

                                if (!IsColumnExist(context, type.Name, p.Name))
                                {
                                    var defaultclass = type.Assembly.CreateInstance(type.FullName);
                                    ColumnAdd(context, type.Name, p.Name, Dic_Map_Type.ContainsKey(p.PropertyType) ? Dic_Map_Type[p.PropertyType].MapType : p.PropertyType.Name, Dic_Map_Type.ContainsKey(p.PropertyType) ? Dic_Map_Type[p.PropertyType].DefaultValue : "");
                                    defaultclass = null;
                                }
                            }
                        }
                        else
                        {
                            CreatTableIfNoExist(context, type);
                            LsNewType.Add(type);
                        }
                    }

                    if (LsNewType.Count > 0)
                    {
                        if (insertObj?.Count > 0)
                        {
                            foreach (var o in insertObj)
                            {
                                if (LsNewType.Contains(o.GetType()))
                                {
                                    context.Insert(o);
                                }
                            }
                        }
                    }
                    context.Session.CommitTransaction();
                    result = true;
                }
                catch (Exception ex)
                {
                    SetErr(ex);
                }
            }
            return result;
        }


       
        protected override bool IsTableExist(IDbContext context, string tablename)
        {

            string sql = $"select count(*) from information_schema.TABLES where table_name='{tablename }' and table_schema='{DBName }'";
            bool result = false;
            result = int.Parse(context.Session.ExecuteScalar(sql).ToString()) > 0;
            return result;
        }


        protected override void CreatTableIfNoExist(IDbContext context, Type T)
        {
            var Properity = T.GetProperties();
            List<string> ls = new List<string>();
            string primarykey = null;
            foreach (var p in Properity)
            {
                var atts = p.GetCustomAttributes(false);
                if (atts.FirstOrDefault(zz => zz.GetType() == typeof(NotMappedAttribute)) != null) { continue; }
                bool iskey = false;
                bool isautomatic = false;

                foreach (var att in atts)
                {
                    if (att.GetType() == typeof(ColumnAttribute))
                    {
                        var atta = att as ColumnAttribute;
                        iskey = atta.IsPrimaryKey;

                        break;
                    }
                    if (att.GetType() == typeof(AutoIncrementAttribute))
                    {
                        isautomatic = true;
                    }
                }
                MapClass type = null;
                if (Dic_Map_Type.ContainsKey(p.PropertyType))
                {
                    type = Dic_Map_Type[p.PropertyType];
                }
                else
                {
                    throw new Exception("不支持的数据类型");
                }

                string basesql = $"`{p.Name  }` {type.MapType } ";
                if (iskey)
                {
                    primarykey = $" PRIMARY KEY (`{p.Name }`)";
                }
                if (isautomatic)
                {
                    basesql += " AUTO_INCREMENT ";
                }
                ls.Add(basesql);

            }
            string sql = $"create table if not exists `{T.Name }`({string.Join(",", ls)},{primarykey})";
            context.Session.ExecuteNonQuery(sql);

        }

        protected override bool IsColumnExist(IDbContext context, string tablename, string columnname)
        {
            var sql = $"select count(*) from information_schema.columns where table_name = '{tablename }' and column_name = '{columnname}' and table_schema='{DBName }'";
            return int.Parse(context.Session.ExecuteScalar(sql).ToString()) > 0;
        }

        protected override bool ColumnAdd(IDbContext context, string tablename, string columnname, string type, string DefaultValue)
        {
            bool result = true;
            string sql = $"alter table {tablename} add column {columnname} {type} DEFAULT  '{DefaultValue}'";
            try
            {
                context.Session.ExecuteNonQuery(sql);
            }
            catch
            {
                result = false;
            }
            return result;
        }
        protected override bool IsDataBaseExist(IDbContext context, string database)
        {
            string sql = $"select count(*) from information_schema.SCHEMATA where schema_name = '{database }'";
            return int.Parse(context.Session.ExecuteScalar(sql).ToString()) > 0;
        }
        protected override void CreatDatabase(IDbContext context, string database)
        {
            string sql = $"CREATE DATABASE IF NOT EXISTS {database }  CHARACTER SET utf8";
            context.Session.ExecuteNonQuery(sql);
        }
    }
}
