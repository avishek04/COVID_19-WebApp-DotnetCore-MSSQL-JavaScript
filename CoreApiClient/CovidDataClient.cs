using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using System.Net.Http;
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

        //public List<CovidDataModel> FetchCovidDataAsync(DateTime inputDate)
        public List<CovidDataModel> FetchCovidDataAsync()
        {
            try
            {
                //var oldDate = new DateTime(2020, 3, 21);
                //var date = inputDate.ToString(@"MM-dd-yyyy");
                var client = _clientFactory.CreateClient();
                //var path = $"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/{date}.csv";
                var path = $"https://raw.githubusercontent.com/owid/covid-19-data/master/public/data/jhu/full_data.csv";
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
                                csv.Context.RegisterClassMap<CovidDataMap>();
                                covidDataCsv = csv.GetRecords<CovidDataModel>().ToList();
                                return covidDataCsv;
                            }
                        }
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

        public List<VaccineDataModel> FetchVaccineDataAsync()
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var path = $"https://raw.githubusercontent.com/owid/covid-19-data/master/public/data/vaccinations/vaccinations.csv";
                HttpWebRequest request = WebRequest.CreateHttp(path);
                request.Method = "GET"; // or "POST", "PUT", "PATCH", "DELETE", etc.
                //var request = new HttpRequestMessage(HttpMethod.Get, path);
                request.Headers.Add("Accept", "*/*");

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Do something with the response
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        List<VaccineDataModel> vaccineDataCsv;
                        // Get a reader capable of reading the response stream
                        using (StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default))
                        {
                            using (CsvReader csv = new CsvReader(myStreamReader, CultureInfo.InvariantCulture))
                            {
                                csv.Context.RegisterClassMap<VaccineDataMap>();
                                vaccineDataCsv = csv.GetRecords<VaccineDataModel>().ToList();
                                return vaccineDataCsv;
                            }
                        }
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
}