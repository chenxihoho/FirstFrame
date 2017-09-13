using FirstFrame.DapperEx;
using System;
using System.Collections.Generic;

namespace FirstFrame.DapperTest
{
    [Database(Name = "AutoTrack")]
    [Owner(Name = "dbo")]
    [Table(Name = "Account")]
    public class Account
    {
        [ID(true)]
        public virtual string ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Password { get; set; }
        public virtual string Email { get; set; }
        public virtual DateTime CreateTime { get; set; }
        public virtual int Age { get; set; }
        [Column(true)]
        public virtual int Flag { get; set; }
        [Ignore]
        public virtual string AgeStr
        {
            get
            {
                return "年龄：" + Age;
            }
        }
    }

    [Database(Name = "yh_account_system")]
    [Owner(Name = "dbo")]
    [Table(Name = "tbReceipt")]
    public class Receipt
    {
        // tbReceipt
        [ID(false)]
        public string ReceiptID { get; set; }// 单据编号
        public string UID { get; set; }// 所属用户ID
        public bool Processed { get; set; }// 是否已处理
        public string PayType { get; set; }// 支付类型：0 支付，1 充值
        public string PayChannel { get; set; }// 支付渠道（PC_YH：益华钱包，PC_
        public string ChannelTradeID { get; set; }// 渠道商户订单号
        public int AccountTypeID { get; set; }// 账户类型
        public decimal TotalFee { get; set; }// 订单金额
        public decimal ReductionFee { get; set; }// 减免的金额
        public decimal RefundFee { get; set; }// 退款金额
        public string DeviceID { get; set; } //设备号
        public string OuterID { get; set; }// 外部ID
        public string Data { get; set; }// Data
        public DateTime? CallbackTime { get; set; }// 回调处理时间
        public DateTime? ProcessedTime { get; set; }// 单据受理时间
        public DateTime? ExprieTime { get; set; } //过期时间
        public DateTime CreateTime { get; set; }// 单据创建时间
        public string PayerUID { get; set; } //付款方UID
        public string PlatformID { get; set; }// 所属平台ID
    }
}
