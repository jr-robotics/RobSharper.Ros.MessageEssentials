using System;
using System.IO;

namespace Joanneum.Robotics.Ros.MessageBase
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