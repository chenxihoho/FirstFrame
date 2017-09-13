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

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    /// <summary>
    /// Description of InitiateMultipartUploadResultDeserializer.
    /// </summary>
    internal class InitiateMultipartUploadResultDeserializer 
        : ResponseDeserializer<InitiateMultipartUploadResult, InitiateMultipartResult>
    {
        public InitiateMultipartUploadResultDeserializer(IDeserializer<Stream, InitiateMultipartResult> contentDeserializer)
                 : base(contentDeserializer)
        {
        }
        
        public override InitiateMultipartUploadResult Deserialize(ServiceResponse response)
        {
            var result = ContentDeserializer.Deserialize(response.Content);
            var initiateMultipartUploadResult = new InitiateMultipartUploadResult();
            initiateMultipartUploadResult.BucketName = result.Bucket;
            initiateMultipartUploadResult.Key = result.Key;
            initiateMultipartUploadResult.UploadId = result.UploadId;
            return initiateMultipartUploadResult;
        }
    }
}
