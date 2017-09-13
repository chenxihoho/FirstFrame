using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFrame.PacketProtocol.JsonResolver
{
    public class NullPropertyNameResolver : DefaultContractResolver
    {
        /// <summary>
        /// 空Name替换序号
        /// </summary>
        public int NullNameSerialNumber = 0;
        /// <summary>
        /// 用指定的值替换空Name
        /// </summary>
        public string PropertyNullNameReplaceValue = "_Null";
        protected override string ResolvePropertyName(string propertyName)
        {
            return ReplaceNullName(propertyName);
        }

        private string ReplaceNullName(string Name)
        {
            return string.IsNullOrEmpty(Name) ? PropertyNullNameReplaceValue + NullNameSerialNumber++.ToString() : Name;
        }
    }
}
