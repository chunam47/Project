using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var tin = ListTin(3);
            return View(tin);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        dbBlogDataContext data = new dbBlogDataContext();
        private List<TIN> ListTin(int count)
        {
            return data.TINs.OrderByDescending(a => a.NGAYDANG).Take(count).ToList();
        }
        public ActionResult TheoChuDe(string id)
        {
            var tin = from s in data.TINs where s.MACHUDE == id select s;
            return View(tin);
        }

        //public ActionResult Detail (string id)
        //{
        //    var tin = from s in data.TINs where s.MATIN == id select s;
        //    return (tin.Single());
        //}
    }
}