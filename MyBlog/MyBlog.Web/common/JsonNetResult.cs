using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;

namespace MyBlog.Web.common
{
    public class JsonNetResult : JsonResult
    {
        public JsonSerializerSettings Settings { get; private set; }

        public JsonNetResult()
        {
            Settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        public JsonNetResult(object data, JsonRequestBehavior behvior = JsonRequestBehavior.AllowGet, string contentType = null, Encoding contentEncoding = null)
        {
            Settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            this.Data = data;
            this.JsonRequestBehavior = behvior;
            this.ContentEncoding = contentEncoding;
            this.ContentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("JSON GET is not allowed");
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;
            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;
            if (this.Data == null)
                return;

            var scriptSerializer = JsonSerializer.Create(this.Settings);
            scriptSerializer.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            using (var sw = new StringWriter())
            {
                scriptSerializer.Serialize(sw, this.Data);
                response.Write(sw.ToString());
            }
        }
    }
}