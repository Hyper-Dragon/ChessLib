﻿using ChessLib.Data;
using ChessLib.Data.Exceptions;
using ChessLib.Data.Helpers;
using ChessLib.Data.MoveRepresentation;
using ChessLib.Types.Interfaces;

namespace ChessLib.Validators.MoveValidation.CastlingRules
{
    public class CastlingHasNoPiecesBlocking : IMoveRule
    {
        public MoveExceptionType? Validate(in IBoard boardInfo, in ulong[][] postMoveBoard, in IMoveExt move)
        {
            ulong piecesBetween;
            switch (move.DestinationIndex)
            {
                case 58:
                    piecesBetween = BoardHelpers.InBetween(56, 60);
                    break;
                case 62:
                    piecesBetween = BoardHelpers.InBetween(60, 63);
                    break;
                case 2:
                    piecesBetween = BoardHelpers.InBetween(0, 4);
                    break;
                case 6:
                    piecesBetween = BoardHelpers.InBetween(4, 7);
                    break;
                default:
                    throw new MoveException("Bad destination square for castling move.", MoveExceptionType.Castle_BadDestinationSquare, move, boardInfo.ActivePlayer);

            }
            if ((boardInfo.TotalOccupancy & piecesBetween) != 0)
            {
                return MoveExceptionType.Castle_OccupancyBetween;
            }
            return null;
        }
    }
}