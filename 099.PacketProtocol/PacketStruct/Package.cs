using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFrame.PacketProtocol
{
    public class Package
    {
        public string Code { get; set; }
        public string Method { get; set; }
        public bool Compressed { get; set; }
        public bool Encrypted { get; set; }
        public Object Message { get; set; }
    }
}
