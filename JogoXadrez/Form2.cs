using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using xadrez;
using maquina;
using tabuleiro;
using JogoXadrez.Classes;
using System.Linq;
using System.Threading;

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
			{ 1 , 59},
			{ 2 , 119},
			{ 3 , 179},
			{ 4 , 239},
			{ 5 , 299},
			{ 6 , 359},
			{ 7 , 419}
		};
		private IList<PicturePeca> PicturePeca;
		private IList<PicturePosicao> PictureMove;
		private IList<Label> HistoryLabel;
		private IList<PictureBox> Capturadas;
		private Posicao Origem;
		private PartidaDeXadrez Partida;
		private int players, color;

		public TabuleiroGame(int players, int color)
		{
			InitializeComponent();

			this.players = players;
			this.color = color;

			InitializeGame();
		}

		private void  InitializeGame()
		{
			PicturePeca = new List<PicturePeca>();
			PictureMove = new List<PicturePosicao>();
			HistoryLabel = new List<Label>();
			Capturadas = new List<PictureBox>();

			Partida = new PartidaDeXadrez();

			EmXeque.Visible = false;
			loadPicture.Visible = false;

			this.Shown += MeuForm_Shown;

			SetPlayers(players, color);

			FillBoard();
		}

		private void MeuForm_Shown(object sender, EventArgs e)
		{
			BeginInvoke(new Action(On_Load));
		}

		private void On_Load()
		{
			Thread.Sleep(1000);

			if (!Partida.tipoJogador(Partida.jogadorAtual))
			{
				RealizaJogadaMaquina();
			}

			ClearBoard();
			FillBoard();
		}

		private void FillBoard()
		{
			ClearBoard();

			CorJogadorAtual.Text = Partida.jogadorAtual.ToString();
				
			EmXeque.Visible = Partida.xeque;

			foreach (Peca peca in Partida.tab.getPecas())
			{
				AddPeca(peca);
			}

			FillHistory();
			FillCapturadas();
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
			RemoveHistory();
			RemoveCapturadas();

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

		private void RemoveHistory()
		{
			foreach (Label label in HistoryLabel)
			{
				label.Dispose();
			}
		}

		private void RemoveCapturadas()
		{
			foreach (PictureBox capturada in Capturadas)
			{
				capturada.Dispose();
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
					newPicture.Height = 57;
					newPicture.Width = 57;

					newPicture.Location = new Point(
						TranslatePositionMove[j],
						TranslatePositionMove[i]
					);

					newPicture.Name = peca.ToString() + peca.cor.ToString() + i + j;

					newPicture.Click += MoveClick;

					if(Partida.tab.existePeca(new Posicao(i,j)) && Partida.tab.peca(new Posicao(i, j)).cor != Partida.jogadorAtual)
					{
						newPicture.Image = GetImagegGray(Partida.tab.peca(new Posicao(i, j)));
						newPicture.Height = 45;
						newPicture.Width = 45;
						newPicture.SizeMode = PictureBoxSizeMode.StretchImage;
						newPicture.Location = new Point(
							TranslatePosition[j],
							TranslatePosition[i]
						);
					}

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

		private Image GetImagegGray(Peca peca){
			Image typeOfPeca = null;

			if (peca is Peao)
				typeOfPeca = Properties.Resources.P_C;

			else if (peca is Cavalo)
				typeOfPeca = Properties.Resources.C_C;

			else if (peca is Bispo)
				typeOfPeca = Properties.Resources.B_C;

			else if (peca is Torre)
				typeOfPeca = Properties.Resources.T_C;

			else if (peca is Dama)
				typeOfPeca = Properties.Resources.D_C;

			else if (peca is Rei)
				typeOfPeca = Properties.Resources.R_C;

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
				MessageBox.Show(
					"Vez das peças " + Partida.jogadorAtual.ToString() + "s",
					"Não é a sua vez!",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
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
				MessageBox.Show(exp.Message, "Não é possivel realizar essa jogada!");
			}

			Origem = null;

			if(!Partida.tipoJogador(Partida.jogadorAtual))
			{
				RealizaJogadaMaquina();
			}

			ClearBoard();
			FillBoard();
		}

		private void RealizaJogadaMaquina()
		{
			RealizaJogada jogada = new RealizaJogada(Partida, Partida.jogadorAtual);

			loadPicture.Visible = true;

			Thread.Sleep(1000);

			Partida.realizaJogada(jogada.origem, jogada.destino);

			loadPicture.Visible = false;
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
	
		private void FillHistory()
		{
			List<Historico> historico = new List<Historico>(Partida.historico);

			for (int i = historico.Count - 1, j = 0; i >= 0 && j < 23; i--, j++)
			{
				Label newLabel = new Label();

				newLabel.Text =
					historico[i].turno.ToString() + " " +
					historico[i].peca.ToString() + " " + 
					historico[i].origem.ToStringTabuleiro() + "-" +
					historico[i].destino.ToStringTabuleiro() + " " +
					historico[i].pecaCapturada?.ToString();

				newLabel.Location = new Point(32, 27 + j * 20);
				newLabel.ForeColor = historico[i].peca.cor == Cor.Branca ?
					Color.Gray : Color.Black;
				newLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);

				HistoryLabel.Add(newLabel);
				moves.Controls.Add(newLabel);
			}
		}

		private void FillCapturadas()
		{
			int brancas = 0;
			int pretas = 0;

			foreach(Peca peca in Partida.capturadas)
			{
				PictureBox newPicture = new PictureBox();

				int fator = peca.cor == Cor.Branca ? brancas : pretas;
				int x = 27 + 36 * fator;
				int y = peca.cor == Cor.Branca ? 32 : 69;

				newPicture.Image = GetImage(peca);
				newPicture.Height = 30;
				newPicture.Width = 30;
				newPicture.Location = new Point( x, y);
				newPicture.SizeMode = PictureBoxSizeMode.StretchImage;
				
				if (peca.cor == Cor.Branca)
				{
					brancas++;
				}
				else
				{
					pretas++;
				}

				Capturadas.Add(newPicture);
				capturadas.Controls.Add(newPicture);
			}
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			DialogResult dialog = MessageBox.Show("Deseja reiniciar, seu processo será perdido?", "Reiniciar partida!", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

			if (dialog.Equals(DialogResult.Yes))
			{
				ClearBoard();

				InitializeGame();
			}
		}
	}
}
