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
using System.IO;
using Aliyun.OpenServices.Properties;
using Aliyun.OpenServices.OpenStorageService.Transform;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Domain;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    /// <summary>
    /// Description of SetBucketLoggingCommand.
    /// </summary>
    internal class SetBucketLoggingCommand : OssCommand
    {
        private readonly string _bucketName;
        private readonly SetBucketLoggingRequest _setBucketLoggingRequest;

        protected override HttpMethod Method
        {
            get { return HttpMethod.Put; }
        }

        protected override string Bucket
        {
            get
            {
                return _bucketName;
            }
        }

        private SetBucketLoggingCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                    string bucketName, SetBucketLoggingRequest setBucketLoggingRequest)
            : base(client, endpoint, context)
        {
            Debug.Assert(setBucketLoggingRequest != null);

            if (string.IsNullOrEmpty(bucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            if (!OssUtils.IsBucketNameValid(bucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");

            _bucketName = bucketName;
            _setBucketLoggingRequest = setBucketLoggingRequest;
        }

        public static SetBucketLoggingCommand Create(IServiceClient client, Uri endpoint,
                                                 ExecutionContext context,
                                                 string bucketName, SetBucketLoggingRequest setBucketLoggingRequest)
        {
            return new SetBucketLoggingCommand(client, endpoint, context, bucketName, setBucketLoggingRequest);
        }
        
        
        protected override IDictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "logging", null }
                };
            }
        }
        protected override Stream Content
        {
            get
            {
                return SerializerFactory.GetFactory()
                    .CreateSetBucketLoggingRequestSerializer()
                    .Serialize(_setBucketLoggingRequest);
            }
        }
    }
}
