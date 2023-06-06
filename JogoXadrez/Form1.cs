using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JogoXadrez
{
	public partial class Selection : Form
	{
		public Selection()
		{
			InitializeComponent();

			onePlayer.Checked = true;
			colorWhite.Checked = true;
		}

		private void initializeGame(object sender, EventArgs e)
		{
			TabuleiroGame tabuleiro = new TabuleiroGame(
				onePlayer.Checked? 1: 2, 
				colorWhite.Checked? 1: 2
			);

			this.Hide();

			tabuleiro.ShowDialog();

			this.Close();
		}
	}
}
