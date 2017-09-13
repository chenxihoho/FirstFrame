/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenStorageService.Utilities;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    /// <summary>
    /// Base class for OSS Commands.
    /// </summary>
    internal abstract class OssCommand
    {
        public ExecutionContext Context { get; private set; }

        public IServiceClient Client { get; private set; }

        public Uri Endpoint { get; private set; }
        
        protected virtual bool LeaveRequestOpen { get { return false; } }

        protected virtual HttpMethod Method
        {
            get { return HttpMethod.Get; }
        }
        
        protected virtual String Bucket
        { 
            get { return null; }
        }
        
        protected virtual String Key 
        { 
            get { return null; }
        }
        
        protected virtual IDictionary<String, String> Headers
        {
            get { return new Dictionary<String, String>(); }
        }
        
        protected virtual IDictionary<String, String> Parameters
        {
            get { return new Dictionary<String, String>(); }
        }
        
        protected virtual Stream Content
        {
            get { return null; }
        }

        protected OssCommand(IServiceClient client, Uri ossEndpoint, ExecutionContext context)
        {
            Debug.Assert(client != null && ossEndpoint != null && context != null);
            Endpoint = ossEndpoint;
            Client = client;
            Context = context;
        }
        
        public ServiceResponse Execute()
        {
            // The response should be disposed.
            var request = BuildRequest();
            try {
                return Client.Send(request, Context);
            } finally {
                if (!LeaveRequestOpen) {
                    request.Dispose();
                }
            }
        }
        
        private ServiceRequest BuildRequest()
        {
            var request = new ServiceRequest();
            request.Method = Method;
            var cc = OssUtils.GetClientConfiguration(Client);
            request.Endpoint = OssUtils.MakeBucketEndpoint(Endpoint, Bucket, cc);
            request.ResourcePath = OssUtils.MakeResourcePath(Key);
            foreach(var p in Parameters)
            {
                request.Parameters.Add(p.Key, p.Value);
            }
            // Put Date in the header
            request.Headers[HttpHeaders.Date] = DateUtils.FormatRfc822Date(DateTime.UtcNow);
            if (!Headers.ContainsKey(HttpHeaders.ContentType))
            {
                request.Headers[HttpHeaders.ContentType] = string.Empty;
            }
            foreach(var h in Headers)
            {
                request.Headers.Add(h.Key, h.Value);
            }
            request.Content = Content;
            return request;
        }
    }
    
    internal abstract class OssCommand<T> : OssCommand
    {
        readonly IDeserializer<ServiceResponse, T> _deserializer;

        protected virtual bool LeaveResponseOpen { get { return false; } }

        public OssCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                          IDeserializer<ServiceResponse, T> deserializer)
            : base(client, endpoint, context)
        {
            Debug.Assert(deserializer != null);
            _deserializer = deserializer;
        }

        public new T Execute()
        {
            var response = base.Execute();
            try
            {
                return _deserializer.Deserialize(response);
            }
            catch(ResponseDeserializationException ex)
            {
                throw ExceptionFactory.CreateInvalidResponseException(ex);
            }
            finally
            {
                if (!LeaveResponseOpen)
                {
                    response.Dispose();
                }
            }
        }

    }
}
