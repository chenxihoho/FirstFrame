using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFrame.PacketProtocol
{
    #region 仅保留指定节点
    public class ContainsResolver : DefaultContractResolver
    {
        string[] _Props = null;
        public ContainsResolver(string[] Props)
        {
            //指定要序列化属性的清单
            this._Props = Props;
        }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> PropertyList = base.CreateProperties(type, memberSerialization);
            //只保留清单有列出的属性
            return PropertyList.Where(p => _Props.Contains(p.PropertyName)).ToList();
        }
    }
    #endregion
    #region 仅排除指定节点
    public class ExclusionResolver : DefaultContractResolver
    {
        string[] _Props = null;
        public ExclusionResolver(string[] Props)
        {
            //指定不要序列化属性的清单
            this._Props = Props;
        }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> PropertyList = base.CreateProperties(type, memberSerialization);
            //排除清单有列出的属性
            return PropertyList.Where(p => !_Props.Contains(p.PropertyName)).ToList();
        }
    }
    #endregion
}
