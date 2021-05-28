using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using System.Net.Http;
using System.Threading.Tasks;
using COVID_19.CoreApiClient.Mappers;
using COVID_19.Data;
using System.Net;

namespace COVID_19.CoreApiClient
{
    public class CovidDataClient : ICovidDataClient
    {
        private readonly IHttpClientFactory _clientFactory;
        public AppDbContext _appDbContext;

        public CovidDataClient(IHttpClientFactory clientFactory, AppDbContext appDbContext)
        {
            _clientFactory = clientFactory;
            _appDbContext = appDbContext;
        }

        public List<CovidDataModel> FetchCovidDataAsync(DateTime inputDate)
        {
            try
            {
                var oldDate = new DateTime(2020, 3, 21);
                var date = inputDate.ToString(@"MM-dd-yyyy");
                var client = _clientFactory.CreateClient();
                var path = $"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/{date}.csv";
                HttpWebRequest request = WebRequest.CreateHttp(path);
                request.Method = "GET"; // or "POST", "PUT", "PATCH", "DELETE", etc.
                //var request = new HttpRequestMessage(HttpMethod.Get, path);
                request.Headers.Add("Accept", "*/*");

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Do something with the response
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        List<CovidDataModel> covidDataCsv;
                        // Get a reader capable of reading the response stream
                        using (StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default))
                        {
                            using (CsvReader csv = new CsvReader(myStreamReader, CultureInfo.InvariantCulture))
                            {
                                if (inputDate > oldDate)
                                {
                                    csv.Context.RegisterClassMap<CovidDataMap>();
                                    covidDataCsv = csv.GetRecords<CovidDataModel>().ToList();
                                    var covidCountryData = from data in covidDataCsv
                                                           group data by data.CountryRegionJH into covidData
                                                           select new CovidDataModel()
                                                           {
                                                               CountryRegionJH = covidData.Key,
                                                               ConfirmedCasesJH = covidData.Sum(x => x.ConfirmedCasesJH),
                                                               DeathCasesJH = covidData.Sum(x => x.DeathCasesJH),
                                                               RecoveredJH = covidData.Sum(x => x.RecoveredJH),
                                                               ActiveCasesJH = covidData.Sum(x => x.ActiveCasesJH)
                                                           };
                                    return covidCountryData.ToList();
                                }
                                else
                                {
                                    csv.Context.RegisterClassMap<CovidOldDataMap>();
                                    covidDataCsv = csv.GetRecords<CovidDataModel>().ToList();
                                    var covidCountryData = from data in covidDataCsv
                                                           group data by data.CountryRegionJH into covidData
                                                           select new CovidDataModel()
                                                           {
                                                               CountryRegionJH = covidData.Key,
                                                               ConfirmedCasesJH = covidData.Sum(x => x.ConfirmedCasesJH),
                                                               DeathCasesJH = covidData.Sum(x => x.DeathCasesJH),
                                                               RecoveredJH = covidData.Sum(x => x.RecoveredJH)
                                                           };
                                    return covidCountryData.ToList();
                                }
                            }
                        }
                    }
                }
                //var response = await client.SendAsync(request);
                //List<CovidDataModel> covidDataCsv;
                //if (response.IsSuccessStatusCode)
                //{
                //    var responseStream = await response.Content.ReadAsStreamAsync();
                //    if (inputDate > oldDate)
                //    {
                //        using (StreamReader reader = new StreamReader(responseStream, Encoding.Default))
                //        using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                //        {
                //            csv.Context.RegisterClassMap<CovidDataMap>();
                //            covidDataCsv = csv.GetRecords<CovidDataModel>().ToList();
                //            var covidCountryData = from data in covidDataCsv
                //                                   group data by data.CountryRegionJH into covidData
                //                                   select new CovidDataModel()
                //                                   {
                //                                       CountryRegionJH = covidData.Key,
                //                                       ConfirmedCasesJH = covidData.Sum(x => x.ConfirmedCasesJH),
                //                                       DeathCasesJH = covidData.Sum(x => x.DeathCasesJH),
                //                                       RecoveredJH = covidData.Sum(x => x.RecoveredJH),
                //                                       ActiveCasesJH = covidData.Sum(x => x.ActiveCasesJH)
                //                                   };
                //            return covidCountryData.ToList();
                //        }
                //    }
                //    else
                //    {
                //        using (StreamReader reader = new StreamReader(responseStream, Encoding.Default))
                //        using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                //        {
                //            csv.Context.RegisterClassMap<CovidOldDataMap>();
                //            covidDataCsv = csv.GetRecords<CovidDataModel>().ToList();
                //            var covidCountryData = from data in covidDataCsv
                //                                   group data by data.CountryRegionJH into covidData
                //                                   select new CovidDataModel()
                //                                   {
                //                                       CountryRegionJH = covidData.Key,
                //                                       ConfirmedCasesJH = covidData.Sum(x => x.ConfirmedCasesJH),
                //                                       DeathCasesJH = covidData.Sum(x => x.DeathCasesJH),
                //                                       RecoveredJH = covidData.Sum(x => x.RecoveredJH)
                //                                   };
                //            return covidCountryData.ToList();
                //        }
                //    }
                //}
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
}