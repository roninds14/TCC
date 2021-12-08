using tabuleiro;

namespace xadrez
{
    class Peao : Peca
    {
        private PartidaDeXadrez partida;
        public Peao(Tabuleiro tab, Cor cor, PartidaDeXadrez partida) : base(tab, cor) {
            this.partida = partida;
            peso = 0;
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
    }
}
