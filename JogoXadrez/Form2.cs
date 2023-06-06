using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using xadrez;
using tabuleiro;

namespace JogoXadrez
{
	public partial class TabuleiroGame : Form
	{
		public TabuleiroGame(int players, int color)
		{
			InitializeComponent();

			PartidaDeXadrez partida = new PartidaDeXadrez();

			if (players == 1)
			{
				partida.setJogadores(color ==1? Cor.Branca: Cor.Preta);
			}
			else
			{
				partida.setJogadores(players);
			}
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			DialogResult dialog = MessageBox.Show("Deseja encerrar?", "Fechar Aplicativo!", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

			if (dialog.Equals(DialogResult.Yes))
			{
				this.Close();
			}
		}
	}
}
