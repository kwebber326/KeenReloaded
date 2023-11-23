namespace KeenReloaded
{
    partial class Form1
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
            this._gamePanel = new System.Windows.Forms.Panel();
            this._keenGame = new KeenReloaded.UserControls.KeenGame();
            this.scoreBoard1 = new KeenReloaded.UserControls.ScoreBoard();
            this._gamePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gamePanel
            // 
            this._gamePanel.AutoScroll = true;
            this._gamePanel.Controls.Add(this._keenGame);
            this._gamePanel.Dock = System.Windows.Forms.DockStyle.Right;
            this._gamePanel.Location = new System.Drawing.Point(-416, 0);
            this._gamePanel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this._gamePanel.Name = "_gamePanel";
            this._gamePanel.Size = new System.Drawing.Size(2400, 1263);
            this._gamePanel.TabIndex = 0;
            // 
            // _keenGame
            // 
            this._keenGame.Location = new System.Drawing.Point(440, 0);
            this._keenGame.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this._keenGame.Name = "_keenGame";
            this._keenGame.Size = new System.Drawing.Size(4330, 3846);
            this._keenGame.TabIndex = 0;
            // 
            // scoreBoard1
            // 
            this.scoreBoard1.Dock = System.Windows.Forms.DockStyle.Left;
            this.scoreBoard1.Keen = null;
            this.scoreBoard1.Location = new System.Drawing.Point(0, 0);
            this.scoreBoard1.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.scoreBoard1.Name = "scoreBoard1";
            this.scoreBoard1.Size = new System.Drawing.Size(528, 1263);
            this.scoreBoard1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1984, 1263);
            this.ControlBox = false;
            this.Controls.Add(this.scoreBoard1);
            this.Controls.Add(this._gamePanel);
            this.Cursor = System.Windows.Forms.Cursors.No;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "Form1";
            this.Text = "Keen Reloaded";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this._gamePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _gamePanel;
        private UserControls.KeenGame _keenGame;
        private UserControls.ScoreBoard scoreBoard1;
    }
}

