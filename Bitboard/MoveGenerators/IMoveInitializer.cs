﻿using System.Collections.Generic;

namespace MagicBitboard
{
    public interface IMoveInitializer
    {
        ulong CalculateMovesFromPosition(int positionIndex, ulong occupancyBoard);
        ulong GenerateKey(BlockerAndMoveBoards[] blockerAndMoveBoards, int maskLength, out ulong[] attackArray);
        IEnumerable<BlockerAndMoveBoards> GetPermutationsForMask(ulong attackMask, IEnumerable<ulong> n, int piecePositionIndex);
    }
}