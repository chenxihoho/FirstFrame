/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Net;

namespace Aliyun.OpenServices.OpenStorageService.Samples
{
    /// <summary>
    /// Sample for the usage of URL Signature.
    /// </summary>
    public static class UrlSignatureSample
    {
        public static void genUrl()
        {
            const string accessId = "<your access id>";
            const string accessKey = "<your access key>";
            const string endpoint = "<valid host name>";

            const string bucketName = "<bucket name>";
            const string key = "<object name>";

            OssClient client = new OssClient(endpoint, accessId, accessKey);

            try
            {
                var metadata = client.GetObjectMetadata(bucketName, key);
                var etag = metadata.ETag;

                var req = new GeneratePresignedUriRequest(bucketName, key, SignHttpMethod.Get);
                req.AddQueryParam("param1", "value1");
                req.ContentType = "text/html";
                req.ContentMd5 = etag;
                req.AddUserMetadata("mk1", "mv1");
                req.AddUserMetadata("mk2", "mv2");
                req.ResponseHeaders.CacheControl = "No-Cache";
                req.ResponseHeaders.ContentEncoding = "utf-8";
                req.ResponseHeaders.ContentType = "text/html";
                var uri = client.GeneratePresignedUri(req);

                Console.WriteLine(uri.ToString());

                var webRequest = (HttpWebRequest)WebRequest.Create(uri);
                webRequest.ContentType = "text/html";
                webRequest.Headers.Add(HttpRequestHeader.ContentMd5, etag);
                webRequest.Headers.Add("x-oss-meta-mk1", "mv1");
                webRequest.Headers.Add("x-oss-meta-mk2", "mv2");
                var resp = webRequest.GetResponse() as HttpWebResponse;
                var output = resp.GetResponseStream();
                var bufferSize = 2048;
                var length = 0;
                var bytes = new byte[bufferSize];
                try
                {
                    do
                    {
                        length = output.Read(bytes, 0, bufferSize);
                        // to do something with bytes...
                    } while (length > 0);
                    output.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ex : " + ex.Message);
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
