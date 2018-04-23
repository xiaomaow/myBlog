using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Web.common;

namespace MyBlog.Web.Controllers
{
    public class ApiController : Controller
    {
        // GET: Api
        public JsonResult QnToken()
        {
            string token = qiniu.Init();
            return Json(token);
        }
    }
}