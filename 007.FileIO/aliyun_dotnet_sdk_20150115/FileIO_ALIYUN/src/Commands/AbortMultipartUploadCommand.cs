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
using Aliyun.OpenServices.Properties;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Domain;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    /// <summary>
    /// Description of AbortMultipartUploadCommand.
    /// </summary>
    internal class AbortMultipartUploadCommand : OssCommand
    {
        private readonly AbortMultipartUploadRequest _abortMultipartUploadRequest;
        
        protected override HttpMethod Method
        {
            get { return HttpMethod.Delete; }
        }

        protected override string Bucket
        {
            get
            {
                return _abortMultipartUploadRequest.BucketName;
            }
        }
        
        protected override string Key
        {
            get
            {
                return _abortMultipartUploadRequest.Key;
            }
        }
        
        protected override IDictionary<string, string> Parameters
        {
            get 
            {
                var parameters = base.Parameters;
                parameters["uploadId"] = _abortMultipartUploadRequest.UploadId;
                return parameters;
            }
        }
        
        private AbortMultipartUploadCommand(IServiceClient client, Uri endpoint, ExecutionContext context, 
                                                AbortMultipartUploadRequest abortMultipartUploadRequest)
            : base(client, endpoint, context)
            
        {
            Debug.Assert(abortMultipartUploadRequest != null);
            _abortMultipartUploadRequest = abortMultipartUploadRequest;
        }
        

        public static AbortMultipartUploadCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context,
                                                 AbortMultipartUploadRequest abortMultipartUploadRequest)
        {
            if (abortMultipartUploadRequest == null)
                throw new ArgumentNullException("abortMultipartUploadRequest");
            if (string.IsNullOrEmpty(abortMultipartUploadRequest.BucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            if (string.IsNullOrEmpty(abortMultipartUploadRequest.Key))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
            if (string.IsNullOrEmpty(abortMultipartUploadRequest.UploadId))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "uploadId");            
            
            if (!OssUtils.IsBucketNameValid(abortMultipartUploadRequest.BucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
            if (!OssUtils.IsObjectKeyValid(abortMultipartUploadRequest.Key))
                throw new ArgumentException(OssResources.ObjectKeyInvalid, "key");
            
            return new AbortMultipartUploadCommand(client, endpoint, context, abortMultipartUploadRequest);
        }
    }
}
