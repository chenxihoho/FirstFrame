using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFrame.PushMessage.Sms.Base
{
    public class SmsVerifyCode
    {
        public string ReceiverNumber;
        public string BusinessName; //所属业务
        public string SerialNumber; //消息号
        public string VerifyCode;
        public int ExpireTime; //短信验证码有效期（单位：毫秒）
        public int ExecutedTime = 0;
        public int RequestTimeOut; //请求超时设置
    }
}
