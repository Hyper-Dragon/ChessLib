﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicBitboard.Enums;
namespace MagicBitboard
{
    public static class MoveHelpers
    {
        static MoveHelpers()
        {
            InitializeFileMasks();
            InitializRankMasks();
        }
        private static void InitializeFileMasks()
        {
            var start = (ulong)0x8080808080808080;
            for (int f = 0; f <= 7; f++)
            {
                FileMasks[f] = start >> f;
            }
        }
        private static void InitializRankMasks()
        {
            var start = (ulong)0xFF;
            for (int r = 0; r <= 7; r++)
            {
                RankMasks[r] = start << (r * 8);
            }
        }
        public static int ToInt(this Color c) => (int)c;
        public static int ToInt(this File f) => (int)f;
        public static int ToInt(this Rank r) => (int)r;
        public static ulong[] FileMasks = new ulong[8];
        public static ulong[] RankMasks = new ulong[8];
        public static File GetFile(this int square)
        {
            return (File)(square % 8);
        }

        public static Rank Rank(this int square)
        {
            var r = square / 8;
            return (Rank)(square / 8);
        }

        public static ulong Not(this ulong u) => ~(u);
        public static ulong Shift2E(this ulong u) { return u >> 2 & ~FileMasks[File.A.ToInt()] & ~FileMasks[File.B.ToInt()]; }
        public static ulong ShiftE(this ulong u) { return u >> 1 & ~FileMasks[File.A.ToInt()]; }
        public static ulong ShiftN(this ulong u) { return u << (8); }
        public static ulong Shift2N(this ulong u) { return u << 16; }
        public static ulong Shift2S(this ulong u) { return u >> 16; }
        public static ulong ShiftS(this ulong u, int count = 1) { return u >> (count * 8); }
        public static ulong ShiftNE(this ulong u) { return u.ShiftE().ShiftN(); }
        public static ulong ShiftSE(this ulong u) { return u.ShiftE().ShiftS(); }
        public static ulong ShiftSW(this ulong u) { return u.ShiftW().ShiftS(); }
        public static ulong ShiftNW(this ulong u) { return u.ShiftW().ShiftN(); }
        public static ulong ShiftNNE(this ulong u) { return u.ShiftE().Shift2N(); }
        public static ulong ShiftSSE(this ulong u) { return u.ShiftE().ShiftS(2); }
        public static ulong ShiftENE(this ulong u) { return u.Shift2E().ShiftN(); }
        public static ulong ShiftESE(this ulong u) { return u.Shift2E().ShiftS(); }
        public static ulong ShiftW(this ulong u) { return (ulong)(u << 1) & ~FileMasks[File.H.ToInt()]; }
        public static ulong Shift2W(this ulong u) { return (ulong)(u << 2) & ~FileMasks[File.H.ToInt()] & ~FileMasks[File.G.ToInt()]; }
        public static ulong ShiftNNW(this ulong u) { return u.ShiftW().Shift2N(); }
        public static ulong ShiftSSW(this ulong u) { return u.ShiftW().ShiftS(2); }
        public static ulong ShiftWSW(this ulong u) { return u.Shift2W().ShiftN(); }
        public static ulong ShiftWNW(this ulong u) { return u.Shift2W().ShiftS(); }


        public static string PrintBoardHtml(string htmlBoards)
        {
            var htmlFormat = string.Format(htmlMain, htmlStyles, htmlBoards);
            return htmlFormat;
        }

        public static string MakeBoardTable(this ulong u, Rank pieceRank, File pieceFile, string header = "", string pieceRep = "*", string attackSquareRep = "^")
        {
            var sb = new StringBuilder("<table class=\"chessboard\">\r\n");
            if (header != string.Empty)
            {
                sb.AppendLine($"<caption>{header}</caption>");
            }
            const string squareFormat = "<td id=\"{1}{0}\" class=\"{3}\">{2}</td>";
            string board = Convert.ToString((long)u, 2).PadLeft(64, '0');
            for (Rank r = Enums.Rank.R8; r >= Enums.Rank.R1; r--)
            {
                var rank = Math.Abs(r.ToInt() - 7);
                sb.AppendLine($"<tr id=\"rank{rank}\">");

                for (File f = File.A; f <= File.H; f++)
                {
                    var file = f.ToInt();

                    var pieceAtSquare = (f == pieceFile && r == pieceRank) ? pieceRep.ToString() : board[(rank * 8) + file] == '1' ? attackSquareRep.ToString() : "&nbsp;";
                    sb.AppendFormat(squareFormat, f.ToString(), rank, pieceAtSquare, board[(rank * 8) + file] == '1' ? "altColor" : "");
                }
                sb.Append("\r\n</tr>\r\n");
            }
            sb.AppendLine("</table>");
            return sb.ToString();
        }
        const string htmlMain = @"<!DOCTYPE html>
<html>
    <head>
        <title>Chess Boards</title>
        {0}
    </head>
    <body>
    {1}
    </body>
</html>";
        private static string htmlStyles = @"
<style>
    * 
    {
        margin: 0; 
        padding: 0; 
    }

    table { 
        border-collapse: collapse; 
        border-spacing: 0; 
    }

    .chessboard { 
        padding: 0px; 
        margin: 0 auto; 
        border: 2px solid #181818; 
    }

    .chessboard tr td {
        font-size: 44px;
        width: 60px; 
        height: 60px; 
        text-align: center;
        vertical-align: middle;
        
    }

    .chessboard tr:nth-child(2n) td:nth-child(2n+1) { 
        background: #9f9f9f; 
    }

    .chessboard tr:nth-child(2n+1) td:nth-child(2n) { 
        background: #9f9f9f; 
    } 
    

</style>
";

        public static string PrintBoard(this ulong u, string header = "", char replaceOnesWith = '1')
        {
            if (!string.IsNullOrWhiteSpace(header))
                header = header + "\r\n";
            var sb = new StringBuilder(header);

            var str = Convert.ToString((long)u, 2).PadLeft(64, '0');
            //if (highlightRank.HasValue && highlightFile.HasValue)
            //{
            //    var r = highlightFile.Value.ToInt();
            //    r = 8 - r;
            //    var f = highlightFile.Value.ToInt();
            //    var position = (r * 8) + f;
            //    if (position == 0)
            //    {
            //        str = "*" + str.Substring(1);
            //    }
            //    else { str = str.Substring(0, position - 1) + "*" + str.Substring(position); }
            //}
            var lRanks = new List<string>();
            var footerHeader = "";

            for (char c = 'a'; c <= 'h'; c++)
                footerHeader += "  " + c.ToString();
            var boardBorder = string.Concat(Enumerable.Repeat("-", footerHeader.Length + 3));
            footerHeader = " " + footerHeader;
            sb.AppendLine(footerHeader);
            sb.AppendLine(boardBorder);
            for (var i = 0; i < 8; i++)
            {
                var rankString = (8 - i).ToString();
                var rank = str.Skip(i * 8).Take(8).Select(x => x.ToString().Replace('1', replaceOnesWith));
                sb.AppendLine(rankString + " | " + string.Join(" | ", rank) + " |");
            }
            sb.AppendLine(boardBorder);
            sb.AppendLine(footerHeader);
            return sb.ToString();
        }
    }
}
