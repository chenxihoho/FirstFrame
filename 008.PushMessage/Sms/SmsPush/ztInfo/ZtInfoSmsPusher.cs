﻿using FirstFrame.Const;
using FirstFrame.PacketProtocol;
using FirstFrame.PushMessage.Sms;
using FirstFrame.PushMessage.Sms.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FirstFrame.PushMessage
{
    public class ZtInfoSmsPusher
    {
        //短信平台配置
        private static string SmsPlatformUserName = "yihua666";
        private static string SmsPlatformPassword = "MtOKwPLS";
        private static string SmsPlatformProductId = "676767";
        private static string SmsPlatformGateWay = "http://www.ztsms.cn:8800/sendXSms.do";

        private static string[] BusinessWhiteList = { BusinessType.BT_YH_AZURE_CONTROL, BusinessType.BT_YH_ACCOUNT, BusinessType.BT_YH_MALL,
                                                      BusinessType.BT_MALL_QUERY, BusinessType.BT_YH_PRODUCT, BusinessType.BT_YH_STORE,
                                                      BusinessType.BT_YH_USER }; //允许发送短信的业务平台

        private static object Locker = new object();
        private static Timer MonitorTimer;
        private const int VerifyCodeTimeOut = 60000; //验证码超时时间，单位毫秒
        private static Dictionary<string, SmsVerifyCode> SmsVerifyCodeList = new Dictionary<string, SmsVerifyCode>();

        public ZtInfoSmsPusher()
        {
            MonitorTimer = new Timer(new TimerCallback(TimeOutCheck), null, 0, 1000);
        }
        #region 超时检测
        private void TimeOutCheck(object state)
        {
            lock (Locker)
            {
                for (int i = SmsVerifyCodeList.Count - 1; i >= 0; i--)
                {
                    SmsVerifyCode _SmsVerifyCode = SmsVerifyCodeList.ElementAt(i).Value;
                    _SmsVerifyCode.ExecutedTime += 1000;
                    if (_SmsVerifyCode.ExecutedTime >= _SmsVerifyCode.ExpireTime)
                    {
                        SmsVerifyCodeList.Remove(SmsVerifyCodeList.ElementAt(i).Key);
                    }
                }
            }
        }
        #endregion
        #region 发送短信验证码
        public string SendSmsVerifyCode(string ReceiverNumber, string BusinessName, string Signature = BaseConst.NullString, int ExpireTime = VerifyCodeTimeOut, int VerifyCodeLength = 4)
        {
            if (!AllowSend(ReceiverNumber, BusinessName)) return ProtocolManager.GetPackage(SmsResource.CodeFail, SmsResource.ForbidSend);

            var VerifyCode = GetNumber(VerifyCodeLength);
            string SmsContent = Signature + VerifyCode;
            string SmsUrl = SmsPlatformGateWay + "?username=" + SmsPlatformUserName + "&password=" + SmsPlatformPassword + 
                            "&mobile=" + ReceiverNumber + "&content=" + SmsContent + "&dstime=&productid=" + SmsPlatformProductId + "&xh=";
            string SendResult = HttpSend(SmsUrl, Encoding.UTF8);
            if (SendResult.Substring(0, SendResult.IndexOf(',')) == "1") //短信网关发送成功
            {
                string _SerialNumber = SendResult.Substring(SendResult.IndexOf(',') + 1, SendResult.Length - SendResult.IndexOf(',') - 1);
                SmsVerifyCode _SmsVerifyCode = GetSmsVerifyCode(ReceiverNumber, BusinessName);
                if (_SmsVerifyCode != null)
                {
                    _SmsVerifyCode.VerifyCode = VerifyCode;
                    _SmsVerifyCode.ExecutedTime = 0;
                }
                else
                {
                    _SmsVerifyCode = new SmsVerifyCode()
                    {
                        BusinessName = BusinessName,
                        ReceiverNumber = ReceiverNumber,
                        SerialNumber = _SerialNumber,
                        ExpireTime = VerifyCodeTimeOut,
                        VerifyCode = VerifyCode
                    };
                    if (!SmsVerifyCodeList.ContainsKey(_SerialNumber)) { SmsVerifyCodeList.Add(_SerialNumber, _SmsVerifyCode); }
                }

                return ProtocolManager.GetPackage(Resource.CodeOK, _SerialNumber); //将消息号返回给前端
            }
            return ProtocolManager.GetPackage(Resource.CodeFail, SendResult);
        }
        #endregion
        #region 检测短信验证码
        public bool CheckVerifyCode(string ReceiverNumber, string SerialNumber, string VerifyCode)
        {
            if (SmsVerifyCodeList.ContainsKey(SerialNumber))
            {
                SmsVerifyCode _SmsVerifyCode = SmsVerifyCodeList[SerialNumber];
                if(_SmsVerifyCode.ReceiverNumber == ReceiverNumber && _SmsVerifyCode.SerialNumber == SerialNumber && _SmsVerifyCode.VerifyCode == VerifyCode)
                {
                    lock (Locker) { SmsVerifyCodeList.Remove(SerialNumber); } //验证完成之后立即清除
                    return true;
                }
            }
            return false;
        }
        #endregion
        #region 检测业务类型
        public bool AllowSend(string ReceiverNumber, string BusinessName)
        {
            bool Allow = false;
            foreach (string _BusinessName in BusinessWhiteList)
            {
                if (_BusinessName == BusinessName)
                {
                    Allow = true;
                    break;
                }
            }
            if (!Allow) return Allow; //不在允许的业务平台中，禁止发送

            for (int i = SmsVerifyCodeList.Count - 1; i >= 0; i--)
            {
                SmsVerifyCode _SmsVerifyCode = SmsVerifyCodeList.ElementAt(i).Value;
                if ((_SmsVerifyCode.ReceiverNumber == ReceiverNumber) && (_SmsVerifyCode.BusinessName == BusinessName)) { return false; }
            }
            return true;
        }
        #endregion
        #region 获取SmsVerifyCode对象
        public SmsVerifyCode GetSmsVerifyCode(string ReceiverNumber, string BusinessName)
        {
            for (int i = SmsVerifyCodeList.Count - 1; i >= 0; i--)
            {
                SmsVerifyCode _SmsVerifyCode = SmsVerifyCodeList.ElementAt(i).Value;
                if ((_SmsVerifyCode.ReceiverNumber == ReceiverNumber) && (_SmsVerifyCode.BusinessName == BusinessName)) return _SmsVerifyCode;
            }
            return null;
        }
        #endregion
        #region 生成验证码
        public string GetNumber(int Length)
        {
            Random _Random = new Random(Guid.NewGuid().GetHashCode());
            string Result = string.Empty;
            string[] s = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            for (int i = 0; i < Length; i++)
            {
                Result += s[_Random.Next(10)].ToString();
            }
            return Result.ToString();
        }
        #endregion 

        #region 发送文本短信
        public void SendSms(string ReceiverNumber, string BusinessName, string SmsContent)
        {
            string SmsUrl = SmsPlatformGateWay + "?username=" + SmsPlatformUserName + "&password=" + SmsPlatformPassword +
                            "&mobile=" + ReceiverNumber + "&content=" + SmsContent + "&dstime=&productid=" + SmsPlatformProductId + "&xh=";
            HttpSend(SmsUrl, Encoding.UTF8);
        }
        #endregion
        #region 执行网络发送
        public static string HttpSend(string Url, Encoding encoding)
        {
            HttpWebRequest Request = null;
            HttpWebResponse Response = null;
            StreamReader Reader = null;
            try
            {
                Request = (HttpWebRequest)WebRequest.Create(Url);
                Request.Timeout = 15000;
                Request.AllowAutoRedirect = false;

                Response = (HttpWebResponse)Request.GetResponse();
                if (Response.StatusCode == HttpStatusCode.OK && Response.ContentLength < 1024 * 1024)
                {
                    if (Response.ContentEncoding != null && Response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                        Reader = new StreamReader(new GZipStream(Response.GetResponseStream(), CompressionMode.Decompress), encoding);
                    else
                        Reader = new StreamReader(Response.GetResponseStream(), encoding);
                    return Reader.ReadToEnd();
                }
            }
            catch { }
            finally
            {
                if (Response != null)
                {
                    Response.Close();
                    Response = null;
                }
                if (Reader != null) Reader.Close();
                if (Response != null) Response = null;
            }
            return string.Empty;
        }
        #endregion
    }
}
