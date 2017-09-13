﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;

namespace Aliyun.OpenServices.OpenStorageService.Samples
{
    /// <summary>
    /// Sample for listing buckets.
    /// </summary>
    public static class ListBucketsSample
    {
        public static void ListBuckets()
        {
            const string accessId = "MtXKLwm7Z8Nd93wj";
            const string accessKey = "sgv8LpQvoxaUkQ7kZDENMtGMteNpO1";
            const string endpoint = "http://oss.aliyuncs.com";

            OssClient client = new OssClient(endpoint, accessId, accessKey);

            try
            {
                // 1. Try to list all buckets. 
                var buckets = client.ListBuckets();
                Console.WriteLine("List all buckets: ");
                foreach (var bucket in buckets)
                {
                    Console.WriteLine(bucket.Name + ", " + bucket.Location + ", " + bucket.Owner);
                }

                // 2. List buckets by specified conditions, such as prefix/marker/max-keys.
                var req = new ListBucketsRequest {Prefix = "test", MaxKeys = 3, Marker = "test2"};
                var res = client.ListBuckets(req);
                buckets = res.Buckets;
                Console.WriteLine("List buckets by page: ");
                Console.WriteLine("Prefix: " + res.Prefix + ", MaxKeys: " + res.MaxKeys + ", Marker: " + res.Marker
                    + ", NextMarker: " + res.NextMaker);
                foreach (var bucket in buckets)
                {
                    Console.WriteLine(bucket.Name + ", " + bucket.Location + ", " + bucket.Owner);
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
    }
}
