namespace KeenReloaded.UserControls
{
    partial class LoadingAnimation
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
            this.pbLoadingIcon = new System.Windows.Forms.PictureBox();
            this.lblLoadingText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoadingIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // pbLoadingIcon
            // 
            this.pbLoadingIcon.Image = global::KeenReloaded.Properties.Resources.loading1;
            this.pbLoadingIcon.Location = new System.Drawing.Point(43, 32);
            this.pbLoadingIcon.Name = "pbLoadingIcon";
            this.pbLoadingIcon.Size = new System.Drawing.Size(31, 31);
            this.pbLoadingIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLoadingIcon.TabIndex = 0;
            this.pbLoadingIcon.TabStop = false;
            // 
            // lblLoadingText
            // 
            this.lblLoadingText.AutoSize = true;
            this.lblLoadingText.Location = new System.Drawing.Point(4, 4);
            this.lblLoadingText.Name = "lblLoadingText";
            this.lblLoadingText.Size = new System.Drawing.Size(107, 25);
            this.lblLoadingText.TabIndex = 1;
            this.lblLoadingText.Text = "Loading...";
            // 
            // LoadingAnimation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblLoadingText);
            this.Controls.Add(this.pbLoadingIcon);
            this.Name = "LoadingAnimation";
            this.Size = new System.Drawing.Size(119, 106);
            this.Load += new System.EventHandler(this.LoadingAnimation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbLoadingIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbLoadingIcon;
        private System.Windows.Forms.Label lblLoadingText;
    }
}
