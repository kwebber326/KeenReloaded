namespace KeenReloaded.UserControls
{
    partial class WeaponInventoryBoard
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
            this.currentWeaponLabel = new System.Windows.Forms.Label();
            this.weaponsPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // currentWeaponLabel
            // 
            this.currentWeaponLabel.AutoSize = true;
            this.currentWeaponLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentWeaponLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.currentWeaponLabel.Location = new System.Drawing.Point(4, 0);
            this.currentWeaponLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.currentWeaponLabel.Name = "currentWeaponLabel";
            this.currentWeaponLabel.Size = new System.Drawing.Size(184, 25);
            this.currentWeaponLabel.TabIndex = 1;
            this.currentWeaponLabel.Text = "Current Weapon: ";
            // 
            // weaponsPanel
            // 
            this.weaponsPanel.Location = new System.Drawing.Point(9, 66);
            this.weaponsPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.weaponsPanel.Name = "weaponsPanel";
            this.weaponsPanel.Size = new System.Drawing.Size(322, 641);
            this.weaponsPanel.TabIndex = 2;
            // 
            // WeaponInventoryBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.weaponsPanel);
            this.Controls.Add(this.currentWeaponLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "WeaponInventoryBoard";
            this.Size = new System.Drawing.Size(524, 712);
            this.Load += new System.EventHandler(this.WeaponInventoryBoard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label currentWeaponLabel;
        private System.Windows.Forms.Panel weaponsPanel;
    }
}
