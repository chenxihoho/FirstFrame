/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.Diagnostics;
using Aliyun.OpenServices.Common.Communication;

namespace Aliyun.OpenServices.Common.Handlers
{
    /// <summary>
    /// Description of ResponseHandler.
    /// </summary>
    internal class ResponseHandler : IResponseHandler
    {
        public virtual void Handle(ServiceResponse response)
        {
            Debug.Assert(response != null);
        }
    }
}
