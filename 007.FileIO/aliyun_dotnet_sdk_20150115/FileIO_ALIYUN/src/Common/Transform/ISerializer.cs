/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

namespace Aliyun.OpenServices.Common.Transform
{
    /// <summary>
    /// Description of ISerializer.
    /// </summary>
    internal interface ISerializer<TInput, TOutput>
    {
        TOutput Serialize(TInput input);       
    }
}
