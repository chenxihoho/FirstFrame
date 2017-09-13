/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */
 
using System;
using Aliyun.OpenServices.Properties;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Domain;

namespace Aliyun.OpenServices.OpenStorageService
{
    /// <summary>
    /// 包含获取<see cref="MultipartUpload" />列表的请求信息。
    /// </summary>
    public class ListMultipartUploadsRequest 
    {
        /// <summary>
        /// 获取<see cref="OssObject" />所在<see cref="Bucket" />的名称。
        /// </summary>
        public string BucketName { get; private set; }
        
        /// <summary>
        /// 获取或设置一个值
        /// 用来对返回结果进行分组
        /// </summary>
        public string Delimiter { get; set; }
        
        /// <summary>
        /// 获取或设置一个值
        /// 限定此次返回Multipart Uploads事件的最大数目
        /// 如果不设定，默认为1000
        /// max-keys取值不能大于1000
        /// </summary>
        public int? MaxUploads { get; set; }
        
        /// <summary>
        /// 获取或设置一个值
        /// 与upload-id-marker参数一同使用来指定返回结果的起始位置。
        /// 如果upload-id-marker参数未设置，查询结果中包含：所有Object名字的字典序大于key-marker参数值的Multipart事件。
        /// 如果upload-id-marker参数被设置，查询结果中包含：所有Object名字的字典序大于key-marker参数值的Multipart事件和Object名字等于key-marker参数值，但是Upload ID比upload-id-marker参数值大的Multipart Uploads事件。
        /// </summary>
        public string KeyMarker { get; set; }
        
        /// <summary>
        /// 获取或设置一个值
        /// 限定返回的object key必须以prefix作为前缀。
        /// </summary>
        public string Prefix { get; set;}
        
        /// <summary>
        /// 获取或设置一个值
        /// 与key-marker参数一同使用来指定返回结果的起始位置。
        /// 如果key-marker参数未设置，则OSS忽略upload-id-marker参数。
        /// 如果key-marker参数被设置，查询结果中包含：所有Object名字的字典序大于key-marker参数值的Multipart事件和Object名字等于key-marker参数值，但是Upload ID比upload-id-marker参数值大的Multipart Uploads事件。
        /// </summary>
        public string UploadIdMarker { get; set;}
        
        public ListMultipartUploadsRequest(string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            if (!OssUtils.IsBucketNameValid(bucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
    
                BucketName = bucketName;
        }
    }
}
