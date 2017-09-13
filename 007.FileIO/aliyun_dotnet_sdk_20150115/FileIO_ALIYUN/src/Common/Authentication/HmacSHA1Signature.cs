/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Aliyun.OpenServices.Common.Authentication
{
    internal class HmacSHA1Signature : ServiceSignature
    {
        private readonly Encoding _encoding = Encoding.UTF8;

        public override string SignatureMethod
        {
            get { return "HmacSHA1"; }
        }

        public override string SignatureVersion
        {
            get { return "1"; }
        }

        protected override string ComputeSignatureCore(string key, string data)
        {
            Debug.Assert(!string.IsNullOrEmpty(data));

            using (var algorithm = KeyedHashAlgorithm.Create(
                SignatureMethod.ToString().ToUpperInvariant()))
            {
                algorithm.Key = _encoding.GetBytes(key.ToCharArray());
                return Convert.ToBase64String(
                    algorithm.ComputeHash(_encoding.GetBytes(data.ToCharArray())));
            }
        }

    }
}
