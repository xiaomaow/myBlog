using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace MyBlog.Web.Models
{
    [Table("sys_config")]
    public class sys_config
    {
        public int id { get; set; }

        public string seo_key { get; set; }

        public string seo_title { get; set; }

        public string seo_description { get; set; }
    }
}