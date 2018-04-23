using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace MyBlog.Web.Models
{
    [Table("artice")]
    public class artice
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string content { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// 文章类型
        /// </summary>
        public int type_id { get; set; }

        /// <summary>
        /// seo标题
        /// </summary>
        public string seo_title { get; set; }

        /// <summary>
        /// seo关键词
        /// </summary>
        public string seo_key { get; set; }

        /// <summary>
        /// seo描述
        /// </summary>
        public string seo_description { get; set; }

        /// <summary>
        /// 文章类型
        /// </summary>
        [NotMapped]
        public artice_type type { get; set; }
    }
}