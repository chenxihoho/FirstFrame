using System;

namespace FirstFrame.DapperEx
{
    /// <summary>
    /// 数据库表
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class, Inherited = false)]
    public class TableAttribute : Attribute
    {
        public TableAttribute()
        { }

        public TableAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 别名，对应数据里面的名字
        /// </summary>
        public string Name { get; set; }
    }

}
