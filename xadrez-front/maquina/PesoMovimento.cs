using tabuleiro;

namespace maquina
{
    class PesoMovimento
    {
        public Peca peca { get; private set; }
        public int[,] pesos { get; private set; }
        public bool[,] movimentos { get; private set; }

        public PesoMovimento(Peca peca, int col, int linha)
        {
            this.peca = peca;
            pesos = new int[linha,col];
            movimentos = new bool[linha, col];
        }

        public void setMovimentos( bool [,] movimentosPossiveis )
        {
            this.movimentos = movimentosPossiveis;
        }

        public void setPesos( int [,] pesos)
        {
            this.pesos = pesos;
        }
    }
}
