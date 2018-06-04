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
        /// <summary>
        /// 文章分类列表
        /// </summary>
        /// <returns></returns>
        [CheckLogin]
        public ActionResult typelist()
        {
            var query = _service.GetTypeList();
            ViewBag.list = query;
            return View(ViewBag);
        }

        /// <summary>
        /// 添加文章分类
        /// </summary>
        /// <returns></returns>
        [CheckLogin]
        public ActionResult typeadd()
        {
            return View();
        }

        /// <summary>
        /// 添加文章分类post方法
        /// </summary>
        /// <param name="type_name">类型名称</param>
        /// <param name="seo_title">SEO标题</param>
        /// <param name="seo_key">SEO关键词</param>
        /// <param name="seo_description">SEO描述</param>
        /// <returns></returns>
        [CheckLogin]
        [HttpPost]
        public ActionResult typeadd(string type_name, string seo_title, string seo_key, string seo_description)
        {
            if (string.IsNullOrEmpty(type_name))
            {
                return new JsonNetResult(new ApiResult(400, "文章类型名称不能为空！", null));
            }
            if (string.IsNullOrEmpty(seo_title))
            {
                return new JsonNetResult(new ApiResult(400, "文章类型SEO标题不能为空!", null));
            }
            if (string.IsNullOrEmpty(seo_key))
            {
                return new JsonNetResult(new ApiResult(400, "文章类型SEO关键词不能为空!", null));
            }
            if (string.IsNullOrEmpty(seo_description))
            {
                return new JsonNetResult(new ApiResult(400, "文章类型SEO描述不能为空!", null));
            }
            _service.TypeAdd(type_name, seo_title, seo_key, seo_description);
            return new JsonNetResult(new ApiResult(200, "", null));
        }

        /// <summary>
        /// 编辑文章分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckLogin]
        public ActionResult typeedit(int id)
        {
            artice_type _type = _service.GetArticeType(id);
            ViewBag.Item = _type;
            return View(ViewBag);
        }


        [HttpPost]
        [CheckLogin]
        public ActionResult typeedit(int id, string type_name, string seo_title, string seo_key, string seo_description)
        {
            if (string.IsNullOrEmpty(type_name))
            {
                return new JsonNetResult(new ApiResult(400, "文章类型名称不能为空！", null));
            }
            if (string.IsNullOrEmpty(seo_title))
            {
                return new JsonNetResult(new ApiResult(400, "文章类型SEO标题不能为空!", null));
            }
            if (string.IsNullOrEmpty(seo_key))
            {
                return new JsonNetResult(new ApiResult(400, "文章类型SEO关键词不能为空!", null));
            }
            if (string.IsNullOrEmpty(seo_description))
            {
                return new JsonNetResult(new ApiResult(400, "文章类型SEO描述不能为空!", null));
            }
            _service.TypeEdit(id, type_name, seo_title, seo_key, seo_description);
            return new JsonNetResult(new ApiResult(200, "", null));
        }

        [HttpPost]
        [CheckLogin]
        public ActionResult typedel(int id)
        {
            int result = _service.TypeDel(id);
            if (result == -1)
            {
                return new JsonNetResult(new ApiResult(400, "分类包含文章不能删除！", null));
            }
            return new JsonNetResult(new ApiResult(200, "", null));
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

        #region 文章管理
        /// <summary>
        /// 文章管理
        /// </summary>
        /// <returns></returns>
        [CheckLogin]
        public ActionResult articelist(int page = 0)
        {
            PageInfo _info = new PageInfo();
            _info.page_size = 20;
            _info.page = page;
            List<artice> _list = _service.ArticeList(_info);
            ViewBag.List = _list;
            return View(ViewBag);
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <returns></returns>
        [CheckLogin]
        public ActionResult ArticeAdd()
        {
            List<artice_type> _type_list = _service.GetTypeList();
            ViewBag.TypeList = _type_list;
            return View(ViewBag);
        }

        [HttpPost]
        [ValidateInput(false)]
        [CheckLogin]
        public ActionResult ArticeAdd(string title, string content, string seo_title, string seo_key, string seo_description, int artice_type)
        {
            if (string.IsNullOrEmpty(title))
            {
                return new JsonNetResult(new ApiResult(400, "文章标题不能为空！", null));
            }
            if (string.IsNullOrEmpty(content))
            {
                return new JsonNetResult(new ApiResult(400, "文章内容不能为空！", null));
            }
            if (string.IsNullOrEmpty(seo_title))
            {
                return new JsonNetResult(new ApiResult(400, "文章SEO标题不能为空！", null));
            }
            if (string.IsNullOrEmpty(seo_key))
            {
                return new JsonNetResult(new ApiResult(400, "文章SEO关键词不能为空！", null));
            }
            if (string.IsNullOrEmpty(seo_description))
            {
                return new JsonNetResult(new ApiResult(400, "文章SEO描述不能为空！", null));
            }
            _service.ArticeAdd(
                title: title,
                seo_title: seo_title,
                seo_key: seo_key,
                seo_description: seo_description,
                content: content,
                artice_type: artice_type
                );
            return new JsonNetResult(new ApiResult(200, "", null));
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckLogin]
        public ActionResult articedel(int id)
        {
            _service.ArticeDel(id);
            return new JsonNetResult(new ApiResult(200, "", null));
        }

        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckLogin]
        public ActionResult articeedit(int id)
        {
            artice _artice = _service.GetArtice(id);
            ViewBag.Item = _artice;
            List<artice_type> _type_list = _service.GetTypeList();
            ViewBag.TypeList = _type_list;
            return View(ViewBag);
        }

        [HttpPost]
        [ValidateInput(false)]
        [CheckLogin]
        public ActionResult articeedit(int id, string title, string content, string seo_title, string seo_key, string seo_description, int artice_type)
        {
            if (string.IsNullOrEmpty(title))
            {
                return new JsonNetResult(new ApiResult(400, "文章标题不能为空！", null));
            }
            if (string.IsNullOrEmpty(content))
            {
                return new JsonNetResult(new ApiResult(400, "文章内容不能为空！", null));
            }
            if (string.IsNullOrEmpty(seo_title))
            {
                return new JsonNetResult(new ApiResult(400, "文章SEO标题不能为空！", null));
            }
            if (string.IsNullOrEmpty(seo_key))
            {
                return new JsonNetResult(new ApiResult(400, "文章SEO关键词不能为空！", null));
            }
            if (string.IsNullOrEmpty(seo_description))
            {
                return new JsonNetResult(new ApiResult(400, "文章SEO描述不能为空！", null));
            }
            _service.ArticeEdit(id, title, content, seo_title, seo_key, seo_description, artice_type);
            return new JsonNetResult(new ApiResult(200, "", null));
        }
        #endregion
    }
}