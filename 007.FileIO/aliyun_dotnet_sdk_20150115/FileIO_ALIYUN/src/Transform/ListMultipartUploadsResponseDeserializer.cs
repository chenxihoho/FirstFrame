/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.IO;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Model;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    /// <summary>
    /// Description of ListMultipartUploadsResponseDeserializer.
    /// </summary>
    internal class ListMultipartUploadsResponseDeserializer 
        : ResponseDeserializer<MultipartUploadListing, ListMultipartUploadsResult>
    {
        public ListMultipartUploadsResponseDeserializer(IDeserializer<Stream, ListMultipartUploadsResult> contentDeserializer)
            : base(contentDeserializer)
        {
        }
             
        public override MultipartUploadListing Deserialize(ServiceResponse response)
        {
            var listMultipartUploadsResult = ContentDeserializer.Deserialize(response.Content);
            var uploadsList = new MultipartUploadListing(listMultipartUploadsResult.Bucket);
            uploadsList.BucketName = listMultipartUploadsResult.Bucket;
            uploadsList.Delimiter = listMultipartUploadsResult.Delimiter;
            uploadsList.IsTruncated = listMultipartUploadsResult.IsTruncated;
            uploadsList.KeyMarker = listMultipartUploadsResult.KeyMarker;
            uploadsList.MaxUploads = listMultipartUploadsResult.MaxUploads;
            uploadsList.NextKeyMarker = listMultipartUploadsResult.NextKeyMarker;
            uploadsList.NextUploadIdMarker = listMultipartUploadsResult.NextUploadIdMarker;
            uploadsList.Prefix = listMultipartUploadsResult.Prefix;
            uploadsList.UploadIdMarker = listMultipartUploadsResult.UploadIdMarker;
            
            if (listMultipartUploadsResult.CommonPrefix != null)
            {
                if (listMultipartUploadsResult.CommonPrefix.Prefixs != null)
                {
                    foreach (var prefix in listMultipartUploadsResult.CommonPrefix.Prefixs)
                    {
                        uploadsList.AddCommonPrefix(prefix);
                    }
                }
            }
            
            if (listMultipartUploadsResult.Uploads != null)
            {
                foreach (var uploadResult in listMultipartUploadsResult.Uploads)
                {
                    var upload = new MultipartUpload();
                    upload.Initiated = uploadResult.Initiated;
                    upload.Key = uploadResult.Key;
                    upload.UploadId = uploadResult.UploadId;
                    upload.StorageClass = uploadResult.StorageClass;
                    uploadsList.AddMultipartUpload(upload);
                }
            }
            
            return uploadsList;
        }
    }
}
