/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.Properties;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Domain;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    /// <summary>
    /// Description of SetBucketAclCommand.
    /// </summary>
    internal class SetBucketAclCommand : OssCommand
    {
        private readonly string _bucketName;
        private readonly SetBucketAclRequest _request;

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

        private SetBucketAclCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                    string bucketName, SetBucketAclRequest request)
            : base(client, endpoint, context)
        {
            if (string.IsNullOrEmpty(bucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            if (!OssUtils.IsBucketNameValid(bucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");

            _bucketName = bucketName;
            _request = request;
        }

        public static SetBucketAclCommand Create(IServiceClient client, Uri endpoint,
                                                 ExecutionContext context,
                                                 string bucketName, SetBucketAclRequest request)
        {
            return new SetBucketAclCommand(client, endpoint, context, bucketName, request);
        }
        
        protected override IDictionary<string, string> Headers
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { OssHeaders.OssCannedAcl, _request.ACL.GetStringValue() }
                };
            }
        }
        
        protected override IDictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "acl", null }
                };
            }
        }
    }
}
