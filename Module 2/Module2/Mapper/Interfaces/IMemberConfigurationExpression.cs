using System;

namespace Mapper.Interfaces
{
    public interface IMemberConfigurationExpression<out TSource, out TDestination, out TMember> : IMemberConfigurationExpression
    {
        void MapFrom<TResult>(Func<TSource, TDestination, TResult> mappingFunction);

        void MapFrom<TResult>(Func<TSource, TDestination, TMember, TResult> mappingFunction);

        void Ignore();
    }

    public interface IMemberConfigurationExpression
    {

    }
}
