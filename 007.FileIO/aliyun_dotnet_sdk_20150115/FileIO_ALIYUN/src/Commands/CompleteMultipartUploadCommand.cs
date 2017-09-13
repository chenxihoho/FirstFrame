/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */
 
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

using Aliyun.OpenServices.Properties;
using Aliyun.OpenServices.OpenStorageService.Transform;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Domain;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    /// <summary>
    /// Description of CompleteMultipartUploadCommand.
    /// </summary>
    internal class CompleteMultipartUploadCommand : OssCommand<CompleteMultipartUploadResult>
    {
        private readonly CompleteMultipartUploadRequest _completeMultipartUploadRequest;
        
        protected override string Bucket
        {
            get
            {
                return _completeMultipartUploadRequest.BucketName;
            }
        }
        
        protected override string Key
        {
            get
            {
                return _completeMultipartUploadRequest.Key;
            }
        }
        
        protected override HttpMethod Method
        {
            get { return HttpMethod.Post; }
        }
        
        
        protected override IDictionary<string, string> Parameters
        {
            get 
            {
                var parameters = base.Parameters;
                parameters["uploadId"] = _completeMultipartUploadRequest.UploadId;
                return parameters;
            }
        }
        
        protected override Stream Content
        {
            get 
            { 
                return SerializerFactory.GetFactory()
                                .CreateCompleteUploadRequestSerializer()
                                .Serialize(_completeMultipartUploadRequest); 
            }
        }
        
        private CompleteMultipartUploadCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                 IDeserializer<ServiceResponse, CompleteMultipartUploadResult> deserializeMethod,
                                CompleteMultipartUploadRequest completeMultipartUploadRequest)
                        : base(client, endpoint, context, deserializeMethod)
        {
            Debug.Assert(completeMultipartUploadRequest != null);
            _completeMultipartUploadRequest = completeMultipartUploadRequest;
        }
        
        public static CompleteMultipartUploadCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context,
                                              CompleteMultipartUploadRequest completeMultipartUploadRequest)
        {
            if (completeMultipartUploadRequest == null)
                throw new ArgumentNullException("completeMultipartUploadRequest");
            if (string.IsNullOrEmpty(completeMultipartUploadRequest.BucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            if (string.IsNullOrEmpty(completeMultipartUploadRequest.Key))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
            if (string.IsNullOrEmpty(completeMultipartUploadRequest.UploadId))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "uploadId"); 
            
            if (!OssUtils.IsBucketNameValid(completeMultipartUploadRequest.BucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
            if (!OssUtils.IsObjectKeyValid(completeMultipartUploadRequest.Key))
                throw new ArgumentException(OssResources.ObjectKeyInvalid, "key");
            
            return new CompleteMultipartUploadCommand(client, endpoint, context, 
                                                      DeserializerFactory.GetFactory().CreateCompleteUploadResultDeserializer(),
                                                        completeMultipartUploadRequest);
            
        }
    }
}
