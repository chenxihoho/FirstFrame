using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFrame.PacketProtocol
{
    public class DataTransRequestPackage
    {
        public string DbName { get; set; }
        public string SqlString { get; set; }
    }
}
