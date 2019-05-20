﻿
using ChessLib.Data;
using ChessLib.Data.Exceptions;
using ChessLib.Data.Helpers;
using ChessLib.Data.MoveRepresentation;
using ChessLib.Data.Types;

namespace ChessLib.Validators.MoveValidation.MoveRules
{
    public class PieceCanMoveToDestination : IMoveRule
    {
        public MoveExceptionType? Validate(in BoardFENInfo boardInfo, in ulong[][] postMoveBoard, in MoveExt move)
        {

            //var piece = BoardHelpers.GetTypeOfPieceAtIndex(move.SourceIndex, boardInfo.PiecePlacement);
            //if (piece == null)
            //{
            //    return MoveExceptionType.ActivePlayerHasNoPieceOnSourceSquare;
            //}

            //var moves = Bitboard.GetPseudoLegalMoves(piece.Value, move.SourceIndex, boardInfo.ActiveOccupancy, boardInfo.OpponentOccupancy,
            //    boardInfo.ActivePlayer);

            //if ((moves & move.DestinationValue) == 0)
            //{
            //    return MoveExceptionType.BadDestination;
            //}
            return boardInfo.CanPieceMoveToDestination(move.SourceIndex, move.DestinationIndex)
                ? (MoveExceptionType?)null
                : MoveExceptionType.BadDestination;
            return null;
        }


    }
}
