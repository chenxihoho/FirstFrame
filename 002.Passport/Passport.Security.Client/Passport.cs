using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FirstFrame.Security.Client
{
    /// <summary>
    /// Passport类，安全相关的算法以及Token相关的操作 
    /// </summary>
    public static class Passport
    {
        private static string key = "b7qyU+Q3Bf4CCnr3GcyTHjpAJLlhMxhrbaEBWnZyGcA=";
        private const int saltLength = 10; //定义salt值的长度 
        /// <summary>
        ///计算输入数据的SHA1散列值。
        ///注意：用户提交上来的密码，需预先调用此函数进行Hash散列，以防明文密码在网路中传输。
        /// </summary>
        /// <param name="Password">用户输入的明文密码</param>
        /// <returns>使用 Little-Endian 字节顺序的 UTF-16 格式的编码。</returns>
        public static byte[] HashPassword(string Password)
        {
            SHA1 sha1 = SHA1.Create();
            return sha1.ComputeHash(Encoding.Unicode.GetBytes(Password + key));
        }
        /// <summary>
        ///取得用户密码的加密结果，此结果可做为密文保存。
        /// </summary>
        /// <param name="Password">用户输入的明文密码</param>
        /// <returns>
        /// 经过salt的密码（经过salt的密码长度为：20+10=34，存储密码的字段为Binary(34)）
        /// 使用 Little-Endian 字节顺序的 UTF-16 格式的编码。</returns>
        public static byte[] GetCryptPassword(string Password)
        {
            return CreateDbPassword(HashPassword(Password));
        }
        /// <summary> 
        /// 对要存储的密码进行salt运算 
        /// </summary> 
        /// <param name="unsaltedPassword">没有进行过salt运算的hash散列密码</param> 
        /// <returns>经过salt的密码（经过salt的密码长度为：20+10=34，存储密码的字段为Binary(34)）</returns> 
        private static byte[] CreateDbPassword(byte[] unsaltedPassword)
        {
            //获得 salt 值 
            byte[] saltValue = new byte[saltLength];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(saltValue);

            return CreateSaltedPassword(saltValue, unsaltedPassword);
        }
        private static byte[] CreateSaltedPassword(byte[] saltValue, byte[] unsaltedPassword)
        {
            //将salt值数组添加到hash散列数组后拼接成rawSalted数组中 
            byte[] rawSalted = new byte[unsaltedPassword.Length + saltValue.Length];
            unsaltedPassword.CopyTo(rawSalted, 0);
            saltValue.CopyTo(rawSalted, unsaltedPassword.Length);

            //将合并后的rawSalted数组再进行SHA1散列的到saltedPassword数组（长度为20字节） 
            SHA1 sha1 = SHA1.Create();
            byte[] saltedPassword = sha1.ComputeHash(rawSalted);

            //将salt值数组在添加到saltedPassword数组后拼接成dbPassword数组（长度为24字节） 
            byte[] dbPassword = new byte[saltedPassword.Length + saltValue.Length];
            saltedPassword.CopyTo(dbPassword, 0);
            saltValue.CopyTo(dbPassword, saltedPassword.Length);

            return dbPassword;
        }
        public static string GetHex(byte[] array)
        {
            string strResult = string.Empty;
            //字节类型的数组转换为字符串  
            for (int i = 0; i < array.Length; i++)
            {
                //16进制转换   
                strResult = strResult + array[i].ToString("X2");
            }
            return strResult;
        }
    }
}
