/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Samples
{
    /// <summary>
    /// Sample for putting object.
    /// </summary>
    public static class PutObjectSample
    {
        public static void PutObject()
        {
            const string accessId = "MtXKLwm7Z8Nd93wj";
            const string accessKey = "sgv8LpQvoxaUkQ7kZDENMtGMteNpO1";
            const string endpoint = "http://oss.aliyuncs.com";

            OssClient client = new OssClient(endpoint, accessId, accessKey);

            const string bucketName = "zxg-user";
            const string key = "123";
            const string fileToUpload = @"D:\Demo\aliyun_dotnet_sdk_20150115\doc\html\SearchHelp.php";

            try
            {
                // 1. put object to specified output stream
                using (var fs = File.Open(fileToUpload, FileMode.Open))
                {
                    var metadata = new ObjectMetadata();
                    metadata.UserMetadata.Add("mykey1", "myval1");
                    metadata.UserMetadata.Add("mykey2", "myval2");
                    metadata.CacheControl = "No-Cache";
                    metadata.ContentType = "text/html";
                    client.PutObject(bucketName, key, fs, metadata);

                    metadata = client.GetObjectMetadata(bucketName, key);
                }

                // 2. put object to specified file
                //client.PutObject(bucketName, key, fileToUpload);

                // 3. put object from specified object with multi-level virtual directory
                //key = "folder/sub_folder/key0";
                //client.PutObject(bucketName, key, fileToUpload);
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
