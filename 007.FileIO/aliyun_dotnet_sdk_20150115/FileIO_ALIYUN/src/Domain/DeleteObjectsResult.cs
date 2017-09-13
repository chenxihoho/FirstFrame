/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenStorageService
{
    /// <summary>
    /// Description of DeleteObjectsResult.
    /// </summary>
    [XmlRoot("DeleteResult")]
    public class DeleteObjectsResult
    {
        [XmlElement("Deleted")]
        public DeletedObject[] keys { get; set; }

        internal DeleteObjectsResult()
        {
        }
        
        [XmlRoot("Deleted")]
        public class DeletedObject
        {
            [XmlElement("Key")]
            public string Key { get; set; }
        }
    }
}
