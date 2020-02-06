using System;

namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    [Flags]
    public enum DetailedRosMessageType
    {
        None = 0,
        Message = 0x10,
        
        Service = 0x20,
        ServiceRequest = 0x21,
        ServiceResponse = 0x22,
        
        Action = 0x40,
        ActionGoal = 0x41,
        ActionResult = 0x42,
        ActionFeedback = 0x44
    }
}