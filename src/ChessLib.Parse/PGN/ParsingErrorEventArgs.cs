using System;
using System.Collections.Generic;
using System.Text;

namespace ChessLib.Parse.PGN
{
    public class ParsingErrorEventArgs : EventArgs
    {
        public ParsingErrorEventArgs()
        {

        }

        public ParsingErrorEventArgs(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; set; }
    }
}
