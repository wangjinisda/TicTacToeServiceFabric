namespace Nelson.TicTacToe.Client
{
    partial class FrmPlayerChoice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPlayerChoice));
            this.cbPlayerChoice = new System.Windows.Forms.ComboBox();
            this.btnStartGame = new System.Windows.Forms.Button();
            this.lblPlayerChoice = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGameRoom = new System.Windows.Forms.TextBox();
            this.btnEndGame = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPlayer = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cbPlayerChoice
            // 
            this.cbPlayerChoice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPlayerChoice.FormattingEnabled = true;
            this.cbPlayerChoice.Items.AddRange(new object[] {
            "Cross",
            "Zero"});
            this.cbPlayerChoice.Location = new System.Drawing.Point(241, 156);
            this.cbPlayerChoice.MaxDropDownItems = 2;
            this.cbPlayerChoice.Name = "cbPlayerChoice";
            this.cbPlayerChoice.Size = new System.Drawing.Size(251, 21);
            this.cbPlayerChoice.TabIndex = 2;
            // 
            // btnStartGame
            // 
            this.btnStartGame.Location = new System.Drawing.Point(241, 194);
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.Size = new System.Drawing.Size(75, 23);
            this.btnStartGame.TabIndex = 3;
            this.btnStartGame.Text = "Start game";
            this.btnStartGame.UseVisualStyleBackColor = true;
            this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_Click);
            // 
            // lblPlayerChoice
            // 
            this.lblPlayerChoice.AutoSize = true;
            this.lblPlayerChoice.Location = new System.Drawing.Point(158, 156);
            this.lblPlayerChoice.Name = "lblPlayerChoice";
            this.lblPlayerChoice.Size = new System.Drawing.Size(77, 13);
            this.lblPlayerChoice.TabIndex = 2;
            this.lblPlayerChoice.Text = "Player choice :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(168, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Game room :";
            // 
            // txtGameRoom
            // 
            this.txtGameRoom.Location = new System.Drawing.Point(241, 74);
            this.txtGameRoom.Name = "txtGameRoom";
            this.txtGameRoom.Size = new System.Drawing.Size(251, 20);
            this.txtGameRoom.TabIndex = 0;
            this.txtGameRoom.Text = "Game room";
            // 
            // btnEndGame
            // 
            this.btnEndGame.Location = new System.Drawing.Point(323, 194);
            this.btnEndGame.Name = "btnEndGame";
            this.btnEndGame.Size = new System.Drawing.Size(75, 23);
            this.btnEndGame.TabIndex = 4;
            this.btnEndGame.Text = "End game";
            this.btnEndGame.UseVisualStyleBackColor = true;
            this.btnEndGame.Click += new System.EventHandler(this.btnEndGame_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(164, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Player name :";
            // 
            // txtPlayer
            // 
            this.txtPlayer.Location = new System.Drawing.Point(241, 115);
            this.txtPlayer.Name = "txtPlayer";
            this.txtPlayer.Size = new System.Drawing.Size(251, 20);
            this.txtPlayer.TabIndex = 1;
            // 
            // FrmPlayerChoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 316);
            this.Controls.Add(this.txtPlayer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtGameRoom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblPlayerChoice);
            this.Controls.Add(this.btnEndGame);
            this.Controls.Add(this.btnStartGame);
            this.Controls.Add(this.cbPlayerChoice);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmPlayerChoice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TicTacToe";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbPlayerChoice;
        private System.Windows.Forms.Button btnStartGame;
        private System.Windows.Forms.Label lblPlayerChoice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGameRoom;
        private System.Windows.Forms.Button btnEndGame;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPlayer;
    }
}