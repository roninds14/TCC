using tabuleiro;

namespace xadrez
{
    class Peao : Peca
    {
        public Peao(Tabuleiro tab, Cor cor) : base(tab, cor) { }

        public override string ToString()
        {
            return "P";
        }

        private bool podeMover(Posicao pos)
        {
            Peca p = tab.peca(pos);
            return p == null || p.cor != cor;
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

            if(cor == Cor.Branca)
            {
                pos.definirValores(posicao.linha - 1, posicao.coluna);
                if (tab.posicaoValida(pos) && podeMover(pos)) mat[pos.linha, pos.coluna] = true;

                pos.definirValores(posicao.linha - 1, posicao.coluna -1);
                if (tab.posicaoValida(pos) && podeCapturar(pos)) mat[pos.linha, pos.coluna] = true;

                pos.definirValores(posicao.linha - 1, posicao.coluna + 1);
                if (tab.posicaoValida(pos) && podeCapturar(pos)) mat[pos.linha, pos.coluna] = true;

                if (qteMovimentos == 0)
                {
                    pos.definirValores(posicao.linha - 2, posicao.coluna);
                    if (tab.posicaoValida(pos) && podeMover(pos)) mat[pos.linha, pos.coluna] = true;

                    pos.definirValores(posicao.linha - 2, posicao.coluna - 1);
                    if (tab.posicaoValida(pos) && podeCapturar(pos)) mat[pos.linha, pos.coluna] = true;

                    pos.definirValores(posicao.linha - 2, posicao.coluna + 1);
                    if (tab.posicaoValida(pos) && podeCapturar(pos)) mat[pos.linha, pos.coluna] = true;
                }
            }
            else
            {
                pos.definirValores(posicao.linha + 1, posicao.coluna);
                if (tab.posicaoValida(pos) && podeMover(pos)) mat[pos.linha, pos.coluna] = true;

                pos.definirValores(posicao.linha + 1, posicao.coluna - 1);
                if (tab.posicaoValida(pos) && podeCapturar(pos)) mat[pos.linha, pos.coluna] = true;
                
                pos.definirValores(posicao.linha + 1, posicao.coluna + 1);
                if (tab.posicaoValida(pos) && podeCapturar(pos)) mat[pos.linha, pos.coluna] = true;

                if (qteMovimentos == 0)
                {
                    pos.definirValores(posicao.linha + 2, posicao.coluna);
                    if (tab.posicaoValida(pos) && podeMover(pos)) mat[pos.linha, pos.coluna] = true;

                    pos.definirValores(posicao.linha + 2, posicao.coluna - 1);
                    if (tab.posicaoValida(pos) && podeCapturar(pos)) mat[pos.linha, pos.coluna] = true;

                    pos.definirValores(posicao.linha + 1, posicao.coluna + 1);
                    if (tab.posicaoValida(pos) && podeCapturar(pos)) mat[pos.linha, pos.coluna] = true;
                }
            }

            return mat;
        }
    }
}
