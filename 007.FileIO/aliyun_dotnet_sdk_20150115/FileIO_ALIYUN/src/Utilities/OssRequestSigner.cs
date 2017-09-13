/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Diagnostics;
using Aliyun.OpenServices.Common.Authentication;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Utilities;

namespace Aliyun.OpenServices.OpenStorageService.Utilities
{
    /// <summary>
    /// Description of OssRequestSigner.
    /// </summary>
    internal class OssRequestSigner : IRequestSigner
    {
        private readonly string _resourcePath;

        public OssRequestSigner(String resourcePath)
        {
            Debug.Assert(resourcePath != null && resourcePath.StartsWith("/"));
            _resourcePath = resourcePath;
        }
        
        public void Sign(ServiceRequest request, ServiceCredentials credentials)
        {
            Debug.Assert(request != null && credentials != null);

            var accessKeyId = credentials.AccessId;
            var secretAccessKey = credentials.AccessKey;
            var httpMethod = request.Method.ToString().ToUpperInvariant();
            // Because the resource path to is different from the one in the request uri,
            // can't use ServiceRequest.ResourcePath here.
            var resourcePath = _resourcePath;

            Debug.Assert(!string.IsNullOrEmpty(accessKeyId));
            if (!string.IsNullOrEmpty(secretAccessKey))
            {
                var canonicalString = SignUtils.BuildCanonicalString(
                    httpMethod, resourcePath, request/*, null*/);
                var signature = ServiceSignature.Create().ComputeSignature(
                    secretAccessKey, canonicalString);
                request.Headers.Add(HttpHeaders.Authorization, "OSS " + accessKeyId + ":" + signature);
            }
            else
            {
                request.Headers.Add(HttpHeaders.Authorization, accessKeyId);
            }
        }
    }
}
