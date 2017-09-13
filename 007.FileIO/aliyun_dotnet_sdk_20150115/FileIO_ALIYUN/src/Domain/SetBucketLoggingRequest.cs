/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Domain;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Properties;
using System;

namespace Aliyun.OpenServices.OpenStorageService
{
   public class SetBucketLoggingRequest
    {
        /// <summary>
        /// 获取或设置<see cref="Bucket"/>名称。
        /// </summary>
        public string BucketName { get; private set; }

        /// <summary>
        /// 获取或设置存放访问日志的Bucket。
        /// </summary>
        public string TargetBucket { get; private set; }

        /// <summary>
        /// 获取或设置存放访问日志的文件名前缀。
        /// </summary>
        public string TargetPrefix { get; private set; }

        public SetBucketLoggingRequest(string bucketName, string targetBucket, string targetPrefix)
        {
            if (string.IsNullOrEmpty(bucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            if (!OssUtils.IsBucketNameValid(bucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
            if (string.IsNullOrEmpty(targetBucket))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "targetBucket");
            if (!OssUtils.IsBucketNameValid(targetBucket))
                throw new ArgumentException(OssResources.BucketNameInvalid, "targetBucket");
            if (!OssUtils.IsLoggingPrefixValid(targetPrefix))
                throw new ArgumentException("Invalid logging prefix");

            BucketName = bucketName;
            TargetBucket = targetBucket;
            TargetPrefix = targetPrefix;
        }
    }
}
