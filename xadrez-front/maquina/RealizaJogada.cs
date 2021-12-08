using System.Collections.Generic;
using tabuleiro;
using xadrez;

namespace maquina
{
    class RealizaJogada
    {
        private PartidaDeXadrez partida;
        private Cor jogadorMax;
        private Cor jogadorMin;
        private HashSet<PesoMovimento> movimentosMax;
        private HashSet<PesoMovimento> movimentosMin;

        public RealizaJogada(PartidaDeXadrez partida, Cor jogador)
        {
            this.partida = partida;
            this.jogadorMax = jogador;
            this.jogadorMin = partida.adversaria(jogador);           

            realizaMovimento();
                
        }

        private void realizaMovimento()
        {
            pesosMovimentosMax();
        }

        private void pesosMovimentosMax()
        {
            foreach (Peca x in partida.pecasEmJogo(jogadorMax))
            {
                bool[,] movimentosPossiveis = x.movimentosPossiveis();

                PesoMovimento pesoMovimento = new PesoMovimento(x, partida.tab.linhas, partida.tab.colunas);

                if (x.existeMovimentosPossiveis())
                {
                    pesoMovimento.setMovimentos(x.movimentosPossiveis());

                    for(int i = 0; i < partida.tab.linhas; i++ )
                    {
                        for(int j = 0; j <partida.tab.colunas; j++)
                        {
                            if (pesoMovimento.movimentos[i, j])
                            {
                                int valor = 0;

                                if(partida.tab.peca(i, j) != null) valor++;
                                if(estaProtegida(jogadorMax, x.posicao, new Posicao(i, j))) valor++;

                            }
                        }
                    }
                }
            }
        }

        private bool estaProtegida(Cor cor, Posicao peca, Posicao destino)
        {
            foreach (Peca x in partida.pecasEmJogo(cor))
            {
                if (Posicao.comparaPosicao(x.posicao, peca))
                {
                    bool[,] R = x.movimentosPossiveis();
                    if (R[destino.linha, destino.coluna]) return true;
                }
            }
                return false;
        }

        private void pesosMovimentosMin(PartidaDeXadrez partida) { }
    }
}

/************************************
   TABELA DE PONTUAÇÃO DE MOVIMENTO
 ************************************
 * Captura   Destino     Adversario
 * S        P           NA
 * S        D           NA
 * S        P           AP        
 * S        D           AP
 * S        P           ANP
 * S        D           ANP
 * N        P           NA
 * N        D           NA
 * N        P           AP        
 * N        D           AP
 * N        P           ANP
 * N        D           ANP
 * 
 * S    - sim (+1)
 * N    - não (+0)
 * P    - protegido (+1)
 * D    - desprotegito (+0)
 * NA   - não ameaça (+0)
 * AP   - ameça e fica protegido
 * ANP  - ameça e não fica protegito
 */