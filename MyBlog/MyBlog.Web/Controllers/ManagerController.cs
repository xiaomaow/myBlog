using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Web.Service;
using MyBlog.Web.Models;
using MyBlog.Web.common;

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
        public JsonResult Login(string login_name, string pass_word)
        {
            if (login_name == null)
            {
                return new JsonNetResult(new ApiResult(400, "登陆名称不能为空", null));
            }
            if (pass_word == null)
            {
                return new JsonNetResult(new ApiResult(400, "登录密码不能为空", null));
            }
            admin _admin = _service.Login(login_name, pass_word);
            if (_admin != null)
            {
                //写入cookie
                string _cookie_value = string.Format("{0}|{1}", _admin.id, _admin.pass_word);
                _cookie_value = SecurityHelper.EncryptDES(_cookie_value, SysConfig.key);
                //_cookie.Value = _cookie_value;
                //HttpContext.Response.Cookies.Add(_cookie);
                CookieHelper.SetCookie(SysConfig.cookie_admin_name, _cookie_value);
                return new JsonNetResult(new ApiResult(200, "", _admin));
            }
            return new JsonNetResult(new ApiResult(400, "登陆失败，账号或密码错误！", null));
        }

        public ActionResult typelist()
        {
            var query = _service.GetTypeList();
            ViewBag.list = query;
            return View(ViewBag);
        }
    }
}