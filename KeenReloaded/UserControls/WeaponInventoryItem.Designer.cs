namespace KeenReloaded.UserControls
{
    partial class WeaponInventoryItem
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
            this.weaponPicBox = new System.Windows.Forms.PictureBox();
            this.indexPicBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.weaponPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.indexPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // weaponPicBox
            // 
            this.weaponPicBox.BackColor = System.Drawing.Color.Gray;
            this.weaponPicBox.Location = new System.Drawing.Point(37, 1);
            this.weaponPicBox.Name = "weaponPicBox";
            this.weaponPicBox.Size = new System.Drawing.Size(86, 50);
            this.weaponPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.weaponPicBox.TabIndex = 0;
            this.weaponPicBox.TabStop = false;
            // 
            // indexPicBox
            // 
            this.indexPicBox.Location = new System.Drawing.Point(3, 3);
            this.indexPicBox.Name = "indexPicBox";
            this.indexPicBox.Size = new System.Drawing.Size(28, 48);
            this.indexPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.indexPicBox.TabIndex = 1;
            this.indexPicBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.Location = new System.Drawing.Point(129, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "X";
            this.label1.Visible = false;
            // 
            // WeaponInventoryItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.indexPicBox);
            this.Controls.Add(this.weaponPicBox);
            this.Name = "WeaponInventoryItem";
            this.Size = new System.Drawing.Size(202, 54);
            this.Load += new System.EventHandler(this.WeaponInventoryItem_Load);
            ((System.ComponentModel.ISupportInitialize)(this.weaponPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.indexPicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.PictureBox weaponPicBox;
        protected System.Windows.Forms.PictureBox indexPicBox;
        protected System.Windows.Forms.Label label1;
    }
}
