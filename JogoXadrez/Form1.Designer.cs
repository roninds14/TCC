
namespace JogoXadrez
{
	partial class Selection
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.onePlayer = new System.Windows.Forms.RadioButton();
			this.projectTitle = new System.Windows.Forms.Label();
			this.gameName = new System.Windows.Forms.Label();
			this.devolep = new System.Windows.Forms.Label();
			this.start = new System.Windows.Forms.Button();
			this.colorWhite = new System.Windows.Forms.RadioButton();
			this.colorBlack = new System.Windows.Forms.RadioButton();
			this.twoPlayers = new System.Windows.Forms.RadioButton();
			this.numberOfPlayers = new System.Windows.Forms.GroupBox();
			this.colorSelector = new System.Windows.Forms.GroupBox();
			this.numberOfPlayers.SuspendLayout();
			this.colorSelector.SuspendLayout();
			this.SuspendLayout();
			// 
			// onePlayer
			// 
			this.onePlayer.AutoSize = true;
			this.onePlayer.Location = new System.Drawing.Point(63, 20);
			this.onePlayer.Name = "onePlayer";
			this.onePlayer.Size = new System.Drawing.Size(31, 19);
			this.onePlayer.TabIndex = 8;
			this.onePlayer.TabStop = true;
			this.onePlayer.Tag = "";
			this.onePlayer.Text = "1";
			this.onePlayer.UseVisualStyleBackColor = true;
			// 
			// projectTitle
			// 
			this.projectTitle.AutoSize = true;
			this.projectTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.projectTitle.Location = new System.Drawing.Point(141, 9);
			this.projectTitle.Name = "projectTitle";
			this.projectTitle.Size = new System.Drawing.Size(168, 37);
			this.projectTitle.TabIndex = 0;
			this.projectTitle.Text = "Projeto TCC";
			// 
			// gameName
			// 
			this.gameName.AutoSize = true;
			this.gameName.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.gameName.Location = new System.Drawing.Point(136, 64);
			this.gameName.Name = "gameName";
			this.gameName.Size = new System.Drawing.Size(178, 32);
			this.gameName.TabIndex = 1;
			this.gameName.Text = "Jogo de Xadrez";
			// 
			// devolep
			// 
			this.devolep.AutoSize = true;
			this.devolep.Location = new System.Drawing.Point(233, 257);
			this.devolep.Name = "devolep";
			this.devolep.Size = new System.Drawing.Size(189, 15);
			this.devolep.TabIndex = 2;
			this.devolep.Text = "Desenvolvido por: Roni dos Santos";
			// 
			// start
			// 
			this.start.Location = new System.Drawing.Point(178, 214);
			this.start.Name = "start";
			this.start.Size = new System.Drawing.Size(76, 23);
			this.start.TabIndex = 5;
			this.start.Text = "Iniciar";
			this.start.UseVisualStyleBackColor = true;
			this.start.Click += new System.EventHandler(this.initializeGame);
			// 
			// colorWhite
			// 
			this.colorWhite.AutoSize = true;
			this.colorWhite.Location = new System.Drawing.Point(33, 20);
			this.colorWhite.Name = "colorWhite";
			this.colorWhite.Size = new System.Drawing.Size(62, 19);
			this.colorWhite.TabIndex = 6;
			this.colorWhite.TabStop = true;
			this.colorWhite.Tag = "";
			this.colorWhite.Text = "Branco";
			this.colorWhite.UseVisualStyleBackColor = true;
			// 
			// colorBlack
			// 
			this.colorBlack.AutoSize = true;
			this.colorBlack.Location = new System.Drawing.Point(101, 20);
			this.colorBlack.Name = "colorBlack";
			this.colorBlack.Size = new System.Drawing.Size(53, 19);
			this.colorBlack.TabIndex = 7;
			this.colorBlack.TabStop = true;
			this.colorBlack.Tag = "";
			this.colorBlack.Text = "Preto";
			this.colorBlack.UseVisualStyleBackColor = true;
			// 
			// twoPlayers
			// 
			this.twoPlayers.AutoSize = true;
			this.twoPlayers.Location = new System.Drawing.Point(100, 20);
			this.twoPlayers.Name = "twoPlayers";
			this.twoPlayers.Size = new System.Drawing.Size(31, 19);
			this.twoPlayers.TabIndex = 9;
			this.twoPlayers.TabStop = true;
			this.twoPlayers.Tag = "";
			this.twoPlayers.Text = "2";
			this.twoPlayers.UseVisualStyleBackColor = true;
			// 
			// numberOfPlayers
			// 
			this.numberOfPlayers.Controls.Add(this.onePlayer);
			this.numberOfPlayers.Controls.Add(this.twoPlayers);
			this.numberOfPlayers.Location = new System.Drawing.Point(24, 132);
			this.numberOfPlayers.Name = "numberOfPlayers";
			this.numberOfPlayers.Size = new System.Drawing.Size(194, 45);
			this.numberOfPlayers.TabIndex = 10;
			this.numberOfPlayers.TabStop = false;
			this.numberOfPlayers.Text = "Selecine o Numero de Jogadores";
			// 
			// colorSelector
			// 
			this.colorSelector.Controls.Add(this.colorWhite);
			this.colorSelector.Controls.Add(this.colorBlack);
			this.colorSelector.Location = new System.Drawing.Point(246, 132);
			this.colorSelector.Name = "colorSelector";
			this.colorSelector.Size = new System.Drawing.Size(176, 45);
			this.colorSelector.TabIndex = 11;
			this.colorSelector.TabStop = false;
			this.colorSelector.Text = "Selecione a sua Cor";
			// 
			// Selection
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(434, 281);
			this.Controls.Add(this.colorSelector);
			this.Controls.Add(this.numberOfPlayers);
			this.Controls.Add(this.start);
			this.Controls.Add(this.devolep);
			this.Controls.Add(this.gameName);
			this.Controls.Add(this.projectTitle);
			this.Name = "Selection";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Projeto de TCC";
			this.numberOfPlayers.ResumeLayout(false);
			this.numberOfPlayers.PerformLayout();
			this.colorSelector.ResumeLayout(false);
			this.colorSelector.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label projectTitle;
		private System.Windows.Forms.Label gameName;
		private System.Windows.Forms.Label devolep;
		private System.Windows.Forms.Button start;
		private System.Windows.Forms.RadioButton colorWhite;
		private System.Windows.Forms.RadioButton colorBlack;
		private System.Windows.Forms.RadioButton onePlayer;
		private System.Windows.Forms.RadioButton twoPlayers;
		private System.Windows.Forms.GroupBox numberOfPlayers;
		private System.Windows.Forms.GroupBox colorSelector;
	}
}

