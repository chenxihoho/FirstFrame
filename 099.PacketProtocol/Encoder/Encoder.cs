using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace FirstFrame.PacketProtocol
{
    public class Encoder
    {
        #region DES 加（解）密。（对称加密）

        /// <summary>
        /// DES 加密（对称加密）。使用密钥将明文加密成密文
        /// </summary>
        /// <param name="code">明文</param>
        /// <param name="sKey">密钥</param>
        /// <returns>密文</returns>
        public static string DESEncrypt(string code, string sKey)
        {
            /* 创建一个DES加密服务提供者 */
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            /* 将要加密的内容转换成一个Byte数组 */
            byte[] inputByteArray = Encoding.Default.GetBytes(code);

            /* 设置密钥和初始化向量 */
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            /* 创建一个内存流对象 */
            MemoryStream ms = new MemoryStream();

            /* 创建一个加密流对象 */
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            /* 将要加密的文本写到加密流中 */
            cs.Write(inputByteArray, 0, inputByteArray.Length);

            /* 更新缓冲 */
            cs.FlushFinalBlock();

            /* 获取加密过的文本 */
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            /* 释放资源 */
            cs.Close();
            ms.Close();

            /* 返回结果 */
            return ret.ToString();
        }

        /// <summary>
        /// DES 解密（对称加密）。使用密钥将密文解码成明文
        /// </summary>
        /// <param name="code">密文</param>
        /// <param name="sKey">密钥</param>
        /// <returns>明文</returns>
        public static string DESDecrypt(string code, string sKey)
        {
            /* 创建一个DES加密服务提供者 */
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            /* 将要解密的内容转换成一个Byte数组 */
            byte[] inputByteArray = new byte[code.Length / 2];

            for (int x = 0; x < code.Length / 2; x++)
            {
                int i = (Convert.ToInt32(code.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            /* 设置密钥和初始化向量 */
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            /* 创建一个内存流对象 */
            MemoryStream ms = new MemoryStream();

            /* 创建一个加密流对象 */
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            /* 将要解密的文本写到加密流中 */
            cs.Write(inputByteArray, 0, inputByteArray.Length);

            /* 更新缓冲 */
            cs.FlushFinalBlock();

            /* 返回结果 */
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

        #endregion

        #region RSA 加（解）密。（不对称加密）
        /// <summary>
        /// 创建一对 RSA 密钥（公钥&私钥）。
        /// </summary>
        /// <returns></returns>
        public static RSAKey CreateRSAKey()
        {
            RSAKey rsaKey = new RSAKey();    //声明一个RSAKey对象

            /* 创建一个RSA加密服务提供者 */
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsaKey.PrivateKey = rsa.ToXmlString(true);    //创建私钥
            rsaKey.PublicKey = rsa.ToXmlString(false);    //创建公钥

            return rsaKey;    //返回结果
        }

        /// <summary>
        /// RSA 加密（不对称加密）。使用公钥将明文加密成密文
        /// </summary>
        /// <param name="code">明文</param>
        /// <param name="key">公钥</param>
        /// <returns>密文</returns>
        public static string RSAEncrypt(string code, string key)
        {
            /* 将文本转换成byte数组 */
            byte[] source = Encoding.Default.GetBytes(code);
            byte[] ciphertext;    //密文byte数组

            /* 创建一个RSA加密服务提供者 */
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);    //设置公钥
            ciphertext = rsa.Encrypt(source, false);    //加密，得到byte数组

            /* 对字符数组进行转码 */
            StringBuilder sb = new StringBuilder();
            foreach (byte b in ciphertext)
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();    //返回结果
        }

        /// <summary>
        /// RSA 解密（不对称加密）。使用私钥将密文解密成明文
        /// </summary>
        /// <param name="code">密文</param>
        /// <param name="key">私钥</param>
        /// <returns>明文</returns>
        public static string RSADecrypt(string code, string key)
        {
            /* 将文本转换成byte数组 */
            byte[] ciphertext = new byte[code.Length / 2];
            for (int x = 0; x < code.Length / 2; x++)
            {
                int i = (Convert.ToInt32(code.Substring(x * 2, 2), 16));
                ciphertext[x] = (byte)i;
            }
            byte[] source;    //原文byte数组

            /* 创建一个RSA加密服务提供者 */
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);    //设置私钥
            source = rsa.Decrypt(ciphertext, false);    //解密，得到byte数组

            return Encoding.Default.GetString(source);    //返回结果
        }

        #endregion

        #region MD5 加密（散列码 Hash 加密）
        /// <summary>
        /// MD5 加密（散列码 Hash 加密）
        /// </summary>
        /// <param name="code">明文</param>
        /// <returns>密文</returns>
        public static string MD5Encrypt(string code)
        {
            /* 获取原文内容的byte数组 */
            byte[] sourceCode = Encoding.Default.GetBytes(code);
            byte[] targetCode;    //声明用于获取目标内容的byte数组

            /* 创建一个MD5加密服务提供者 */
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            targetCode = md5.ComputeHash(sourceCode);    //执行加密

            /* 对字符数组进行转码 */
            StringBuilder sb = new StringBuilder();
            foreach (byte b in targetCode)
            {
                sb.AppendFormat("{0:X2}", b);
            }

            return sb.ToString();
        }
        #endregion
    }

    /// <summary>
    /// RSA 密钥。公钥&私钥
    /// </summary>
    public class RSAKey
    {
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
    }

}