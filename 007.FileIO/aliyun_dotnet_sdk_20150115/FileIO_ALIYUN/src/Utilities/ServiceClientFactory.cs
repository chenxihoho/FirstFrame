/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.Diagnostics;
using System.Net;
using Aliyun.OpenServices.Common.Communication;

namespace Aliyun.OpenServices.OpenStorageService.Utilities
{
    /// <summary>
    /// Description of ServiceClientFactory.
    /// </summary>
    internal static class ServiceClientFactory
    {
        static ServiceClientFactory()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.DefaultConnectionLimit = ClientConfiguration.ConnectionLimit;
        }

        public static IServiceClient CreateServiceClient(ClientConfiguration configuration)
        {
            Debug.Assert(configuration != null);

            var retryableServiceClient =
                new RetryableServiceClient(ServiceClient.Create(configuration))
                {
                    MaxErrorRetry = configuration.MaxErrorRetry
                };

            return retryableServiceClient;
        }
    }
}
