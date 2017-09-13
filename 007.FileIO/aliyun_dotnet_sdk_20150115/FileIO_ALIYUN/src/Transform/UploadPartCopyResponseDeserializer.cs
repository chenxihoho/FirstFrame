/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.IO;
using System.Diagnostics;

using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.OpenStorageService.Model;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    /// <summary>
    /// Description of UploadPartCopyResultDeserializer.
    /// </summary>
    internal class UploadPartCopyResultDeserializer 
        : ResponseDeserializer<UploadPartCopyResult, UploadPartCopyRequestModel>
    {
        private readonly int _partNumber;

        public UploadPartCopyResultDeserializer(IDeserializer<Stream, UploadPartCopyRequestModel> contentDeserializer, int partNumber)
            : base(contentDeserializer)
        {
            _partNumber = partNumber;
        }
        
        public override UploadPartCopyResult Deserialize(ServiceResponse response)
        {
            Debug.Assert(response != null);

            var partCopyRequestModel = ContentDeserializer.Deserialize(response.Content);
            var result = new UploadPartCopyResult();
            result.ETag = OssUtils.TrimETag(partCopyRequestModel.ETag);
            result.PartNumber = _partNumber;
            
            return result;
        }
    }
}
