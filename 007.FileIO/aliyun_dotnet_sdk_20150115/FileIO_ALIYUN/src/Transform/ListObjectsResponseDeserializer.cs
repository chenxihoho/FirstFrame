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
    /// Description of ListObjectsResponseDeserializer.
    /// </summary>
    internal class ListObjectsResponseDeserializer : ResponseDeserializer<ObjectListing, ListBucketResult>
    {
        public ListObjectsResponseDeserializer(IDeserializer<Stream, ListBucketResult> contentDeserializer)
            : base(contentDeserializer)
        {
        }
        
        public override ObjectListing Deserialize(ServiceResponse response)
        {
            var listBucketResult = ContentDeserializer.Deserialize(response.Content);
            
            var objectList = new ObjectListing(listBucketResult.Name);
            objectList.Delimiter = listBucketResult.Delimiter;
            objectList.IsTruncated = listBucketResult.IsTruncated;
            objectList.Marker = listBucketResult.Marker;
            objectList.MaxKeys = listBucketResult.MaxKeys;
            objectList.NextMarker = listBucketResult.NextMarker;
            objectList.Prefix = listBucketResult.Prefix;
            
            if (listBucketResult.Contents != null)
            {
                foreach(var contents in listBucketResult.Contents)
                {
                    var summary = new OssObjectSummary();
                    summary.BucketName = listBucketResult.Name;
                    summary.Key = contents.Key;
                    summary.LastModified = contents.LastModified;
                    summary.ETag = contents.ETag != null ? contents.ETag.Trim('\"') : string.Empty;
                    summary.Size = contents.Size;
                    summary.StorageClass = contents.StorageClass;
                    summary.Owner = contents.Owner != null ? new Owner(contents.Owner.Id, contents.Owner.DisplayName) : null;
                    
                    objectList.AddObjectSummary(summary);
                }
            }

            if (listBucketResult.CommonPrefixes != null)
            {
                foreach(var commonPrefixes in listBucketResult.CommonPrefixes)
                {
                    if (commonPrefixes.Prefix != null)
                    {
                        foreach(var prefix in commonPrefixes.Prefix)
                        {
                            objectList.AddCommonPrefix(prefix);
                        }
                    }
                }
            }
            return objectList;
        }
    }
}
