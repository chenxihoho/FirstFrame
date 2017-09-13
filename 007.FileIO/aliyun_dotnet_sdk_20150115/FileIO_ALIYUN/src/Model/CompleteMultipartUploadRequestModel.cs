﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenStorageService.Model
{
    /// <summary>
    /// Description of CompleteUploadRequest.
    /// </summary>
    [XmlRoot("CompleteMultipartUpload")]
    public class CompleteMultipartUploadRequestModel
    {
        [XmlElement("Part")]
        public CompletePart[] Parts { get; set; }

        [XmlRoot("Part")]
        public class CompletePart
        {
            [XmlElement("PartNumber")]
            public int PartNumber { get; set; }
            
            [XmlElement("ETag")]
            public string ETag { get; set; }
        }
    }
}
