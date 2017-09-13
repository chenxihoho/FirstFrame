using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Net;
using Newtonsoft.Json.Linq;

namespace FirstFrame.Security
{
    public sealed class PermissionHandler
    {
        private static readonly PermissionHandler instance = new PermissionHandler();        
        public PermissionHandler GetInstance() { return instance; }
        public PermissionHandler() { }
        public static bool HasMethodPermission(string PlatformID, string UID, string Method)
        {            
            //MessageProperties properties = OperationContext.Current.IncomingMessageProperties;
            //RemoteEndpointMessageProperty endPoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            //Message request = OperationContext.Current.RequestContext.RequestMessage;
            //Message reply = Message.CreateMessage(MessageVersion.None, null, "");
            //HttpResponseMessageProperty responseProperty = new HttpResponseMessageProperty() { StatusCode = HttpStatusCode.Forbidden };
            //responseProperty.Headers[HttpResponseHeader.ContentType] = "text/html";
            //reply.Properties[HttpResponseMessageProperty.Name] = responseProperty;
            //OperationContext.Current.RequestContext.Reply(request);

            if (GetMethodPermissionPass(PlatformID, UID, Method)) { return true; }
            return false;
        }
        public static bool GetMethodPermissionPass(string PlatformID, string UID, string Method)
        {
            return AppHandler.IAppDAL.CheckMethodPermission(PlatformID, UID, Method);
        }
        public static bool CheckEmployeePermission(string PlatformID, string UID, string Method)
        {
            return AppHandler.IAppDAL.CheckEmployeePermission(PlatformID, UID, Method);
        }
    }
}
