/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;
using System.IO;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.Properties;

namespace Aliyun.OpenServices.Common.Communication
{
    /// <summary>
    /// Represents the <see cref="IServiceClient"/> implementation
    /// that can automatically retry the request operations if they are failed
    /// due to specific exceptions.
    /// </summary>
    internal class RetryableServiceClient : IServiceClient
    {

        #region Fields & Properties
        private const int _defaultRetryPauseScale = 300; // milliseconds.

        private readonly IServiceClient _innerClient;

        public Func<Exception, bool> ShouldRetryCallback { get; set; }

        /// <summary>
        /// Gets or sets the max retry times on error.
        /// </summary>
        public int MaxErrorRetry { get; set; }

        #endregion

        #region Constructors

        public RetryableServiceClient(IServiceClient innerClient)
        {
            Debug.Assert(innerClient != null);
            _innerClient = innerClient;

            MaxErrorRetry = 3;
        }

        #endregion

        #region IServiceClient Members

        internal IServiceClient InnerServiceClient()
        {
            return _innerClient;
        }

        public ServiceResponse Send(ServiceRequest request, ExecutionContext context)
        {
            return SendImpl(request, context, 0);
        }

        [SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails")]
        private ServiceResponse SendImpl(ServiceRequest request, ExecutionContext context, int retries)
        {          
            long originalContentPosition = -1;
            try
            {
                if (request.Content != null && request.Content.CanSeek)
                {
                    originalContentPosition = request.Content.Position;
                }
                return _innerClient.Send(request, context);
            }
            catch (Exception ex)
            {
                if (ShouldRetry(request, ex, retries))
                {
                    if (request.Content != null && (originalContentPosition >= 0 && request.Content.CanSeek))
                    {
                        request.Content.Seek(originalContentPosition, SeekOrigin.Begin);
                    }
                    Pause(retries);
                    return SendImpl(request, context, ++retries);
                }
                else
                {
                    throw;
                }
            }
        }
        
        public IAsyncResult BeginSend(ServiceRequest request, ExecutionContext context, AsyncCallback callback, object state)
        {
            var asyncResult = new RetryableAsyncResult(callback, state, request, context);
            BeginSendImpl(request, context, asyncResult);
            return asyncResult;
        }

        private void BeginSendImpl(ServiceRequest request, ExecutionContext context, RetryableAsyncResult asyncResult)
        {
            if (asyncResult.InnerAsyncResult != null)
            {
                asyncResult.InnerAsyncResult.Dispose();
            }

            asyncResult.InnerAsyncResult =
                _innerClient.BeginSend(request, context, OnBeginSendCompleted, asyncResult) as AsyncResult;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
                                                         Justification="Catch the exception to dispatch it another thread with async result.")]
        private void OnBeginSendCompleted(IAsyncResult ar)
        {
            var retryableAsyncResult = ar.AsyncState as RetryableAsyncResult;
            try
            {
                // Success
                var result = _innerClient.EndSend(ar);
                retryableAsyncResult.Complete(result);
            }
            catch (Exception ex)
            {
                if (retryableAsyncResult.OriginalContentPosition >= 0)
                {
                    retryableAsyncResult.Request.Content.Seek(retryableAsyncResult.OriginalContentPosition, SeekOrigin.Begin);
                }
                
                if (ShouldRetry(retryableAsyncResult.Request, ex, retryableAsyncResult.Retries))
                {
                    // Retry
                    Pause(retryableAsyncResult.Retries++);
                    BeginSendImpl(retryableAsyncResult.Request,
                                  retryableAsyncResult.Context,
                                  retryableAsyncResult);
                }
                else
                {
                    retryableAsyncResult.Complete(ex);
                }
            }
        }

        public ServiceResponse EndSend(IAsyncResult ar)
        {
            var retryableAsyncResult = ar as RetryableAsyncResult;
            Debug.Assert(ar != null);

            try
            {
                var result = retryableAsyncResult.GetResult();
                retryableAsyncResult.Dispose();

                return result;
            }
            catch (ObjectDisposedException)
            {
                throw new InvalidOperationException(Resources.ExceptionEndOperationHasBeenCalled);
            }
        }

        private bool ShouldRetry(ServiceRequest request, Exception ex, int retries)
        {
            if (retries > MaxErrorRetry)
            {
                return false;
            }
            
            if (!request.IsRepeatable)
            {
                return false;
            }

            var webException = ex as WebException;
            if (webException != null)
            {
                var httpWebResponse = webException.Response as HttpWebResponse;
                if (httpWebResponse != null &&
                    (httpWebResponse.StatusCode == HttpStatusCode.ServiceUnavailable ||
                     httpWebResponse.StatusCode == HttpStatusCode.InternalServerError))
                {
                    return true;
                }
            }
            
            if (ShouldRetryCallback != null && ShouldRetryCallback(ex))
            {
                return true;
            }

            return false;
        }

        private static void Pause(int retries)
        {
            // make the pause time increase exponentially
            // based on an assumption that the more times it retries,
            // the less probability it succeeds.
            var scale = _defaultRetryPauseScale;
            var delay = (int)Math.Pow(2, retries) * scale;

            Thread.Sleep(delay);
        }

        #endregion

    }

    internal class RetryableAsyncResult : AsyncResult<ServiceResponse>
    {
        public ServiceRequest Request { get; private set; }
        
        public ExecutionContext Context { get; private set; }

        public AsyncResult InnerAsyncResult { get; set; }

        public int Retries { get; set; }
        
        public long OriginalContentPosition { get; set; }

        public RetryableAsyncResult(AsyncCallback callback, object state,
                                    ServiceRequest request, ExecutionContext context)
            : base(callback, state)
        {
            Debug.Assert(request != null);
            Request = request;
            Context = context;
            OriginalContentPosition = (request.Content != null && request.Content.CanSeek)
                ? request.Content.Position : -1;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing && InnerAsyncResult != null)
            {
                InnerAsyncResult.Dispose();
                InnerAsyncResult = null;
            }
        }
    }
}


