﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Globalization;
using System.IO;
using Aliyun.OpenServices.Properties;

namespace Aliyun.OpenServices.OpenStorageService
{
    /// <summary>
    /// 表示OSS中的Object。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 在 OSS 中，用户的每个文件都是一个 Object，每个文件需小于 5G。
    /// Object包含key、data和user meta。其中，key是Object 的名字；
    /// data是Object 的数据；user meta是用户对该object的描述。
    /// </para>
    /// </remarks>
    public class OssObject : IDisposable
    {
        private bool _disposed;
        private string _key;
        private ObjectMetadata _metadata = new ObjectMetadata();

        /// <summary>
        /// 获取或设置Object的Key。
        /// </summary>
        public string Key
        {
            get { return _key; }
            internal  set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "value");

                _key = value;
            }
        }

        /// <summary>
        /// 获取或设置Object所在<see cref="Bucket" />的名称。
        /// </summary>
        public string BucketName { get; internal set; }

        /// <summary>
        /// 获取Object的元数据。
        /// </summary>
        public ObjectMetadata Metadata
        {
            get { return _metadata; }
            internal set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _metadata = value;
            }
        }

        /// <summary>
        /// 获取或设置Object内容的数据流。
        /// </summary>
        public Stream Content { get; internal set; }

        /// <summary>
        /// 构造一个新的<see cref="OssObject" />实例。
        /// </summary>
        internal OssObject(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");

            _key = key;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                                 "[OSSObject Key={0}, targetBucket={1}]", _key, BucketName ?? string.Empty);
        }

        /// <summary>
        /// 释放<see cref="OssObject.Content" />。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (Content != null)
                {
                    Content.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
