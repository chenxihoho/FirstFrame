using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstFrame.SecurityManager
{
    public interface IAccessToken
    {
        bool AppPass(string PlatformID, string AppKey, string AppSecret); //平台校验
        bool CheckPlatformID(string PlatformID); //检查PlatformID
        bool PlatformAccessTokenPass(string PlatformID, string AccessToken); //校验平台AccessToken
        bool UserAccessTokenPass(string PlatformID, string UID, string AccessToken); //校验用户AccessToken
        string GrantPlatformAccessToken(string PlatformID); //颁发平台AccessToken （内部使用）
        string GrantUserAccessToken(string PlatformID, string UID); //颁发用户AccessToken （内部使用）
        string GetPlatformAccessToken(string PlatformID, string AppKey, string AppSecret); //获取平台AccessToken
        string GetUserAccessToken(string PlatformID, string UserIdentifier, string HashPassword); //获取用户AccessToken
    }
}
