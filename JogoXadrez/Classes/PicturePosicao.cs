using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using tabuleiro;

namespace JogoXadrez.Classes
{
	class PicturePosicao
	{
		public Posicao posicao;
		public PictureBox pictureBox;

		public PicturePosicao(Posicao posicao, PictureBox pictureBox)
		{
			this.posicao = posicao;
			this.pictureBox = pictureBox;
		}
	}
}
