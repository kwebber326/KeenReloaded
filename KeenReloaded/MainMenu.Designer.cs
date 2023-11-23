namespace KeenReloaded
{
    partial class MainMenu
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
            this.label1 = new System.Windows.Forms.Label();
            this.pnlMenuOptions = new System.Windows.Forms.Panel();
            this.cmbCharacters = new System.Windows.Forms.ComboBox();
            this.lblCharacter = new System.Windows.Forms.Label();
            this.btnRandomCharacter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Lime;
            this.label1.Location = new System.Drawing.Point(624, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(307, 46);
            this.label1.TabIndex = 0;
            this.label1.Text = "Keen Reloaded";
            this.label1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // pnlMenuOptions
            // 
            this.pnlMenuOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMenuOptions.AutoScroll = true;
            this.pnlMenuOptions.Location = new System.Drawing.Point(401, 146);
            this.pnlMenuOptions.MaximumSize = new System.Drawing.Size(1157, 825);
            this.pnlMenuOptions.MinimumSize = new System.Drawing.Size(1157, 825);
            this.pnlMenuOptions.Name = "pnlMenuOptions";
            this.pnlMenuOptions.Size = new System.Drawing.Size(1157, 825);
            this.pnlMenuOptions.TabIndex = 1;
            // 
            // cmbCharacters
            // 
            this.cmbCharacters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCharacters.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCharacters.FormattingEnabled = true;
            this.cmbCharacters.Location = new System.Drawing.Point(12, 146);
            this.cmbCharacters.Name = "cmbCharacters";
            this.cmbCharacters.Size = new System.Drawing.Size(383, 37);
            this.cmbCharacters.TabIndex = 2;
            this.cmbCharacters.SelectedIndexChanged += new System.EventHandler(this.CmbCharacters_SelectedIndexChanged);
            // 
            // lblCharacter
            // 
            this.lblCharacter.AutoSize = true;
            this.lblCharacter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCharacter.ForeColor = System.Drawing.Color.Lime;
            this.lblCharacter.Location = new System.Drawing.Point(12, 114);
            this.lblCharacter.Name = "lblCharacter";
            this.lblCharacter.Size = new System.Drawing.Size(244, 29);
            this.lblCharacter.TabIndex = 3;
            this.lblCharacter.Text = "Selected Character:";
            // 
            // btnRandomCharacter
            // 
            this.btnRandomCharacter.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnRandomCharacter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRandomCharacter.ForeColor = System.Drawing.Color.Lime;
            this.btnRandomCharacter.Location = new System.Drawing.Point(12, 190);
            this.btnRandomCharacter.Name = "btnRandomCharacter";
            this.btnRandomCharacter.Size = new System.Drawing.Size(383, 50);
            this.btnRandomCharacter.TabIndex = 4;
            this.btnRandomCharacter.Text = "Choose Random Character";
            this.btnRandomCharacter.UseVisualStyleBackColor = false;
            this.btnRandomCharacter.Click += new System.EventHandler(this.BtnRandomCharacter_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1284, 701);
            this.Controls.Add(this.btnRandomCharacter);
            this.Controls.Add(this.lblCharacter);
            this.Controls.Add(this.cmbCharacters);
            this.Controls.Add(this.pnlMenuOptions);
            this.Controls.Add(this.label1);
            this.Name = "MainMenu";
            this.Text = "Main Menu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainMenu_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainMenu_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlMenuOptions;
        private System.Windows.Forms.ComboBox cmbCharacters;
        private System.Windows.Forms.Label lblCharacter;
        private System.Windows.Forms.Button btnRandomCharacter;
    }
}