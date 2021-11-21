using System;

using tabuleiro;

namespace xadrez_front
{
    class Program
    {
        static void Main(string[] args)
        {
            Tabuleiro tab;

            tab = new Tabuleiro(8, 8);

            Tela.imprimirTabuleiro(tab);

            Console.ReadLine();
        }
    }
}
