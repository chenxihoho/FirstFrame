/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Transform;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    /// <summary>
    /// Description of GetObjectCommand.
    /// </summary>
    internal class GetObjectCommand : OssCommand<OssObject>
    {
        private readonly GetObjectRequest _request;

        protected override string Bucket
        {
            get
            {
                return _request.BucketName;
            }
        }
        
        protected override string Key
        {
            get
            {
                return _request.Key;
            }
        }

        protected override IDictionary<string, string> Headers
        {
            get
            {
                var headers = new Dictionary<string, string>();
                _request.Populate(headers);
                return headers;
            }
        }

        protected override IDictionary<string, string> Parameters
        {
            get
            {
                var parameters = base.Parameters;
                _request.ResponseHeaders.Populate(parameters);
                return parameters;
            }
        }

        protected override bool LeaveResponseOpen
        {
            get { return true; }
        }

        private GetObjectCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                 IDeserializer<ServiceResponse, OssObject> deserializer,
                                 GetObjectRequest request)
            : base(client, endpoint, context, deserializer)
        {
            _request = request;
        }

        public static GetObjectCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context,
                                              GetObjectRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            return new GetObjectCommand(client, endpoint, context,
                                        DeserializerFactory.GetFactory().CreateGetObjectResultDeserializer(request),
                                        request);
        }

        public static GetObjectCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context,
                                              string bucketName, string key)
        {
            return Create(client, endpoint, context, new GetObjectRequest(bucketName, key));
        }
    }
}
