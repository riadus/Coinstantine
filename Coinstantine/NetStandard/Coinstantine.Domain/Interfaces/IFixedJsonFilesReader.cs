using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Data;

namespace Coinstantine.Domain.Interfaces
{
    public interface IFixedJsonFilesReader
    {
        Task<IEnumerable<Translation>> GetFixedTranslations();
        Task<IEnumerable<Country>> GetCountries(string locale);
    }
}
