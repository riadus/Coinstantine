using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Data;

namespace Coinstantine.Domain.Interfaces
{
    public interface ICountriesProvider
    {
        Task<IEnumerable<Country>> GetCountries();
    }
}
