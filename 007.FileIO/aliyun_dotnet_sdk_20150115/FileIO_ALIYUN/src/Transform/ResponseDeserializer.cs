/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.IO;

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    /// <summary>
    /// Description of ResponseDeserializer.
    /// </summary>
    internal abstract class ResponseDeserializer<TResult, TModel> : IDeserializer<ServiceResponse, TResult>
    {
        protected IDeserializer<Stream, TModel> ContentDeserializer { get; private set; }

        public ResponseDeserializer(IDeserializer<Stream, TModel> contentDeserializer)
        {
            ContentDeserializer = contentDeserializer;
        }

        public abstract TResult Deserialize(ServiceResponse response);
    }
}
