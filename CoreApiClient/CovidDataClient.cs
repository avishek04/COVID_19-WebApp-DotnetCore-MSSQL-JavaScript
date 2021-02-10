using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using COVID_19.Models;
using CsvHelper;
using System.Net.Http;
using System.Threading.Tasks;
using COVID_19.CoreApiClient.Mappers;
using COVID_19.Data;

namespace COVID_19.CoreApiClient
{
    public class CovidDataClient
    {
        private readonly IHttpClientFactory _clientFactory;

        public AppDbContext _appDbContext;

        public CovidDataClient(IHttpClientFactory clientFactory, AppDbContext appDbContext)
        {
            _clientFactory = clientFactory;
            _appDbContext = appDbContext;
        }

        public async void UpdateLatestCovidCountryDataAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get,
            "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/01-01-2021.csv");
                request.Headers.Add("Accept", "*/*");
                //request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    using (StreamReader reader = new StreamReader(responseStream, Encoding.Default))
                    using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csv.Context.RegisterClassMap<CovidCountryDataMap>();
                        var covidCountryDataCsv = csv.GetRecords<CovidCountryData>();
                        var covidCountryDataDb = _appDbContext.CovidCountryData;
                        var covidDataToAdd = covidCountryDataCsv.Where(x => covidCountryDataDb.All(y => x.Last_Update != y.Last_Update && x.Combined_Key != y.Combined_Key)).ToList();
                        _appDbContext.CovidCountryData.AddRange(covidDataToAdd);
                        _appDbContext.SaveChanges();
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
                throw new Exception(e.Message);
            }
            catch (FieldValidationException e)
            {
                throw new Exception(e.Message);
            }
            catch (CsvHelperException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
    //reader.ReadLine();
    //string line;
    //var dateTime = new DateTime();
    //decimal num;
    //int num1;
    //while ((line = reader.ReadLine()) != null)
    //{
    //    var lineValue = line.Split(",");
    //    var covidCountryData = new CovidCountryData();
    //    covidCountryData.Admin = lineValue[1];
    //    covidCountryData.Province_State = lineValue[2];
    //    covidCountryData.Country_Region = lineValue[3];
    //    if (!String.IsNullOrEmpty(lineValue[4]) && !String.IsNullOrWhiteSpace(lineValue[4]) && DateTime.TryParse(lineValue[4], out dateTime))
    //    {
    //        covidCountryData.Last_Update = Convert.ToDateTime(lineValue[4]);
    //    }
    //    if (!String.IsNullOrEmpty(lineValue[5]) && !String.IsNullOrWhiteSpace(lineValue[5]) && Decimal.TryParse(lineValue[5], out num))
    //    {
    //        covidCountryData.Latitude = Convert.ToDecimal(lineValue[5]);
    //    }
    //    if (!String.IsNullOrEmpty(lineValue[6]) && !String.IsNullOrWhiteSpace(lineValue[6]) && Decimal.TryParse(lineValue[6], out num))
    //    {
    //        covidCountryData.Longitude = Convert.ToDecimal(lineValue[6]);
    //    }
    //    if (!String.IsNullOrEmpty(lineValue[7]) && !String.IsNullOrWhiteSpace(lineValue[7]) && int.TryParse(lineValue[7], out num1))
    //    {
    //        covidCountryData.ConfirmedCases = Convert.ToInt32(lineValue[7]);
    //    }
    //    if (!String.IsNullOrEmpty(lineValue[8]) && !String.IsNullOrWhiteSpace(lineValue[8]) && int.TryParse(lineValue[8], out num1))
    //    {
    //        covidCountryData.Deaths = Convert.ToInt32(lineValue[8]);
    //    }
    //    if (!String.IsNullOrEmpty(lineValue[9]) && !String.IsNullOrWhiteSpace(lineValue[9]) && int.TryParse(lineValue[9], out num1))
    //    {
    //        covidCountryData.Recovered = Convert.ToInt32(lineValue[9]);
    //    }
    //    if (!String.IsNullOrEmpty(lineValue[10]) && !String.IsNullOrWhiteSpace(lineValue[10]) && int.TryParse(lineValue[10], out num1))
    //    {
    //        covidCountryData.ActiveCases = Convert.ToInt32(lineValue[10]);
    //    }
    //    covidCountryData.Combined_Key = lineValue[11];
    //    covidCountryData.Incident_Rate = Convert.ToDecimal(lineValue[12]);
    //    covidCountryData.Case_Fatality_Ratio = Convert.ToDecimal(lineValue[13]);
    //    result.Add(covidCountryData);
    //FIPS,Admin2,Province_State,Country_Region,Last_Update,Lat,Long_,Confirmed,Deaths,Recovered,Active,Combined_Key,Incident_Rate,Case_Fatality_Ratio
}