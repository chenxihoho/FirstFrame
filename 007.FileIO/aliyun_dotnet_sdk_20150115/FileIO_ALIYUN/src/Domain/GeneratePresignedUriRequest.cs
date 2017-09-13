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
    /// 指定生成URL预签名的请求参数.
    /// </summary>
    public class GeneratePresignedUriRequest
    {
        private SignHttpMethod _method;
        private IDictionary<string, string> _userMetadata = new Dictionary<string, string>();
        private IDictionary<string, string> _queryParams = new Dictionary<string, string>();
        private ResponseHeaderOverrides _responseHeader = new ResponseHeaderOverrides();

        /// <summary>
        /// 获取或者设置HttpMethod。
        /// </summary>
        public SignHttpMethod Method
        {
            get { return _method; }
            set
            {
                if (_method != SignHttpMethod.Get && _method != SignHttpMethod.Put)
                    throw new ArgumentException("Only supports Get & Put method.");
                _method = value;
            }
        }

        /// <summary>
        /// 获取或者设置Object所在Bucket的名称。
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// 获取或者设置Object的名称。
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 获取或设置签名URL对应的文件类型。
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 获取或设置签名URL对应的文件MD5。
        /// </summary>
        public string ContentMd5 { get; set; }

        /// <summary>
        /// 获取或者设置过期时间
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// 获取或者设置要重载的返回请求头。
        /// </summary>
        public ResponseHeaderOverrides ResponseHeaders
        {
            get { return _responseHeader; }
            set
            {
                if (value == null)
                    throw new ArgumentException("ResponseHeaderOverrides should not be null");
                _responseHeader = value;
            }
        }
        
        /// <summary>
        /// 获取或者设置用户自定义的元数据，表示以x-oss-meta-为前缀的请求头。
        /// </summary>
        public IDictionary<string, string> UserMetadata 
        {
            get { return _userMetadata; }
            set
            {
                if (value == null)
                    throw new ArgumentException("UserMetadata should not be null");
                _userMetadata = value;
            }
        }

        /// <summary>
        /// 获取或者设置用户请求参数。
        /// </summary>
        public IDictionary<string, string> QueryParams
        {
            get { return _queryParams; }
            set
            {
                if (value == null)
                    throw new ArgumentException("QueryParams should not be null");
                _queryParams = value;
            }
        }

        /// <summary>
        /// 添加用户Meta项。
        /// </summary>
        public void AddUserMetadata(string metaItem, string value)
        {
            _userMetadata.Add(metaItem, value);
        }

        /// <summary>
        /// 添加用户参数。
        /// </summary>
        public void AddQueryParam(string param, string value)
        {
            _queryParams.Add(param, value);
        }

        public GeneratePresignedUriRequest(string bucketName, string key)
            : this(bucketName, key, SignHttpMethod.Get)
        {
        }

        public GeneratePresignedUriRequest(string bucketName, string key, SignHttpMethod httpMethod)
        {
            if (string.IsNullOrEmpty(bucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            if (!OssUtils.IsBucketNameValid(bucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
            if (!OssUtils.IsObjectKeyValid(key))
                    throw new ArgumentException(OssResources.ObjectKeyInvalid, "key");
            
            BucketName = bucketName;
            Key = key;
            Method = httpMethod;
            // Default expiration(15 minutes from now) for generated url.
            Expiration = DateTime.Now.AddMinutes(15);
        }
        
    }
}
