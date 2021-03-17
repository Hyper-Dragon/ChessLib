﻿#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using ChessLib.Data.Boards;
using ChessLib.Data.Magic;
using ChessLib.Data.Magic.Init;
using ChessLib.Data.MoveRepresentation;
using ChessLib.Data.Types.Enums;
using ChessLib.Data.Types.Exceptions;
using ChessLib.Data.Types.Interfaces;
using ChessLib.Data.Validators.BoardValidation;

#endregion

namespace ChessLib.Data.Helpers
{
    public static class BoardHelpers
    {
        public enum CheckType
        {
            None,
            Single,
            Double
        }

        public static readonly ulong[][] InitialBoard;

        /// <summary>
        ///     Contains boardIndex values for a boardIndex index
        /// </summary>
        public static readonly ulong[] IndividualSquares =
        {
            0x1, 0x2, 0x4, 0x8, 0x10, 0x20, 0x40, 0x80,
            0x100, 0x200, 0x400, 0x800, 0x1000, 0x2000, 0x4000,
            0x8000, 0x10000, 0x20000, 0x40000, 0x80000, 0x100000, 0x200000,
            0x400000, 0x800000, 0x1000000, 0x2000000, 0x4000000, 0x8000000, 0x10000000,
            0x20000000, 0x40000000, 0x80000000, 0x100000000, 0x200000000, 0x400000000, 0x800000000,
            0x1000000000, 0x2000000000, 0x4000000000, 0x8000000000, 0x10000000000, 0x20000000000, 0x40000000000,
            0x80000000000, 0x100000000000, 0x200000000000, 0x400000000000, 0x800000000000, 0x1000000000000,
            0x2000000000000,
            0x4000000000000, 0x8000000000000, 0x10000000000000, 0x20000000000000, 0x40000000000000, 0x80000000000000,
            0x100000000000000,
            0x200000000000000, 0x400000000000000, 0x800000000000000, 0x1000000000000000, 0x2000000000000000,
            0x4000000000000000, 0x8000000000000000
        };

        /// <summary>
        ///     Contains set bits for each rank
        /// </summary>
        public static ulong[] RankMasks =
        {
            0xff, //R1
            0xff00, //R2
            0xff0000, //R3
            0xff000000, //R4
            0xff00000000, //R5
            0xff0000000000, //R6
            0xff000000000000, //R7
            0xff00000000000000 //R8
        };

        /// <summary>
        ///     Contains set bits for each file
        /// </summary>
        public static ulong[] FileMasks =
        {
            0x101010101010101, //A
            0x202020202020202, //B
            0x404040404040404, //C
            0x808080808080808, //D
            0x1010101010101010, //E
            0x2020202020202020, //F
            0x4040404040404040, //G
            0x8080808080808080 //H
        };

        private static readonly ulong[,] ArrInBetween = new ulong[64, 64];

        static BoardHelpers()
        {
            InitialBoard = new ulong[2][];
            InitialBoard[0] = new ulong[6];
            InitialBoard[0][0] = 65280;
            InitialBoard[0][1] = 66;
            InitialBoard[0][2] = 36;
            InitialBoard[0][3] = 129;
            InitialBoard[0][4] = 8;
            InitialBoard[0][5] = 16;
            InitialBoard[1] = new ulong[6];
            InitialBoard[1][0] = 71776119061217280;
            InitialBoard[1][1] = 4755801206503243776;
            InitialBoard[1][2] = 2594073385365405696;
            InitialBoard[1][3] = 9295429630892703744;
            InitialBoard[1][4] = 576460752303423488;
            InitialBoard[1][5] = 1152921504606846976;

            InitializeInBetween();
        }

        #region Initialization

        private static void InitializeInBetween()
        {
            for (var f = 0; f < 64; f++)
            for (var t = f; t < 64; t++)
            {
                const long m1 = -1;
                const long aFileBorder = 0x0001010101010100;
                const long b2DiagonalBorder = 0x0040201008040200;
                const long hFileBorder = 0x0002040810204080;

                var between = (m1 << f) ^ (m1 << t);
                long file = (t & 7) - (f & 7);
                long rank = ((t | 7) - f) >> 3;
                var line = ((file & 7) - 1) & aFileBorder;
                line += 2 * (((rank & 7) - 1) >> 58); /* b1g1 if same rank */
                line += (((rank - file) & 15) - 1) & b2DiagonalBorder; /* b2g7 if same diagonal */
                line += (((rank + file) & 15) - 1) & hFileBorder; /* h1b7 if same anti-diagonal */
                line *= between & -between; /* mul acts like shift by smaller boardIndex */
                ArrInBetween[f, t] = (ulong) (line & between); /* return the bits on that line in-between */
            }
        }

        #endregion

        /// <summary>
        ///     Gets the opposite color
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Color Toggle(this Color c)
        {
            return c == Color.White ? Color.Black : Color.White;
        }

        /// <summary>
        ///     Gets the squares in between two squares, returns 0 for squares not linked diagonally or by file or rank
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static ulong InBetween(int from, int to)
        {
            var square1 = Math.Min(from, to);
            var square2 = Math.Max(from, to);
            return ArrInBetween[square1, square2];
        }


        /// <summary>
        ///     Gets the occupancy of a pieceLayout by color and/or piece (or neither = TotalOccupancy)
        /// </summary>
        /// <param name="board"></param>
        /// <param name="c"></param>
        /// <param name="p"></param>
        /// <returns>
        ///     <see cref="ulong" /> of pieces on board, optionally by
        ///     <param name="c">color</param>
        ///     and
        ///     <param name="p">piece</param>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Occupancy(this ulong[][] board, Color? c = null, Piece? p = null)
        {
            if (c == null && p == null)
                return board.Select(x => x.Aggregate((acc, val) => acc | val)).Aggregate((acc, val) => acc | val);
            if (c == null)
                return board[(int) Color.White][(int) p] | board[(int) Color.Black][(int) p];
            if (p == null) return board[(int) c].Aggregate((current, val) => current | val);

            return board[(int) c][(int) p];
        }

        /// <summary>
        ///     Gets the <see cref="Piece" /> object occupying the supplied
        ///     <param name="boardIndex">pieceLayout index</param>
        ///     on the current
        ///     <param name="occupancy">pieceLayout</param>
        /// </summary>
        /// <param name="occupancy">occupancy arrays in [white][black] format</param>
        /// <param name="boardIndex">pieceLayout index</param>
        /// <returns>Type of Piece, if found, otherwise null</returns>
        /// <exception cref="ArgumentException">
        ///     if
        ///     <param name="boardIndex">index</param>
        ///     is not in range
        /// </exception>
        public static Piece? GetPieceAtIndex(in ulong[][] occupancy, in ushort boardIndex)
        {
            boardIndex.ValidateIndex();
            var pocAtIndex = GetPieceOfColorAtIndex(occupancy, boardIndex);
            return pocAtIndex?.Piece;
        }

        /// <summary>
        ///     Gets the <see cref="Piece" /> object occupying the supplied
        ///     <param name="index">pieceLayout index</param>
        ///     on the current
        ///     <param name="board">pieceLayout</param>
        /// </summary>
        /// <param name="board">The current pieceLayout configuration</param>
        /// <param name="index">The pieceLayout index</param>
        /// <returns>Type of Piece, if found, otherwise null</returns>
        public static Piece? GetPieceAtIndex(this IBoard board, ushort index)
        {
            index.ValidateIndex();
            var poc = GetPieceOfColorAtIndex(board, index);
            return poc?.Piece;
        }

        /// <summary>
        ///     Gets a piece of color object for the index
        /// </summary>
        /// <param name="occupancy">Board's piece occupancy</param>
        /// <param name="boardIndex">Index on pieceLayout</param>
        /// <returns>
        ///     The object representing the piece at an index, or null if no piece occupies the supplied
        ///     <param name="boardIndex">index</param>
        ///     .
        /// </returns>
        public static PieceOfColor? GetPieceOfColorAtIndex(this ulong[][] occupancy, ushort boardIndex)
        {
            boardIndex.ValidateIndex();
            var val = 1ul << boardIndex;
            for (var c = 0; c < 2; c++)
            {
                var color = (Color) c;
                var piecePosition = occupancy[c]
                    .Select((placementValue, arrIdx) => new
                        {Color = color, PlacementValue = placementValue, Piece = (Piece) arrIdx})
                    .FirstOrDefault(p => (p.PlacementValue & val) != 0);

                if (piecePosition != null)
                {
                    return new PieceOfColor
                    {
                        Piece = piecePosition.Piece,
                        Color = piecePosition.Color
                    };
                }
            }

            return null;
        }

        /// <summary>
        ///     Gets a piece of color object for the index
        /// </summary>
        /// <param name="board">The current pieceLayout configuration</param>
        /// <param name="index">The pieceLayout index in which to find piece of color</param>
        /// <returns>The object representing the piece at an index, or null of the piece isn't there.</returns>
        public static PieceOfColor? GetPieceOfColorAtIndex(this IBoard board, ushort index)
        {
            return GetPieceOfColorAtIndex(board.Occupancy, index);
        }


        /// <summary>
        ///     Method to validate if index is in range.
        /// </summary>
        /// <param name="index">pieceLayout index to validate</param>
        /// <exception cref="ArgumentException">if index is out of range (0...63)</exception>
        public static void ValidateIndex(this ushort index)
        {
            if (index >= 64) throw new ArgumentException($"Board index {index} is out of range.");
        }

        /// <summary>
        ///     Method to validate if all index params are in range (0...63)
        /// </summary>
        /// <param name="indices">indexes to validate</param>
        /// <exception cref="ArgumentException">if index is out of range (0...63)</exception>
        public static void ValidateIndices(params ushort[] indices)
        {
            foreach (var index in indices) index.ValidateIndex();
        }

        /// <summary>
        ///     Returns a boolean to represent if the specified player is in check
        /// </summary>
        /// <param name="board"></param>
        /// <param name="playerInCheckColor"></param>
        /// <returns></returns>
        public static bool IsPlayerInCheck(this ulong[][] board, int playerInCheckColor)
        {
            var kingIndex = board[playerInCheckColor][KING].GetSetBits()[0];
            return kingIndex.IsSquareAttackedByColor((Color) (1 - playerInCheckColor), board, kingIndex);
        }

        ///// <summary>
        /////     Generate array of potential moves, disregarding legality of move
        ///// </summary>
        ///// <param name="pieceLayout">Piece Layout of board: [white][black]</param>
        ///// <param name="activeColor">Active color to move</param>
        ///// <param name="enPassantSquare">En Passant boardIndex, if any, null if not</param>
        ///// <param name="castlingAvailability">Castling Availability flags</param>
        ///// <returns></returns>
        //public static MoveExt[] GenerateAllPseudoLegalMoves(this ulong[][] pieceLayout, Color activeColor,
        //    ushort? enPassantSquare, CastlingAvailability castlingAvailability)
        //{
        //    var rv = new List<MoveExt>();
        //    var nColor = (int) activeColor;

        //    for (var i = 0; i < 6; i++)
        //    {
        //        var p = (Piece) i;
        //        var pieceLocations = pieceLayout[nColor][i].GetSetBits();
        //        foreach (var sq in pieceLocations)
        //        {
        //            Bitboard.GetPseudoLegalMoves(p, sq, Occupancy(pieceLayout, activeColor),
        //                Occupancy(pieceLayout, activeColor.Toggle()), activeColor, enPassantSquare,
        //                castlingAvailability, out var plm);
        //            rv.AddRange(plm);
        //        }
        //    }

        //    return rv.ToArray();
        //}

        /// <summary>
        ///     Clears appropriate castling availability flag when <paramref name="movingPiece">piece moving</paramref> is a
        ///     <see cref="Piece.Rook">Rook</see> or <see cref="Piece.King">King</see>
        /// </summary>
        /// <param name="board">The current pieceLayout configuration</param>
        /// <param name="move">Move object</param>
        /// <param name="movingPiece">Piece that is moving</param>
        public static CastlingAvailability GetCastlingAvailabilityPostMove(IBoard board, MoveExt move,
            Piece movingPiece)
        {
            var ca = board.CastlingAvailability;
            switch (movingPiece)
            {
                case Piece.Rook:
                    if (move.SourceIndex == 56) ca &= ~CastlingAvailability.BlackQueenside;
                    if (move.SourceIndex == 63) ca &= ~CastlingAvailability.BlackKingside;
                    if (move.SourceIndex == 0) ca &= ~CastlingAvailability.WhiteQueenside;
                    if (move.SourceIndex == 7) ca &= ~CastlingAvailability.WhiteKingside;
                    break;
                case Piece.King:
                    if (move.SourceIndex == 60)
                        ca &=
                            ~(CastlingAvailability.BlackKingside | CastlingAvailability.BlackQueenside);
                    if (move.SourceIndex == 4)
                        ca &=
                            ~(CastlingAvailability.WhiteKingside | CastlingAvailability.WhiteQueenside);
                    break;
            }

            return ca;
        }

        public static bool IsEnPassantCaptureAvailable(this Board board)
        {
            var epSquare = board.EnPassantSquare;
            if (epSquare == null)
            {
                return false;
            }

            var epAttackFromSquares =
                PieceAttackPatterns.Instance.PawnAttackMask[(int) board.OpponentColor][epSquare.Value];
            return (epAttackFromSquares & board.Occupancy[(int) board.ActivePlayer][(int) Piece.Pawn]) != 0;
        }

        /// <summary>
        ///     Sets EnPassant flag appropriately, clearing it if no En Passant is available
        /// </summary>
        /// <param name="move"></param>
        /// <param name="pocSource"></param>
        public static ushort? GetEnPassantIndex(MoveExt move, PieceOfColor? pocSource)
        {
            ushort? rv = null;
            if (pocSource.HasValue)
            {
                var startRank = pocSource.Value.Color == Color.White ? 1 : 6;
                var endRank = pocSource.Value.Color == Color.White ? 3 : 4;
                var enPassantIndexOffset = pocSource.Value.Color == Color.White ? 8 : -8;
                if (pocSource.Value.Piece == Piece.Pawn)
                    if ((move.SourceValue & RankMasks[startRank]) != 0
                        && (move.DestinationValue & RankMasks[endRank]) != 0)
                        rv = (ushort) (move.SourceIndex + enPassantIndexOffset);
            }

            return rv;
        }


        /// <summary>
        ///     Applies a move to a pieceLayout
        /// </summary>
        /// <param name="currentBoard">Board to which move will be applied.</param>
        /// <param name="move"></param>
        /// <param name="bypassMoveValidation">Bypass validation; useful when move was previously validated</param>
        /// <returns>The board after the move has been applied.</returns>
        /// <exception cref="MoveException">If no piece exists at source.</exception>
        public static IBoard ApplyMoveToBoard(this IBoard currentBoard, in MoveExt move,
            bool bypassMoveValidation = false)
        {
            var board = (IBoard) currentBoard.Clone();
            if (!bypassMoveValidation)
            {
                var boardValidator = new BoardValidator(board);
                boardValidator.Validate(true);
            }

            var pieceMoving = GetPieceOfColorAtIndex(board.Occupancy, move.SourceIndex);
            if (pieceMoving == null)
                throw new MoveException("No piece at current source to apply move to.",
                    MoveError.ActivePlayerHasNoPieceOnSourceSquare, move, board.ActivePlayer);


            var capturedPiece = GetPieceAtIndex(board.Occupancy, move.DestinationIndex);


            var isPawnMove = IsPawnMove(board, move);

            var halfMoveClock = capturedPiece != null || isPawnMove ? 0 : board.HalfMoveClock + 1;
            var fullMoveCounter =
                board.ActivePlayer == Color.Black ? board.FullMoveCounter + 1 : board.FullMoveCounter;

            var piecePlacement = GetBoardPostMove(board, move);
            var castlingAvailability = GetCastlingAvailabilityPostMove(board, move, pieceMoving.Value.Piece);
            var enPassantSquare = GetEnPassantIndex(move, pieceMoving.Value);
            var activePlayer = board.ActivePlayer.Toggle();
            return new Board(piecePlacement, (ushort) halfMoveClock, enPassantSquare, capturedPiece,
                castlingAvailability, activePlayer,
                (ushort) fullMoveCounter);
        }

        private static bool IsPawnMove(in IBoard board, in MoveExt move)
        {
            return (board.Occupancy[(int) board.ActivePlayer][PAWN] & move.SourceValue) != 0;
        }


        /// <summary>
        ///     Gets the piece setup post-move
        /// </summary>
        /// <param name="boardInfo"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        public static ulong[][] GetBoardPostMove(in IBoard boardInfo, in MoveExt move)
        {
            var board = (IBoard) boardInfo.Clone();
            var pieces = board.Occupancy;
            var activeColor = (int) board.ActivePlayer;
            var oppColor = activeColor ^ 1;
            var oppOccupancy = Occupancy(board.Occupancy, board.OpponentColor());
            var piece = board.GetPieceOfColorAtIndex(move.SourceIndex);
            if (piece == null)
            {
                throw new MoveException(
                    $"No piece is present at the source indicated: {move.SourceIndex.IndexToSquareDisplay()}",
                    boardInfo);
            }

            if (piece.Value.Color != board.ActivePlayer)
            {
                throw new MoveException(
                    $"Piece found was {piece.Value.Color.ToString()} when it is {board.ActivePlayer}'s move.",
                    boardInfo);
            }

            var nPiece = (int) piece.Value.Piece;
            pieces[activeColor][nPiece] ^= move.SourceValue ^ move.DestinationValue;
            if ((oppOccupancy & move.DestinationValue) != 0)
            {
                for (var idx = 0; idx < pieces[oppColor].Length; idx++)
                {
                    pieces[oppColor][idx] &= ~move.DestinationValue;
                }
            }

            switch (move.MoveType)
            {
                case MoveType.Promotion:
                    pieces[activeColor][nPiece] ^= move.DestinationValue;
                    var promotionPiece = (int) move.PromotionPiece + 1;
                    pieces[activeColor][promotionPiece] ^= move.DestinationValue;
                    break;
                case MoveType.EnPassant:
                    if (!board.EnPassantSquare.HasValue)
                    {
                        throw new MoveException(
                            "En Passant is not available, but move was flagged as En Passant capture.",
                            MoveError.EpNotAvailable, move, board.ActivePlayer);
                    }

                    var enPassantSqIdx = board.EnPassantSquare.Value;
                    var captureSquare = board.ActivePlayer == Color.White ? enPassantSqIdx - 8 : enPassantSqIdx + 8;
                    pieces[oppColor][PAWN] &= ~((ushort) captureSquare).ToBoardValue();
                    break;
                case MoveType.Castle:
                    var rookMove = MoveHelpers.GetRookMoveForCastleMove(move);
                    pieces[activeColor][ROOK] ^= rookMove.SourceValue ^ rookMove.DestinationValue;
                    break;
            }

            return pieces;
            //return GetBoardPostMove(board.GetPiecePlacement(), board.ActivePlayer, move);
        }

        /// <summary>
        ///     Gets a pieceLayout's piece setup after the specified player makes the specified move
        /// </summary>
        /// <param name="currentBoard"></param>
        /// <param name="activePlayerColor"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        public static ulong[][] GetBoardPostMove(this ulong[][] currentBoard, in Color activePlayerColor,
            in MoveExt move)
        {
            var nActiveColor = (int) activePlayerColor;
            var opponentColor = activePlayerColor.Toggle();
            var nOppColor = (int) opponentColor;
            var resultantBoard = new ulong[2][];
            var pieceMoving = GetPieceAtIndex(currentBoard, move.SourceIndex);
            for (var i = 0; i < 2; i++)
            {
                resultantBoard[i] = new ulong[6];
                foreach (var p in Enum.GetValues(typeof(Piece)))
                {
                    resultantBoard[i][(int) p] = currentBoard[i][(int) p];
                    if (i == nActiveColor && (Piece) p == pieceMoving)
                    {
                        resultantBoard[i][(int) p] = BitHelpers.ClearBit(resultantBoard[i][(int) p], move.SourceIndex);
                        resultantBoard[i][(int) p] = resultantBoard[i][(int) p].SetBit(move.DestinationIndex);
                    }
                    else if (i == (int) opponentColor)
                    {
                        resultantBoard[i][(int) p] =
                            BitHelpers.ClearBit(resultantBoard[i][(int) p], move.DestinationIndex);
                    }
                }
            }

            if (move.MoveType == MoveType.Castle)
            {
                resultantBoard[nActiveColor][ROOK] = GetRookBoardPostCastle(move, resultantBoard[nActiveColor][ROOK]);
            }
            else if (move.MoveType == MoveType.EnPassant)
            {
                var capturedPawnValue = 1ul << (opponentColor == Color.Black
                    ? move.DestinationIndex - 8
                    : move.DestinationIndex + 8);
                resultantBoard[nOppColor][PAWN] &= ~capturedPawnValue;
            }
            else if (move.MoveType == MoveType.Promotion)
            {
                resultantBoard[nActiveColor][PAWN] &= ~move.DestinationValue;
                switch (move.PromotionPiece)
                {
                    case PromotionPiece.Knight:
                        resultantBoard[nActiveColor][KNIGHT] |= move.DestinationValue;
                        break;
                    case PromotionPiece.Bishop:
                        resultantBoard[nActiveColor][BISHOP] |= move.DestinationValue;
                        break;
                    case PromotionPiece.Rook:
                        resultantBoard[nActiveColor][ROOK] |= move.DestinationValue;
                        break;
                    case PromotionPiece.Queen:
                        resultantBoard[nActiveColor][QUEEN] |= move.DestinationValue;
                        break;
                }
            }

            return resultantBoard;
        }

        private static ulong GetRookBoardPostCastle(MoveExt move, ulong rookBoard)
        {
            var rank = move.DestinationIndex.RankFromIdx();
            var file = move.DestinationIndex.FileFromIdx();
            var rookSource = rank == 7 // black castling
                ? file == 2
                    ? 0x100000000000000ul // BLACK O-O-O
                    : 0x8000000000000000ul // BLACK O-O
                : file == 2
                    ? 0x01ul // WHITE O-O-O
                    : 0x80ul; // WHITE O-O

            var rookDest = rank == 7 // black castling
                ? file == 2
                    ? 0x800000000000000ul // BLACK O-O-O
                    : 0x2000000000000000ul // BLACK O-O
                : file == 2
                    ? 0x08ul // WHITE O-O-O
                    : 0x20ul; // WHITE O-O

            return (rookBoard & ~rookSource) | rookDest;
        }


        public static Color OpponentColor(this IBoard board)
        {
            return board.ActivePlayer.Toggle();
        }

        public static ushort ActiveKingIndex(this IBoard board)
        {
            return board.Occupancy[(int) board.ActivePlayer][KING].GetSetBits()[0];
        }

        public static bool IsActivePlayerInCheck(this IBoard board)
        {
            return IsColorInCheck(board.Occupancy, board.ActivePlayer);
        }

        public static bool IsOpponentInCheck(this IBoard board)
        {
            return IsColorInCheck(board.Occupancy, board.OpponentColor());
        }

        private static ushort GetKingIndex(this ulong[][] board, Color kingColor)
        {
            var indices = board.Occupancy(kingColor, Piece.King).GetSetBits();
            Debug.Assert(indices.Length == 1);
            return indices[0];
        }

        public static CheckType GetCheckType(ulong[][] board, Color activeColor, out ushort[] attackingPieces)
        {
            var checkedColorKingIdx = board.GetKingIndex(activeColor);
            var attackingColor = activeColor.Toggle();
            attackingPieces = GetPiecesAttackingSquare(board, checkedColorKingIdx, attackingColor);
            if (!attackingPieces.Any())
            {
                return CheckType.None;
            }

            if (attackingPieces.Length > 1)
            {
                return CheckType.Double;
            }

            return CheckType.Single;
        }

        public static bool IsColorInCheck(ulong[][] board, Color checkedColor)
        {
            Debug.Assert(board.Length == 2);
            var nCheckedColor = (int) checkedColor;
            Debug.Assert(board[nCheckedColor].Length == 6);
            var kingOccupancy = board[nCheckedColor][KING];
            var setBits = kingOccupancy.GetSetBits();
            Debug.Assert(setBits.Length == 1);

            var checkedColorKingIdx = setBits[0];
            return checkedColorKingIdx.IsSquareAttackedByColor(checkedColor.Toggle(), board, checkedColorKingIdx);
        }

        /// <summary>
        ///     Determines if active player is stalemated.
        /// </summary>
        /// <remarks>
        ///     Algorithm is (active player is not in check (thus, not mate, either)) AND (active player has no moves)
        /// </remarks>
        /// <param name="board">Current board configuration</param>
        /// <returns>true if activePlayer is stalemated, false otherwise</returns>
        public static bool IsStalemate(this IBoard board)
        {
            if (board.IsActivePlayerInCheck()) return false;
            var activeColor = (int) board.ActivePlayer;
            foreach (var pieceSet in board.Occupancy[activeColor].Where(x => x != 0))
            {
                foreach (var pieceIdx in pieceSet.GetSetBits())
                {
                    var piece = board.GetPieceAtIndex(pieceIdx);
                    Debug.Assert(piece.HasValue);
                    if (board.CanPieceMove(pieceIdx))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool IsStalemate(ulong[][] occupancy, Color activeColor, ushort? enPassantIdx,
            CastlingAvailability castlingAvailability)
        {
            if (IsColorInCheck(occupancy, activeColor))
            {
                return false;
            }

            foreach (var pieceSet in occupancy[(int) activeColor].Where(x => x != 0))
            {
                foreach (var pieceIdx in pieceSet.GetSetBits())
                {
                    var piece = GetPieceAtIndex(occupancy, pieceIdx);
                    Debug.Assert(piece.HasValue);
                    if (CanPieceMove(occupancy, activeColor, pieceIdx, enPassantIdx, castlingAvailability))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool CanPieceMove(ulong[][] occupancy, Color activeColor, ushort pieceIndex,
            ushort? enPassantIdx, CastlingAvailability castlingAvailability)
        {
            return Bitboard.CanPieceMove(pieceIndex, occupancy, castlingAvailability, enPassantIdx, activeColor);
        }

        /// <summary>
        ///     Determines if active player has been mated
        /// </summary>
        /// <returns></returns>
        public static bool IsCheckmate(ulong[][] occupancy, Color activeColor)
        {
            var checkType = GetCheckType(occupancy, activeColor, out var attackingPieces);
            var kingIndex = GetKingIndex(occupancy, activeColor);
            if (checkType == CheckType.None)
            {
                return false;
            }

            var kingMoves = GetValidKingMoves(occupancy, activeColor);
            if (kingMoves.Any())
            {
                return false;
            }

            if (checkType == CheckType.Single)
            {
                //Can checking piece be captured by piece
                var attackingPiece = attackingPieces.Single();
                var piecesAttackingCheckingPiece = GetPiecesAttackingSquare(occupancy, attackingPiece, activeColor);
                //if any piece other than the King attacks the checking piece (King attack was already checked above), then it is not mate.
                if (piecesAttackingCheckingPiece.Any(x => x != kingIndex))
                {
                    return false;
                }

                //Can check be blocked
                var squaresBetweenKingAndAttackingPiece = InBetween(kingIndex, attackingPiece);
                foreach (var squareBetween in squaresBetweenKingAndAttackingPiece.GetSetBits())
                {
                    var availableBlockers = GetPiecesAttackingSquare(occupancy, squareBetween, activeColor);
                    //Can any piece block the check (besides the King).
                    if (availableBlockers.Any(blocker => blocker != kingIndex))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static IEnumerable<ushort> GetValidKingMoves(ulong[][] occupancy, Color activeColor)
        {
            var rv = new List<ushort>();
            var kingIndex = GetKingIndex(occupancy, activeColor);
            var activeOccupancy = occupancy.Occupancy(activeColor);
            var opponentOccupancy = occupancy.Occupancy(activeColor.Toggle());
            Bitboard.GetPseudoLegalMoves(Piece.King, kingIndex, activeOccupancy,
                opponentOccupancy, activeColor, null, CastlingAvailability.NoCastlingAvailable, out var moves);
            foreach (var move in moves)
            {
                var resultantBoard = GetBoardPostMove(occupancy, activeColor, move);
                var checkType = GetCheckType(resultantBoard, activeColor, out _);
                if (checkType == CheckType.None)
                {
                    rv.Add(move.DestinationIndex);
                }
            }

            return rv;
        }


        /// <summary>
        ///     Can the specified color's king evade an attack through a block or a capture
        /// </summary>
        /// <returns></returns>
        public static bool DoesKingHaveEvasions(ulong[][] occupancy, Color activeColor)
        {
            var kindIndex = occupancy[(int) activeColor][(int) Piece.King].GetSetBits()[0];
            var activeOccupancy = Occupancy(occupancy, activeColor);
            var opponentOccupancy = Occupancy(occupancy, activeColor.Toggle());
            var moves = Bitboard.GetPseudoLegalMoves(Piece.King, kindIndex, activeOccupancy, opponentOccupancy,
                activeColor, null,
                CastlingAvailability.NoCastlingAvailable, out var _);
            foreach (var move in moves.GetSetBits())
            {
                if (!move.IsSquareAttackedByColor(activeColor.Toggle(), occupancy, kindIndex))
                {
                    return true;
                }
            }

            return false;
        }


        private static ushort[] GetPiecesAttackingSquare(ulong[][] pieces, ushort idx, Color attackerColor,
            ushort? blockerToRemove = null)
        {
            pieces = blockerToRemove.HasValue ? RemovePotentialBlocker(pieces, blockerToRemove.Value) : pieces;
            var piecesAttacking = PiecesAttackingSquare(pieces, idx);
            var attackingColorOccupancy = pieces.Occupancy(attackerColor);
            var attackingPieces = piecesAttacking & attackingColorOccupancy;
            var attackerIndexes = attackingPieces.GetSetBits();
            return attackerIndexes;
        }

        private static ulong[][] RemovePotentialBlocker(ulong[][] pieces, ushort blocker)
        {
            var notPieceVal = ~blocker.GetBoardValueOfIndex();
            var pieceArrayRv = new ulong[2][];
            for (var i = 0; i < pieces.Length; i++)
            {
                var colorSet = pieces[i];
                pieceArrayRv[i] = new ulong[6];
                for (var pieceIdx = 0; pieceIdx < colorSet.Length; pieceIdx++)
                {
                    var piece = colorSet[pieceIdx];
                    pieceArrayRv[i][pieceIdx] = piece & notPieceVal;
                }
            }

            return pieceArrayRv;
        }


        /// <summary>
        ///     Gets piece value of pieces that attack the boardIndex index.
        /// </summary>
        /// <param name="board">The IBoard from which to derive pieceLayout information.</param>
        /// <param name="squareIndex">index of boardIndex being attacked.</param>
        /// <returns>
        ///     <see cref="ulong" /> that represents squares containing pieces attacking
        ///     <param name="squareIndex">index</param>
        ///     boardIndex
        /// </returns>
        public static ulong PiecesAttackingSquare(this IBoard board, in ushort squareIndex)
        {
            var piecesOnBoard = board.Occupancy;
            return PiecesAttackingSquare(piecesOnBoard, squareIndex);
        }

        private static ulong PiecesAttackingSquare(in ulong[][] piecesOnBoard, in ushort squareIndex)
        {
            var total = piecesOnBoard.Occupancy();
            var pawnWhite = piecesOnBoard[WHITE][PAWN];
            var pawnBlack = piecesOnBoard[BLACK][PAWN];
            var knight = piecesOnBoard[BLACK][KNIGHT] | piecesOnBoard[WHITE][KNIGHT];
            var bishop = piecesOnBoard[BLACK][BISHOP] | piecesOnBoard[WHITE][BISHOP];
            var rook = piecesOnBoard[BLACK][ROOK] | piecesOnBoard[WHITE][ROOK];
            var queen = piecesOnBoard[BLACK][QUEEN] | piecesOnBoard[WHITE][QUEEN];
            var king = piecesOnBoard[BLACK][KING] | piecesOnBoard[WHITE][KING];
            var blackPawnPseudoAttacks = Bitboard.GetAttackedSquares(Piece.Pawn, squareIndex, total, Color.White);
            var blackPawnAttacks = blackPawnPseudoAttacks & pawnBlack;
            var whitePawnPseudoAttacks = Bitboard.GetAttackedSquares(Piece.Pawn, squareIndex, total, Color.Black);
            var whitePawnAttacks = whitePawnPseudoAttacks & pawnWhite;
            return blackPawnAttacks
                   | whitePawnAttacks
                   | (Bitboard.GetAttackedSquares(Piece.Knight, squareIndex, total, Color.White) & knight)
                   | (Bitboard.GetAttackedSquares(Piece.Bishop, squareIndex, total, Color.White) & bishop)
                   | (Bitboard.GetAttackedSquares(Piece.Rook, squareIndex, total, Color.White) & rook)
                   | (Bitboard.GetAttackedSquares(Piece.Queen, squareIndex, total, Color.White) & queen)
                   | (Bitboard.GetAttackedSquares(Piece.King, squareIndex, total, Color.White) & king);
        }

        #region Constant Piece and Color Values for Indexing arrays

        // ReSharper disable InconsistentNaming
        public const int PAWN = (int) Piece.Pawn;
        public const int BISHOP = (int) Piece.Bishop;
        public const int KNIGHT = (int) Piece.Knight;
        public const int ROOK = (int) Piece.Rook;
        public const int QUEEN = (int) Piece.Queen;
        public const int KING = (int) Piece.King;
        public const int WHITE = (int) Color.White;

        public const int BLACK = (int) Color.Black;
        // ReSharper restore InconsistentNaming

        #endregion


        #region Enum ToInt() methods

        /// <summary>
        ///     Extension to get the int of a color
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this Color c)
        {
            return (int) c;
        }

        /// <summary>
        ///     Extension to get the int of a piece
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this Piece p)
        {
            return (int) p;
        }

        /// <summary>
        ///     Extension to get the int of a file
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this File f)
        {
            return (int) f;
        }

        /// <summary>
        ///     Extension to get the int of a rank
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this Rank r)
        {
            return (int) r;
        }

        /// <summary>
        ///     Gets the hex display of a long (for debugging, mainly)
        /// </summary>
        /// <param name="u">long to get display from</param>
        /// <param name="appendHexNotation">append '0x' to the representation</param>
        /// <param name="pad">pad length to a certain size</param>
        /// <param name="padSize">size to pad</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToHexDisplay(this ulong u, bool appendHexNotation = true, bool pad = false,
            int padSize = 64)
        {
            var str = Convert.ToString((long) u, 16);
            if (pad) str = str.PadLeft(padSize, '0');
            if (appendHexNotation) str = "0x" + str;
            return str;
        }

        #endregion

        #region Array Position to Friendly Position Helpers

        /// <summary>
        ///     Gets the index of a boardIndex
        /// </summary>
        /// <param name="square">
        ///     SAN boardIndex representation (A1, H5, E4, etc). Must be either '-' (Premove FEN En Passant) or 2
        ///     characters
        /// </param>
        /// <returns>Square index</returns>
        /// <exception cref="ArgumentException">Thrown if boardIndex length, File, or Rank is invalid.</exception>
        public static ushort? SquareTextToIndex(this string square)
        {
            if (square.Trim() == "-") return null;
            if (square.Length != 2)
                throw new ArgumentException($"Square passed to SquareTextToIndex(), {square} has an invalid length.");
            var file = char.ToLower(square[0]);
            var rank = ushort.Parse(square[1].ToString());
            if (!char.IsLetter(file) || file < 'a' || file > 'h')
                throw new ArgumentException("File portion of boardIndex-text should be a letter, between 'a' and 'h'.");
            if (rank < 1 || rank > 8)
                throw new ArgumentException(
                    "Rank portion of boardIndex-text should be a digit with a value between 1 and 8.");
            var rankMultiplier = rank - 1;
            return (ushort) (rankMultiplier * 8 + file - 'a');
        }


        /// <summary>
        ///     Gets a File basked on boardIndex index
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static File GetFile(this int square)
        {
            return (File) (square % 8);
        }

        /// <summary>
        ///     Gets a File basked on boardIndex index
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort GetFile(this ushort square)
        {
            return (ushort) (square % 8);
        }

        /// <summary>
        ///     Gets a Rank basked on boardIndex index
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rank GetRank(this int square)
        {
            Debug.Assert(square >= 0 && square < 64);
            return (Rank) ((ushort) square).GetRank();
        }

        /// <summary>
        ///     Gets a Rank basked on boardIndex index
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     if
        ///     <param name="boardIndex">index</param>
        ///     is not in range.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort GetRank(this ushort boardIndex)
        {
            boardIndex.ValidateIndex();
            return (ushort) (boardIndex / 8);
        }

        /// <summary>
        ///     Gets a rank index from boardIndex
        ///     <param name="boardIndex">index</param>
        /// </summary>
        /// <param name="boardIndex"></param>
        /// <returns>Board rank (First rank: 0)</returns>
        /// <exception cref="ArgumentException">
        ///     if
        ///     <param name="boardIndex">index</param>
        ///     is out of range.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort RankFromIdx(this ushort boardIndex)
        {
            boardIndex.ValidateIndex();
            return (ushort) (boardIndex / 8);
        }

        /// <summary>
        ///     Gets a file index from a boardIndex index
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort FileFromIdx(this ushort idx)
        {
            return (ushort) (idx % 8);
        }

        /// <summary>
        ///     Gets
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort RankCompliment(this ushort rank)
        {
            return (ushort) Math.Abs(rank - 7);
        }

        #endregion


        #region PremoveFEN String Retrieval

        public static string GetFENPiecePlacement(this IBoard board)
        {
            return board.Occupancy.GetPiecePlacement();
        }

        public static string GetFENSideToMoveStrRepresentation(this IBoard board)
        {
            return board.ActivePlayer == Color.Black ? "b" : "w";
        }

        public static string GetFENCastlingAvailabilityString(this IBoard board)
        {
            return FENHelpers.MakeCastlingAvailabilityStringFromBitFlags(board.CastlingAvailability);
        }

        public static string GetFENEnPassantString(this IBoard board)
        {
            return board.EnPassantSquare == null ? "-" : board.EnPassantSquare.Value.IndexToSquareDisplay();
        }

        public static string GetFENHalfMoveClockString(this IBoard board)
        {
            return board.HalfMoveClock.ToString();
        }

        public static string GetFENMoveCounterString(this IBoard board)
        {
            return board.FullMoveCounter.ToString();
        }

        public static string ToFEN(this IBoard b)
        {
            return
                $"{b.GetFENPiecePlacement()} {b.GetFENSideToMoveStrRepresentation()} {b.GetFENCastlingAvailabilityString()} {b.GetFENEnPassantString()} {b.GetFENHalfMoveClockString()} {b.GetFENMoveCounterString()}";
        }

        #endregion
    }
}