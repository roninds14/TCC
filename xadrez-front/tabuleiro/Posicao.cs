using System;
using System.Collections.Generic;
using System.Text;

namespace tabuleiro
{
    public class Posicao
    {
        public int linha { get; set; }
        public int coluna { get; set; }

        public Posicao(int linha, int coluna)
        {
            this.linha = linha;
            this.coluna = coluna;
        }

        public void definirValores(int linha, int coluna)
        {
            this.linha = linha;
            this.coluna = coluna;
        }

        public override string ToString()
        {
            return this.linha + ", " + this.coluna;
        }

        public static bool comparaPosicao(Posicao p1, Posicao p2)
        {
            return p1.linha == p2.linha && p1.coluna == p2.coluna;
        }

        public string ToStringTabuleiro()
		{
            string linha = Math.Abs(this.linha - 8).ToString();
            char coluna = (char)('a' + this.coluna);

            return coluna + linha;
		}
    }
}
