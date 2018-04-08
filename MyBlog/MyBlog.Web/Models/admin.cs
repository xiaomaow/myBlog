using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace MyBlog.Web.Models
{
    [Table("admin")]
    public class admin
    {
        /// <summary>
        /// 管理员Id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 登陆名称
        /// </summary>
        public string login_name { get; set; }

        /// <summary>
        /// 登陆密码
        /// </summary>
        public string pass_word { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string real_name { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string head_img { get; set; }

        public bool is_super { get; set; }
    }
}