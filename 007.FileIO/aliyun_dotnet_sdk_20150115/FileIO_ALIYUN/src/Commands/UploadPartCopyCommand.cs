/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */
 
using System;
using System.Diagnostics;
using System.Collections.Generic;

using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenStorageService.Transform;
using Aliyun.OpenServices.OpenStorageService.Utilities;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
    /// <summary>
    /// Description of UploadPartCopyCommand.
    /// </summary>
    internal class UploadPartCopyCommand : OssCommand<UploadPartCopyResult>
    {
        private readonly UploadPartCopyRequest _uploadPartCopyRequest;
        
        protected override HttpMethod Method
        {
            get { return HttpMethod.Put; }
        }

        protected override string Bucket
        {
            get { return _uploadPartCopyRequest.TargetBucket; }
        } 
        
        protected override string Key
        {
            get { return _uploadPartCopyRequest.TargetKey; }
        } 
        
        protected override IDictionary<string, string> Parameters
        {
            get {
                var parameters = base.Parameters;
                parameters["partNumber"] = _uploadPartCopyRequest.PartNumber.ToString();
                parameters["uploadId"] = _uploadPartCopyRequest.UploadId;
                return parameters;
            }
        }
        
        protected override IDictionary<string, string> Headers
        {
            get
            {
                var headers = new Dictionary<string, string>();
                
                headers[HttpHeaders.ContentLength] = _uploadPartCopyRequest.PartSize.ToString();
                if (!string.IsNullOrEmpty(_uploadPartCopyRequest.Md5Digest))
                    headers[HttpHeaders.ContentMd5] = _uploadPartCopyRequest.Md5Digest;
                
                headers[HttpHeaders.CopySource] = OssUtils.BuildPartCopySource(_uploadPartCopyRequest.SourceBucket,
                    _uploadPartCopyRequest.SourceKey);
                headers[HttpHeaders.CopySourceRange] = "bytes=" + _uploadPartCopyRequest.BeginIndex.ToString()
                    + "-" + (_uploadPartCopyRequest.BeginIndex + _uploadPartCopyRequest.PartSize - 1).ToString();

                if (_uploadPartCopyRequest.MatchingETagConstraints.Count > 0)
                    headers[OssHeaders.CopySourceIfMatch] =
                        OssUtils.JoinETag(_uploadPartCopyRequest.MatchingETagConstraints);
                if (_uploadPartCopyRequest.NonmatchingETagConstraints.Count > 0)
                    headers[OssHeaders.CopySourceIfNoneMatch] =
                        OssUtils.JoinETag(_uploadPartCopyRequest.NonmatchingETagConstraints);
                if (_uploadPartCopyRequest.ModifiedSinceConstraint != null)
                { 
                    headers[OssHeaders.CopySourceIfModifedSince] 
                        = DateUtils.FormatRfc822Date(_uploadPartCopyRequest.ModifiedSinceConstraint.Value);
                }
                if (_uploadPartCopyRequest.UnmodifiedSinceConstraint != null)
                { 
                    headers[OssHeaders.CopySourceIfUnmodifiedSince]
                        = DateUtils.FormatRfc822Date(_uploadPartCopyRequest.UnmodifiedSinceConstraint.Value);
                }
                return headers;
            }
        }
        
        protected override bool LeaveRequestOpen
        {
            get { return true; }
        }

        private UploadPartCopyCommand(IServiceClient client, Uri endpoint, ExecutionContext context,
                                 IDeserializer<ServiceResponse, UploadPartCopyResult> deserializer,                                  
                                                UploadPartCopyRequest uploadPartCopyRequest)
            : base(client, endpoint, context, deserializer)
        {
            Debug.Assert(uploadPartCopyRequest != null);
            _uploadPartCopyRequest = uploadPartCopyRequest;
        }
        
        private static bool IsPartNumberInRange(int? partNumber)
        {
            return (partNumber > 0 && partNumber <= 10000);
        }
        
        public static UploadPartCopyCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context,
                                                 UploadPartCopyRequest uploadPartCopyRequest)
        {
            if (uploadPartCopyRequest == null)
                throw new ArgumentNullException("uploadPartCopyRequest");

            if (uploadPartCopyRequest.PartNumber == null)
                throw new ArgumentNullException("partNumber");
            if (uploadPartCopyRequest.PartSize == null)
                throw new ArgumentNullException("partSize");
            if (uploadPartCopyRequest.BeginIndex == null)
                throw new ArgumentNullException("beginIndex");
            
            if (uploadPartCopyRequest.PartSize < 0 || uploadPartCopyRequest.PartSize > OssUtils.MaxFileSize)
                throw new ArgumentOutOfRangeException("partSize");
            if (!IsPartNumberInRange(uploadPartCopyRequest.PartNumber))
                throw new ArgumentOutOfRangeException("partNumber");

            return new UploadPartCopyCommand(client, endpoint, context, 
                                         DeserializerFactory.GetFactory().CreateUploadPartCopyResultDeserializer((int)uploadPartCopyRequest.PartNumber),
                                         uploadPartCopyRequest);
        }
    }
}
