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
        public int valor { get; }

        public MovimentoMiniMax(Posicao origem, Posicao destino, int valor)
        {
            this.origem = origem;
            this.destino = destino;
            this.valor = valor;
        }

        public static MovimentoMiniMax GetRequiredValue(MovimentoMiniMax atual, MovimentoMiniMax novo, string tipo)
        {
            return novo.valor >= atual.valor && tipo == "max" ? novo : atual;
        }
    }
}
