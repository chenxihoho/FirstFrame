/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Runtime.Serialization;

namespace Aliyun.OpenServices.Common.Transform
{
    /// <summary>
    /// Exception thrown during deserializing the response.
    /// </summary>
    [Serializable]
    internal class ResponseDeserializationException : InvalidOperationException, ISerializable
    {
        public ResponseDeserializationException()
        {
        }

         public ResponseDeserializationException(string message) : base(message)
        {
        }

        public ResponseDeserializationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        // This constructor is needed for serialization.
        protected ResponseDeserializationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}