using System.Collections.Generic;
using System.Linq;
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
                return _appDbContext.country;
            }
        }

        public List<Country> AllCountryData()
        {
            return AllCountries.ToList();
        }
    }
}