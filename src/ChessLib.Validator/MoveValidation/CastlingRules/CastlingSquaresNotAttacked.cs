﻿using ChessLib.Data;
using ChessLib.Data.Exceptions;
using ChessLib.Data.Helpers;
using ChessLib.Types.Enums;
using ChessLib.Types.Interfaces;

namespace ChessLib.Validators.MoveValidation.CastlingRules
{
    public class CastlingSquaresNotAttacked : IMoveRule
    {
        public MoveExceptionType? Validate(in IBoard boardInfo, in ulong[][] postMoveBoard, in IMoveExt move)
        {
            if (IsKingsPathInCheck(boardInfo.OpponentColor(), boardInfo.PiecePlacement.GetPiecePlacementArray(), move))
            {
                return MoveExceptionType.Castle_ThroughCheck;
            }
            return null;
        }
        /// <summary>
        /// Returns a boolean representing if castling path is attacked
        /// </summary>
        /// <param name="opponentColor"></param>
        /// <param name="occupancy"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        protected static bool IsKingsPathInCheck(in Color opponentColor, in ulong[][] occupancy, in IMoveExt move)
        {
            var moveToAndFromValues = move.SourceValue | move.DestinationValue;
            var squaresBetween = BoardHelpers.InBetween(move.SourceIndex, move.DestinationIndex) | moveToAndFromValues;
            while (squaresBetween != 0)
            {
                var square = BitHelpers.BitScanForward(squaresBetween);
                if (Bitboard.IsSquareAttackedByColor(square, opponentColor, occupancy)) return true;
                squaresBetween &= squaresBetween - 1;
            }
            return false;
        }
    }
}
