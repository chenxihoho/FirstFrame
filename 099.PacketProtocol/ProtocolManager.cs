using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FirstFrame.Const;
using ICSharpCode.SharpZipLib.BZip2;
using System.Web;
using System.Text.RegularExpressions;
using FirstFrame.Helper.Log;

namespace FirstFrame.PacketProtocol
{
    public static class ProtocolManager
    {
        #region 生成唯一流水号
        public static long GetSerialID()
        {
            return SnowFlake.Instance().GetSerialID();
        }
        #endregion
        #region 生成数据包
        public static string GetPackage(string ReturnCode, Object Data, string ResultFormat = BaseConst.FORMAT_JSON, string Method = BaseConst.NullMethod, bool Compressed = false, bool Encrypted = false, JsonSerializerSettings JsonSettings = null)
        {
            #region JSON格式
            if (ResultFormat == BaseConst.FORMAT_JSON)
            {
                Method = Method == BaseConst.NullString ? BaseConst.NullMethod : Method;

                string _StringMessage = string.Empty;
                if (Data is string) //字符串
                {
                    _StringMessage = Compressed ? "\"" + Compress((string)Data, Compressed) + "\"" : JsonConvert.SerializeObject(Data, Formatting.None, JsonSettings);
                }
                else if (Data is JContainer) //如果已经是JSON对象，则不能再次序列化，直接转为JSON字符
                {
                    _StringMessage = Compressed ? "\"" + Compress(Data.ToString(), Compressed) + "\"" : Data.ToString();
                }
                else //.Net对象，如果压缩则输出为字符串，不压缩则输出为JSON
                {
                    _StringMessage = Compressed ? "\"" + Compress(JsonConvert.SerializeObject(Data, Formatting.None, JsonSettings), Compressed) + "\"" : JsonConvert.SerializeObject(Data, Formatting.None, JsonSettings);
                }

                return "{\"Code\":\"" + ReturnCode + "\",\"Method\":" + Method + ",\"Compressed\":" + Compressed.ToString().ToLower() + ",\"Encrypted\":" + Encrypted.ToString().ToLower() + ",\"Message\":" + _StringMessage + "}";
            }
            #endregion
            #region TEXT格式
            if (ResultFormat == BaseConst.FORMAT_TEXT) //将数据集转换为Text字符
            {
                string _Message = JsonSettings == null ? JsonConvert.SerializeObject(Data) : JsonConvert.SerializeObject(Data, Formatting.None, JsonSettings);
                try
                {
                    JArray DataArray = JArray.Parse(_Message);

                    StringBuilder TabResultString = new StringBuilder();
                    foreach (JObject Record in DataArray)
                    {
                        IEnumerable<JProperty> properties = Record.Properties();
                        foreach (JProperty Node in properties)
                        {
                            string Value = Node.Value.ToString();
                            if ((Value.IndexOf("\r") != -1) || (Value.IndexOf("\n") != -1) || (Value.IndexOf("\t") != -1)) 
                            {
                                Value = Value.Replace("\"", "\"\"");
                                Value = "\"" + Value + "\""; 
                            }
                            TabResultString.Append(Value + "	");
                        }
                        TabResultString.Append("\r");
                    }

                    return GetPackage(Resource.CodeOK, "0000" + FixLength(DataArray.Count.ToString(), 4) + "XXXXXXXX" + TabResultString.ToString());
                }
                catch
                {
                    return GetPackage(Resource.CodeFail, "00010000" + "XXXXXXXX" + _Message);
                }
            }
            #endregion
            return Resource.ReturnNoData;
        }
        #endregion
        #region 生成数据包
        public static string GetJsonPackage(string ReturnCode, Object Data, string ResultFormat = BaseConst.FORMAT_JSON, string Method = BaseConst.NullMethod, bool Compressed = false, bool Encrypted = false, JsonSerializerSettings JsonSettings = null)
        {
            Package _Package = new Package()
            {
                Code = ReturnCode,
                Method = Method == BaseConst.NullString ? BaseConst.NullMethod : Method,
                Compressed = Compressed,
                Encrypted = Encrypted,
                Message = Data
            };
            return JsonConvert.SerializeObject(_Package, Formatting.None, JsonSettings);
       }
        #endregion
        #region 获取解压缩后的包
        public static string GetDecompressPackage(string Package)
        {
            Package _ResponsePackage = JsonConvert.DeserializeObject<Package>(Package);

            if (_ResponsePackage.Compressed)
            {
                _ResponsePackage.Message = Decompress(_ResponsePackage.Message.ToString(), _ResponsePackage.Compressed);
                _ResponsePackage.Compressed = false;
                return JsonConvert.SerializeObject(_ResponsePackage);
            }
            else
            {
                return Package;
            }
        }
        #endregion
        #region 向Package中追加数据
        public static string AppendData(string Package, string Name, object Data)
        {
            Package _Package = JsonConvert.DeserializeObject<Package>(Package);
            JObject _Data = JObject.FromObject(_Package.Message);
            _Data.Add(new JProperty(Name, Data));
            _Package.Message = _Data;
            return JsonConvert.SerializeObject(_Package);
        }
        #endregion
        #region 修改Package中的数据
        public static string ModifyData(string Package, string Name, object Data)
        {
            Package _Package = JsonConvert.DeserializeObject<Package>(Package);
            JObject _Data = JObject.FromObject(_Package.Message);
            _Data.Remove(Name);
            _Data.Add(new JProperty(Name, Data));
            _Package.Message = _Data;
            return JsonConvert.SerializeObject(_Package);
        }
        #endregion
        #region 获取Code值
        public static string GetCode(string Package)
        {
            try
            {
                if (string.IsNullOrEmpty(Package)) return string.Empty;

                Package _ResponsePackage = JsonConvert.DeserializeObject<Package>(Package);
                return _ResponsePackage.Code;
            }
            catch(Exception)
            {
                return string.Empty;
            }
        }
        #endregion
        #region 获取Method值
        public static string GetMethod(string Package)
        {
            Package _ResponsePackage = JsonConvert.DeserializeObject<Package>(Package);
            return _ResponsePackage.Method;
        }
        #endregion
        #region 获取Compressed值
        public static bool GetIsCompressed(string Package)
        {
            Package _ResponsePackage = JsonConvert.DeserializeObject<Package>(Package);
            return _ResponsePackage.Compressed;
        }
        #endregion
        #region 获取Encrypted值
        public static bool GetIsEncrypted(string Package)
        {
            Package _ResponsePackage = JsonConvert.DeserializeObject<Package>(Package);
            return _ResponsePackage.Encrypted;
        }
        #endregion
        #region 获取Message值
        public static Object GetMessage(string _Package, JsonConverter _JsonConverter = null)
        {
            if (string.IsNullOrEmpty(_Package)) return null;

            Package _ResponsePackage = _JsonConverter == null ? JsonConvert.DeserializeObject<Package>(_Package) : JsonConvert.DeserializeObject<Package>(_Package, _JsonConverter);
            return Decompress(_ResponsePackage.Message.ToString(), _ResponsePackage.Compressed);
        }
        #endregion
        #region 使用路径获取指定节点值（无需判断是否存在）
        public static string GetNodeValue(JObject _JObject, string NodeNames)
        {
            string Value = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(NodeNames)) return string.Empty;

                JObject CurrentNode = _JObject;
                string[] NameArray = Regex.Split(NodeNames, "/", RegexOptions.IgnoreCase);
                for (int i = 0; i < NameArray.Length; i++)
                {
                    if (_JObject[NameArray[i]] == null) return Value;
                    Value = CurrentNode[NameArray[i]].ToString();
                    CurrentNode = JObject.Parse(Value);

                    if (CurrentNode == null) return Value; //只要有一层不存在，就失败

                    if (i + 2 < NameArray.Length) continue; //一直解析到最后一层再取值

                    Value = CurrentNode[NameArray[i + 1]] == null ? string.Empty : CurrentNode[NameArray[i + 1]].ToString();
                }
                return Value;
            }
            catch(Exception)
            {
                return Value;
            }
        }
        #endregion
        #region 压缩与解压缩方法
        private static string Compress(string Param, bool Compressed = false)
        {
            if (!Compressed) return Param;

            byte[] Data = System.Text.Encoding.UTF8.GetBytes(Param);
            MemoryStream _MemoryStream = new MemoryStream();
            Stream _Stream = new BZip2OutputStream(_MemoryStream);
            try
            {
                _Stream.Write(Data, 0, Data.Length);
            }
            finally
            {
                _Stream.Close();
                _MemoryStream.Close();
            }
            return Convert.ToBase64String(_MemoryStream.ToArray());
        }
        private static string Decompress(string Param, bool Compressed = false)
        {
            if (!Compressed) return Param;

            string CommonString = string.Empty;
            byte[] Buffer = Convert.FromBase64String(Param);
            MemoryStream _MemoryStream = new MemoryStream(Buffer);
            Stream _Stream = new BZip2InputStream(_MemoryStream);
            StreamReader reader = new StreamReader(_Stream, System.Text.Encoding.UTF8);
            try
            {
                CommonString = reader.ReadToEnd();
            }
            finally
            {
                _Stream.Close();
                _MemoryStream.Close();
            }
            return CommonString;
        }
        #endregion
        #region 处理JSON转义 ProcessRestJson
        public static string ProcessRestJson(string JsonString)
        {
            try
            {
                string ResultString = JsonConvert.DeserializeObject<string>(JsonString);
                if (string.IsNullOrEmpty(ResultString)) return ResultString;

                return GetDecompressPackage(ResultString);
            }
            catch(Exception)
            {
                return string.Empty;
            }
        }
        #endregion
        #region 用 JSON 属性值更新 DataModel 属性值

        public static T UpdateDmProperties<T>(string jsonString, T destDM)
        {
            try
            {
                JObject joSrcDM = JObject.Parse(jsonString);
                return UpdateDmProperties<T>(joSrcDM, destDM);
            }
            catch (Exception)
            {
                return destDM;
            }
        }

        public static T UpdateDmProperties<T>(JObject joSrcDM, T destDM)
        {
            try
            {
                JObject joDestDM = JObject.FromObject(destDM);

                IEnumerable<JProperty> jpSrcDMProperties = joSrcDM.Properties();
                foreach (JProperty jpItem in jpSrcDMProperties)
                {
                    if (jpItem.Value == null) continue;
                    if (string.IsNullOrEmpty(jpItem.Value.ToString())) continue;

                    joDestDM[jpItem.Name] = jpItem.Value;
                }
                return joDestDM.ToObject<T>();
            }
            catch (Exception)
            {
                return destDM;
            }
        }
        #endregion
        #region DES加解密
        public static string DESEncrypt(string Content, string Key)
        {
            try
            {
                if (string.IsNullOrEmpty(Content)) return string.Empty;
                return Encoder.DESEncrypt(Content, Key);
            }
            catch(Exception)
            {
                return string.Empty;
            }
        }
        public static string DESDecrypt(string Content, string Key)
        {
            try
            {
                if (string.IsNullOrEmpty(Content)) return string.Empty;
                return Encoder.DESDecrypt(Content, Key);
            }
            catch(Exception)
            {
                return string.Empty;
            }
        }
        #endregion
        #region 字符串补长（在前面加0）
        public static string FixLength(string _String, int ToLength)
        {
            while (_String.Length < ToLength)
            {
                _String = "0" + _String;
            }
            return _String;
        }
        #endregion
        #region 对指定的Key进行排序
        public static void SortJson(ref JArray _JArray, string SortKey)
        {
            List<JToken> _List = new List<JToken>();
            _List = _JArray.ToList();
            IEnumerable<JToken> Query = _List.OrderBy(JToken => JToken[SortKey]);
            _JArray = JArray.Parse(JsonConvert.SerializeObject(Query.ToList()));
        }
        #endregion
        #region 正则验证工具
        public static bool IsMoney(string value)
        {
            return Regex.IsMatch(value, @"^\d{1,12}(?:\.\d{1,4})?$");
        }
        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^([0-9]{1,})$");
        }
        public static bool IsUnsign(string value)
        {
            return Regex.IsMatch(value, @"^/d*[.]?/d*$");
        }
        #endregion
        #region URL转码
        public static string UrlEncode(string Content)
        {
            return HttpUtility.UrlEncode(Content);
        }
        public static string UrlDecode(string Content)
        {
            return HttpUtility.UrlDecode(Content);
        }
        #endregion
        #region 从时间戳取得时间
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        #endregion
        #region 计算时间差
        public static TimeSpan TimeDiff(DateTime BeginTime, DateTime EndTime)
        {
            TimeSpan tsBeginTime = new TimeSpan(BeginTime.Ticks);
            TimeSpan tsEndTime = new TimeSpan(EndTime.Ticks);
            return tsBeginTime.Subtract(tsEndTime);
        }
        #endregion
    }
}
