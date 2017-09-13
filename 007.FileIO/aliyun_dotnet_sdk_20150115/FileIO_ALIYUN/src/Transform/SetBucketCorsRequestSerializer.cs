﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Model;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    /// <summary>
    /// Description of SetBucketCorsRequestSerializer.
    /// </summary>
    internal class SetBucketCorsRequestSerializer : RequestSerializer<SetBucketCorsRequest, SetBucketCorsRequestModel>
    {
        public SetBucketCorsRequestSerializer(ISerializer<SetBucketCorsRequestModel, Stream> contentSerializer)
            : base(contentSerializer)
        {

        }

       public override Stream Serialize(SetBucketCorsRequest request)
       {
            var model = new SetBucketCorsRequestModel();
            model.CORSRuleModels = new SetBucketCorsRequestModel.CORSRuleModel[request.CORSRules.Count];       
           
            for (var i = 0; i < request.CORSRules.Count ;i++ )
            {
                var corsRuleModel = new SetBucketCorsRequestModel.CORSRuleModel();

                if (request.CORSRules[i].AllowedHeaders != null)
                {
                    corsRuleModel.AllowedHeaders = new string[request.CORSRules[i].AllowedHeaders.Count];
                    for (var j = 0; j < request.CORSRules[i].AllowedHeaders.Count; j++)
                    {
                        corsRuleModel.AllowedHeaders[j] = request.CORSRules[i].AllowedHeaders[j];
                    }
                }

                if (request.CORSRules[i].AllowedMethods != null)
                {
                    corsRuleModel.AllowedMethods = new string[request.CORSRules[i].AllowedMethods.Count];
                    for (var j = 0; j < request.CORSRules[i].AllowedMethods.Count; j++)
                    {
                        corsRuleModel.AllowedMethods[j] = request.CORSRules[i].AllowedMethods[j];
                    }
                }

                if (request.CORSRules[i].AllowedOrigins != null)
                {
                    corsRuleModel.AllowedOrigins = new string[request.CORSRules[i].AllowedOrigins.Count];
                    for (var j = 0; j < request.CORSRules[i].AllowedOrigins.Count; j++)
                    {
                        corsRuleModel.AllowedOrigins[j] = request.CORSRules[i].AllowedOrigins[j];
                    }
                }

                if (request.CORSRules[i].ExposeHeaders != null)
                {
                    corsRuleModel.ExposeHeaders = new string[request.CORSRules[i].ExposeHeaders.Count];
                    for (var j = 0; j < request.CORSRules[i].ExposeHeaders.Count; j++)
                    {
                        corsRuleModel.ExposeHeaders[j] = request.CORSRules[i].ExposeHeaders[j];
                    }
                }

                corsRuleModel.MaxAgeSeconds = request.CORSRules[i].MaxAgeSeconds;

                model.CORSRuleModels[i] = corsRuleModel;
            }

            return ContentSerializer.Serialize(model);
        }
    }
}
