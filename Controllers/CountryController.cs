using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using COVID_19.Data;
using COVID_19.Data.Repository;
using Microsoft.AspNetCore.Mvc;

//namespace COVID_19.Controllers
//{
//    public class CountryController : Controller
//    {
//        /*private readonly AppDbContext _appDbContext;

//        public CountryController(AppDbContext appDbContext)
//        {
//            _appDbContext = appDbContext; 
//        }*/
//        //private readonly ICountryRepository _countryRepository;
//        //private readonly ICountryCategoryRepository _countryCategoryRepository;

//        /*public CountryController(ICountryRepository countryRepository, ICountryCategoryRepository countryCategoryRepository)
//        {
//            _countryRepository = countryRepository;
//            _countryCategoryRepository = countryCategoryRepository;
//        }*/


//        public ViewResult Index()
//        {
//            //var Countries = from c in _appDbContext.Country
//            //select c;
//            /*if(Id != null)
//            {
//                var country = _countryRepository.AllCountries.FirstOrDefault(c => c.Id == Id);
//                return View(country);
//            }*/
//            return View();
//        }
//    }
//}
