/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenStorageService.Samples
{
    /// <summary>
    /// Sample for the usage of CNAME.
    /// </summary>
    public static class CNameSample
    {
        public static void SetRootDomains()
        {
            const string accessId = "<your access id>";
            const string accessKey = "<your access key>";
            const string endpoint = "<valid host name>";

            var client = new OssClient(endpoint, accessId, accessKey);

            try
            {
                var cc = new ClientConfiguration();
                var domainList = cc.RootDomainList;
                foreach (var domain in domainList)
                {
                    Console.WriteLine(domain);
                }

                Console.WriteLine("\nafter modifed: ");
                var domains = new List<string>();
                domains.Add(".alibaba-inc.com");
                domains.Add(".aliyuncs.gd");
                cc.RootDomainList = domains;
                foreach (string domain in cc.RootDomainList)
                {
                    Console.WriteLine(domain);
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
