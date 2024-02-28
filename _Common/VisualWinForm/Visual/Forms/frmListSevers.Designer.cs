namespace VisualWinForm.Visual.Forms
{
    partial class frmListSevers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmListSevers));
            this.pnButton = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.pnServers = new System.Windows.Forms.Panel();
            this.lboxServers = new System.Windows.Forms.ListBox();
            this.pnButton.SuspendLayout();
            this.pnServers.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnButton
            // 
            this.pnButton.Controls.Add(this.btnCancel);
            this.pnButton.Controls.Add(this.btnOk);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 332);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(338, 58);
            this.pnButton.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(184, 16);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(140, 27);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(17, 16);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(140, 27);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "Выбор";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // pnServers
            // 
            this.pnServers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnServers.Controls.Add(this.lboxServers);
            this.pnServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnServers.Location = new System.Drawing.Point(0, 0);
            this.pnServers.Name = "pnServers";
            this.pnServers.Size = new System.Drawing.Size(338, 332);
            this.pnServers.TabIndex = 2;
            // 
            // lboxServers
            // 
            this.lboxServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lboxServers.FormattingEnabled = true;
            this.lboxServers.ItemHeight = 15;
            this.lboxServers.Location = new System.Drawing.Point(0, 0);
            this.lboxServers.Name = "lboxServers";
            this.lboxServers.Size = new System.Drawing.Size(334, 328);
            this.lboxServers.TabIndex = 1;
            // 
            // frmListSevers
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(338, 390);
            this.Controls.Add(this.pnServers);
            this.Controls.Add(this.pnButton);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmListSevers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Список SQL серверов";
            this.pnButton.ResumeLayout(false);
            this.pnServers.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Panel pnServers;
        private System.Windows.Forms.ListBox lboxServers;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;

    }
}