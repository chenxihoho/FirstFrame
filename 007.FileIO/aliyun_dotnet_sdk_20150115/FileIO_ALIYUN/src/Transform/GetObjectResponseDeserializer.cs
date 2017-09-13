/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.Diagnostics;
using Aliyun.OpenServices.Common.Communication;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    /// <summary>
    /// Description of GetObjectResponseDeserializer.
    /// </summary>
    internal class GetObjectResponseDeserializer : ResponseDeserializer<OssObject, OssObject>
    {
        private readonly GetObjectRequest _getObjectRequest;

        public GetObjectResponseDeserializer(GetObjectRequest getObjectRequest)
            : base(null)
        {
            Debug.Assert(getObjectRequest != null
                         && !string.IsNullOrEmpty(getObjectRequest.BucketName)
                         && !string.IsNullOrEmpty(getObjectRequest.Key));
            _getObjectRequest = getObjectRequest;
        }

        public override OssObject Deserialize(ServiceResponse response)
        {
            var ossObject = new OssObject(_getObjectRequest.Key);
            ossObject.BucketName = _getObjectRequest.BucketName;
            ossObject.Content = response.Content;
            ossObject.Metadata = DeserializerFactory.GetFactory()
                .CreateGetObjectMetadataResultDeserializer().Deserialize(response);

            return ossObject;
        }
    }
}
