using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace FirstFrame.NetIO
{
    public interface IMessageCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnCallback(long Method, string Param, bool TransportCompressed = false);
    }
}
