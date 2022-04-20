using System;
using System.Collections.Generic;
using System.Text;
using tabuleiro;

namespace maquina
{
    class MovimentoMiniMax
    {
        public Posicao origem { get; }
        public Posicao destino { get; }
        public int valor { get; private set; }

        public void setValor(int valor)
        {
            this.valor = valor;
        }

        public MovimentoMiniMax(Posicao origem, Posicao destino, int valor)
        {
            this.origem = origem;
            this.destino = destino;
            this.valor = valor;
        }

        public static MovimentoMiniMax GetRequiredValue(MovimentoMiniMax atual, MovimentoMiniMax novo, string tipo)
        {
            return (novo.valor >= atual.valor && tipo == "max") ||
                (novo.valor < atual.valor && tipo == "min") ? novo : atual;
        }

        public static Boolean LessThen(MovimentoMiniMax atual, MovimentoMiniMax novo)
        {
            return novo.valor < atual.valor;
        }

        public static Boolean GreatThen(MovimentoMiniMax atual, MovimentoMiniMax novo)
        {
            return novo.valor >= atual.valor;
        }
    }
}
