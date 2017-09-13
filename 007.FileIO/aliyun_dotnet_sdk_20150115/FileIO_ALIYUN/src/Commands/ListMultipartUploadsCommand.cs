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
using Aliyun.OpenServices.Properties;
using Aliyun.OpenServices.OpenStorageService.Transform;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    /// <summary>
    /// Description of ListMultipartUploadsCommand.
    /// </summary>
    internal class ListMultipartUploadsCommand : OssCommand<MultipartUploadListing>
    {
        private readonly ListMultipartUploadsRequest _listMultipartUploadsRequest;

        protected override HttpMethod Method
        {
            get { return HttpMethod.Get; }
        }

        protected override string Bucket
        {
            get
            {
                return _listMultipartUploadsRequest.BucketName;
            }
        }
        
        protected override IDictionary<string, string> Parameters
        {
            get 
            {
                var parameters = base.Parameters;
                Populate(_listMultipartUploadsRequest, parameters);
                return parameters;
            }
        }
        
        private ListMultipartUploadsCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                 IDeserializer<ServiceResponse, MultipartUploadListing> deserializeMethod,
                                ListMultipartUploadsRequest listMultipartUploadsRequest)
            : base(client, endpoint, context, deserializeMethod)
        {
            Debug.Assert(listMultipartUploadsRequest != null);
            
            if (string.IsNullOrEmpty(listMultipartUploadsRequest.BucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            
            _listMultipartUploadsRequest = listMultipartUploadsRequest;
        }
        
        public static ListMultipartUploadsCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context,
                                                ListMultipartUploadsRequest listMultipartUploadsRequest)
        {
            if (listMultipartUploadsRequest == null)
                throw new ArgumentNullException("listMultipartUploadsRequest");
            return new ListMultipartUploadsCommand(client, endpoint,context, 
                                                   DeserializerFactory.GetFactory().CreateListMultipartUploadsResultDeserializer(),
                                                   listMultipartUploadsRequest);
        }
        
        private static void Populate(ListMultipartUploadsRequest listMultipartUploadsRequest, IDictionary<string, string> parameters)
        {
            parameters["uploads"] = null;
            if (listMultipartUploadsRequest.Delimiter != null)
            {
                parameters["delimiter"] = listMultipartUploadsRequest.Delimiter;
            }
            
            if (listMultipartUploadsRequest.KeyMarker != null)
            {
                parameters["key-marker"] = listMultipartUploadsRequest.KeyMarker;
            }
            
            if (listMultipartUploadsRequest.MaxUploads.HasValue)
            {
                parameters["max-uploads"] = listMultipartUploadsRequest.MaxUploads.Value.ToString(CultureInfo.InvariantCulture);;
            }
            
            if (listMultipartUploadsRequest.Prefix != null)
            {
                parameters["prefix"] = listMultipartUploadsRequest.Prefix;
            }
            
            if (listMultipartUploadsRequest.UploadIdMarker != null)
            {
                parameters["upload-id-marker"] = listMultipartUploadsRequest.UploadIdMarker;
            }
        }
        
    }
}
