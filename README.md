# ROS Message Essentials
> Mapping ROS messages to .Net Objects with ease

ROS message essentials provides the basic infrastructure for mapping ROS types to .Net objects.
It supports:
* Serialization and deserialization in the ROS specific binary message format
* Generation of ROS message
    * Message definitions
    * MD5 Sums
    
## Installation
ROS Message Parser for .Net is available as [NuGet Package](https://www.nuget.org/packages/RobSharper.Ros.MessageEssentials/).

![](https://img.shields.io/nuget/v/RobSharper.Ros.MessageEssentials.svg)


```
dotnet add package RobSharper.Ros.MessageEssentials
``` 

### Supported .NET versions

* **.NET Standard 2.0**
    * .NET Core 2.0 or later
    * .NET Framework 4.6.1 or later
    * Mono 5.4 or later
    * Xamarin.iOS 10.14 or later
    * Xamarin.Mac 3.8 or later
    * Xamarin.Android 8.0 or later
    * Universal Windows Platform 10.0.16299 or later

### Dependencies

* [ANTLR 4 .NET Standard Runtime](https://www.nuget.org/packages/Antlr4.Runtime.Standard/)


## Usage

### Mapping ROS types to .Net Objects

RobSharper.Ros.MessageEssentials allow the conversion between ROS messages and .Net objects.
You can manually create and annotate .Net classes to use them as ROS messages.

Here is an example for [geometry_msgs/Point](https://docs.ros.org/en/api/geometry_msgs/html/msg/Point.html) 
ROS message.

ROS message:
``` 
# This contains the position of a point in free space
float64 x
float64 y
float64 z 
```

.Net Class:
```c#
[RosMessage("geometry_msgs/Point")]
public class Point
{
    [RosMessageField("float64", "x", 1)]
    public double X { get; set; }
    
    [RosMessageField("float64", "y", 2)]
    public double Y { get; set; }
    
    [RosMessageField("float64", "z", 3)]
    public double Z { get; set; }
}
```

The Class is annotated with [`RosMessageAttribute`](RobSharper.Ros.MessageEssentials/RosMessageAttribute.cs) 
specifying the name of the ROS message type.

Public class properties are mapped to ROS message fields, if they are annotated with a 
[`RosMessageFieldAttribute`](RobSharper.Ros.MessageEssentials/RosMessageAttribute.cs). 
The mapped ROS type, the field name, and the position of the field in the ROS message can be specified. 
If the values are not set, datatype, field name and position are reflected from the property itself 
(it is assumed that fields have the same order as the properties are stated in the class).
The `RosMessageFieldAttribute` can be used on public properties or fields.
Properties must be readable (get) and writable (set).

The `RosMessageFieldAttribute` is also used for ROS constants. In this case the underlying field must be 
a `const` field:
```c#
[RosMessage("test_msgs/FieldVsConstant")]
public class LongConstant
{   
    [RosMessageField("int64", "MY_CONSTANT", 1)]
    public const long MY_CONSTANT = 99;
    
    [RosMessageField("int64", "my_field", 2)]
    public long MyField { get; set; }
}
```

#### Mapping of ROS data types to .Net data types

**Primitive types**

| ROS       | .Net      | Notes |
| --------- | --------- | ------|
| bool      | bool      |
| int8      | sbyte     |
| uint8     | byte      |
| int16     | short     |
| uint16    | ushort    |
| int32     | int       |
| uint32    | uint      |
| int64     | long      |
| uint64    | ulong     |
| float32   | float     |
| float64   | double    |
| string    | string    | Initialize fields with `string.Empty`. ROS middleware serialization does not support null values. |
| time      | DateTime  | [RosTime](RobSharper.Ros.MessageEssentials/RosTime.cs) is a helper struct to map between ROS time and DateTime. Initialize fields with `RosTime.Zero`.|
| duration  | TimeSpan  | [RosDuration](RobSharper.Ros.MessageEssentials/RosDuration.cs) is a helper struct to map between ROS duration and TimeSpan. | 

These are the default type mappings between ROS and .Net.
You can define alternative mappings by setting the `RosType` property of the 
`RosMessageFieldAttribute` explicitly. This works, as long as types are convertible
via [Convert.ChangeType()](https://docs.microsoft.com/en-us/dotnet/api/system.convert.changetype?view=netstandard-2.0).
Be aware that this might result in an OverflowException at runtime.

The following example maps a ROS int8 to a .Net int property. 
```c#
[RosMessage("equivalent_msgs/Int")]
public class IntMessage
{
    [RosMessageField("int8", "value", 1)]
    public int Value { get; set; }
}
```

Example of ROS message with default values
```c#
[RosMessage("test_msgs/Example")]
public class IntMessage
{
    // Value types have a default value
    [RosMessageField("int8", "IntValue", 1)]
    public int IntValue { get; set; }
    
    // strings should be set to string.Empty (null is not allowed for ROS serialization)
    [RosMessageField("int8", "IntValue", 2)]
    public string StringValue { get; set; } = string.Empty;
    
    // A default DateTime (January 1, 0001) is not the same as the default ROS time (January 1, 1970)
    [RosMessageField("time", "TimeValue", 3)]
    public DateTime TimeValue { get; set; } = RosTime.Zero;  
}
```

**Arrays**

Arrays are mapped to `IList<T>`, but it also accepts `List<T>`, `IEnumerable<T>` or `ICollection<T>`.
For fixed size ROS arrays make sure, that the list contains exactly the required number of elements,
otherwise serialization will throw an exception. 
RobSharper offers is a `PopulateWithInitializedRosValues` extension method defined on `ICollection<T>` for
initializing fixed size ROS arrays.

Remember:
 * ROS does not support null values. Make sure to initialize your arrays. 
 * As for all properties annotated with `RosMessageFieldAttribute`, they must be public, readable (get) and
writable (set).


Variable size ROS array example:
```c#
[RosMessage("test_msgs/IntArray")]
public class SimpleIntArray
{
    [RosMessageField("int32[]", "values", 1)]
    public IList<int> Values { get; set; } = new List<int>();
}
```

Fixed size ROS array example:
```c#
[RosMessage("test_msgs/IntArray")]
public class SimpleFixedSizeIntArray
{
    [RosMessageField("int32[5]", "values", 1)]
    public IList<int> Values { get; set; } = new List<int>();
    
    public SimpleFixedSizeIntArray()
    {
        // Populate list with 5 default values.
        Values.PopulateWithInitializedRosValues(5);
    }
}
```

**Nested Types**

Of course, a message type can contain other message types.
As ROS messages do not support null values, you should initialize
nested fields with a default value.

```c#
[RosMessage("test_msgs/IntValue")]
public class IntValue
{
    [RosMessageField("int32", "value", 1)]
    public int Value { get; set; }
}

[RosMessage("test_msgs/NestedIntValue")]
public class NestedIntValue
{
    [RosMessageField("test_msgs/IntValue", "a", 1)]
    public IntValue A { get; set; } = new SimpleInt();
}
```

**Enumerations**

While ROS messages do not support enumerations directly, they are
often realized with the help of constants.
You can map these to .Net Enums.

The following example shows the [`actionlib_msgs/GoalStatus`](http://docs.ros.org/en/api/actionlib_msgs/html/msg/GoalStatus.html)
message and it's mapping to a .Net enum.

ROS:
```
GoalID goal_id
uint8 status
uint8 PENDING         = 0   # The goal has yet to be processed by the action server
uint8 ACTIVE          = 1   # The goal is currently being processed by the action server
uint8 PREEMPTED       = 2   # The goal received a cancel request after it started executing
                            #   and has since completed its execution (Terminal State)
uint8 SUCCEEDED       = 3   # The goal was achieved successfully by the action server (Terminal State)
uint8 ABORTED         = 4   # The goal was aborted during execution by the action server due
                            #    to some failure (Terminal State)
uint8 REJECTED        = 5   # The goal was rejected by the action server without being processed,
                            #    because the goal was unattainable or invalid (Terminal State)
uint8 PREEMPTING      = 6   # The goal received a cancel request after it started executing
                            #    and has not yet completed execution
uint8 RECALLING       = 7   # The goal received a cancel request before it started executing,
                            #    but the action server has not yet confirmed that the goal is canceled
uint8 RECALLED        = 8   # The goal received a cancel request before it started executing
                            #    and was successfully cancelled (Terminal State)
uint8 LOST            = 9   # An action client can determine that a goal is LOST. This should not be
                            #    sent over the wire by an action server

#Allow for the user to associate a string with GoalStatus for debugging
string text
```

.Net:
```c#
public enum GoalStatusValue
{
    Pending = 0,
    Active = 1,
    Preempted = 2,
    Succeeded = 3,
    Aborted = 4,
    Rejected = 5,
    Preempting = 6,
    Recalling = 7,
    Recalled = 8,
    Lost = 9
}

[RosMessage("actionlib_msgs/GoalStatus")]
public class EnumGoalStatus
{
    [RosMessageField("actionlib_msgs/GoalID", "goal_id", 1)]
    public GoalID GoalId { get; set; }
    
    [RosMessageField("uint8", "status", 2)]
    public GoalStatusValue Status { get; set; }
    
    [RosMessageField("string", "text", 13)]
    public string Text { get; set; }
    
    

    [RosMessageField("uint8", "PENDING", 3)]
    public const GoalStatusValue Pending = GoalStatusValue.Pending;
    
    [RosMessageField("uint8", "ACTIVE", 4)]
    public const GoalStatusValue Active = GoalStatusValue.Active;
    
    [RosMessageField("uint8", "PREEMPTED", 5)]
    public const GoalStatusValue Preempted = GoalStatusValue.Preempted;
    
    [RosMessageField("uint8", "SUCCEEDED", 6)]
    public const GoalStatusValue Succeeded = GoalStatusValue.Succeeded;
    
    [RosMessageField("uint8", "ABORTED", 7)]
    public const GoalStatusValue Aborted = GoalStatusValue.Aborted;
    
    [RosMessageField("uint8", "REJECTED", 8)]
    public const GoalStatusValue Rejected = GoalStatusValue.Rejected;
    
    [RosMessageField("uint8", "PREEMPTING", 9)]
    public const GoalStatusValue Preempting = GoalStatusValue.Preempting;
    
    [RosMessageField("uint8", "RECALLING", 10)]
    public const GoalStatusValue Recalling = GoalStatusValue.Recalling;
    
    [RosMessageField("uint8", "RECALLED", 11)]
    public const GoalStatusValue Recalled = GoalStatusValue.Recalled;
    
    [RosMessageField("uint8", "LOST", 12)]
    public const GoalStatusValue Lost = GoalStatusValue.Lost;
}
```

### Serialization

The annotated types can now be used for serialization and deserialization to binary streams in
the ROS binary format.

```c#
public void SerializePoint(Stream stream)
{
    var typeRegistry = new MessageTypeRegistry();
    var serializer = new RosMessageSerializer(typeRegistry);

    var point = new Point
    {
        X = 1.0,
        Y = 2.0,
        Z = 0.5
    };

    serializer.Serialize(point, stream);
}

public Point DeserializePoint(Stream stream)
{
    var typeRegistry = new MessageTypeRegistry();
    var serializer = new RosMessageSerializer(typeRegistry);

    var point = serializer.Deserialize<Point>(stream);
    return point;
}
```

The `MessageTypeRegistry` is responsible for extracting the ROS message definition form .Net types.
A `RosMessageSerializer` converts between the binary ROS message format and .Net objects.
Both can be reused and must not be newly created for every operation. In fact, it is most efficient 
if only one global type registry exists, because extracted message definitions are cached. 
The Serializer can read and write all ROS types known to the type registry and is not bound to a specific one.
Both classes are thread safe and designed for parallel use.


### ROS Message definition and MD5 sum 

You can retrieve [`IRosMessageTypeInfo`](RobSharper.Ros.MessageEssentials/IRosMessageTypeInfo.cs) objects 
for a .Net type or ROS message type. A type info object can calculate the ROS message definition and MD5 sum.

```c#
public void PrintMessageInfo()
{
      var messageType = typeof(Point);
      var registry = new MessageTypeRegistry();
      var typeInfo = registry.GetOrCreateMessageTypeInfo(messageType);
      
      Console.WriteLine("Message Definition:");
      Console.WriteLine(typeInfo.MessageDefinition);
      
      Console.WriteLine("Message MD5: " + typeInfo.MD5Sum);
}
```

## Extensibility

Using annotated .Net types is the default way of representing ROS message types in .Net.
But it is not the only way!

How a ROS message looks like is defined by a [`IRosMessageTypeInfo`](RobSharper.Ros.MessageEssentials/IRosMessageTypeInfo.cs).
A MessageTypeInfo object is created by a [`IRosMessageTypeInfoFactory`](RobSharper.Ros.MessageEssentials/IRosMessageTypeInfoFactory.cs).

A [`MessageTypeRegistry`](RobSharper.Ros.MessageEssentials/MessageTypeRegistry.cs) stores the mappings between 
ROS type names and .Net types. It uses a set of `IRosMessageTypeInfoFactory` to extract the ROS message info. 
You can register custom factories in a registry.
See the [`AttributedMessageTypeInfoFactory`](RobSharper.Ros.MessageEssentials/AttributedMessageTypeInfoFactory.cs) 
class for the implementation of attribute based discovery. 


A similar concept exists for the [`RosMessageSerializer`](RobSharper.Ros.MessageEssentials/Serialization/RosMessageSerializer.cs).
It uses a set of [`IRosMessageFormatter`](RobSharper.Ros.MessageEssentials/Serialization/IRosMessageFormatter.cs)
objects to serialize objects based on the `IRosMessageTypeInfo`.
See [`DescriptorBasedMessageFormatter`](RobSharper.Ros.MessageEssentials/Serialization/RosMessageFormatter.cs)
for the default implementation.


## License

This project is licensed under the [BSD 3-clause](LICENSE) license. [Learn more](https://choosealicense.com/licenses/bsd-3-clause/)

