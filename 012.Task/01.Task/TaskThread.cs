using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Specialized;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FirstFrame.Const;
using RestSharp;
using FirstFrame.DapperEx;
using FirstFrame.PacketProtocol;
using FirstFrame.Helper.Log;

namespace FirstFrame.Task
{
    public class TaskThread
    {
        private object Lock = new object();
        public TaskThread() { }
        public void CallTaskThread(BaseTask _Task)
        {
            Thread TaskThread = new Thread(new ParameterizedThreadStart(RunTask));
            TaskThread.Start(_Task);
        }
        private void RunTask(object _Task)
        {
            BaseTask Task = _Task as BaseTask;

            switch (Task.TaskType)
            {
                #region WCF调用任务
                case TaskType.TT_WCF_EXCUTE:
                    {
                        RestNotify _RestNotify = ProcessTaskParam(ref Task); //处理执行任务时所需要的动态参数
                        PostTask(ref Task, _RestNotify); //投递任务（无需关心任务结果，失败的任务将由框架调度重新执行直到成功）
                        break;
                    }
                #endregion
                default: break;
            }
        }
        #region 处理WCF任务参数 ProcessTaskParam
        public static RestNotify ProcessTaskParam(ref BaseTask _Task)
        {
            string PlatformAccessToken = _Super.GetPlatformAccessTokenBySuperToken(_Task.PlatformID);
            RestNotify _RestNotify = JsonConvert.DeserializeObject<RestNotify>(_Task.TaskData);

            JObject Param = new JObject(new JProperty("{$PlatformAccessToken$}", PlatformAccessToken)); //进行参数模板替换
            foreach (var Item in Param)
            {
                _Task.TaskData = _Task.TaskData.Replace(Item.Key, Item.Value.ToString());
            }
            return _RestNotify;
        }
        #endregion
        #region 投递WCF执行任务 PostTask
        public static void PostTask(ref BaseTask _Task, RestNotify _RestNotify)
        {
            try
            {
                JObject Param = JObject.Parse(_RestNotify.Param);
                if (string.IsNullOrEmpty(_RestNotify.Url)) return;

                string PlatformAccessToken = _Super.GetPlatformAccessTokenBySuperToken(_Task.PlatformID);
                JObject ParamValue = new JObject(new JProperty("PlatformAccessToken", PlatformAccessToken)); //进行参数模板替换
                foreach (var Item in ParamValue)
                {
                    Param[Item.Key] = Item.Value.ToString();
                }

                RestClient _RestClient = new RestClient(_RestNotify.Url);
                RestRequest _Request = null;

                if (_RestNotify.Type == "GET")
                {
                    _Request = new RestRequest(_RestNotify.Method + _RestNotify.Param, Method.GET);
                }

                if (_RestNotify.Type == "POST")
                {
                    Dictionary<string, object> RequestObject = new Dictionary<string, object>();
                    foreach (var Item in Param)
                    {
                        RequestObject.Add(Item.Key, Item.Value.ToString());
                    }

                    _Request = new RestRequest(_RestNotify.Method, Method.POST);
                    _Request.RequestFormat = DataFormat.Json;
                    _Request.AddBody(RequestObject);
                }

                var _Response = _RestClient.Execute(_Request);
                string _Result = _Super.ProcessRestJson(_Response.Content);

                #region 标记执行次数
                _Task.DbBase.Execute("update tbRemoteTask set ExcuteTimes = ExcuteTimes + 1 where TaskID = @TaskID and PlatformID = @PlatformID",
                                     new { TaskID = _Task.TaskID, PlatformID = _Task.PlatformID });
                #endregion

                if (ProtocolManager.GetCode(_Result) == Resource.CodeOK)
                {
                    _Task.DbBase.Execute("update tbRemoteTask set ProcessedTime = GetDate() where TaskID = @TaskID and PlatformID = @PlatformID",
                                         new { TaskID = _Task.TaskID, PlatformID = _Task.PlatformID });
                }
                else
                {
                    _Task.DbBase.Execute("update tbRemoteTask set LastError = @LastError where TaskID = @TaskID and PlatformID = @PlatformID",
                     new { LastError = ProtocolManager.GetMessage(_Result).ToString(), TaskID = _Task.TaskID, PlatformID = _Task.PlatformID });
                }
            }
            catch(Exception e)
            {
                LogHelper.Error(e.Message);
            }
        }
        #endregion
    }
}
