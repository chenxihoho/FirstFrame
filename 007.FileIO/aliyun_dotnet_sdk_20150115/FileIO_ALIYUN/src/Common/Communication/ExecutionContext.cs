/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.Collections.Generic;
using Aliyun.OpenServices.Common.Authentication;
using Aliyun.OpenServices.Common.Handlers;
using Aliyun.OpenServices.OpenStorageService;

namespace Aliyun.OpenServices.Common.Communication
{
    /// <summary>
    /// Description of ExecutionContext.
    /// </summary>
    internal class ExecutionContext
    {
        /// <summary>
        /// The default encoding (charset name).
        /// </summary>
        private const string DefaultEncoding = "utf-8";
        
        private readonly IList<IResponseHandler> _responseHandlers = new List<IResponseHandler>();

        /// <summary>
        /// Gets or sets the charset.
        /// </summary>
        public string Charset { get; set; }

        /// <summary>
        /// Gets or sets the request signer.
        /// </summary>
        public IRequestSigner Signer { get; set; }
        
        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        public ServiceCredentials Credentials { get; set ;}
        
        /// <summary>
        /// Gets the list of <see cref="IResponseHandler" />.
        /// </summary>
        public IList<IResponseHandler> ResponseHandlers
        {
            get { return _responseHandlers; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExecutionContext()
        {
            Charset = DefaultEncoding;
        }

    }
}
