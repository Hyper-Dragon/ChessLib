﻿using System.Text;
using ChessLib.Core.Types.Enums;

namespace ChessLib.Core.Types.PgnExport
{
    public sealed class PgnMoveBuilder
    {
        private readonly StringBuilder moveTextBuilder;
        private readonly PGNFormatterOptions _options;
        public PgnMoveBuilder(PGNFormatterOptions options)
        {
            moveTextBuilder = new StringBuilder();
            _options = options;
        }

        public string BuildMove(PgnMoveInformation pgnMoveInformation, bool isBlackMoveNeeded = false)
        {
            BuildVariationStart(pgnMoveInformation).
            BuildMoveSeparator(pgnMoveInformation).
            BuildMoveValue(pgnMoveInformation, isBlackMoveNeeded).
            BuildEndOfVariation(pgnMoveInformation);
            var move = moveTextBuilder.ToString();
            moveTextBuilder.Clear();
            return move;
        }
        public PgnMoveBuilder BuildMoveValue(PgnMoveInformation pgnMoveInformation, bool isBlackMoveNeeded)
        {
            if (isBlackMoveNeeded)
            {
                moveTextBuilder.Append(pgnMoveInformation.MoveNumber)
                       .Append(pgnMoveInformation.ColorMakingMove == Color.Black ? "..." : ".")
                       .Append(_options.SpaceAfterMoveNumber ? _options.WhitespaceSeparator.ToString() : "");
            }
            moveTextBuilder.Append(pgnMoveInformation.MoveSan);
            return this;
        }

        private PgnMoveBuilder BuildVariationStart(PgnMoveInformation pgnMoveInformation)
        {
            if (pgnMoveInformation.FirstMoveInVariation)
            {
                if (_options.IndentVariations)
                {
                    moveTextBuilder.Append(_options.NewLineIndicator);
                }
                else
                {
                    moveTextBuilder.Append(_options.WhitespaceSeparator);
                }
                moveTextBuilder.Append("(").Append(_options.GetVariationPadding());
            }

            return this;
        }
        private PgnMoveBuilder BuildMoveSeparator(PgnMoveInformation pgnMoveInformation)
        {
            //if a newline after Black's move is indicated, begin the line with it.
            if (_options.NewlineAfterBlackMove && pgnMoveInformation.ColorMakingMove == Color.White)
            {
                moveTextBuilder.Append(_options.NewLineIndicator);
            }
            else
            {
                //if the type is not a variation beginning and it's not the first move, prefix with a space
                if (!pgnMoveInformation.FirstMoveInVariation)
                {
                    if (!pgnMoveInformation.IsFirstGameMove)
                    {
                        moveTextBuilder.Append(_options.WhitespaceSeparator);
                    }
                }
            }

            return this;
        }
        private void BuildEndOfVariation(PgnMoveInformation pgnMoveInformation)
        {
            if (pgnMoveInformation.IsLastMoveInVariation)
            {
                moveTextBuilder.Append(_options.GetVariationPadding()).Append(")");
                if (_options.IndentVariations)
                {
                    moveTextBuilder.Append(_options.NewLineIndicator);
                }
            }
        }
    }
}