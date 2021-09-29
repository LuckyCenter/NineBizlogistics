using Chloe;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NineBizlogistics.DB;

namespace NineBizlogistics.DB
{
    public class DBBase<T> : DBTableBase where T : DBTableBase
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="t"></param>
        /// <param name="conns">连接</param>
        public static T Add(T t, IDbContext context = null)
        {
            return Run(conn =>
            {
                return conn.Insert(t);
            }, context);
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="conns">连接</param>
        public static void AddRange(List<T> ls, IDbContext context = null)
        {
            Run(conn =>
            {
                conn.InsertRange(ls);
            }, context);
        }

        /// <summary>
        /// 删除指定条件的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="conns"></param>
        /// <returns></returns>
        public static int Delete(Expression<Func<T, bool>> filter = null, IDbContext context = null)
        {
            return Run(conn =>
            {
                if (filter == null) { filter = zz => true; }
                return conn.Delete<T>(filter);
            }, context);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="item"></param>
        /// <param name="context"></param>
        public static void Update(Expression<Func<T, bool>> filter, Expression<Func<T, T>> item, IDbContext context = null)
        {
            Run(conn =>
            {
                return conn.Update<T>(filter, item);
            }, context);
        }
        public static void UpdateById(T item, IDbContext context = null)
        {
            Run(conn =>
            {
                return conn.Update<T>(item);
            }, context);
        }


        /// <summary>
        /// 获取默认第一个数据
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static T FirstOrDefault(Expression<Func<T, bool>> filter = null, IDbContext context = null)
        {
            return Run(conn =>
            {
                if (filter == null) { filter = zz => true; }
                return conn.Query<T>().Where(filter).TakePage(1, 1).FirstOrDefault();
            }, context);
        }
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<T> TakeAll(Expression<Func<T, bool>> filter = null, IDbContext context = null)
        {
            return Run(conn =>
            {
                if (filter == null) { filter = zz => true; }
                return conn.Query<T>().Where(filter).TakePage(1, int.MaxValue).ToList();
            }, context);
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>my        
        /// <returns></returns>
        public static List<T> TakePage(Expression<Func<T, bool>> filter, int pageNumber, int pageSize, IDbContext context = null)
        {
            return Run(conn =>
            {
                if (filter == null) { filter = zz => true; }
                return conn.Query<T>().Where(filter).TakePage(pageNumber, pageSize).ToList();
            }, context);
        }
        public static List<T> TakePageDesc(Expression<Func<T, bool>> filter, int pageNumber, int pageSize, IDbContext context = null)
        {
            return Run(conn =>
            {
                if (filter == null) { filter = zz => true; }
                return conn.Query<T>().OrderByDesc(zz => zz.Id).Where(filter).TakePage(pageNumber, pageSize).ToList();
            }, context);
        }
        /// <summary>
        /// 获取复合条件的数据数量
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="configKey"></param>
        /// <param name="conns"></param>
        /// <returns></returns>
        public static int Count(Expression<Func<T, bool>> filter = null, IDbContext context = null)
        {
            return Run(conn =>
            {
                if (filter == null) { filter = zz => true; }
                var query = conn.Query<T>();
                return query.Where(filter).Count();
            }, context);

        }
        /// <summary>
        /// 指定的数据是否存在
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool Exist(Expression<Func<T, bool>> filter = null, IDbContext context = null)
        {
            return Run(conn =>
            {
                if (filter == null) { filter = zz => true; }
                var query = conn.Query<T>();
                return query.Any(filter);
            }, context);

        }
        /// <summary>
        /// 查询，连表
        /// </summary>
        /// <param name="configKey"></param>
        /// <returns></returns>
        public static IQuery<T> Query()
        {
            return GetCommomContext().Query<T>();
        }
        /// <summary>
        /// 连接管理工厂,自动创建和销毁连接
        /// </summary>
        /// <param name="context">如果为空，自动创建和销毁configkey连接，不为空则使用后不销毁</param>
        /// <param name="A"></param>
        /// <param name="configKey"></param>
        public static void Run(Action<IDbContext> A, IDbContext context = null)
        {
            bool newson = false;
            if (context == null)
            {
                context = GetCommomContext();
                newson = true;
            }
            A.Invoke(context);
            if (newson)
            {
                context.Dispose();
            }
        }
        /// <summary>
        /// 异步连接管理工厂，自动创建和销毁连接
        /// </summary>
        /// <param name="context"></param>
        /// <param name="A"></param>
        /// <param name="configKey"></param>
        public static void RunAsync(Action<IDbContext> A, IDbContext context = null)
        {
            Task.Factory.StartNew(new Action(() =>
            {
                Run(A, context);
            }));
        }
        /// <summary>
        /// 自动创建和销毁连接
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn">如果为空，自动创建和销毁连接，不为空则使用后不销毁</param>
        /// <param name="A"></param>
        /// <param name="configKey"></param>
        /// <returns></returns>
        public static T2 Run<T2>(Func<IDbContext, T2> A, IDbContext conn = null)
        {
            bool newson = false;
            if (conn == null)
            {
                conn = GetCommomContext();
                newson = true;
            }
            var data = A.Invoke(conn);
            if (newson)
            {
                conn.Dispose();
            }
            return data;
        }
    }
}
