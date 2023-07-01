using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TABproject_MVC_C.model.DBcontext;

namespace TABproject_MVC_C.Controllers
{
    public class CountryStateCityController : Controller
    {
        Sandhya_380Entities _dbCSC = new Sandhya_380Entities();
        // GET: CountryStateCity
        public ActionResult Index()
        {
            List<Country> CountryList = _dbCSC.Country.ToList();
            ViewBag.CountryList = new SelectList(CountryList, "CountryId", "CountryName");
            ViewBag.State = new SelectList("");
            ViewBag.CityList = new SelectList("");
            return View();
        }

        public JsonResult ListAll()
        {
            var listdata = (from city in _dbCSC.City
                            join country in _dbCSC.Country on city.CountryId equals country.CountryId
                            join state in _dbCSC.States on city.StateId equals state.StateId
                            select new
                            {
                                CityId = city.Cityid,
                                StateId = city.StateId,
                                CountryId = city.CountryId,
                                CityName = city.CityName,
                                CountryName = country.CountryName,
                                StateName = state.StateName
                            }).ToList();

            return Json(listdata, JsonRequestBehavior.AllowGet);
        }



        public JsonResult stateList(int countryId)
        {
            _dbCSC.Configuration.ProxyCreationEnabled = false;
            List<States> statelist = _dbCSC.States.Where(x => x.CountryId == countryId).ToList();
            ViewBag.countryList = new SelectList(statelist, "StateId", "StateName");
            return Json(statelist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult cityList(int stateId)
        {
            _dbCSC.Configuration.ProxyCreationEnabled = false;
            List<City> cityList = _dbCSC.City.Where(x => x.StateId == stateId).ToList();
            return Json(cityList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddResult(int? id)
        {
            if(id == null)
            {
                return Json(JsonRequestBehavior.AllowGet);
            }
            else
            {
                var city = _dbCSC.City.FirstOrDefault(x => x.Cityid == id);
                return Json(city, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public JsonResult AddResult(City city, int? id) 
        { 
            if(id == null)
            {
                City cityData = new City();
                cityData.Cityid = city.Cityid;
                cityData.StateId = city.StateId;
                cityData.CountryId = city.CountryId;
                _dbCSC.City.Add(cityData);
                _dbCSC.SaveChanges();
                return Json(cityData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = _dbCSC.City.FirstOrDefault(x => x.Cityid == id);
                if(result != null)
                {
                    City cityData = new City();
                    cityData.Cityid = result.Cityid;
                    cityData.StateId = result.StateId;
                    cityData.CountryId = result.CountryId;
                    _dbCSC.SaveChanges();
                    return Json(cityData, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}