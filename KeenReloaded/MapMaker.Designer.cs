namespace KeenReloaded
{
    partial class MapMaker
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
            this.txtMapName = new System.Windows.Forms.TextBox();
            this.cmbGameMode = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbWidth = new System.Windows.Forms.ComboBox();
            this.cmbHeight = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlCanvas = new System.Windows.Forms.Panel();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.objectMenu1 = new KeenReloaded.UserControls.ObjectMenu();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Map Name:";
            // 
            // txtMapName
            // 
            this.txtMapName.Location = new System.Drawing.Point(160, 16);
            this.txtMapName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtMapName.Name = "txtMapName";
            this.txtMapName.Size = new System.Drawing.Size(240, 31);
            this.txtMapName.TabIndex = 1;
            // 
            // cmbGameMode
            // 
            this.cmbGameMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGameMode.FormattingEnabled = true;
            this.cmbGameMode.Location = new System.Drawing.Point(160, 56);
            this.cmbGameMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbGameMode.Name = "cmbGameMode";
            this.cmbGameMode.Size = new System.Drawing.Size(240, 33);
            this.cmbGameMode.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Game Mode:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(440, 16);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(155, 75);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 104);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 25);
            this.label3.TabIndex = 5;
            this.label3.Text = "Dimensions: ";
            // 
            // cmbWidth
            // 
            this.cmbWidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWidth.FormattingEnabled = true;
            this.cmbWidth.Location = new System.Drawing.Point(160, 100);
            this.cmbWidth.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbWidth.Name = "cmbWidth";
            this.cmbWidth.Size = new System.Drawing.Size(100, 33);
            this.cmbWidth.TabIndex = 6;
            this.cmbWidth.SelectedIndexChanged += new System.EventHandler(this.CmbWidth_SelectedIndexChanged);
            // 
            // cmbHeight
            // 
            this.cmbHeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHeight.FormattingEnabled = true;
            this.cmbHeight.Location = new System.Drawing.Point(300, 100);
            this.cmbHeight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbHeight.Name = "cmbHeight";
            this.cmbHeight.Size = new System.Drawing.Size(100, 33);
            this.cmbHeight.TabIndex = 7;
            this.cmbHeight.SelectedIndexChanged += new System.EventHandler(this.CmbHeight_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(271, 100);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 25);
            this.label4.TabIndex = 8;
            this.label4.Text = "x";
            // 
            // pnlCanvas
            // 
            this.pnlCanvas.AllowDrop = true;
            this.pnlCanvas.AutoScroll = true;
            this.pnlCanvas.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlCanvas.Location = new System.Drawing.Point(1000, 20);
            this.pnlCanvas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlCanvas.Name = "pnlCanvas";
            this.pnlCanvas.Size = new System.Drawing.Size(1999, 1874);
            this.pnlCanvas.TabIndex = 10;
            this.pnlCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel1_Paint);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(617, 16);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(163, 75);
            this.btnLoad.TabIndex = 11;
            this.btnLoad.Text = "Load Map";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(801, 16);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(163, 75);
            this.btnTest.TabIndex = 12;
            this.btnTest.Text = "Test Map";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.BtnTest_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // objectMenu1
            // 
            this.objectMenu1.AutoScroll = true;
            this.objectMenu1.Location = new System.Drawing.Point(16, 142);
            this.objectMenu1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.objectMenu1.Name = "objectMenu1";
            this.objectMenu1.Size = new System.Drawing.Size(976, 1318);
            this.objectMenu1.TabIndex = 9;
            // 
            // MapMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(2260, 1474);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.pnlCanvas);
            this.Controls.Add(this.objectMenu1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbHeight);
            this.Controls.Add(this.cmbWidth);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbGameMode);
            this.Controls.Add(this.txtMapName);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MapMaker";
            this.Text = "MapMaker";
            this.Load += new System.EventHandler(this.MapMaker_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MapMaker_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMapName;
        private System.Windows.Forms.ComboBox cmbGameMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbWidth;
        private System.Windows.Forms.ComboBox cmbHeight;
        private System.Windows.Forms.Label label4;
        private UserControls.ObjectMenu objectMenu1;
        private System.Windows.Forms.Panel pnlCanvas;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}