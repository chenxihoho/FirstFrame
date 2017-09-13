/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.IO;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Model;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{

    /// <summary>
    /// Description of DeserializerFactory.
    /// </summary>
    internal abstract class SerializerFactory
    {
        public static SerializerFactory GetFactory()
        {
            return GetFactory(null);
        }

        public static SerializerFactory GetFactory(string contentType)
        {
            // Use XML for default.
            if (contentType == null)
            {
                contentType = "text/xml";
            }

            if (contentType.Contains("xml"))
            {
                return new XmlSerializerFactory();
            }

            return null;
        }

        protected abstract ISerializer<T, Stream> CreateContentSerializer<T>();

        public ISerializer<CompleteMultipartUploadRequest, Stream> CreateCompleteUploadRequestSerializer()
        {
            return new CompleteMultipartUploadRequestSerializer(CreateContentSerializer<CompleteMultipartUploadRequestModel>());
        }

        public ISerializer<SetBucketLoggingRequest, Stream> CreateSetBucketLoggingRequestSerializer()
        {
            return new SetBucketLoggingRequestSerializer(CreateContentSerializer<SetBucketLoggingRequestModel>());
        }

        public ISerializer<SetBucketWebsiteRequest, Stream> CreateSetBucketWebsiteRequestSerializer()
        {
            return new SetBucketWebsiteRequestSerializer(CreateContentSerializer<SetBucketWebsiteRequestModel>());
        }

        public ISerializer<SetBucketCorsRequest, Stream> CreateSetBucketCorsRequestSerializer()
        {
            return new SetBucketCorsRequestSerializer(CreateContentSerializer<SetBucketCorsRequestModel>());
        }

        public ISerializer<DeleteObjectsRequest, Stream> CreateDeleteObjectsRequestSerializer()
        {
            return new DeleteObjectsRequestSerializer(CreateContentSerializer<DeleteObjectsRequestModel>());
        }

        public ISerializer<SetBucketRefererRequest, Stream> CreateSetBucketRefererRequestSerializer()
        {
            return new SetBucketRefererRequestSerializer(CreateContentSerializer<RefererConfiguration>());
        }
    }

    internal class XmlSerializerFactory : SerializerFactory
    {
        protected override ISerializer<T, Stream> CreateContentSerializer<T>()
        {
            return new XmlStreamSerializer<T>();
        }
    }
}
