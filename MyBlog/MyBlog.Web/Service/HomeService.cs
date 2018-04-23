using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyBlog.Web.Models;
using MyBlog.Web.Service;

namespace MyBlog.Web.Service
{
    public class HomeService
    {
        public MyBlogContext _context = new MyBlogContext();

        public List<artice> GetArticeList(PageInfo _info, int typeid = 0)
        {
            var query = this._context.artice.OrderByDescending(a => a.create_time).ToList();
            if (typeid > 0)
            {
                query = query.Where(a => a.type_id == typeid).ToList();
            }
            _info.count = query.Count();
            query = query.Skip(_info.page_size * _info.page).Take(_info.page_size).ToList();
            foreach (artice _artice in query)
            {
                _artice.type = GetArticeType(_artice.type_id);
            }
            return query;
        }

        public artice_type GetArticeType(int id)
        {
            return this._context.artice_type.Where(a => a.id == id).FirstOrDefault();
        }

        public List<artice_type> GetArticeTypeList()
        {
            var query = this._context.artice_type.OrderByDescending(a => a.id).ToList();
            return query;
        }

        public artice GetArtice(int id)
        {
            var query = this._context.artice.Where(a => a.id == id).FirstOrDefault();
            if (query != null)
            {
                query.type = GetArticeType(query.type_id);
            }
            return query;
        }
    }
}