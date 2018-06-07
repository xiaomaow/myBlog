using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MyBlog.Web.Models;
using MyBlog.Web.common;
using Dapper;
using System.Text;

namespace MyBlog.Web.Service
{
    public class ManagerService
    {
        #region 文章分类管理
        public List<artice_type> GetTypeList()
        {
            using (IDbConnection connection =DBHelper.MySqlConnection())
            {
                string sql = "select * from artice_type order by id desc";
                var query = connection.Query<artice_type>(sql).ToList();

                foreach (artice_type _type in query)
                {
                    string sql_1 = "select count(1) from artice where type_id=@type_id";
                    _type.artice_count = connection.Query<int>(sql_1, new { type_id = _type.id }).FirstOrDefault();
                }
                return query;
            }

        }

        public artice_type GetArticeType(int id)
        {
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                string sql = "select * from artice_type where id=@id";
                artice_type _type = connection.Query<artice_type>(sql, new { id = id }).SingleOrDefault();
                return _type;
            }
        }

        public void TypeEdit(int id, string type_name, string seo_title, string seo_key, string seo_description)
        {
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                string sql = "update article set seo_description=@seo_description,seo_key=@seo_key,seo_title=@seo_title,type_name=@type_name where id=@id";
                var model = new
                {
                    type_name = type_name,
                    seo_title = seo_title,
                    seo_key = seo_key,
                    seo_description = seo_description,
                };
                connection.Execute(sql, model);
            }
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
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                artice_type _type = new artice_type();
                _type.type_name = type_name;
                _type.seo_title = seo_title;
                _type.seo_description = seo_description;
                _type.seo_key = seo_key;
                _type.sort = 0;
                string sql = "insert into article_type (type_name,seo_title,seo_description,seo_key,sort) values (@type_name,@seo_title,@seo_description,@seo_key,@sort)";
                connection.Execute(sql, _type);
            }
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
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                string sql = string.Format("delete from article_type where id={0}", id);
                return connection.Execute(sql);
            }

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
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                string sql = string.Format("select * from admin where login_name=@login_name and @pass_word");
                admin _admin = connection.Query<admin>(sql, new { login_name = login_name, pass_word = pw }).FirstOrDefault();
                return _admin;
            }
        }

        public int Update_PassWord(int user_id, string pass_word)
        {
            var pw = SecurityHelper.ComputeMD5_16bit(pass_word);
            int result = 0;
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                string sql = string.Format("update admin set pass_word=@pass_word where id=@id");
                result = connection.Execute(sql, new { id = user_id, pass_word = pw });
            }
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
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                //查询总数
                string sql_count = "select count(1) from admin where 1=1 ";
                if (!string.IsNullOrEmpty(key))
                {
                    sql_count += " and (real_name like '%" + key + "%' or phone like '%" + key + "%')";
                }
                int count = connection.Query<int>(sql_count).FirstOrDefault();
                _page_info.count = count;
                StringBuilder sql = new StringBuilder();
                sql.Append("select * from admin where 1=1 ");
                if (string.IsNullOrEmpty(key))
                {
                    sql.Append(" and (real_name like '%" + key + "%' or phone like '%" + key + "%')");
                }
                sql.Append("LIMIT " + _page_info.page * _page_info.page_size + "," + _page_info.page_size + " order by id desc");
                List<admin> _query = connection.Query<admin>(sql.ToString()).ToList();
                return _query;
            }
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="id"></param>
        public void admin_del(int id)
        {
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                string sql = string.Format("delete from admin where id={0}", id);
                connection.Execute(sql);
            }
        }

        /// <summary>
        /// 管理员信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public admin admin_info(int id)
        {
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                string sql = string.Format("select * from admin where id={0}", id);
                admin _admin = connection.Query<admin>(sql).FirstOrDefault();
                return _admin;
            }
        }

        public void admin_edit(int id, string login_name, string real_name, string phone, string pass_word, string head_img, int is_super)
        {
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                admin _admin = admin_info(id);
                string sql_update = "update admin set is_super=@is_super,login_name=@login_name,real_name=@real_name,phone=@phone,pass_word=@pass_word,head_img=@head_img where id=@id";
                _admin.id = id;
                _admin.login_name = login_name;
                _admin.real_name = real_name;
                _admin.is_super = Convert.ToBoolean(is_super);
                _admin.phone = phone;
                if (!string.IsNullOrEmpty(pass_word))
                {
                    _admin.pass_word = SecurityHelper.ComputeMD5_16bit(pass_word);
                }
                _admin.head_img = head_img;
                connection.Execute(sql_update, _admin);
            }
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
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                //查询总数
                string sql_count = "select count(1) from artice where 1=1 ";
                int count = connection.Query<int>(sql_count).FirstOrDefault();
                _info.count = count;

                StringBuilder sql = new StringBuilder();
                sql.Append("select * from artice where 1=1 ");
                sql.Append("order by id desc limit " + _info.page * _info.page_size + "," + _info.page_size + "");
                List<artice> _list = connection.Query<artice>(sql.ToString()).ToList();
                foreach (artice _artice in _list)
                {
                    string sql_type = "select * from artice_type where id=" + _artice.type_id;
                    artice_type _type = connection.Query<artice_type>(sql_type).FirstOrDefault();
                    _artice.type = _type;
                }
                return _list;
            }
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int ArticeDel(int id)
        {
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                string sql = "delete from artice where id=" + id;
                return connection.Execute(sql);
            }

        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="seo_title">seo标题</param>
        /// <param name="seo_key">seo关键词</param>
        /// <param name="seo_description">seo描述</param>
        /// <param name="artice_type">文章类型</param>
        public void ArticeAdd(string title, string content, string seo_title, string seo_key, string seo_description, int artice_type)
        {
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into artice (title,content,seo_title,seo_key,seo_description,type_id,create_time) ");
                sql.Append("values ");
                sql.Append("(@title,@content,@seo_title,@seo_key,@seo_description,@type_id,@create_time)");
                artice _artice = new artice();
                _artice.title = title;
                _artice.content = content;
                _artice.seo_title = seo_title;
                _artice.seo_key = seo_key;
                _artice.seo_description = seo_description;
                _artice.type_id = artice_type;
                _artice.create_time = DateTime.Now;
                connection.Execute(sql.ToString(), _artice);
            }
        }

        /// <summary>
        /// 获得文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public artice GetArtice(int id)
        {
            //var query = this._context.artice.Where(a => a.id == id).FirstOrDefault();
            //return query;
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                string sql = "select * from artice where id=" + id;
                artice _artice = connection.Query<artice>(sql).FirstOrDefault();
                return _artice;
            }
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
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                var _artice = GetArtice(id);
                _artice.title = title;
                _artice.content = content;
                _artice.seo_title = seo_title;
                _artice.seo_key = seo_key;
                _artice.seo_description = seo_description;
                _artice.type_id = artice_type;
                string sql = "update artice set title=@title,content=@content,seo_title=@seo_title,seo_key=@seo_key,seo_description=@seo_description,type_id=@type_id where id=@id";
                connection.Execute(sql, _artice);
            }
        }
        #endregion

        #region 系统设置
        /// <summary>
        /// 获取系统信息
        /// </summary>
        /// <returns></returns>
        public sys_config getSysConfig()
        {
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                string sql = "select * from sys_config";
                sys_config _config = connection.Query<sys_config>(sql).FirstOrDefault();
                return _config;
            }
        }

        /// <summary>
        /// 修改系统信息
        /// </summary>
        /// <param name="seo_title"></param>
        /// <param name="seo_description"></param>
        /// <param name="seo_key"></param>
        public void UpdateSysConfig(string seo_title, string seo_description, string seo_key)
        {
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                sys_config _config = new sys_config();
                _config.seo_description = seo_description;
                _config.seo_key = seo_key;
                _config.seo_title = seo_title;
                string sql = "update sys_config set seo_title=@seo_title,seo_description=@seo_description,seo_key=@seo_key where id=1";
                connection.Execute(sql, _config);
            }
        }
        #endregion
    }
}