using System;
using System.Collections.Generic;
using tabuleiro;
using xadrez;

namespace maquina
{
    class RealizaJogada
    {
        private const int PROFUNDIDADE = 1;

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

            valor += AvaliarVantagemPosicional(cor);
            valor += AvaliarSegurancaDoRei(cor);
            valor += AvaliarControleDoCentro(cor);
            valor += EstaAmeacada(peca, cor);
            valor += EstaProtegida(peca, cor);

            if (!(pecaCapturada is null))
            {
                valor += this.valorPeca[pecaCapturada.GetType().Name];
            }

            if (JogoEstaNoInicio())
            {
                valor += AvaliarJogadaInicioJogo(peca, pecaCapturada);
            }
            else
            {
                valor += AvaliarMaterial(cor);
            }

            return new MovimentoMiniMax(origem, peca.posicao, (int)valor);
        }

        private Posicao[] MiniMax()
        {
            MovimentoMiniMax alpha = new MovimentoMiniMax(null, null, Int32.MinValue);
            MovimentoMiniMax beta = new MovimentoMiniMax(null, null, Int32.MaxValue);

            MovimentoMiniMax movimentoMiniMax = MaxMove(
                partida,
                0,
                ref alpha,
                ref beta
            );

            return new Posicao[2] { movimentoMiniMax.origem, movimentoMiniMax.destino };
        }

        private MovimentoMiniMax MaxMove(
            PartidaDeXadrez partida,
            int profundidade,
            ref MovimentoMiniMax alpha,
            ref MovimentoMiniMax beta
        )
        {
            MovimentoMiniMax movimentoMiniMax = new MovimentoMiniMax(null, null, int.MinValue);

            if (profundidade == PROFUNDIDADE || partida.terminada)
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
                                Posicao origem = peca.posicao;
                                Posicao destino = new Posicao(i, j);
                                Peca pecaEnPAssant = partida.pecaVulneravelEnPassant;

                                Peca pecaCapturada = partida.baseExecutaMovimento(origem, destino);

                                //en Passant
                                if (peca is Peao && Math.Abs(origem.linha - destino.linha) == 2)
                                {
                                    partida.setPecaVulneravelEnPassant(peca);
                                }
                                else
                                {
                                    partida.setPecaVulneravelEnPassant(null);
                                }

                                if(partida.estaEmXeque(jogadorMax))
								{
                                    partida.baseDesfazMovimento(origem, destino, pecaCapturada);
                                    partida.setPecaVulneravelEnPassant(pecaEnPAssant);
                                    continue;
                                }

                                MovimentoMiniMax destinoMiniMax = PesosMovimentos(partida, peca, origem, jogadorMax, pecaCapturada);

                                movimentoMiniMax = MovimentoMiniMax.GetRequiredValue(movimentoMiniMax, destinoMiniMax, "max");

                                partida.baseDesfazMovimento(origem, destino, pecaCapturada);
                                partida.setPecaVulneravelEnPassant(pecaEnPAssant);

                                if (beta.valor <= movimentoMiniMax.valor)
                                {
                                    return movimentoMiniMax;
                                }

                                alpha = MovimentoMiniMax.GetRequiredValue(movimentoMiniMax, alpha, "max");
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
                                    Posicao origem = peca.posicao;
                                    Posicao destino = new Posicao(i, j);
                                    Peca pecaEnPAssant = partida.pecaVulneravelEnPassant;

                                    Peca pecaCapturada = partida.baseExecutaMovimento(peca.posicao, destino);

                                    //en Passant
                                    if (peca is Peao && Math.Abs(origem.linha - destino.linha) == 2)
                                    {
                                        partida.setPecaVulneravelEnPassant(peca);
                                    }
                                    else
                                    {
                                        partida.setPecaVulneravelEnPassant(null);
                                    }

                                    if (partida.estaEmXeque(jogadorMax))
                                    {
                                        partida.baseDesfazMovimento(origem, destino, pecaCapturada);
                                        partida.setPecaVulneravelEnPassant(pecaEnPAssant);
                                        continue;
                                    }

                                    MovimentoMiniMax destinoMiniMax = PesosMovimentos(partida, peca, origem, jogadorMax, pecaCapturada);

                                    MovimentoMiniMax minMove = MinMove(partida, profundidade + 1, ref alpha, ref beta);

                                    partida.baseDesfazMovimento(origem, destino, pecaCapturada);
                                    partida.setPecaVulneravelEnPassant(pecaEnPAssant);

                                    if(MovimentoMiniMax.GreatThen(minMove,destinoMiniMax))
									{
                                        movimentoMiniMax = MovimentoMiniMax.GetRequiredValue(movimentoMiniMax, destinoMiniMax, "max");
                                    }

                                    if(beta.valor <= movimentoMiniMax.valor)
									{
                                        return movimentoMiniMax;
                                    }

                                    alpha = MovimentoMiniMax.GetRequiredValue(movimentoMiniMax, alpha, "max");
                            }
                        }
                }
            }
            return movimentoMiniMax;
        }

        private MovimentoMiniMax MinMove(
            PartidaDeXadrez partida,
            int profundidade,
            ref MovimentoMiniMax alpha,
            ref MovimentoMiniMax beta
        )
        {
            MovimentoMiniMax movimentoMiniMin = new MovimentoMiniMax(null, null, int.MaxValue);

            foreach (Peca peca in partida.pecasEmJogo(jogadorMin))
            {
                bool[,] mat = peca.movimentosPossiveis();
                for (int i = 0; i < partida.tab.linhas; i++)
                    for (int j = 0; j < partida.tab.colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = peca.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaEnPAssant = partida.pecaVulneravelEnPassant;

                            Peca pecaCapturada = partida.baseExecutaMovimento(origem, destino);

                            //en Passant
                            if (peca is Peao && Math.Abs(origem.linha - destino.linha) == 2)
                            {
                                partida.setPecaVulneravelEnPassant(peca);
                            }
                            else
                            {
                                partida.setPecaVulneravelEnPassant(null);
                            }

                            if (partida.estaEmXeque(jogadorMin))
                            {
                                partida.baseDesfazMovimento(origem, destino, pecaCapturada);
                                partida.setPecaVulneravelEnPassant(pecaEnPAssant);
                                return new MovimentoMiniMax( origem, destino, Int32.MaxValue - 1);
                            }

                            if (pecaCapturada is Rei)
                            {
                                partida.baseDesfazMovimento(origem, destino, pecaCapturada);
                                partida.setPecaVulneravelEnPassant(pecaEnPAssant);
                                return new MovimentoMiniMax(origem, destino, Int32.MinValue + 1); ;
                            }

                            MovimentoMiniMax destinoMiniMin = PesosMovimentos(partida, peca, origem, jogadorMin, pecaCapturada);

                            MovimentoMiniMax maxMove = MaxMove(partida, profundidade, ref alpha, ref beta);

                            partida.baseDesfazMovimento(origem, destino, pecaCapturada);
                            partida.setPecaVulneravelEnPassant(pecaEnPAssant);

                            if(MovimentoMiniMax.LessThen(maxMove, destinoMiniMin))
							{
                                movimentoMiniMin = MovimentoMiniMax.GetRequiredValue(movimentoMiniMin, destinoMiniMin, "min");
                            }

                            if (movimentoMiniMin.valor <= alpha.valor)
                            {
                                return movimentoMiniMin;
							}

                            beta = MovimentoMiniMax.GetRequiredValue(movimentoMiniMin, beta, "min");
                        }
                    }
            }

            return movimentoMiniMin;
        }

        public float AvaliarVantagemPosicional(Cor cor)
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
                        float valorPosicional = ObterValorPosicional(peca);

                        if (peca.cor == cor)
                            vantagemJogadorAtual += valorPosicional;
                        else
                            vantagemAdiversario += valorPosicional;
                    }
                }
            }

            return vantagemJogadorAtual - vantagemAdiversario;
        }

        private float ObterValorPosicional(Peca peca)
        {
            float valor = this.valorPeca[peca.GetType().Name];

            if (peca.posicao.linha >= 2 && peca.posicao.linha <= 5 && peca.posicao.coluna >= 2 && peca.posicao.coluna <= 5)
                valor += 0.5f;

            return valor;
        }

        public float AvaliarMaterial(Cor cor)
        {
            float totalJogadorAtual = 0;
            float totalAdversario = 0;

            foreach (var peca in partida.tab.getPecas())
            {
                if (peca is null)
                    continue;

                if (peca.cor == cor)
                    totalJogadorAtual += this.valorPeca[peca.GetType().Name];
                else
                    totalAdversario += this.valorPeca[peca.GetType().Name];
            }

            return totalJogadorAtual - totalAdversario;
        }

        public int AvaliarSegurancaDoRei(Cor cor)
        {
            int seguranca = 0;

            Rei rei = (Rei)partida.rei(cor);

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

                    if(!partida.tab.posicaoValida(new Posicao(linha, coluna)))
                        continue;

                    Peca pecaVizinha = partida.tab.peca(linha, coluna);

                    if (pecaVizinha is Peca && pecaVizinha.cor != cor)
                    {
                        seguranca--;
                    }
                }
            }

            return seguranca;
        }

        public int AvaliarControleDoCentro(Cor cor)
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
                        controle = peca.cor == cor ? controle + 1 : controle - 1;
                    }
                }
            }

            return controle;
        }

        public bool JogoEstaNoInicio()
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

        public float AvaliarJogadaInicioJogo(Peca peca, Peca pecaDestino)
        {
            float valorJogada = 0;

            if (peca is Peao)
                valorJogada += this.valorPeca[peca.GetType().Name] - peca.qteMovimentos;
            else
                valorJogada -= this.valorPeca[peca.GetType().Name];

            return valorJogada;
        }

        public float EstaAmeacada(Peca peca, Cor cor)
		{
            float valor = 0;

            foreach(Peca p in partida.pecasEmJogo(partida.adversaria(cor)))
			{
                bool[,] moves = p.movimentosPossiveis();

                for(int i = 0; i < partida.tab.linhas; i++)
				{
                    for (int j = 0; j < partida.tab.colunas; j++)
                    {
                        bool posicaoPeca = peca.posicao.linha == i && peca.posicao.coluna == j;

                        if (moves[i,j] && posicaoPeca)
						{
                            valor -= this.valorPeca[p.GetType().Name];
                        }
                    }
                }
			}

            return valor;
		}

        public float EstaProtegida(Peca peca, Cor cor)
        {
            float valor = 0;

            foreach (Peca p in partida.pecasEmJogo(cor))
            {
                if(Posicao.comparaPosicao(p.posicao, peca.posicao))
				{
                    continue;
				}
            }

            return valor;
        }
    }
}