using System;
using System.Collections.Generic;
using System.Text;

using tabuleiro;

namespace xadrez
{
    class PosicaoXadrez
    {
        public char coluna { get; set; }
        public int linha { get; set; }

        public PosicaoXadrez(char coluna, int linha)
        {
            this.linha = linha;
            this.coluna = coluna;
        }

        public Posicao toPosicao()
        {
            return new Posicao(6 - linha, coluna - 'a');
        }

        public override string ToString()
        {
            return "" + coluna + linha;
        }
    }
}
