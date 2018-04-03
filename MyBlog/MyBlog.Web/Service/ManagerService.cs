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
    }
}