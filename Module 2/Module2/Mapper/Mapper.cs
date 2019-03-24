using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Mapper
{
    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(TSource source);

        IMapper<TSource, TDestination> ForMember(
            Expression<Func<TSource, object>> source,
            Expression<Func<TDestination, object>> destination);
    }

    public class Mapper<TSource, TDestination> : IMapper<TSource, TDestination>
    {
        private readonly Dictionary<Expression<Func<TSource, object>>, 
            Expression<Func<TDestination, object>>> _mappings =
            new Dictionary<Expression<Func<TSource, object>>, 
                Expression<Func<TDestination, object>>>();

        readonly Func<TSource, TDestination> _mapFunction;

        internal Mapper(Func<TSource, TDestination> func)
        {
            _mapFunction = func;
        }

        public TDestination Map(TSource source)
        {
            var destination = _mapFunction(source);
            foreach (var srcMap in _mappings.Keys)
            {
                var srcValue = (srcMap.Compile())(source);
                string destPropertyName;
                if (_mappings[srcMap].Body is UnaryExpression expression)
                {
                    destPropertyName = (expression.Operand as MemberExpression)?.
                        Member.Name;
                }
                else
                {
                    destPropertyName = (_mappings[srcMap].Body as MemberExpression)?.
                        Member.Name;
                }
                var destPropInfo = destination.GetType().
                    GetProperty(destPropertyName ?? 
                    throw new MemberConfigurationException());
                if (destPropInfo != null)
                    destPropInfo.SetValue(destination, srcValue);
            }
            return destination;
        }

        public IMapper<TSource, TDestination> ForMember(
            Expression<Func<TSource, object>> source,
            Expression<Func<TDestination, object>> destination)
        {
            _mappings.Add(source, destination);
            return this;
        }
    }

    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            var sourceParam = Expression.Parameter(typeof(TSource));
            var mapFunction =
                Expression.Lambda<Func<TSource, TDestination>>(
                    Expression.New(typeof(TDestination)),
                    sourceParam
                );

            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }
    }
}
