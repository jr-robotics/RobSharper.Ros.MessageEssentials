using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RobSharper.Ros.MessageBase
{
    public class MessageTypeInfo : IMessageTypeInfo
    {
        private readonly RosMessageDescriptor _messageDescriptor;
        private readonly IEnumerable<IMessageTypeInfo> _dependencies;
        
        private string _md5Sum;
        private string _messageDefinition;

        public RosType Type => _messageDescriptor.RosType;

        public RosMessageDescriptor MessageDescriptor => _messageDescriptor;

        public IEnumerable<IMessageTypeInfo> Dependencies => _dependencies;

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

        public string MessageDefinition
        {
            get
            {
                if (_messageDefinition == null)
                {
                    _messageDefinition = CreateMessageDefinition();
                }
                
                return _messageDefinition;
            }
        }

        public bool HasHeader => _messageDescriptor.HasHader;
        
        public MessageTypeInfo(RosMessageDescriptor messageDescriptor, IEnumerable<IMessageTypeInfo> dependencies)
        {
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

        public void WriteHashFields(StreamWriter writer)
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
                        .First(x => x.MessageDescriptor.RosType.ToString("T") == field.RosType.ToString("T"));

                    var typeHash = typeInfo.MD5Sum;

                    writer.Write(typeHash);
                }

                writer.Write(" ");
                writer.Write(field.RosIdentifier);
            }
        }

        private string CreateMessageDefinition()
        {
            var dependencies = new List<IMessageTypeInfo>();
            CollectDependencies(this, dependencies);
            
            var sb = new StringBuilder();

            sb.Append(_messageDescriptor.MessageDefinition);

            foreach (var dependency in dependencies)
            {
                sb.Append("\n================================================================================\n");
                
                sb.Append("MSG: ");
                sb.Append(dependency.MessageDescriptor.RosType);
                sb.Append("\n");
                sb.Append(dependency.MessageDescriptor.MessageDefinition);
            }

            return sb.ToString();
        }

        private void CollectDependencies(IMessageTypeInfo messageType, List<IMessageTypeInfo> dependencies)
        {
            foreach (var candidate in messageType.Dependencies)
            {
                var candidateType = candidate.MessageDescriptor.RosType.ToString("T");
                
                if (dependencies.Any(x => x.MessageDescriptor.RosType.ToString("T") == candidateType))
                    continue;

                dependencies.Add(candidate);
                CollectDependencies(candidate, dependencies);
            }
        }

        public override string ToString()
        {
            return Type.ToString();
        }

        protected bool Equals(MessageTypeInfo other)
        {
            return Equals(_messageDescriptor, other._messageDescriptor) && Equals(_dependencies, other._dependencies);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MessageTypeInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_messageDescriptor != null ? _messageDescriptor.GetHashCode() : 0) * 397) ^ (_dependencies != null ? _dependencies.GetHashCode() : 0);
            }
        }
    }

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