namespace VisualWinForm.Visual.Forms
{
    partial class frmLongProcess
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
            this.pnMain = new System.Windows.Forms.Panel();
            this.cbStop = new System.Windows.Forms.CheckBox();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.lbCaption = new System.Windows.Forms.Label();
            this.pnMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pnMain
            // 
            this.pnMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnMain.Controls.Add(this.cbStop);
            this.pnMain.Controls.Add(this.picBox);
            this.pnMain.Controls.Add(this.lbCaption);
            this.pnMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMain.Location = new System.Drawing.Point(0, 0);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new System.Drawing.Size(130, 32);
            this.pnMain.TabIndex = 4;
            // 
            // cbStop
            // 
            this.cbStop.Location = new System.Drawing.Point(0, 0);
            this.cbStop.Name = "cbStop";
            this.cbStop.Size = new System.Drawing.Size(15, 14);
            this.cbStop.TabIndex = 6;
            this.cbStop.UseVisualStyleBackColor = true;
            this.cbStop.CheckedChanged += new System.EventHandler(this.cbStop_CheckedChanged);
            // 
            // picBox
            // 
            this.picBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picBox.ErrorImage = null;
            this.picBox.InitialImage = null;
            this.picBox.Location = new System.Drawing.Point(0, 0);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(32, 32);
            this.picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBox.TabIndex = 5;
            this.picBox.TabStop = false;
            // 
            // lbCaption
            // 
            this.lbCaption.AutoSize = true;
            this.lbCaption.Font = new System.Drawing.Font("Times New Roman", 15F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbCaption.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbCaption.Location = new System.Drawing.Point(60, 8);
            this.lbCaption.Name = "lbCaption";
            this.lbCaption.Size = new System.Drawing.Size(61, 23);
            this.lbCaption.TabIndex = 4;
            this.lbCaption.Text = "label1";
            // 
            // frmLongProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(130, 32);
            this.Controls.Add(this.pnMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(130, 32);
            this.Name = "frmLongProcess";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmLongProcess";
            this.pnMain.ResumeLayout(false);
            this.pnMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel pnMain;
        public System.Windows.Forms.PictureBox picBox;
        public System.Windows.Forms.Label lbCaption;
        public System.Windows.Forms.CheckBox cbStop;

    }
}