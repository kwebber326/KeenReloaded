namespace KeenReloaded.UserControls
{
    partial class ScoreBoard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScoreBoard));
            this.pictureBoxScoreBoard = new System.Windows.Forms.PictureBox();
            this.flagInventoryBoard1 = new KeenReloaded.UserControls.FlagInventoryBoard();
            this.shieldInventoryItem1 = new KeenReloaded.UserControls.ShieldInventoryItem();
            this.keyCardInventoryBoard1 = new KeenReloaded.UserControls.KeyCardInventoryBoard();
            this.lifeletCounter1 = new KeenReloaded.UserControls.LifeletCounter();
            this.keyInventoryBoard1 = new KeenReloaded.UserControls.KeyInventoryBoard();
            this.weaponInventoryBoard1 = new KeenReloaded.UserControls.WeaponInventoryBoard();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxScoreBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxScoreBoard
            // 
            this.pictureBoxScoreBoard.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxScoreBoard.Image")));
            this.pictureBoxScoreBoard.Location = new System.Drawing.Point(6, 5);
            this.pictureBoxScoreBoard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBoxScoreBoard.Name = "pictureBoxScoreBoard";
            this.pictureBoxScoreBoard.Size = new System.Drawing.Size(248, 100);
            this.pictureBoxScoreBoard.TabIndex = 0;
            this.pictureBoxScoreBoard.TabStop = false;
            // 
            // flagInventoryBoard1
            // 
            this.flagInventoryBoard1.Keen = null;
            this.flagInventoryBoard1.Location = new System.Drawing.Point(261, 803);
            this.flagInventoryBoard1.Name = "flagInventoryBoard1";
            this.flagInventoryBoard1.Size = new System.Drawing.Size(166, 285);
            this.flagInventoryBoard1.TabIndex = 6;
            // 
            // shieldInventoryItem1
            // 
            this.shieldInventoryItem1.BackColor = System.Drawing.Color.Black;
            this.shieldInventoryItem1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.shieldInventoryItem1.Keen = null;
            this.shieldInventoryItem1.Location = new System.Drawing.Point(4, 829);
            this.shieldInventoryItem1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.shieldInventoryItem1.Name = "shieldInventoryItem1";
            this.shieldInventoryItem1.Size = new System.Drawing.Size(250, 71);
            this.shieldInventoryItem1.TabIndex = 5;
            this.shieldInventoryItem1.Weapon = null;
            this.shieldInventoryItem1.WeaponIndex = 0;
            this.shieldInventoryItem1.Load += new System.EventHandler(this.ShieldInventoryItem1_Load);
            // 
            // keyCardInventoryBoard1
            // 
            this.keyCardInventoryBoard1.AutoSize = true;
            this.keyCardInventoryBoard1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.keyCardInventoryBoard1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.keyCardInventoryBoard1.Keen = null;
            this.keyCardInventoryBoard1.Location = new System.Drawing.Point(10, 1038);
            this.keyCardInventoryBoard1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.keyCardInventoryBoard1.Name = "keyCardInventoryBoard1";
            this.keyCardInventoryBoard1.Size = new System.Drawing.Size(229, 50);
            this.keyCardInventoryBoard1.TabIndex = 4;
            // 
            // lifeletCounter1
            // 
            this.lifeletCounter1.BackColor = System.Drawing.Color.DarkGray;
            this.lifeletCounter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lifeletCounter1.Keen = null;
            this.lifeletCounter1.Location = new System.Drawing.Point(10, 959);
            this.lifeletCounter1.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.lifeletCounter1.Name = "lifeletCounter1";
            this.lifeletCounter1.Size = new System.Drawing.Size(229, 74);
            this.lifeletCounter1.TabIndex = 3;
            // 
            // keyInventoryBoard1
            // 
            this.keyInventoryBoard1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.keyInventoryBoard1.BlueGem = null;
            this.keyInventoryBoard1.GreenGem = null;
            this.keyInventoryBoard1.Keen = null;
            this.keyInventoryBoard1.Location = new System.Drawing.Point(6, 901);
            this.keyInventoryBoard1.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.keyInventoryBoard1.Name = "keyInventoryBoard1";
            this.keyInventoryBoard1.RedGem = null;
            this.keyInventoryBoard1.Size = new System.Drawing.Size(236, 82);
            this.keyInventoryBoard1.TabIndex = 2;
            this.keyInventoryBoard1.YellowGem = null;
            // 
            // weaponInventoryBoard1
            // 
            this.weaponInventoryBoard1.Keen = null;
            this.weaponInventoryBoard1.Location = new System.Drawing.Point(6, 114);
            this.weaponInventoryBoard1.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.weaponInventoryBoard1.Name = "weaponInventoryBoard1";
            this.weaponInventoryBoard1.SelectedWeapon = null;
            this.weaponInventoryBoard1.Size = new System.Drawing.Size(386, 676);
            this.weaponInventoryBoard1.TabIndex = 1;
            // 
            // ScoreBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flagInventoryBoard1);
            this.Controls.Add(this.shieldInventoryItem1);
            this.Controls.Add(this.keyCardInventoryBoard1);
            this.Controls.Add(this.lifeletCounter1);
            this.Controls.Add(this.keyInventoryBoard1);
            this.Controls.Add(this.weaponInventoryBoard1);
            this.Controls.Add(this.pictureBoxScoreBoard);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ScoreBoard";
            this.Size = new System.Drawing.Size(465, 1094);
            this.Load += new System.EventHandler(this.ScoreBoard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxScoreBoard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxScoreBoard;
        private WeaponInventoryBoard weaponInventoryBoard1;
        private KeyInventoryBoard keyInventoryBoard1;
        private LifeletCounter lifeletCounter1;
        private KeyCardInventoryBoard keyCardInventoryBoard1;
        private ShieldInventoryItem shieldInventoryItem1;
        private FlagInventoryBoard flagInventoryBoard1;
    }
}
