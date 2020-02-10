using System;
using System.Collections.Generic;
using System.Linq;

namespace RobSharper.Ros.MessageBase
{
    public class MessageTypeRegistry
    {
        private readonly IDictionary<Type, IRosMessageTypeInfo> _messageTypes = new Dictionary<Type, IRosMessageTypeInfo>();
        private readonly IDictionary<string, IRosMessageTypeInfo> _rosTypes = new Dictionary<string, IRosMessageTypeInfo>();
        
        public IList<IRosMessageTypeInfoFactory> RosMessageTypeInfoFactories { get; }

        public IRosMessageTypeInfo this[Type mappedType]
        {
            get => _messageTypes[mappedType];
        }

        public IRosMessageTypeInfo this[string rosTypeName]
        {
            get => _rosTypes[rosTypeName];
        }

        public MessageTypeRegistry()
        {
            RosMessageTypeInfoFactories = new List<IRosMessageTypeInfoFactory>
            {
                new AttributedMessageTypeInfoFactory(this)
            };
        }

        public bool IsRegistered(Type messageType)
        {
            return _messageTypes.ContainsKey(messageType);
        }

        public bool IsRegistered(RosType rosMessageType)
        {
            return IsRegistered(rosMessageType.ToString());
        }
        
        public bool IsRegistered(string rosMessageType)
        {
            return _rosTypes.ContainsKey(rosMessageType);
        }

        public void RegisterMessageTypeInfo(IRosMessageTypeInfo messageTypeInfo)
        {
            if (messageTypeInfo == null) throw new ArgumentNullException(nameof(messageTypeInfo));
            
            _messageTypes.Add(messageTypeInfo.Type, messageTypeInfo);
            _rosTypes.Add(messageTypeInfo.RosType.ToString(), messageTypeInfo);
        }

        public IRosMessageTypeInfo GetOrCreateMessageTypeInfo(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (_messageTypes.ContainsKey(type))
                return _messageTypes[type];

            var factoryCandidates = RosMessageTypeInfoFactories
                .Where(f => f.CanCreate(type))
                .ToList();
            
            if (!factoryCandidates.Any())
                throw new NotSupportedException($"No registered factory supports {type}");
            
            
            IRosMessageTypeInfo messageTypeInfo = null;
            var exceptions = new List<Exception>();
            
            foreach (var typeInfoFactory in factoryCandidates)
            {
                try
                {
                    messageTypeInfo = typeInfoFactory.Create(type);
                    break;
                }
                catch (Exception e)
                {
                    var ex = new RosMessageTypeInfoCreationException($"Could not create message type info for type {type}",
                        type, typeInfoFactory.GetType(), e);
                    
                    exceptions.Add(e);
                }
            }

            if (messageTypeInfo == null)
            {
                Exception innerException = null;

                if (exceptions.Count == 1)
                {
                    innerException = exceptions.First();
                }
                else if (exceptions.Count > 0)
                {
                    innerException = new AggregateException(exceptions);
                }
                
                throw new NotSupportedException($"No registered factory supports {type}", innerException);
            }

            RegisterMessageTypeInfo(messageTypeInfo);

            return messageTypeInfo;
        }
    }
}