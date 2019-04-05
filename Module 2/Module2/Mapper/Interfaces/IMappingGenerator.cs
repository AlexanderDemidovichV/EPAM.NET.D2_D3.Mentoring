namespace Mapper.Interfaces
{
    public interface IMappingGenerator
    {
        IMapper<TSource, TDestination> Generate<TSource, TDestination>();
    }
}
