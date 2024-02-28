namespace VisualWinForm.Visual.Forms
{
    partial class frmLoginWithServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLoginWithServer));
            this.pnServer = new System.Windows.Forms.Panel();
            this.cbServer = new System.Windows.Forms.ComboBox();
            this.lbServer = new System.Windows.Forms.Label();
            this.pnDB = new System.Windows.Forms.Panel();
            this.cbDB = new System.Windows.Forms.ComboBox();
            this.lbDB = new System.Windows.Forms.Label();
            this.pnServer.SuspendLayout();
            this.pnDB.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnServer
            // 
            this.pnServer.Controls.Add(this.cbServer);
            this.pnServer.Controls.Add(this.lbServer);
            this.pnServer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnServer.Location = new System.Drawing.Point(0, 61);
            this.pnServer.Name = "pnServer";
            this.pnServer.Size = new System.Drawing.Size(313, 31);
            this.pnServer.TabIndex = 12;
            // 
            // cbServer
            // 
            this.cbServer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cbServer.FormattingEnabled = true;
            this.cbServer.Location = new System.Drawing.Point(112, 6);
            this.cbServer.MaxDropDownItems = 12;
            this.cbServer.Name = "cbServer";
            this.cbServer.Size = new System.Drawing.Size(192, 23);
            this.cbServer.TabIndex = 3;
            // 
            // lbServer
            // 
            this.lbServer.AutoSize = true;
            this.lbServer.Location = new System.Drawing.Point(7, 9);
            this.lbServer.Name = "lbServer";
            this.lbServer.Size = new System.Drawing.Size(50, 15);
            this.lbServer.TabIndex = 18;
            this.lbServer.Text = "Сервер";
            // 
            // pnDB
            // 
            this.pnDB.Controls.Add(this.cbDB);
            this.pnDB.Controls.Add(this.lbDB);
            this.pnDB.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnDB.Location = new System.Drawing.Point(0, 93);
            this.pnDB.Name = "pnDB";
            this.pnDB.Size = new System.Drawing.Size(313, 31);
            this.pnDB.TabIndex = 14;
            // 
            // cbDB
            // 
            this.cbDB.FormattingEnabled = true;
            this.cbDB.Location = new System.Drawing.Point(112, 6);
            this.cbDB.MaxDropDownItems = 12;
            this.cbDB.Name = "cbDB";
            this.cbDB.Size = new System.Drawing.Size(192, 23);
            this.cbDB.TabIndex = 4;
            // 
            // lbDB
            // 
            this.lbDB.AutoSize = true;
            this.lbDB.Location = new System.Drawing.Point(7, 9);
            this.lbDB.Name = "lbDB";
            this.lbDB.Size = new System.Drawing.Size(81, 15);
            this.lbDB.TabIndex = 18;
            this.lbDB.Text = "База данных";
            // 
            // frmLoginWithServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(313, 260);
            this.Controls.Add(this.pnDB);
            this.Controls.Add(this.pnServer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLoginWithServer";
            this.Controls.SetChildIndex(this.pnServer, 0);
            this.Controls.SetChildIndex(this.pnDB, 0);
            this.pnServer.ResumeLayout(false);
            this.pnServer.PerformLayout();
            this.pnDB.ResumeLayout(false);
            this.pnDB.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnServer;
        private System.Windows.Forms.ComboBox cbServer;
        private System.Windows.Forms.Label lbServer;
        private System.Windows.Forms.Panel pnDB;
        private System.Windows.Forms.ComboBox cbDB;
        private System.Windows.Forms.Label lbDB;
    }
}
