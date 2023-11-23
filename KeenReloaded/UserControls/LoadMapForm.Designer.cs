namespace KeenReloaded.UserControls
{
    partial class LoadMapForm
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lblMapName = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnPlayMap = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(425, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(259, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose Normal Map:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // lblMapName
            // 
            this.lblMapName.AutoSize = true;
            this.lblMapName.Location = new System.Drawing.Point(640, 112);
            this.lblMapName.Name = "lblMapName";
            this.lblMapName.Size = new System.Drawing.Size(171, 25);
            this.lblMapName.TabIndex = 1;
            this.lblMapName.Text = "<none selected>";
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(515, 112);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 46);
            this.button1.TabIndex = 2;
            this.button1.Text = "Browse...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // btnPlayMap
            // 
            this.btnPlayMap.Enabled = false;
            this.btnPlayMap.ForeColor = System.Drawing.Color.Black;
            this.btnPlayMap.Location = new System.Drawing.Point(515, 184);
            this.btnPlayMap.Name = "btnPlayMap";
            this.btnPlayMap.Size = new System.Drawing.Size(119, 46);
            this.btnPlayMap.TabIndex = 3;
            this.btnPlayMap.Text = "Play Map";
            this.btnPlayMap.UseVisualStyleBackColor = true;
            this.btnPlayMap.Click += new System.EventHandler(this.BtnPlayMap_Click);
            // 
            // LoadMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(1121, 686);
            this.Controls.Add(this.btnPlayMap);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblMapName);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.Lime;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1145, 744);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1145, 744);
            this.Name = "LoadMapForm";
            this.Text = "LoadMapForm";
            this.Load += new System.EventHandler(this.LoadMapForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label lblMapName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnPlayMap;
    }
}