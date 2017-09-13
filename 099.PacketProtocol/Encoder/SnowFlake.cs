using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FirstFrame.PacketProtocol
{
    /// <summary>  
    /// 动态生产有规律的ID  
    /// </summary>  
    public class SnowFlake
    {
        private static long MachineID = 0;//机器ID  
        private static long DataCenterID = 0L;//数据ID  
        private static long Sequence = 0L;//计数从零开始  

        public static long Twepoch = 0L; //唯一时间随机量

        private static long MachineIdBits = 5L; //机器码字节数  
        private static long DataCenterIdBits = 5L;//数据中心ID长度  
        public static long MaxMachineID = -1L ^ -1L << (int)MachineIdBits; //最大支持机器节点数0~31，一共32个  
        private static long MaxDataCenterID = -1L ^ (-1L << (int)DataCenterIdBits);//最大支持数据中心节点数0~31，一共32个 

        private static long SequenceBits = 12L; //序列号12位，12个字节用来保存计数码，机器节点左移12位        
        private static long MachineIdShift = SequenceBits; //机器码数据左移位数，就是后面计数器占用的位数，数据中心节点左移17位
        private static long DataCenterIdShift = SequenceBits + MachineIdBits;
        private static long TimestampLeftShift = SequenceBits + MachineIdBits + DataCenterIdBits; //时间戳左移动位数就是机器码+计数器总字节数+数据字节数，时间毫秒数左移22位  
        public static long SequenceMask = -1L ^ -1L << (int)SequenceBits; //一微秒内可以产生计数，如果达到该值则等到下一微秒在进行生成，4095  
        private static long LastTimestamp = -1L;//最后时间戳  

        private static object SyncRoot = new object();//加锁对象  
        static SnowFlake _Snowflake;

        #region 初始化部分
        public static SnowFlake Instance()
        {
            if (_Snowflake == null) _Snowflake = new SnowFlake();
            _Snowflake.GetMachineID();
            return _Snowflake;
        }

        public SnowFlake()
        {
            Snowflakes(0L, -1);
        }

        public SnowFlake(long MachineID)
        {
            Snowflakes(MachineID, -1);
        }

        public SnowFlake(long MachineID, long DataCenterID)
        {
            Snowflakes(MachineID, DataCenterID);
        }
        #endregion
        #region 生成SerialID部分
        private void GetMachineID()
        {
            try
            {
                string HostName = Dns.GetHostName();
                HostName = "WCF-001";
                string _MachineID = HostName.Substring(HostName.IndexOf("-") + 1, HostName.Length - HostName.IndexOf("-") - 1);
                MachineID = int.Parse(_MachineID);
            }
            catch
            {
                MachineID = Process.GetCurrentProcess().Id;
            }
        }
        private void Snowflakes(long MachineID, long DataCenterID)
        {
            if (MachineID >= 0)
            {
                if (MachineID > MaxMachineID)
                {
                    throw new Exception("机器码ID非法");
                }
                SnowFlake.MachineID = MachineID;
            }
            if (DataCenterID >= 0)
            {
                if (DataCenterID > MaxDataCenterID)
                {
                    throw new Exception("数据中心ID非法");
                }
                SnowFlake.DataCenterID = DataCenterID;
            }
        }

        /// <summary>  
        /// 生成当前时间戳  
        /// </summary>  
        /// <returns>毫秒</returns>  
        private static long GetTimestamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        /// <summary>  
        /// 获取下一微秒时间戳  
        /// </summary>  
        /// <param name="lastTimestamp"></param>  
        /// <returns></returns>  
        private static long GetNextTimestamp(long LastTimestamp)
        {
            long Timestamp = GetTimestamp();
            if (Timestamp <= LastTimestamp)
            {
                Timestamp = GetTimestamp();
            }
            return Timestamp;
        }

        /// <summary>  
        /// 获取长整形的ID  
        /// </summary>  
        /// <returns></returns>  
        public long GetSerialID()
        {
            lock (SyncRoot)
            {
                long Timestamp = GetTimestamp();
                if (SnowFlake.LastTimestamp == Timestamp)
                { //同一微秒中生成ID  
                    Sequence = (Sequence + 1) & SequenceMask; //用&运算计算该微秒内产生的计数是否已经到达上限  
                    if (Sequence == 0)
                    {
                        //一微秒内产生的ID计数已达上限，等待下一微秒 
                        Timestamp = GetNextTimestamp(SnowFlake.LastTimestamp);
                    }
                }
                else
                {
                    //不同微秒生成ID  
                    Sequence = 0L;
                }
                if (Timestamp < LastTimestamp)
                {
                    throw new Exception("时间戳比上一次生成ID时时间戳还小，故异常");
                }
                SnowFlake.LastTimestamp = Timestamp; //把当前时间戳保存为最后生成ID的时间戳  
                long Id = ((Timestamp - Twepoch) << (int)TimestampLeftShift) | (DataCenterID << (int)DataCenterIdShift) | (MachineID << (int)MachineIdShift) | Sequence;
                return Id;
            }
        }
        #endregion
        #region 解读SerialID部分
        public string GetSerialString(long SerialID)
        {
            long Timestamp = SerialID >> (int)TimestampLeftShift; //取出时间戳
            return GetTime(Timestamp).ToString();
        }
        public DateTime GetTime(long timeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddMilliseconds(timeStamp).ToLocalTime();
            return dtDateTime;
        }
        #endregion
    }
}
