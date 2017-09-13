﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.Diagnostics.CodeAnalysis;

namespace Aliyun.OpenServices.OpenStorageService
{
    /// <summary>
    /// 表示访问控制权限。
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public enum Permission {
        /// <summary>
        /// 只读权限。
        /// </summary>
        Read = 0,

        /// <summary>
        /// 完全控制权限，即可读可写。
        /// </summary>
        FullControl
    }

}
