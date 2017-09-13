﻿/*
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
using System.IO;
using Aliyun.OpenServices.OpenStorageService.Transform;
using Aliyun.OpenServices.Domain;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    /// <summary>
    /// Description of SetBucketWebsiteCommand.
    /// </summary>
    internal class SetBucketWebsiteCommand : OssCommand
    {
        private readonly string _bucketName;
        private readonly SetBucketWebsiteRequest _setBucketWebsiteRequest;

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

        private SetBucketWebsiteCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                    string bucketName, SetBucketWebsiteRequest setBucketWebsiteRequest)
            : base(client, endpoint, context)
        {
            Debug.Assert(setBucketWebsiteRequest != null);

            if (string.IsNullOrEmpty(bucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            if (!OssUtils.IsBucketNameValid(bucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
            if (string.IsNullOrEmpty(setBucketWebsiteRequest.IndexDocument))
                throw new ArgumentException("index document must not be empty");
            if (!OssUtils.IsWebpageValid(setBucketWebsiteRequest.IndexDocument))
                throw new ArgumentException("Invalid index document, must be end with .html");
            if (!string.IsNullOrEmpty(setBucketWebsiteRequest.ErrorDocument) 
                && !OssUtils.IsWebpageValid(setBucketWebsiteRequest.ErrorDocument))
                throw new ArgumentException("Invalid error document, must be end with .html");

            _bucketName = bucketName;
            _setBucketWebsiteRequest = setBucketWebsiteRequest;
        }

        public static SetBucketWebsiteCommand Create(IServiceClient client, Uri endpoint,
                                                 ExecutionContext context,
                                                 string bucketName, SetBucketWebsiteRequest setBucketWebsiteRequest)
        {
            return new SetBucketWebsiteCommand(client, endpoint, context, bucketName, setBucketWebsiteRequest);
        }
        
        
        protected override IDictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "website", null }
                };
            }
        }
        protected override Stream Content
        {
            get
            {
                return SerializerFactory.GetFactory()
                    .CreateSetBucketWebsiteRequestSerializer()
                    .Serialize(_setBucketWebsiteRequest);
            }
        }
    }
}
