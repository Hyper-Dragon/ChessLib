﻿using MagicBitboard.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MagicBitboard
{
    public abstract class MoveInitializer : IMoveInitializer
    {
        private readonly MoveDirection _moveDirectionFlags;
        private Random _random = new Random();

        protected MoveInitializer(MoveDirection moveDirectionFlags)
        {
            _moveDirectionFlags = moveDirectionFlags;
        }

        public ulong NextRandom()
        {
            var leftPart = (ulong)_random.Next() << 32;
            var rightPart = (ulong)_random.Next();

            return leftPart | rightPart;
        }

        private ulong GetRandomKey()
        {
            return NextRandom() & NextRandom() & NextRandom();
        }

        public IEnumerable<BlockerAndMoveBoards> GetPermutationsForMask(ulong attackMask, IEnumerable<ulong> occupancyBoard, int pieceLocationIndex)
        {
            var boardCombos = new List<BlockerAndMoveBoards>();
            var dtStart = DateTime.Now;
            var totalBoards = occupancyBoard.Count();
            foreach (var board in occupancyBoard)
            {
                //Debug.Write(string.Format("\r{0,4} | {1,-4}", count++, totalBoards));
                boardCombos.Add(new BlockerAndMoveBoards(board, CalculateMovesFromPosition(pieceLocationIndex, board)));
            }
            return boardCombos;
        }

        //public abstract ulong CalculateMovesFromPosition(int positionIndex, ulong occupancyBoard);

        public ulong GenerateKey(BlockerAndMoveBoards[] blockerAndMoveBoards, int maskLength, out ulong[] attackArray)
        {
            var maxMoves = 1 << maskLength;
            attackArray = new ulong[maxMoves];

            var key = (ulong)0;
            var fail = true;
            var dtStart = DateTime.Now;
            var count = 1;
            while (fail)
            {
                key = GetRandomKey();
                fail = false;

                Array.Clear(attackArray, 0, maxMoves);

                foreach (var pattern in blockerAndMoveBoards)
                {
                    var hash = (pattern.Occupancy * key) >> (64 - maskLength);
                    if (attackArray[hash] != 0 && attackArray[hash] != pattern.MoveBoard)
                    {
                        fail = true;
                        count++;
                        break;
                    }

                    attackArray[hash] = pattern.MoveBoard;
                }
            }
            var totalMs = DateTime.Now.Subtract(dtStart).TotalMilliseconds;
            return key;
        }

        protected ulong CalculateMovesFromPosition(int positionIndex, ulong occupancyBoard)
        {
            var rv = (ulong)0;
            var startingValue = (ulong)1 << positionIndex;
            var positionalValue = startingValue;
            //N
            if (_moveDirectionFlags.HasFlag(MoveDirection.N))
            {

                while ((positionalValue = positionalValue.ShiftN()) != 0)
                {

                    rv |= positionalValue;
                    if ((occupancyBoard & positionalValue) == positionalValue) break;
                }
            }

            //E
            if (_moveDirectionFlags.HasFlag(MoveDirection.E))
            {
                positionalValue = startingValue;
                while ((positionalValue = positionalValue.ShiftE()) != 0)
                {
                    rv |= positionalValue;
                    if ((occupancyBoard & positionalValue) == positionalValue) break;
                }
            }

            //S
            if (_moveDirectionFlags.HasFlag(MoveDirection.S))
            {
                positionalValue = startingValue;
                while ((positionalValue = positionalValue.ShiftS()) != 0)
                {
                    rv |= positionalValue;
                    if ((occupancyBoard & positionalValue) == positionalValue) break;
                }
            }

            //W
            if (_moveDirectionFlags.HasFlag(MoveDirection.W))
            {
                positionalValue = startingValue;
                while ((positionalValue = positionalValue.ShiftW()) != 0)
                {
                    rv |= positionalValue;
                    if ((occupancyBoard & positionalValue) == positionalValue) break;
                }
                return rv;
            }

            //NE
            if (_moveDirectionFlags.HasFlag(MoveDirection.NE))
            {
                positionalValue = startingValue;
                while ((positionalValue = positionalValue.ShiftNE()) != 0)
                {
                    rv |= positionalValue;
                    if ((occupancyBoard & positionalValue) == positionalValue) break;
                }
            }

            //NW
            if (_moveDirectionFlags.HasFlag(MoveDirection.NW))
            {
                positionalValue = startingValue;
                while ((positionalValue = positionalValue.ShiftNW()) != 0)
                {
                    rv |= positionalValue;
                    if ((occupancyBoard & positionalValue) == positionalValue) break;
                }
            }

            //SE
            if (_moveDirectionFlags.HasFlag(MoveDirection.SE))
            {
                positionalValue = startingValue;
                while ((positionalValue = positionalValue.ShiftSE()) != 0)
                {
                    rv |= positionalValue;
                    if ((occupancyBoard & positionalValue) == positionalValue) break;
                }
            }

            //SW
            if (_moveDirectionFlags.HasFlag(MoveDirection.SW))
            {
                positionalValue = startingValue;
                while ((positionalValue = positionalValue.ShiftSW()) != 0)
                {
                    rv |= positionalValue;
                    if ((occupancyBoard & positionalValue) == positionalValue) break;
                }
            }
            return rv;
        }

        public static IEnumerable<ulong> GetAllPermutations(ulong mask)
        {
            var setBitIndices = BitHelpers.GetSetBits(mask);
            return GetAllPermutations(setBitIndices, 0, 0).Distinct();
        }

        private static IEnumerable<ulong> GetAllPermutations(List<int> SetBits, int Index, ulong Value)
        {
            BitHelpers.SetBit(ref Value, SetBits[Index]);
            yield return Value;
            int index = Index + 1;
            if (index < SetBits.Count)
            {
                using (IEnumerator<ulong> occupancyPermutations = GetAllPermutations(SetBits, index, Value).GetEnumerator())
                {
                    while (occupancyPermutations.MoveNext())
                    {
                        yield return occupancyPermutations.Current;
                    }
                }
            }
            BitHelpers.ClearBit(ref Value, SetBits[Index]);
            yield return Value;
            if (index < SetBits.Count)
            {
                using (IEnumerator<ulong> occupancyPermutations = GetAllPermutations(SetBits, index, Value).GetEnumerator())
                {
                    while (occupancyPermutations.MoveNext())
                    {
                        yield return occupancyPermutations.Current;
                    }
                }
            }

        }
    }
}