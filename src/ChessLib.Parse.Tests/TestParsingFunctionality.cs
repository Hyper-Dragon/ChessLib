﻿using System.IO;
using ChessLib.Data;
using ChessLib.Parse.PGN;
using NUnit.Framework;

namespace ChessLib.Parse.Tests
{
    [TestFixture]
    public class TestParsingFunctionality
    {
        string commentedPgn;
        string unCommentedPgn;
        string variationPgn;
        string fullGame01Pgn;
        //string tagsNewLines = @"
        //        [Event ""The Big Test Event""]
        //        [Site ""My City, NY""]
        //        [Date ""2019.01.04""]
        //        [Round ""3""]
        //        [White ""Me""]
        //        [Black ""The other guy""]
        //        [Result ""1-0""]
        //        ";
        //string tagsNoNewLines = @"[Event ""The Big Test Event""][Site ""My City, NY""][Date ""2019.01.04""][Round ""3""][White ""Me""][Black ""The other guy""][Result ""1-0""]";
        //string tagsRandomWhiteSpace = "[Event  \"The Big Test Event\"]\t\t[Site \"My City, NY  \"]\r\n\r\n[Date \"2019.01.04\"][Round \"3\"][White              \"Me\"][Black\"The other guy\"][Result \t\t   \"1-0\"]";
        Tags expectedTags;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            commentedPgn = File.ReadAllText(".\\PGN\\commented.pgn");
            unCommentedPgn = File.ReadAllText(".\\PGN\\uncommented.pgn");
            variationPgn = File.ReadAllText(".\\PGN\\withvariations.pgn");
            fullGame01Pgn = File.ReadAllText(".\\PGN\\fullgame01.pgn");
            expectedTags = new Tags
            {
                { "Event", "The Big Test Event" },
                { "Site", "My City, NY" },
                { "Date", "2019.01.04" },
                { "Round", "3" },
                { "White", "Me" },
                { "Black", "The other guy" },
                { "Result", "1-0" }
            };
        }
        //[Test]
        //public void ShouldRetrieveTagsWithNewLines()
        //{
        //    var actualTags = GetTagValues(tagsNewLines);
        //    Assert.AreEqual(expectedTags, actualTags);
        //}
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
