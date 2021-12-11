using tabuleiro;

namespace maquina
{
    class PesoMovimento
    {
        private int linha, coluna;
        public Peca peca { get; private set; }
        public MovePeso[,] movimentos { get; private set; }

        public PesoMovimento(Peca peca, int linha, int col)
        {
            this.linha = linha;
            coluna = col;
            this.peca = peca;
            movimentos = new MovePeso[linha, col];
        }

        public void setMovimentos( bool [,] movimentosPossiveis, int [,] pesosMovimentos )
        {
            for (int i = 0; i < linha; i++)
            {
                for (int j = 0; j < coluna; j++)
                {
                    movimentos[i, j] = new MovePeso(pesosMovimentos[i, j], movimentosPossiveis[i, j]);
                }
            }
        }
    }
}
