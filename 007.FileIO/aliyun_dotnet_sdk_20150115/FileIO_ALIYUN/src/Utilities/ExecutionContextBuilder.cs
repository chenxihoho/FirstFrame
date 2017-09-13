/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.Collections.Generic;
using Aliyun.OpenServices.Common.Authentication;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Handlers;

namespace Aliyun.OpenServices.OpenStorageService.Utilities
{
    internal class ExecutionContextBuilder
    {
        public ServiceCredentials Credentials { get; set; }
        
        public IList<IResponseHandler> ResponseHandlers { get; private set; }
        
        public HttpMethod Method { get; set; }
        
        public string Bucket { get; set; }
        
        public string Key { get; set; }
        
        public ExecutionContextBuilder()
        {
            ResponseHandlers = new List<IResponseHandler>();
        }
        
        public ExecutionContext Build()
        {
            var context = new ExecutionContext();
            context.Signer = CreateSigner(Bucket, Key);
            context.Credentials = Credentials;
            foreach(var h in ResponseHandlers)
            {
                context.ResponseHandlers.Add(h);
            }
            return context;
        }
        
        private static IRequestSigner CreateSigner(string bucket, string key){
            var resourcePath = "/" +
                ((bucket != null) ? bucket : "") +
                ((key != null ? "/" + key : ""));
            
            // Hacked. the sign path is /bucket/key for two-level-domain mode
            // but /bucket/key/ for the three-level-domain mode.
            if (bucket != null && key == null)
            {
                resourcePath = resourcePath + "/";
            }
            
            return new OssRequestSigner(resourcePath);
        }
    }
}
