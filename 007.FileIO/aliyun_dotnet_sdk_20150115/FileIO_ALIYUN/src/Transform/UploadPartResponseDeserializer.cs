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
    /// Description of UploadPartResultDeserializer.
    /// </summary>
    internal class UploadPartResultDeserializer : ResponseDeserializer<UploadPartResult, UploadPartResult>
    {
        private readonly int _partNumber;
        
        public UploadPartResultDeserializer(int partNumber)
            : base(null)
        {
            _partNumber = partNumber;
        }
        
        public override UploadPartResult Deserialize(ServiceResponse response)
        {
            Debug.Assert(response != null);
            var result = new UploadPartResult();
            if (response.Headers.ContainsKey(HttpHeaders.ETag))
            {
                result.ETag = OssUtils.TrimETag(response.Headers[HttpHeaders.ETag]);
            }
            result.PartNumber = _partNumber;
            
            return result;
        }
    }
}
