/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */
 
using System;
using System.IO;
using System.Xml.Serialization;

namespace Aliyun.OpenServices.Common.Transform
{
    /// <summary>
    /// XmlSerializer.
    /// It serializes objects to XML content.
    /// </summary>
    internal class XmlStreamSerializer<T> : ISerializer<T, Stream>
    {
        private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(T));

        public Stream Serialize(T obj)
        {
            Stream stream = new MemoryStream();
            try
            {
                _serializer.Serialize(stream, obj);
                stream.Seek(0, 0);
                return stream;
            }
            catch (InvalidOperationException ex)
            {
                stream.Close();
                throw new RequestSerializationException(ex.Message, ex);
            }              
        }
    }
}
