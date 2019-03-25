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
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="PGNParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.2")]
[System.CLSCompliant(false)]
public interface IPGNVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.parse"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParse([NotNull] PGNParser.ParseContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.pgn_database"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPgn_database([NotNull] PGNParser.Pgn_databaseContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.pgn_game"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPgn_game([NotNull] PGNParser.Pgn_gameContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.tag_section"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTag_section([NotNull] PGNParser.Tag_sectionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.tag_pair"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTag_pair([NotNull] PGNParser.Tag_pairContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.tag_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTag_name([NotNull] PGNParser.Tag_nameContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.tag_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTag_value([NotNull] PGNParser.Tag_valueContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.movetext_section"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMovetext_section([NotNull] PGNParser.Movetext_sectionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.element_sequence"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitElement_sequence([NotNull] PGNParser.Element_sequenceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.element"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitElement([NotNull] PGNParser.ElementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.nag_item"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNag_item([NotNull] PGNParser.Nag_itemContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.move_number_indication"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMove_number_indication([NotNull] PGNParser.Move_number_indicationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.san_move"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSan_move([NotNull] PGNParser.San_moveContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.recursive_variation"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRecursive_variation([NotNull] PGNParser.Recursive_variationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="PGNParser.game_termination"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGame_termination([NotNull] PGNParser.Game_terminationContext context);
}
} // namespace ChessLib.Parse.PGN.Parser.BaseClasses
