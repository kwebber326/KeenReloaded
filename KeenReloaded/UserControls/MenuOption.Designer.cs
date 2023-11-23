namespace KeenReloaded.UserControls
{
    partial class MenuOption
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
            this.pbKeen = new System.Windows.Forms.PictureBox();
            this.lblOptionText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbKeen)).BeginInit();
            this.SuspendLayout();
            // 
            // pbKeen
            // 
            this.pbKeen.Location = new System.Drawing.Point(3, 3);
            this.pbKeen.Name = "pbKeen";
            this.pbKeen.Size = new System.Drawing.Size(270, 121);
            this.pbKeen.TabIndex = 0;
            this.pbKeen.TabStop = false;
            // 
            // lblOptionText
            // 
            this.lblOptionText.AutoSize = true;
            this.lblOptionText.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOptionText.Location = new System.Drawing.Point(279, 3);
            this.lblOptionText.Name = "lblOptionText";
            this.lblOptionText.Size = new System.Drawing.Size(244, 46);
            this.lblOptionText.TabIndex = 1;
            this.lblOptionText.Text = "Sample Text";
            this.lblOptionText.DoubleClick += new System.EventHandler(this.LblOptionText_DoubleClick);
            // 
            // MenuOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.lblOptionText);
            this.Controls.Add(this.pbKeen);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.Name = "MenuOption";
            this.Size = new System.Drawing.Size(885, 134);
            this.Load += new System.EventHandler(this.MenuOption_Load);
            this.MouseHover += new System.EventHandler(this.MenuOption_MouseHover);
            ((System.ComponentModel.ISupportInitialize)(this.pbKeen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbKeen;
        private System.Windows.Forms.Label lblOptionText;
    }
}
