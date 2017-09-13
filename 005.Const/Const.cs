/*
* 常量单元
* 张国伟
* 2014-12-17
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstFrame.Const
{
    [Flags]
    public enum ValidateType { vtPassword = 0, vtToken = 1 };
    public enum UserStatus { usWrongPassword = -3, usNoUser = -1, usNoActive = 0, usNormal = 1, usLocked = 2, usDeleted = 3, usMustReLogin = 4 };
    public enum AuditStatus { asValid = 0, asInvalid = 1, asApplying = 2, asEditing = 3 };
    public enum PushType { ptAll = 1, ptConcern = 2, ptAppoint = 3 };
    public enum PaymentType { ptOnline = 1, ptCOD = 2 }; //支付方式
    public enum gAccountType { atCash = 1, atBonus = 2, atCard = 3 }; //账户类型
    public static class ReturnCode
    {
        public const string OK = "0";
        public const string Fail = "-1";
    }
    public static class BaseConst
    {
        #if DEBUG
            public const string ServiceUrl = "http://127.0.0.1:9000/Service.svc";
            public const int RequestTimeOut = 500000;
        #endif
        #if !DEBUG
            public const string ServiceUrl = "http://www.sure001.com:9000/Service.svc";
            public const int RequestTimeOut = 15000;
        #endif

        #region 消息类
        public const int MOUSEEVENTF_MOVE = 0x0001;     // 移动鼠标 
        public const int MOUSEEVENTF_LEFTDOWN = 0x0002; //模拟鼠标左键按下 
        public const int MOUSEEVENTF_LEFTUP = 0x0004; //模拟鼠标左键抬起 
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008; //模拟鼠标右键按下 
        public const int MOUSEEVENTF_RIGHTUP = 0x0010; //模拟鼠标右键抬起 
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;// 模拟鼠标中键按下 
        public const int MOUSEEVENTF_MIDDLEUP = 0x0040;// 模拟鼠标中键抬起 
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000; //标示是否采用绝对坐标
        #endregion

        public const string AzureControlPlatformID = "0"; //总控系统PlatformID

        public const string NullString = "";
        public const string NullMethod = "0";
        public const string DefaultRootID = "0";
        public const string DefaultString = "0";
        public const string UrlEmptyParam = "null";
        public static bool HideMode = false; //隐藏开发模式

        public const string DES_KEY_1 = "4E607BAF"; //密钥
        public const string DES_KEY_2 = "EBE96E96"; //向量

        public const string key1 = "bccd4b19413d4e2bbb288a352ebfe061"; //http://www.aikuaidi.cn/api/

        private const int USER_MESSAGE = 0x0800;  //用户自定义消息
        public const int USER_LOG_MESSAGE = USER_MESSAGE + 1; //发送回显日志
        public const int USER_SCAN_OVER = USER_MESSAGE + 2; //扫描结束

        public const char TabChar = '\t';  //Excel 制表符

        public const string FORMAT_TEXT = "text";
        public const string FORMAT_JSON = "json";

        public const int ColIndex_InputDate = 0;
        public const int ColIndex_Icon = 1;
        public const int ColIndex_ReceiptId = 3;

        #if DEBUG
            public const int TrcackInterval = 1;        //两扫描最小间隔，单位分钟
        #endif
        #if !DEBUG
            public const int TrcackInterval = 90;        //两扫描最小间隔，单位分钟
        #endif

        public const string InitDate = "1900-01-01";
        public const string InitDateTime = "1900-01-01 00:00:00";
        public const string SqlInitDateTime = "1900-01-01T00:00:00";
        public const string WaitWindowOpenHint = "";
        public const int AllowUploadImageSize = 256 * 1024;  //上传图片大小限制
        public const int AllowUploadImageHeight = 900; //允许上传图片高度
        public const int AllowUploadImageWidth = 900; //允许上传图片宽度
    }

    #region 任务类
    //任务类型
    public static class TaskType
    {
        public const string TT_WCF_EXCUTE = "TT_WCF_EXCUTE"; //WCF接口调用任务
        public const string TT_DATABASE = "TT_DATABASE"; //数据库任务
    }
    #endregion
    #region 结算类
    public static class PayType
    {
        //支付类型（PT_PAY：支付，PT_DEPOSIT：充值，PT_TRANSFER：转账，PT_REFUND：退款)
        public const string PT_PAY = "PT_PAY"; //支付
        public const string PT_DEPOSIT = "PT_DEPOSIT"; //充值
        public const string PT_TRANSFER = "PT_TRANSFER"; //转账
        public const string PT_EXCHANGE = "PT_EXCHANGE"; //兑换
        public const string PT_REFUND = "PT_REFUND"; //退款
    }
    public static class PayCodeType
    {
        //支付码类型（PCT_PAYABLE：支付，PCT_RECEIVABLE：收款)
        public const string PCT_PAYABLE = "PCT_PAYABLE"; //支付
        public const string PCT_RECEIVABLE = "PCT_RECEIVABLE"; //收款
    }
    public static class AccountLevel
    {
        //账户级别（AL_USER：用户级账户，AL_PLATFOEM：平台级账户)
        public const string AL_USER = "AL_USER"; //用户级账户
        public const string AL_PLATFOEM = "AL_PLATFOEM"; //平台级账户
    }
    public static class AccountTypeStatus
    {
        //账户类型状态（ATS_VALID：启用)
        public const int ATS_VALID = 1; //启用
    }
    #endregion
    #region 平台类
    //配置类型
    public static class ConfigType
    {
        public const string CT_PUBLIC = "PublicConfig"; //公共配置
        public const string CT_PRIVATE = "PrivateConfig"; //内部配置
        public const string CT_ADVANCE = "AdvanceConfig"; //扩展配置
        public const string CT_SYSTEM = "SystemConfig"; //系统配置
        public const string CT_SECRET = "SecretConfig"; //机密配置
    }
    #endregion
    #region 业务类
    //业务类型
    public static class BusinessType
    {
        public const string BT_YH_AZURE_CONTROL = "YH_AZURE_CONTROL"; //总控

        public const string BT_YH_USER = "YH_USER"; //云用户
        public const string BT_YH_ACCOUNT = "YH_ACCOUNT"; //云结算
        public const string BT_YH_MALL = "YH_MALL"; //云商城
        public const string BT_YH_PRODUCT = "YH_PRODUCT"; //云商品
        public const string BT_YH_STORE = "YH_STORE"; //云存储
        public const string BT_MALL_QUERY = "YH_MALL_QUERY"; //云商品查询机

        public const string YH_LOG = "YH_AZURE_LOG"; //日志
    }
    #endregion
    #region 业务状态
    public static class BusinessStatus
    {
        public const string BS_AVAILABLE = "BS_AVAILABLE"; //可用
        public const string BS_SUSPENDED = "BS_SUSPENDED"; //暂停
        public const string BS_UNAVAILABLE = "BS_UNAVAILABLE"; //失效
    }
    #endregion
    #region Function 运行模式
    public static class ExecuteMode
    {
        public const string EM_CHECK = "EM_CHECK"; //检查模式
        public const string EM_EXECUTE = "EM_EXECUTE"; //执行模式
    }
    #endregion
    #region 缓存类常量
    public static class CacheTime
    {
        public static TimeSpan CT_FOREVER = new TimeSpan(24 * 365, 0, 0); //永远有效（1年）
        public static TimeSpan CT_LONG = new TimeSpan(24, 0, 0); //长期有效（1天）
        public static TimeSpan CT_SHORT = new TimeSpan(0, 10, 0); //短期有效（10分钟）
    }
    #endregion
    #region 缓存类型
    public static class CacheType
    {
        public const string CT_NULL = "CT_NULL";
        public const string CT_RUNTIME = "CT_RUNTIME";
        public const string CT_MEMCACHED = "CT_MEMCACHED";
        public const string CT_REDIS = "CT_REDIS";
    }
    #endregion
}
