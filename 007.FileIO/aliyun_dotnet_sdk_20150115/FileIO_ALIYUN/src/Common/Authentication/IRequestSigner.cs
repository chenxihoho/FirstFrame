/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenStorageService;

namespace Aliyun.OpenServices.Common.Authentication
{
    /// <summary>
    /// Description of IRequestSigner.
    /// </summary>
    internal interface IRequestSigner
    {
        /// <summary>
        /// Signs a request.
        /// </summary>
        /// <param name="request">The request to sign.</param>
        /// <param name="credentials">The credentials used to sign.</param>
        void Sign(ServiceRequest request, ServiceCredentials credentials);
    }
}
