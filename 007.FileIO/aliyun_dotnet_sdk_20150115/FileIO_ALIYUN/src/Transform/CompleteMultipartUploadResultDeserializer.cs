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
    /// Description of CompleteMultipartUploadResultDeserializer.
    /// </summary>
    internal class CompleteMultipartUploadResultDeserializer 
        : ResponseDeserializer<CompleteMultipartUploadResult, CompleteMultipartUploadResultModel>
    {
        public CompleteMultipartUploadResultDeserializer(IDeserializer<Stream, CompleteMultipartUploadResultModel> contentDeserializer)
                 : base(contentDeserializer)
        {
        }
        
        public override CompleteMultipartUploadResult Deserialize(ServiceResponse response)
        {
            var result = ContentDeserializer.Deserialize(response.Content);
            var completeMultipartUploadResult = new CompleteMultipartUploadResult();
            completeMultipartUploadResult.BucketName = result.Bucket;
            completeMultipartUploadResult.Key = result.Key;
            completeMultipartUploadResult.Location = result.Location;
            completeMultipartUploadResult.ETag = OssUtils.TrimETag(result.ETag);
            return completeMultipartUploadResult;
        }
    }
}
