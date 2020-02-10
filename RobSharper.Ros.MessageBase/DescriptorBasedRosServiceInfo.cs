using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace RobSharper.Ros.MessageBase
{
    public class DescriptorBasedRosServiceInfo : IRosServiceInfo
    {
        private readonly DescriptorBasedMessageTypeInfo _request;
        private readonly DescriptorBasedMessageTypeInfo _response;
        
        private string _md5Sum;

        public RosType RosType { get; }

        public IRosMessageTypeInfo Request => _request;

        public IRosMessageTypeInfo Response => _response;

        public string MD5Sum
        {
            get
            {
                if (_md5Sum == null)
                {
                    _md5Sum = CalculateMd5Sum();
                }

                return _md5Sum;
            }
        }

        public DescriptorBasedRosServiceInfo(RosType type, DescriptorBasedMessageTypeInfo request, DescriptorBasedMessageTypeInfo response)
        {
            RosType = type ?? throw new ArgumentNullException(nameof(type));
            _request = request ?? throw new ArgumentNullException(nameof(request));
            _response = response ?? throw new ArgumentNullException(nameof(response));
        }

        private string CalculateMd5Sum()
        {
            var md5 = MD5.Create();

            using (var ms = new MemoryStream())
            {
                var writer = new StreamWriter(ms, Encoding.ASCII);

                _request.WriteHashFields(writer);
                _response.WriteHashFields(writer);

                writer.Flush();
                
                ms.Position = 0;
                var md5Bytes = md5.ComputeHash(ms);
                var md5String = md5Bytes.ToHexString();

                return md5String;
            }
        }
    }
}