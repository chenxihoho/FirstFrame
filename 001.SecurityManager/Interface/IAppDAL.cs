using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstFrame.SecurityManager
{
    public interface IAppDAL
    {
        Object GetDbBase(); //取得数据连接对象  
        string GetAppSecret(string PlatformID, string AppKey);
        string GetPlatformID(string UID);
        bool CheckMethodPermission(string PlatformID, string UID, string Method); //检查接口调用权限
        bool CheckEmployeePermission(string PlatformID, string UID, string Method); //检查员工权限
    }
}
