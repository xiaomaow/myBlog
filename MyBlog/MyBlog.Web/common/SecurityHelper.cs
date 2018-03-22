using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

namespace MyBlog.Web.common
{
    public class SecurityHelper
    {
        #region 加密字符串MD5
        /// <summary>
        /// 利用MD5加密算法 加密字符串
        /// </summary>
        /// <param name="src"></param>
        /// <param name="isLower"></param>
        /// <returns></returns>
        public static string ComputeMD5(string src, bool isLower = false)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(src);
            //计算MD5密码
            byteArray = (new MD5CryptoServiceProvider().ComputeHash(byteArray));
            string md5str = BitConverter.ToString(byteArray).Replace("-", "");
            return isLower ? md5str.ToLower() : md5str.ToUpper();
        }

        /// <summary>
        /// 利用MD5几秒算法 生成16位字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ComputeMD5_16bit(string str)
        {
            str = ComputeMD5(str, false);
            return str.Substring(8, 16);
        }
        #endregion
    }
}