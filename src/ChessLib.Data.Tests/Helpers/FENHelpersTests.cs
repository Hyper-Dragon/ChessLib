﻿using ChessLib.Data.Exceptions;
using ChessLib.Data.Types;
using EnumsNET;
using MagicBitboard;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessLib.Data.Helpers.Tests
{
    [TestFixture]
    public class FENHelpersTests
    {
        struct CastleInfo
        {
            public string CastlingAvailabilityString { get; set; }
            public CastlingAvailability CastlingAvailability { get; set; }
        }
        char[] DisallowedCastlingChars;
        CastleInfo[] CastleInfos;

        #region One Time Setup

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            SetupDisallowedChars();
            SetupAllowedCastlingStrings();
        }

        private void SetupAllowedCastlingStrings()
        {
            var permutationsOfCastlingChars = GetPermutations(FENHelpers.ValidCastlingStringChars.Take(4).ToArray());
            permutationsOfCastlingChars.Add("-");
            var dict = new Dictionary<char, CastlingAvailability>();
            var castlingInfos = new List<CastleInfo>();
            foreach (var ca in Enum.GetValues(typeof(CastlingAvailability)))
            {
                dict.Add(((CastlingAvailability)ca).AsString(EnumFormat.Description)[0], (CastlingAvailability)ca);
            }
            foreach (var perm in permutationsOfCastlingChars)
            {
                CastlingAvailability ca = 0;
                foreach (var c in perm)
                {
                    ca |= dict[c];
                }
                castlingInfos.Add(new CastleInfo() { CastlingAvailability = ca, CastlingAvailabilityString = perm });
            }
            CastleInfos = castlingInfos.ToArray();
        }

        private void SetupDisallowedChars()
        {
            List<char> chars = new List<char>();
            for (char c = 'a'; c < 'z'; c++)
            {
                if (FENHelpers.ValidCastlingStringChars.Contains(c)) continue;
                chars.Add(c);
            }
            for (char c = 'A'; c < 'Z'; c++)
            {
                if (FENHelpers.ValidCastlingStringChars.Contains(c)) continue;
                chars.Add(c);
            }
            DisallowedCastlingChars = chars.ToArray();
        }
        static List<string> GetPermutations(char[] set)
        {
            int n = set.Length;
            var storage = new List<string>();
            for (int i = n; i > 0; i--)
            {
                storage.AddRange(GetKCombs(set, i).Select(x => string.Concat(x)));
            }
            return storage;
        }

        static IEnumerable<IEnumerable<T>> GetKCombs<T>(IEnumerable<T> list, int length) where T : IComparable
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetKCombs(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        #endregion

        #region Constant Fields

        const string startingFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        const string fenFormat = "{0} {1} {2} {3} {4} {5}";
        const string ValidPiecePlacement = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
        const string ValidColor = "w";
        const string ValidCastling = "KQkq";
        const string ValidEnPassent = "-";
        const string ValidEnPassentSquare = "e6";
        const int ValidHalfMove = 0;
        const int ValidFullMove = 0;
        #endregion

        #region Delegates
        private delegate void ValidationDelegate(string s);

        private ValidationDelegate MainValidation = FENHelpers.ValidateFENString;
        #endregion

        #region Private Methods
        private string GetFENFromProvidedFENPieceInfo(string piecePlacement, string activeColor, string castling, string enPassent, int halfMove, int fullMove)
        {
            return string.Format(fenFormat, piecePlacement, activeColor, castling, enPassent, halfMove, fullMove);
        }

        private FENException AssertThrowsFenException(ValidationDelegate validate, string fen, FENError errorTypeExpected = FENError.NULL, string assertMessage = "")
        {
            FENException exc = new FENException(fen, FENError.NULL);
            Assert.Throws(typeof(FENException), () =>
            {
                try { validate(fen); }
                catch (FENException e) { exc = e; throw; }
            }, assertMessage);
            if (errorTypeExpected != FENError.NULL)
            {
                Assert.AreEqual(errorTypeExpected, exc.FENError);
            }
            return exc;
        }

        #endregion

        #region Tests
        [Test]
        public void ValidateCastlingAvailabilityString_ShouldThrowException_WhenGivenInvalidCastlingCharacters()
        {
            var message = "";
            foreach (var c in DisallowedCastlingChars)
            {
                Assert.AreEqual(FENError.CastlingUnrecognizedChar, FENHelpers.ValidateCastlingAvailabilityString(c.ToString()));
                AssertThrowsFenException(MainValidation, GetFENFromProvidedFENPieceInfo(ValidPiecePlacement, ValidColor, c.ToString(), ValidEnPassent, 0, 0));
            }
            Console.WriteLine(message);
        }

        [Test]
        public void ValidateCastlingAvailabilityString_ShouldNotHaveError_WhenCastlingAvailabilityStringIsValid()
        {
            foreach (var info in CastleInfos)
            {
                var s = string.Join(" | ", info.CastlingAvailability.GetFlags().Select(x => x.ToString()));
                Console.WriteLine($"{info.CastlingAvailabilityString} should equal {s}");
                Assert.AreEqual(FENError.NULL, FENHelpers.ValidateCastlingAvailabilityString(info.CastlingAvailabilityString));
            }
        }

        [Test]
        public void ValidateEnPassentSquare_ShouldThrowException_WhenEnPassentSqIsInvalid()
        {
            Assert.AreEqual(FENError.InvalidEnPassentSquare, FENHelpers.ValidateEnPassentSquare("4e"));
            Assert.AreEqual(FENError.InvalidEnPassentSquare, FENHelpers.ValidateEnPassentSquare("4"));
            Assert.AreEqual(FENError.InvalidEnPassentSquare, FENHelpers.ValidateEnPassentSquare("e9"));
            Assert.AreEqual(FENError.InvalidEnPassentSquare, FENHelpers.ValidateEnPassentSquare("i3"));
            Assert.AreEqual(FENError.InvalidEnPassentSquare, FENHelpers.ValidateEnPassentSquare("z8"));
            Assert.AreEqual(FENError.InvalidEnPassentSquare, FENHelpers.ValidateEnPassentSquare("--"));
            Assert.AreEqual(FENError.InvalidEnPassentSquare, FENHelpers.ValidateEnPassentSquare("12"));
            Assert.AreEqual(FENError.InvalidEnPassentSquare, FENHelpers.ValidateEnPassentSquare("ee"));
            // AssertThrowsFenException(MainValidation, GetFENFromProvidedFENPieceInfo(ValidPiecePlacement, ValidColor, ValidCastling, "e9", 0, 0), FENError.InvalidEnPassentSquare);
        }

        [Test]
        public void ValidateEnPassentSquare_ShouldNotReturnError_GivenValidSquare()
        {
            Assert.AreEqual(FENError.NULL, FENHelpers.ValidateEnPassentSquare("e6"));
            Assert.AreEqual(FENError.NULL, FENHelpers.ValidateEnPassentSquare("a3"));
            Assert.AreEqual(FENError.NULL, FENHelpers.ValidateEnPassentSquare("h3"));
            Assert.AreEqual(FENError.NULL, FENHelpers.ValidateEnPassentSquare("-"));
        }

        [Test]
        public void ValidateHalfmoveClock_ShouldReturnAppropriateFENError_GivenInvalidInput()
        {
            Assert.AreEqual(FENError.HalfmoveClock, FENHelpers.ValidateHalfmoveClock("-1"));
            Assert.AreEqual(FENError.HalfmoveClock, FENHelpers.ValidateHalfmoveClock(""));
            Assert.AreEqual(FENError.HalfmoveClock, FENHelpers.ValidateHalfmoveClock("-"));
        }

        [Test]
        public void ValidateFullMoveCounter_ShouldReturnAppropriateFENError_GivenInvalidInput()
        {
            Assert.AreEqual(FENError.FullMoveCounter, FENHelpers.ValidateFullMoveCounter("-1"));
            Assert.AreEqual(FENError.FullMoveCounter, FENHelpers.ValidateFullMoveCounter(""));
            Assert.AreEqual(FENError.FullMoveCounter, FENHelpers.ValidateFullMoveCounter("-"));
        }

        [Test]
        public void ValidateHalfmoveClock_ShouldReturnNullFENError_GivenValidInput()
        {
            Assert.AreEqual(FENError.NULL, FENHelpers.ValidateHalfmoveClock("1"));
            Assert.AreEqual(FENError.NULL, FENHelpers.ValidateHalfmoveClock("0"));
            Assert.AreEqual(FENError.NULL, FENHelpers.ValidateHalfmoveClock("3"));
        }

        [Test]
        public void ValidateFullMoveCounter_ShouldReturnNullFENError_GivenValidInput()
        {
            Assert.AreEqual(FENError.NULL, FENHelpers.ValidateFullMoveCounter("31"));
            Assert.AreEqual(FENError.NULL, FENHelpers.ValidateFullMoveCounter("0"));
            Assert.AreEqual(FENError.NULL, FENHelpers.ValidateFullMoveCounter("1"));
        }

        [Test]
        public void GetMoveNumberFromString_ShouldReturnAppropriateUINT_GivenValidInput()
        {
            Assert.AreEqual((uint?)24, FENHelpers.GetMoveNumberFromString("24"));
        }

        [Test]
        public void ValidateActiveColor_ShouldReturnAppropriateFENError_GivenInvalidInput()
        {
            Assert.AreEqual(FENError.InvalidActiveColor, FENHelpers.ValidateActiveColor("z"));
            Assert.AreEqual(FENError.InvalidActiveColor, FENHelpers.ValidateActiveColor(""));
        }

        [Test]
        public void ValidateActiveColor_ShouldReturnNullFENError_GivenValidInput()
        {
            Assert.AreEqual(FENError.NULL, FENHelpers.ValidateActiveColor("w"));
            Assert.AreEqual(FENError.NULL, FENHelpers.ValidateActiveColor("b"));
        }

        [Test]
        public void GetActiveColor_ShouldReturnAppropriateColor_GivenValidInput()
        {

            Assert.AreEqual(Color.White, FENHelpers.GetActiveColor("w"));
            Assert.AreEqual(Color.Black, FENHelpers.GetActiveColor("b"));
        }

        [Test]
        public void ValidatePiecePlacement_ShouldReturnAppropriateFENError_GivenInvalidPieceInput()
        {
            var fen = "rnbqkbnr/ppspppzp/8/8/8/8/PPPPPPPP/RNBQKBNR";
            Assert.AreEqual(FENError.PiecePlacementInvalidChars, FENHelpers.ValidatePiecePlacement(fen));
        }

        [Test]
        public void ValidatePiecePlacement_ShouldReturnAppropriateFENError_GivenInvalidRankCount()
        {

            var fen = ValidPiecePlacement;
            Assert.AreEqual(FENError.PiecePlacementRankCount, FENHelpers.ValidatePiecePlacement(fen + "/rnbqkbnr"));
            Assert.AreEqual(FENError.PiecePlacementRankCount, FENHelpers.ValidatePiecePlacement("rnbqkbnr/rnbqkbnr"));
        }

        [Test]
        public void ValidatePiecePlacement_ShouldReturnAppropriateFENError_GivenTooManyPieceInRank()
        {
            var fen = "rnbqkbnr/ppppppppp/8/8/8/8/PPPPPPPP/RNBQrKBNR";

            Assert.AreEqual(FENError.PiecePlacementPieceCountInRank, FENHelpers.ValidatePiecePlacement(fen));
        }

        [Test]
        public void ValidatePiecePlacement_ShouldReturnNullFENError_GivenValidPieceInput()
        {
            Assert.AreEqual(FENError.NULL, FENHelpers.ValidatePiecePlacement(ValidPiecePlacement));
        }

        #endregion
    }
}
