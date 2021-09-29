using Chloe;
using Chloe.Annotations;
using Chloe.Infrastructure;
using Newtonsoft.Json;
using System;

namespace NineBizlogistics.DB
{
    /// <summary>
    /// 数据库专用（可建立数据库）
    /// </summary>
    public abstract class DBTableBase
    {
        [AutoIncrement]
        [Column(IsPrimaryKey = true)]
        [JsonIgnore]
        public long Id { get; set; }
        public string Uid { get; set; } = Guid.NewGuid().ToString();
       
      
        static IDbConnectionFactory myfactory = null;
        static Func<IDbConnectionFactory, IDbContext> mycontext = null;
        /// <summary>
        /// 初始化连接工厂和context委托，系统自动调用
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="contextf"></param>
        public static void InitContextFactory(IDbConnectionFactory factory, Func<IDbConnectionFactory, IDbContext> contextf)
        {
            if (factory == null) { throw new Exception("连接不能为空"); }
            myfactory = factory;
            mycontext = contextf;
        }
        /// <summary>
        /// 获取通用的context
        /// </summary>
        /// <returns></returns>
        public  static IDbContext GetCommomContext()
        {
            return mycontext.Invoke(myfactory);
        }
    }
}
