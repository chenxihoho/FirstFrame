/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.IO;

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.Properties;
using Aliyun.OpenServices.OpenStorageService.Transform;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Domain;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    /// <summary>
    /// Description of PutObjectCommand.
    /// </summary>
    internal class PutObjectCommand : OssCommand<PutObjectResult>
    {
        private readonly OssObject _ossObject;

        protected override string Bucket
        {
            get
            {
                return _ossObject.BucketName;
            }
        } 
        
        protected override string Key
        {
            get
            {
                return _ossObject.Key;
            }
        } 
        
        protected override bool LeaveRequestOpen
        {
            get { return true; }
        }

        private PutObjectCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                 IDeserializer<ServiceResponse, PutObjectResult> deserializer,
                                 OssObject ossObject)
            : base(client, endpoint, context, deserializer)
        {
            _ossObject = ossObject;
        }

        protected override HttpMethod Method
        {
            get { return HttpMethod.Put; }
        }

        protected override Stream Content
        {
            get { return _ossObject.Content; }
        }

        protected override IDictionary<string, string> Headers
        {
            get
            {
                var headers = new Dictionary<string, string>();
                _ossObject.Metadata.Populate(headers);
                return headers;
            }
        }

        public static PutObjectCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context,
                                              string bucketName, string key,
                                              Stream content, ObjectMetadata metadata)
        {
            if (string.IsNullOrEmpty(bucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");

            if (!OssUtils.IsBucketNameValid(bucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
            if (!OssUtils.IsObjectKeyValid(key))
                throw new ArgumentException(OssResources.ObjectKeyInvalid, "key");

            if (content == null)
                throw new ArgumentNullException("content");

            var ossObject = new OssObject(key);
            ossObject.BucketName = bucketName;
            ossObject.Content = content;
            ossObject.Metadata = metadata ?? new ObjectMetadata();

            return new PutObjectCommand(client, endpoint, context,
                                        DeserializerFactory.GetFactory().CreatePutObjectReusltDeserializer(),
                                        ossObject);
        }
    }
}
