/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Model;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    internal class GetBucketWebSiteResultDeserializer : ResponseDeserializer<BucketWebsiteResult, SetBucketWebsiteRequestModel>
    {
        public GetBucketWebSiteResultDeserializer(IDeserializer<Stream, SetBucketWebsiteRequestModel> contentDeserializer)
            : base(contentDeserializer)
        {
        }

        public override BucketWebsiteResult Deserialize(ServiceResponse response)
        {
            var model = ContentDeserializer.Deserialize(response.Content);
            var result = new BucketWebsiteResult();
            result.IndexDocument = model.IndexDocument.Suffix;
            result.ErrorDocument = model.ErrorDocument.Key;
            return result;
            
        }
    }
}
