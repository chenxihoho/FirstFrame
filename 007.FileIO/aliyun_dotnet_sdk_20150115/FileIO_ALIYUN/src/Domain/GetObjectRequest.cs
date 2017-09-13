﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.Properties;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Domain;

namespace Aliyun.OpenServices.OpenStorageService
{
    /// <summary>
    /// 指定从OSS下载Object的请求参数。
    /// </summary>
    public class GetObjectRequest
    {
        private IList<string> _matchingETagConstraints = new List<string>();
        private IList<string> _nonmatchingEtagConstraints = new List<string>();
        private readonly ResponseHeaderOverrides _responseHeaders = new ResponseHeaderOverrides();

        /// <summary>
        /// 获取或设置<see cref="Bucket" />的名称。
        /// </summary>
        public string BucketName { get; private set; }

        /// <summary>
        /// 获取或设置要下载<see cref="OssObject" />的Key。
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// 获取表示请求应当返回<see cref="OssObject" />内容的字节范围。
        /// </summary>
        /// <remarks>
        /// 调用<see cref="SetRange" />方法进行设置，如果没有设置，则返回null。
        /// </remarks>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public long[] Range { get; private set; }

        /// <summary>
        /// 获取或设置“If-Unmodified-Since”参数。
        /// </summary>
        /// <remarks>
        /// 该参数表示：如果传入参数中的时间等于或者晚于文件实际修改时间，则传送文件；
        /// 如果早于实际修改时间，则返回错误。
        /// </remarks>
        public DateTime? UnmodifiedSinceConstraint { get; set; }

        /// <summary>
        /// 获取或设置“If-Modified-Since”参数。
        /// </summary>
        /// <remarks>
        /// 该参数表示：如果指定的时间早于实际修改时间，则正常传送文件，并返回 200 OK；
        /// 如果参数中的时间和实际修改时间一样或者更晚，会返回错误。
        /// </remarks>
        public DateTime? ModifiedSinceConstraint { get; set; }

        /// <summary>
        /// 获取一个列表表示：如果传入期望的ETag和<see cref="OssObject" />的ETag匹配，则正常的发送文件。
        /// 如果不符合，返回错误。
        /// 对应“If-Match”参数，
        /// </summary>
        public IList<string> MatchingETagConstraints
        {
            get { return _matchingETagConstraints; }
        }

        /// <summary>
        /// 获取一个列表表示：如果传入期望的ETag和<see cref="OssObject" />的ETag不匹配，则正常的发送文件。
        /// 如果符合，返回错误。
        /// 对应“If-None-Match”参数，
        /// </summary>
        public IList<string> NonmatchingETagConstraints
        {
            get { return _nonmatchingEtagConstraints; }
        }

        /// <summary>
        /// 获取的返回请求头重载<see cref="ResponseHeaderOverrides" />实例。
        /// </summary>
        public ResponseHeaderOverrides ResponseHeaders
        {
            get { return _responseHeaders; }
        }

        /// <summary>
        /// 构造一个新的<see cref="GetObjectRequest" />实例。
        /// </summary>
        /// <param name="bucketName"><see cref="OssObject" />所在<see cref="Bucket" />的名称。</param>
        /// <param name="key"><see cref="OssObject" />的<see cref="P:OssObject.Key" />。</param>
        public GetObjectRequest(string bucketName, string key)
        {
            if (string.IsNullOrEmpty(bucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");

            if (!OssUtils.IsBucketNameValid(bucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
            if (!OssUtils.IsObjectKeyValid(key))
                throw new ArgumentException(OssResources.ObjectKeyInvalid, "key");

            BucketName = bucketName;
            Key = key;
        }

        /// <summary>
        /// 设置一个值表示请求应当返回Object内容的字节范围（可选）。
        /// </summary>
        /// <param name="start">
        /// 范围的起始值。
        /// <para>
        /// 当值大于或等于0时，表示起始的字节位置。
        /// 当值为-1时，表示不设置起始的字节位置，此时end参数不能-1，
        /// 例如end为100，Range请求头的值为bytes=-100，表示获取最后100个字节。
        /// </para>
        /// </param>
        /// <param name="end">
        /// 范围的结束值，应当小于内容的字节数。（最大为内容的字节数-1）
        /// <para>
        /// 当值小于或等于0时，表示结束的字节位或最后的字节数。
        /// 当值为-1时，表示不设置结束的字节位置，此时start参数不能为-1，
        /// 例如start为99，Range请求头的值为bytes=99-，表示获取第100个字节及
        /// 以后的所有内容。
        /// </para>
        /// </param>
        public void SetRange(long start, long end)
        {
            Range = new long[] { start, end };
        }

        // Populate the request headers with the request parameters.
        internal void Populate(IDictionary<string, string> headers)
        {
            Debug.Assert(headers != null);

            if (Range != null && (Range[0] >= 0 || Range[1] >= 0))
            {
                var rangeValue = new StringBuilder().Append("bytes=");
                if (Range[0] >= 0) {
                    rangeValue.Append(Range[0].ToString(CultureInfo.InvariantCulture));
                }
                rangeValue.Append("-");
                if (Range[1] >= 0) {
                    rangeValue.Append(Range[1].ToString(CultureInfo.InvariantCulture));
                }

                headers.Add(HttpHeaders.Range, rangeValue.ToString());
            }
            if (ModifiedSinceConstraint != null)
            {
                headers.Add(OssHeaders.GetObjectIfModifiedSince,
                            DateUtils.FormatRfc822Date(ModifiedSinceConstraint.Value));
            }
            if (UnmodifiedSinceConstraint != null)
            {
                headers.Add(OssHeaders.GetObjectIfUnmodifiedSince,
                            DateUtils.FormatRfc822Date(UnmodifiedSinceConstraint.Value));
            }
            if (_matchingETagConstraints.Count > 0)
            {
                headers.Add(OssHeaders.GetObjectIfMatch, OssUtils.JoinETag(_matchingETagConstraints));
            }
            if (_nonmatchingEtagConstraints.Count > 0)
            {
                headers.Add(OssHeaders.GetObjectIfNoneMatch, OssUtils.JoinETag(_nonmatchingEtagConstraints));
            }
        }
    }
}
