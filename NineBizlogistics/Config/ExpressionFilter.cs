using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace NineBizlogistics.Config
{
    /// <summary>
    /// Expression动态组合，可用于数据库筛选，一些不确定的筛选参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExpressionFilter<T> : ExpressionVisitor
    {
        Expression<Func<T, bool>> _Filter;
        Dictionary<ParameterExpression, ParameterExpression> map;
        public ExpressionFilter(bool BaseFilter = true)
        {
            _Filter = T => BaseFilter;
        }
        Func<T, bool> Func
        {
            get { return _Filter.Compile(); }
            set { }
        }
        Expression<Func<T, bool>> Expressions
        {
            get { return _Filter; }
            set { }
        }

        public void Or(Expression<Func<T, bool>> expression)
        {
            _Filter = Compose(_Filter, expression, Expression.OrElse);

        }
        public void And(Expression<Func<T, bool>> expression)
        {
            _Filter = Compose(_Filter, expression, Expression.AndAlso);
        }

        public static implicit operator Func<T, bool>(ExpressionFilter<T> filter)
        {
            return filter.Func;
        }
        public static implicit operator Expression<Func<T, bool>>(ExpressionFilter<T> filter)
        {
            return filter.Expressions;
        }
        private Expression<TT> Compose<TT>(Expression<TT> first, Expression<TT> second, Func<Expression, Expression, Expression> merge)
        {
            map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            var secondBody = Visit(second.Body);
            return Expression.Lambda<TT>(merge(first.Body, secondBody), first.Parameters);
        }
        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }      
    }
}
