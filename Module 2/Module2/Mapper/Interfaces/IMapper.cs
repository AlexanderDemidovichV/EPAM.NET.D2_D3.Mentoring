using System;
using System.Linq.Expressions;

namespace Mapper.Interfaces
{
    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(TSource source);

        Mapper.IMapper<TSource, TDestination> ForMember(
            Expression<Func<TSource, object>> source,
            Expression<Func<TDestination, object>> destination);
    }
}
