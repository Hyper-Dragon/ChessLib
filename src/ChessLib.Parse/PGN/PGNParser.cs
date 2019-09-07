﻿using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ChessLib.Data;
using ChessLib.Data.Boards;
using ChessLib.Data.MoveRepresentation;
using ChessLib.Data.Types.Interfaces;
using ChessLib.Parse.PGN.Parser;
using ChessLib.Parse.PGN.Parser.BaseClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChessLib.Parse.PGN
{
    public class PGNParser
    {
        public TimeSpan TotalValidationTime;
        private int _total;
        private int _processed;
        private double _percentProcessed;

        public IEnumerable<Game<MoveStorage>> GetGamesFromPGN(string pgn) => ProcessGames(pgn, 1);

        public IEnumerable<Game<MoveStorage>> GetGamesFromPGN(Stream stream) => GetGamesFromPGNAsync(stream);

        public IEnumerable<Game<MoveStorage>> GetGamesFromPGNAsync(string pgn) => GetValidatedGames(new AntlrInputStream(pgn));

        public IEnumerable<Game<MoveStorage>> GetGamesFromPGNAsync(Stream stream) => GetValidatedGames(new AntlrInputStream(stream));

        private IEnumerable<string> GetIndividualGames(AntlrInputStream inputStream, int groupSize = 50)
        {
            var lexer = new PGNLexer(inputStream);
            lexer.RemoveErrorListeners();
            var parser = new Parser.BaseClasses.PGNParser(new CommonTokenStream(lexer));
            var parseTree = parser.parse();
            var walker = new ParseTreeWalker();
            var listener = new PGNGameListener(inputStream);
            walker.Walk(listener, parseTree);
            var games = listener.Games;
            _total = games.Count;
            var tmpGroupSize = _total / 20;
            groupSize = tmpGroupSize < 1 ? 1 : tmpGroupSize;
            _processed = 0;
            _percentProcessed = 0;
            var grouped = games
                .Select((x, i) => new { Item = x, Index = i })
                .GroupBy(x => x.Index / groupSize, x => x.Item).ToList();
            Debug.WriteLine($"Made {grouped} groups");
            var lines = grouped.Select((x, i) => $"[{i}].Count is {x.Count()}").ToList();
            Debug.WriteLine(string.Join(Environment.NewLine, lines));

            return grouped.Select(x => string.Join("", x));
        }

        protected List<Game<MoveStorage>> GetValidatedGames(AntlrInputStream inputStream)
        {
            List<Game<MoveStorage>> rv = new List<Game<MoveStorage>>();
            var gameGroups = GetIndividualGames(inputStream);
            //var tasks = gameGroups.Select((x, i) => ProcessGames(x, i)).ToArray();
            Parallel.ForEach(gameGroups, group =>
            {
                var games = ProcessGames(group);
                rv.AddRange(games);
            });
            return rv;
        }



        protected List<Game<MoveStorage>> ProcessGames(string gameGroup, int? index = null)
        {
            if (index.HasValue)
            {
                Debug.WriteLine($"Starting group index {index}");
            }
            var lexer = new PGNLexer(new AntlrInputStream(gameGroup));
            lexer.RemoveErrorListeners();
            var parser = new Parser.BaseClasses.PGNParser(new CommonTokenStream(lexer));
            var parseTree = parser.parse();
            var walker = new ParseTreeWalker();
            var listener = new PGNGameDetailListener();
            listener.GameProcessed += OnGameProcessed;
            walker.Walk(listener, parseTree);
            listener.Games.ForEach(game => game.GoToInitialState());
            if (index.HasValue)
            {
                Debug.WriteLine($"Finished group index {index}");
            }
            return listener.Games;

        }

        private void OnGameProcessed(object sender, EventArgs e)
        {
            _processed++;
            var percentProcessed = Math.Round((float)_processed / _total, 4);
            if (percentProcessed > _percentProcessed)
            {
                _percentProcessed = percentProcessed;
                if (ProgressUpdate != null)
                {
                    ProgressUpdate.Invoke(this, _percentProcessed);
                }
            }
        }

        public event EventHandler<double> ProgressUpdate;
    }
}
