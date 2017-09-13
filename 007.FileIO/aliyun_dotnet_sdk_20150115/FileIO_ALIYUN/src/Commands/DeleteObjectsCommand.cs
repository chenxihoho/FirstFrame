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

using Aliyun.OpenServices.OpenStorageService.Transform;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Domain;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    /// <summary>
    /// Description of DeleteObjectsCommand.
    /// </summary>
    internal class DeleteObjectsCommand : OssCommand<DeleteObjectsResult>
    {
        private readonly DeleteObjectsRequest _deleteObjectsRequest;
        
        protected override string Bucket
        {
            get { return _deleteObjectsRequest.BucketName; }
        }
        
        protected override HttpMethod Method
        {
            get { return HttpMethod.Post; }
        }

        protected override IDictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "delete", null }
                };
            }
        }
        
        protected override Stream Content
        {
            get 
            { 
                return SerializerFactory.GetFactory().CreateDeleteObjectsRequestSerializer()
                                .Serialize(_deleteObjectsRequest); 
            }
        }

        protected override IDictionary<string, string> Headers
        {
            get
            {
                var headers = new Dictionary<string, string>();
                headers[HttpHeaders.ContentLength] = Content.Length.ToString();
                headers[HttpHeaders.ContentMd5] = OssUtils.ComputeContentMd5(Content);
                return headers;
            }
        }

        private DeleteObjectsCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                 IDeserializer<ServiceResponse, DeleteObjectsResult> deserializeMethod,
                                DeleteObjectsRequest deleteObjectsRequest)
                        : base(client, endpoint, context, deserializeMethod)
        {
            Debug.Assert(deleteObjectsRequest != null);
            _deleteObjectsRequest = deleteObjectsRequest;
        }

        public static DeleteObjectsCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context,
                                              DeleteObjectsRequest deleteObjectsRequest)
        {
            if (deleteObjectsRequest == null)
                throw new ArgumentException("deleteObjectsRequest should not be null or empty");
            if (string.IsNullOrEmpty(deleteObjectsRequest.BucketName))
                throw new ArgumentException("deleteObjectsRequest.BucketName should not be null or empty");
            if (!OssUtils.IsBucketNameValid(deleteObjectsRequest.BucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");

            return new DeleteObjectsCommand(client, endpoint, context, 
                                                      DeserializerFactory.GetFactory().CreateDeleteObjectsResultDeserializer(),
                                                        deleteObjectsRequest);
        }
    }
}
