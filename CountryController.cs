using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WoodStore.Domain.Helpers;
using WoodStore.Domain.Models;
using WoodStore.Domain.Resources;
using WoodStore.Domain.Uow;
using WoodStore.UI.Models;
using WoodStore.UI.Helpers;

namespace WoodStore.UI.Controllers
{
   [CustomAuthorize(Url = "/Country")]
    public class CountryController : BaseController
    {
        private DropDownServices services = new DropDownServices();
        private List<Country> countryList;
        private UnitOfWork uow = new UnitOfWork();
        // GET: Country
        public ActionResult Index()
        {
            List<Country> coutries = uow.CountryRepository.Get(a => a.Country_IsDeleted != true).ToList();
           
           
            //List<CountryViewModel> dest = Helper.Mapper<Country, CountryViewModel>(coutries);


            return View(coutries);
        }
        public ActionResult AddCountry()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCountry(Country ct)
        {
            if (!ModelState.IsValid)
            {
                return View(ct);
            }
            var check = uow.CountryRepository.Get(a => a.Country_Name.Equals(ct.Country_Name,StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (check != null)
            {
                ModelState.AddModelError(string.Empty, Token.errCountryAlreadyExists);
                return View(ct);
            }

            //Country dest = Helper.Mapper<CountryViewModel, Country>(ct);

            ct.Country_reg_id = 1;
            ct.Country_regdate = GetCurrentDateTime();
            uow.CountryRepository.Add(ct);
            uow.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var ct = uow.CountryRepository.FindById(id);
            if (ct == null)
            {
                return View(new Country());
            }
           
            return View(ct);
        }
        [HttpPost]
        public ActionResult Edit(Country country)
        {
            if (!ModelState.IsValid)
            {
                return View(country);
            }
          
            country.Country_reg_id = 1;
            country.Country_regdate = GetCurrentDateTime();
            uow.CountryRepository.Update(country);
            uow.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int Country_Id)
        {
            var check = uow.CountryRepository.Get(a => a.Country_Id == Country_Id).FirstOrDefault();
            if (check != null)
            {
                uow.CountryRepository.Remove(check);
                if (uow.Save())
                {
                    Helper.Notify(this, true, Token.ItemDeleted);
                    return RedirectToAction("Index");
                }
            }
            Helper.Notify(this, false, Token.ItemNotDeleted);
            
            return RedirectToAction("Index");
        }



        public ActionResult Cities()
        {
            return View(uow.CityRepository.Get(a => a.City_IsDeleted != true));
        }
        public ActionResult CreateCity()
        {
            ViewBag.Country_Id = services.GetCountries();
            return View();
        }
        [HttpPost]
        public ActionResult CreateCity(City ct)
        {
            ViewBag.Country_Id = services.GetCountries();

            if (!ModelState.IsValid)
            {
                return View(ct);
            }

            var check = uow.CityRepository.Get(a => a.City_name.Equals(ct.City_name, StringComparison.OrdinalIgnoreCase) && a.Country_Id == ct.Country_Id).FirstOrDefault();
            if (check != null)
            {
                ModelState.AddModelError(string.Empty, Token.errCountryAlreadyExists);
                return View(ct);
            }

            ct.City_RegId = 1;
            ct.City_RegDate = GetCurrentDateTime();
            uow.CityRepository.Add(ct);
            uow.Save();
            return RedirectToAction("Cities");
        }

        public ActionResult EditCity(int id)
        {

            var ct = uow.CityRepository.FindById(id);
            ViewBag.Country_Id = services.GetCountries(ct.Country_Id.ToString());

            if (ct == null)
            {
                return View(new City());
            }

            return View(ct);
        }
        [HttpPost]
        public ActionResult EditCity(City e)
        {
            ViewBag.Country_Id = services.GetCountries();

            if (!ModelState.IsValid)
            {
                return View(e);
            }


            e.City_RegId = 1;
            e.City_RegDate = GetCurrentDateTime();
            uow.CityRepository.Update(e);
            uow.Save();
            return RedirectToAction("Cities");
        }

        public ActionResult DeleteCity(int City_Id)
        {
            var check = uow.CityRepository.Get(a => a.City_Id == City_Id).FirstOrDefault();
            if (check != null)
            {
                uow.CityRepository.Remove(check);
                if (uow.Save())
                {
                    Helper.Notify(this, true, Token.ItemDeleted);
                    return RedirectToAction("Cities");
                }
            }
            Helper.Notify(this, false, Token.ItemNotDeleted);

            return RedirectToAction("Cities");
        }

        public ActionResult Districts()
        {
            return View(uow.DistrictRepository.GetAll());
        }
        public ActionResult CreateDistrict()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateDistrict(District ct, int? countryId)
        {

            if (!ModelState.IsValid)
            {
                return View(ct);
            }

            var check = uow.DistrictRepository.Get(a => a.Name.Equals(ct.Name, StringComparison.OrdinalIgnoreCase) && a.City.Country_Id == countryId).FirstOrDefault();
            if (check != null)
            {
                ModelState.AddModelError(string.Empty, Token.errUnitAlreadyExists);
                return View(ct);
            }

            uow.DistrictRepository.Add(ct);
            uow.Save();
            return RedirectToAction("Districts");
        }

        public ActionResult EditDistrict(int id)
        {

            var ct = uow.DistrictRepository.FindById(id);
            if (ct == null)
            {
                return View(new District());
            }
            ViewBag.CountryId = ct.City.Country_Id;
            return View(ct);
        }
        [HttpPost]
        public ActionResult EditDistrict(District e,int? countryId)
        {
            ViewBag.CountryId = countryId;

            if (!ModelState.IsValid)
            {
                return View(e);
            }
            var check = uow.DistrictRepository.Get(a => a.Id != e.Id && a.Name == e.Name&& a.City.Country_Id == countryId).FirstOrDefault();
            if (check != null)
            {
                ModelState.AddModelError(string.Empty, Token.errUnitAlreadyExists);
                return View(e);
            }
            uow.DistrictRepository.Update(e);
            uow.Save();
            return RedirectToAction("Districts");
        }

        public ActionResult DeleteDistrict(int id)
        {
            var check = uow.DistrictRepository.Get(a => a.Id == id).FirstOrDefault();
            if (check != null)
            {
                uow.DistrictRepository.Remove(check);
                if (uow.Save())
                {
                    Helper.Notify(this, true, Token.ItemDeleted);
                    return RedirectToAction("Districts");
                }
            }
            Helper.Notify(this, false, Token.ItemNotDeleted);

            return RedirectToAction("Districts");
        }


        protected override void Dispose(bool disposing)
        {
            services.Dispose();
            uow.Dispose();
            base.Dispose(disposing);
        }
    }
}