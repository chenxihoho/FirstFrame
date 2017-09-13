/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;

namespace Aliyun.OpenServices.OpenStorageService.Samples
{
    /// <summary>
    /// Sample for copying object.
    /// </summary>
    public static class CopyObjectSample
    {
        public static void CopyObject()
        {
            const string accessId = "<your access id>";
            const string accessKey = "<your access key>";
            const string endpoint = "<valid host name>";

            var client = new OssClient(endpoint, accessId, accessKey);

            const string sourceBucket = "<source bucket>";
            const string sourceKey = "<source key>";
            const string targetBucket = "<target bucket>";
            const string targetKey = "<target key>";

            try
            {
                var metadata = new ObjectMetadata();
                metadata.AddHeader("mk1", "mv1");
                metadata.AddHeader("mk2", "mv2");
                var req = new CopyObjectRequest(sourceBucket, sourceKey, targetBucket, targetKey);
                req.NewObjectMetaData = metadata;
                var ret = client.CopyObject(req);
                Console.WriteLine("target key's etag: " + ret.ETag);
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
