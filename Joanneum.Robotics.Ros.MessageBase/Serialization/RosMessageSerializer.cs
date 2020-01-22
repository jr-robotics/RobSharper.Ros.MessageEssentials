using System;
using System.IO;
using System.Runtime.Serialization;

namespace Joanneum.Robotics.Ros.MessageBase.Serialization
{
    public class RosMessageSerializer
    {
        public static TMessage Deserialize<TMessage>(Stream serializedMessage)
        {
            throw new NotImplementedException();
        }

        public static void Serialize(object message, Stream output)
        {
            throw new NotImplementedException();
        }
    }
}