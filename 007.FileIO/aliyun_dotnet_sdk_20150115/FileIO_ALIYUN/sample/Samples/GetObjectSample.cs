﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;

namespace Aliyun.OpenServices.OpenStorageService.Samples
{
    /// <summary>
    /// Sample for getting object.
    /// </summary>
   public static class GetObjectSample
    {
        public static void GetObject()
        {
            const string accessId = "<your access id>";
            const string accessKey = "<your access key>";
            const string endpoint = "<valid host name>";

            const string bucketName = "<bucket name>";
            const string key = "<object name>";
            const string filePath = "<file to upload>";

            try
            {
                OssClient ossClient = new OssClient(new Uri(endpoint), accessId, accessKey);

                string eTag;
                using (Stream fs = File.Open(filePath, FileMode.Open))
                {
                    // compute content's md5
                    eTag = ComputeContentMd5(fs);
                }

                // put object
                var metadata = new ObjectMetadata();
                metadata.ETag = eTag;
                ossClient.PutObject(bucketName, key, filePath, metadata);

                // verify etag
                var o = ossClient.GetObject(bucketName, key);
                using (var requestStream = o.Content)
                {
                    eTag = ComputeContentMd5(requestStream);
                    Assert.AreEqual(o.Metadata.ETag, eTag.ToUpper());
                }
            }
            catch (OssException ex)
            {
                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}", 
                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
            }
        }

        public static string ComputeContentMd5(Stream inputStream)
        {
            using (var md5 = MD5.Create())
            {
                var data = md5.ComputeHash(inputStream);
                var sBuilder = new StringBuilder();
                foreach (var t in data)
                {
                    sBuilder.Append(t.ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }
}
