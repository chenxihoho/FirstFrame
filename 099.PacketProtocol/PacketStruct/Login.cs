using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFrame.PacketProtocol
{
    #region 登录返回包
    public class VerifyUserPackage
    {
        public string UserIdentifier { get; set; } //用于携带请求包的用户标识（支持UID、UserID、MobilePhone三种验证方式）
        public string UID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string MobilePhone { get; set; }
        public string AccessPassword { get; set; }
        public string AccessToken { get; set; }
        public string PlatformID { get; set; }
        public string PlatformName { get; set; }
    }
    #endregion
}
