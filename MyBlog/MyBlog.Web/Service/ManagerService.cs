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

        public List<artice_type> GetTypeList()
        {
            var query = _context.artice_type.ToList();
            return query;
        }

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
            query.pass_word = pass_word;
            query.head_img = head_img;
            this._context.SaveChanges();
        }
        #endregion
    }
}