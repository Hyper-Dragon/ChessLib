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
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="PGNParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.2")]
[System.CLSCompliant(false)]
public interface IPGNListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.parse"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParse([NotNull] PGNParser.ParseContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.parse"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParse([NotNull] PGNParser.ParseContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.pgn_database"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPgn_database([NotNull] PGNParser.Pgn_databaseContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.pgn_database"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPgn_database([NotNull] PGNParser.Pgn_databaseContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.pgn_game"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPgn_game([NotNull] PGNParser.Pgn_gameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.pgn_game"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPgn_game([NotNull] PGNParser.Pgn_gameContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.tag_section"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTag_section([NotNull] PGNParser.Tag_sectionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.tag_section"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTag_section([NotNull] PGNParser.Tag_sectionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.tag_pair"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTag_pair([NotNull] PGNParser.Tag_pairContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.tag_pair"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTag_pair([NotNull] PGNParser.Tag_pairContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.tag_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTag_name([NotNull] PGNParser.Tag_nameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.tag_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTag_name([NotNull] PGNParser.Tag_nameContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.tag_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTag_value([NotNull] PGNParser.Tag_valueContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.tag_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTag_value([NotNull] PGNParser.Tag_valueContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.movetext_section"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMovetext_section([NotNull] PGNParser.Movetext_sectionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.movetext_section"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMovetext_section([NotNull] PGNParser.Movetext_sectionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.element_sequence"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterElement_sequence([NotNull] PGNParser.Element_sequenceContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.element_sequence"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitElement_sequence([NotNull] PGNParser.Element_sequenceContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.element"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterElement([NotNull] PGNParser.ElementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.element"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitElement([NotNull] PGNParser.ElementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.move_number_indication"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMove_number_indication([NotNull] PGNParser.Move_number_indicationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.move_number_indication"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMove_number_indication([NotNull] PGNParser.Move_number_indicationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.nag"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNag([NotNull] PGNParser.NagContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.nag"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNag([NotNull] PGNParser.NagContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.comment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterComment([NotNull] PGNParser.CommentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.comment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitComment([NotNull] PGNParser.CommentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.san_move"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSan_move([NotNull] PGNParser.San_moveContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.san_move"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSan_move([NotNull] PGNParser.San_moveContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.recursive_variation"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRecursive_variation([NotNull] PGNParser.Recursive_variationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.recursive_variation"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRecursive_variation([NotNull] PGNParser.Recursive_variationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="PGNParser.game_termination"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterGame_termination([NotNull] PGNParser.Game_terminationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="PGNParser.game_termination"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitGame_termination([NotNull] PGNParser.Game_terminationContext context);
}
} // namespace ChessLib.Parse.PGN.Parser.BaseClasses
