/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.Diagnostics;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Properties;
using Aliyun.OpenServices.OpenStorageService.Transform;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using Aliyun.OpenServices.Domain;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    /// <summary>
    /// Description of SetBucketCorsCommand.
    /// </summary>
    internal class SetBucketCorsCommand : OssCommand
    {
        private readonly string _bucketName;
        private readonly SetBucketCorsRequest _setBucketCorsRequest;

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

        protected override Stream Content
        {
            get
            {
                return SerializerFactory.GetFactory()
                                .CreateSetBucketCorsRequestSerializer()            
                                .Serialize(_setBucketCorsRequest);
            }
        }

        private SetBucketCorsCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                    string bucketName, SetBucketCorsRequest setBucketCorsRequest)
            : base(client, endpoint, context)
        {
            Debug.Assert(setBucketCorsRequest != null);

            if (string.IsNullOrEmpty(bucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            if (!OssUtils.IsBucketNameValid(bucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");

            _bucketName = bucketName;
            _setBucketCorsRequest = setBucketCorsRequest;
        }

        public static SetBucketCorsCommand Create(IServiceClient client, Uri endpoint,
                                                 ExecutionContext context,
                                                 string bucketName, SetBucketCorsRequest setBucketCorsRequest)
        {
            return new SetBucketCorsCommand(client, endpoint, context, bucketName, setBucketCorsRequest);
        }


        protected override IDictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "cors", null }
                };
            }
        }
    }
}
