using tabuleiro;

namespace xadrez
{
    public class Jogador
    {
        public Cor cor { get; private set; }
        public bool tipo { get; private set; }

        public Jogador( bool tipo, Cor cor)
        {
            this.cor = cor;
            this.tipo = tipo;
        }
    }
}
