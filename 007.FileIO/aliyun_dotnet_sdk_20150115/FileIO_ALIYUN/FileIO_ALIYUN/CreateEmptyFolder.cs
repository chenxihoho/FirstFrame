/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService
{
    /// <summary>
    /// Sample for creating an empty folder.
    /// </summary>
    public static class CreateEmptyFolder
    {
        public static void CreateFolder(string accessId, string accessKey, string endpoint, string _bucketName, string _FolderName)
        {
            var client = new OssClient(endpoint, accessId, accessKey);

            string bucketName = _bucketName;
            // Note: key treats as a folder and must end with slash.
            string key = _FolderName + "/";  
            try
            {
                // create bucket
                client.CreateBucket(bucketName);
                Console.WriteLine("Created bucket name: " + bucketName);

                // put object with zero bytes stream.
                using (MemoryStream memStream = new MemoryStream())
                {
                    PutObjectResult ret = client.PutObject(bucketName, key, memStream);
                    Console.WriteLine("Uploaded empty object's ETag: " + ret.ETag);
                }
            }
            catch (OssException ex)
            {
                if (ex.ErrorCode == OssErrorCode.BucketAlreadyExists)
                {
                    Console.WriteLine("Bucket '{0}' already exists, please modify and recreate it.", bucketName);
                }
                else
                {
                    Console.WriteLine("CreateBucket Failed with error info: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}", 
                        ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                }
            }
        }
    }
}
