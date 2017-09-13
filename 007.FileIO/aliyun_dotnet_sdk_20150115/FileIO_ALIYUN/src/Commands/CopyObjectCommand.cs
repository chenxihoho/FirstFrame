/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.Properties;
using Aliyun.OpenServices.OpenStorageService.Transform;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Domain;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    /// <summary>
    /// Description of CopyObjectCommand.
    /// </summary>
    internal class CopyObjectCommand : OssCommand<CopyObjectResult>
    {
        
        private readonly CopyObjectRequest _copyObjectRequset;
        
        
        protected override string Bucket
        {
            get
            {
                return _copyObjectRequset.DestinationBucketName;
            }
        }
        
        protected override string Key
        {
            get
            {
                return _copyObjectRequset.DestinationKey;
            }
        }
        
        protected override HttpMethod Method
        {
            get { return HttpMethod.Put; }
        }
        
        protected override IDictionary<string, string> Headers
        {
            get
            {
                var headers = new Dictionary<string, string>();
                _copyObjectRequset.Populate(headers);
                return headers;
            }
        }
        
        private CopyObjectCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                 IDeserializer<ServiceResponse, CopyObjectResult> deserializer,
                                 CopyObjectRequest copyObjectRequest)
                        : base(client, endpoint, context, deserializer)
        {
            Debug.Assert(copyObjectRequest != null);
            _copyObjectRequset = copyObjectRequest;
        }
        
        public static CopyObjectCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context,
                                              CopyObjectRequest copyObjectRequest)
        {
            if (string.IsNullOrEmpty(copyObjectRequest.SourceBucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "sourceBucketName");
            if (string.IsNullOrEmpty(copyObjectRequest.SourceKey))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "sourceKey");
            
            if (string.IsNullOrEmpty(copyObjectRequest.DestinationBucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "destinationBucketName");
            if (string.IsNullOrEmpty(copyObjectRequest.DestinationKey))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "destinationKey");
            
            if (!OssUtils.IsBucketNameValid(copyObjectRequest.SourceBucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "sourceBucketName");
            if (!OssUtils.IsObjectKeyValid(copyObjectRequest.SourceKey))
                throw new ArgumentException(OssResources.ObjectKeyInvalid, "sourceKey");
            
            if (!OssUtils.IsBucketNameValid(copyObjectRequest.DestinationBucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "destinationBucketName");
            if (!OssUtils.IsObjectKeyValid(copyObjectRequest.DestinationKey))
                throw new ArgumentException(OssResources.ObjectKeyInvalid, "destinationKey");
            
            return new CopyObjectCommand(client, endpoint, context,
                                         DeserializerFactory.GetFactory().CreateCopyObjectResultDeserializer(),
                                        copyObjectRequest);
        
        }
    }
}
