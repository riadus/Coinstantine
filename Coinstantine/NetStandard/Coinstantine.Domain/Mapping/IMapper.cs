namespace Coinstantine.Domain.Mapping
{
    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(TSource source);
        TSource MapBack(TDestination source);
    }

    public interface IMapperFactory
    {
        IMapper<TSource, TDestination> GetMapper<TSource, TDestination>();
    }
}
