﻿using ChessLib.Data;
using ChessLib.Data.Exceptions;
using ChessLib.Data.Helpers;
using ChessLib.Data.MoveRepresentation;
using ChessLib.Data.Types;
using ChessLib.MagicBitboard.MoveValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ChessLib.Data.Boards;
using ChessLib.MagicBitboard.MoveValidation.MoveRules;

namespace ChessLib.MagicBitboard
{
    public class BoardInfo : BoardInformationService<MoveHashStorage>
    {

        //public readonly MoveTree<MoveHashStorage> MoveTree = new MoveTree<MoveHashStorage>(null);
        public override MoveTree<MoveHashStorage> MoveTree { get; set; }
        public BoardInfo() : base(FENHelpers.FENInitial, false) { }

        public BoardInfo(string fen, bool is960 = false) : base(fen, is960) { }



        public override void ApplyMove(string moveText)
        {
            var move = GenerateMoveFromText(moveText);
            ApplyMove(move);
        }

        public MoveExceptionType? ApplyMove(MoveExt move)
        {
            GetPiecesAtSourceAndDestination(move, out var pocSource, out var pocDestination);
            if (pocSource == null) throw new ArgumentException("No piece at source.");
            var moveValidator = new MoveValidator(this, move);
            var validationError = moveValidator.Validate();
            if (validationError.HasValue)
                throw new MoveException("Error with move.", validationError.Value, move, ActivePlayerColor);
            var isCapture = (OpponentTotalOccupancy & move.DestinationValue) != 0;
            var isPawnMove = (ActivePawnOccupancy & move.SourceValue) != 0;
            if (isCapture || isPawnMove) HalfmoveClock = 0;
            else HalfmoveClock++;

            UnsetCastlingAvailability(move, pocSource.Value.Piece);
            SetEnPassantFlag(move, pocSource);

            var san = MoveToSAN(move, pocSource.Value, pocDestination);
            PiecesOnBoard = moveValidator.PostMoveBoard;
            MoveTree.AddLast(new MoveNode<MoveHashStorage>(new MoveHashStorage(move, pocSource.Value.Piece, ActivePlayerColor, ToFEN(), san)));

            if (ActivePlayerColor == Color.Black)
            {
                MoveCounter++;
                ActivePlayerColor = Color.White;
            }
            else
            {
                ActivePlayerColor = Color.Black;
            }
            return null;
        }

        private bool IsPlayerOfColorInCheck(Color c, ulong[][] board) =>
            Bitboard.IsAttackedBy(c.Toggle(), board[(int)c][KING].GetSetBits()[0], board);
        protected bool OpposingPlayerInCheck => IsAttackedBy(ActivePlayerColor, OpposingPlayerKingIndex);
        protected bool ActivePlayerInCheck => IsAttackedBy(OpponentColor, ActivePlayerKingIndex);
        protected override Check GetChecks(Color activePlayer)
        {
            var rv = Check.None;

            if (ActivePlayerInCheck && OpposingPlayerInCheck)
            {
                rv &= ~Check.None;
                rv |= Check.Double;
            }
            else if (OpposingPlayerInCheck)
            {
                rv &= ~Check.None;
                rv |= Check.Opposite;
            }
            else if (ActivePlayerInCheck)
            {
                rv &= ~Check.None;
                rv |= Check.Normal;
            }

            return rv;
        }

        private void GetPiecesAtSourceAndDestination(MoveExt move, out PieceOfColor? pocSource,
            out PieceOfColor? pocDestination)
        {
            var sVal = move.SourceValue;
            pocSource = null;
            pocDestination = null;
            foreach (Piece piece in Enum.GetValues(typeof(Piece)))
            {
                var p = (int)piece;
                if (pocSource == null)
                {
                    if ((PiecesOnBoard[(int)Color.White][p] & sVal) != 0)
                        pocSource = new PieceOfColor { Color = Color.White, Piece = piece };
                    if ((PiecesOnBoard[(int)Color.Black][p] & sVal) != 0)
                        pocSource = new PieceOfColor { Color = Color.Black, Piece = piece };
                }

                if (pocDestination == null)
                {
                    if ((PiecesOnBoard[(int)Color.White][p] & sVal) != 0)
                        pocDestination = new PieceOfColor { Color = Color.White, Piece = piece };
                    if ((PiecesOnBoard[(int)Color.Black][p] & sVal) != 0)
                        pocDestination = new PieceOfColor { Color = Color.Black, Piece = piece };
                }
            }
        }



        public static ulong XRayRookAttacks(ulong occupancy, ulong blockers, ushort squareIndex)
        {
            var rookMovesFromSquare = PieceAttackPatternHelper.RookMoveMask[squareIndex];
            //blockers &= rookMovesFromSquare;
            return rookMovesFromSquare ^ Bitboard.GetAttackedSquares(Piece.Rook, squareIndex, occupancy);
        }

        public static ulong XRayBishopAttacks(ulong occupancy, ulong blockers, ushort squareIndex)
        {
            var bishopMovesFromSquare = PieceAttackPatternHelper.BishopMoveMask[squareIndex];
            //blockers &= bishopMovesFromSquare;
            return bishopMovesFromSquare ^ Bitboard.GetAttackedSquares(Piece.Bishop, squareIndex, occupancy);
        }

        public bool IsPiecePinned(ulong pieceValue)
        {
            return (GetPinnedPieces() & pieceValue) != 0;
        }

        public bool IsPiecePinned(ushort indexOfSquare)
        {
            return IsPiecePinned(indexOfSquare.IndexToValue());
        }

        public ulong GetPinnedPieces()
        {
            ulong pinned = 0;
            var xRayBishopAttacks = XRayBishopAttacks(TotalOccupancy, ActiveTotalOccupancy, ActivePlayerKingIndex);
            var xRayRookAttacks = XRayRookAttacks(TotalOccupancy, ActiveTotalOccupancy, ActivePlayerKingIndex);
            var bishopPinnedPieces = (PiecesOnBoard[nOpponentColor][BISHOP] | PiecesOnBoard[nOpponentColor][QUEEN]) &
                                     xRayBishopAttacks;
            var rookPinnedPieces = (PiecesOnBoard[nOpponentColor][ROOK] | PiecesOnBoard[nOpponentColor][QUEEN]) &
                                   xRayRookAttacks;
            var allPins = rookPinnedPieces | bishopPinnedPieces;
            while (allPins != 0)
            {
                var square = BitHelpers.BitScanForward(allPins);
                var squaresBetween = BoardHelpers.InBetween(square, ActivePlayerKingIndex);
                var piecesBetween = squaresBetween & ActiveTotalOccupancy;
                if (piecesBetween.CountSetBits() == 1) pinned |= piecesBetween;
                allPins &= allPins - 1;
            }

            return pinned;
        }

        public MoveExt GenerateMoveFromText(string moveText)
        {
            var md = MoveHelpers.GetAvailableMoveDetails(moveText, ActivePlayerColor);
            if (!md.SourceFile.HasValue || !md.SourceRank.HasValue)
            {
                var sourceIndex = FindPieceSourceIndex(md);
                md.SourceIndex = sourceIndex;
            }

            Debug.Assert(md.SourceIndex != null, "md.SourceIndex != null");
            Debug.Assert(md.DestinationIndex != null, "md.DestinationIndex != null");
            if (md.IsCapture && md.Piece == Piece.Pawn &&
                (OpponentTotalOccupancy & (1ul << md.DestinationIndex)) == 0 &&
                (md.DestinationRank == 2 || md.DestinationRank == 5)) md.MoveType = MoveType.EnPassant;
            var moveExt = MoveHelpers.GenerateMove(md.SourceIndex.Value, md.DestinationIndex.Value, md.MoveType,
                md.PromotionPiece ?? 0);
            return moveExt;
        }


        public override ulong GetAttackedSquares(Piece p, ushort index, ulong occupancy)
        {
            return Bitboard.GetAttackedSquares(p, index, occupancy);
        }








        /// <summary>
        ///     Instance method to find if <paramref name="squareIndex" /> is attacked by a piece of <paramref name="color" />
        /// </summary>
        /// <param name="color">Color of possible attacker</param>
        /// <param name="squareIndex">Square which is possibly under attack</param>
        /// <returns>true if <paramref name="squareIndex" /> is attacked by <paramref name="color" />. False if not.</returns>
        public override bool IsAttackedBy(Color color, ushort squareIndex)
        {
            return Bitboard.IsAttackedBy(color, squareIndex, PiecesOnBoard);
        }

        public override bool DoesPieceAtSquareAttackSquare(ushort attackerSquare, ushort attackedSquare,
            Piece attackerPiece)
        {
            var attackedSquares = GetAttackedSquares(attackerPiece, attackerSquare, TotalOccupancy);
            var attackedValue = attackedSquare.GetBoardValueOfIndex();
            return (attackedSquares & attackedValue) != 0;
        }




        #region Occupancy / Index Properties for shorthand access to occupancy board arrays

        /// <summary>
        ///     Occupancy of side-to-move's pawns
        /// </summary>
        public ulong ActivePawnOccupancy => PiecesOnBoard[nActiveColor][PAWN];

        /// <summary>
        ///     Occupancy of side-to-move's Knights
        /// </summary>
        public ulong ActiveKnightOccupancy => PiecesOnBoard[nActiveColor][KNIGHT];

        /// <summary>
        ///     Occupancy of side-to-move's Bishops
        /// </summary>
        public ulong ActiveBishopOccupancy => PiecesOnBoard[nActiveColor][BISHOP];

        /// <summary>
        ///     Occupancy of side-to-move's Rooks
        /// </summary>
        public ulong ActiveRookOccupancy => PiecesOnBoard[nActiveColor][ROOK];

        /// <summary>
        ///     Occupancy of side-to-move's Queen(s)
        /// </summary>
        public ulong ActiveQueenOccupancy => PiecesOnBoard[nActiveColor][QUEEN];



        /// <summary>
        ///     Index of side-to-move's King
        /// </summary>
        public ushort ActivePlayerKingIndex => KingIndex(ActivePlayerColor);

        /// <summary>
        ///     Value (occupancy board) of side-to-move's King
        /// </summary>
        public ulong ActivePlayerKingOccupancy => PiecesOnBoard[nActiveColor][KING];

        /// <summary>
        ///     Opponent's King's square index
        /// </summary>
        public ushort OpposingPlayerKingIndex =>
            KingIndex(OpponentColor);

        public ushort KingIndex(Color c) => PiecesOnBoard[(int)c][KING].GetSetBits()[0];
        #endregion

        #region MoveDetail- Finding the Source Square From SAN

        /// <summary>
        ///     Find's a piece's source index, given some textual clues, such as piece type, color, and destination
        /// </summary>
        /// <param name="moveDetail">Details of move, gathered from text description (SAN)</param>
        /// <returns>The index from which the move was made.</returns>
        /// <exception cref="MoveException">
        ///     Thrown when the source can't be determined, piece on square cannot be determined, more
        ///     than one piece of type could reach destination, or piece cannot reach destination.
        /// </exception>
        private ushort FindPieceSourceIndex(MoveDetail moveDetail)
        {
            switch (moveDetail.Piece)
            {
                case Piece.Pawn:
                    if (moveDetail.IsCapture)
                        throw new MoveException("Could not determine source square for pawn capture.");
                    return FindPawnMoveSourceIndex(moveDetail, PiecesOnBoard[nActiveColor][PAWN], TotalOccupancy);

                case Piece.Knight:
                    return FindKnightMoveSourceIndex(moveDetail, ActiveKnightOccupancy);
                case Piece.Bishop:
                    return FindBishopMoveSourceIndex(moveDetail, ActiveBishopOccupancy, TotalOccupancy);
                case Piece.Rook:
                    return FindRookMoveSourceIndex(moveDetail, ActiveRookOccupancy, TotalOccupancy);
                case Piece.Queen:
                    return FindQueenMoveSourceIndex(moveDetail, ActiveQueenOccupancy, TotalOccupancy);
                case Piece.King:
                    return FindKingMoveSourceIndex(moveDetail, ActivePlayerKingOccupancy, TotalOccupancy);
                default: throw new MoveException("Invalid piece specified for move.");
            }
        }

        /// <summary>
        ///     Used by the Find[piece]MoveSourceIndex to find the source of a piece moving parsed from SAN text.
        /// </summary>
        /// <param name="md">Available Move details</param>
        /// <param name="pieceMoveMask">The move mask for the piece</param>
        /// <param name="pieceOccupancy">The occupancy for the piece in question</param>
        /// <returns></returns>
        private ushort? FindPieceMoveSourceIndex(MoveDetail md, ulong pieceMoveMask, ulong pieceOccupancy)
        {
            ulong sourceSquares;
            if ((sourceSquares = pieceMoveMask & pieceOccupancy) == 0) return null;

            if (md.SourceFile != null)
                sourceSquares &= BoardHelpers.FileMasks[md.SourceFile.Value];
            if (md.SourceRank != null)
                sourceSquares &= BoardHelpers.RankMasks[md.SourceRank.Value];
            var indices = sourceSquares.GetSetBits();

            if (indices.Length == 0) return null;
            if (indices.Length > 1)
            {
                var possibleSources = new List<ushort>();

                foreach (var sourceIndex in indices)
                {
                    if (!md.DestinationIndex.HasValue) throw new MoveException("No destination value provided.");
                    var proposedMove = MoveHelpers.GenerateMove(sourceIndex, md.DestinationIndex.Value, md.MoveType, md.PromotionPiece ?? PromotionPiece.Knight);
                    var moveValidator = new MoveValidator(this, proposedMove);
                    var validationResult = moveValidator.Validate();
                    if (validationResult == null)
                        possibleSources.Add(sourceIndex);
                }
                if (possibleSources.Count > 1) throw new MoveException("More than one piece can reach destination square.");
                if (possibleSources.Count == 0) return null;
                return possibleSources[0];
            }
            return indices[0];
        }

        /// <summary>
        ///     Finds the source square index for a King's move
        /// </summary>
        /// <param name="moveDetail">Move details, gathered from text input</param>
        /// <param name="kingOccupancy">The King's occupancy board</param>
        /// <param name="totalOccupancy">The board's occupancy</param>
        /// <returns></returns>
        public ushort FindKingMoveSourceIndex(MoveDetail moveDetail, ulong kingOccupancy, ulong totalOccupancy)
        {
            Debug.Assert(moveDetail.DestinationIndex != null, "moveDetail.DestinationIndex != null");
            var possibleSquares =
                Bitboard.GetAttackedSquares(Piece.King, moveDetail.DestinationIndex.Value, totalOccupancy);
            var sourceSquare = FindPieceMoveSourceIndex(moveDetail, possibleSquares, kingOccupancy);
            if (!sourceSquare.HasValue)
                throw new MoveException("The King can possibly get to the specified destination.");
            return sourceSquare.Value;
        }

        /// <summary>
        ///     Finds the source square index for a Queen's move
        /// </summary>
        /// <param name="moveDetail">Move details, gathered from text input</param>
        /// <param name="queenOccupancy">The Queen's occupancy board</param>
        /// <param name="totalOccupancy">The board's occupancy</param>
        /// <returns></returns>
        public ushort FindQueenMoveSourceIndex(MoveDetail moveDetail, ulong queenOccupancy, ulong totalOccupancy)
        {
            Debug.Assert(moveDetail.DestinationIndex != null, "moveDetail.DestinationIndex != null");
            var possibleSquares =
                Bitboard.GetAttackedSquares(Piece.Queen, moveDetail.DestinationIndex.Value, totalOccupancy);
            var sourceSquare = FindPieceMoveSourceIndex(moveDetail, possibleSquares, queenOccupancy);
            if (!sourceSquare.HasValue)
                throw new MoveException("No Queen can possibly get to the specified destination.");
            if (sourceSquare == ushort.MaxValue)
                throw new MoveException("More than one Queen can get to the specified square.");
            return sourceSquare.Value;
        }

        /// <summary>
        ///     Finds the source square index for a Rook's move
        /// </summary>
        /// <param name="moveDetail">Move details, gathered from text input</param>
        /// <param name="rookOccupancy">The Rook's occupancy board</param>
        /// <param name="totalOccupancy">The board's occupancy</param>
        /// <returns></returns>
        public ushort FindRookMoveSourceIndex(MoveDetail moveDetail, ulong rookOccupancy, ulong totalOccupancy)
        {
            //var possibleSquares = PieceAttackPatternHelper.BishopMoveMask[md.DestRank.Value, md.DestFile.Value];
            Debug.Assert(moveDetail.DestinationIndex != null, "moveDetail.DestinationIndex != null");
            var possibleSquares =
                Bitboard.GetAttackedSquares(Piece.Rook, moveDetail.DestinationIndex.Value, totalOccupancy);
            var sourceSquare = FindPieceMoveSourceIndex(moveDetail, possibleSquares, rookOccupancy);
            if (!sourceSquare.HasValue)
                throw new MoveException("No Rook can possibly get to the specified destination.");
            if (sourceSquare == ushort.MaxValue)
                throw new MoveException("More than one Rook can get to the specified square.");
            return sourceSquare.Value;
        }

        /// <summary>
        ///     Finds the source square index for a Bishop's move
        /// </summary>
        /// <param name="moveDetail">Move details, gathered from text input</param>
        /// <param name="bishopOccupancy">The Bishop's occupancy board</param>
        /// <param name="totalOccupancy">The board's occupancy</param>
        /// <returns></returns>
        public ushort FindBishopMoveSourceIndex(MoveDetail moveDetail, ulong bishopOccupancy,
            ulong totalOccupancy)
        {
            Debug.Assert(moveDetail.DestinationIndex != null, "moveDetail.DestinationIndex != null");
            var possibleSquares =
                Bitboard.GetAttackedSquares(Piece.Bishop, moveDetail.DestinationIndex.Value, totalOccupancy);
            var sourceSquare = FindPieceMoveSourceIndex(moveDetail, possibleSquares, bishopOccupancy);
            if (!sourceSquare.HasValue)
                throw new MoveException("No Bishop can possibly get to the specified destination.");
            if (sourceSquare == ushort.MaxValue)
                throw new MoveException("More than one Bishop can get to the specified square.");
            return sourceSquare.Value;
        }

        /// <summary>
        ///     Finds the source square index for a Knight's move
        /// </summary>
        /// <param name="moveDetail">Move details, gathered from text input</param>
        /// <param name="relevantPieceOccupancy">The Knight's occupancy board</param>
        /// <returns></returns>
        public ushort FindKnightMoveSourceIndex(MoveDetail moveDetail, ulong relevantPieceOccupancy)
        {
            Debug.Assert(moveDetail.DestinationIndex != null, "moveDetail.DestinationIndex != null");
            var possibleSquares = PieceAttackPatternHelper.KnightAttackMask[moveDetail.DestinationIndex.Value];
            var sourceSquare = FindPieceMoveSourceIndex(moveDetail, possibleSquares, relevantPieceOccupancy);
            if (!sourceSquare.HasValue)
                throw new MoveException("No Knight can possibly get to the specified destination.");
            if (sourceSquare == short.MaxValue)
                throw new MoveException("More than one Knight can get to the specified square.");
            return sourceSquare.Value;
        }

        /// <summary>
        ///     Finds the source square index for a Pawn's move
        /// </summary>
        /// <param name="moveDetail">Move details, gathered from text input</param>
        /// <param name="pawnOccupancy">The pawn's occupancy board</param>
        /// <param name="totalOccupancy">The board's occupancy</param>
        /// <returns></returns>
        public ushort FindPawnMoveSourceIndex(MoveDetail moveDetail, ulong pawnOccupancy, ulong totalOccupancy)
        {
            if (moveDetail.DestinationIndex == null)
                throw new ArgumentException("moveDetail.DestinationIndex cannot be null");
            if (moveDetail.DestinationRank == null)
                throw new ArgumentException("moveDetail.DestinationRank cannot be null");
            if (moveDetail.DestinationFile == null)
                throw new ArgumentException("moveDetail.DestinationFile cannot be null");
            var rank = moveDetail.Color == Color.Black
                ? moveDetail.DestinationRank.Value.RankCompliment()
                : moveDetail.DestinationRank.Value;
            var file = moveDetail.DestinationFile.Value;
            ushort sourceIndex = 0;
            var adjustedRelevantPieceOccupancy =
                moveDetail.Color == Color.Black ? pawnOccupancy.FlipVertically() : pawnOccupancy;
            Debug.Assert(rank < 8);
            var supposedRank = (ushort)(rank - 1);
            if (rank == 3) // 2 possible source ranks, 2 & 3 (offsets 1 & 2)
            {
                //Check 3rd rank first, logically if a pawn is there that is the source
                if ((adjustedRelevantPieceOccupancy & BoardHelpers.RankMasks[2] & BoardHelpers.FileMasks[file]) != 0)
                    sourceIndex = (ushort)((8 * 2) + (file % 8));
                else if ((adjustedRelevantPieceOccupancy & BoardHelpers.RankMasks[1] & BoardHelpers.FileMasks[file]) != 0)
                    sourceIndex = (ushort)((1 * 8) + (file % 8));
            }
            else //else source square was destination + 8 (a move one rank ahead), but we need to make sure a pawn was there
            {
                var supposedIndex = BoardHelpers.RankAndFileToIndex(
                    moveDetail.Color == Color.Black ? supposedRank.RankCompliment() : supposedRank,
                    moveDetail.DestinationFile.Value);
                if (supposedRank == 0)
                    throw new MoveException(
                        $"{moveDetail.MoveText}: Cannot possibly be a pawn at the source square {supposedIndex.IndexToSquareDisplay()} implied by move.");
                sourceIndex = (ushort)((supposedRank * 8) + moveDetail.DestinationFile.Value);
            }

            var idx = moveDetail.Color == Color.Black ? sourceIndex.FlipIndexVertically() : sourceIndex;
            ValidatePawnMove(moveDetail.Color, idx, moveDetail.DestinationIndex.Value, pawnOccupancy, totalOccupancy,
                moveDetail.MoveText);
            return idx;
        }

        /// <summary>
        ///     Validates a pawn move that has been parsed via SAN, after the source has been determined.
        /// </summary>
        /// <param name="pawnColor"></param>
        /// <param name="sourceIndex"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="pawnOccupancy">Active pawn occupancy board</param>
        /// <param name="opponentOccupancy">Opponent's occupancy board; used to validate captures</param>
        /// <param name="moveText">SAN that is used in the error messages only.</param>
        /// <exception cref="MoveException">
        ///     Thrown if no pawn exists at source, pawn cannot move from source to destination
        ///     (blocked, wrong square), destination is occupied, or if move is capture, but no opposing piece is there for
        ///     capture.
        /// </exception>
        public static void ValidatePawnMove(Color pawnColor, ushort sourceIndex, ushort destinationIndex,
            ulong pawnOccupancy, ulong opponentOccupancy, string moveText = "")
        {
            moveText = !string.IsNullOrEmpty(moveText) ? moveText + ": " : "";
            var sourceValue = sourceIndex.IndexToValue();
            var isCapture = sourceIndex.FileFromIdx() != destinationIndex.FileFromIdx();
            var destValue = destinationIndex.IndexToValue();
            //validate pawn is at supposed source
            var pawnAtSource = sourceValue & pawnOccupancy;
            if (pawnAtSource == 0)
                throw new MoveException(
                    $"There is no pawn on {sourceIndex.IndexToSquareDisplay()} to move to {destinationIndex.IndexToSquareDisplay()}.");

            //validate pawn move to square is valid
            var pawnMoves = isCapture
                ? PieceAttackPatternHelper.PawnAttackMask[(int)pawnColor][sourceIndex]
                : PieceAttackPatternHelper.PawnMoveMask[(int)pawnColor][sourceIndex];
            if ((pawnMoves & destValue) == 0)
                throw new MoveException(
                    $"{moveText}Pawn from {sourceIndex.IndexToSquareDisplay()} to {destinationIndex.IndexToSquareDisplay()} is illegal.");

            var destinationOccupancy = destValue & opponentOccupancy;
            //validate pawn is not blocked from move, if move is not a capture
            if (!isCapture)
            {
                if (destinationOccupancy != 0)
                    throw new MoveException($"{moveText}Destination square is occupied.");
            }
            else // validate Piece is on destination for capture
            {
                if (destinationOccupancy == 0)
                    throw new MoveException($"{moveText}Destination capture square is unoccupied.");
            }
        }

        #endregion


        #region FEN String Retrieval

        public string GetPiecePlacement()
        {
            var pieceSection = new char[64];
            for (var iColor = 0; iColor < 2; iColor++)
                for (var iPiece = 0; iPiece < 6; iPiece++)
                {
                    var pieceArray = PiecesOnBoard[iColor][iPiece];
                    var charRepForPieceOfColor = PieceHelpers.GetCharRepresentation((Color)iColor, (Piece)iPiece);
                    while (pieceArray != 0)
                    {
                        var squareIndex = BitHelpers.BitScanForward(pieceArray);
                        var fenIndex = FENHelpers.BoardIndexToFENIndex(squareIndex);
                        pieceSection[fenIndex] = charRepForPieceOfColor;
                        pieceArray &= pieceArray - 1;
                    }
                }

            var sb = new StringBuilder();
            for (var rank = 0; rank < 8; rank++) //start at FEN Rank of zero -> 7
            {
                var emptyCount = 0;
                for (var file = 0; file < 8; file++)
                {
                    var paChar = pieceSection[(rank * 8) + file];
                    if (paChar == 0)
                    {
                        emptyCount++;
                    }
                    else
                    {
                        if (emptyCount != 0)
                        {
                            sb.Append(emptyCount.ToString());
                            emptyCount = 0;
                        }

                        sb.Append(paChar);
                    }
                }

                if (emptyCount != 0) sb.Append(emptyCount);
                if (rank != 7) sb.Append('/');
            }

            return sb.ToString();
        }

        public string GetSideToMoveStrRepresentation()
        {
            return ActivePlayerColor == Color.Black ? "b" : "w";
        }

        public string GetCastlingAvailabilityString()
        {
            return FENHelpers.MakeCastlingAvailabilityStringFromBitFlags(CastlingAvailability);
        }

        public string GetEnPassantString()
        {
            return EnPassantIndex == null ? "-" : EnPassantIndex.Value.IndexToSquareDisplay();
        }

        public string GetHalfMoveClockString()
        {
            return HalfmoveClock.ToString();
        }

        public string GetMoveCounterString()
        {
            return MoveCounter.ToString();
        }

        public string ToFEN()
        {
            return
                $"{GetPiecePlacement()} {GetSideToMoveStrRepresentation()} {GetCastlingAvailabilityString()} {GetEnPassantString()} {GetHalfMoveClockString()} {GetMoveCounterString()}";
        }

        #endregion

        public string MoveToSAN(MoveExt move)
        {
            var src = BoardHelpers.GetPieceOfColorAtIndex(move.SourceIndex, PiecesOnBoard);
            if (src == null) throw new MoveException("Source index is empty.", MoveExceptionType.ActivePlayerHasNoPieceOnSourceSquare, move, ActivePlayerColor);
            var dst = BoardHelpers.GetPieceOfColorAtIndex(move.DestinationIndex, PiecesOnBoard);
            return MoveToSAN(move, src.Value, dst);
        }

        public string MoveToSAN(MoveExt move, PieceOfColor srcPiece, PieceOfColor? dstPiece)
        {
            var postMoveBoard = BoardHelpers.GetBoardPostMove(PiecesOnBoard, ActivePlayerColor, move);
            string strSrcPiece = GetSANSourceString(move, srcPiece);
            string strDstSquare = move.DestinationIndex.IndexToSquareDisplay();
            string checkInfo = "";
            if (srcPiece.Piece == Piece.Pawn && !dstPiece.HasValue)
            {
                strSrcPiece = "";
            }
            string capture = dstPiece.HasValue ? "x" : "";
            var promotionInfo = "";
            if (move.MoveType == MoveType.Promotion)
            {
                promotionInfo = $"={PieceHelpers.GetCharFromPromotionPiece(move.PromotionPiece)}";
            }

            if (IsPlayerOfColorInCheck(srcPiece.Color.Toggle(), postMoveBoard))
            {
                var activeColor = srcPiece.Color;
                checkInfo = "+";
                if (IsPlayerOfColorMated(activeColor.Toggle(), postMoveBoard))
                {
                    checkInfo = $"# {(activeColor == Color.White ? "1-0" : "0-1")}";
                }
            }
            //Get piece representation
            return $"{strSrcPiece}{capture}{move.DestinationIndex.IndexToSquareDisplay()}{promotionInfo}{checkInfo}";
        }

        private bool IsPlayerOfColorMated(Color color, ulong[][] postMoveBoard)
        {
            var mate = false;
            mate = CanKingMoveToAnotherSquare(color, postMoveBoard);
            return mate;
        }

        public bool CanKingMoveToAnotherSquare(Color kingColor, ulong[][] piecesOnBoard)
        {
            var kingIndex = piecesOnBoard[(int)kingColor][KING].GetSetBits()[0];
            var kingMoves = Bitboard.GetPseudoLegalMoves(Piece.King, kingIndex, Occupancy(kingColor),
                Occupancy(kingColor.Toggle()));
            var inCheck = true;
            foreach (var mv in kingMoves.GetSetBits())
            {
                var postMoveBoard =
                    BoardHelpers.GetBoardPostMove(piecesOnBoard, kingColor, MoveHelpers.GenerateMove(kingIndex, mv));
                inCheck &= Bitboard.IsAttackedBy(kingColor.Toggle(), mv, postMoveBoard);
            }

            return !inCheck; //if in check with all moves, then return false- king cannot move out of check
        }

        public static bool CanEvadeThroughBlockOrCapture(Color kingColor, ulong[][] pieceOnBoard)
        {
            return GetEvasions(kingColor, pieceOnBoard).Any();
        }

        public static MoveExt[] GetEvasions(Color kingColor, ulong[][] piecesOnBoard)
        {
            var rv = new List<MoveExt>();
            var attacks = 0ul;
            var evasionPossible = true;
            var nColor = (int)kingColor;

            var nOppColor = (int)kingColor.Toggle();
            var kingIndex = piecesOnBoard[nColor][KING].GetSetBits()[0];
            var totalOcc = piecesOnBoard.Occupancy();
            var activeOccupancy = piecesOnBoard.Occupancy(kingColor);
            var oppOccupancy = piecesOnBoard.Occupancy((Color)nOppColor);
            var piecesAttacking = AttackersOn(kingIndex, piecesOnBoard) & piecesOnBoard.Occupancy(kingColor.Toggle());
            var attackerIndexes = piecesAttacking.GetSetBits();
            var pseudoLegalKingMoves = Bitboard.GetPseudoLegalMoves(Piece.King, kingIndex, activeOccupancy, oppOccupancy);

            //double check - king must move
            foreach (var mv in pseudoLegalKingMoves.GetSetBits())
            {
                var move = MoveHelpers.GenerateMove(kingIndex, mv);
                var board = BoardHelpers.GetBoardPostMove(piecesOnBoard, kingColor, move);
                if (!Bitboard.IsAttackedBy((Color)nOppColor, mv, board))
                {
                    rv.Add(move);
                }
            }

            if (attackerIndexes.Length == 1)
            {
                foreach (var attackerIdx in attackerIndexes)
                {
                    var activeOccupancyNotKing = activeOccupancy & ~(kingIndex.GetBoardValueOfIndex());
                    foreach (var occupiedSquare in activeOccupancyNotKing.GetSetBits())
                    {
                        ulong destination;
                        var piece = BoardHelpers.GetPieceOfColorAtIndex(occupiedSquare, piecesOnBoard);
                        var attackedSquares =
                            Bitboard.GetPseudoLegalMoves(piece.Value.Piece, occupiedSquare, activeOccupancy, oppOccupancy, kingColor);
                        var squaresBetween = BoardHelpers.InBetween(kingIndex, attackerIdx);
                        if ((destination = (attackedSquares & squaresBetween)) != 0)
                        {
                            var destIdx = destination.GetSetBits();
                            Debug.Assert(destIdx.Length == 1);
                            var move = MoveHelpers.GenerateMove(occupiedSquare, destIdx[0]);
                            rv.Add(move);
                        }
                    }
                }
            }

            return rv.ToArray();
        }

        private static ulong AttackersOn(in ushort i, in ulong[][] piecesOnBoard)
        {

            var total = piecesOnBoard
                .Select(color => color.Aggregate((current, x) => current |= x))
                .Aggregate((current, x) => current |= x);
            var pawnWhite = piecesOnBoard[WHITE][PAWN];
            var pawnBlack = piecesOnBoard[BLACK][PAWN];
            var knight = piecesOnBoard[BLACK][KNIGHT] | piecesOnBoard[WHITE][KNIGHT];
            var bishop = piecesOnBoard[BLACK][BISHOP] | piecesOnBoard[WHITE][BISHOP];
            var rook = piecesOnBoard[BLACK][ROOK] | piecesOnBoard[WHITE][ROOK];
            var queen = piecesOnBoard[BLACK][QUEEN] | piecesOnBoard[WHITE][QUEEN];
            var king = piecesOnBoard[BLACK][KING] | piecesOnBoard[WHITE][KING];
            return (Bitboard.GetAttackedSquares(Piece.Pawn, i, total, Color.Black) & pawnWhite)
                   | (Bitboard.GetAttackedSquares(Piece.Pawn, i, total, Color.White) & pawnBlack)
                   | (Bitboard.GetAttackedSquares(Piece.Knight, i, total) & knight)
                   | (Bitboard.GetAttackedSquares(Piece.Bishop, i, total) & bishop)
                   | (Bitboard.GetAttackedSquares(Piece.Rook, i, total) & rook)
                   | (Bitboard.GetAttackedSquares(Piece.Queen, i, total) & (queen))
                   | (Bitboard.GetAttackedSquares(Piece.King, i, total) & king);

        }


        public string GetSANSourceString(MoveExt mv, PieceOfColor p)
        {
            if (p.Piece == Piece.King)
            {
                return "K";
            }
            if (p.Piece == Piece.Pawn)
            {
                return mv.SourceIndex.IndexToFileDisplay().ToString();
            }

            var strSrcPiece = p.Piece.GetCharRepresentation().ToString().ToUpper();
            var otherLikePieces = PiecesOnBoard[(int)p.Color][(int)p.Piece];
            var duplicateAttackerIndexes = new List<ushort>();

            foreach (var attackerIndex in otherLikePieces.GetSetBits())
            {
                if (DoesPieceAtSquareAttackSquare(mv.DestinationIndex, attackerIndex, p.Piece))
                {
                    duplicateAttackerIndexes.Add(attackerIndex);
                }
            }

            if (duplicateAttackerIndexes.Count() == 1) return strSrcPiece;
            var duplicateFiles = duplicateAttackerIndexes.Select(x => x.GetFile()).GroupBy(x => x)
                .Any(x => x.Count() > 1);
            var duplicateRanks = duplicateAttackerIndexes.Select(x => x.GetRank()).GroupBy(x => x)
                .Any(x => x.Count() > 1);

            if (!duplicateFiles)
            {
                return strSrcPiece += mv.SourceIndex.IndexToFileDisplay();
            }
            else if (!duplicateRanks)
            {
                return strSrcPiece += mv.SourceIndex.IndexToRankDisplay();
            }
            else
            {
                return strSrcPiece += mv.SourceIndex.IndexToSquareDisplay();
            }
        }
    }
}