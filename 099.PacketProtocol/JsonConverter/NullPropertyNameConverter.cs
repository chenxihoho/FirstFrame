using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFrame.PacketProtocol
{
    public class NullPropertyNameConverter : JsonConverter
    {
        /// <summary>
        /// 空Name替换序号
        /// </summary>
        public int NullNameSerialNumber = 0;
        /// <summary>
        /// 用指定的值替换空Name
        /// </summary>
        public string PropertyNullNameReplaceValue = "_Null";
        /// <summary>
        /// 用指定的值替换空值NULL
        /// </summary>
        public object PropertyNullValueReplaceValue { get; set; }

        /// <summary>
        /// 属性名称命名规则约定
        /// </summary>
        public ConverterPropertyNameCase PropertyNameCase { get; set; }

        /// <summary>
        /// 自定义属性名称映射规则
        /// </summary>
        public Func<string, string> ProperyNameConverter { get; set; }

        /// <summary>
        /// 从字符流读取对象
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            using (JTokenWriter writer = new JTokenWriter())
            {
                JsonReaderToJsonWriter(reader, writer);

                return writer.Token.ToObject(objectType);
            }
        }

        /// <summary>
        /// 通过对象写字符流
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject jobject = JObject.FromObject(value);
            JsonReader reader = jobject.CreateReader();
            JsonReaderToJsonWriter(reader, writer);
        }

        public void JsonReaderToJsonWriter(JsonReader reader, JsonWriter writer)
        {
            do
            {
                switch (reader.TokenType)
                {
                    case JsonToken.None:
                        break;
                    case JsonToken.StartObject:
                        writer.WriteStartObject();
                        break;
                    case JsonToken.StartArray:
                        writer.WriteStartArray();
                        break;
                    case JsonToken.PropertyName:
                        string propertyName = reader.Value.ToString();
                        writer.WritePropertyName(ConvertPropertyName(propertyName));
                        break;
                    case JsonToken.Comment:
                        writer.WriteComment((reader.Value != null) ? reader.Value.ToString() : null);
                        break;
                    case JsonToken.Integer:
                        writer.WriteValue(Convert.ToInt64(reader.Value, CultureInfo.InvariantCulture));
                        break;
                    case JsonToken.Float:
                        object value = reader.Value;
                        if (value is decimal)
                        {
                            writer.WriteValue((decimal)value);
                        }
                        else if (value is double)
                        {
                            writer.WriteValue((double)value);
                        }
                        else if (value is float)
                        {
                            writer.WriteValue((float)value);
                        }
                        else
                        {
                            writer.WriteValue(Convert.ToDouble(value, CultureInfo.InvariantCulture));
                        }
                        break;
                    case JsonToken.String:
                        writer.WriteValue(reader.Value.ToString());
                        break;
                    case JsonToken.Boolean:
                        writer.WriteValue(Convert.ToBoolean(reader.Value, CultureInfo.InvariantCulture));
                        break;
                    case JsonToken.Null:
                        if (this.PropertyNullValueReplaceValue != null)
                        {
                            writer.WriteValue(this.PropertyNullValueReplaceValue);
                        }
                        else
                        {
                            writer.WriteNull();
                        }
                        break;
                    case JsonToken.Undefined:
                        writer.WriteUndefined();
                        break;
                    case JsonToken.EndObject:
                        writer.WriteEndObject();
                        break;
                    case JsonToken.EndArray:
                        writer.WriteEndArray();
                        break;
                    case JsonToken.EndConstructor:
                        writer.WriteEndConstructor();
                        break;
                    case JsonToken.Date:
                        if (reader.Value is DateTimeOffset)
                        {
                            writer.WriteValue((DateTimeOffset)reader.Value);
                        }
                        else
                        {
                            writer.WriteValue(Convert.ToDateTime(reader.Value, CultureInfo.InvariantCulture));
                        }
                        break;
                    case JsonToken.Raw:
                        writer.WriteRawValue((reader.Value != null) ? reader.Value.ToString() : null);
                        break;
                    case JsonToken.Bytes:
                        if (reader.Value is Guid)
                        {
                            writer.WriteValue((Guid)reader.Value);
                        }
                        else
                        {
                            writer.WriteValue((byte[])reader.Value);
                        }
                        break;
                }
            } while (reader.Read());
        }

        /// <summary>
        /// 自定义转换器是否可用
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            if (objectType != typeof(DateTime))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据外部规则转换属性名
        /// </summary>
        private string ConvertPropertyName(string propertyName)
        {
            if (this.ProperyNameConverter != null)
            {
                propertyName = this.ProperyNameConverter(propertyName);
                if (string.IsNullOrEmpty(propertyName)) { propertyName = PropertyNullNameReplaceValue + NullNameSerialNumber++.ToString(); }
            }

            char[] chars = propertyName.ToCharArray();

            switch (this.PropertyNameCase)
            {
                case ConverterPropertyNameCase.None:
                    break;
                case ConverterPropertyNameCase.CamelCase:
                    for (int i = 0; i < chars.Length; i++)
                    {
                        bool hasNext = (i + 1 < chars.Length);
                        if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                            break;
                        chars[i] = char.ToLower(chars[i], CultureInfo.InvariantCulture);
                    }
                    break;
                case ConverterPropertyNameCase.PascalCase:
                    chars[0] = char.ToUpper(chars.First());
                    break;
            }

            return new string(chars);
        }
    }

    /// <summary>
    /// 属性命名规则
    /// </summary>
    public enum ConverterPropertyNameCase
    {
        /// <summary>
        /// 默认拼写法(默认首字母)
        /// </summary>
        None,

        /// <summary>
        /// 骆驼拼写法(首字母小写)
        /// </summary>
        CamelCase,

        /// <summary>
        /// 帕斯卡拼写法(首字母大写)
        /// </summary>
        PascalCase
    };
}
