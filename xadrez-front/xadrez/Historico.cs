using System;
using System.Collections.Generic;
using System.Text;
using tabuleiro;

namespace xadrez
{
	public class Historico
	{
		public Peca peca { get; private set; }
		public Peca pecaCapturada { get; private set; }
		public Posicao origem { get; private set; }
		public Posicao destino { get; private set; }
		public int turno { get; private set; }

		public Historico(Peca peca, Posicao origem, Posicao destino, int turno, Peca pecaCapturada = null)
		{
			this.peca = peca;
			this.origem = origem;
			this.destino = destino;
			this.turno = turno;
			this.pecaCapturada = pecaCapturada;
		}
	}
}
