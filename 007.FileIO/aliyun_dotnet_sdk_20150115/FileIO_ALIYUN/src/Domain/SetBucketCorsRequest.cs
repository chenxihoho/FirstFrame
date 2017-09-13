﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using Aliyun.OpenServices.OpenStorageService.Utilities;

namespace Aliyun.OpenServices.OpenStorageService
{
    public class SetBucketCorsRequest
    {
        private IList<CORSRule> _corsRules = new List<CORSRule>(); 

        /// <summary>
        /// 获取<see cref="OssObject" />所在<see cref="Bucket" />的名称。
        /// </summary>
        public string BucketName { get; private set; }

        /// <summary>
        /// CORS规则的容器，每个bucket最多允许10条规则。
        /// </summary>
        public IList<CORSRule> CORSRules 
        {
            get { return ((List<CORSRule>) _corsRules).AsReadOnly(); }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("CORSRule list should not be null.");
                }
                if (value.Count > OssUtils.BucketCORSRuleLimit)
                {
                    throw new ArgumentException("One bucket not allow exceed ten item of CORSRules.");
                }
                _corsRules = value;
            }
        }

        public SetBucketCorsRequest(string bucketName) 
        {
            BucketName = bucketName;
        }

        /// <summary>
        /// 添加一条CORSRule。
        /// </summary>
        /// <param name="corsRule"></param>
        public void AddCORSRule(CORSRule corsRule)
        {
            if (corsRule == null)
            {
                throw new ArgumentException("corsRule should not be null or empty");
            }

            if (_corsRules.Count >= OssUtils.BucketCORSRuleLimit)
            {
                throw new ArgumentException("One bucket not allow exceed ten item of CORSRules.");
            }

            if (corsRule.AllowedOrigins.Count == 0)
            {
                throw new ArgumentException("corsRule.AllowedOrigins should not be empty");
            }

            if (corsRule.AllowedMethods.Count == 0)
            {
                throw new ArgumentException("corsRule.AllowedMethods should not be empty.");
            }

            _corsRules.Add(corsRule);
        }

    }

}
