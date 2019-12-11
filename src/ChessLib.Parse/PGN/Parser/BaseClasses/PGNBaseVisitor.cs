//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from .\PGN.g4 by ANTLR 4.7.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace ChessLib.Parse.PGN.Parser.BaseClasses {

	#pragma warning disable 3021

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IPGNVisitor{Result}"/>,
/// which can be extended to create a visitor which only needs to handle a subset
/// of the available methods.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.2")]
[System.CLSCompliant(false)]
internal partial class PGNBaseVisitor<Result> : AbstractParseTreeVisitor<Result>, IPGNVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.parse"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitParse([NotNull] PGNParser.ParseContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.pgn_database"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitPgn_database([NotNull] PGNParser.Pgn_databaseContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.pgn_game"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitPgn_game([NotNull] PGNParser.Pgn_gameContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.tag_section"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitTag_section([NotNull] PGNParser.Tag_sectionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.tag_pair"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitTag_pair([NotNull] PGNParser.Tag_pairContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.tag_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitTag_name([NotNull] PGNParser.Tag_nameContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.tag_value"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitTag_value([NotNull] PGNParser.Tag_valueContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.movetext_section"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitMovetext_section([NotNull] PGNParser.Movetext_sectionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.element_sequence"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitElement_sequence([NotNull] PGNParser.Element_sequenceContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.element"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitElement([NotNull] PGNParser.ElementContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.move_number_indication"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitMove_number_indication([NotNull] PGNParser.Move_number_indicationContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.nag"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitNag([NotNull] PGNParser.NagContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.comment"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitComment([NotNull] PGNParser.CommentContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.san_move"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSan_move([NotNull] PGNParser.San_moveContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.recursive_variation"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitRecursive_variation([NotNull] PGNParser.Recursive_variationContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.game_termination"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitGame_termination([NotNull] PGNParser.Game_terminationContext context) { return VisitChildren(context); }
}
} // namespace ChessLib.Parse.PGN.Parser.BaseClasses

