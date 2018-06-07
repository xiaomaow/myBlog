using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Web.Models;
using MyBlog.Web.Service;

namespace MyBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        HomeService _service = new HomeService();
        public ActionResult Index(int typeid=0,int page=0)
        {
            PageInfo _info = new PageInfo();
            _info.page = page;
            _info.page_size = 20;
            List<artice> _artice_list = _service.GetArticeList(_info, typeid);
            ViewBag.ArticeList = _artice_list;
            List<artice_type> _artice_type_list = _service.GetArticeTypeList();
            ViewBag.ArticeTypeLit = _artice_type_list;
            sys_config _config = _service.getSysConfig();
            ViewBag.sys_config = _config;
            artice_type _artice_type = _service.GetArticeType(typeid);
            ViewBag.type = _artice_type;


            return View(ViewBag);
        }

        /// <summary>
        /// 文章详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult artice(int id)
        {
            artice _artice = _service.GetArtice(id);
            ViewBag.Item = _artice;
            List<artice_type> _artice_type_list = _service.GetArticeTypeList();
            ViewBag.ArticeTypeLit = _artice_type_list;
            return View(ViewBag);
        }
    }
}