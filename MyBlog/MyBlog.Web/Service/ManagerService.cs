using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MyBlog.Web.Models;
using MyBlog.Web.common;

namespace MyBlog.Web.Service
{
    public class ManagerService
    {
        public MyBlogContext _context = new MyBlogContext();

        #region 文章分类管理
        public List<artice_type> GetTypeList()
        {
            var query = _context.artice_type.ToList();
            foreach (artice_type _type in query)
            {
                _type.artice_count = this._context.artice.Where(a => a.type_id == _type.id).Count();
            }
            return query;
        }

        public artice_type GetArticeType(int id)
        {
            var query = this._context.artice_type.Where(a => a.id == id).FirstOrDefault();
            return query;
        }

        public void TypeEdit(int id, string type_name, string seo_title, string seo_key, string seo_description)
        {
            var query = this._context.artice_type.Where(a => a.id == id).FirstOrDefault();
            query.seo_description = seo_description;
            query.seo_key = seo_key;
            query.seo_title = seo_title;
            query.typename = type_name;
            this._context.SaveChanges();
        }

        /// <summary>
        /// 添加文章分类
        /// </summary>
        /// <param name="type_name">类型名称</param>
        /// <param name="seo_title">SEO标题</param>
        /// <param name="seo_key">SEO关键词</param>
        /// <param name="seo_description">SEO描述</param>
        public void TypeAdd(string type_name, string seo_title, string seo_key, string seo_description)
        {
            artice_type _type = new artice_type();
            _type.typename = type_name;
            _type.seo_title = seo_title;
            _type.seo_description = seo_description;
            _type.seo_key = seo_key;
            _type.sort = 0;
            this._context.artice_type.Add(_type);
            this._context.SaveChanges();
        }

        /// <summary>
        /// 删除文章类型
        /// </summary>
        /// <param name="id">文章类型Id</param>
        /// <returns>-1 类型中包含文章，不能删除</returns>
        public int TypeDel(int id)
        {
            var query = GetArticeType(id);
            if (query.artice_count > 0)
            {
                return -1;
            }
            this._context.artice_type.Remove(query);
            return this._context.SaveChanges();
        }
        #endregion

        #region 登录
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="login_name">登陆名称</param>
        /// <param name="pass_word">登陆密码</param>
        /// <returns></returns>
        public admin Login(string login_name, string pass_word)
        {
            var pw = SecurityHelper.ComputeMD5_16bit(pass_word);
            var query = _context.admin.Where(a => a.login_name == login_name).Where(a => a.pass_word.ToLower() == pw.ToLower()).FirstOrDefault();
            return query;
        }

        public int Update_PassWord(int user_id, string pass_word)
        {
            var pw = SecurityHelper.ComputeMD5_16bit(pass_word);
            var query = _context.admin.FirstOrDefault(a => a.id == user_id);
            query.pass_word = pw;
            int result = _context.SaveChanges();
            return result;
        }
        #endregion

        #region 管理员管理
        /// <summary>
        /// 管理员列表
        /// </summary>
        /// <param name="_page_info"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<admin> admin_list(PageInfo _page_info, string key = "")
        {
            var query = this._context.admin.ToList();
            if (string.IsNullOrEmpty(key))
            {
                query = query.Where(a => a.real_name.Contains(key) || a.phone.Contains(key)).ToList();
            }
            _page_info.count = query.Count();
            query = query.OrderByDescending(a => a.id).Skip(_page_info.page_size * _page_info.page).Take(_page_info.page_size).ToList();
            return query;
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="id"></param>
        public void admin_del(int id)
        {
            var query = this._context.admin.FirstOrDefault(a => a.id == id);
            if (query == null)
            {
                return;
            }
            this._context.admin.Remove(query);
            this._context.SaveChanges();
        }

        /// <summary>
        /// 管理员信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public admin admin_info(int id)
        {
            var query = this._context.admin.FirstOrDefault(a => a.id == id);
            return query;
        }

        public void admin_edit(int id, string login_name, string real_name, string phone, string pass_word, string head_img, int is_super)
        {
            var query = this._context.admin.FirstOrDefault(a => a.id == id);
            query.is_super = Convert.ToBoolean(is_super);
            query.login_name = login_name;
            query.real_name = real_name;
            query.phone = phone;
            if (!string.IsNullOrEmpty(pass_word))
            {
                query.pass_word = SecurityHelper.ComputeMD5_16bit(pass_word);
            }
            query.head_img = head_img;
            this._context.SaveChanges();
        }
        #endregion

        #region 文章管理
        /// <summary>
        /// 文章列表
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public List<artice> ArticeList(PageInfo _info)
        {
            var query = this._context.artice.ToList();
            _info.count = query.Count();
            query = query.Skip(_info.page * _info.page_size).Take(_info.page_size).ToList();
            foreach (artice _artice in query)
            {
                _artice.type = this._context.artice_type.FirstOrDefault();
            }
            return query;
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int ArticeDel(int id)
        {
            var query = GetArtice(id);
            this._context.artice.Remove(query);
            return this._context.SaveChanges();
        }

        public void ArticeAdd(string title, string content, string seo_title, string seo_key, string seo_description, int artice_type)
        {
            artice _artice = new artice();
            _artice.title = title;
            _artice.content = content;
            _artice.seo_title = seo_title;
            _artice.seo_key = seo_key;
            _artice.seo_description = seo_description;
            _artice.type_id = artice_type;
            _artice.create_time = DateTime.Now;
            this._context.artice.Add(_artice);
            this._context.SaveChanges();
        }

        /// <summary>
        /// 获得文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public artice GetArtice(int id)
        {
            var query = this._context.artice.Where(a => a.id == id).FirstOrDefault();
            return query;
        }

        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="seo_title"></param>
        /// <param name="seo_key"></param>
        /// <param name="seo_description"></param>
        /// <param name="artice_type"></param>
        public void ArticeEdit(int id, string title, string content, string seo_title, string seo_key, string seo_description, int artice_type)
        {
            var _artice = GetArtice(id);
            _artice.title = title;
            _artice.content = content;
            _artice.seo_title = seo_title;
            _artice.seo_key = seo_key;
            _artice.seo_description = seo_description;
            _artice.type_id = artice_type;
            this._context.SaveChanges();
        }
        #endregion
    }
}