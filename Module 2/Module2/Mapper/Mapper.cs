using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mapper.Interfaces;

namespace Mapper
{
    public interface IMappingExpression<TSource, TDestination>
    {
        IMappingExpression<TSource, TDestination> ForMember(
            Expression<Func<TSource, object>> source,
            Expression<Func<TDestination, object>> destination);
    }

    public class Mapper<TSource, TDestination>: IMappingExpression<TSource, TDestination>
    {
        private readonly Dictionary<Expression<Func<TSource, object>>, 
            Expression<Func<TDestination, object>>> _mappings = 
                new Dictionary<Expression<Func<TSource, object>>, 
                    Expression<Func<TDestination, object>>>();

        private readonly Func<TSource, TDestination> _mapFunction;

        internal Mapper(Func<TSource, TDestination> func)
        {
            _mapFunction = func;
        }

        public TDestination Map(TSource source)
        {
            //var memberSelectorExpression = memberLamda.Body as MemberExpression;
            //if (memberSelectorExpression != null)
            //{
            //    var property = memberSelectorExpression.Member as PropertyInfo;
            //    if (property != null)
            //    {
            //        property.SetValue(target, value, null);
            //    }
            //}
            var destination = _mapFunction(source);
            foreach (var srcMap in _mappings.Keys)
            {
                object srcValue = (srcMap.Compile())(source);
                var destPropertyName = (_mappings[srcMap].Body as MemberExpression).Member.Name;
                PropertyInfo destPropInfo = destination.GetType().GetProperty(destPropertyName);
                destPropInfo.SetValue(destination, srcValue);
            }
            return destination;
        }

        public IMappingExpression<TSource, TDestination> ForMember(
            Expression<Func<TSource, object>> source, 
            Expression<Func<TDestination, object>> destination)
        {
            _mappings.Add(source, destination);
            return this;
        }
    }
    public class Mapper2<TSource, TDestination>
    {
        private Dictionary<Expression<Func<TSource, object>>, Expression<Func<TDestination, object>>> mappings =
            new Dictionary<Expression<Func<TSource, object>>, Expression<Func<TDestination, object>>>();

        Func<TSource, TDestination> mapFunction;
        internal Mapper2(Func<TSource, TDestination> func)
        {
            mapFunction = func;
        }
        public TDestination Map(TSource source)
        {
            var destination = mapFunction(source);
            foreach (var srcMap in mappings.Keys)
            {
                object srcValue = (srcMap.Compile())(source);
                var destPropertyName = (mappings[srcMap].Body as MemberExpression).Member.Name;
                PropertyInfo destPropInfo = destination.GetType().GetProperty(destPropertyName);
                destPropInfo.SetValue(destination, srcValue);
            }
            return destination;
        }

        public void AddMapping(Expression<Func<TSource, object>> source, Expression<Func<TDestination, object>> destination)
        {
            mappings.Add(source, destination);
        }
    }

    public class MappingGenerator
    {
        public Mapper2<TSource, TDestination> Generate<TSource, TDestination>()
        {
            var sourceParam = Expression.Parameter(typeof(TSource));
            var mapFunction =
                Expression.Lambda<Func<TSource, TDestination>>(
                    Expression.New(typeof(TDestination)),
                    sourceParam
                );

            return new Mapper2<TSource, TDestination>(mapFunction.Compile());
        }
    }
}
