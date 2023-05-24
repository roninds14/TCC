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

            valor += AvaliarVantagemPosicional(partida, cor);
            valor += AvaliarSegurancaDoRei(partida, cor);
            valor += AvaliarControleDoCentro(partida, cor);
            valor += EstaAmeacada(partida, peca);
            valor += EstaProtegida(partida, peca, cor);

            if (!(pecaCapturada is null))
            {
                valor += this.valorPeca[pecaCapturada.GetType().Name];
            }

            if(partida.estaEmXeque(partida.jogadorAtual))
			{
                valor += this.valorPeca["Rei"];
            }

            if (JogoEstaNoInicio(partida))
            {
                valor += AvaliarJogadaInicioJogo(partida, peca, pecaCapturada);
            }
            else
            {
                valor += AvaliarMaterial(partida, cor);
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
                                PartidaDeXadrez partidaClone = new PartidaDeXadrez();
                                partida = (PartidaDeXadrez)partida.Clone();

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

                                    if(partida.estaEmXeque(partida.jogadorAtual))
									{
                                        partida.desfazMovimento(origem, destino, pecaCapturada);
                                        partida.setPecaVulneravelEnPassant(pecaEnPAssant);
                                        continue;
                                    }

                                    MovimentoMiniMax destinoMiniMax = PesosMovimentos(partida, peca, origem, jogadorMax, pecaCapturada);

                                    movimentoMiniMax = MovimentoMiniMax.GetRequiredValue(movimentoMiniMax, destinoMiniMax, "max");

                                    partida.desfazMovimento(origem, destino, pecaCapturada);
                                    partida.setPecaVulneravelEnPassant(pecaEnPAssant);
                                }
                                catch(Exception e)
								{
                                    partida = partidaClone;
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
                                PartidaDeXadrez partidaClone = new PartidaDeXadrez();
                                partida = (PartidaDeXadrez)partida.Clone();

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

                                    if (partida.estaEmXeque(partida.jogadorAtual))
                                    {
                                        partida.desfazMovimento(origem, destino, pecaCapturada);
                                        partida.setPecaVulneravelEnPassant(pecaEnPAssant);
                                        continue;
                                    }

                                    MovimentoMiniMax destinoMiniMax = PesosMovimentos(partida, peca, origem, jogadorMax, pecaCapturada);

                                    MovimentoMiniMax minMove = MinMove(partida, profundidade + 1);
                                    partida.desfazMovimento(origem, destino, pecaCapturada);

                                    partida.setPecaVulneravelEnPassant(pecaEnPAssant);

                                    movimentoMiniMax = MovimentoMiniMax.GetRequiredValue(movimentoMiniMax, destinoMiniMax, "max");
                                }
                                catch(Exception e)
								{
                                    partida = partidaClone;
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
                            PartidaDeXadrez partidaClone = new PartidaDeXadrez();
                            partida = (PartidaDeXadrez)partida.Clone();

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

                                if (partida.estaEmXeque(partida.jogadorAtual))
                                {
                                    partida.desfazMovimento(origem, destino, pecaCapturada);
                                    partida.setPecaVulneravelEnPassant(pecaEnPAssant);
                                    continue;
                                }

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
                                partida = partidaClone;
                            }
                        }
                    }
            }

            return movimentoMiniMin;
        }

        public float AvaliarVantagemPosicional(PartidaDeXadrez partida, Cor cor)
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

        public float AvaliarMaterial(PartidaDeXadrez partida, Cor cor)
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

        public int AvaliarSegurancaDoRei(PartidaDeXadrez partida, Cor cor)
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

        public int AvaliarControleDoCentro(PartidaDeXadrez partida, Cor cor)
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
                        if (peca.cor == cor)
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

        public float EstaAmeacada(PartidaDeXadrez partida, Peca peca)
		{
            float valor = 0;

            foreach(Peca p in partida.pecasEmJogo(partida.jogadorAtual))
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

        public float EstaProtegida(PartidaDeXadrez partida, Peca peca, Cor cor)
        {
            float valor = 0;

            foreach (Peca p in partida.pecasEmJogo(cor))
            {
                bool[,] moves = p.movimentosPossiveis();

                for (int i = 0; i < partida.tab.linhas; i++)
                {
                    for (int j = 0; j < partida.tab.colunas; j++)
                    {
                        bool posicaoPeca = peca.posicao.linha == i && peca.posicao.coluna == j;

                        if (moves[i, j] && posicaoPeca)
                        {
                            valor += this.valorPeca[p.GetType().Name];
                        }
                    }
                }
            }

            return valor;
        }
    }
}