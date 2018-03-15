namespace MyBlog.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class artice_type
    {
        public int id { get; set; }

        [StringLength(50)]
        public string typename { get; set; }

        public int? sort { get; set; }

        [StringLength(50)]
        public string seo_title { get; set; }

        [StringLength(200)]
        public string seo_key { get; set; }

        [StringLength(200)]
        public string seo_description { get; set; }
    }
}
