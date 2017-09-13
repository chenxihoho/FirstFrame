using System;

namespace FirstFrame.DapperEx
{
    /// <summary>
    /// 数据库
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class DatabaseAttribute : Attribute
    {
        public DatabaseAttribute()
        { }

        public DatabaseAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 别名，对应数据库的名字
        /// </summary>
        public string Name { get; set; }

    }
}
