namespace VisualWinForm.Visual.Forms
{
    partial class frmMessageDialog
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
            this.components = new System.ComponentModel.Container();
            this.toolTipMessage = new System.Windows.Forms.ToolTip(this.components);
            this.btnBuffer = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnButton = new System.Windows.Forms.Panel();
            this.lbMessage = new System.Windows.Forms.Label();
            this.pnLeft = new System.Windows.Forms.Panel();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.pnMessage = new System.Windows.Forms.Panel();
            this.pnButton.SuspendLayout();
            this.pnLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.pnMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBuffer
            // 
            this.btnBuffer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuffer.Location = new System.Drawing.Point(146, 11);
            this.btnBuffer.Name = "btnBuffer";
            this.btnBuffer.Size = new System.Drawing.Size(27, 25);
            this.btnBuffer.TabIndex = 4;
            this.toolTipMessage.SetToolTip(this.btnBuffer, "Скопировать сообщение в буфер обмена (Ctrl+B)");
            this.btnBuffer.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(81, 11);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(27, 25);
            this.btnSave.TabIndex = 3;
            this.toolTipMessage.SetToolTip(this.btnSave, "Сохранить сообщение в файле (Ctrl+S)");
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // pnButton
            // 
            this.pnButton.Controls.Add(this.btnBuffer);
            this.pnButton.Controls.Add(this.btnSave);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 45);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(236, 40);
            this.pnButton.TabIndex = 44;
            // 
            // lbMessage
            // 
            this.lbMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbMessage.Location = new System.Drawing.Point(0, 0);
            this.lbMessage.Name = "lbMessage";
            this.lbMessage.Size = new System.Drawing.Size(181, 35);
            this.lbMessage.TabIndex = 45;
            this.lbMessage.Text = "label1;";
            this.lbMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnLeft
            // 
            this.pnLeft.Controls.Add(this.picBox);
            this.pnLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnLeft.Location = new System.Drawing.Point(0, 10);
            this.pnLeft.Name = "pnLeft";
            this.pnLeft.Size = new System.Drawing.Size(55, 35);
            this.pnLeft.TabIndex = 46;
            // 
            // picBox
            // 
            this.picBox.Location = new System.Drawing.Point(12, 2);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(32, 32);
            this.picBox.TabIndex = 0;
            this.picBox.TabStop = false;
            // 
            // pnMessage
            // 
            this.pnMessage.Controls.Add(this.lbMessage);
            this.pnMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMessage.Location = new System.Drawing.Point(55, 10);
            this.pnMessage.Name = "pnMessage";
            this.pnMessage.Size = new System.Drawing.Size(181, 35);
            this.pnMessage.TabIndex = 47;
            // 
            // frmMessageDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(241, 85);
            this.Controls.Add(this.pnMessage);
            this.Controls.Add(this.pnLeft);
            this.Controls.Add(this.pnButton);
            this.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMessageDialog";
            this.Padding = new System.Windows.Forms.Padding(0, 10, 5, 0);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.onKeyUp);
            this.pnButton.ResumeLayout(false);
            this.pnLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.pnMessage.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.ToolTip toolTipMessage;
        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btnBuffer;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lbMessage;
        private System.Windows.Forms.Panel pnLeft;
        private System.Windows.Forms.PictureBox picBox;
        private System.Windows.Forms.Panel pnMessage;

	}
}