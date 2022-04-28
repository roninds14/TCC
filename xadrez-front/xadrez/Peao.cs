using System;
using tabuleiro;

namespace xadrez
{
    class Peao : Peca
    {
        private PartidaDeXadrez partida;
        private Boolean ameaca;
        public Peao(Tabuleiro tab, Cor cor, PartidaDeXadrez partida) : base(tab, cor) {
            this.partida = partida;
            peso = 0;
            ameaca = false;
        }

        public override string ToString()
        {
            return "P";
        }       

        private bool podeMover(Posicao pos)
        {
            Peca p = tab.peca(pos);
            return p == null;
        }

        private bool podeCapturar(Posicao pos)
        {
            Peca p = tab.peca(pos);
            return p != null && p.cor != cor;
        }

        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[tab.linhas, tab.colunas];

            Posicao pos = new Posicao(0, 0);
            Posicao posAnt = new Posicao(0, 0);

            if (cor == Cor.Branca)
            {
                pos.definirValores(posicao.linha - 1, posicao.coluna);
                if (tab.posicaoValida(pos) && podeMover(pos)) mat[pos.linha, pos.coluna] = true;

                pos.definirValores(posicao.linha - 1, posicao.coluna -1);
                if (tab.posicaoValida(pos) && (podeCapturar(pos) || ameaca)) mat[pos.linha, pos.coluna] = true;

                pos.definirValores(posicao.linha - 1, posicao.coluna + 1);
                if (tab.posicaoValida(pos) && (podeCapturar(pos) || ameaca)) mat[pos.linha, pos.coluna] = true;

                if (qteMovimentos == 0)
                {
                    pos.definirValores(posicao.linha - 2, posicao.coluna);
                    posAnt.definirValores(posicao.linha - 1, posicao.coluna);
                    if (tab.posicaoValida(pos) && podeMover(pos) && podeMover(posAnt)) mat[pos.linha, pos.coluna] = true;                    
                }

                //en Passant
                if(posicao.linha == 3)
                {
                    Posicao esquerda = new Posicao(posicao.linha, posicao.coluna - 1);
                    if(tab.posicaoValida(esquerda) && podeCapturar(esquerda) && tab.peca(esquerda) == partida.pecaVulneravelEnPassant)
                    {
                        mat[esquerda.linha - 1, esquerda.coluna] = true;
                    }

                    Posicao direita = new Posicao(posicao.linha, posicao.coluna + 1);
                    if (tab.posicaoValida(direita) && podeCapturar(direita) && tab.peca(direita) == partida.pecaVulneravelEnPassant)
                    {
                        mat[direita.linha - 1, direita.coluna] = true;
                    }
                }
            }
            else
            {
                pos.definirValores(posicao.linha + 1, posicao.coluna);                
                if (tab.posicaoValida(pos) && podeMover(pos)) mat[pos.linha, pos.coluna] = true;

                pos.definirValores(posicao.linha + 1, posicao.coluna - 1);
                if (tab.posicaoValida(pos) && (podeCapturar(pos)||ameaca)) mat[pos.linha, pos.coluna] = true;
                
                pos.definirValores(posicao.linha + 1, posicao.coluna + 1);
                if (tab.posicaoValida(pos) && (podeCapturar(pos)||ameaca)) mat[pos.linha, pos.coluna] = true;

                if (qteMovimentos == 0)
                {
                    pos.definirValores(posicao.linha + 2, posicao.coluna);
                    posAnt.definirValores(posicao.linha + 1, posicao.coluna);
                    if (tab.posicaoValida(pos) && podeMover(pos) && podeMover(posAnt)) mat[pos.linha, pos.coluna] = true;                    
                }

                //en Passant
                if (posicao.linha == 4)
                {
                    Posicao esquerda = new Posicao(posicao.linha, posicao.coluna - 1);
                    if (tab.posicaoValida(esquerda) && podeCapturar(esquerda) && tab.peca(esquerda) == partida.pecaVulneravelEnPassant)
                    {
                        mat[esquerda.linha + 1, esquerda.coluna] = true;
                    }

                    Posicao direita = new Posicao(posicao.linha, posicao.coluna + 1);
                    if (tab.posicaoValida(direita) && podeCapturar(direita) && tab.peca(direita) == partida.pecaVulneravelEnPassant)
                    {
                        mat[direita.linha + 1, direita.coluna] = true;
                    }
                }
            }

            return mat;
        }

        public override bool[,] movimentosPossiveisAmeaca()
        {
            ameaca = true;
            bool[,] mat = new bool[tab.linhas, tab.colunas];
            mat = this.movimentosPossiveis();
            ameaca = false;
            return mat;
        }

        public override int getTableState(Cor cor)
        {
            int[,] table;

            if (cor == Cor.Branca)
            {
                table = new int[,] {
                    { 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 5, 10, 10, -20, -20, 10, 10, 5 },
                    { 5, -5, -10, 0, 0, -10, -5, 5 },
                    { 0, 0, 0, 20, 20, 0, 0, 0 },
                    { 5, 5, 10, 25, 25, 10, 5, 5 },
                    { 10, 10, 20, 30, 30, 20, 10, 10 },
                    { 50, 50, 50, 50, 50, 50, 50, 50 },
                    { 0, 0, 0, 0, 0, 0, 0, 0 }
                };
            }else
            {
                table = new int[,] {
                    { 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 50, 50, 50, 50, 50, 50, 50, 50 },
                    { 10, 10, 20, 30, 30, 20, 10, 10 },
                    { 5, 5, 10, 25, 25, 10, 5, 5 },
                    { 0, 0, 0, 20, 20, 0, 0, 0 },
                    { 5, -5, -10, 0, 0, -10, -5, 5 },
                    { 5, 10, 10, -20, -20, 10, 10, 5 },
                    { 0, 0, 0, 0, 0, 0, 0, 0 }
                };
            }

            return table[this.posicao.linha, this.posicao.coluna];            
        }
    }
}
