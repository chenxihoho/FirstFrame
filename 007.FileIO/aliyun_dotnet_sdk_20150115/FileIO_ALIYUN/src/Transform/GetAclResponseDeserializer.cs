/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.IO;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenStorageService.Model;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    /// <summary>
    /// Description of GetAclResponseDeserializer.
    /// </summary>
    internal class GetAclResponseDeserializer : ResponseDeserializer<AccessControlList, AccessControlPolicy>
    {
        public GetAclResponseDeserializer(IDeserializer<Stream, AccessControlPolicy> contentDeserializer)
            : base(contentDeserializer)
        {
        }

        public override AccessControlList Deserialize(ServiceResponse response)
        {
            var model = ContentDeserializer.Deserialize(response.Content);
            var acl = new AccessControlList();
            acl.Owner = new Owner(model.Owner.Id, model.Owner.DisplayName);
            foreach(var grant in model.Grants)
            {
                // Do not need to grant permission if the acl is "private".
                if (grant == CannedAccessControlList.PublicRead.GetStringValue())
                {
                    acl.GrantPermission(GroupGrantee.AllUsers, Permission.Read);
                }
                else if (grant == CannedAccessControlList.PublicReadWrite.GetStringValue())
                {
                    acl.GrantPermission(GroupGrantee.AllUsers, Permission.FullControl);
                }

            }
            return acl;
        }
    }
}
