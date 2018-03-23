using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.IO;

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

        #region DES加密字符串
        public static string EncryptDES(string encryptString, string key)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(key);
                byte[] rgbIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0XEF };
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                using (MemoryStream mStream = new MemoryStream())
                {
                    CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                    cStream.Write(inputByteArray, 0, inputByteArray.Length);
                    cStream.FlushFinalBlock();
                    return Convert.ToBase64String(mStream.ToArray());
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static string EncryptDES(string encryptString, byte[] key)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            sa.Key = key;
            sa.Mode = CipherMode.ECB;
            sa.Padding = PaddingMode.Zeros;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, sa.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] byt = Encoding.Unicode.GetBytes(encryptString);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());

        }
        #endregion

        #region DES解密字符串
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="key">解密密钥，要求8位</param>
        /// <returns></returns>
        public static string DecryptDES(string decryptString, string key)
        {
            try
            {
                //默认密钥向量
                byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                byte[] rgbKey = Encoding.UTF8.GetBytes(key);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
        /// <summary>
        /// 默认向量为默认值的解密方法
        /// </summary>
        /// <param name="decryptString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DecryptDES(string strSource, byte[] key)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            sa.Key = key;
            sa.Mode = CipherMode.ECB;
            sa.Padding = PaddingMode.Zeros;
            ICryptoTransform ct = sa.CreateDecryptor();
            try
            {
                byte[] byt = Convert.FromBase64String(strSource);
                MemoryStream ms = new MemoryStream(byt);
                CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs, Encoding.Unicode);
                return sr.ReadToEnd();
            }
            catch (Exception)
            {
                return strSource;
            }
        }
        #endregion
    }
}