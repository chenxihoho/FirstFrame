/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService
{
    public class MultipartUploadSample
    {
        //const string accessId = "zfPJSCn8iPV8zFJa";
        //const string accessKey = "tIFbM7SU6MdKrMwdLuLmEXyiHnroIC";
        //private const string _endpoint = "http://oss.aliyuncs.com";

        public static OssClient _ossClient = null;

        /// <summary>
        /// 分片上传。
        /// </summary>
        /// <param name="bucketName"><see cref="Bucket"/></param>
        /// <param name="objectName"><see cref="OssObject"/></param>
        /// <param name="fileToUpload">指定分片上传文件路径</param>
        /// <param name="partSize">分片大小（单位：字节）</param>
        public static string UploadMultipart(String bucketName, String objectName, String fileToUpload, int partSize)
        {
            var uploadId = InitiateMultipartUpload(bucketName, objectName);
            var partETags = BeginUploadPart(bucketName, objectName, fileToUpload, uploadId, partSize);
            var completeResult = CompleteUploadPart(bucketName, objectName, uploadId, partETags);
            return completeResult.Location;
        }

        /// <summary>
        /// 分片拷贝。
        /// </summary>
        /// <param name="targetBucket">目标<see cref="Bucket"/></param>
        /// <param name="targetKey">目标<see cref="OssObject"/></param>
        /// <param name="sourceBucket">源<see cref="Bucket"/></param>
        /// <param name="sourceKey">源<see cref="OssObject"/></param>
        /// <param name="partSize">分片大小（单位：字节）</param>
        public static void UploadMultipartCopy(String targetBucket, String targetKey, String sourceBucket, String sourceKey,  int partSize)
        {
            var uploadId = InitiateMultipartUpload(targetBucket, targetKey);
            var partETags = BeginUploadPartCopy(targetBucket, targetKey, sourceBucket, sourceKey, uploadId, partSize);
            var completeResult = CompleteUploadPart(targetBucket, targetKey, uploadId, partETags);

            Console.WriteLine("Upload multipart copy result : ");
            Console.WriteLine(completeResult.Location);
        }

        private static string InitiateMultipartUpload(String bucketName, String objectName)
        {
            var request = new InitiateMultipartUploadRequest(bucketName, objectName);
            var result = _ossClient.InitiateMultipartUpload(request);
            return result.UploadId;
        }

        private static List<PartETag> BeginUploadPart(String bucketName, String objectName, String fileToUpload,
            String uploadId, int partSize)
        {
            var fileSize = -1;
            using (var fs = File.Open(fileToUpload, FileMode.Open))
            {
                fileSize = (int)fs.Length;
            }

            var partCount = fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            var partETags = new List<PartETag>();
            for (var i = 0; i < partCount; i++)
            {
                using (FileStream fs = File.Open(fileToUpload, FileMode.Open))
                {
                    var skipBytes = (long)partSize * i;
                    fs.Seek(skipBytes, 0);
                    var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                    var request = new UploadPartRequest(bucketName, objectName, uploadId);
                    request.InputStream = fs;
                    request.PartSize = size;
                    request.PartNumber = i + 1;
                    var result = _ossClient.UploadPart(request);
                    partETags.Add(result.PartETag);
                }
            }
            return partETags;
        }

        private static List<PartETag> BeginUploadPartCopy(String targetBucket, String targetKey, String sourceBucket, String sourceKey,
            String uploadId, int partSize)
        {
            var metadata = _ossClient.GetObjectMetadata(sourceBucket, sourceKey);
            var fileSize = metadata.ContentLength;

            var partCount = (int)fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            var partETags = new List<PartETag>();
            for (var i = 0; i < partCount; i++)
            {
                var skipBytes = (long)partSize * i;
                var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                var request = 
                    new UploadPartCopyRequest(targetBucket, targetKey, sourceBucket, sourceKey, uploadId);
                request.PartSize = size;
                request.PartNumber = i + 1;
                request.BeginIndex = skipBytes;
                var result = _ossClient.UploadPartCopy(request);
                partETags.Add(result.PartETag);
            }
            return partETags;
        }

        private static CompleteMultipartUploadResult CompleteUploadPart(String bucketName, String objectName,
            String uploadId, List<PartETag> partETags)
        {
            var completeMultipartUploadRequest =
                new CompleteMultipartUploadRequest(bucketName, objectName, uploadId);
            foreach (var partETag in partETags)
            {
                completeMultipartUploadRequest.PartETags.Add(partETag);
            }

            return _ossClient.CompleteMultipartUpload(completeMultipartUploadRequest);
        }
    }
 }

