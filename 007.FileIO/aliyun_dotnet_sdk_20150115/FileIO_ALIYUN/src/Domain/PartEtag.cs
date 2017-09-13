/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

namespace Aliyun.OpenServices.OpenStorageService
{
    /// <summary>
    /// 某块PartNumber和ETag的信息，用于Complete Multipart Upload请求参数的设置。
    /// </summary>
    public class PartETag
    {
        /// <summary>
        /// 获取或者设置一个值表示表示分块的标识
        /// </summary>
        public int PartNumber { get; set; }
        
        /// <summary>
        /// 获取或者设置一个值表示与Object相关的hex编码的128位MD5摘要。
        /// </summary>
        public string ETag { get; set; }
        
        public PartETag(int partNumber, string eTag)
        {
            PartNumber = partNumber;
            ETag = eTag;
        }
    }
}
