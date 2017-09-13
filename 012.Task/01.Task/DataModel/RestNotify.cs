using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstFrame.Task
{
    public enum ExecuteLocation { elNone = 0, elServer = 1, slClient = 2 }; //执行位置
    public class RestNotify  //回调通知类
    {
        public string Url = string.Empty; //接口地址
        public string Method = string.Empty;//调用方法
        public string Type = string.Empty; //调用类型
        public string Param = string.Empty;//通知参数
        public ExecuteLocation ExecuteLocation = ExecuteLocation.elNone; //调用发起位置
    }
}
