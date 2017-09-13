using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace FirstFrame.Security
{
    public class SecurityManager
    {
        public RemoteEndpointMessageProperty endPoint;
        public OperationContext Context;
        private StressHandler _StressHandller = StressHandler.GetInstance();
        #region 接口访问压力检测
        public bool StressSafe
        {
            get
            {
                bool IsSafe = _StressHandller.GetStressSafe();
                if (!IsSafe)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("DDOS 行为：{0} {1}", this.GetHashCode().ToString("x"), DateTime.Now.ToString());
                }
                return IsSafe;
            }
        }
        #endregion

        #region 平台校验        AppPass
        public bool AppPass(string PlatformID, string AppKey, string AppSecret)
        {
            return TokenHandler.AppPass(PlatformID, AppKey, AppSecret);
        }
        #endregion
        #region 检查PlatformID        CheckPlatformID
        public bool CheckPlatformID(string PlatformID)
        {
            return TokenHandler.CheckPlatformID(PlatformID);
        }
        #endregion
        #region 校验平台AccessToken     PlatformAccessTokenPass
        public bool PlatformAccessTokenPass(string Platform, string AccessToken)
        {
            return TokenHandler.PlatformAccessTokenPass(Platform, AccessToken);
        }
        #endregion
        #region 校验用户AccessToken     UserAccessTokenPass
        public bool UserAccessTokenPass(string Platform, string UID, string UserAccessToken)
        {
            return TokenHandler.UserAccessTokenPass(Platform, UID, UserAccessToken);
        }
        #endregion
        #region 校验用户员工权限 EmployeePermissionPass
        public bool EmployeePermissionPass(string Platform, string UID, string UserAccessToken)
        {
            if (TokenHandler.UserAccessTokenPass(Platform, UID, UserAccessToken) == false) return false;

            return PermissionHandler.CheckEmployeePermission(Platform, UID, UserAccessToken);
        }
        #endregion
        #region 颁发平台PlatformToken       GrantPlatformAccessToken
        public string GrantPlatformAccessToken(string PlatformID)
        {
            return TokenHandler.GrantPlatformAccessToken(PlatformID);
        }
        #endregion
        #region 颁发用户AccessToken     GrantUserAccessToken
        public string GrantUserAccessToken(string PlatformID, string UserName)
        {
            return TokenHandler.GrantUserAccessToken(PlatformID, UserName);
        }
        #endregion
        #region 跨站检查 CheckCrossPlatform
        public bool CrossPlatformPass(string Platform, string UID)
        {
            return AppHandler.CheckCrossPlatform(Platform, UID);
        }
        #endregion
        #region 接口权限检查 HasMethodPermission
        public bool HasMethodPermission(string PlatformID, string UID)
        {
            return PermissionHandler.HasMethodPermission(PlatformID, UID, new StackFrame(1).GetMethod().Name);
        }
        #endregion
    }
}
