using System;
using System.Collections.Generic;
using System.Linq;
using FirstFrame.DapperEx;

namespace FirstFrame.Task
{
    public class BaseTask
    {
        [ID(false)]
        public string TaskID{ get; set; }// 任务编号
        public string TaskName { get; set; }// 任务名称
        public string TaskType { get; set; }// 任务类型
        public string UID { get; set; }// 任务所涉及云会员UID
        public string ReceiptID { get; set; } //涉及云结算单据编号
        public string OuterID { get; set; }// 任务所涉及外部ID
        public string DeviceID { get; set; }//  设备号
        public int ExcuteTimes { get; set; }// 任务运行次数
        public int AbortThreshold { get; set; } //任务中止阀值（最大重试次数）
        public string TaskSource { get; set; }//  任务来源
        public DateTime? StartTime { get; set; }//  任务开始时间
        public DateTime? EndTime { get; set; }//  任务结束时间
        public DateTime? CancelTime { get; set; }//  任务取消时间
        public DateTime? ProcessedTime { get; set; }//  远程成功受理时间
        public DateTime? AppointTime { get; set; }//  指定运行时间
        public DateTime CreateTime { get; set; } //任务创建时间
        public int ExcuteCycle { get; set; }// 任务执行间隔时间（秒）
        public bool Enable { get; set; }// 是否启用
        public string OuterData { get; set; }//  外部数据
        public string TaskData { get; set; }//  任务数据
        public string LastError { get; set; } //上次执行错误信息
        public string PlatformID { get; set; }//  所属平台ID
        [Ignore]
        public DbBase DbBase { get; set; } //任务所在数据库
    }

}
