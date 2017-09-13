using FirstFrame.DapperEx;
using FirstFrame.PacketProtocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DapperTest
{
    [Database(Name = "yh_mall")]
    [Owner(Name = "dbo")]
    [Table(Name = "tbGoods")]
    public class Goods
    {
        // tbGoods
        [ID(false)]
        public string GoodsID { get; set; }// 商品ID
        public string yhProductId { get; set; }// 益华产品全局唯一码
        public string ProductCategoryId { get; set; }// 商品所属类目ID
        public string SellerCategoryId { get; set; }// 商家自定义类目
        public string PlatformName { get; set; }// 卖家名称
        public string GoodsTitle { get; set; }// 商品标题
        public string GoodsDescript { get; set; }// 商品描述
        public string GoodsProperty { get; set; }// 商品属性
        public string GoodsImages { get; set; }// 商品图片
        public int GoodsNumber { get; set; }// 商品数量
        public int SalesVolume { get; set; }// 商品销量
        public string Supplier { get; set; } //商品供应商
        public decimal OldPrice { get; set; }// 商品旧价
        public decimal GoodPrice { get; set; }// 商品定价
        public decimal SalePrice { get; set; }// 商品销价
        public decimal WholesalePrice { get; set; } //商品批发价
        public string GoodsOuterId { get; set; }// 商家外部编码
        public DateTime ListTime { get; set; }// 上架时间
        public DateTime? DelistTime { get; set; }// 下架时间
        public int StuffStatus { get; set; }// 新旧程度（1-100，例如99成新，
        public string Province { get; set; }// 所在省份
        public string City { get; set; }// 所在城市
        public string PostageTempletID { get; set; }// 邮费模板ID
        //[Ignore]
        //public GoodsShelfNumber GoodsLocation
        //{
        //    get { return JsonConvert.DeserializeObject<GoodsShelfNumber>(System.Web.HttpUtility.UrlDecode(ShelfNumber)); }
        //    set { }
        //}
        [JsonConverter(typeof(HttpUrlEncodeConverter))]
        public string ShelfNumber { get; set; }
        public int FreightPayer { get; set; }// 运费承担方式（1 卖家承担，0 买
        public bool HasInvoice { get; set; }// 是否有发票
        public bool HasWarranty { get; set; }// 是否有保修
        public int ApproveStatus { get; set; }// 商品状态（1在售中，0仓库中）
        public bool Violation { get; set; }// 商品是否违规(1正常,0违规)
        public bool Visible { get; set; }// 商品是否可见(1可见,0不可见)
        public bool Borrow { get; set; }// 是否支持借出
        public bool Recommend { get; set; }// 是否推荐
        [ID(false)]
        public string PlatformID { get; set; }// 所属平台ID
    }

    public class GoodsShelfNumber
    {
        public List<ShelfNumber> ShelfNumber { get; set; }
    }
    public class ShelfNumber
    {
        public string Number { get; set; }// 架号
        public string Name { get; set; } //架名
        public string GoodsAmount { get; set; } //架上商品数量
        public string Location { get; set; }// 所在位置
    }
}
