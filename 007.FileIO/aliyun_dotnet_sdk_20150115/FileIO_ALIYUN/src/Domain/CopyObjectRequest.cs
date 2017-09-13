﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */
 
using System;
using System.Collections.Generic;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.Properties;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Domain;

namespace Aliyun.OpenServices.OpenStorageService
{
    /// <summary>
    /// 指定拷贝Object的请求参数
    /// </summary>
    public class CopyObjectRequest
    {
        private IList<string> _matchingETagConstraints = new List<string>();
        private IList<string> _nonmatchingETagConstraints = new List<string>();

        /// <summary>
        /// 获取或者设置源Object所在的Bucket的名称。
        /// </summary>
        public string SourceBucketName { get; set; }
        
        /// <summary>
        /// 获取或者设置源Object的Key。
        /// </summary>
        public string SourceKey { get; set; }
        
        /// <summary>
        /// 获取或者设置目标Object所在的Bucket的名称。
        /// </summary>
        public string DestinationBucketName { get; set; }
        
        /// <summary>
        /// 获取或者设置目标Object的Key。
        /// </summary>
        public string DestinationKey { get; set; }
        
        /// <summary>
        /// 获取或者设置目标Object的Metadata信息。
        /// </summary>
        public ObjectMetadata NewObjectMetaData { get; set; }

        /// <summary>
        /// 如果源Object的ETAG值和用户提供的ETAG相等，则执行拷贝操作；否则返回412 HTTP错误码（预处理失败）。
        /// </summary>        
        public IList<string> MatchingETagConstraints
        {
            get { return _matchingETagConstraints; }
        }

        /// <summary>
        /// 如果源Object的ETAG值和用户提供的ETAG不相等，则执行拷贝操作；否则返回412 HTTP错误码（预处理失败）。
        /// </summary>       
        public IList<string> NonmatchingETagConstraints
        {
            get { return _nonmatchingETagConstraints; }
        }

        /// <summary>
        /// 如果传入参数中的时间等于或者晚于文件实际修改时间，则正常传输文件，并返回200 OK；
        /// 否则返回412 precondition failed错误
        /// </summary>
        public DateTime? UnmodifiedSinceConstraint { get; set; }

        /// <summary>
        /// 如果源Object自从用户指定的时间以后被修改过，则执行拷贝操作；
        /// 否则返回412 HTTP错误码（预处理失败）。    
        /// </summary>   
        public DateTime? ModifiedSinceConstraint { get; set; }
        
        /// <summary>
        /// 构造一个新的<see cref="CopyObjectRequest" /> 实例
        /// </summary>
        /// <param name="sourceBucketName">需要拷贝的<see cref="OssObject" />所在的Bucket</param>
        /// <param name="sourceKey">需要拷贝的<see cref="OssObject" />名称</param>
        /// <param name="destinationBucketName">要拷贝到的目的<see cref="OssObject" />所在的Bucket</param>
        /// <param name="destinationKey">要拷贝到的目的<see cref="OssObject" />的名称</param>
        public CopyObjectRequest(string sourceBucketName, string sourceKey,
            string destinationBucketName, string destinationKey)
        {
            if (string.IsNullOrEmpty(sourceBucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "sourceBucketName");
            if (string.IsNullOrEmpty(sourceKey))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "sourceKey");
            
            if (string.IsNullOrEmpty(destinationBucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "destinationBucketName");
            if (string.IsNullOrEmpty(destinationKey))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "destinationKey");
            
            if (!OssUtils.IsBucketNameValid(sourceBucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "sourceBucketName");
            if (!OssUtils.IsObjectKeyValid(sourceKey))
                throw new ArgumentException(OssResources.ObjectKeyInvalid, "sourceKey");
            
            if (!OssUtils.IsBucketNameValid(destinationBucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "destinationBucketName");
            if (!OssUtils.IsObjectKeyValid(destinationKey))
                throw new ArgumentException(OssResources.ObjectKeyInvalid, "destinationKey");
            
            SourceBucketName = sourceBucketName;
            SourceKey = sourceKey;
            DestinationBucketName = destinationBucketName;
            DestinationKey = destinationKey;
   
        }
        
        internal void Populate(IDictionary<string, string> headers)
        {           
            var sourceHeader = "/" + SourceBucketName + "/" + SourceKey;

            headers.Add(OssHeaders.CopyObjectSource, sourceHeader);

            if (ModifiedSinceConstraint != null)
            {
                headers.Add(OssHeaders.CopySourceIfModifedSince, DateUtils.FormatRfc822Date(ModifiedSinceConstraint.Value));
            }

            if (UnmodifiedSinceConstraint != null)
            {
                headers.Add(OssHeaders.CopySourceIfUnmodifiedSince, DateUtils.FormatRfc822Date(UnmodifiedSinceConstraint.Value));
            }

            if (_matchingETagConstraints.Count > 0)
            {
                headers.Add(OssHeaders.CopySourceIfMatch, OssUtils.JoinETag(_matchingETagConstraints));
            }

            if (_nonmatchingETagConstraints.Count > 0)
            {
                headers.Add(OssHeaders.CopySourceIfNoneMatch, OssUtils.JoinETag(_nonmatchingETagConstraints));
            }
            
            if (NewObjectMetaData != null)
            {
                headers.Add(OssHeaders.CopyObjectMetaDataDirective, "REPLACE");
                NewObjectMetaData.Populate(headers);
            }
            
            // Remove Content-Length
            // ObjectMeta#Populate will create ContentLength header, 
            // but we do not need it for the request body is empty.
            headers.Remove(HttpHeaders.ContentLength);
        }
    }
}
