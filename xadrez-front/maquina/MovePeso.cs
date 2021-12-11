using System;
using System.Collections.Generic;
using System.Text;

namespace maquina
{
    class MovePeso
    {
        int valor { get; set; }
        bool move { get; set; }

        public MovePeso()
        {
            valor = 0;
            move = false;
        }

        public MovePeso(int valor,bool move)
        {
            this.valor = valor;
            this.move = move;
        }
    }
}
