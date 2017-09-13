/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.IO;
using System.Diagnostics;
using Aliyun.OpenServices.Common.Transform;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    /// <summary>
    /// Description of SetBucketRefererRequestSerializer.
    /// </summary>
    internal class SetBucketRefererRequestSerializer : RequestSerializer<SetBucketRefererRequest, RefererConfiguration>
    {
        public SetBucketRefererRequestSerializer(ISerializer<RefererConfiguration, Stream> contentSerializer)
            : base(contentSerializer)
        {
        }

       public override Stream Serialize(SetBucketRefererRequest request)
        {
           Debug.Assert(request != null);

           var model  = new RefererConfiguration();
           model.AllowEmptyReferer = request.AllowEmptyReferer;
           model.RefererList = new RefererConfiguration.RefererListModel();
           model.RefererList.Referers = new string[request.RefererList.Count];
           for (var i = 0; i < request.RefererList.Count ; i++)
               model.RefererList.Referers[i] = request.RefererList[i];

            return ContentSerializer.Serialize(model);
        }
    }
}
