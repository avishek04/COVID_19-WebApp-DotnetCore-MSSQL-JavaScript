using COVID_19.Models;
using CsvHelper.Configuration;

namespace COVID_19.CoreApiClient.Mappers
{
    public class CovidCountryDataMap : ClassMap<CovidCountryData>
    {
        public CovidCountryDataMap()
        {
            //checking in again
            Map(m => m.Province_State).Name("Province_State").Optional();
            Map(m => m.Country_Region).Name("Country_Region").Optional();
            Map(m => m.Report_Date).Name("Last_Update").Optional();
            Map(m => m.Latitude).Name("Lat").Optional();
            Map(m => m.Longitude).Name("Long_").Optional();
            Map(m => m.ConfirmedCases).Name("Confirmed");
            Map(m => m.Deaths).Name("Deaths");
            Map(m => m.Recovered).Name("Recovered");
            Map(m => m.ActiveCases).Name("Active").Optional();
            Map(m => m.Combined_Key).Name("Combined_Key").Optional();
            Map(m => m.Incident_Rate).Name("Incident_Rate").Optional();
            //Map(m => m.Case_Fatality_Ratio).Name("Case_Fatality_Ratio");
        }
    }
}