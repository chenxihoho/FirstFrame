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
    /// Sample for setting bucket acl.
    /// </summary>
    public static class SetBucketAclSample
    {
        public static void SetBucketAcl()
        {
            const string accessId = "<your access id>";
            const string accessKey = "<your access key>";
            const string endpoint = "<valid host name>";

            const string bucketName = "<bucket name>";

            OssClient client = new OssClient(endpoint, accessId, accessKey);

            try
            {
                client.SetBucketAcl(bucketName, CannedAccessControlList.PublicRead);
                //client.SetBucketAcl(new SetBucketAclRequest(bucketName, CannedAccessControlList.PublicRead));
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
