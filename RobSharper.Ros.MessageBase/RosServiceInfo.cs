using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace RobSharper.Ros.MessageBase
{
    public class RosServiceInfo : IRosServiceInfo
    {
        private readonly Lazy<string> _md5Sum;
        
        public RosType Type { get; }

        public IRosMessageTypeInfo Request { get; }

        public IRosMessageTypeInfo Response { get; }

        public string MD5Sum => _md5Sum.Value;

        public RosServiceInfo(RosType type, IRosMessageTypeInfo request, IRosMessageTypeInfo response, string md5Sum)
            : this(type, request, response, new Lazy<string>(() => md5Sum))
        {
            
        }
        
        public RosServiceInfo(RosType type, IRosMessageTypeInfo request, IRosMessageTypeInfo response, Lazy<string> md5Sum)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Request = request ?? throw new ArgumentNullException(nameof(request));
            Response = response ?? throw new ArgumentNullException(nameof(response));
            _md5Sum = md5Sum ?? throw new ArgumentNullException(nameof(md5Sum));
        }
        
        public static RosServiceInfo Create(RosType type, RosMessageTypeInfo request, RosMessageTypeInfo response)
        {
            var md5 = new Lazy<string>(() => CalculateMd5Sum(request, response), LazyThreadSafetyMode.None);
            return new RosServiceInfo(type, request, response, md5);
        }

        private static string CalculateMd5Sum(RosMessageTypeInfo request, RosMessageTypeInfo response)
        {
            var md5 = MD5.Create();

            using (var ms = new MemoryStream())
            {
                var writer = new StreamWriter(ms, Encoding.ASCII);

                request.WriteHashFields(writer);
                response.WriteHashFields(writer);

                writer.Flush();
                
                ms.Position = 0;
                var md5Bytes = md5.ComputeHash(ms);
                var md5String = md5Bytes.ToHexString();

                return md5String;
            }
        }
    }
}