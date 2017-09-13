using System;
using System.Collections.Generic;
using System.Text;
using ybzf.Storage.Dapper.Common;


namespace ybzf.Storage.Model
{
    public enum ProduitsStatusEnum : int { Establish = 0, PendingAudit = 1, ExaminationFail = 2, RefusedAdd = 3, AuditSuccess = 4, ProhibitTheSale = 5, InTheRecycleBin = 6, Deleted = 7 };
    [Database(Name = "DB_Produits")]
    [Owner(Name = "dbo")]
    [Table(Name = "T_Produits_Items")]
    public class ProduceItemsInfo
    {
        #region ID
        [Ignore]
        public int iID { get; set; }
        #endregion
        #region 产品ID号
        [ID]
        public int iItemID { get; set; }
        #endregion
        #region 批准文号
        public string vcApproval { get; set; }
        #endregion
        #region 常用名
        public string vcEasyName { get; set; }
        #endregion
        #region 主分类
        public int iGeneralID { get; set; }
        #endregion
        #region 通用名
        public string vcGeneralName { get; set; }
        #endregion
        #region 简称
        public string vcShortName { get; set; }
        #endregion
        #region 拼音
        public string vcSpell { get; set; }
        #endregion
        #region 生产厂家ID号
        public int iFactoryID { get; set; }
        [Ignore]
        public string vcCompanyName { get; set; }
        #endregion
        #region 品牌ID号
        public int iMarkID { get; set; }
        [Ignore]
        public string vcMarkName { get; set; } //品牌名称
        #endregion
        #region 药品属性ID号
        public int iPharmaTypeID { get; set; }
        [Ignore]
        public string vcPharmaTypeName { get; set; }
        #endregion
        #region 药码类型ID号
        public int iPharmacodeID { get; set; }
        [Ignore]
        public string vcPharmacodeName { get; set; }
        [Ignore]
        public string vcEPharmacodeName { get; set; }
        #endregion
        #region 药品剂型ID号
        public int? iPharmaFomulationID { get; set; }
        [Ignore]
        public string vcPharmaFomulationName { get; set; }
        #endregion
        #region 药物识别代码（RX：处方药、OTC：非处方药、AOTC：甲类非处方、BOTC：乙类非处方）
        public string ncPharmaCode { get; set; }
        #endregion
        #region 科室类型号
        public int? iPharmaBranchID { get; set; }
        #endregion
        #region 季节ID号
        public int? iSeasonPeriodID { get; set; }
        #endregion
        #region 时间段ID号
        public int? iTimePeriodID { get; set; }
        #endregion
        #region 有效期
        public string vcValidPeriod { get; set; }
        #endregion
        #region 重要性（1：临时、2：一般、3：黑名单、4：重要）
        public int? iImportant { get; set; }
        #endregion
        #region 是否严查（0：否、1：是）
        public Boolean? iCheck { get; set; }
        #endregion
        #region 产品状态（0：创建、1：待审核、2：审核失败、3：拒绝添加、4：审核成功、5：禁止销售、6：放入回收站、7：删除）
        public int iStatus { get; set; }
        #endregion
        #region 审核人用户ID
        public string vcAuditUserID { get; set; }
        #endregion
        #region 审核拒绝理由
        public string vcAuditReason { get; set; }
        #endregion
        #region 审核时间
        public DateTime? dAuditTime { get; set; }
        #endregion
        #region 操作人
        public string vcOperUserName { get; set; }
        #endregion
        #region 操作时间
        public DateTime? dOperTime { get; set; }
        #endregion
        #region 是否删除（0：否、1：是）
        public Boolean? bIsDelete { get; set; }
        #endregion
        #region 来源平台（1：医宝后台、2：商家后台、3：用户前台）
        public int? iPlatform { get; set; }
        #endregion
        #region 更新时间
        public DateTime? dUpdateTime { get; set; }
        #endregion
        #region 创建时间
        public DateTime? dCreateTime { get; set; }
        #endregion
    }
    [Database(Name = "DB_Produits")]
    [Owner(Name = "dbo")]
    [Table(Name = "T_Produits_Items")]
    public class ProduceSearchItem
    {
        #region 产品ID
        public int iItemID { get; set; }
        #endregion
        #region 批准文号
        public string vcApproval { get; set; }
        #endregion
        #region 常用名
        public string vcEasyName { get; set; }
        #endregion
        #region 通用名
        public string vcGeneralName { get; set; }
        #endregion
        #region 药物识别代码（RX：处方药、OTC：非处方药、AOTC：甲类非处方、BOTC：乙类非处方）
        public string ncPharmaCode { get; set; }
        #endregion
    }
}
