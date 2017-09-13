/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */
 
using System;
using System.Collections.Generic;
using Aliyun.OpenServices.Properties;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Domain;

namespace Aliyun.OpenServices.OpenStorageService
{
    /// <summary>
    /// 获取List Multipart Upload的请求结果.
    /// </summary>
    public class MultipartUploadListing
    {
        
        private readonly IList<MultipartUpload> _multipartUploads = new List<MultipartUpload>();
        private readonly IList<string> _commonPrefixes = new List<string>();
        /// <summary>
        /// 获取Object所在的<see cref="Bucket" />的名称。
        /// </summary>
        public string BucketName { get; internal set; }
        
        /// <summary>
        /// 获取请求参数<see cref="P:ListMultipartUploadsRequest.KeyMarker" />的值。
        /// </summary>
        public string KeyMarker { get; internal set; }
        
        /// <summary>
        /// 获取请求参数<see cref="P:ListMultipartUploadsRequest.Delimiter" />的值。
        /// </summary>
        public string Delimiter { get; internal set; }
        
        /// <summary>
        /// 获取请求参数<see cref="P:ListMultipartUploadsRequest.Prefix" />的值。
        /// </summary>
        public string Prefix { get; internal set; }
        
        /// <summary>
        /// 获取请求参数<see cref="P:ListMultipartUploadsRequest.UploadIdMarker" />的值。
        /// </summary>
        public string UploadIdMarker { get; internal set; }
        
        /// <summary>
        /// 获取请求参数<see cref="P:ListMultipartUploadsRequest.MaxUploads" />的值。
        /// </summary>
        public int MaxUploads { get; internal set; }
        
        /// <summary>
        /// 标明是否本次返回的Multipart Upload结果列表被截断。
        /// “true”表示本次没有返回全部结果；
        /// “false”表示本次已经返回了全部结果。
        /// </summary>
        public bool IsTruncated { get; internal set; }
        
        /// <summary>
        /// 列表的起始Object位置。
        /// </summary>
        public string NextKeyMarker { get; internal set; }
        
        /// <summary>
        /// 如果本次没有返回全部结果，响应请求中将包含NextUploadMarker元素，用于标明接下来请求的UploadMarker值。
        /// </summary>
        public string NextUploadIdMarker { get; internal set; }
        
        /// <summary>
        /// 所有Multipart Upload事件
        /// </summary>
        public IEnumerable<MultipartUpload> MultipartUploads
        {
            get { return _multipartUploads; }
        }
        
        /// <summary>
        /// 获取返回结果中的CommonPrefixes部分。
        /// </summary>
        public IEnumerable<string> CommonPrefixes
        {
            get { return _commonPrefixes; }
        }

        public MultipartUploadListing(string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            if (!OssUtils.IsBucketNameValid(bucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");

            BucketName = bucketName;
        }
        
        internal void AddMultipartUpload(MultipartUpload multipartUpload)
        {
            _multipartUploads.Add(multipartUpload);
        }
        
        internal void AddCommonPrefix(string prefix)
        {
            _commonPrefixes.Add(prefix);
        }
    }
}
