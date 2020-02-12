using System.Text;

namespace RobSharper.Ros.MessageEssentials
{
    public static class RosServiceInfoExtensions
    {
        public static string GetServiceDefinition(this IRosServiceInfo serviceInfo)
        {
            if (serviceInfo == null)
                return null;
            
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(serviceInfo.Request.MessageDefinition))
                sb.Append(serviceInfo.Request.MessageDefinition);
            
            sb.Append("\n---\n");
            
            if (!string.IsNullOrEmpty(serviceInfo.Response.MessageDefinition))
                sb.Append(serviceInfo.Response.MessageDefinition);

            return sb.ToString();
        }
    }
}