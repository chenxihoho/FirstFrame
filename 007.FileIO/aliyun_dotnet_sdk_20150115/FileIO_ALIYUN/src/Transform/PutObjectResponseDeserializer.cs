/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.Diagnostics;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenStorageService.Utilities;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    /// <summary>
    /// Description of PutObjectResponseDeserializer.
    /// </summary>
    internal class PutObjectResponseDeserializer : ResponseDeserializer<PutObjectResult, PutObjectResult>
    {
        public PutObjectResponseDeserializer()
            : base(null)
        {
        }
        
        public override PutObjectResult Deserialize(ServiceResponse response)
        {
            Debug.Assert(response != null);
            var result = new PutObjectResult();
            if (response.Headers.ContainsKey(HttpHeaders.ETag))
            {
                result.ETag = OssUtils.TrimETag(response.Headers[HttpHeaders.ETag]);
            }
            
            return result;
        }
    }
}
