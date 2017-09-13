/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

namespace Aliyun.OpenServices.Common.Transform
{
    /// <summary>
    /// Description of Deserializer.
    /// </summary>
    internal interface IDeserializer<TInput, TOutput>
    {
        /// <summary>
        /// Deserialize the instance <typeparamref name="TOutput" />
        /// from an instance of <typeparamref name="TInput" />
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="ResponseDeserializationException">Failed to deserialize the response.</exception>
        TOutput Deserialize(TInput input);
    }
}
