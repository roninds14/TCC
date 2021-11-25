using System;
using System.Collections.Generic;
using System.Text;

namespace tabuleiro
{
    class TelaException : Exception
    {
        public TelaException(string msg) : base(msg) { }
    }
}
