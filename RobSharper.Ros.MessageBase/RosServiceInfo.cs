using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RobSharper.Ros.MessageBase
{
    public class RosServiceInfo : IRosServiceInfo
    {
        private string _md5Sum;
        private readonly MessageTypeInfo _request;
        private readonly MessageTypeInfo _response;

        public RosType Type { get; }

        public IMessageTypeInfo Request => _request;

        public IMessageTypeInfo Response => _response;
        
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

        public RosServiceInfo(RosType type, MessageTypeInfo request, MessageTypeInfo response)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
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