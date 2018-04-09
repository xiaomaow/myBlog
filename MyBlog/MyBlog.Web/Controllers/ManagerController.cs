using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Web.Attribute;
using MyBlog.Web.Service;
using MyBlog.Web.Models;
using MyBlog.Web.common;


namespace MyBlog.Web.Controllers
{
    public class ManagerController : Controller
    {
        ManagerService _service = new ManagerService();

        // GET: Manager
        [CheckLogin]
        public ActionResult Index()
        {
            return View();
        }

        #region 登录
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
        #endregion

        #region 文章分类
        [CheckLogin]
        public ActionResult typelist()
        {
            var query = _service.GetTypeList();
            ViewBag.list = query;
            return View(ViewBag);
        }
        #endregion

        #region 管理员管理
        [CheckLogin]
        public ActionResult AdminList(int page = 0, string key = "")
        {
            PageInfo _page_info = new PageInfo(page, 20);
            List<admin> _admin_list = _service.admin_list(_page_info, key);
            ViewBag.List = _admin_list;
            return View();
        }

        [CheckLogin]
        public ActionResult Admin_Del(int id)
        {
            _service.admin_del(id);
            return Redirect("adminlist");
        }

        [CheckLogin]
        public ActionResult profile(int id = 0)
        {
            var _admin = _service.admin_info(id);
            ViewBag.EditAdmin = _admin;
            return View(ViewBag);
        }

        /// <summary>
        /// 编辑管理员信息
        /// </summary>
        /// <param name="id">管理员id</param>
        /// <param name="login_name">登录名称</param>
        /// <param name="real_name">真实姓名</param>
        /// <param name="phone">联系号码</param>
        /// <param name="pass_word">密码</param>
        /// <param name="head_img">头像</param>
        /// <param name="is_supper">是否超级管理员</param>
        /// <returns></returns>
        [CheckLogin]
        [HttpPost]
        public JsonResult Admin_Edit(int id, string login_name, string real_name, string phone, string password, string head_img, int is_super)
        {
            _service.admin_edit(id, login_name, real_name, phone, password, head_img, is_super);
            return Json("200");
        }
        #endregion
    }
}