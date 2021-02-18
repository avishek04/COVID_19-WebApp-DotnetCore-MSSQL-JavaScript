using System.Collections.Generic;
using System.Threading.Tasks;
using COVID_19.Models;

namespace COVID_19.CoreApiClient
{
    public interface ICovidDataClient
    {
        Task<List<CovidCountryData>> FetchCovidCountryDataAsync(string path);
    }
}
