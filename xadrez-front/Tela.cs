using System;
using System.Collections.Generic;
using tabuleiro;
using xadrez;
using maquina;

namespace xadrez_front
{
    class Tela
    {
        public static void imprimirMenu(PartidaDeXadrez partida)
        {
            Console.WriteLine("Bem vindo a Partida de Xadrez!\n\n");

            Console.WriteLine("Digite 'ajuda' para o exibir comandos possíveis!\nDigite 'sair' para encerrar o jogo!\n");

            Console.Write("Digite o número de jogadores para começar: ");

            try
            {
                int jogadores = int.Parse(Console.ReadLine());

                if (jogadores < 0 || jogadores > 2)
                    throw new TelaException("Número de jogadores deve ser entre 0 (zero) e 3 (três)!");

                if(jogadores == 1)
                {
                    Console.Write("Digite sua cor: ");
                    string corDigitada  = (Console.ReadLine()).Trim();

                    Cor cor = corDigitada[0] == 'B' ? Cor.Branca: Cor.Preta;

                    partida.setJogadores(cor);
                }
                else
                {
                    partida.setJogadores(jogadores);
                }

                partida.iniciaPartida();

                Console.Clear();
            }
            catch (TelaException e)
            {
                throw new TelaException(e.Message);
            }
            catch (Exception)
            {
                throw new TelaException("Não foi possível capturar o número de jogadores!");
            }
        }

        public static void imprimirPartida(PartidaDeXadrez partida)
        {
            if(!partida.iniciada)
                Tela.imprimirMenu(partida);

            Tela.imprimirTabuleiro(partida.tab);
            Console.WriteLine();
            imprimirPecasCapturadas(partida);
            Console.WriteLine();

            if (!partida.terminada)
            {
                Console.WriteLine("Turno: " + partida.turno);

                if (partida.tipoJogador(partida.jogadorAtual))
                {
                    Console.WriteLine("Aguardando jogada da: " + partida.jogadorAtual);
                    if (partida.xeque)
                        Console.WriteLine("XEQUE!");

                    Console.WriteLine();

                    Console.Write("Origem: ");
                    Posicao origem = Tela.lerPosicaoXadrez().toPosicao();
                    partida.validarPosicaoDeOrigem(origem);

                    bool[,] posicoesPossiveis = partida.tab.peca(origem).movimentosPossiveis();

                    Console.Clear();
                    Tela.imprimirTabuleiro(partida.tab, posicoesPossiveis);

                    Console.Write("Destino: ");
                    Posicao destino = Tela.lerPosicaoXadrez().toPosicao();
                    partida.validarPosicaoDeDestino(origem, destino);

                    partida.realizaJogada(origem, destino);
                }
                else
                {
                    Console.WriteLine("Aguarde a jogada da: " + partida.jogadorAtual);

                    RealizaJogada jogada = new RealizaJogada(partida, partida.jogadorAtual);

                    partida.realizaJogada(jogada.origem, jogada.destino);
                }
            }
            else
            {
                Console.WriteLine("Xeque Mate!");
                Console.WriteLine("Vencedor: " + partida.jogadorAtual);
            }
            
        }

        public static void imprimirPecasCapturadas(PartidaDeXadrez partida)
        {
            Console.WriteLine("Peças capturadas:");
            Console.Write("\tBrancas\t");
            imprimirConjunto(partida.pecasCapturadas(Cor.Branca));
            Console.WriteLine();
            Console.Write("\tPretas\t");
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            imprimirConjunto(partida.pecasCapturadas(Cor.Preta));
            Console.ForegroundColor = aux;
            Console.WriteLine();
        }

        public static void imprimirConjunto(HashSet<Peca> conjunto)
        {
            Console.Write("[");
            foreach(Peca x in conjunto)
            {
                Console.Write(x + " ");
            }

            Console.Write("]");
        }

        public static void imprimirTabuleiro(Tabuleiro tab)
        {
            for(int i=0; i < tab.linhas; i++)
            {
                Console.Write(8 - i + " ");
                for(int j=0; j<tab.colunas; j++)
                {
                    imprimirPeca(tab.peca(i, j));                                                               
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }

        public static void imprimirTabuleiro(Tabuleiro tab, bool[,] possicoesPossiveis)
        {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;

            for (int i = 0; i < tab.linhas; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < tab.colunas; j++)
                {
                    if (possicoesPossiveis[i, j])
                    {
                        Console.BackgroundColor = fundoAlterado;
                    }
                    else
                    {
                        Console.BackgroundColor = fundoOriginal;
                    }
                    imprimirPeca(tab.peca(i, j));
                    Console.BackgroundColor = fundoOriginal;
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");

            Console.BackgroundColor = fundoOriginal;
        }

        public static PosicaoXadrez lerPosicaoXadrez()
        {
            try
            {
                string s = Console.ReadLine();
                char coluna = s[0];
                int linha = int.Parse(s[1] + "");
                return new PosicaoXadrez(coluna, linha);
            }
            catch (Exception)
            {
                throw new TelaException("Entrada não reconhecida!");
            }          
        }

        public static void imprimirPeca(Peca peca)
        {

            if(peca == null)
            {
                Console.Write("- ");
            }
            else
            {
                if (peca.cor == Cor.Branca)
                {
                    Console.Write(peca);
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }
    }
}
