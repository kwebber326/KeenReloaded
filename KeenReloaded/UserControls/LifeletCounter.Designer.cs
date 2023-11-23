namespace KeenReloaded.UserControls
{
    partial class LifeletCounter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LifeletCounter));
            this.pBoxBoard = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // pBoxBoard
            // 
            this.pBoxBoard.Image = ((System.Drawing.Image)(resources.GetObject("pBoxBoard.Image")));
            this.pBoxBoard.Location = new System.Drawing.Point(3, 0);
            this.pBoxBoard.Name = "pBoxBoard";
            this.pBoxBoard.Size = new System.Drawing.Size(134, 44);
            this.pBoxBoard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pBoxBoard.TabIndex = 0;
            this.pBoxBoard.TabStop = false;
            // 
            // LifeletCounter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pBoxBoard);
            this.Name = "LifeletCounter";
            this.Size = new System.Drawing.Size(139, 43);
            this.Load += new System.EventHandler(this.LifeletCounter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pBoxBoard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pBoxBoard;

    }
}
