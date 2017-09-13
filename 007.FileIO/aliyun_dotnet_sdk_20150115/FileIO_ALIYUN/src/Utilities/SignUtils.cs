/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Utilities;

namespace Aliyun.OpenServices.OpenStorageService.Utilities
{
    /// <summary>
    /// Description of SignUtils.
    /// </summary>
    internal class SignUtils
    {
        private class KeyComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return String.Compare(x, y, StringComparison.Ordinal);
            }
        }

        private const string _newLineMarker = "\n";

        private static readonly IList<string> SIGNED_PARAMTERS = new List<string> {
            "acl", "uploadId", "partNumber", "uploads", "cors", "logging", "website", "delete", "referer",
            ResponseHeaderOverrides.ResponseCacheControl,
            ResponseHeaderOverrides.ResponseContentDisposition,
            ResponseHeaderOverrides.ResponseContentEncoding,
            ResponseHeaderOverrides.ResponseHeaderContentLanguage,
            ResponseHeaderOverrides.ResponseHeaderContentType,
            ResponseHeaderOverrides.ResponseHeaderExpires
        };

        public static string BuildCanonicalString(string method, string resourcePath,
                                                  ServiceRequest request/*, string expires*/)
        {
            var builder = new StringBuilder();
            builder.Append(method).Append(_newLineMarker);

            var headers = request.Headers;
            IDictionary<string, string> headersToSign = new Dictionary<string, string>();

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    var lowerKey = header.Key.ToLowerInvariant();

                    if (lowerKey == HttpHeaders.ContentType.ToLowerInvariant()
                        || lowerKey == HttpHeaders.ContentMd5.ToLowerInvariant()
                        || lowerKey == HttpHeaders.Date.ToLowerInvariant()
                        || lowerKey.StartsWith(OssHeaders.OssPrefix))
                    {
                        headersToSign.Add(lowerKey, header.Value);
                    }
                }
            }

            if (!headersToSign.ContainsKey(HttpHeaders.ContentType.ToLowerInvariant()))
            {
                headersToSign.Add(HttpHeaders.ContentType.ToLowerInvariant(), "");
            }
            if (!headersToSign.ContainsKey(HttpHeaders.ContentMd5.ToLowerInvariant()))
            {
                headersToSign.Add(HttpHeaders.ContentMd5.ToLowerInvariant(), "");
            }

            // Add params that have the prefix "x-oss-"
            if (request.Parameters != null)
            {
                foreach (var p in request.Parameters)
                {
                    if (p.Key.StartsWith(OssHeaders.OssPrefix))
                    {
                        headersToSign.Add(p.Key, p.Value);
                    }
                }
            }

            // Add all headers to sign to the builder
            // The headers should be ordered.
            foreach (var entry in headersToSign.OrderBy(e => e.Key, new KeyComparer()))
            {
                var key = entry.Key;
                Object value = entry.Value;

                if (key.StartsWith(OssHeaders.OssPrefix))
                {
                    builder.Append(key).Append(':').Append(value);
                }
                else
                {
                    builder.Append(value);
                }

                builder.Append(_newLineMarker);
            }

            // Add canonical resource
            builder.Append(BuildCanonicalizedResource(resourcePath, request.Parameters));

            return builder.ToString();
        }

        private static string BuildCanonicalizedResource(string resourcePath,
                                                         IDictionary<string, string> parameters)
        {
            Debug.Assert(resourcePath.StartsWith("/"));

            var builder = new StringBuilder();
            builder.Append(resourcePath);

            if (parameters != null)
            {
                var parameterNames = parameters.Keys.OrderBy(e => e);
                var separater = '?';
                foreach (var paramName in parameterNames)
                {
                    if (!SIGNED_PARAMTERS.Contains(paramName))
                    {
                        continue;
                    }

                    builder.Append(separater);
                    builder.Append(paramName);
                    var paramValue = parameters[paramName];
                    if (paramValue != null)
                    {
                        builder.Append("=").Append(paramValue);
                    }

                    separater = '&';
                }
            }

            return builder.ToString();
        }
    }
}
