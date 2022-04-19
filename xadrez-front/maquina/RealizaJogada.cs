using System;
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
        public Posicao origem { get; private set; }
        public Posicao destino { get; private set; }

        public RealizaJogada(PartidaDeXadrez partida, Cor jogador)
        {
            this.partida = partida;
            this.jogadorMax = jogador;
            this.jogadorMin = partida.adversaria(jogador);

            realizaMovimento();            
        }

        private void realizaMovimento()
        {
            Posicao[] posicoes = MiniMax();

            this.origem = posicoes[0];
            this.destino = posicoes[1];
        }

        private MovimentoMiniMax pesosMovimentos(PartidaDeXadrez partida, Peca peca, Posicao destino, Cor cor)
        {
            int valor = 0;
            Peca pecaDestino = partida.tab.peca(destino.linha, destino.coluna);

            if (pecaDestino != null && pecaDestino.cor != cor) valor += 2 * Math.Abs(pecaDestino.peso);     //Captura Peca
            if (estaProtegida(partida, cor, peca.posicao, destino)) valor++;                            //Estara protegida
            estaAmeacada(partida, partida.adversaria(cor), destino, ref valor);                         //Ficara ameaçada

            return new MovimentoMiniMax(peca.posicao, destino, valor);
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

        private bool estaProtegida(PartidaDeXadrez partida, Cor cor, Posicao peca, Posicao destino)
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

        private void estaAmeacada(PartidaDeXadrez partida, Cor cor, Posicao destino, ref int valor)
        {
            foreach (Peca x in partida.pecasEmJogo(cor))
            {
                bool[,] movimentosPossiveis = x.movimentosPossiveis();

                if (x.existeMovimentosPossiveis())
                    for (int i = 0; i < partida.tab.linhas; i++)
                    {
                        for (int j = 0; j < partida.tab.colunas; j++)
                        {
                            if (movimentosPossiveis[i, j] && Posicao.comparaPosicao(destino, new Posicao(i, j)))
                            {
                                valor--;
                                valor += x.getPeso();
                                if (estaProtegida(cor, x.posicao, destino))
                                {
                                    valor--;
                                }
                            }
                        }
                    }
            }
        }

        private Posicao[] MiniMax()
        {
            MovimentoMiniMax movimentoMiniMax = MaxMove(partida, 0);
            return new Posicao[2] { movimentoMiniMax.origem, movimentoMiniMax.destino };
        }

        private MovimentoMiniMax MaxMove( PartidaDeXadrez partida, int profundidade)
        {
            MovimentoMiniMax movimentoMiniMax = new MovimentoMiniMax(null, null, int.MinValue);            

            if (profundidade == 1 || partida.terminada)
            {
                return null;
            }
            else
            {
                foreach(Peca peca in partida.pecasEmJogo(jogadorMax))
                {
                    MovimentoMiniMax movimentoMiniMin = new MovimentoMiniMax(null, null, int.MaxValue);
                    bool[,] mat = peca.movimentosPossiveis();
                    for (int i = 0; i < partida.tab.linhas;  i++)
                        for (int j = 0; j < partida.tab.colunas; j++)
                        {
                            if (mat[i, j])
                            {
                                if (partida.estaEmXeque(jogadorMax))
                                {
                                    Posicao origemXeque = peca.posicao;
                                    Posicao destino = new Posicao(i, j);
                                    Peca pecaCapturada = partida.executaMovimento(origemXeque, destino);
                                    bool testeXeque = partida.estaEmXeque(jogadorMax);
                                    partida.desfazMovimento(origemXeque, destino, pecaCapturada);
                                    if (testeXeque) continue;
                                }

                                MovimentoMiniMax destinoMiniMax = pesosMovimentos(partida, peca, new Posicao(i,j), jogadorMax);
                                //movimentoMiniMax = MovimentoMiniMax.GetRequiredValue(movimentoMiniMax, destinoMiniMax, "max");
                                Posicao origem = peca.posicao;
                                Peca capturada = partida.executaMovimento(peca.posicao, new Posicao(i, j));

                                MovimentoMiniMax minMove = MinMove(partida, profundidade);

                                if (MovimentoMiniMax.LessThen(movimentoMiniMin, minMove)){
                                    movimentoMiniMax = MovimentoMiniMax.GetRequiredValue(movimentoMiniMax, destinoMiniMax, "max");
                                    movimentoMiniMin = minMove?? movimentoMiniMin;
                                }                               

                                partida.desfazMovimento(origem, peca.posicao, capturada);
                            }
                        }                    
                }                
            }

            return movimentoMiniMax;
        }

        private MovimentoMiniMax MinMove(PartidaDeXadrez partida, int profundidade)
        {
            MovimentoMiniMax movimentoMiniMax = new MovimentoMiniMax(null, null, int.MaxValue);

            if (partida.terminada)
            {
                return null;
            }
            else
            {
                foreach (Peca peca in partida.pecasEmJogo(jogadorMin))
                {
                    bool[,] mat = peca.movimentosPossiveis();
                    for (int i = 0; i < partida.tab.linhas; i++)
                        for (int j = 0; j < partida.tab.colunas; j++)
                        {
                            if (mat[i, j])
                            {
                                MovimentoMiniMax destinoMiniMax = pesosMovimentos(partida, peca, new Posicao(i, j), jogadorMin);
                                //movimentoMiniMax = MovimentoMiniMax.GetRequiredValue(movimentoMiniMax, destinoMiniMax, "min");
                                Posicao origem = peca.posicao;
                                Peca capturada = partida.executaMovimento(peca.posicao, new Posicao(i, j));

                                movimentoMiniMax = MovimentoMiniMax.GetRequiredValue(movimentoMiniMax, MaxMove(partida, profundidade + 1)?? destinoMiniMax, "min");
 
                                partida.desfazMovimento(origem, peca.posicao, capturada);
                            }
                        }
                }
            }

            return movimentoMiniMax;
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