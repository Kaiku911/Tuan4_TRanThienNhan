
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuan4_TRanThienNhan.Models;
using PagedList;
namespace Tuan4_TRanThienNhan.Controllers
{
    public class HomeController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        public ActionResult Index(int ?page)
        {
            if(page == null)
            {
                page = 1;
            }
            var all_sach = from ss in data.Saches select ss;
            int pageSize = 2;
            int pageNum = page ?? 1;
            return View(all_sach.ToPagedList(pageNum,pageSize));
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
    }
}