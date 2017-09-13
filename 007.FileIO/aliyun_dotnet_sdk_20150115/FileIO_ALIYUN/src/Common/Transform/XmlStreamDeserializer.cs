/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Aliyun.OpenServices.Common.Communication;

namespace Aliyun.OpenServices.Common.Transform
{
    /// <summary>
    /// XmlStreamDeserializer.
    /// It deserializes the object from XML stream.
    /// </summary>
    internal class XmlStreamDeserializer<T> : IDeserializer<Stream, T>
    {
        private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(T));

        /// <summary>
        /// Deserialize the result to an object of T from the <see cref="ServiceResponse" />.
        /// It will close the underlying stream.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public T Deserialize(Stream xml)
        {
            Debug.Assert(xml != null);
            using (xml)
            {
                try
                {
                    return (T)_serializer.Deserialize(xml);
                }
                catch (XmlException ex)
                {
                    throw new ResponseDeserializationException(ex.Message, ex);
                }
                catch (InvalidOperationException ex)
                {
                    throw new ResponseDeserializationException(ex.Message, ex);
                }
            }
        }
    }
}
