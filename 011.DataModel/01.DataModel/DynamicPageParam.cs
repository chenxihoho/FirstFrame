
using System.Collections.Generic;

namespace FirstFrame.DataModel
{
    public class DynamicPageParam
    {
        public string TableName { get; set; }// " tbGoods A "
        public string FieldName { get; set; }// " A.* "
        public string Condition { get; set; }// " A.PlatformID = "0" AND A.SellerCategoryId = "0" 
        public string FieldSort { get; set; }// " ListTime desc "
        public string PageSize { get; set; }// 每页显示条数
        public string PageIndex { get; set; }// 当前第几页
        public string ResultFormat { get; set; }// 返回数据格式：BaseConst.FORMAT_JSON，BaseConst.FORMAT_TEXT
    }
    public class PageResult<T>
    {
        public long RecordCount { get; set; }
        public List<T> ResultList { get; set; }
    }

}
