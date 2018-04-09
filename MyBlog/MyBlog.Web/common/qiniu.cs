using Qiniu.Util;

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Qiniu.Storage;

namespace MyBlog.Web.common
{
    public class qiniu
    {
        public static string AccessKey = "";
        public static string SeretKey = "";

        public static string Init()
        {
            AccessKey = ConfigurationManager.AppSettings["qn_ak"];
            SeretKey = ConfigurationManager.AppSettings["qn_sk"];
            Mac mac = new Mac(AccessKey, SeretKey);
            //存储空间名称
            string Bucket = "xiaomaow";
            //设置上传策略
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            putPolicy.SetExpires(3600);
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            return token;
        }
    }
}