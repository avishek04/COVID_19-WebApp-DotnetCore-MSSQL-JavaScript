using System;
namespace COVID_19.CoreApiClient
{
    public interface ICovidDataClient
    {
        void UpdateLatestCovidCountryDataAsync();
    }
}
