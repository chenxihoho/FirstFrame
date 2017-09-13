/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Utilities;

namespace Aliyun.OpenServices.OpenStorageService
{
    /// <summary>
    /// 表示一组常用的用户访问权限。
    /// <para>
    /// 这一组常用权限相当于给所有用户指定权限的快捷方法。
    /// </para>
    /// </summary>
    public enum CannedAccessControlList
    {
        /// <summary>
        /// 
        /// </summary>
        [StringValue("private")]
        Private = 0,

        /// <summary>
        /// 
        /// </summary>
        [StringValue("public-read")]
        PublicRead,

        /// <summary>
        /// 
        /// </summary>
        [StringValue("public-read-write")]
        PublicReadWrite
    }
}
