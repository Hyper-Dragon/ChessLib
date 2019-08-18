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
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.2")]
[System.CLSCompliant(false)]
internal partial class PGNParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		WHITE_WINS=1, BLACK_WINS=2, DRAWN_GAME=3, SECTION_MARKER=4, REST_OF_LINE_COMMENT=5, 
		BRACE_COMMENT=6, ESCAPE=7, SPACES=8, STRING=9, INTEGER=10, PERIOD=11, 
		TRIP_PERIOD=12, ASTERISK=13, LEFT_BRACKET=14, RIGHT_BRACKET=15, LEFT_PARENTHESIS=16, 
		RIGHT_PARENTHESIS=17, LEFT_ANGLE_BRACKET=18, RIGHT_ANGLE_BRACKET=19, NUMERIC_ANNOTATION_GLYPH=20, 
		SYMBOL=21, SUFFIX_ANNOTATION=22, UNEXPECTED_CHAR=23;
	public const int
		RULE_parse = 0, RULE_pgn_database = 1, RULE_pgn_game = 2, RULE_tag_section = 3, 
		RULE_tag_pair = 4, RULE_tag_name = 5, RULE_tag_value = 6, RULE_movetext_section = 7, 
		RULE_element_sequence = 8, RULE_element = 9, RULE_move_number_indication = 10, 
		RULE_nag = 11, RULE_comment = 12, RULE_san_move = 13, RULE_recursive_variation = 14, 
		RULE_game_termination = 15;
	public static readonly string[] ruleNames = {
		"parse", "pgn_database", "pgn_game", "tag_section", "tag_pair", "tag_name", 
		"tag_value", "movetext_section", "element_sequence", "element", "move_number_indication", 
		"nag", "comment", "san_move", "recursive_variation", "game_termination"
	};

	private static readonly string[] _LiteralNames = {
		null, "'1-0'", "'0-1'", "'1/2-1/2'", "'\r\n\r\n'", null, null, null, null, 
		null, null, "'.'", "'...'", "'*'", "'['", "']'", "'('", "')'", "'<'", 
		"'>'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "WHITE_WINS", "BLACK_WINS", "DRAWN_GAME", "SECTION_MARKER", "REST_OF_LINE_COMMENT", 
		"BRACE_COMMENT", "ESCAPE", "SPACES", "STRING", "INTEGER", "PERIOD", "TRIP_PERIOD", 
		"ASTERISK", "LEFT_BRACKET", "RIGHT_BRACKET", "LEFT_PARENTHESIS", "RIGHT_PARENTHESIS", 
		"LEFT_ANGLE_BRACKET", "RIGHT_ANGLE_BRACKET", "NUMERIC_ANNOTATION_GLYPH", 
		"SYMBOL", "SUFFIX_ANNOTATION", "UNEXPECTED_CHAR"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "PGN.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static PGNParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}

		public PGNParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public PGNParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	internal partial class ParseContext : ParserRuleContext {
		public Pgn_databaseContext pgn_database() {
			return GetRuleContext<Pgn_databaseContext>(0);
		}
		public ITerminalNode Eof() { return GetToken(PGNParser.Eof, 0); }
		public ParseContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_parse; } }
		public override void EnterRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.EnterParse(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.ExitParse(this);
		}
	}

	[RuleVersion(0)]
	public ParseContext parse() {
		ParseContext _localctx = new ParseContext(Context, State);
		EnterRule(_localctx, 0, RULE_parse);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 32; pgn_database();
			State = 33; Match(Eof);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	internal partial class Pgn_databaseContext : ParserRuleContext {
		public Pgn_gameContext[] pgn_game() {
			return GetRuleContexts<Pgn_gameContext>();
		}
		public Pgn_gameContext pgn_game(int i) {
			return GetRuleContext<Pgn_gameContext>(i);
		}
		public Pgn_databaseContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_pgn_database; } }
		public override void EnterRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.EnterPgn_database(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.ExitPgn_database(this);
		}
	}

	[RuleVersion(0)]
	public Pgn_databaseContext pgn_database() {
		Pgn_databaseContext _localctx = new Pgn_databaseContext(Context, State);
		EnterRule(_localctx, 2, RULE_pgn_database);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 38;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==SECTION_MARKER || _la==LEFT_BRACKET) {
				{
				{
				State = 35; pgn_game();
				}
				}
				State = 40;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	internal partial class Pgn_gameContext : ParserRuleContext {
		public Tag_sectionContext tag_section() {
			return GetRuleContext<Tag_sectionContext>(0);
		}
		public Movetext_sectionContext movetext_section() {
			return GetRuleContext<Movetext_sectionContext>(0);
		}
		public Pgn_gameContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_pgn_game; } }
		public override void EnterRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.EnterPgn_game(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.ExitPgn_game(this);
		}
	}

	[RuleVersion(0)]
	public Pgn_gameContext pgn_game() {
		Pgn_gameContext _localctx = new Pgn_gameContext(Context, State);
		EnterRule(_localctx, 4, RULE_pgn_game);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 41; tag_section();
			State = 42; movetext_section();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	internal partial class Tag_sectionContext : ParserRuleContext {
		public ITerminalNode SECTION_MARKER() { return GetToken(PGNParser.SECTION_MARKER, 0); }
		public Tag_pairContext[] tag_pair() {
			return GetRuleContexts<Tag_pairContext>();
		}
		public Tag_pairContext tag_pair(int i) {
			return GetRuleContext<Tag_pairContext>(i);
		}
		public Tag_sectionContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_tag_section; } }
		public override void EnterRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.EnterTag_section(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.ExitTag_section(this);
		}
	}

	[RuleVersion(0)]
	public Tag_sectionContext tag_section() {
		Tag_sectionContext _localctx = new Tag_sectionContext(Context, State);
		EnterRule(_localctx, 6, RULE_tag_section);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 47;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==LEFT_BRACKET) {
				{
				{
				State = 44; tag_pair();
				}
				}
				State = 49;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 50; Match(SECTION_MARKER);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	internal partial class Tag_pairContext : ParserRuleContext {
		public ITerminalNode LEFT_BRACKET() { return GetToken(PGNParser.LEFT_BRACKET, 0); }
		public Tag_nameContext tag_name() {
			return GetRuleContext<Tag_nameContext>(0);
		}
		public Tag_valueContext tag_value() {
			return GetRuleContext<Tag_valueContext>(0);
		}
		public ITerminalNode RIGHT_BRACKET() { return GetToken(PGNParser.RIGHT_BRACKET, 0); }
		public Tag_pairContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_tag_pair; } }
		public override void EnterRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.EnterTag_pair(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.ExitTag_pair(this);
		}
	}

	[RuleVersion(0)]
	public Tag_pairContext tag_pair() {
		Tag_pairContext _localctx = new Tag_pairContext(Context, State);
		EnterRule(_localctx, 8, RULE_tag_pair);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 52; Match(LEFT_BRACKET);
			State = 53; tag_name();
			State = 54; tag_value();
			State = 55; Match(RIGHT_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	internal partial class Tag_nameContext : ParserRuleContext {
		public ITerminalNode SYMBOL() { return GetToken(PGNParser.SYMBOL, 0); }
		public Tag_nameContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_tag_name; } }
		public override void EnterRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.EnterTag_name(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.ExitTag_name(this);
		}
	}

	[RuleVersion(0)]
	public Tag_nameContext tag_name() {
		Tag_nameContext _localctx = new Tag_nameContext(Context, State);
		EnterRule(_localctx, 10, RULE_tag_name);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 57; Match(SYMBOL);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	internal partial class Tag_valueContext : ParserRuleContext {
		public ITerminalNode STRING() { return GetToken(PGNParser.STRING, 0); }
		public Tag_valueContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_tag_value; } }
		public override void EnterRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.EnterTag_value(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.ExitTag_value(this);
		}
	}

	[RuleVersion(0)]
	public Tag_valueContext tag_value() {
		Tag_valueContext _localctx = new Tag_valueContext(Context, State);
		EnterRule(_localctx, 12, RULE_tag_value);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 59; Match(STRING);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	internal partial class Movetext_sectionContext : ParserRuleContext {
		public Element_sequenceContext element_sequence() {
			return GetRuleContext<Element_sequenceContext>(0);
		}
		public Game_terminationContext game_termination() {
			return GetRuleContext<Game_terminationContext>(0);
		}
		public ITerminalNode SECTION_MARKER() { return GetToken(PGNParser.SECTION_MARKER, 0); }
		public Movetext_sectionContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_movetext_section; } }
		public override void EnterRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.EnterMovetext_section(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.ExitMovetext_section(this);
		}
	}

	[RuleVersion(0)]
	public Movetext_sectionContext movetext_section() {
		Movetext_sectionContext _localctx = new Movetext_sectionContext(Context, State);
		EnterRule(_localctx, 14, RULE_movetext_section);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 61; element_sequence();
			State = 62; game_termination();
			State = 63; Match(SECTION_MARKER);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	internal partial class Element_sequenceContext : ParserRuleContext {
		public ElementContext[] element() {
			return GetRuleContexts<ElementContext>();
		}
		public ElementContext element(int i) {
			return GetRuleContext<ElementContext>(i);
		}
		public Recursive_variationContext[] recursive_variation() {
			return GetRuleContexts<Recursive_variationContext>();
		}
		public Recursive_variationContext recursive_variation(int i) {
			return GetRuleContext<Recursive_variationContext>(i);
		}
		public Element_sequenceContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_element_sequence; } }
		public override void EnterRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.EnterElement_sequence(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.ExitElement_sequence(this);
		}
	}

	[RuleVersion(0)]
	public Element_sequenceContext element_sequence() {
		Element_sequenceContext _localctx = new Element_sequenceContext(Context, State);
		EnterRule(_localctx, 16, RULE_element_sequence);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 69;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << BRACE_COMMENT) | (1L << INTEGER) | (1L << LEFT_PARENTHESIS) | (1L << NUMERIC_ANNOTATION_GLYPH) | (1L << SYMBOL))) != 0)) {
				{
				State = 67;
				ErrorHandler.Sync(this);
				switch (TokenStream.LA(1)) {
				case BRACE_COMMENT:
				case INTEGER:
				case NUMERIC_ANNOTATION_GLYPH:
				case SYMBOL:
					{
					State = 65; element();
					}
					break;
				case LEFT_PARENTHESIS:
					{
					State = 66; recursive_variation();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				State = 71;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	internal partial class ElementContext : ParserRuleContext {
		public Move_number_indicationContext move_number_indication() {
			return GetRuleContext<Move_number_indicationContext>(0);
		}
		public San_moveContext san_move() {
			return GetRuleContext<San_moveContext>(0);
		}
		public NagContext nag() {
			return GetRuleContext<NagContext>(0);
		}
		public CommentContext comment() {
			return GetRuleContext<CommentContext>(0);
		}
		public ElementContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_element; } }
		public override void EnterRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.EnterElement(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.ExitElement(this);
		}
	}

	[RuleVersion(0)]
	public ElementContext element() {
		ElementContext _localctx = new ElementContext(Context, State);
		EnterRule(_localctx, 18, RULE_element);
		try {
			State = 76;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case INTEGER:
				EnterOuterAlt(_localctx, 1);
				{
				State = 72; move_number_indication();
				}
				break;
			case SYMBOL:
				EnterOuterAlt(_localctx, 2);
				{
				State = 73; san_move();
				}
				break;
			case NUMERIC_ANNOTATION_GLYPH:
				EnterOuterAlt(_localctx, 3);
				{
				State = 74; nag();
				}
				break;
			case BRACE_COMMENT:
				EnterOuterAlt(_localctx, 4);
				{
				State = 75; comment();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	internal partial class Move_number_indicationContext : ParserRuleContext {
		public ITerminalNode INTEGER() { return GetToken(PGNParser.INTEGER, 0); }
		public ITerminalNode PERIOD() { return GetToken(PGNParser.PERIOD, 0); }
		public ITerminalNode TRIP_PERIOD() { return GetToken(PGNParser.TRIP_PERIOD, 0); }
		public Move_number_indicationContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_move_number_indication; } }
		public override void EnterRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.EnterMove_number_indication(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.ExitMove_number_indication(this);
		}
	}

	[RuleVersion(0)]
	public Move_number_indicationContext move_number_indication() {
		Move_number_indicationContext _localctx = new Move_number_indicationContext(Context, State);
		EnterRule(_localctx, 20, RULE_move_number_indication);
		int _la;
		try {
			State = 84;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,6,Context) ) {
			case 1:
				EnterOuterAlt(_localctx, 1);
				{
				{
				State = 78; Match(INTEGER);
				State = 80;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				if (_la==PERIOD) {
					{
					State = 79; Match(PERIOD);
					}
				}

				}
				}
				break;
			case 2:
				EnterOuterAlt(_localctx, 2);
				{
				{
				State = 82; Match(INTEGER);
				State = 83; Match(TRIP_PERIOD);
				}
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	internal partial class NagContext : ParserRuleContext {
		public ITerminalNode NUMERIC_ANNOTATION_GLYPH() { return GetToken(PGNParser.NUMERIC_ANNOTATION_GLYPH, 0); }
		public NagContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_nag; } }
		public override void EnterRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.EnterNag(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.ExitNag(this);
		}
	}

	[RuleVersion(0)]
	public NagContext nag() {
		NagContext _localctx = new NagContext(Context, State);
		EnterRule(_localctx, 22, RULE_nag);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 86; Match(NUMERIC_ANNOTATION_GLYPH);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	internal partial class CommentContext : ParserRuleContext {
		public ITerminalNode BRACE_COMMENT() { return GetToken(PGNParser.BRACE_COMMENT, 0); }
		public CommentContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_comment; } }
		public override void EnterRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.EnterComment(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.ExitComment(this);
		}
	}

	[RuleVersion(0)]
	public CommentContext comment() {
		CommentContext _localctx = new CommentContext(Context, State);
		EnterRule(_localctx, 24, RULE_comment);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 88; Match(BRACE_COMMENT);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	internal partial class San_moveContext : ParserRuleContext {
		public ITerminalNode SYMBOL() { return GetToken(PGNParser.SYMBOL, 0); }
		public San_moveContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_san_move; } }
		public override void EnterRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.EnterSan_move(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.ExitSan_move(this);
		}
	}

	[RuleVersion(0)]
	public San_moveContext san_move() {
		San_moveContext _localctx = new San_moveContext(Context, State);
		EnterRule(_localctx, 26, RULE_san_move);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 90; Match(SYMBOL);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	internal partial class Recursive_variationContext : ParserRuleContext {
		public ITerminalNode LEFT_PARENTHESIS() { return GetToken(PGNParser.LEFT_PARENTHESIS, 0); }
		public Element_sequenceContext element_sequence() {
			return GetRuleContext<Element_sequenceContext>(0);
		}
		public ITerminalNode RIGHT_PARENTHESIS() { return GetToken(PGNParser.RIGHT_PARENTHESIS, 0); }
		public Recursive_variationContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_recursive_variation; } }
		public override void EnterRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.EnterRecursive_variation(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.ExitRecursive_variation(this);
		}
	}

	[RuleVersion(0)]
	public Recursive_variationContext recursive_variation() {
		Recursive_variationContext _localctx = new Recursive_variationContext(Context, State);
		EnterRule(_localctx, 28, RULE_recursive_variation);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 92; Match(LEFT_PARENTHESIS);
			State = 93; element_sequence();
			State = 94; Match(RIGHT_PARENTHESIS);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	internal partial class Game_terminationContext : ParserRuleContext {
		public ITerminalNode WHITE_WINS() { return GetToken(PGNParser.WHITE_WINS, 0); }
		public ITerminalNode BLACK_WINS() { return GetToken(PGNParser.BLACK_WINS, 0); }
		public ITerminalNode DRAWN_GAME() { return GetToken(PGNParser.DRAWN_GAME, 0); }
		public ITerminalNode ASTERISK() { return GetToken(PGNParser.ASTERISK, 0); }
		public Game_terminationContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_game_termination; } }
		public override void EnterRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.EnterGame_termination(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IPGNListener typedListener = listener as IPGNListener;
			if (typedListener != null) typedListener.ExitGame_termination(this);
		}
	}

	[RuleVersion(0)]
	public Game_terminationContext game_termination() {
		Game_terminationContext _localctx = new Game_terminationContext(Context, State);
		EnterRule(_localctx, 30, RULE_game_termination);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 96;
			_la = TokenStream.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << WHITE_WINS) | (1L << BLACK_WINS) | (1L << DRAWN_GAME) | (1L << ASTERISK))) != 0)) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x3', '\x19', '\x65', '\x4', '\x2', '\t', '\x2', '\x4', '\x3', 
		'\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', '\x5', '\x4', 
		'\x6', '\t', '\x6', '\x4', '\a', '\t', '\a', '\x4', '\b', '\t', '\b', 
		'\x4', '\t', '\t', '\t', '\x4', '\n', '\t', '\n', '\x4', '\v', '\t', '\v', 
		'\x4', '\f', '\t', '\f', '\x4', '\r', '\t', '\r', '\x4', '\xE', '\t', 
		'\xE', '\x4', '\xF', '\t', '\xF', '\x4', '\x10', '\t', '\x10', '\x4', 
		'\x11', '\t', '\x11', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', 
		'\x3', '\a', '\x3', '\'', '\n', '\x3', '\f', '\x3', '\xE', '\x3', '*', 
		'\v', '\x3', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x5', '\a', 
		'\x5', '\x30', '\n', '\x5', '\f', '\x5', '\xE', '\x5', '\x33', '\v', '\x5', 
		'\x3', '\x5', '\x3', '\x5', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', 
		'\x3', '\x6', '\x3', '\x6', '\x3', '\a', '\x3', '\a', '\x3', '\b', '\x3', 
		'\b', '\x3', '\t', '\x3', '\t', '\x3', '\t', '\x3', '\t', '\x3', '\n', 
		'\x3', '\n', '\a', '\n', '\x46', '\n', '\n', '\f', '\n', '\xE', '\n', 
		'I', '\v', '\n', '\x3', '\v', '\x3', '\v', '\x3', '\v', '\x3', '\v', '\x5', 
		'\v', 'O', '\n', '\v', '\x3', '\f', '\x3', '\f', '\x5', '\f', 'S', '\n', 
		'\f', '\x3', '\f', '\x3', '\f', '\x5', '\f', 'W', '\n', '\f', '\x3', '\r', 
		'\x3', '\r', '\x3', '\xE', '\x3', '\xE', '\x3', '\xF', '\x3', '\xF', '\x3', 
		'\x10', '\x3', '\x10', '\x3', '\x10', '\x3', '\x10', '\x3', '\x11', '\x3', 
		'\x11', '\x3', '\x11', '\x2', '\x2', '\x12', '\x2', '\x4', '\x6', '\b', 
		'\n', '\f', '\xE', '\x10', '\x12', '\x14', '\x16', '\x18', '\x1A', '\x1C', 
		'\x1E', ' ', '\x2', '\x3', '\x4', '\x2', '\x3', '\x5', '\xF', '\xF', '\x2', 
		']', '\x2', '\"', '\x3', '\x2', '\x2', '\x2', '\x4', '(', '\x3', '\x2', 
		'\x2', '\x2', '\x6', '+', '\x3', '\x2', '\x2', '\x2', '\b', '\x31', '\x3', 
		'\x2', '\x2', '\x2', '\n', '\x36', '\x3', '\x2', '\x2', '\x2', '\f', ';', 
		'\x3', '\x2', '\x2', '\x2', '\xE', '=', '\x3', '\x2', '\x2', '\x2', '\x10', 
		'?', '\x3', '\x2', '\x2', '\x2', '\x12', 'G', '\x3', '\x2', '\x2', '\x2', 
		'\x14', 'N', '\x3', '\x2', '\x2', '\x2', '\x16', 'V', '\x3', '\x2', '\x2', 
		'\x2', '\x18', 'X', '\x3', '\x2', '\x2', '\x2', '\x1A', 'Z', '\x3', '\x2', 
		'\x2', '\x2', '\x1C', '\\', '\x3', '\x2', '\x2', '\x2', '\x1E', '^', '\x3', 
		'\x2', '\x2', '\x2', ' ', '\x62', '\x3', '\x2', '\x2', '\x2', '\"', '#', 
		'\x5', '\x4', '\x3', '\x2', '#', '$', '\a', '\x2', '\x2', '\x3', '$', 
		'\x3', '\x3', '\x2', '\x2', '\x2', '%', '\'', '\x5', '\x6', '\x4', '\x2', 
		'&', '%', '\x3', '\x2', '\x2', '\x2', '\'', '*', '\x3', '\x2', '\x2', 
		'\x2', '(', '&', '\x3', '\x2', '\x2', '\x2', '(', ')', '\x3', '\x2', '\x2', 
		'\x2', ')', '\x5', '\x3', '\x2', '\x2', '\x2', '*', '(', '\x3', '\x2', 
		'\x2', '\x2', '+', ',', '\x5', '\b', '\x5', '\x2', ',', '-', '\x5', '\x10', 
		'\t', '\x2', '-', '\a', '\x3', '\x2', '\x2', '\x2', '.', '\x30', '\x5', 
		'\n', '\x6', '\x2', '/', '.', '\x3', '\x2', '\x2', '\x2', '\x30', '\x33', 
		'\x3', '\x2', '\x2', '\x2', '\x31', '/', '\x3', '\x2', '\x2', '\x2', '\x31', 
		'\x32', '\x3', '\x2', '\x2', '\x2', '\x32', '\x34', '\x3', '\x2', '\x2', 
		'\x2', '\x33', '\x31', '\x3', '\x2', '\x2', '\x2', '\x34', '\x35', '\a', 
		'\x6', '\x2', '\x2', '\x35', '\t', '\x3', '\x2', '\x2', '\x2', '\x36', 
		'\x37', '\a', '\x10', '\x2', '\x2', '\x37', '\x38', '\x5', '\f', '\a', 
		'\x2', '\x38', '\x39', '\x5', '\xE', '\b', '\x2', '\x39', ':', '\a', '\x11', 
		'\x2', '\x2', ':', '\v', '\x3', '\x2', '\x2', '\x2', ';', '<', '\a', '\x17', 
		'\x2', '\x2', '<', '\r', '\x3', '\x2', '\x2', '\x2', '=', '>', '\a', '\v', 
		'\x2', '\x2', '>', '\xF', '\x3', '\x2', '\x2', '\x2', '?', '@', '\x5', 
		'\x12', '\n', '\x2', '@', '\x41', '\x5', ' ', '\x11', '\x2', '\x41', '\x42', 
		'\a', '\x6', '\x2', '\x2', '\x42', '\x11', '\x3', '\x2', '\x2', '\x2', 
		'\x43', '\x46', '\x5', '\x14', '\v', '\x2', '\x44', '\x46', '\x5', '\x1E', 
		'\x10', '\x2', '\x45', '\x43', '\x3', '\x2', '\x2', '\x2', '\x45', '\x44', 
		'\x3', '\x2', '\x2', '\x2', '\x46', 'I', '\x3', '\x2', '\x2', '\x2', 'G', 
		'\x45', '\x3', '\x2', '\x2', '\x2', 'G', 'H', '\x3', '\x2', '\x2', '\x2', 
		'H', '\x13', '\x3', '\x2', '\x2', '\x2', 'I', 'G', '\x3', '\x2', '\x2', 
		'\x2', 'J', 'O', '\x5', '\x16', '\f', '\x2', 'K', 'O', '\x5', '\x1C', 
		'\xF', '\x2', 'L', 'O', '\x5', '\x18', '\r', '\x2', 'M', 'O', '\x5', '\x1A', 
		'\xE', '\x2', 'N', 'J', '\x3', '\x2', '\x2', '\x2', 'N', 'K', '\x3', '\x2', 
		'\x2', '\x2', 'N', 'L', '\x3', '\x2', '\x2', '\x2', 'N', 'M', '\x3', '\x2', 
		'\x2', '\x2', 'O', '\x15', '\x3', '\x2', '\x2', '\x2', 'P', 'R', '\a', 
		'\f', '\x2', '\x2', 'Q', 'S', '\a', '\r', '\x2', '\x2', 'R', 'Q', '\x3', 
		'\x2', '\x2', '\x2', 'R', 'S', '\x3', '\x2', '\x2', '\x2', 'S', 'W', '\x3', 
		'\x2', '\x2', '\x2', 'T', 'U', '\a', '\f', '\x2', '\x2', 'U', 'W', '\a', 
		'\xE', '\x2', '\x2', 'V', 'P', '\x3', '\x2', '\x2', '\x2', 'V', 'T', '\x3', 
		'\x2', '\x2', '\x2', 'W', '\x17', '\x3', '\x2', '\x2', '\x2', 'X', 'Y', 
		'\a', '\x16', '\x2', '\x2', 'Y', '\x19', '\x3', '\x2', '\x2', '\x2', 'Z', 
		'[', '\a', '\b', '\x2', '\x2', '[', '\x1B', '\x3', '\x2', '\x2', '\x2', 
		'\\', ']', '\a', '\x17', '\x2', '\x2', ']', '\x1D', '\x3', '\x2', '\x2', 
		'\x2', '^', '_', '\a', '\x12', '\x2', '\x2', '_', '`', '\x5', '\x12', 
		'\n', '\x2', '`', '\x61', '\a', '\x13', '\x2', '\x2', '\x61', '\x1F', 
		'\x3', '\x2', '\x2', '\x2', '\x62', '\x63', '\t', '\x2', '\x2', '\x2', 
		'\x63', '!', '\x3', '\x2', '\x2', '\x2', '\t', '(', '\x31', '\x45', 'G', 
		'N', 'R', 'V',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace ChessLib.Parse.PGN.Parser.BaseClasses

