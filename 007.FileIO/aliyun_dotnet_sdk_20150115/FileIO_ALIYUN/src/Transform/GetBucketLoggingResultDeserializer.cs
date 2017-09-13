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
    /// Description of GetBucketLoggingResultDeserializer.
    /// </summary>
    internal class GetBucketLoggingResultDeserializer 
        : ResponseDeserializer<BucketLoggingResult, SetBucketLoggingRequestModel>
    {
      public GetBucketLoggingResultDeserializer(IDeserializer<Stream, SetBucketLoggingRequestModel> contentDeserializer)
            : base(contentDeserializer)
        {
        }

      public override BucketLoggingResult Deserialize(ServiceResponse response)
        {
            var model = ContentDeserializer.Deserialize(response.Content);
            var result = new BucketLoggingResult();
            result.TargetBucket = model.LoggingEnabled.TargetBucket;
            result.TargetPrefix = model.LoggingEnabled.TargetPrefix;
            return result;
            
        }
    }
}
