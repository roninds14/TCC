﻿using System;
using System.Collections.Generic;
using System.Text;

using tabuleiro;

namespace tabuleiro
{
    public abstract class Peca
    {
        public Posicao posicao { get; set; }
        public Cor cor { get; protected set; }
        public int qteMovimentos { get; protected set; } 
        public int peso { get; protected set; }

        public Tabuleiro tab { get; protected set; }

        public Peca(Tabuleiro tab, Cor cor)
        {
            this.posicao = null;
            this.tab = tab;
            this.cor = cor;
            this.qteMovimentos = 0;
        }

        public void incrementarQtdeMovimentos()
        {
            qteMovimentos++;
        }

        public void decrementarQtdeMovimentos()
        {
            qteMovimentos--;
        }

        public int getPeso()
        {
            return peso;
        }

        public bool existeMovimentosPossiveis()
        {
            bool[,] mat = movimentosPossiveis();

            for (int i = 0; i < tab.linhas; i++)
                for (int j = 0; j < tab.colunas; j++)
                    if (mat[i, j]) return true;

            return false;
        }

        public bool movimentoPossivel(Posicao pos)
        {
            return movimentosPossiveis()[pos.linha, pos.coluna];
        }

        public abstract bool[,] movimentosPossiveis();

        public virtual bool[,] movimentosPossiveisAmeaca() { return null; }
    }
}
