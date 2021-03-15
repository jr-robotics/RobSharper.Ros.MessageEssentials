using System;

namespace RobSharper.Ros.MessageEssentials
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class RosMessageBaseAttribute : Attribute
    {
        public abstract string MessageName { get; }
        
        protected RosMessageBaseAttribute()
        {
        }
    }
}