namespace RobSharper.Ros.MessageEssentials.Tests.RosMessages
{
    public enum GoalStatusValue
    {
        /// <summary>
        /// The goal has yet to be processed by the action server
        /// </summary>
        Pending = 0,
        
        /// <summary>
        /// The goal is currently being processed by the action server
        /// </summary>
        Active = 1,
        
        /// <summary>
        /// The goal received a cancel request after it started executing
        /// and has since completed its execution (Terminal State)
        /// </summary>
        Preempted = 2,
        
        /// <summary>
        /// The goal was achieved successfully by the action server (Terminal State)
        /// </summary>
        Succeeded = 3,
        
        /// <summary>
        /// The goal was aborted during execution by the action server due
        /// to some failure (Terminal State)
        /// </summary>
        Aborted = 4,
        
        /// <summary>
        /// The goal was rejected by the action server without being processed,
        /// because the goal was unattainable or invalid (Terminal State)
        /// </summary>
        Rejected = 5,
        
        /// <summary>
        /// he goal received a cancel request after it started executing
        /// and has not yet completed execution
        /// </summary>
        Preempting = 6,
        
        /// <summary>
        /// The goal received a cancel request before it started executing,
        /// but the action server has not yet confirmed that the goal is canceled
        /// </summary>
        Recalling = 7,
        
        /// <summary>
        /// he goal received a cancel request before it started executing
        /// and was successfully cancelled (Terminal State)
        /// </summary>
        Recalled = 8,
        
        /// <summary>
        /// An action client can determine that a goal is LOST. This should not be
        /// sent over the wire by an action server
        /// </summary>
        Lost = 9
    }
    
    [RosMessage("actionlib_msgs/EnumGoalStatus")]
    public class EnumGoalStatus
    {
        
        // Result of "rosmsg md5"
        // TODO: verify
        public const string ROS_MD5 = "8cb38b482ac67b6c4b490371bee5a687";

        // Result of "gendeps --cat"
        // TODO: verify
        public const string MESSAGE_DEFINITION = @"uint8 PENDING=0
uint8 ACTIVE=1
uint8 PREEMPTED=2
uint8 SUCCEEDED=3
uint8 ABORTED=4
uint8 REJECTED=5
uint8 PREEMPTING=6
uint8 RECALLING=7
uint8 RECALLED=8
uint8 LOST=9
int32 goal_id
uint8 status
string text";
        
        
        [RosMessageField(1, "int32", "goal_id")]
        public int GoalId { get; set; }
        
        [RosMessageField(2, "uint8", "status")]
        public GoalStatusValue Status { get; set; }
        
        [RosMessageField(13, "string", "text")]
        public string Text { get; set; }
        
        

        [RosMessageField(3, "uint8", "PENDING")]
        public const GoalStatusValue Pending = GoalStatusValue.Pending;
        
        [RosMessageField(4, "uint8", "ACTIVE")]
        public const GoalStatusValue Active = GoalStatusValue.Active;
        
        [RosMessageField(5, "uint8", "PREEMPTED")]
        public const GoalStatusValue Preempted = GoalStatusValue.Preempted;
        
        [RosMessageField(6, "uint8", "SUCCEEDED")]
        public const GoalStatusValue Succeeded = GoalStatusValue.Succeeded;
        
        [RosMessageField(7, "uint8", "ABORTED")]
        public const GoalStatusValue Aborted = GoalStatusValue.Aborted;
        
        [RosMessageField(8, "uint8", "REJECTED")]
        public const GoalStatusValue Rejected = GoalStatusValue.Rejected;
        
        [RosMessageField(9, "uint8", "PREEMPTING")]
        public const GoalStatusValue Preempting = GoalStatusValue.Preempting;
        
        [RosMessageField(10, "uint8", "RECALLING")]
        public const GoalStatusValue Recalling = GoalStatusValue.Recalling;
        
        [RosMessageField(11, "uint8", "RECALLED")]
        public const GoalStatusValue Recalled = GoalStatusValue.Recalled;
        
        [RosMessageField(12, "uint8", "LOST")]
        public const GoalStatusValue Lost = GoalStatusValue.Lost;
    }
    
    [RosMessage("actionlib_msgs/GoalStatus")]
    public class GoalStatus
    {
        
        // Result of "rosmsg md5"
        // TODO: verify
        public const string ROS_MD5 = "8cb38b482ac67b6c4b490371bee5a687";

        // Result of "gendeps --cat"
        // TODO: verify
        public const string MESSAGE_DEFINITION = @"uint8 PENDING=0
uint8 ACTIVE=1
uint8 PREEMPTED=2
uint8 SUCCEEDED=3
uint8 ABORTED=4
uint8 REJECTED=5
uint8 PREEMPTING=6
uint8 RECALLING=7
uint8 RECALLED=8
uint8 LOST=9
int32 goal_id
uint8 status
string text";
        
        
        [RosMessageField(1, "int32", "goal_id")]
        public int GoalId { get; set; }
        
        [RosMessageField(2, "uint8", "status")]
        public byte Status { get; set; }
        
        [RosMessageField(13, "string", "text")]
        public string Text { get; set; }
        
        

        [RosMessageField(3, "uint8", "PENDING")]
        public const byte Pending = 0;
        
        [RosMessageField(4, "uint8", "ACTIVE")]
        public const byte Active = 1;
        
        [RosMessageField(5, "uint8", "PREEMPTED")]
        public const byte Preempted = 2;
        
        [RosMessageField(6, "uint8", "SUCCEEDED")]
        public const byte Succeeded = 3;
        
        [RosMessageField(7, "uint8", "ABORTED")]
        public const byte Aborted = 4;
        
        [RosMessageField(8, "uint8", "REJECTED")]
        public const byte Rejected = 5;
        
        [RosMessageField(9, "uint8", "PREEMPTING")]
        public const byte Preempting = 6;
        
        [RosMessageField(10, "uint8", "RECALLING")]
        public const byte Recalling = 7;
        
        [RosMessageField(11, "uint8", "RECALLED")]
        public const byte Recalled = 8;
        
        [RosMessageField(12, "uint8", "LOST")]
        public const byte Lost = 9;
    }
}