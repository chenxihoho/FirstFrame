﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FirstFrame.Const {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FirstFrame.Const.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 -1 的本地化字符串。
        /// </summary>
        public static string CodeFail {
            get {
                return ResourceManager.GetString("CodeFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 0 的本地化字符串。
        /// </summary>
        public static string CodeOK {
            get {
                return ResourceManager.GetString("CodeOK", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请稍候... 的本地化字符串。
        /// </summary>
        public static string ProgressPanelCaption {
            get {
                return ResourceManager.GetString("ProgressPanelCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 正在连接服务器 ... 的本地化字符串。
        /// </summary>
        public static string ProgressPanelDescription {
            get {
                return ResourceManager.GetString("ProgressPanelDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 该账号不存在 的本地化字符串。
        /// </summary>
        public static string ReturnAccountNotExists {
            get {
                return ResourceManager.GetString("ReturnAccountNotExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户认证失败 的本地化字符串。
        /// </summary>
        public static string ReturnBadToken {
            get {
                return ResourceManager.GetString("ReturnBadToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 数据库操作异常 的本地化字符串。
        /// </summary>
        public static string ReturnDatabaseException {
            get {
                return ResourceManager.GetString("ReturnDatabaseException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 调用速度超过限制 的本地化字符串。
        /// </summary>
        public static string ReturnDDOS {
            get {
                return ResourceManager.GetString("ReturnDDOS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 操作失败 的本地化字符串。
        /// </summary>
        public static string ReturnFail {
            get {
                return ResourceManager.GetString("ReturnFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 跨站调用数据被阻止 的本地化字符串。
        /// </summary>
        public static string ReturnForbiddenCrossPlatform {
            get {
                return ResourceManager.GetString("ReturnForbiddenCrossPlatform", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 没有此接口的使用权限 的本地化字符串。
        /// </summary>
        public static string ReturnInterfaceRefuse {
            get {
                return ResourceManager.GetString("ReturnInterfaceRefuse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无效的结算单据编号 的本地化字符串。
        /// </summary>
        public static string ReturnInvalidReceiptID {
            get {
                return ResourceManager.GetString("ReturnInvalidReceiptID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 此操作需要员工权限 的本地化字符串。
        /// </summary>
        public static string ReturnNeedEmployeePermission {
            get {
                return ResourceManager.GetString("ReturnNeedEmployeePermission", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 需要PlatformID参数 的本地化字符串。
        /// </summary>
        public static string ReturnNeedPlatformID {
            get {
                return ResourceManager.GetString("ReturnNeedPlatformID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 未向 GetPackage 提供数据 的本地化字符串。
        /// </summary>
        public static string ReturnNoData {
            get {
                return ResourceManager.GetString("ReturnNoData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无数据 的本地化字符串。
        /// </summary>
        public static string ReturnNoDataList {
            get {
                return ResourceManager.GetString("ReturnNoDataList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 操作已成功 的本地化字符串。
        /// </summary>
        public static string ReturnOK {
            get {
                return ResourceManager.GetString("ReturnOK", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 平台授权无效 的本地化字符串。
        /// </summary>
        public static string ReturnPlatformAuthorizeFail {
            get {
                return ResourceManager.GetString("ReturnPlatformAuthorizeFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 结算单据已经处理 的本地化字符串。
        /// </summary>
        public static string ReturnReceiptProcessed {
            get {
                return ResourceManager.GetString("ReturnReceiptProcessed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 没有此接口使用权限 的本地化字符串。
        /// </summary>
        public static string ReturnRefuse {
            get {
                return ResourceManager.GetString("ReturnRefuse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户授权无效 的本地化字符串。
        /// </summary>
        public static string ReturnUserAuthorizeFail {
            get {
                return ResourceManager.GetString("ReturnUserAuthorizeFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 此用户名已经注册 的本地化字符串。
        /// </summary>
        public static string ReturnUserExists {
            get {
                return ResourceManager.GetString("ReturnUserExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户手机号不存在 的本地化字符串。
        /// </summary>
        public static string ReturnUserMobilePhoneNotExists {
            get {
                return ResourceManager.GetString("ReturnUserMobilePhoneNotExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户不存在 的本地化字符串。
        /// </summary>
        public static string ReturnUserNotExists {
            get {
                return ResourceManager.GetString("ReturnUserNotExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 金额错误 的本地化字符串。
        /// </summary>
        public static string ReturnWrongAmount {
            get {
                return ResourceManager.GetString("ReturnWrongAmount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 密码错误 的本地化字符串。
        /// </summary>
        public static string ReturnWrongPassword {
            get {
                return ResourceManager.GetString("ReturnWrongPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 支付密码错误 的本地化字符串。
        /// </summary>
        public static string ReturnWrongPayPassword {
            get {
                return ResourceManager.GetString("ReturnWrongPayPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 短信验证码错误 的本地化字符串。
        /// </summary>
        public static string ReturnWrongSmsVerifyCode {
            get {
                return ResourceManager.GetString("ReturnWrongSmsVerifyCode", resourceCulture);
            }
        }
    }
}
