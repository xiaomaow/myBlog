using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MyBlog.Web.Models;

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
    }
}