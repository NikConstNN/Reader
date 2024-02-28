using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.DB_Classes;
using VisualWinForm.Visual.Tools;

namespace VisualWinForm.Visual.Forms
{
    public partial class frmLogin : Form
    {
        public Logon m_logon = null;
        public int m_attempt;

        public frmLogin() : this(null, string.Empty) { }
        public frmLogin(string pCaption) : this(null, pCaption) { }
        public frmLogin(IWin32Window owner) : this(owner, string.Empty) { }

        public frmLogin(IWin32Window owner, string pCaption)
        {
            InitializeComponent();
            if (owner != null)
            {
                this.Owner = (Form)owner;
                this.Icon = ((Form)owner).Icon;
                this.Font = ((Form)owner).Font;
            }
            if (string.IsNullOrWhiteSpace(pCaption))
                this.Text = "Авторизация";
            else
                this.Text = pCaption;
        }
        /// <summary>
        /// Установка начальных значений компонентов
        /// </summary>
        public virtual void SetValueComponent()
        {
            if (m_logon == null)
                m_logon = new Logon(this);
            m_attempt = 1;
            edUser.Text = m_logon.User;
            edPassword.Text = m_logon.Password;
            if (edPassword.Text.Length > 0 && edPassword.Text.Length < 20)
                edPassword.Text = edPassword.Text.PadRight(20);
            rbWin.Checked = (m_logon.AuthenticationType == AuthenticationType.Windows || m_logon.AuthenticationType == AuthenticationType.OnlyWindows);
            rbSQL.Checked = (m_logon.AuthenticationType == AuthenticationType.SQLServer || m_logon.AuthenticationType == AuthenticationType.OnlySQLServer);
            gbTypeAut.Enabled = (m_logon.AuthenticationType == AuthenticationType.Windows || m_logon.AuthenticationType == AuthenticationType.SQLServer);
            gbTypeAut.Visible = (m_logon.AuthenticationType != AuthenticationType.OnlySQLServer);
            if (!gbTypeAut.Visible)
            {
                this.Size = new Size(this.Size.Width, this.Size.Height - 61); //61
            }
            if (rbWin.Checked)
            {
                edUser.Enabled = false;
                edPassword.Enabled = false;
            }
            edUser.TextChanged += this.Component_Changed;
            edPassword.TextChanged += this.Component_Changed;
            rbSQL.CheckedChanged += this.Component_Changed;
            rbWin.CheckedChanged += this.Component_Changed;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            ValidateUser();
        }
        /// <summary>
        /// Обработчик изменения значений компонентов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Component_Changed(object sender, EventArgs e)
        {
            ComponentChanged(sender);
        }

        public virtual void ComponentChanged(object sender)
        {
            if (sender == edUser)
                m_logon.User = edUser.Text;
            else if (sender == edPassword)
                m_logon.Password = edPassword.Text.Trim();
            else if (sender == rbWin)
            {
                if (rbWin.Checked)
                {
                    m_logon.AuthenticationType = AuthenticationType.Windows;
                    edUser.Enabled = false;
                    edPassword.Enabled = false;
                }
            }
            else if (sender == rbSQL)
            {
                if (rbSQL.Checked)
                {
                    m_logon.AuthenticationType = AuthenticationType.SQLServer;
                    edUser.Enabled = true;
                    edPassword.Enabled = true;
                }
            }        
        }

        /// <summary>
        /// Проверка соединения пользователя
        /// </summary>
        private void ValidateUser()
        {
            if (m_logon.ValidateConnect(true))
            {
                m_attempt = Logon.MAX_ATTEMPT + 1;
                DialogResult = DialogResult.OK;
                //m_logon.User = null;
                //m_logon.Password = null;
                //m_logon = null;
            }
            else
            {
                m_attempt++;
                if (m_attempt > Logon.MAX_ATTEMPT)
                {
                    DialogResult = DialogResult.Cancel;
                }
            }
        }


        private void frmLogin_Load(object sender, EventArgs e)
        {
            SetValueComponent();
        }

        //public override dis
    }
}
