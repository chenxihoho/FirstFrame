/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Transform;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    /// <summary>
    /// Description of ListObjectsCommand.
    /// </summary>
    internal class ListObjectsCommand : OssCommand<ObjectListing>
    {
        private readonly ListObjectsRequest _listObjectsRequest;

        protected override string Bucket
        {
            get
            {
                return _listObjectsRequest.BucketName;
            }
        }  

        protected override IDictionary<string, string> Parameters
        {
            get 
            {
                var parameters = base.Parameters;
                Populate(_listObjectsRequest, parameters);
                return parameters;
            }
        }

        private ListObjectsCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                   IDeserializer<ServiceResponse, ObjectListing> deserializer,
                                   ListObjectsRequest listObjectsRequest)
            : base(client, endpoint, context, deserializer)
        {
            Debug.Assert(listObjectsRequest != null);
            _listObjectsRequest = listObjectsRequest;
        }
        
        public static ListObjectsCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context,
                                                ListObjectsRequest listObjectsRequest)
        {
            return new ListObjectsCommand(client, endpoint, context,
                                          DeserializerFactory.GetFactory().CreateListObjectsResultDeserializer(),
                                          listObjectsRequest);
        }

        private static void Populate(ListObjectsRequest listObjectsRequest, IDictionary<string, string> parameters)
        {
            if (listObjectsRequest.Prefix != null) 
            {
                parameters["prefix"] = listObjectsRequest.Prefix;
            }

            if (listObjectsRequest.Marker != null) 
            {
                parameters["marker"] = listObjectsRequest.Marker;
            }

            if (listObjectsRequest.Delimiter != null) 
            {
                parameters["delimiter"] = listObjectsRequest.Delimiter;
            }

            if (listObjectsRequest.MaxKeys.HasValue) 
            {
                parameters["max-keys"] = listObjectsRequest.MaxKeys.Value.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
