﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenStorageService
{
    /// <summary>
    /// 表示OSS实体的所有者。
    /// </summary>
    [XmlRoot("Owner")]
    public class Owner : ICloneable
    {
        /// <summary>
        /// 获取或设置所有者的ID。
        /// </summary>
        [XmlElement("ID")]
        public string Id { get; set; }
        
        /// <summary>
        /// 获取或设置所有者的显示名称。
        /// </summary>
        [XmlElement("DisplayName")]
        public string DisplayName { get; set; }
        
        /// <summary>
        /// 构造一个新的<see cref="Owner" />实例。
        /// </summary>
        internal Owner()
        {
        }
        
        /// <summary>
        /// 使用给定的所有者ID和显示名称构造一个新的<see cref="Owner" />实例。
        /// </summary>
        /// <param name="id">所有者的ID。</param>
        /// <param name="displayName">所有者的显示名称。</param>
        internal Owner(string id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                                 "[Owner Id={0}, DisplayName={1}]",
                                 Id ?? string.Empty,
                                 DisplayName ?? string.Empty);
        }

        public object Clone()
        {
            return new Owner(Id, DisplayName);
        }
    }
}
