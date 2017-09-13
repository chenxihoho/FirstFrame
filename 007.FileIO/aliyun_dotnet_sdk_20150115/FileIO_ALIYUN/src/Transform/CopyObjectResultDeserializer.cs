/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.IO;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Model;
using Aliyun.OpenServices.OpenStorageService.Utilities;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    /// <summary>
    /// Description of CopyObjectResultDeserializer.
    /// </summary>
    internal class CopyObjectResultDeserializer : ResponseDeserializer<CopyObjectResult, CopyObjectResultModel>
    {
        public CopyObjectResultDeserializer(IDeserializer<Stream, CopyObjectResultModel> contentDeserializer)
                 : base(contentDeserializer)
        {
        }
        
        public override CopyObjectResult Deserialize(ServiceResponse response)
        {
            var result = ContentDeserializer.Deserialize(response.Content);
            var copyObjectResult = new CopyObjectResult();
            copyObjectResult.ETag = OssUtils.TrimETag(result.ETag);
            copyObjectResult.LastModified = result.LastModified;
            return copyObjectResult;
        }
    }
}
