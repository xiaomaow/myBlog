using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Web.Models
{
    public class PageInfo
    {
        public PageInfo()
        {
 
        }

        public PageInfo(int page, int page_size)
        {
            this.page = page;
            this.page_size = page_size;
        }
        /// <summary>
        /// 页码
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 页面显示数量
        /// </summary>
        public int page_size { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public string paramter { get; set; }
        /// <summary>
        /// 记录总数
        /// </summary>
        public int count { get; set; }

        private int _page_count;
        /// <summary>
        /// 页面总数
        /// </summary>
        public int page_count
        {
            get
            {
                if (count % page_size == 0)
                {
                    _page_count = (count / page_size) - 1;
                }
                else
                {
                    _page_count = count / page_size;
                }
                return _page_count;
            }
            set { _page_count = value; }
        }
    }
}