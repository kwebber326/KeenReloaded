namespace KeenReloaded.UserControls
{
    partial class FlagColorScoreKeeper
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbFlagImage = new System.Windows.Forms.PictureBox();
            this.lblScore = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbFlagImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pbFlagImage
            // 
            this.pbFlagImage.Image = global::KeenReloaded.Properties.Resources.Yellow_Flag;
            this.pbFlagImage.Location = new System.Drawing.Point(3, 3);
            this.pbFlagImage.Name = "pbFlagImage";
            this.pbFlagImage.Size = new System.Drawing.Size(24, 33);
            this.pbFlagImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbFlagImage.TabIndex = 0;
            this.pbFlagImage.TabStop = false;
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScore.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.lblScore.Location = new System.Drawing.Point(3, 39);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(35, 13);
            this.lblScore.TabIndex = 1;
            this.lblScore.Text = "label1";
            // 
            // FlagColorScoreKeeper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.pbFlagImage);
            this.Name = "FlagColorScoreKeeper";
            this.Size = new System.Drawing.Size(140, 58);
            ((System.ComponentModel.ISupportInitialize)(this.pbFlagImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbFlagImage;
        private System.Windows.Forms.Label lblScore;
    }
}
