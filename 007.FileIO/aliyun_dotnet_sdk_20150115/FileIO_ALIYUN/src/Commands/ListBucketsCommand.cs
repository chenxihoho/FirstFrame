/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Transform;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    /// <summary>
    /// Description of ListBucketsCommand.
    /// </summary>
    internal class ListBucketsCommand : OssCommand<ListBucketsResult>
    {
        private readonly ListBucketsRequest _request;

        protected override IDictionary<string, string> Parameters
        {
            get
            {
                var parameters = base.Parameters;
                if (_request != null)
                {
                    Populate(_request, parameters);
                }
                return parameters;
            }
        }

        private static void Populate(ListBucketsRequest request, IDictionary<string, string> parameters)
        {
            if (request.Prefix != null)
            {
                parameters["prefix"] = request.Prefix;
            }

            if (request.Marker != null)
            {
                parameters["marker"] = request.Marker;
            }

            if (request.MaxKeys.HasValue)
            {
                parameters["max-keys"] = request.MaxKeys.Value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public ListBucketsCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                 IDeserializer<ServiceResponse, ListBucketsResult> deserializeMethod, ListBucketsRequest request)
            : base(client, endpoint, context, deserializeMethod)
        {
            _request = request;
        }
        
        public static ListBucketsCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context,
            ListBucketsRequest request)
        {
            return new ListBucketsCommand(client, endpoint, context,
                                          DeserializerFactory.GetFactory().CreateListBucketResultDeserializer(), request);
        }
    }
}
