namespace KeenReloaded.UserControls
{
    partial class ObjectMenu
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.pnlItems = new System.Windows.Forms.Panel();
            this.pnlSpecialProperties = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbEpisode = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbBiome = new System.Windows.Forms.ComboBox();
            this.pbSelectedItem = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbEditMode = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbSelectedItem)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Category:";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(86, 3);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(160, 28);
            this.cmbCategory.TabIndex = 1;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.CmbCategory_SelectedIndexChanged);
            // 
            // pnlItems
            // 
            this.pnlItems.AutoScroll = true;
            this.pnlItems.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pnlItems.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlItems.Location = new System.Drawing.Point(7, 153);
            this.pnlItems.Name = "pnlItems";
            this.pnlItems.Size = new System.Drawing.Size(681, 549);
            this.pnlItems.TabIndex = 2;
            // 
            // pnlSpecialProperties
            // 
            this.pnlSpecialProperties.AutoScroll = true;
            this.pnlSpecialProperties.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pnlSpecialProperties.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlSpecialProperties.Location = new System.Drawing.Point(7, 708);
            this.pnlSpecialProperties.Name = "pnlSpecialProperties";
            this.pnlSpecialProperties.Size = new System.Drawing.Size(681, 285);
            this.pnlSpecialProperties.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(262, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Episode:";
            // 
            // cmbEpisode
            // 
            this.cmbEpisode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEpisode.FormattingEnabled = true;
            this.cmbEpisode.Location = new System.Drawing.Point(339, 3);
            this.cmbEpisode.Name = "cmbEpisode";
            this.cmbEpisode.Size = new System.Drawing.Size(59, 28);
            this.cmbEpisode.TabIndex = 5;
            this.cmbEpisode.SelectedIndexChanged += new System.EventHandler(this.CmbEpisode_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(405, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Biome:";
            // 
            // cmbBiome
            // 
            this.cmbBiome.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBiome.FormattingEnabled = true;
            this.cmbBiome.Location = new System.Drawing.Point(469, 3);
            this.cmbBiome.Name = "cmbBiome";
            this.cmbBiome.Size = new System.Drawing.Size(219, 28);
            this.cmbBiome.TabIndex = 7;
            this.cmbBiome.SelectedIndexChanged += new System.EventHandler(this.CmbBiome_SelectedIndexChanged);
            // 
            // pbSelectedItem
            // 
            this.pbSelectedItem.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pbSelectedItem.Location = new System.Drawing.Point(117, 37);
            this.pbSelectedItem.Name = "pbSelectedItem";
            this.pbSelectedItem.Size = new System.Drawing.Size(64, 64);
            this.pbSelectedItem.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbSelectedItem.TabIndex = 8;
            this.pbSelectedItem.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "SelectedItem:";
            // 
            // cmbEditMode
            // 
            this.cmbEditMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEditMode.FormattingEnabled = true;
            this.cmbEditMode.Location = new System.Drawing.Point(469, 38);
            this.cmbEditMode.Name = "cmbEditMode";
            this.cmbEditMode.Size = new System.Drawing.Size(219, 28);
            this.cmbEditMode.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(378, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Edit Mode:";
            // 
            // ObjectMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbEditMode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pbSelectedItem);
            this.Controls.Add(this.cmbBiome);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbEpisode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pnlSpecialProperties);
            this.Controls.Add(this.pnlItems);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.label1);
            this.Name = "ObjectMenu";
            this.Size = new System.Drawing.Size(702, 1010);
            this.Load += new System.EventHandler(this.ObjectMenu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbSelectedItem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Panel pnlItems;
        private System.Windows.Forms.Panel pnlSpecialProperties;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbEpisode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbBiome;
        private System.Windows.Forms.PictureBox pbSelectedItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbEditMode;
        private System.Windows.Forms.Label label5;
    }
}
