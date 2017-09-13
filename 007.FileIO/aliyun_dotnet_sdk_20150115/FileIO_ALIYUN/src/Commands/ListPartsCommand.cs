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
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.Properties;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.OpenStorageService.Transform;
using Aliyun.OpenServices.Domain;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    /// <summary>
    /// Description of ListPartsCommands.
    /// </summary>
    internal class ListPartsCommand : OssCommand<PartListing>
    {
        private readonly ListPartsRequest _listPartsRequest;
        
        protected override HttpMethod Method
        {
            get { return HttpMethod.Get; }
        }

        protected override string Bucket
        {
            get
            {
                return _listPartsRequest.BucketName;
            }
        } 
        
        protected override string Key
        {
            get
            {
                return _listPartsRequest.Key;
            }
        } 
        
        protected override IDictionary<string, string> Parameters
        {
            get {
                var parameters = base.Parameters;
                Populate(_listPartsRequest, parameters);
                return parameters;
            }
        }
        
        private static void Populate(ListPartsRequest listPartsRequst, IDictionary<string, string> parameters)
        {
            parameters["uploadId"] = listPartsRequst.UploadId;
            if (listPartsRequst.MaxParts != null)
            {
                parameters["max-parts"] = listPartsRequst.MaxParts.ToString();
            }
            
            if (listPartsRequst.PartNumberMarker != null)
            {
                parameters["part-number-marker"] = listPartsRequst.PartNumberMarker.ToString();
            }
        }
        
        private ListPartsCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                 IDeserializer<ServiceResponse, PartListing> deserializeMethod,
                                ListPartsRequest listPartsRequest)
            : base(client, endpoint, context, deserializeMethod)
        {
            Debug.Assert(listPartsRequest != null);
            
            if (string.IsNullOrEmpty(listPartsRequest.BucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            if (string.IsNullOrEmpty(listPartsRequest.Key))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
            if (string.IsNullOrEmpty(listPartsRequest.UploadId))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "uploadId");
            
            if (!OssUtils.IsBucketNameValid(listPartsRequest.BucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
            if (!OssUtils.IsObjectKeyValid(listPartsRequest.Key))
                throw new ArgumentException(OssResources.ObjectKeyInvalid, "key");
            
            _listPartsRequest = listPartsRequest;
        }
        
        public static ListPartsCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context,
                                                ListPartsRequest listPartsRequest)
        {
            return new ListPartsCommand(client, endpoint,context, 
                                                   DeserializerFactory.GetFactory().CreateListPartsResultDeserializer(),
                                                   listPartsRequest);
        }
    }
}
