namespace KeenReloaded.UserControls
{
    partial class ShieldInventoryItem
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
            this.lblKey = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.weaponPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.indexPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // lblKey
            // 
            this.lblKey.AutoSize = true;
            this.lblKey.ForeColor = System.Drawing.SystemColors.Info;
            this.lblKey.Location = new System.Drawing.Point(3, 5);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(53, 20);
            this.lblKey.TabIndex = 6;
            this.lblKey.Text = "Shield";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.Info;
            this.label2.Location = new System.Drawing.Point(4, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "(Alt)";
            // 
            // ShieldInventoryItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblKey);
            this.Name = "ShieldInventoryItem";
            this.Size = new System.Drawing.Size(347, 81);
            this.Load += new System.EventHandler(this.ShieldInventoryItem_Load);
            this.Controls.SetChildIndex(this.weaponPicBox, 0);
            this.Controls.SetChildIndex(this.indexPicBox, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.lblKey, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.weaponPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.indexPicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.Label label2;
    }
}
