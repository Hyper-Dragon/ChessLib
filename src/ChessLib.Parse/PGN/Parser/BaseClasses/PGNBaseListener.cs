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

using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IPGNListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.2")]
[System.CLSCompliant(false)]
internal partial class PGNBaseListener : IPGNListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.parse"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterParse([NotNull] PGNParser.ParseContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.parse"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitParse([NotNull] PGNParser.ParseContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.pgn_database"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPgn_database([NotNull] PGNParser.Pgn_databaseContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.pgn_database"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPgn_database([NotNull] PGNParser.Pgn_databaseContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.pgn_game"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPgn_game([NotNull] PGNParser.Pgn_gameContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.pgn_game"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPgn_game([NotNull] PGNParser.Pgn_gameContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.tag_section"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTag_section([NotNull] PGNParser.Tag_sectionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.tag_section"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTag_section([NotNull] PGNParser.Tag_sectionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.tag_pair"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTag_pair([NotNull] PGNParser.Tag_pairContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.tag_pair"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTag_pair([NotNull] PGNParser.Tag_pairContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.tag_name"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTag_name([NotNull] PGNParser.Tag_nameContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.tag_name"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTag_name([NotNull] PGNParser.Tag_nameContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.tag_value"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTag_value([NotNull] PGNParser.Tag_valueContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.tag_value"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTag_value([NotNull] PGNParser.Tag_valueContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.movetext_section"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMovetext_section([NotNull] PGNParser.Movetext_sectionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.movetext_section"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMovetext_section([NotNull] PGNParser.Movetext_sectionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.element_sequence"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterElement_sequence([NotNull] PGNParser.Element_sequenceContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.element_sequence"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitElement_sequence([NotNull] PGNParser.Element_sequenceContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.element"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterElement([NotNull] PGNParser.ElementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.element"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitElement([NotNull] PGNParser.ElementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.move_number_indication"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMove_number_indication([NotNull] PGNParser.Move_number_indicationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.move_number_indication"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMove_number_indication([NotNull] PGNParser.Move_number_indicationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.nag"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNag([NotNull] PGNParser.NagContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.nag"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNag([NotNull] PGNParser.NagContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.comment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterComment([NotNull] PGNParser.CommentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.comment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitComment([NotNull] PGNParser.CommentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.san_move"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSan_move([NotNull] PGNParser.San_moveContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.san_move"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSan_move([NotNull] PGNParser.San_moveContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.recursive_variation"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterRecursive_variation([NotNull] PGNParser.Recursive_variationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.recursive_variation"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitRecursive_variation([NotNull] PGNParser.Recursive_variationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.game_termination"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterGame_termination([NotNull] PGNParser.Game_terminationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.game_termination"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitGame_termination([NotNull] PGNParser.Game_terminationContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
} // namespace ChessLib.Parse.PGN.Parser.BaseClasses

