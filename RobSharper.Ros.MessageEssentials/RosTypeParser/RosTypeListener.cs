//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.9.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from /home/brg/dev/RobSharper.Ros.MessageEssentials/RobSharper.Ros.MessageEssentials/RosType.g4 by ANTLR 4.9.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace RobSharper.Ros.MessageEssentials.RosTypeParser {
using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="RosTypeParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.9.1")]
[System.CLSCompliant(false)]
public interface IRosTypeListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="RosTypeParser.type_input"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterType_input([NotNull] RosTypeParser.Type_inputContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="RosTypeParser.type_input"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitType_input([NotNull] RosTypeParser.Type_inputContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="RosTypeParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterType([NotNull] RosTypeParser.TypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="RosTypeParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitType([NotNull] RosTypeParser.TypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="RosTypeParser.built_in_type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBuilt_in_type([NotNull] RosTypeParser.Built_in_typeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="RosTypeParser.built_in_type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBuilt_in_type([NotNull] RosTypeParser.Built_in_typeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="RosTypeParser.ros_type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRos_type([NotNull] RosTypeParser.Ros_typeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="RosTypeParser.ros_type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRos_type([NotNull] RosTypeParser.Ros_typeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="RosTypeParser.ros_package_type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRos_package_type([NotNull] RosTypeParser.Ros_package_typeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="RosTypeParser.ros_package_type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRos_package_type([NotNull] RosTypeParser.Ros_package_typeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="RosTypeParser.array_type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArray_type([NotNull] RosTypeParser.Array_typeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="RosTypeParser.array_type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArray_type([NotNull] RosTypeParser.Array_typeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="RosTypeParser.variable_array_type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariable_array_type([NotNull] RosTypeParser.Variable_array_typeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="RosTypeParser.variable_array_type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariable_array_type([NotNull] RosTypeParser.Variable_array_typeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="RosTypeParser.fixed_array_type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFixed_array_type([NotNull] RosTypeParser.Fixed_array_typeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="RosTypeParser.fixed_array_type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFixed_array_type([NotNull] RosTypeParser.Fixed_array_typeContext context);
}
} // namespace RobSharper.Ros.MessageEssentials.RosTypeParser
