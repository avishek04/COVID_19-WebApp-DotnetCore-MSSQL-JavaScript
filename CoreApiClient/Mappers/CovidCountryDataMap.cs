using COVID_19.Models;
using CsvHelper.Configuration;

namespace COVID_19.CoreApiClient.Mappers
{
    public class CovidCountryDataMap : ClassMap<CovidCountryData>
    {
        public CovidCountryDataMap()
        {
            //checking in again
            Map(m => m.Province_State).Name("Province_State");
            Map(m => m.Country_Region).Name("Country_Region");
            Map(m => m.Last_Update).Name("Last_Update");
            Map(m => m.Latitude).Name("Lat");
            Map(m => m.Longitude).Name("Long_");
            Map(m => m.ConfirmedCases).Name("Confirmed");
            Map(m => m.Deaths).Name("Deaths");
            Map(m => m.Recovered).Name("Recovered");
            Map(m => m.ActiveCases).Name("Active");
            Map(m => m.Combined_Key).Name("Combined_Key");
            Map(m => m.Incident_Rate).Name("Incident_Rate");
            Map(m => m.Case_Fatality_Ratio).Name("Case_Fatality_Ratio");
        }
    }
}
