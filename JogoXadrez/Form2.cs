using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using xadrez;
using tabuleiro;
using JogoXadrez.Classes;
using System.Linq;

namespace JogoXadrez
{
	public partial class TabuleiroGame : Form
	{
		private readonly Dictionary<int, int> TranslatePosition = new Dictionary<int, int>()
		{
			{ 0 , 7},
			{ 1 , 66},
			{ 2 , 126},
			{ 3 , 186},
			{ 4 , 246},
			{ 5 , 306},
			{ 6 , 366},
			{ 7 , 426}
		};
		private readonly Dictionary<int, int> TranslatePositionMove = new Dictionary<int, int>()
		{
			{ 0 , 0},
			{ 1 , 58},
			{ 2 , 118},
			{ 3 , 178},
			{ 4 , 238},
			{ 5 , 298},
			{ 6 , 358},
			{ 7 , 418}
		};
		private IList<PicturePeca> PicturePeca;
		private IList<PicturePosicao> PictureMove;
		private Posicao Origem;
		private PartidaDeXadrez Partida;

		public TabuleiroGame(int players, int color)
		{
			InitializeComponent();

			PicturePeca = new List<PicturePeca>();
			PictureMove = new List<PicturePosicao>();

			Partida = new PartidaDeXadrez();

			SetPlayers(players, color);
			FillBoard();
		}

		private void FillBoard()
		{
			ClearBoard();

			CorJogadorAtual.Text = Partida.jogadorAtual.ToString();

			foreach (Peca peca in Partida.tab.getPecas())
			{
				AddPeca(peca);
			}
		}

		private void SetPlayers(int players, int color)
		{
			if (players == 1)
			{
				Partida.setJogadores(color == 1 ? Cor.Branca : Cor.Preta);
			}
			else
			{
				Partida.setJogadores(players);
			}
		}

		private void ClearBoard()
		{
			RemovePeca();

			foreach (PicturePosicao picturePosicao in PictureMove)
			{
				picturePosicao.pictureBox.Dispose();
			}

			PicturePeca = new List<PicturePeca>();
			PictureMove = new List<PicturePosicao>();
		}

		private void RemovePeca()
		{
			foreach (PicturePeca picturePeca in PicturePeca)
			{
				picturePeca.pictureBox.Dispose();
			}
		}

		private void AddPeca(Peca peca)
		{
			if (peca is null) return;

			PictureBox newPicture = new PictureBox();

			newPicture.Image = GetImage(peca);
			newPicture.Height = 45;
			newPicture.Width = 45;
			newPicture.Location = new Point(
				TranslatePosition[peca.posicao.coluna],
				TranslatePosition[peca.posicao.linha]
			);
			newPicture.Name = peca.ToString() + peca.cor.ToString() + peca.posicao.linha + peca.posicao.coluna;

			newPicture.Click += PecaClick;

			PicturePeca.Add(new PicturePeca(peca, newPicture));
			pictureBoard.Controls.Add(newPicture);
		}

		private void AddPossivelMove(Peca peca)
		{
			bool[,] move = peca.movimentosPossiveis();

			for (int i = 0; i < Partida.tab.linhas; i++)
			{
				for (int j = 0; j < Partida.tab.colunas; j++)
				{
					if (!move[i, j])
						continue;

					PictureBox newPicture = new PictureBox();

					newPicture.BackColor = Color.FromArgb(128, Color.Gray);
					newPicture.Height = 59;
					newPicture.Width = 59;

					newPicture.Location = new Point(
						TranslatePositionMove[j],
						TranslatePositionMove[i]
					);

					newPicture.Name = peca.ToString() + peca.cor.ToString() + i + j;

					newPicture.Click += MoveClick;

					PictureMove.Add(new PicturePosicao(new Posicao(i, j), newPicture));
					pictureBoard.Controls.Add(newPicture);
					newPicture.BringToFront();
				}
			}
		}

		private Image GetImage(Peca peca)
		{
			Image typeOfPeca = null;

			if (peca is Peao)
				typeOfPeca = peca.cor == Cor.Branca ?
					Properties.Resources.P_B : Properties.Resources.P_P;

			else if (peca is Cavalo)
				typeOfPeca = peca.cor == Cor.Branca ?
					Properties.Resources.C_B : Properties.Resources.C_P;

			else if (peca is Bispo)
				typeOfPeca = peca.cor == Cor.Branca ?
					Properties.Resources.B_B : Properties.Resources.B_P;

			else if (peca is Torre)
				typeOfPeca = peca.cor == Cor.Branca ?
					Properties.Resources.T_B : Properties.Resources.T_P;

			else if (peca is Dama)
				typeOfPeca = peca.cor == Cor.Branca ?
					Properties.Resources.D_B : Properties.Resources.D_P;

			else if (peca is Rei)
				typeOfPeca = peca.cor == Cor.Branca ?
					Properties.Resources.R_B : Properties.Resources.R_P;

			return typeOfPeca;
		}

		private void PecaClick(object sender, EventArgs e)
		{
			if (!(Origem is null))
				return;

			Control clickedObject = sender as Control;

			Peca pecaClicada = GetPecaClicado(clickedObject);

			if(pecaClicada.cor != Partida.jogadorAtual)
			{
				return;
			}

			if (pecaClicada.existeMovimentosPossiveis())
			{
				Origem = pecaClicada.posicao;

				AddPossivelMove(pecaClicada);
			}

			return;
		}

		private void MoveClick(object sender, EventArgs e)
		{
			Control clickedObject = sender as Control;

			try
			{
				Partida.realizaJogada(Origem, GetPosicaoClicado(clickedObject));
			}
			catch (TabuleiroException exp)
			{
				MessageBox.Show(exp.Message, "Não é possivel realziar essa jogada!");
			}

			Origem = null;

			ClearBoard();

			FillBoard();
		}

		private Posicao GetPosicaoClicado(Control clickedObject)
		{
			Posicao posicaoClicado = null;

			foreach (PicturePosicao picturePosicao in PictureMove)
			{
				posicaoClicado = clickedObject.Name == picturePosicao.pictureBox.Name ? picturePosicao.posicao : null;

				if (!(posicaoClicado is null))
					return posicaoClicado;
			}

			return posicaoClicado;
		}

		private Peca GetPecaClicado(Control clickedObject)
		{
			Peca pecaClicada = null;

			foreach (PicturePeca picturePeca in PicturePeca)
			{
				pecaClicada = clickedObject.Name == picturePeca.pictureBox.Name? picturePeca.peca: null;

				if (!(pecaClicada is null))
					return pecaClicada;
			}

			return pecaClicada;
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
