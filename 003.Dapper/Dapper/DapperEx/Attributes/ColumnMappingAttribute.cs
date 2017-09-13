using System;

namespace FirstFrame.DapperEx
{
    /// <summary>
    /// 忽略字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ColumnMappingAttribute : BaseAttribute
    {
        public string Name { get; set; }
    }
}
