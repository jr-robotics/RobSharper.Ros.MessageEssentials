using System.Text;

namespace RobSharper.Ros.MessageCli
{
    public class NugetSourceConfiguration
    {
        public string Name { get; set; }
        public string Source { get; set; }
        
        public int ProtocolVersion { get; set; }
        
        public string GetXmlString()
        {
            var sb = new StringBuilder();
            sb.Append("<add key=\"");
            sb.Append(Name);
            sb.Append("\" value=\"");
            sb.Append(Source);
            sb.Append("\"");

            if (ProtocolVersion > 0)
            {
                sb.Append($" protocolVersion=\"{ProtocolVersion}\"");
            }
            
            sb.Append(" />");

            return sb.ToString();
        }
    }
}