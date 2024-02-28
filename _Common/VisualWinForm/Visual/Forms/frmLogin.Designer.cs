namespace VisualWinForm.Visual.Forms
{
    partial class frmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.pnLogin = new System.Windows.Forms.Panel();
            this.edUser = new System.Windows.Forms.TextBox();
            this.edPassword = new System.Windows.Forms.TextBox();
            this.lbPassword = new System.Windows.Forms.Label();
            this.lbUser = new System.Windows.Forms.Label();
            this.pnButton = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.pnBevel = new System.Windows.Forms.Panel();
            this.gbTypeAut = new System.Windows.Forms.GroupBox();
            this.rbSQL = new System.Windows.Forms.RadioButton();
            this.rbWin = new System.Windows.Forms.RadioButton();
            this.pnLogin.SuspendLayout();
            this.pnButton.SuspendLayout();
            this.gbTypeAut.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnLogin
            // 
            this.pnLogin.Controls.Add(this.edUser);
            this.pnLogin.Controls.Add(this.edPassword);
            this.pnLogin.Controls.Add(this.lbPassword);
            this.pnLogin.Controls.Add(this.lbUser);
            this.pnLogin.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnLogin.Location = new System.Drawing.Point(0, 68);
            this.pnLogin.Name = "pnLogin";
            this.pnLogin.Size = new System.Drawing.Size(311, 82);
            this.pnLogin.TabIndex = 4;
            // 
            // edUser
            // 
            this.edUser.Location = new System.Drawing.Point(112, 8);
            this.edUser.Name = "edUser";
            this.edUser.Size = new System.Drawing.Size(192, 21);
            this.edUser.TabIndex = 5;
            // 
            // edPassword
            // 
            this.edPassword.AcceptsReturn = true;
            this.edPassword.Location = new System.Drawing.Point(112, 42);
            this.edPassword.Name = "edPassword";
            this.edPassword.PasswordChar = '*';
            this.edPassword.Size = new System.Drawing.Size(192, 21);
            this.edPassword.TabIndex = 6;
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.Location = new System.Drawing.Point(7, 45);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(51, 15);
            this.lbPassword.TabIndex = 11;
            this.lbPassword.Text = "Пароль";
            // 
            // lbUser
            // 
            this.lbUser.AutoSize = true;
            this.lbUser.Location = new System.Drawing.Point(7, 12);
            this.lbUser.Name = "lbUser";
            this.lbUser.Size = new System.Drawing.Size(92, 15);
            this.lbUser.TabIndex = 10;
            this.lbUser.Text = "Пользователь";
            // 
            // pnButton
            // 
            this.pnButton.Controls.Add(this.btnCancel);
            this.pnButton.Controls.Add(this.btnOk);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnButton.Location = new System.Drawing.Point(0, 151);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(311, 53);
            this.pnButton.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(190, 14);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(111, 27);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(69, 14);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(111, 27);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "Вход";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // pnBevel
            // 
            this.pnBevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnBevel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnBevel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnBevel.Location = new System.Drawing.Point(0, 150);
            this.pnBevel.Name = "pnBevel";
            this.pnBevel.Size = new System.Drawing.Size(311, 1);
            this.pnBevel.TabIndex = 11;
            // 
            // gbTypeAut
            // 
            this.gbTypeAut.Controls.Add(this.rbSQL);
            this.gbTypeAut.Controls.Add(this.rbWin);
            this.gbTypeAut.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbTypeAut.Location = new System.Drawing.Point(0, 0);
            this.gbTypeAut.Name = "gbTypeAut";
            this.gbTypeAut.Size = new System.Drawing.Size(311, 61);
            this.gbTypeAut.TabIndex = 1;
            this.gbTypeAut.TabStop = false;
            this.gbTypeAut.Text = "Соединится используя авторизацию:";
            // 
            // rbSQL
            // 
            this.rbSQL.AutoSize = true;
            this.rbSQL.Location = new System.Drawing.Point(127, 28);
            this.rbSQL.Name = "rbSQL";
            this.rbSQL.Size = new System.Drawing.Size(80, 17);
            this.rbSQL.TabIndex = 3;
            this.rbSQL.Text = "SQL Server";
            this.rbSQL.UseVisualStyleBackColor = true;
            // 
            // rbWin
            // 
            this.rbWin.AutoSize = true;
            this.rbWin.Location = new System.Drawing.Point(14, 28);
            this.rbWin.Name = "rbWin";
            this.rbWin.Size = new System.Drawing.Size(69, 17);
            this.rbWin.TabIndex = 2;
            this.rbWin.Text = "Windows";
            this.rbWin.UseVisualStyleBackColor = true;
            // 
            // frmLogin
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(311, 204);
            this.Controls.Add(this.gbTypeAut);
            this.Controls.Add(this.pnLogin);
            this.Controls.Add(this.pnBevel);
            this.Controls.Add(this.pnButton);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmLogin";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.pnLogin.ResumeLayout(false);
            this.pnLogin.PerformLayout();
            this.pnButton.ResumeLayout(false);
            this.gbTypeAut.ResumeLayout(false);
            this.gbTypeAut.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnLogin;
        private System.Windows.Forms.TextBox edUser;
        private System.Windows.Forms.Label lbPassword;
        private System.Windows.Forms.Label lbUser;
        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel pnBevel;
        private System.Windows.Forms.GroupBox gbTypeAut;
        private System.Windows.Forms.RadioButton rbSQL;
        private System.Windows.Forms.RadioButton rbWin;
        protected System.Windows.Forms.TextBox edPassword;
    }
}