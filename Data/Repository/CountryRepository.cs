using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COVID_19.Models;

namespace COVID_19.Data.Repository
{
    public class CountryRepository : ICountryRepository
    {
        public AppDbContext _appDbContext;
        public CountryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<Country> AllCountries
        {
            get
            {
                return _appDbContext.Country;
            }
        }

        public Country GetCountryById(int countryId)
        {
            return _appDbContext.Country.FirstOrDefault(c => c.Id == countryId);
        }
    }
}
