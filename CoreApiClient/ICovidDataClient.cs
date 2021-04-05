using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using COVID_19.CoreApiClient.Mappers;

namespace COVID_19.CoreApiClient
{
    public interface ICovidDataClient
    {
        Task<List<CovidDataModel>> FetchCovidDataAsync(DateTime date);
    }
}
