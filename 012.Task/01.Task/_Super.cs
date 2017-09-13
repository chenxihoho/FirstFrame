using FirstFrame.Const;
using FirstFrame.Helper.Log;
using FirstFrame.PacketProtocol;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFrame.Task
{
    public sealed class _Super
    {
        public static string SuperToken = GetSuperToken();
        public static RestClient BaseRestClient = new RestClient(ConfigurationManager.AppSettings["yhAzure.Authenticate.Base.Service"]);
        public static RestClient UserRestClient = new RestClient(ConfigurationManager.AppSettings["yhAzure.Authenticate.User.Service"]);
        public static RestClient AccountRestClient = new RestClient(ConfigurationManager.AppSettings["yhAzure.Authenticate.Account.Service"]);
        public static RestClient BonusRestClient = new RestClient(ConfigurationManager.AppSettings["yhAzure.Authenticate.Bonus.Service"]);

        //private _Super() { }
        //private static readonly _Super instance = new _Super();

        //private _Super GetInstance() { return instance; }

        #region 从配置文件中加载SuperToken
        public static string GetSuperToken()
        {
            SuperToken = ConfigurationManager.AppSettings["yhAzure.SuperToken"];
            return SuperToken;
        }
        #endregion
        #region 使用SuperToken获取PlatformAccessToken
        public static string GetPlatformAccessTokenBySuperToken(string PlatformID)
        {
            try
            {
                if (String.IsNullOrEmpty(PlatformID)) PlatformID = BaseConst.UrlEmptyParam;

                var _Request = new RestRequest("GetPlatformAccessTokenBySuperToken/" + PlatformID + "/" + GetSuperToken(), Method.GET);
                var _Response = BaseRestClient.Execute(_Request);

                string _Result = ProcessRestJson(_Response.Content);
                if (ProtocolManager.GetCode(_Result) == Resource.CodeOK)
                {
                    return ProtocolManager.GetMessage(_Result).ToString();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error("GetPlatformAccessTokenBySuperToken Error : " + e.Message + e.StackTrace.ToString());
            }
            return BaseConst.UrlEmptyParam; //用于Url参数填充时，不可返回string.Empty
        }
        #endregion
        #region 处理JSON转义 ProcessRestJson
        public static string ProcessRestJson(string JsonString)
        {
            string ResultString = JsonConvert.DeserializeObject<string>(JsonString);
            if (string.IsNullOrEmpty(ResultString)) return ResultString;

            return ProtocolManager.GetDecompressPackage(ResultString);
        }
        #endregion
    }
}
