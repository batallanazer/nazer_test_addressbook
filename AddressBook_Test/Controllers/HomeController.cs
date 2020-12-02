using AddressBook_Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace AddressBook_Test.Controllers
{
    public class HomeController : Controller
    {
        static List<Address> addresses = new List<Address>();
        public ActionResult Index()
        {
            var model = addresses.OrderBy(i => i.Name);
            return View(model);
        }

        public ActionResult Search()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.Message = Guid.NewGuid().ToString();
            
            return View();
        }
        [HttpPost]
        public JsonResult PostSearch(SearchCriteria model)
        {
            if (model.CriteriaId == 0) 
            {
                return Json(new { data = addresses.Where(i=>i.Name.Contains(model.Text))});
            }
            else
            {
                return Json(new { data = addresses.Where(i => i.Number == model.Text)});
            }
        }

        [HttpPost]
        public JsonResult PostCreate(Address model) 
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Number))
            {
                return Json(new { message = "Some Data are missing" });
            }

            if (addresses.Count(i => i.Batch == model.Batch) == 50)
            {
                return Json(new { message = "Batch number cant add more than 50 addresses" });
            }

            var nameFormat = new Regex("^[A-Za-z ]+$");
            if (!nameFormat.IsMatch(model.Name))
            {
                return Json(new { message = "name is invalid"});
            }
            string[] prefix = { "0", "+" };
            var numberFormat = new Regex("^[0-9]+$");

            //check if number does not start with 0 or +
            if (!prefix.Contains(model.Number[0].ToString()))
            {
                return Json(new { message = "number is invalid" });
            }
            //check if number starts with +0
            else if (model.Number.StartsWith("+0"))
            {
                return Json(new { message = "number is invalid" });
            }
            //check if number has special character or letter
            else if (!numberFormat.IsMatch(model.Number.Remove(0,1)))
            {
                return Json(new { message = "number is invalid" });
            }
            

            addresses.Add(model);
            return Json(new { message = "address succefuly saved" });

        }
    }
}