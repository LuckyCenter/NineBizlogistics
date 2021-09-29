using ModelClass.DB;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using NineBizlogistics.Model;
using NineBizlogistics.DB;

namespace NineBizlogistics.Config
{
    /// <summary>
    /// 数据表管理
    /// </summary>
    public class Tables
    {
        /// <summary>
        /// 初始化数据库表
        /// </summary>
        public static void Init()
        {
            List<Type> Tables = new List<Type>();
            Tables.Add(typeof(Customer));
            Tables.Add(typeof(UserInfo));
            Tables.Add(typeof(SystemConfig));
            
            //初始化数据
            List<object> InitData = new List<object>();
            InitData.Add(new UserInfo()
            {
                UserName = "admin",
                Pwd = "123456",//123456
                Contact = "",
                PersonName = "管理员",

            });
            InitData.Add(new UserInfo()
            {
                UserName = "hospital",
                Pwd = "123456",
                Contact = "",
                PersonName = "管理员",

            });

            //  SQLiteInit(Tables, InitData);
            MySqlInit(Tables, InitData);

        }
        
        static void MySqlInit(List<Type> Tables, List<object> InitData)
        {
            MysqlConnectionFactory SCF = new MysqlConnectionFactory(GlobalSetting.MysqlHost, GlobalSetting.MysqlPort, GlobalSetting.MysqlUserName, GlobalSetting.MysqlPwd, GlobalSetting.MysqlDBName);//建立数据库时不能限制数据库名
            NineBizlogistics.DB.MysqlHelper SH = new MysqlHelper();
            SH.InitTable(SCF, Tables, InitData);
        }
    }
}
