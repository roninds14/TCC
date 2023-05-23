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
        
        private Dictionary<string, float> valorPeca = new Dictionary<string, float>()
        {
            { "Peao", 1 },
            { "Cavalo", 3 },
            { "Bispo", 3.5f },
            { "Torre", 5 },
            { "Dama", 9 },
            { "Rei", 100 }
        };

        public RealizaJogada(PartidaDeXadrez partida, Cor jogador)
        {
            this.partida = partida;
            this.jogadorMax = jogador;
            this.jogadorMin = partida.adversaria(jogador);

            RealizaMovimento();
        }

        private void RealizaMovimento()
        {
            Posicao[] posicoes = MiniMax();

            this.origem = posicoes[0];
            this.destino = posicoes[1];
        }

        private MovimentoMiniMax PesosMovimentos(PartidaDeXadrez partida, Peca peca, Posicao origem, Cor cor, Peca pecaCapturada)
        {
            float valor = 0;

            valor += AvaliarVantagemPosicional(partida);
            valor += AvaliarSegurancaDoRei(partida);
            valor += AvaliarControleDoCentro(partida);

            if (!(pecaCapturada is null))
            {
                valor += this.valorPeca[pecaCapturada.GetType().Name];
            }

            if (JogoEstaNoInicio(partida))
            {
                AvaliarJogadaInicioJogo(partida, peca, pecaCapturada);
            }
            else
            {
                valor += AvaliarMaterial(partida);
            }

            return new MovimentoMiniMax(origem, peca.posicao, (int)valor);
        }

        private Posicao[] MiniMax()
        {
            MovimentoMiniMax movimentoMiniMax = MaxMove(partida, 0);
            return new Posicao[2] { movimentoMiniMax.origem, movimentoMiniMax.destino };
        }

        private MovimentoMiniMax MaxMove(PartidaDeXadrez partida, int profundidade)
        {
            MovimentoMiniMax movimentoMiniMax = new MovimentoMiniMax(null, null, int.MinValue);

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
                                try
								{
                                    Posicao origem = peca.posicao;
                                    Posicao destino = new Posicao(i, j);
                                    Peca pecaEnPAssant = partida.pecaVulneravelEnPassant;

                                    //en Passant
                                    if (peca is Peao && Math.Abs(origem.linha - destino.linha) == 2)
                                    {
                                        partida.setPecaVulneravelEnPassant(peca);
                                    }
                                    else
                                    {
                                        partida.setPecaVulneravelEnPassant(null);
                                    }

                                    Peca pecaCapturada = partida.executaMovimento(origem, destino);
                                    MovimentoMiniMax destinoMiniMax = PesosMovimentos(partida, peca, origem, jogadorMax, pecaCapturada);

                                    movimentoMiniMax = MovimentoMiniMax.GetRequiredValue(movimentoMiniMax, destinoMiniMax, "max");

                                    partida.desfazMovimento(origem, destino, pecaCapturada);
                                    partida.setPecaVulneravelEnPassant(pecaEnPAssant);
                                }
                                catch(Exception e)
								{
                                    Console.WriteLine("Erro ao realizar jogada terminal: " + e.Message);
								}
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
								try
								{
                                    Posicao origem = peca.posicao;
                                    Posicao destino = new Posicao(i, j);
                                    Peca pecaEnPAssant = partida.pecaVulneravelEnPassant;

                                    //en Passant
                                    if (peca is Peao && Math.Abs(origem.linha - destino.linha) == 2)
                                    {
                                        partida.setPecaVulneravelEnPassant(peca);
                                    }
                                    else
                                    {
                                        partida.setPecaVulneravelEnPassant(null);
                                    }

                                    Peca pecaCapturada = partida.executaMovimento(peca.posicao, new Posicao(i, j));

                                    MovimentoMiniMax destinoMiniMax = PesosMovimentos(partida, peca, origem, jogadorMax, pecaCapturada);

                                    MovimentoMiniMax minMove = MinMove(partida, profundidade + 1);
                                    partida.desfazMovimento(origem, destino, pecaCapturada);

                                    partida.setPecaVulneravelEnPassant(pecaEnPAssant);

                                    movimentoMiniMax = MovimentoMiniMax.GetRequiredValue(movimentoMiniMax, destinoMiniMax, "max");
                                }
                                catch(Exception e)
								{
                                    Console.WriteLine("Erro ao realizar jogada maxina: " + e.Message);
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
                                Posicao origem = peca.posicao;
                                Posicao destino = new Posicao(i, j);
                                Peca pecaEnPAssant = partida.pecaVulneravelEnPassant;

                                //en Passant
                                if (peca is Peao && Math.Abs(origem.linha - destino.linha) == 2)
                                {
                                    partida.setPecaVulneravelEnPassant(peca);
                                }
                                else
                                {
                                    partida.setPecaVulneravelEnPassant(null);
                                }

                                Peca pecaCapturada = partida.executaMovimento(origem, destino);

                                MovimentoMiniMax destinoMiniMin = PesosMovimentos(partida, peca, origem, jogadorMin, pecaCapturada);

                                if (pecaCapturada is Rei)
                                {
                                    destinoMiniMin.setValor(int.MinValue + 1);
                                    partida.desfazMovimento(origem, destino, pecaCapturada);
                                    partida.setPecaVulneravelEnPassant(pecaEnPAssant);
                                    return destinoMiniMin;
                                }

                                MovimentoMiniMax minMove = MaxMove(partida, profundidade);

                                partida.desfazMovimento(origem, destino, pecaCapturada);
                                partida.setPecaVulneravelEnPassant(pecaEnPAssant);

                                if (MovimentoMiniMax.GreatThen(movimentoMiniMin, minMove))
                                {
                                    movimentoMiniMin = destinoMiniMin;
                                }
                            }catch(Exception e)
                            {
                                Console.WriteLine("Erro ao executar nomimento min:" + e.Message);
                            }
                        }
                    }
            }

            return movimentoMiniMin;
        }

        public float AvaliarVantagemPosicional(PartidaDeXadrez partida)
        {
            float vantagemJogadorAtual = 0;
            float vantagemAdiversario = 0;

            for (int linha = 0; linha < partida.tab.linhas; linha++)
            {
                for (int coluna = 0; coluna < partida.tab.colunas; coluna++)
                {
                    Peca peca = partida.tab.peca(new Posicao(linha, coluna));

                    if (peca != null)
                    {
                        float valorPosicional = ObterValorPosicional(peca, linha, coluna);

                        if (peca.cor == partida.jogadorAtual)
                            vantagemJogadorAtual += valorPosicional;
                        else
                            vantagemAdiversario += valorPosicional;
                    }
                }
            }

            return vantagemJogadorAtual - vantagemAdiversario;
        }

        private float ObterValorPosicional(Peca peca, int linha, int coluna)
        {
            float valor = this.valorPeca[peca.GetType().Name];
            
            if (linha >= 2 && linha <= 5 && coluna >= 2 && coluna <= 5)
                valor += 0.5f;

            return valor;
        }

        public float AvaliarMaterial(PartidaDeXadrez partida)
        {
            float totalJogadorAtual = 0;
            float totalAdversario = 0;

            foreach (var peca in partida.tab.getPecas())
            {
                if (peca is null)
                    continue;

                if (peca.cor == partida.jogadorAtual)
                    totalJogadorAtual += this.valorPeca[peca.GetType().Name];
                else
                    totalAdversario += this.valorPeca[peca.GetType().Name];
            }

            return totalJogadorAtual - totalAdversario;
        }

        public int AvaliarSegurancaDoRei(PartidaDeXadrez partida)
        {
            int seguranca = 0;

            Rei rei = (Rei)partida.rei(partida.jogadorAtual);

            if (rei is null)
                return seguranca;

            Posicao posicaoRei = rei.posicao;
            int linhaRei = posicaoRei.linha;
            int colunaRei = posicaoRei.coluna;

            for (int linha = linhaRei - 1; linha <= linhaRei + 1; linha++)
            {
                for (int coluna = colunaRei - 1; coluna <= colunaRei + 1; coluna++)
                {
                    if (linha == linhaRei && coluna == colunaRei)
                        continue;

                    if(linha < 0 || linha >= partida.tab.linhas || coluna < 0 || coluna >= partida.tab.colunas)
                        continue;

                    Peca pecaVizinha = partida.tab.peca(linha, coluna);

                    if (pecaVizinha is Peca && pecaVizinha.cor != partida.jogadorAtual)
                    {
                        seguranca--;
                    }
                }
            }

            return seguranca;
        }

        public int AvaliarControleDoCentro(PartidaDeXadrez partida)
        {
            int controle = 0;

            int linhaCentro = partida.tab.linhas / 2;
            int colunaCentro = partida.tab.colunas / 2;

            for (int linha = linhaCentro - 1; linha <= linhaCentro + 1; linha++)
            {
                for (int coluna = colunaCentro - 1; coluna <= colunaCentro + 1; coluna++)
                {
                    Peca peca = partida.tab.peca(linha, coluna);

                    if (peca is Peca)
                    {
                        if (peca.cor == partida.jogadorAtual)
                        {
                            controle++;
                        }
                        else if (peca.cor == partida.jogadorAtual)
                        {
                            controle--;
                        }
                    }
                }
            }

            return controle;
        }

        public bool JogoEstaNoInicio(PartidaDeXadrez partida)
        {
            if (partida.pecasEmJogo(partida.jogadorAtual).Count < 16 &&
                partida.pecasEmJogo(partida.adversaria(partida.jogadorAtual)).Count < 16)
			    return false;

            Peca reiBranco = partida.rei(Cor.Branca);
		    Peca reiPreto = partida.rei(Cor.Preta);
		    if (reiBranco == null || reiPreto == null ||
                reiPreto.posicao.linha != 0 || reiPreto.posicao.coluna != 4 ||
                reiBranco.posicao.linha != 7 || reiBranco.posicao.coluna != 4)
		    {
			    return false;
		    }

			Peca torreBranca1A = partida.tab.peca(new Posicao(0, 0));
			Peca torreBranca1H = partida.tab.peca(new Posicao(0, 7));
			Peca torrePreta2A = partida.tab.peca(new Posicao(7, 0));
			Peca torrePreta2H = partida.tab.peca(new Posicao(7, 7));
			if (torreBranca1A is null || torreBranca1H is null ||
                torrePreta2A is null || torrePreta2H is null ||
                !(torreBranca1A is Torre) || !(torreBranca1H is Torre) ||
                !(torrePreta2A is Torre) || !(torrePreta2H is Torre)
            )
			{
				return false;
			}

			return true;
        }

        public float AvaliarJogadaInicioJogo(PartidaDeXadrez partida, Peca peca, Peca pecaDestino)
        {
            float valorJogada = 0;

            if (peca is Peao)
                valorJogada += this.valorPeca[peca.GetType().Name] + peca.qteMovimentos - partida.turno ;

            return valorJogada;
        }
    }
}