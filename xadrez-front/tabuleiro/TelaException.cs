using System;
using System.Collections.Generic;
using System.Text;

namespace tabuleiro
{
    public class TelaException : Exception
    {
        public TelaException(string msg) : base(msg) { }
    }
}
