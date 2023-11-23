namespace KeenReloaded
{
    partial class HighScoreBoard
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
            this.pbHighScoreBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbHighScoreBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pbHighScoreBox
            // 
            this.pbHighScoreBox.Image = global::KeenReloaded.Properties.Resources.high_score_panel;
            this.pbHighScoreBox.Location = new System.Drawing.Point(2, 1);
            this.pbHighScoreBox.Name = "pbHighScoreBox";
            this.pbHighScoreBox.Size = new System.Drawing.Size(640, 400);
            this.pbHighScoreBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbHighScoreBox.TabIndex = 0;
            this.pbHighScoreBox.TabStop = false;
            // 
            // HighScoreBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 614);
            this.Controls.Add(this.pbHighScoreBox);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(982, 670);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(982, 670);
            this.Name = "HighScoreBoard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sample Text";
            this.Load += new System.EventHandler(this.HighScoreBoard_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HighScoreBoard_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pbHighScoreBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbHighScoreBox;
    }
}