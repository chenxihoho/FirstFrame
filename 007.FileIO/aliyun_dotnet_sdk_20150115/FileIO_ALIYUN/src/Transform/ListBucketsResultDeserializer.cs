/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.IO;
using System.Linq;

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Model;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    /// <summary>
    /// Description of ListBucketsResultDeserializer.
    /// </summary>
    internal class ListBucketsResultDeserializer : ResponseDeserializer<ListBucketsResult, ListAllMyBucketsResult>
    {
        public ListBucketsResultDeserializer(IDeserializer<Stream, ListAllMyBucketsResult> contentDeserializer)
            : base(contentDeserializer)
        {
        }

        public override ListBucketsResult Deserialize(ServiceResponse response)
        {
            var model = ContentDeserializer.Deserialize(response.Content);
            var result = new ListBucketsResult();
            result.Prefix = model.Prefix;
            result.Marker = model.Marker;
            if (model.MaxKeys.HasValue)
            {
                result.MaxKeys = model.MaxKeys.Value;
            }
            if (model.IsTruncated.HasValue)
            {
                result.IsTruncated = model.IsTruncated.Value;
            }
            result.NextMaker = model.NextMarker;

            result.Buckets = model.Buckets.Select(e => 
                                        new Bucket(e.Name)
                                        {
                                            Location = e.Location,
                                            Owner = new Owner(model.Owner.Id, model.Owner.DisplayName),
                                            CreationDate = e.CreationDate
                                        });

            return result;
        }
    }
}
