using System;

namespace ybzf.Storage.Dapper.Rainbow
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IgnorePropertyAttribute : Attribute
    {
        public IgnorePropertyAttribute(bool ignore)
        {
            Value = ignore;
        }

        public bool Value { get; set; }
    }
}
