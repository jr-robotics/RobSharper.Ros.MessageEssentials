using System;

namespace RobSharper.Ros.MessageEssentials
{
    public abstract class RosMessageFieldDescriptor
    {
        public int Index { get;}
        public RosType RosType { get; }
        public string RosIdentifier { get; }
        public abstract Type Type { get; }
        
        protected RosMessageFieldDescriptor(int index, RosType rosType, string rosIdentifier)
        {
            Index = index;
            RosType = rosType ?? throw new ArgumentNullException(nameof(rosType));
            RosIdentifier = rosIdentifier ?? throw new ArgumentNullException(nameof(rosIdentifier));
        }

        public override string ToString()
        {
            return $"{RosType} {RosIdentifier}";
        }

        public abstract object GetValue(object o);
        public abstract void SetValue(object obj, object value);
    }
}