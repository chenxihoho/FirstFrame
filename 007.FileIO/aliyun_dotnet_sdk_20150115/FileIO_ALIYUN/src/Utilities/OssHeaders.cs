﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

namespace Aliyun.OpenServices.OpenStorageService.Utilities
{
    /// <summary>
    /// Description of OSSHeaders.
    /// </summary>
    internal class OssHeaders
    {
        public const string OssPrefix = "x-oss-";
        public const string OssUserMetaPrefix = "x-oss-meta-";

        public const string OssCannedAcl = "x-oss-acl";
        public const string OssStorageClass = "x-oss-storage-class";
        public const string OssVersionId = "x-oss-version-id";

        public const string GetObjectIfModifiedSince = "If-Modified-Since";
        public const string GetObjectIfUnmodifiedSince = "If-Unmodified-Since";
        public const string GetObjectIfMatch = "If-Match";
        public const string GetObjectIfNoneMatch = "If-None-Match";
        
        public const string CopyObjectSource = "x-oss-copy-source";
        public const string CopySourceIfMatch = "x-oss-copy-source-if-match";
        public const string CopySourceIfNoneMatch = "x-oss-copy-source-if-none-match";
        public const string CopySourceIfUnmodifiedSince = "x-oss-copy-source-if-unmodified-since";
        public const string CopySourceIfModifedSince = "x-oss-copy-source-if-modified-since";
        public const string CopyObjectMetaDataDirective = "x-oss-metadata-directive";

        public const string OptionsOrigin = "Origin";
        public const string AccessControlRequestMethod = "Access-Control-Request-Method";
        public const string AccessControlRequestHeaders = "Access-Control-Request-Headers";

    }
}
