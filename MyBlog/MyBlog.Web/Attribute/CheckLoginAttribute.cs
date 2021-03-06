﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MyBlog.Web.common;
using MyBlog.Web.Models;
using MyBlog.Web.Service;

namespace MyBlog.Web.Attribute
{
    public class CheckLoginAttribute : FilterAttribute, IAuthorizationFilter
    {
        private ManagerService _service = new ManagerService();

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            ActionResult loginPage = new RedirectResult("/manager/login");
            HttpCookie _cookie = CookieHelper.GetCookie(SysConfig.cookie_admin_name);
            if (_cookie == null || string.IsNullOrEmpty(_cookie.Value))
            {
                filterContext.Result = loginPage;
                return;
            }
            string[] v = SecurityHelper.DecryptDES(_cookie.Value, SysConfig.key).Split('|');
            if (v.Length != 2)
            {
                filterContext.Result = loginPage;
                return;
            }
            int login_id = Convert.ToInt32(v[0]);
            admin _admin = _service.admin_info(login_id);
            if (_admin == null)
            {
                filterContext.Result = loginPage;
            }
            if (_admin.pass_word != v[1])
            {
                filterContext.Result = loginPage;
                return;
            }
            filterContext.Controller.ViewBag.Admin = _admin;
        }
    }
}