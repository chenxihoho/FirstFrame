/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Utilities;

namespace Aliyun.OpenServices.OpenStorageService.Utilities
{
    /// <summary>
    /// Description of OssUtils.
    /// </summary>
    internal static class OssUtils
    {
        public static readonly Uri DefaultEndpoint = new Uri("http://oss.aliyuncs.com");

        public const string Charset = "utf-8";

        public const int BufferSize = 8192;

        public const long MaxFileSize = 5*1024*1024*1024L;

        public static readonly int DeleteObjectsUpperLimit = 1000;

        public const string DefaultBaseChar = "0123456789ABCDEF";

        public const int MaxPrefixStringSize = 1024;
        public const int MaxMarkerStringSize = 1024;
        public const int MaxDelimiterStringSize = 1024;
        public const int MaxReturnedKeys = 1000;

        public const int BucketCORSRuleLimit = 10;

        public static bool IsBucketNameValid(string bucketName)
        {
            // The caller is responsible for checking name is not null or empty.
            Debug.Assert(!string.IsNullOrEmpty(bucketName));

            var pattern = "^[a-z0-9][a-z0-9\\-]{1,61}[a-z0-9]$";
            var regex = new Regex(pattern);
            var m = regex.Match(bucketName);
            return m.Success;
        }

        public static bool IsObjectKeyValid(string key)
        {
            Debug.Assert(!string.IsNullOrEmpty(key));

            if (key.StartsWith("/") || key.StartsWith("\\"))
                return false;

            var byteCount = Encoding.GetEncoding(Charset).GetByteCount(key);
            return byteCount > 0 && byteCount < 1024;
        }

        public static String MakeResourcePath(string key)
        {
            return (key == null) ? string.Empty : UrlEncodeKey(key);
        }

        public static String MakeResourcePath(String bucket, String key)
        {
            if (bucket != null)
            {
                return bucket + (key != null ? "/" + UrlEncodeKey(key) : string.Empty);
            }
            else
            {
                return string.Empty;
            }
        }

        public static Uri MakeBucketEndpoint(Uri endpoint, string bucket, ClientConfiguration cc)
        {
            return new Uri(endpoint.Scheme + "://"
                           + ((bucket != null && !IsCName(cc, endpoint.Host)) ? (bucket + "." + endpoint.Host) : endpoint.Host)
                           + ((endpoint.Port != 80) ? (":" + endpoint.Port) : ""));
        }

        private static String UrlEncodeKey(String key)
        {
            const char separater = '/';
            var keys = key.Split(separater);
            var uri = new StringBuilder();

            uri.Append(HttpUtils.UrlEncode(keys[0], Charset));

            for (var i = 1; i < keys.Length; i++)
            {
                uri.Append(separater).Append(HttpUtils.UrlEncode(keys[i], Charset));
            }

            if (key.EndsWith(separater.ToString()))
            {
                // String#split ignores trailing empty strings,
                // e.g., "a/b/" will be split as a 2-entries array,
                // so we have to Append all the trailing slash to the uri.
                foreach (var keyChar in key)
                {
                    if (keyChar == separater)
                    {
                        uri.Append(separater);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return uri.ToString();
        }

        public static string TrimETag(string eTag)
        {
            return eTag != null ? eTag.Trim('\"') : null;
        }

        public static string ComputeContentMd5(Stream input)
        {
            using (var md5 = MD5.Create())
            {
                var data = md5.ComputeHash(input);
                var charset = DefaultBaseChar.ToCharArray();
                var sBuilder = new StringBuilder();
                for (var i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(charset[data[i] >> 4]);
                    sBuilder.Append(charset[data[i] & 0x0F]);
                }

                return Convert.ToBase64String(data);
            }
        }

        public static string ExtractBucketName(string location)
        {
            if (string.IsNullOrEmpty(location) || !location.StartsWith("/"))
                return "";
            return location.TrimStart(Convert.ToChar("/"));
        }

        public static bool IsWebpageValid(string webpage)
        {
            if (!string.IsNullOrEmpty(webpage) && webpage.EndsWith(".html") 
                && webpage.Length > 5)
                return true;
            return false;
        }

        public static bool IsLoggingPrefixValid(string loggingPrefix)
        {
            if (string.IsNullOrEmpty(loggingPrefix))
                return true;

            var pattern = "^[a-zA-Z][a-zA-Z0-9\\-]{0,31}$";
            var regex = new Regex(pattern);
            var m = regex.Match(loggingPrefix);
            return m.Success;
        }

        public static string BuildPartCopySource(string bucketName, string objectKey)
        {
            return "/" + bucketName + "/" + objectKey;
        }

        private static bool IsCName(ClientConfiguration cc, string host)
        {
            if (string.IsNullOrEmpty(host) || host.Trim().Length == 0)
                throw new ArgumentException("Host name should not be null or empty.");

            var domain = host.ToLower();
            foreach (var str in cc.RootDomainList)
            {
                if (domain.EndsWith(str))
                    return false;
            }

            return true;
        }

        public static string GetOSVersion()
        {
            try
            {
                var os = Environment.OSVersion;
                return "widnows " + os.Version.Major + "." + os.Version.Minor;
            }
            catch (InvalidOperationException /* ex */)
            {
                return "Unknown OSVersion";
            }
        }

        public static string GetSystemArchitecture()
        {
            return Environment.Is64BitProcess ? "x86_64" : "x86";
        }

        public static string JoinETag(IEnumerable<string> etags)
        {
            StringBuilder result = new StringBuilder();

            bool first = true;
            foreach (string etag in etags)
            {
                if (!first)
                {
                    result.Append(", ");
                }
                result.Append(etag);
                first = false;
            }

            return result.ToString();
        }

        internal static ClientConfiguration GetClientConfiguration(IServiceClient serviceClient)
        {
            var outerClient = (RetryableServiceClient) serviceClient;
            var innerClient = (ServiceClient)outerClient.InnerServiceClient();
            return innerClient.Configuration;
        }
    }
}
