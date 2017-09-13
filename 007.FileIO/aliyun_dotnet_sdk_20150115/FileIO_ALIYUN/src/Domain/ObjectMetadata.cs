﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenStorageService.Utilities;

namespace Aliyun.OpenServices.OpenStorageService
{
    /// <summary>
    /// OSS中Object的元数据。
    /// <para>
    /// 包含了用户自定义的元数据，也包含了OSS发送的标准HTTP头(如Content-Length, ETag等）。
    /// </para>
    /// </summary>
    public class ObjectMetadata
    {
        private const string DefaultObjectContentType = "application/octet-stream";
        private readonly IDictionary<string, string> _userMetadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly IDictionary<string, object> _metadata = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public const string AES_256_SERVER_SIDE_ENCRYPTION = "AES256";

        /// <summary>
        /// 获取用户自定义的元数据。
        /// </summary>
        /// <remarks>
        /// OSS内部保存用户自定义的元数据时，会以x-oss-meta-为请求头的前缀。
        /// 但用户通过该接口处理用户自定义元数据里，不需要加上前缀“x-oss-meta-”。
        /// 同时，元数据字典的键名是不区分大小写的，并且在从服务器端返回时会全部以小写形式返回，
        /// 即使在设置时给定了大写字母。比如键名为：MyUserMeta，通过GetObjectMetadata接口
        /// 返回时键名会变为：myusermeta。
        /// </remarks>
        public IDictionary<string, string> UserMetadata
        {
            get { return _userMetadata; }
        }

        /// <summary>
        /// 获取Last-Modified请求头的值，表示Object最后一次修改的时间。
        /// </summary>
        public DateTime LastModified
        {
            get
            {
                return _metadata.ContainsKey(HttpHeaders.LastModified)
                    ? (DateTime)_metadata[HttpHeaders.LastModified] : DateTime.MinValue;
            }
            internal set
            {
                _metadata[HttpHeaders.LastModified] = value;
            }
        }

        /// <summary>
        /// 获取Expires请求头，表示Object的过期时间。
        /// 如果Object没有定义过期时间，则返回null。
        /// </summary>
        public DateTime ExpirationTime
        {
            get
            {
                return _metadata.ContainsKey(HttpHeaders.Expires)
                     ? (DateTime)_metadata[HttpHeaders.Expires] : DateTime.MinValue;
            }
            internal set
            {
                _metadata[HttpHeaders.Expires] = value;
            }
        }

        /// <summary>
        /// 获取Content-Length请求头，表示Object内容的大小。
        /// </summary>
        public long ContentLength
        {
            get
            {
                return _metadata.ContainsKey(HttpHeaders.ContentLength)
                    ? (long)_metadata[HttpHeaders.ContentLength] : 0;
            }
            set
            {
                if (value > OssUtils.MaxFileSize)
                {
                    throw new ArgumentException("content length not allow excess 5GB.");
                }
                _metadata[HttpHeaders.ContentLength] = value;
            }
        }

        /// <summary>
        /// 获取或设置Content-Type请求头，表示Object内容的类型，为标准的MIME类型。
        /// </summary>
        public string ContentType
        {
            get
            {
                return _metadata.ContainsKey(HttpHeaders.ContentType)
                    ? _metadata[HttpHeaders.ContentType] as string : null;
            }
            set
            {
                _metadata[HttpHeaders.ContentType] = value;
            }
        }

        /// <summary>
        /// 获取或设置Content-Encoding请求头，表示Object内容的编码方式。
        /// </summary>
        public string ContentEncoding
        {
            get
            {
                return _metadata.ContainsKey(HttpHeaders.ContentEncoding)
                    ? _metadata[HttpHeaders.ContentEncoding] as string : null;
            }
            set
            {
                _metadata[HttpHeaders.ContentEncoding] = value;
            }
        }

        /// <summary>
        /// 获取或设置Cache-Control请求头，表示用户指定的HTTP请求/回复链的缓存行为。
        /// </summary>
        public string CacheControl
        {
            get
            {
                return _metadata.ContainsKey(HttpHeaders.CacheControl)
                    ? _metadata[HttpHeaders.CacheControl] as string : null;
            }
            set
            {
                _metadata[HttpHeaders.CacheControl] = value;
            }
        }

        /// <summary>
        /// 获取Content-Disposition请求头，表示MIME用户代理如何显示附加的文件。
        /// </summary>
        public string ContentDisposition
        {
            get
            {
                return _metadata.ContainsKey(HttpHeaders.ContentDisposition) 
                    ? _metadata[HttpHeaders.ContentDisposition] as string : null;
            }
            set
            {
                _metadata[HttpHeaders.ContentDisposition] = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值表示与Object相关的hex编码的128位MD5摘要。
        /// </summary>
        public string ETag
        {
            get
            {
                return _metadata.ContainsKey(HttpHeaders.ETag) 
                    ? _metadata[HttpHeaders.ETag] as string : null;
            }
            set
            {
                _metadata[HttpHeaders.ETag] = value;
            }
        }

        public string ServerSideEncryption
        {
            get
            {
                return _metadata.ContainsKey(HttpHeaders.ServerSideEncryption)
                    ? _metadata[HttpHeaders.ServerSideEncryption] as string : null;
            }
            set
            {
                if (AES_256_SERVER_SIDE_ENCRYPTION != value)
                    throw new ArgumentException("Unsupported server side encryption");
                _metadata[HttpHeaders.ServerSideEncryption] = value;
            }
        }

        /// <summary>
        /// 初始化一个新的<see cref="ObjectMetadata" />实例。
        /// </summary>
        public ObjectMetadata()
        {
            ContentLength = -1L;
        }

        public void AddHeader(string key, object value)
        {
            Debug.Assert(!string.IsNullOrEmpty(key));
            _metadata.Add(key, value);
        }

        /// <summary>
        /// Populates the request header dictionary with the metdata and user metadata.
        /// </summary>
        /// <param name="requestHeaders"></param>
        internal void Populate(IDictionary<string, string> requestHeaders)
        {
            Debug.Assert(requestHeaders != null);
            foreach(var entry in _metadata)
            {
                requestHeaders.Add(entry.Key, entry.Value.ToString());
            }
            if (!requestHeaders.ContainsKey(HttpHeaders.ContentType))
            {
                requestHeaders.Add(HttpHeaders.ContentType, DefaultObjectContentType);
            }

            foreach(var entry in _userMetadata)
            {
                requestHeaders.Add(OssHeaders.OssUserMetaPrefix + entry.Key, entry.Value);
            }
        }
    }
}
