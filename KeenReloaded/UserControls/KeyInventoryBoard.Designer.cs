namespace KeenReloaded.UserControls
{
    partial class KeyInventoryBoard
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
            this.inventoryPicBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // inventoryPicBox
            // 
            this.inventoryPicBox.BackColor = System.Drawing.Color.Black;
            this.inventoryPicBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.inventoryPicBox.Image = global::KeenReloaded.Properties.Resources.KeyInventoryTemplate;
            this.inventoryPicBox.Location = new System.Drawing.Point(6, 0);
            this.inventoryPicBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.inventoryPicBox.Name = "inventoryPicBox";
            this.inventoryPicBox.Size = new System.Drawing.Size(154, 35);
            this.inventoryPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.inventoryPicBox.TabIndex = 0;
            this.inventoryPicBox.TabStop = false;
            // 
            // KeyInventoryBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Controls.Add(this.inventoryPicBox);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "KeyInventoryBoard";
            this.Size = new System.Drawing.Size(314, 69);
            this.Load += new System.EventHandler(this.KeyInventoryBoard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.inventoryPicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox inventoryPicBox;
    }
}
