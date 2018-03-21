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

        /// <summary>
        /// 登陆页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(string login_name,string pass_word)
        {
            admin _admin = _service.Login(login_name, pass_word);
            if (_admin != null)
            {
                return Json(200);
            }
            return Json(400);
        }

        public ActionResult typelist()
        {
            var query = _service.GetTypeList();
            ViewBag.list = query;
            return View(ViewBag);
        }
    }
}