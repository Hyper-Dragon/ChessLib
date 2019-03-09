﻿using MagicBitboard;
using MagicBitboard.Enums;
using MagicBitboard.SlidingPieces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Bitboard.Tests.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var bb = new MagicBitboard.Bitboard();


            //Console.WriteLine(bb.KnightAttackMask[Rank.R1.ToInt(), File.A.ToInt()].PrintBoard("a1 knight Attack", Rank.R1, File.A));
            //Console.WriteLine(bb.KnightAttackMask[Rank.R4.ToInt(), File.E.ToInt()].PrintBoard("e4 knight Attack", '*'));
            WritePawnMovesAndAttacks(bb);
            WriteKingAttacks(bb);
            WriteQueenAttacks(bb);
            WriteKnightAttacks(bb);
            WriteBishopAttacks(bb);
            WriteRookAttacks(bb);
            var br = new BoardRepresentation("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            var board = br[Color.White, Piece.Knight].MakeBoardTable("Initial White Knight Position", MoveHelpers.HtmlPieceRepresentations[Color.White][Piece.Knight]);
            var html = MoveHelpers.PrintBoardHtml(board);
            System.IO.File.WriteAllText("InitialWhiteKnights.html", html);

            var fullBoard = br.MakeBoardTable();
            html = MoveHelpers.PrintBoardHtml(fullBoard);
            System.IO.File.WriteAllText("initialBoard.html", html);

            board = br[Color.Black, Piece.Pawn].MakeBoardTable("Initial Black Pawns Position", MoveHelpers.HtmlPieceRepresentations[Color.Black][Piece.Pawn]);
            html = MoveHelpers.PrintBoardHtml(board);
            System.IO.File.WriteAllText("InitialBlackPawns.html", html);
            //Console.ReadKey();
        }

        private static void WriteBishopAttacks(MagicBitboard.Bitboard bb)
        {
            const string message = "Bishop Moves/Attacks";
            StringBuilder sb = new StringBuilder(message + "\r\n");
            var bishop = new BishopPatterns();
            var regMs = new List<double>();
            var arrayMs = new List<double>();
            for (var i = 0; i < 64; i++)
            {
                var file = MoveHelpers.GetFile(i);
                var rank = MoveHelpers.GetRank(i);
                var attack = bb.BishopAttackMask[rank.ToInt(), file.ToInt()];

                sb.AppendLine(attack.MakeBoardTable(i, $"{file.ToString().ToLower()}{rank.ToString()[1]} {message}", MoveHelpers.HtmlPieceRepresentations[Color.White][Piece.Bishop], "&#9670;"));
                for (int occupancyIndex = 0; occupancyIndex < bishop.OccupancyAndMoveBoards[i].Length; occupancyIndex++)
                {
                    var occupancy = bishop.OccupancyAndMoveBoards[i][occupancyIndex].Occupancy;
                    var legalMovesForOccupancy = bishop.OccupancyAndMoveBoards[i][occupancyIndex].MoveBoard;
                    var dtReg = DateTime.Now;
                    var ob = bishop.GetLegalMoves((uint)i, bishop.OccupancyAndMoveBoards[i][occupancyIndex].Occupancy);
                    regMs.Add(DateTime.Now.Subtract(dtReg).TotalMilliseconds);
                    dtReg = DateTime.Now;
                    var obFromQuery = bishop.OccupancyAndMoveBoards[i].FirstOrDefault(x => x.Occupancy == occupancy).MoveBoard;
                    arrayMs.Add(DateTime.Now.Subtract(dtReg).TotalMilliseconds);
                    Debug.Assert(bishop.OccupancyAndMoveBoards[i][occupancyIndex].MoveBoard == ob);
                }
            }
            Debug.WriteLine($"Avg time to get legal moves for bishop from magics: {regMs.Average()}");
            Debug.WriteLine($"Avg time to get legal moves for bishop from linq query: {arrayMs.Average()}");

            var html = MoveHelpers.PrintBoardHtml(sb.ToString());
            System.IO.File.WriteAllText("BishopMoves.html", html);
        }

        private static void WriteRookAttacks(MagicBitboard.Bitboard bb)
        {
            const string message = "Rook Moves/Attacks";
            StringBuilder sb = new StringBuilder(message + "\r\n");

            var masks = new List<ulong>();
            var dtStart = DateTime.Now;
            var rook = new RookPatterns();
            var totalMS = (DateTime.Now - dtStart).TotalMilliseconds;
            var regMs = new List<double>();
            var arrayMs = new List<double>();
            for (var i = 0; i < 64; i++)
            {
                ulong attackMask = rook[i];
                for (int occupancyIndex = 0; occupancyIndex < rook.OccupancyAndMoveBoards[i].Length; occupancyIndex++)
                {
                    var occupancy = rook.OccupancyAndMoveBoards[i][occupancyIndex].Occupancy;
                    var legalMovesForOccupancy = rook.OccupancyAndMoveBoards[i][occupancyIndex].MoveBoard;
                    var dtReg = DateTime.Now;
                    var ob = rook.GetLegalMoves((uint)i, occupancy);
                    regMs.Add(DateTime.Now.Subtract(dtReg).TotalMilliseconds);
                    dtReg = DateTime.Now;
                    var obFromQuery = rook.OccupancyAndMoveBoards[i].FirstOrDefault(x => x.Occupancy == occupancy).MoveBoard;
                    arrayMs.Add(DateTime.Now.Subtract(dtReg).TotalMilliseconds);
                    Debug.Assert(legalMovesForOccupancy == ob);
                }
                sb.AppendLine(rook[i].MakeBoardTable(i, $"{i.IndexToSquareDisplay()} {message}", MoveHelpers.HtmlPieceRepresentations[Color.White][Piece.Rook], "&#9670;"));
            }
            var regAvg = regMs.Average();
            Debug.WriteLine($"Avg time to get legal moves for rook from magics: {regAvg}");
            Debug.WriteLine($"Avg time to get legal moves for rook from linq query: {arrayMs.Average()}");

            var html = MoveHelpers.PrintBoardHtml(sb.ToString());
            System.IO.File.WriteAllText("RookMoves.html", html);
        }

        private static void WriteQueenAttacks(MagicBitboard.Bitboard bb)
        {
            const string message = "Queen Moves/Attacks";
            var queen = new QueenPatterns();
            var regMs = new List<double>();
            var arrayMs = new List<double>();
            StringBuilder sb = new StringBuilder(message + "\r\n");
            for (var i = 0; i < 64; i++)
            {
                ulong attackMask = queen[i];
                for (int occupancyIndex = 0; occupancyIndex < queen.OccupancyAndMoveBoards[i].Length; occupancyIndex++)
                {
                    var occupancy = queen.OccupancyAndMoveBoards[i][occupancyIndex].Occupancy;
                    var legalMovesForOccupancy = queen.OccupancyAndMoveBoards[i][occupancyIndex].MoveBoard;
                    var dtReg = DateTime.Now;
                    var ob = queen.GetLegalMoves((uint)i, occupancy);
                    regMs.Add(DateTime.Now.Subtract(dtReg).TotalMilliseconds);
                    dtReg = DateTime.Now;
                    var obFromQuery = queen.OccupancyAndMoveBoards[i].FirstOrDefault(x => x.Occupancy == occupancy).MoveBoard;
                    arrayMs.Add(DateTime.Now.Subtract(dtReg).TotalMilliseconds);
                    Debug.Assert(legalMovesForOccupancy == ob);
                }
            }
            Debug.WriteLine($"Avg time to get legal moves for queen from magics: { regMs.Average()}");
            Debug.WriteLine($"Avg time to get legal moves for queen from linq query: {arrayMs.Average()}");
            var html = MoveHelpers.PrintBoardHtml(sb.ToString());
            System.IO.File.WriteAllText("QueenMoves.html", html);
        }


        private static void WriteKnightAttacks(MagicBitboard.Bitboard bb)
        {
            const string message = "Knight Moves/Attacks";
            StringBuilder sb = new StringBuilder(message + "\r\n");
            for (var i = 0; i < 64; i++)
            {
                var file = MoveHelpers.GetFile(i);
                var rank = MoveHelpers.GetRank(i);
                sb.AppendLine(bb.KnightAttackMask[rank.ToInt(), file.ToInt()].MakeBoardTable(i, $"{file.ToString().ToLower()}{rank.ToString()[1]} {message}", MoveHelpers.HtmlPieceRepresentations[Color.White][Piece.Knight], "&#9670;"));
            }
            var html = MoveHelpers.PrintBoardHtml(sb.ToString());
            System.IO.File.WriteAllText("KnightMoves.html", html);
        }


        private static void WriteKingAttacks(MagicBitboard.Bitboard bb)
        {
            const string message = "King Moves/Attacks";
            StringBuilder sb = new StringBuilder(message + "\r\n");
            for (var i = 0; i < 64; i++)
            {
                var file = MoveHelpers.GetFile(i);
                var rank = MoveHelpers.GetRank(i);
                sb.AppendLine(bb.KingMoveMask[rank.ToInt(), file.ToInt()].MakeBoardTable(i, $"{file.ToString().ToLower()}{rank.ToString()[1]} {message}", MoveHelpers.HtmlPieceRepresentations[Color.White][Piece.King], "&#9670;"));
            }
            var html = MoveHelpers.PrintBoardHtml(sb.ToString());
            System.IO.File.WriteAllText("KingMoves.html", html);
        }

        private static void WritePawnMovesAndAttacks(MagicBitboard.Bitboard bb)
        {
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < 64; i++)
            {
                var file = MoveHelpers.GetFile(i);
                var rank = MoveHelpers.GetRank(i);
                if (rank == Rank.R1 || rank == Rank.R8) continue;
                sb.AppendLine(bb.PawnAttackMask[Color.White.ToInt(), rank.ToInt(), file.ToInt()].MakeBoardTable(i, $"{file.ToString().ToLower()}{rank.ToString()[1]} White Pawn Attack", MoveHelpers.HtmlPieceRepresentations[Color.White][Piece.Pawn], "&#9670;"));
            }
            var html = MoveHelpers.PrintBoardHtml(sb.ToString());
            System.IO.File.WriteAllText("WhitePawnAttack.html", html);

            sb.Clear();
            for (var i = 0; i < 64; i++)
            {
                var file = MoveHelpers.GetFile(i);
                var rank = MoveHelpers.GetRank(i);
                if (rank == Rank.R1 || rank == Rank.R8) continue;
                sb.AppendLine(bb.PawnAttackMask[Color.Black.ToInt(), rank.ToInt(), file.ToInt()].MakeBoardTable(i, $"{file.ToString().ToLower()}{rank.ToString()[1]} Black Pawn Attack", MoveHelpers.HtmlPieceRepresentations[Color.Black][Piece.Pawn], "&#9670;"));
            }
            html = MoveHelpers.PrintBoardHtml(sb.ToString());
            System.IO.File.WriteAllText("BlackPawnAttack.html", html);
            sb.Clear();
            for (var i = 0; i < 64; i++)
            {
                var file = MoveHelpers.GetFile(i);
                var rank = MoveHelpers.GetRank(i);
                if (rank == Rank.R1 || rank == Rank.R8) continue;
                sb.AppendLine(bb.PawnMoveMask[Color.White.ToInt(), rank.ToInt(), file.ToInt()].MakeBoardTable(i, $"{file.ToString().ToLower()}{rank.ToString()[1]} White Pawn Move", MoveHelpers.HtmlPieceRepresentations[Color.White][Piece.Pawn], "&#9678;"));
            }
            html = MoveHelpers.PrintBoardHtml(sb.ToString());
            System.IO.File.WriteAllText("WhitePawnMove.html", html);
            sb.Clear();
            for (var i = 0; i < 64; i++)
            {
                var file = MoveHelpers.GetFile(i);
                var rank = MoveHelpers.GetRank(i);
                if (rank == Rank.R1 || rank == Rank.R8) continue;
                sb.AppendLine(bb.PawnMoveMask[Color.Black.ToInt(), rank.ToInt(), file.ToInt()].MakeBoardTable(i, $"{file.ToString().ToLower()}{rank.ToString()[1]} Black Pawn Move", MoveHelpers.HtmlPieceRepresentations[Color.Black][Piece.Pawn], "&#9678;"));
            }
            html = MoveHelpers.PrintBoardHtml(sb.ToString());
            System.IO.File.WriteAllText("BlackPawnMove.html", html);
        }
    }
}
