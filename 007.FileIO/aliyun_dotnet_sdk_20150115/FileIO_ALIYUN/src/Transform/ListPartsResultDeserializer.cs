/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */
 
using System;
using System.IO;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Model;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    /// <summary>
    /// Description of ListPartsResponseDeserializer.
    /// </summary>
    internal class ListPartsResponseDeserializer : ResponseDeserializer<PartListing, ListPartsResult>
    {
        public ListPartsResponseDeserializer(IDeserializer<Stream, ListPartsResult> contentDeserializer)
            : base(contentDeserializer)
        {
        }
        
        public override PartListing Deserialize(ServiceResponse response)
        {
            var listPartResult = ContentDeserializer.Deserialize(response.Content);
            var partListing = new PartListing();
            partListing.BucketName = listPartResult.Bucket;
            partListing.Key = listPartResult.Key;
            partListing.MaxParts = listPartResult.MaxParts;
            partListing.NextPartNumberMarker =
               listPartResult.NextPartNumberMarker.Length == 0 ? 0 : Convert.ToInt32(listPartResult.NextPartNumberMarker);
            partListing.PartNumberMarker = listPartResult.PartNumberMarker;
            partListing.UploadId = listPartResult.UploadId;
            partListing.IsTruncated = listPartResult.IsTruncated;
            if (listPartResult.PartResults != null)
            {
                foreach (var partResult in listPartResult.PartResults)
                {
                    var part = new Part();
                    part.ETag = partResult.ETag != null ? partResult.ETag.Trim('\"') : string.Empty;
                    part.LastModified = partResult.LastModified;
                    part.PartNumber = partResult.PartNumber;
                    part.Size = partResult.Size;
                    partListing.AddPart(part);
                }
            }
            return partListing;
        }
    }
}
