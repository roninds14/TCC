
namespace JogoXadrez
{
	partial class TabuleiroGame
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TabuleiroGame));
			this.tabuleiro = new System.Windows.Forms.GroupBox();
			this.capturadas = new System.Windows.Forms.GroupBox();
			this.btnExit = new System.Windows.Forms.Button();
			this.btnReset = new System.Windows.Forms.Button();
			this.moves = new System.Windows.Forms.GroupBox();
			this.move1 = new System.Windows.Forms.Label();
			this.move2 = new System.Windows.Forms.Label();
			this.moves.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabuleiro
			// 
			this.tabuleiro.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.tabuleiro.Location = new System.Drawing.Point(12, 12);
			this.tabuleiro.Name = "tabuleiro";
			this.tabuleiro.Size = new System.Drawing.Size(600, 600);
			this.tabuleiro.TabIndex = 0;
			this.tabuleiro.TabStop = false;
			this.tabuleiro.Text = "Tabuleiro";
			// 
			// capturadas
			// 
			this.capturadas.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.capturadas.Location = new System.Drawing.Point(8, 618);
			this.capturadas.Name = "capturadas";
			this.capturadas.Size = new System.Drawing.Size(603, 106);
			this.capturadas.TabIndex = 1;
			this.capturadas.TabStop = false;
			this.capturadas.Text = "Capturadas";
			// 
			// btnExit
			// 
			this.btnExit.BackColor = System.Drawing.Color.Red;
			this.btnExit.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.btnExit.Location = new System.Drawing.Point(617, 689);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(155, 35);
			this.btnExit.TabIndex = 2;
			this.btnExit.Text = "Sair";
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// btnReset
			// 
			this.btnReset.BackColor = System.Drawing.SystemColors.WindowFrame;
			this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.btnReset.Image = ((System.Drawing.Image)(resources.GetObject("btnReset.Image")));
			this.btnReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnReset.Location = new System.Drawing.Point(617, 632);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(155, 35);
			this.btnReset.TabIndex = 3;
			this.btnReset.Text = "Desfazer";
			this.btnReset.UseVisualStyleBackColor = false;
			// 
			// moves
			// 
			this.moves.Controls.Add(this.move2);
			this.moves.Controls.Add(this.move1);
			this.moves.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.moves.Location = new System.Drawing.Point(620, 12);
			this.moves.Name = "moves";
			this.moves.Size = new System.Drawing.Size(152, 600);
			this.moves.TabIndex = 4;
			this.moves.TabStop = false;
			this.moves.Text = "Movimentos";
			// 
			// move1
			// 
			this.move1.AutoSize = true;
			this.move1.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.move1.Location = new System.Drawing.Point(20, 39);
			this.move1.Name = "move1";
			this.move1.Size = new System.Drawing.Size(47, 19);
			this.move1.TabIndex = 5;
			this.move1.Text = "a7-a8";
			// 
			// move2
			// 
			this.move2.AutoSize = true;
			this.move2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.move2.Location = new System.Drawing.Point(85, 40);
			this.move2.Name = "move2";
			this.move2.Size = new System.Drawing.Size(43, 17);
			this.move2.TabIndex = 6;
			this.move2.Text = "b7-d8";
			// 
			// TabuleiroGame
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(780, 761);
			this.Controls.Add(this.moves);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.capturadas);
			this.Controls.Add(this.tabuleiro);
			this.Name = "TabuleiroGame";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Projeto TCC";
			this.moves.ResumeLayout(false);
			this.moves.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox tabuleiro;
		private System.Windows.Forms.GroupBox capturadas;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.GroupBox moves;
		private System.Windows.Forms.Label move2;
		private System.Windows.Forms.Label move1;
	}
}