using FirstFrame.Const;
using FirstFrame.Helper.Log;
using FirstFrame.PacketProtocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace FirstFrame.SessionManager
{
    public class SessionManager
    {
        private static HttpSessionState _Session = HttpContext.Current.Session;

        public static string SuperToken = ConfigurationManager.AppSettings["yhAzure.SuperToken"];
        public static RestClient BaseRestClient = new RestClient(ConfigurationManager.AppSettings["yhAzure.Authenticate.Base.Service"]);
        public static RestClient UserRestClient = new RestClient(ConfigurationManager.AppSettings["yhAzure.Authenticate.User.Service"]);
        public static RestClient AccountRestClient = new RestClient(ConfigurationManager.AppSettings["yhAzure.Authenticate.Account.Service"]);
        public static JObject GetBusinessSession(string PlatformID, string _BusinessType, string _ConfigType)
        {
            bool CreateResult = CreateBusinessSesstion(PlatformID, _BusinessType, _ConfigType);
            if (CreateResult == false) return null;

            return _Session[PlatformID + _BusinessType + _ConfigType] as JObject;
        }
        private static bool CreateBusinessSesstion(string PlatformID, string _BusinessType, string _ConfigType)
        {
            bool GetConfigResult = false;
            try
            {
                if (string.IsNullOrEmpty(PlatformID)) { return false; }
                if (_Session == null) LogHelper.Error("相关类未实现 IRequiresSessionState 接口，无法调用 Session 功能！");
                if (_Session[PlatformID + _BusinessType + _ConfigType] == null) //在Session中未找到记录
                {
                    switch (_ConfigType)
                    {
                        case ConfigType.CT_PUBLIC:
                            {
                                JObject BusinessPublicConfig = GetBusinessPublicConfigFromWCF(PlatformID);
                                if (BusinessPublicConfig == null) break;

                                _Session[PlatformID + _BusinessType + ConfigType.CT_PUBLIC] = BusinessPublicConfig;
                                GetConfigResult = true;
                                break;
                            }
                        case ConfigType.CT_SECRET:
                            {
                                JObject BusinessSecretConfig = GetBusinessSecretConfigFromWCF(PlatformID);
                                if (BusinessSecretConfig == null) break;
                                if (BusinessSecretConfig["SecretConfig"] == null) break; //不是所有的Platform都上传了安全配置

                                JObject SecretConfig = JObject.Parse(BusinessSecretConfig["SecretConfig"].ToString()); //对于SecretConfig配置，无需再解析SecretConfig节点，直接解析好放入Session
                                _Session[PlatformID + _BusinessType + ConfigType.CT_SECRET] = SecretConfig;
                                GetConfigResult = true;
                                break;
                            }
                        default: break;
                    }
                }
                else
                {
                    GetConfigResult = true;
                }
                return GetConfigResult;
            }
            catch (Exception e)
            {
                LogHelper.Error("SessionManager : " + e.StackTrace.ToString());
            }
            return true;
        }
        #region 获取云结算业务配置信息 GetBusinessPublicConfigFromWCF  "PlatformID", "PlatformName", "PlatformStatus", "ExprieTime"
        public static JObject GetBusinessPublicConfigFromWCF(string PlatformID)
        {
            try
            {
                var _Request = new RestRequest("GetBusinessPublicConfig/" + PlatformID + "/" + BusinessType.BT_YH_ACCOUNT, Method.GET);
                var _Response = BaseRestClient.Execute(_Request);
                string Result = ProtocolManager.ProcessRestJson(_Response.Content);
                if (ProtocolManager.GetCode(Result) == Resource.CodeOK)
                {
                    return (JObject)JsonConvert.DeserializeObject(ProtocolManager.GetMessage(Result).ToString());
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                LogHelper.Error("Common.GetBusinessPublicConfig Error : " + e.Message + e.StackTrace.ToString());
            }
            return null;
        }
        #endregion
        #region 获取云结算业务机密配置（包含渠道方配置信息） GetBusinessSecretConfigFromWCF
        public static JObject GetBusinessSecretConfigFromWCF(string PlatformID)
        {
            try
            {
                var _Request = new RestRequest("GetBusinessSecretConfig/" + PlatformID + "/" + GetPlatformAccessTokenBySuperToken(PlatformID) + "/" + BusinessType.BT_YH_ACCOUNT, Method.GET);
                var _Response = BaseRestClient.Execute(_Request);

                string Result = ProtocolManager.ProcessRestJson(_Response.Content);

                if (ProtocolManager.GetCode(Result) == Resource.CodeOK)
                {
                    return (JObject)JsonConvert.DeserializeObject(ProtocolManager.GetMessage(Result).ToString());
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                LogHelper.Error("Common.GetBusinessSecretConfig Error : " + e.Message + e.StackTrace.ToString());
            }
            return null;
        }
        #endregion
        #region 从配置文件中获取SuperToken
        public static string GetSuperToken()
        {
            return ConfigurationManager.AppSettings["yhAzure.SuperToken"];
        }
        #endregion
        #region 通过SuperToken获取PlatformAccessToken
        public static string GetPlatformAccessTokenBySuperToken(string PlatformID)
        {
            var _Request = new RestRequest("GetPlatformAccessTokenBySuperToken/" + PlatformID + "/" + GetSuperToken(), Method.GET);
            var _Response = BaseRestClient.Execute(_Request);
            string Result = ProtocolManager.ProcessRestJson(_Response.Content);

            if (ProtocolManager.GetCode(Result) == ReturnCode.OK)
            {
                return ProtocolManager.GetMessage(Result).ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion
    }
}
