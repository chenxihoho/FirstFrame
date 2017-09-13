/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.Linq;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Model;
using System.Collections.Generic;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
    /// <summary>
    /// Description of GetCorsResponseDeserializer.
    /// </summary>
    internal class GetCorsResponseDeserializer : ResponseDeserializer<IList<CORSRule>, SetBucketCorsRequestModel>
    {
        public GetCorsResponseDeserializer(IDeserializer<Stream, SetBucketCorsRequestModel> contentDeserializer)
            : base(contentDeserializer)
        {
        }

        public override IList<CORSRule> Deserialize(ServiceResponse response)
        {
            var model = ContentDeserializer.Deserialize(response.Content);
            IList<CORSRule> corsRuleList = new List<CORSRule>();
            
            foreach (var corsRuleModel in model.CORSRuleModels)
            {
                var corsRule = new CORSRule();
                if (corsRuleModel.AllowedHeaders != null && corsRuleModel.AllowedHeaders.Length > 0)
                {
                    corsRule.AllowedHeaders = corsRuleModel.AllowedHeaders.ToList();
                }
                if (corsRuleModel.AllowedMethods != null && corsRuleModel.AllowedMethods.Length > 0)
                {
                    corsRule.AllowedMethods = corsRuleModel.AllowedMethods.ToList();
                }
                if (corsRuleModel.AllowedOrigins != null && corsRuleModel.AllowedOrigins.Length > 0)
                {
                    corsRule.AllowedOrigins = corsRuleModel.AllowedOrigins.ToList();
                }
                if (corsRuleModel.ExposeHeaders != null && corsRuleModel.ExposeHeaders.Length > 0)
                {
                    corsRule.ExposeHeaders = corsRuleModel.ExposeHeaders.ToList();
                }          
                corsRule.MaxAgeSeconds = corsRuleModel.MaxAgeSeconds;
                corsRuleList.Add(corsRule);
            }

            return corsRuleList;
        }
    }
}
