using FirstFrame.DapperEx;
using FirstFrame.DBHelper;
using FirstFrame.Helper.Log;
using FirstFrame.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FirstFrame.Task
{
    public class TaskSchedule
    {
        private static List<BaseTask> UnProcessedTaskList = new List<BaseTask>();
        private static DbBase DataBase = null;
        private static Timer LoadTaskTimer;
        private static int MaxThread = 1; //最大同时运行线程数
        private static readonly TaskSchedule instance = new TaskSchedule();
        public TaskSchedule() { }
        public void SetDataBase(DbBase _DataBase)
        {
            DataBase = _DataBase;
        }
        public void StartSchedule()
        {
            LoadTaskTimer = new Timer(new TimerCallback(LoadTask), null, 0, 1000 * 10); //任务装载间隔时间
        }
        public static TaskSchedule GetInstance() { return instance; }
        public void LaunchTask()
        {
            lock (instance)
            {
                for (int i = UnProcessedTaskList.Count - 1; i >= 0; i--)
                {
                    if (UnProcessedTaskList[i] == null) continue;

                    UnProcessedTaskList[i].DbBase = DataBase; //从数据库持久化中恢复，需要重新指定DbBase对象
                    new TaskThread().CallTaskThread(UnProcessedTaskList[i]); //执行任务(使用线程进行，防止订单数量很多情况下，长时间阻塞WCF工作线程事务)
                    UnProcessedTaskList.RemoveAt(i);
                }
            }
        }
        #region 立即调度执行
        public void Execute(BaseTask _BaseTask)
        {
            lock (instance)
            {
                UnProcessedTaskList.Add(_BaseTask);
                LaunchTask();
            }
        }
        #endregion
        #region 从数据库中装载任务
        private void LoadTask(object State)
        {
            if(UnProcessedTaskList.Count >= MaxThread)
            {
                LogHelper.Info("任务队列已经超过系统所设置的最大并发线程数，本次任务装载不执行。");
                return;
            }

            string SqlString = string.Format("select top {0} * from tbRemoteTask where ProcessedTime is NULL and Enable = '1' " +
                                             "and ExcuteTimes < AbortThreshold and TaskID not in ({1}) order by CreateTime desc", //使用降序查询，因为在执行时是倒序
                                             MaxThread, SQLHelper.ConvertToInSyntax<BaseTask>(UnProcessedTaskList, "TaskID")); //再次装载时排除掉已经装载过的任务
            List<BaseTask> _UnProcessedTaskList = DataBase.Query<BaseTask>(SqlString).ToList();

            lock (instance) { _UnProcessedTaskList.ForEach(i => UnProcessedTaskList.Add(i)); }
            LaunchTask();
        }
        #endregion
        #region 状态
        public int GetUnProcessedTaskCount()
        {
            return UnProcessedTaskList.Count;
        }
        #endregion
    }
}
