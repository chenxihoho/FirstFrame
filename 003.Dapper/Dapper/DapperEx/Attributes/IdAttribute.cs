﻿using System;

namespace FirstFrame.DapperEx
{
    /// <summary>
    /// 主键
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IDAttribute : BaseAttribute
    {
        /// <summary>
        /// 是否为自动主键
        /// </summary>
        public bool CheckAutoId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDAttribute()
        {
            this.CheckAutoId = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkAutoId">是否为自动主键</param>
        public IDAttribute(bool checkAutoId)
        {
            this.CheckAutoId = checkAutoId;
        }

    }
}
