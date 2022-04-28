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
            /*
            int valor = 0;
            Peca pecaDestino = partida.tab.peca(destino.linha, destino.coluna);

            if (pecaDestino != null && pecaDestino.cor != cor) valor += 5;                                  //Captura Peca
            if (estaProtegida(partida, cor, peca.posicao, destino)) valor++;                                //Estara protegida
            estaAmeacada(partida, partida.adversaria(cor), destino, ref valor, peca);                             //Ficara ameaçada
            */
            return new MovimentoMiniMax(peca.posicao, destino, this.evalBoard(partida,cor));
        }

        private int evalBoard(PartidaDeXadrez partida, Cor cor)
        {
            int wp, wr, wk, wb, wd, bp, br, bk, bb, bd;
            wp = wr = wk = wb = wd = bp = br = bk = bb = bd = 0;

            foreach (Peca peca in partida.pecasEmJogo(Cor.Branca)) {
                if (peca is Peao) wp++;
                else if (peca is Torre) wr++;
                else if (peca is Cavalo) wk++;
                else if (peca is Bispo) wb++;
                else if (peca is Dama) wd++;
            }

            foreach (Peca peca in partida.pecasEmJogo(Cor.Preta))
            {
                if (peca is Peao) bp++;
                else if (peca is Torre) br++;
                else if (peca is Cavalo) bk++;
                else if (peca is Bispo) bb++;
                else if (peca is Dama) bd++;
            }

            int material = cor == Cor.Branca ? 100 * (wp - bp) + 320 * (wk - bk) + 330 * (wb - bb) + 500 * (wr - br) + 900 * (wd - bd) :
                100 * (bp - wp) + 320 * (bk - wk) + 330 * (bb - wb) + 500 * (br - wr) + 900 * (bd - wd);

            int peaoPosicao, torreProsicao, cavaloPosicao, bispoPosicao, damaPosicao, reiPosicao;
            peaoPosicao = torreProsicao = cavaloPosicao =  bispoPosicao =  damaPosicao = reiPosicao = 0;

            foreach (Peca peca in partida.pecasEmJogo(Cor.Branca))
            {
                if (peca is Peao) peaoPosicao = cor == Cor.Branca? +peca.getTableState(cor): -peca.getTableState(cor);
                else if (peca is Torre) torreProsicao = cor == Cor.Branca ? +peca.getTableState(cor) : -peca.getTableState(cor);
                else if (peca is Cavalo) cavaloPosicao = cor == Cor.Branca ? +peca.getTableState(cor) : -peca.getTableState(cor);
                else if (peca is Bispo) bispoPosicao = cor == Cor.Branca ? +peca.getTableState(cor) : -peca.getTableState(cor);
                else if (peca is Dama) damaPosicao = cor == Cor.Branca ? +peca.getTableState(cor) : -peca.getTableState(cor);
                else if (peca is Rei) reiPosicao = cor == Cor.Branca ? +peca.getTableState(cor) : -peca.getTableState(cor);
            }

            foreach (Peca peca in partida.pecasEmJogo(Cor.Preta))
            {
                if (peca is Peao) peaoPosicao = cor == Cor.Preta ? +peca.getTableState(cor) : -peca.getTableState(cor);
                else if (peca is Torre) torreProsicao = cor == Cor.Preta ? +peca.getTableState(cor) : -peca.getTableState(cor);
                else if (peca is Cavalo) cavaloPosicao = cor == Cor.Preta ? +peca.getTableState(cor) : -peca.getTableState(cor);
                else if (peca is Bispo) bispoPosicao = cor == Cor.Preta ? +peca.getTableState(cor) : -peca.getTableState(cor);
                else if (peca is Dama) damaPosicao = cor == Cor.Preta ? +peca.getTableState(cor) : -peca.getTableState(cor);
                else if (peca is Rei) reiPosicao = cor == Cor.Preta ? +peca.getTableState(cor) : -peca.getTableState(cor);
            }

            return material + peaoPosicao + torreProsicao + cavaloPosicao + bispoPosicao + damaPosicao + reiPosicao;
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

        private void estaAmeacada(PartidaDeXadrez partida, Cor cor, Posicao destino, ref int valor, Peca peca)
        {
            foreach (Peca x in partida.pecasEmJogo(cor))
            {
                bool[,] movimentosPossiveis = x is Peao? x.movimentosPossiveisAmeaca(): x.movimentosPossiveis();

                if (x.existeMovimentosPossiveis())
                    for (int i = 0; i < partida.tab.linhas; i++)
                    {
                        for (int j = 0; j < partida.tab.colunas; j++)
                        {
                            if (movimentosPossiveis[i, j] && Posicao.comparaPosicao(destino, new Posicao(i, j)))
                            {
                                valor -= 10;
                                valor += peca.getPeso();
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
            MovimentoMiniMax movimentoMiniMin = new MovimentoMiniMax(null, null, int.MaxValue);

            if (profundidade == 1 || partida.terminada)
            {
                if (partida.terminada) return null;

                foreach (Peca peca in partida.pecasEmJogo(jogadorMax))
                {
                    bool[,] mat = peca.movimentosPossiveis();
                    for (int i = 0; i < partida.tab.linhas; i++)
                        for (int j = 0; j < partida.tab.colunas; j++)
                        {
                            if (mat[i, j])
                            {
                                if (partida.estaEmXeque(jogadorMax))
                                {
                                    Posicao origemXeque = peca.posicao;
                                    Posicao destino = new Posicao(i, j);                                    
                                    Peca pecaCapturadaXeque = partida.executaMovimento(origemXeque, destino);                                    
                                    bool testeOnXeque = partida.estaEmXeque(jogadorMax);                                    
                                    partida.desfazMovimento(origemXeque, destino, pecaCapturadaXeque);                                    
                                    if (testeOnXeque) continue;
                                }
                                MovimentoMiniMax destinoMiniMax = pesosMovimentos(partida, peca, new Posicao(i, j), jogadorMax);

                                Peca pecaCapturada = partida.executaMovimento(destinoMiniMax.origem, destinoMiniMax.destino);
                                bool testeXeque = partida.estaEmXeque(jogadorMax);                                
                                partida.desfazMovimento(destinoMiniMax.origem, destinoMiniMax.destino, pecaCapturada);

                                if (!testeXeque)
                                    movimentoMiniMax = MovimentoMiniMax.GetRequiredValue(movimentoMiniMax, destinoMiniMax, "max");
                            }
                        }
                }

                return movimentoMiniMax;
            }
            else
            {
                foreach (Peca peca in partida.pecasEmJogo(jogadorMax))
                {                    
                    bool[,] mat = peca.movimentosPossiveis();
                    for (int i = 0; i < partida.tab.linhas; i++)
                        for (int j = 0; j < partida.tab.colunas; j++)
                        {
                            if (mat[i, j])
                            {
                                if (partida.estaEmXeque(jogadorMax))
                                {
                                    Posicao origemXeque = peca.posicao;
                                    Posicao destino = new Posicao(i, j);                                    
                                    Peca pecaCapturadaXeque = partida.executaMovimento(origemXeque, destino);                                    
                                    bool testeOnXeque = partida.estaEmXeque(jogadorMax);
                                    partida.desfazMovimento(origemXeque, destino, pecaCapturadaXeque);                                    
                                    if (testeOnXeque) continue;
                                }
                                MovimentoMiniMax destinoMiniMax = pesosMovimentos(partida, peca, new Posicao(i, j), jogadorMax);

                                Peca vulneravelEnPassant = partida.pecaVulneravelEnPassant;
                                //en Passant            
                                if (peca is Peao && (i == peca.posicao.linha - 2 || i == peca.posicao.linha + 2))
                                {
                                    partida.setPecaVulneravelEnPassant(peca);
                                }
                                else
                                {
                                    partida.setPecaVulneravelEnPassant(null);
                                }
                                Peca pecaCapturada = partida.executaMovimento(destinoMiniMax.origem, destinoMiniMax.destino);
                                
                                bool testeXeque = partida.estaEmXeque(jogadorMax);
                                if (testeXeque) 
                                {
                                    partida.setPecaVulneravelEnPassant(vulneravelEnPassant);
                                    partida.desfazMovimento(destinoMiniMax.origem, destinoMiniMax.destino, pecaCapturada);                                    
                                    continue; 
                                }

                                MovimentoMiniMax minMove = MinMove(partida, profundidade + 1);
                                partida.setPecaVulneravelEnPassant(vulneravelEnPassant);
                                partida.desfazMovimento(destinoMiniMax.origem, destinoMiniMax.destino, pecaCapturada);                                

                                if (MovimentoMiniMax.LessThen(movimentoMiniMin, minMove) && !testeXeque)
                                {
                                    movimentoMiniMax = MovimentoMiniMax.GetRequiredValue(movimentoMiniMax, destinoMiniMax, "max");
                                    movimentoMiniMin = MovimentoMiniMax.GetRequiredValue(movimentoMiniMin, minMove, "min");
                                }
                            }
                        }
                }
            }
            return movimentoMiniMax;
        }

        private MovimentoMiniMax MinMove(PartidaDeXadrez partida, int profundidade)
        {
            MovimentoMiniMax movimentoMiniMin = new MovimentoMiniMax(null, null, int.MinValue);

            if (partida.estaEmXeque(jogadorMin))
            {
                return new MovimentoMiniMax(null, null, int.MinValue);
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
                                try
                                {
                                    MovimentoMiniMax destinoMiniMin = pesosMovimentos(partida, peca, new Posicao(i, j), jogadorMin);
                                    Peca vulneravelEnPassant = partida.pecaVulneravelEnPassant;
                                    //en Passant            
                                    if (peca is Peao && (i == peca.posicao.linha - 2 || i == peca.posicao.linha + 2))
                                    {
                                        partida.setPecaVulneravelEnPassant(peca);
                                    }
                                    else
                                    {
                                        partida.setPecaVulneravelEnPassant(null);
                                    }
                                    Peca pecaCapturada = partida.executaMovimento(destinoMiniMin.origem, destinoMiniMin.destino);

                                    if (pecaCapturada is Rei)
                                    {
                                        destinoMiniMin.setValor(int.MinValue + 1);
                                        partida.setPecaVulneravelEnPassant(vulneravelEnPassant);
                                        partida.desfazMovimento(destinoMiniMin.origem, destinoMiniMin.destino, pecaCapturada);                                        
                                        return destinoMiniMin;
                                    }
                                    
                                    MovimentoMiniMax minMove = MaxMove(partida, profundidade);

                                    bool testeXeque = partida.estaEmXeque(jogadorMin);

                                    partida.setPecaVulneravelEnPassant(vulneravelEnPassant);
                                    partida.desfazMovimento(destinoMiniMin.origem, destinoMiniMin.destino, pecaCapturada);                                    

                                    if (testeXeque)
                                        minMove.setValor(int.MaxValue);

                                    if (MovimentoMiniMax.GreatThen(movimentoMiniMin, minMove))
                                    {
                                        movimentoMiniMin = destinoMiniMin;
                                    }
                                }catch(Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                }
            }
            return movimentoMiniMin;
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