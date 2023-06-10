using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using tabuleiro;

namespace JogoXadrez.Classes
{
	public class PicturePeca
	{
		public Peca peca;
		public PictureBox pictureBox;

		public PicturePeca(Peca peca, PictureBox pictureBox)
		{
			this.peca = peca;
			this.pictureBox = pictureBox;
		}

	}
}
