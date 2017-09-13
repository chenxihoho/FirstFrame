using System;

namespace FirstFrame.DapperEx
{
    /// <summary>
    /// 所有者
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class OwnerAttribute : Attribute
    {
        public OwnerAttribute()
        { }

        public OwnerAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 别名，对应数据库所有者
        /// </summary>
        public string Name { get; set; }

    }
}
