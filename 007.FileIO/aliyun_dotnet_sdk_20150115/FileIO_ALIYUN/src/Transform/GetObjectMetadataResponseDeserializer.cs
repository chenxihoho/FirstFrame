/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Diagnostics;
using System.Globalization;

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenStorageService.Utilities;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    /// <summary>
    /// Description of GetObjectMetadataResponseDeserializer.
    /// </summary>
    internal class GetObjectMetadataResponseDeserializer : ResponseDeserializer<ObjectMetadata, ObjectMetadata>
    {
        public GetObjectMetadataResponseDeserializer()
            : base(null)
        {
        }

        public override ObjectMetadata Deserialize(ServiceResponse response)
        {
            Debug.Assert(response != null && response.Headers != null);

            var metadata = new ObjectMetadata();
            foreach(var header in response.Headers)
            {
                if (header.Key.StartsWith(OssHeaders.OssUserMetaPrefix, false, CultureInfo.InvariantCulture))
                {
                    // The key of user in the metadata should not contain the prefix.
                    metadata.UserMetadata.Add(header.Key.Substring(OssHeaders.OssUserMetaPrefix.Length),
                                              header.Value);
                }
                else if (string.Equals(header.Key, HttpHeaders.ContentLength, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Content-Length. Parse should not fail.
                    metadata.ContentLength = long.Parse(header.Value, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(header.Key, HttpHeaders.ETag, StringComparison.InvariantCultureIgnoreCase))
                {
                    metadata.ETag = OssUtils.TrimETag(header.Value);
                }
                else if (string.Equals(header.Key, HttpHeaders.LastModified, StringComparison.InvariantCultureIgnoreCase))
                {
                    metadata.LastModified = DateUtils.ParseRfc822Date(header.Value);
                }
                else
                {
                    // Treat the other headers just as strings.
                    metadata.AddHeader(header.Key, header.Value);
                }
            }
            return metadata;
        }
    }
}
