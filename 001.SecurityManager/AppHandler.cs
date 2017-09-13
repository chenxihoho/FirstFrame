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
    public sealed class AppHandler
    {
        public static IAppDAL IAppDAL;
        private static readonly AppHandler instance = new AppHandler();
        public AppHandler() { }
        public AppHandler GetInstance() { return instance; }
        public static bool AppPass(string PlatformID, string AppKey, string AppSecret)
        {
            if (string.IsNullOrEmpty(AppKey)) return false;
            MessageProperties properties = OperationContext.Current.IncomingMessageProperties;
            RemoteEndpointMessageProperty endPoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            if (CheckAppPass(PlatformID, AppKey, AppSecret, OperationContext.Current, endPoint)) { return true; }
            return false;
        }
        public static bool CheckAppPass(string PlatformID, string AppKey, string AppSecret, OperationContext Context, RemoteEndpointMessageProperty endPoint)
        {
            string _AppSecret = string.Empty;
            //try
            //{
            //    _Token = CachedClient.Get(UserId + "_Token").ToString();
            //    if (_Token == null) return false;
            //}
            //catch (Exception)
            //{
            //    _Token = SystemImpl.GetToken(_UserType, UserId); //如果缓存服务器无法连接，则使用数据库认证
            //}

            _AppSecret = IAppDAL.GetAppSecret(PlatformID, AppKey);
            return _AppSecret == AppSecret ? true : false;
        }
        public static bool CheckCrossPlatform(string PlatformID, string UID)
        {
            string _PlatformID = IAppDAL.GetPlatformID(UID);
            return _PlatformID == PlatformID ? true : false;
        }
        public static bool HasMethodPermission(string PlatformID, string UID, string Method)
        {
            return IAppDAL.CheckMethodPermission(PlatformID, UID, Method);
        }
    }
}
