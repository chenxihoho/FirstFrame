/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Diagnostics;
using System.Xml;

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Handlers;
using Aliyun.OpenServices.OpenStorageService.Model;
using Aliyun.OpenServices.OpenStorageService.Transform;

namespace Aliyun.OpenServices.OpenStorageService.Utilities
{
    /// <summary>
    /// Description of ErrorResponseHandler.
    /// </summary>
    internal class ErrorResponseHandler : ResponseHandler
    {
        public override void Handle(ServiceResponse response)
        {
            base.Handle(response);

            if (response.IsSuccessful())
            {
                return;
            }

            ErrorResult errorResult = null;
            try
            {
                var d = DeserializerFactory.GetFactory().CreateErrorResultDeserializer();
                if (d == null)
                    // Re-throw the web exception if the response cannot be parsed.
                    response.EnsureSuccessful();

                errorResult =  d.Deserialize(response);
            }
            catch (XmlException)
            {
                // Re-throw the web exception if the response cannot be parsed.
                response.EnsureSuccessful();
            }
            catch (InvalidOperationException)
            {
                // Re-throw the web exception if the response cannot be parsed.
                response.EnsureSuccessful();
            }

            // This throw must be out of the try block because otherwise
            // the exception would be caught be the following catch.
            Debug.Assert(errorResult != null);
            throw ExceptionFactory.CreateException(errorResult.Code,
                                                   errorResult.Message,
                                                   errorResult.RequestId,
                                                   errorResult.HostId);
        }
    }
}

