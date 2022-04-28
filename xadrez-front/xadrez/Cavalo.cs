using tabuleiro;

namespace xadrez
{
    class Cavalo : Peca
    {
        public Cavalo(Tabuleiro tab, Cor cor) : base(tab, cor) {
            peso = -2;
        }

        public override string ToString()
        {
            return "C";
        }

        private bool podeMover(Posicao pos)
        {
            Peca p = tab.peca(pos);
            return p == null || p.cor != cor;
        }

        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[tab.linhas, tab.colunas];

            Posicao pos = new Posicao(0, 0);

            //acima-esquerda
            pos.definirValores(posicao.linha - 2, posicao.coluna - 1 );
            if (tab.posicaoValida(pos) && podeMover(pos)) mat[pos.linha, pos.coluna] = true;

            //acima-direita
            pos.definirValores(posicao.linha - 2, posicao.coluna + 1 );
            if (tab.posicaoValida(pos) && podeMover(pos)) mat[pos.linha, pos.coluna] = true;

            //direita-cima
            pos.definirValores(posicao.linha - 1, posicao.coluna + 2);
            if (tab.posicaoValida(pos) && podeMover(pos)) mat[pos.linha, pos.coluna] = true;

            //direita-baixo
            pos.definirValores(posicao.linha + 1, posicao.coluna + 2);
            if (tab.posicaoValida(pos) && podeMover(pos)) mat[pos.linha, pos.coluna] = true;

            //baixo-esquerda
            pos.definirValores(posicao.linha + 2, posicao.coluna - 1);
            if (tab.posicaoValida(pos) && podeMover(pos)) mat[pos.linha, pos.coluna] = true;

            //baixo-direita
            pos.definirValores(posicao.linha + 2, posicao.coluna + 1);
            if (tab.posicaoValida(pos) && podeMover(pos)) mat[pos.linha, pos.coluna] = true;

            //esquerda-cima
            pos.definirValores(posicao.linha - 1, posicao.coluna - 2);
            if (tab.posicaoValida(pos) && podeMover(pos)) mat[pos.linha, pos.coluna] = true;

            //esquerda-baixo
            pos.definirValores(posicao.linha + 1, posicao.coluna - 2);
            if (tab.posicaoValida(pos) && podeMover(pos)) mat[pos.linha, pos.coluna] = true;


            return mat;
        }

        public override int getTableState(Cor cor)
        {
            int[,] table;

            if (cor == Cor.Branca)
            {
                table = new int[,] {
                    { 20, 30, 10, 0, 0, 10, 30, 20 },
                    { 20, 20, 0, 0, 0, 0, 20, 20 },
                    { -10, -20, -20, -20, -20, -20, -20, -10 },
                    { -20, -30, -30, -40, -40, -30, -30, -20 },
                    { -30, -40, -40, -50, -50, -40, -40, -30 },
                    { -30, -40, -40, -50, -50, -40, -40, -30 },
                    { -30, -40, -40, -50, -50, -40, -40, -30 },
                    { -30, -40, -40, -50, -50, -40, -40, -30 }
                };
            }
            else
            {
                table = new int[,] {
                    { -30, -40, -40, -50, -50, -40, -40, -30 },
                    { -30, -40, -40, -50, -50, -40, -40, -30 },
                    { -30, -40, -40, -50, -50, -40, -40, -30 },
                    { -30, -40, -40, -50, -50, -40, -40, -30 },
                    { -20, -30, -30, -40, -40, -30, -30, -20 },
                    { -10, -20, -20, -20, -20, -20, -20, -10 },
                    { 20, 20, 0, 0, 0, 0, 20, 20 },
                    { 20, 30, 10, 0, 0, 10, 30, 20 }
                };
            }

            return table[this.posicao.linha, this.posicao.coluna];
        }
    }
}
