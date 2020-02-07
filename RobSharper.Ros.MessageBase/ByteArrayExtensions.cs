using System.Text;

namespace RobSharper.Ros.MessageBase
{
    internal static class ByteArrayExtensions
    {
        public static string ToHexString(this byte[] buffer)
        {
            var sb = new StringBuilder(buffer.Length * 2);
            foreach (byte b in buffer)
            {
                sb.AppendFormat("{0:x2}", b);
            }

            return sb.ToString();
        }
    }
}