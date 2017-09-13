/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */
 
using System;
using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenStorageService.Model
{
    /// <summary>
    /// Description of UploadPartCopyRequestModel.
    /// </summary>
    [XmlRoot("CopyPartResult")]
    public class UploadPartCopyRequestModel
    {
        [XmlElement("LastModified")]
        public DateTime LastModified { get; set; }
        
        [XmlElement("ETag")]
        public string ETag { get; set; }
    }
}
