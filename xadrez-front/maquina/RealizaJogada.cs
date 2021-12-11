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
            movimentosMax = new HashSet<PesoMovimento>();
            movimentosMin = new HashSet<PesoMovimento>();

            realizaMovimento();
                
        }

        private void realizaMovimento()
        {
            pesosMovimentos(jogadorMax);
            pesosMovimentos(jogadorMin);
        }

        private void pesosMovimentos(Cor cor)
        {
            foreach (Peca x in partida.pecasEmJogo(cor))
            {
                int[,] pesos = new int[partida.tab.linhas, partida.tab.colunas];

                PesoMovimento pesoMovimento = new PesoMovimento(x, partida.tab.linhas, partida.tab.colunas);

                if (x.existeMovimentosPossiveis())
                {
                    for(int i = 0; i < partida.tab.linhas; i++ )
                    {
                        for(int j = 0; j <partida.tab.colunas; j++)
                        {
                            if (x.movimentoPossivel(new Posicao(i,j)))
                            {
                                int valor = 0;

                                if(partida.tab.peca(i, j) != null) valor++;
                                if(estaProtegida(cor, x.posicao, new Posicao(i, j))) valor++;
                                estaAmeacada(partida.adversaria(cor), new Posicao(i, j), ref valor);

                                pesos[i, j] = valor;
                            }
                        }
                    }
                }
                pesoMovimento.setMovimentos(x.movimentosPossiveis(), pesos);


                if (jogadorMax == cor)
                    movimentosMax.Add(pesoMovimento);
                else
                    movimentosMin.Add(pesoMovimento);
            }
        }

        private bool estaProtegida(Cor cor, Posicao peca, Posicao destino)
        {
            foreach (Peca x in partida.pecasEmJogo(cor))
            {
                if (!Posicao.comparaPosicao(x.posicao, peca))
                {
                    bool[,] R = x.movimentosPossiveis();
                    if (R[destino.linha, destino.coluna]) return true;
                }
            }
                return false;
        }

        private void estaAmeacada(Cor cor, Posicao destino, ref int valor)
        {
            foreach(Peca x in partida.pecasEmJogo(cor)){
                bool[,] movimentosPossiveis = x.movimentosPossiveis();

                if( x.existeMovimentosPossiveis() )
                    for (int i = 0; i < partida.tab.linhas; i++)
                    {
                        for (int j = 0; j < partida.tab.colunas; j++)
                        {
                            if (movimentosPossiveis[i, j] && Posicao.comparaPosicao(destino, new Posicao(i,j)))
                            {
                                valor--;
                                valor += x.getPeso();
                                if(estaProtegida(cor, x.posicao, destino))
                                {
                                    valor--;
                                }
                            }
                        }
                    }
            }
        }
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