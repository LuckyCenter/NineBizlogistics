using Chloe;
using Chloe.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NineBizlogistics.DB
{
    /// <summary>
    /// 代码优先
    /// </summary>
    public abstract class DBHelper
    {
        public event Action<Exception> OnErr;
        protected void SetErr(Exception ex)
        {
            Task.Run(() =>
            {
                OnErr?.Invoke(ex);
            });

        }
        /// <summary>
        /// 初始化数据表格
        /// </summary>
        /// <param name="factory">连接工厂</param>
        /// <param name="mapClass">实体映射类</param>
        /// <param name="insertObj">插入的对象实体</param>
        /// <returns></returns>
        public virtual bool InitTable(IDbConnectionFactory factory, List<Type> mapClass, List<object> insertObj = null)
        {
            return false;
        }

        protected class MapClass
        {
            public string MapType { get; set; }
            public string DefaultValue { get; set; }
        }
        protected Dictionary<Type, MapClass> Dic_Map_Type { get; set; }
        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        protected virtual bool IsTableExist(IDbContext context, string tablename)
        {
            return false;
        }
        /// <summary>
        /// 创建表（如果不存在）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="T"></param>
        protected virtual void CreatTableIfNoExist(IDbContext context, Type T)
        {

        }
        /// <summary>
        /// 列是否存在
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tablename"></param>
        /// <param name="columnname"></param>
        /// <returns></returns>
        protected virtual bool IsColumnExist(IDbContext context, string tablename, string columnname)
        {
            return false;
        }
        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tablename"></param>
        /// <param name="columnname"></param>
        /// <param name="type"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        protected virtual bool ColumnAdd(IDbContext context, string tablename, string columnname, string type, string DefaultValue)
        {
            return false;
        }

        protected virtual bool IsDataBaseExist(IDbContext context, string database)
        {
            return false;
        }
        protected virtual void CreatDatabase(IDbContext context, string database)
        {

        }
    }
}
