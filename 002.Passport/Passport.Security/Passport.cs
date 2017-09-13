using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FirstFrame.Security
{
    /// <summary>
    /// Passport类，安全相关的算法以及Token相关的操作 
    /// </summary>
    public static class Passport
    {
        private static string key = "b7qyU+Q3Bf4CCnr3GcyTHjpAJLlhMxhrbaEBWnZyGcA=";
        private const int saltLength = 10; //定义salt值的长度 

        /// <summary>
        ///生成一个Token。        
        /// </summary>
        /// <param name="Data">可携带的字符串数据</param>
        /// <returns>Base64字符串表示形式的随机数</returns>
        public static string GenToken(string Data)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[32];
            rng.GetBytes(buffer);
            string _Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(Data));
            string _Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(FixLength(_Data.Length.ToString(), 2))) + _Data + Convert.ToBase64String(buffer);
            //返回Base64字符串表示形式的随机数 
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(_Token));
        }
        private static string FixLength(string _String,int ToLength)
        {
            if(_String.Length < ToLength)
            {
                for (int i=0;i<ToLength-_String.Length;i++)
                {
                    _String = "0" + _String;
                }
            }
            return _String;
        }
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
        ///校验密码。
        /// </summary>
        /// <param name="storedPassword">密码密文</param>
        /// <param name="hashedPassword">密码明文经过HashPassword散列后的密文</param>
        /// <returns>true值为校验通过，false值为校验失败</returns>
        public static bool ComparePassword(byte[] storedPassword, byte[] hashedPassword)
        {
            //注意：用户提交上来的密码，已预先进行过Hash散列 
            //byte[] hashedPassword = HashPassword(Password);

            if (storedPassword == null || hashedPassword == null || hashedPassword.Length != storedPassword.Length - saltLength)
            {
                return false;
            }

            //获取数据库中的密码的salt 值，数据库中的密码的后10个字节为salt 值 
            byte[] saltValue = new byte[saltLength];
            int saltOffset = storedPassword.Length - saltLength;
            for (int i = 0; i < saltLength; i++)
            {
                saltValue[i] = storedPassword[saltOffset + i];
            }

            //用户输入的密码用户输入的密码加上salt 值，进行salt 
            byte[] saltedPassword = CreateSaltedPassword(saltValue, hashedPassword);

            //比较数据库中的密码和经过salt的用户输入密码是否相等 
            return CompareByteArray(storedPassword, saltedPassword);
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

        private static bool CompareByteArray(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }

        /*public static string GET_SHA1_PASSWORD(string strSource)
        {
            string strResult = string.Empty;
            SHA1 sha = SHA1.Create();
            //注意编码UTF8、UTF7、Unicode等的选择
            strSource += GlobalSalt; //加Salt
            byte[] bytResult = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strSource));
            //字节类型的数组转换为字符串  
            for (int i = 0; i < bytResult.Length; i++)
            {
                //16进制转换   
                strResult = strResult + bytResult[i].ToString("X");
            }
            return strResult;
        }*/
        /// <summary> 
        /// 将字节数组转换为十六进制内容的字符串
        /// </summary> 
        /// <param name="array">需要转换的字节数组</param> 
        /// <returns>转换结果</returns> 
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
        /// <summary> 
        /// 将十六进制内容的字符串转换为字节数组
        /// </summary> 
        /// <param name="hexString">需要转换的字符串</param> 
        /// <returns>转换结果</returns> 
        public static byte[] HexStringToByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="base64String">需要解码的Base64字符串</param>
        /// <returns>解码结果</returns>
        private static string Base64ToString(string base64String)
        {
            byte[] _Byte = Convert.FromBase64String(base64String);
            return Encoding.UTF8.GetString(_Byte);
        }
        /// <summary>
        /// 从Token中获取Data
        /// </summary>
        /// <param name="Token">Token</param>
        /// <returns>Data</returns>
        public static string GetData(string Token)
        {
            string _Token = Base64ToString(Token);
            int DataLength = 0;
            int.TryParse(Base64ToString(_Token.Substring(0, 4)), out DataLength);
            return Base64ToString(_Token.Substring(4, DataLength));
        }
    }
}
