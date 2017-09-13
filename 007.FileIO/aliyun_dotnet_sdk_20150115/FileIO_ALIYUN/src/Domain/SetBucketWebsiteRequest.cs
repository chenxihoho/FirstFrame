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
    public class SetBucketWebsiteRequest
    {
        /// <summary>
        /// 获取或者设置<see cref="OssObject" />所在<see cref="Bucket" />的名称。
        /// </summary>
        public string BucketName { get; private set; }

        /// <summary>
        /// 索引页面
        /// </summary>
        public string IndexDocument { get; private set; }

        /// <summary>
        /// 错误页面
        /// </summary>
        public string ErrorDocument { get; private set; }

        public SetBucketWebsiteRequest(string bucketName, string indexDocument, string errorDocument)
        {
            if (string.IsNullOrEmpty(bucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");

            if (!OssUtils.IsBucketNameValid(bucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");

            BucketName = bucketName;
            IndexDocument = indexDocument;
            ErrorDocument = errorDocument;
        }
    }
}
