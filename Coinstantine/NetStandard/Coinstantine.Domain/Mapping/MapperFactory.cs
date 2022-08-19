using Coinstantine.Common.Attributes;
using MvvmCross;

namespace Coinstantine.Domain.Mapping
{
    [RegisterInterfaceAsDynamic]
    public class MapperFactory : IMapperFactory
    {
        public IMapper<TSource, TDestination> GetMapper<TSource, TDestination>()
        {
            return Mvx.Resolve<IMapper<TSource, TDestination>>();
        }
    }
}
