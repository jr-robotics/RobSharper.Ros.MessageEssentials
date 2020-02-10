using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RobSharper.Ros.MessageBase
{
    public class DescriptorBasedMessageTypeInfo : IRosMessageTypeInfo
    {
        private readonly RosMessageDescriptor _messageDescriptor;
        private readonly IEnumerable<IRosMessageTypeInfo> _dependencies;
        
        private string _md5Sum;

        public RosType RosType => _messageDescriptor.RosType;

        public Type Type { get; }

        public RosMessageDescriptor MessageDescriptor => _messageDescriptor;
        
        public IEnumerable<IRosMessageTypeInfo> Dependencies => _dependencies;

        public string MessageDefinition => _messageDescriptor.MessageDefinition;

        public bool HasHeader => _messageDescriptor.HasHader;

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
        
        public DescriptorBasedMessageTypeInfo(Type mappedMessageType, RosMessageDescriptor messageDescriptor, IEnumerable<IRosMessageTypeInfo> dependencies)
        {
            Type = mappedMessageType ?? throw new ArgumentNullException(nameof(mappedMessageType));
            _messageDescriptor = messageDescriptor ?? throw new ArgumentNullException(nameof(messageDescriptor));
            _dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
        }

        private string CalculateMd5Sum()
        {
            var md5 = MD5.Create();

            using (var ms = new MemoryStream())
            {
                var writer = new StreamWriter(ms, Encoding.ASCII);

                WriteHashFields(writer);
                writer.Flush();

                ms.Position = 0;
                var md5Bytes = md5.ComputeHash(ms);
                var md5String = md5Bytes.ToHexString();

                return md5String;
            }
        }

        internal void WriteHashFields(StreamWriter writer)
        {
            var firstElement = true;
            
            // MD5 of Constants
            foreach (var constant in _messageDescriptor.Constants)
            {
                if (firstElement)
                {
                    firstElement = false;
                }
                else
                {
                    writer.Write("\n");
                }

                // Only built in types supported for constants
                writer.Write(constant.ToString());
            }

            // MD5 Of Fields
            foreach (var field in _messageDescriptor.Fields)
            {
                if (firstElement)
                {
                    firstElement = false;
                }
                else
                {
                    writer.Write("\n");
                }

                if (field.RosType.IsBuiltIn)
                {
                    writer.Write(field.RosType);
                }
                else
                {
                    var typeInfo = _dependencies
                        .First(x => x.RosType.ToString("T") == field.RosType.ToString("T"));

                    var typeHash = typeInfo.MD5Sum;

                    writer.Write(typeHash);
                }

                writer.Write(" ");
                writer.Write(field.RosIdentifier);
            }
        }

        public override string ToString()
        {
            return RosType.ToString();
        }

        protected bool Equals(DescriptorBasedMessageTypeInfo other)
        {
            return Equals(_messageDescriptor, other._messageDescriptor) && Equals(_dependencies, other._dependencies);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DescriptorBasedMessageTypeInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_messageDescriptor != null ? _messageDescriptor.GetHashCode() : 0) * 397) ^ (_dependencies != null ? _dependencies.GetHashCode() : 0);
            }
        }
    }
}