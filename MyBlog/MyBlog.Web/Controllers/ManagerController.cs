using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Web.Service;
using MyBlog.Web.Models;

namespace MyBlog.Web.Controllers
{
    public class ManagerController : Controller
    {
        ManagerService _service = new ManagerService();
        // GET: Manager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult typelist()
        {
            var query = _service.GetTypeList();
            ViewBag.list = query;
            return View(ViewBag);
        }
    }
}