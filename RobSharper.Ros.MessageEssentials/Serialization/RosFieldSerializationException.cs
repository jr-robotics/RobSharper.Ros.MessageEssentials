using System;
using System.Text;

namespace RobSharper.Ros.MessageEssentials.Serialization
{
    public class RosFieldSerializationException : Exception
    {
        private readonly SerializationOperation _operation;

        public enum SerializationOperation
        {
            Serialize,
            Deserialize
        };

        public string Identifier
        {
            get;
            private set;
        }

        public override string Message
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append("Could not ");
                
                switch (_operation)
                {
                    case SerializationOperation.Serialize:
                        sb.Append("serialize");
                        break;
                    case SerializationOperation.Deserialize:
                        sb.Append("deserialize");
                        break;
                    default:
                        sb.Append("<undefined>");
                        break;
                }

                sb.Append(" field '");
                sb.Append(Identifier);
                sb.Append("'");

                var innerMessage = InnerException?.Message;

                if (!string.IsNullOrEmpty(innerMessage))
                {
                    sb.Append(": ");
                    sb.Append(innerMessage);
                }

                return sb.ToString();
            }
        }

        public RosFieldSerializationException(SerializationOperation operation, string rosFieldIdentifier, Exception innerException) : base(null, innerException)
        {
            _operation = operation;
            Identifier = rosFieldIdentifier ?? string.Empty;
        }

        public void AddLeadingRosIdentifier(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                return;

            if (Identifier == string.Empty)
                Identifier = identifier;
            else if (Identifier.StartsWith("["))
                Identifier = identifier + Identifier;
            else
                Identifier = identifier + "." + Identifier;
        }
    }
}