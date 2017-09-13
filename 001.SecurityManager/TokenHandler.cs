using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using FirstFrame.SecurityManager;

namespace FirstFrame.Security
{
    public sealed class TokenHandler
    {
        public static IAccessToken AccessTokenHandler;
        private static readonly TokenHandler instance = new TokenHandler();
        public TokenHandler() { }
        public TokenHandler GetInstance() { return instance; }

        #region 平台校验        AppPass
        public static bool AppPass(string PlatformID, string AppKey, string AppSecret)
        {
            return AccessTokenHandler.AppPass(PlatformID, AppKey, AppSecret);
        }
        #endregion
        #region 检查PlatformID    CheckPlatformID
        public static bool CheckPlatformID(string PlatformID)
        {
            return AccessTokenHandler.CheckPlatformID(PlatformID);
        }
        #endregion
        #region 校验用户AccessToken     UserAccessTokenPass
        public static bool UserAccessTokenPass(string PlatformID, string UserName, string AccessToken)
        {
            if (string.IsNullOrEmpty(AccessToken)) return false;
            MessageProperties properties = OperationContext.Current.IncomingMessageProperties;
            RemoteEndpointMessageProperty endPoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            if (GetUserAccessTokenPass(PlatformID, UserName, AccessToken, OperationContext.Current, endPoint)) { return true; }
            return false;
        }
        public static bool GetUserAccessTokenPass(string PlatformID, string UID, string UserAccessToken, OperationContext Context, RemoteEndpointMessageProperty endPoint)
        {
            //string _UserAccessToken = string.Empty;
            //try
            //{
            //    _UserAccessToken = CachedClient.Get(UserId + "_Token").ToString();
            //    if (_UserAccessToken == null) return false;
            //}
            //catch (Exception)
            //{
            //    _UserAccessToken = SystemImpl.GetToken(_UserType, UserId); //如果缓存服务器无法连接，则使用数据库认证
            //}

            return AccessTokenHandler.UserAccessTokenPass(PlatformID, UID, UserAccessToken);
        }
        #endregion
        #region 校验平台AccessToken     PlatformAccessTokenPass
        public static bool PlatformAccessTokenPass(string PlatformID, string AccessToken)
        {
            if (string.IsNullOrEmpty(AccessToken)) return false;

            if (GetPlatformAccessTokenPass(PlatformID, AccessToken)) { return true; }
            return false;
        }
        public static bool GetPlatformAccessTokenPass(string PlatformID, string PlatformAccessToken)
        {
            //string _PlatformAccessToken = string.Empty;
            //try
            //{
            //    _PlatformAccessToken = CachedClient.Get(UserId + "_Token").ToString();
            //    if (_PlatformAccessToken == null) return false;
            //}
            //catch (Exception)
            //{
            //    _PlatformAccessToken = SystemImpl.GetToken(_UserType, UserId); //如果缓存服务器无法连接，则使用数据库认证
            //}

            return AccessTokenHandler.PlatformAccessTokenPass(PlatformID, PlatformAccessToken);
        }
        #endregion
        #region 颁发平台PlatformToken      GrantPlatformAccessToken
        public static string GrantPlatformAccessToken(string PlatformID)
        {
            return AccessTokenHandler.GrantPlatformAccessToken(PlatformID);
        }
        #endregion
        #region 颁发用户AccessToken      GrantUserAccessToken
        public static string GrantUserAccessToken(string PlatformID, string UserName)
        {
            return AccessTokenHandler.GrantUserAccessToken(PlatformID, UserName);
        }
        #endregion
    }
}
