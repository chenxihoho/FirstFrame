/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */
 
using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.Properties;
using Aliyun.OpenServices.OpenStorageService.Transform;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Domain;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    /// <summary>
    /// Description of UploadPartCommand.
    /// </summary>
    internal class UploadPartCommand : OssCommand<UploadPartResult>
    {
        private readonly UploadPartRequest _uploadPartRequest;
        
        protected override HttpMethod Method
        {
            get { return HttpMethod.Put; }
        }

        protected override string Bucket
        {
            get
            {
                return _uploadPartRequest.BucketName;
            }
        } 
        
        protected override string Key
        {
            get
            {
                return _uploadPartRequest.Key;
            }
        } 
        
        protected override IDictionary<string, string> Parameters
        {
            get 
            {
                var parameters = base.Parameters;
                parameters["partNumber"] = _uploadPartRequest.PartNumber.ToString();
                parameters["uploadId"] = _uploadPartRequest.UploadId;
                return parameters;
            }
        }
        
        protected override IDictionary<string, string> Headers
        {
            get
            {
                var headers = new Dictionary<string, string>();
                headers[HttpHeaders.ContentLength] = _uploadPartRequest.PartSize.ToString();
                return headers;
            }
        }
        
        protected override Stream Content
        {
            get { return _uploadPartRequest.InputStream; }
        }
        
        protected override bool LeaveRequestOpen
        {
            get { return true; }
        }
        
        private UploadPartCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                 IDeserializer<ServiceResponse, UploadPartResult> deserializer,                                  
                                                UploadPartRequest uploadPartRequest)
            : base(client, endpoint, context, deserializer)
        {
            Debug.Assert(uploadPartRequest != null);
            _uploadPartRequest = uploadPartRequest;
        }
        
        private static bool IsPartNumberInRange(int? partNumber)
        {
            return (partNumber > 0 && partNumber <= 10000);
        }
        
        public static UploadPartCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context,
                                                 UploadPartRequest uploadPartRequest)
        {
            if (uploadPartRequest == null)
                throw new ArgumentNullException("uploadPartRequest");
            if (string.IsNullOrEmpty(uploadPartRequest.BucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            if (string.IsNullOrEmpty(uploadPartRequest.Key))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
            if (string.IsNullOrEmpty(uploadPartRequest.UploadId))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "uploadId");  
            if (uploadPartRequest.PartNumber == null)
                throw new ArgumentNullException("partNumber");
            if (uploadPartRequest.PartSize == null)
                throw new ArgumentNullException("partSize");
            
            if (!OssUtils.IsBucketNameValid(uploadPartRequest.BucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
            if (!OssUtils.IsObjectKeyValid(uploadPartRequest.Key))
                throw new ArgumentException(OssResources.ObjectKeyInvalid, "key");
            
            if (uploadPartRequest.InputStream == null)
                throw new ArgumentNullException("inputStream");
            
            if (uploadPartRequest.PartSize < 0 || uploadPartRequest.PartSize > OssUtils.MaxFileSize)
                throw new ArgumentOutOfRangeException("partSize");
            if (!IsPartNumberInRange(uploadPartRequest.PartNumber))
                throw new ArgumentOutOfRangeException("partNumber");    

            return new UploadPartCommand(client, endpoint, context, 
                                         DeserializerFactory.GetFactory().CreateUploadPartResultDeserializer((int)uploadPartRequest.PartNumber),
                                         uploadPartRequest);
        }
    }
}
