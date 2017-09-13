/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

namespace Aliyun.OpenServices.OpenStorageService
{
    public class SetBucketAclRequest
    {
        /// <summary>
        /// 获取<see cref="OssObject" />所在<see cref="Bucket" />的名称。
        /// </summary>
        public string BucketName { get; private set; }

        /// <summary>
        /// 获取用户访问权限。
        /// </summary>
        public CannedAccessControlList ACL { get; private set; }

        public SetBucketAclRequest(string bucketName, CannedAccessControlList acl) 
        {
            BucketName = bucketName;
            ACL = acl;
        }
    }

}
