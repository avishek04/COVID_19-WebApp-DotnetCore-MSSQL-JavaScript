using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COVID_19.Models;

namespace COVID_19.Data.Repository
{
    public interface ICountryRepository
    {
        IEnumerable<Country> AllCountries { get; }

        List<Country> AllCountryData();
    }
}