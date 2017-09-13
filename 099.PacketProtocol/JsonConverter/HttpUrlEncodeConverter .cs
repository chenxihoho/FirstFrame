using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFrame.PacketProtocol
{
    public class HttpUrlEncodeConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return System.Web.HttpUtility.UrlDecode(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //writer.WriteValue(System.Web.HttpUtility.UrlDecode(value.ToString()));
            writer.WriteValue(System.Web.HttpUtility.UrlEncode(value.ToString()));
        }
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
    }
    
}
