using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.Entity;
using MyBlog.Web.Models;
using MyBlog.Web.common;
using Dapper;
using System.Text;

namespace MyBlog.Web.Service
{
    public class HomeService
    {

        public List<artice> GetArticeList(PageInfo _info, int typeid = 0)
        {
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                //查询总数
                string sql_count = "select count(1) from artice where 1=1 ";
                if (typeid > 0)
                {
                    sql_count += " and type_id=" + typeid + " ";
                }
                int count = connection.Query<int>(sql_count).FirstOrDefault();
                _info.count = count;
                //查询列表
                string sql = "select * from artice where 1=1 ";
                if (typeid > 0)
                {
                    sql += "and type_id=" + typeid + "";
                }
                sql += " order by create_time desc limit " + _info.page * _info.page_size + "," + _info.page_size;
                List<artice> _list = connection.Query<artice>(sql).ToList();
                foreach (artice _artice in _list)
                {
                    string sql_type = "select * from artice_type where id=" + _artice.type_id;
                    artice_type _type = connection.Query<artice_type>(sql_type).FirstOrDefault();
                }
                return _list;
            }
        }

        public artice_type GetArticeType(int id)
        {
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                string sql = "select * from artice_type where id=" + id;
                artice_type _type = connection.Query<artice_type>(sql).FirstOrDefault();
                return _type;
            }
        }

        public List<artice_type> GetArticeTypeList()
        {
            //var query = this._context.artice_type.OrderByDescending(a => a.id).ToList();
            //return query;
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                string sql = "select * from artice_type order by id desc";
                List<artice_type> _list = connection.Query<artice_type>(sql).ToList();
                return _list;
            }
        }

        public artice GetArtice(int id)
        {
            using (IDbConnection connection = DBHelper.MySqlConnection())
            {
                string sql = "select * from artice where id=" + id;
                artice _artice = connection.Query<artice>(sql).FirstOrDefault();
                if (_artice != null)
                {
                    string sql_type = "select * from artice_type where id=" + _artice.type_id;
                    artice_type _type = connection.Query<artice_type>(sql).FirstOrDefault();
                    _artice.type = _type;
                }
                return _artice;
            }
        }
    }
}