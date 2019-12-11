﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ChessLib.Data;
using ChessLib.Data.MoveRepresentation;
using ChessLib.Data.MoveRepresentation.NAG;
using ChessLib.Parse.PGN;
using NUnit.Framework;

namespace ChessLib.Parse.Tests
{
    [TestFixture]
    public class TestParsingFunctionality
    {
        private readonly PGNParser _parser = new PGNParser();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //  _finishedWithLargeDb = ParseLargeGame();
            //_withVariation = _parser.GetGamesFromPGNAsync(PGNResources.GameWithVariation).First();
        }

        private LinkedListNode<MoveStorage> GetNodeAt(int index, MoveTree tree)
        {
            var count = 0;
            var rv = tree.First;
            while (count < index)
            {
                rv = rv.Next;
                count++;
            }

            return rv;
        }

        private string MoveDisplay(int moveNumber, string SAN)
        {
            var str = (moveNumber / 2 + 1).ToString();
            str += moveNumber % 2 == 1 ? "... " : ". ";
            str += SAN;
            return str;
        }

        /// <summary>
        ///     An extensive test for comments, variations and nags in a real db scenario
        /// </summary>
        [Test]
        public void LongWait_TestParsingLargeDb()
        {
            var pgnDb = Encoding.UTF8.GetString(PGNResources.talMedium);
            var sw = new Stopwatch();
            sw.Start();
            var largeDb = _parser.GetGamesFromPGNAsync(pgnDb).Result.ToArray();
            sw.Stop();
            Debug.WriteLine($"Finished parsing {largeDb.Count()} games in {sw.ElapsedMilliseconds / 1000} seconds");
            const int expectedGameCount = 1000;
            Assert.AreEqual(expectedGameCount, largeDb.Length,
                $"Expected {expectedGameCount} games, but found {largeDb.Length}.");
        }

        [Test]
        public void TestColumnStylePGN()
        {
            var games = _parser.GetGamesFromPGNAsync(PGNResources.ColumnStyle).Result.ToArray();
            Assert.AreEqual(1, games.Length, $"Expected only one game, but found {games.Length}.");
            Assert.AreEqual(51, games[0].MainMoveTree.Count(), "Game should have 50 moves.");
        }

        [Test]
        public void TestCommentParsing()
        {
            const string expected = "Qc7 is a great move, here.";
            const int movePosition = 18; //Black's move 9...Qc7
            var game = _parser.GetGamesFromPGNAsync(PGNResources.GameWithNAG).Result.First();
            var move = game.MainMoveTree.ElementAt(movePosition);
            Assert.AreEqual(expected, move.Comment,
                $"Expected comment '{expected}' at move {MoveDisplay(movePosition, move.SAN)}.");
        }

        [Test]
        public void ShouldRespectMaxGameCount()
        {
            const int gamesToParse = 2;
            var pgnDb = Encoding.UTF8.GetString(PGNResources.talMedium);
            var parserOptions = new PGNParserOptions() { GameCountToParse = 2 };
            var parser = new PGNParser(parserOptions);
            var largeDb = parser.GetGamesFromPGNAsync(pgnDb).Result.ToArray();
            Assert.AreEqual(gamesToParse, largeDb.Length,
                $"Expected {gamesToParse} games, but found {largeDb.Length}.");
        }

        [Test]
        public void ShouldRespectMaxPlyCount()
        {
            const int maxPliesToParse = 10;
            var pgnDb = Encoding.UTF8.GetString(PGNResources.talMedium);
            var parserOptions = new PGNParserOptions() { GameCountToParse = 5, MaximumPlyPerGame = maxPliesToParse };
            var parser = new PGNParser(parserOptions);
            var largeDb = parser.GetGamesFromPGNAsync(pgnDb).Result.ToArray();
            foreach (var game in largeDb)
            {
                Assert.IsTrue(game.PlyCount <= maxPliesToParse, $"Expected ply count of {maxPliesToParse} but was {game.MainMoveTree.Count}.");
            }
        }

        [Test]
        public void ShouldNotParseVarsWhenMaxPlyCountIsSet()
        {
            const int maxPliesToParse = 10;
            var pgnDb = PGNResources.GameWithVars;
            var parserOptions = new PGNParserOptions() { GameCountToParse = PGNParserOptions.MaximumParseValue, MaximumPlyPerGame = maxPliesToParse };
            var parser = new PGNParser(parserOptions);
            var largeDb = parser.GetGamesFromPGNAsync(pgnDb).Result.ToArray();
            foreach (var move in largeDb.First().MainMoveTree)
            {
                Assert.IsEmpty(move.Variations, $"Found a variation on move {move.SAN} and shouldn't have any.");
            }
        }

        [Test]
        public void ShouldIgnoreVariationsWhenSetToTrue()
        {
            const int maxPliesToParse = 10;
            var pgnDb = PGNResources.GameWithVars;
            var parserOptions = new PGNParserOptions() { IgnoreVariations = true};
            var parser = new PGNParser(parserOptions);
            var largeDb = parser.GetGamesFromPGNAsync(pgnDb).Result.ToArray();
            foreach (var move in largeDb.First().MainMoveTree)
            {
                Assert.IsEmpty(move.Variations, $"Found a variation on move {move.SAN} and shouldn't have any.");
            }
        }

        [Test]
        public void TestNAGParsing()
        {
            const string expected = "$1";
            const int moveIndex = 16;
            var game = _parser.GetGamesFromPGNAsync(PGNResources.GameWithNAG).Result.First();
            var move = game.MainMoveTree.ElementAt(moveIndex);
            Assert.AreEqual(MoveNAG.GoodMove, move.Annotation.MoveNAG,
                $"Expected NAG to be '{expected}' at move {MoveDisplay(moveIndex, move.SAN)}.");
        }

        [Test]
        public void ShouldParsePreGameComment()
        {
            var game = _parser.GetGamesFromPGNAsync(PGNResources.PregameComment).Result.First();
            Assert.IsNotEmpty(game.MainMoveTree.First.Value.Comment);
        }

        [Test]
        public void TestRealGameParsing()
        {
            var pgn = PGNResources.GameWithVars;
            var game = _parser.GetGamesFromPGNAsync(pgn).Result.ToArray();
            Assert.AreEqual(1, game.Length, $"Expected only one game, but found {game.Length}.");
        }

        [Test]
        public void TestRealGameParsingVarsAndComments()
        {
            var pgn = PGNResources.VariationsAndComments;
            var game = _parser.GetGamesFromPGNAsync(pgn).Result.ToArray();
            Assert.AreEqual(1, game.Length, $"Expected only one game, but found {game.Length}.");
        }

        [Test]
        public void TestSimpleGameParsing()
        {
            var pgn = PGNResources.Simple;
            var game = _parser.GetGamesFromPGNAsync(pgn).Result.ToArray();
            Assert.AreEqual(1, game.Length, $"Expected only one game, but found {game.Length}.");
        }

        [Test]
        public void TestSmallPgnGame()
        {
            var games = _parser.GetGamesFromPGNAsync(PGNResources.smallPgn).Result.ToArray();
            Assert.AreEqual(1, games.Length, $"Expected only one game, but found {games.Length}.");
            //Assert.AreEqual(51, games[0].MainMoveTree.Count(), "Game should have 50 moves.");
        }


        [Test]
        public void TestVariationParsing()
        {
            const int variationOnMovePosition = 1;
            const int expectedVariationCount = 1;
            const string variationSAN = "c4";
            var withVarDb = _parser.GetGamesFromPGNAsync(PGNResources.GameWithVariation).Result.First();
            var variationMove = withVarDb.MainMoveTree.ElementAt(variationOnMovePosition);
            Assert.AreEqual(expectedVariationCount, variationMove.Variations.Count);
            Assert.AreEqual(variationSAN, variationMove.Variations[0].ElementAt(0).SAN);
        }

        //[Test]
        //public void ShouldRetrieveTagsWithNoNewLines()
        //{
        //    var actualTags = GetTagValues(tagsNoNewLines);
        //    Assert.AreEqual(expectedTags, actualTags);
        //}
        //[Test]
        //public void ShouldRetrieveTagsWithRandomWhitespace()
        //{
        //    var actualTags = GetTagValues(tagsRandomWhiteSpace);
        //    Console.WriteLine(tagsRandomWhiteSpace);
        //    Assert.AreEqual(expectedTags, actualTags);
        //}
        //[Test]
        //public void RemoveCommentsShouldRemoveAllComments()
        //{
        //    var s = RemoveTags(commentedPgn, out Dictionary<string, string> d);
        //    Assert.AreEqual(unCommentedPgn, RemoveComments(commentedPgn));
        //}
    }
}