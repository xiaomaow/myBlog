using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace MyBlog.Web.common
{
    public class ApiResult
    {
        public ApiResult()
            : this(200, "", null)
        { }

        public ApiResult(int code)
            : this(code, "", null)
        {
        }

        public ApiResult(string message)
            : this(200, message, null)
        { }

        public ApiResult(object data)
            : this(200, "", data)
        {

        }

        public ApiResult(int code, string message)
            : this(code, message, null)
        {

        }

        public ApiResult(int code, object data)
            : this(code, "", data)
        { }

        public ApiResult(string message, object data)
            : this(200, message, data)
        {

        }

        public ApiResult(int code = 200, string message = "", object data = null)
        {
            this.code = code;
            this.message = message;
            this.data = data;
        }

        public int code { get; set; }

        public string message { get; set; }

        public object data { get; set; }
    }
}