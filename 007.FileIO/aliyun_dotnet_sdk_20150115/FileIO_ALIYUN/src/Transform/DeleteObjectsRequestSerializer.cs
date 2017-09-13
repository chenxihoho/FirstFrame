/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.IO;
using System.Collections.Generic;

using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Model;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    /// <summary>
    /// Description of DeleteObjectsRequestSerializer.
    /// </summary>
    internal class DeleteObjectsRequestSerializer : RequestSerializer<DeleteObjectsRequest, DeleteObjectsRequestModel>
    {
        public DeleteObjectsRequestSerializer(ISerializer<DeleteObjectsRequestModel, Stream> contentSerializer)
            : base(contentSerializer)
        {
        }

        public override Stream Serialize(DeleteObjectsRequest request)
        {
            var model = new DeleteObjectsRequestModel();
            var objectsToDel = new List<DeleteObjectsRequestModel.ObjectToDel>();
            model.Quiet = request.Quiet;
            foreach (var key in request.Keys)
            {
                var obj = new DeleteObjectsRequestModel.ObjectToDel();
                obj.Key = key;
                objectsToDel.Add(obj);
            }
            model.Keys = objectsToDel.ToArray();
            return ContentSerializer.Serialize(model);
        }
    }
}
