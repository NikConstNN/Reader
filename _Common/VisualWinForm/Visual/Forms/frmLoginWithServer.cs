using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Common.DB_Classes;

namespace VisualWinForm.Visual.Forms
{
    public enum FormLoginShowType
    {
        /// <summary>
        /// Показывать только выбор сервера
        /// </summary>
        OnlyServer = 0,
        /// <summary>
        /// Показывать выбор сервера и выбор базы данных
        /// </summary>
        ServerAndDB = 1
    }

    public partial class frmLoginWithServer : VisualWinForm.Visual.Forms.frmLogin
    {


        public delegate void ChangeServer(string Server, Control edText);
        public ChangeServer LoginChangeServer;

        public frmLoginWithServer() : this(null, string.Empty, FormLoginShowType.OnlyServer) { }
        public frmLoginWithServer(string pCaption) : this(null, pCaption, FormLoginShowType.OnlyServer) { }
        public frmLoginWithServer(IWin32Window owner) : this(owner, string.Empty, FormLoginShowType.OnlyServer) { }
        public frmLoginWithServer(FormLoginShowType pShowType) : this(null, string.Empty, pShowType) { }
        public frmLoginWithServer(string pCaption, FormLoginShowType pShowType) : this(null, pCaption, pShowType) { }
        public frmLoginWithServer(IWin32Window owner, FormLoginShowType pShowType) : this(owner, string.Empty, pShowType) { }

        private bool m_ShowDB;

        public frmLoginWithServer(IWin32Window owner, string pCaption, FormLoginShowType pShowType)
        {
            InitializeComponent();
            m_ShowDB = pShowType == FormLoginShowType.ServerAndDB;
            if (!m_ShowDB)
            {
                this.pnDB.Visible = false;
                this.Size = new Size(this.Size.Width, this.Size.Height - 27);
            }
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
            LoginChangeServer = null;
        }
        public override void SetValueComponent()
        {
            base.SetValueComponent();
            cbServerAddItems(m_logon.Server);
            FillCB_DB(null);
        }

        private void FillCB_DB(List<string> aList)
        {
            if (m_ShowDB)
            {
                cbDB.SelectedIndexChanged -= cbDBSelectedIndexChanged;
                cbDB.TextChanged -= Component_Changed;
                cbDB.Items.Clear();
                if (aList != null)
                {
                    for (int i = 0; i < aList.Count; i++)
                        cbDB.Items.Add(aList[i]);
                }
                else if (!string.IsNullOrWhiteSpace(m_logon.DataBaseName))
                {
                    cbDB.Items.Add(m_logon.DataBaseName);
                }
                cbDB.Items.Add("Базы данных сервера...");
                if (cbDB.Items.Count > 1)
                    cbDB.SelectedIndex = cbDB.FindString(m_logon.DataBaseName);
                else
                    cbDB.SelectedIndex = -1;
                cbDB.SelectedIndexChanged += cbDBSelectedIndexChanged;
                cbDB.TextChanged += Component_Changed;
                lbDB.Focus();
            }                           
        }

        public void cbServerAddItems(string pItem)
        {
            List<string> list = new List<string> { pItem };
            cbServerAddItems(list);
        }

        public void cbServerAddItems(List<string> pList)
        {
            cbServer.SelectedIndexChanged -= cbServer_SelectedIndexChanged;
            cbServer.TextChanged -= Component_Changed;
            for (int i = 0; i < cbServer.Items.Count; i++)
			{
                string item = cbServer.Items[i].ToString();
                if (!item.Equals("Поиск серверов..."))
                {
                    pList.Add(item);
                }                
			}
            pList.Add("Поиск серверов...");
            cbServer.Items.Clear();
            cbServer.Items.AddRange(pList.ToArray());
            if (!string.IsNullOrWhiteSpace(m_logon.Server) && cbServer.FindString(m_logon.Server) < 0)
            {
                cbServer.Items.Insert(0, m_logon.Server);
            }
            if (cbServer.Items.Count > 1)
                cbServer.SelectedIndex = cbServer.FindString(m_logon.Server);
            else
                cbServer.SelectedIndex = -1;
            cbServer.SelectedIndexChanged += cbServer_SelectedIndexChanged;
            cbServer.TextChanged += Component_Changed;
            lbServer.Focus();
        }
        /*
        private void FillCBServer()
        {
            cbServer.SelectedIndexChanged -= cbServer_SelectedIndexChanged;
            cbServer.TextChanged -= Component_Changed;
            if (cbServer.Items.Count == 0)
                cbServer.Items.Add("Поиск серверов..."); 
            if (!string.IsNullOrWhiteSpace(m_logon.Server) && cbServer.FindString(m_logon.Server) < 0)
            {
                cbServer.Items.Insert(0, m_logon.Server);
            }
            if (cbServer.Items.Count > 1)
                cbServer.SelectedIndex = cbServer.FindString(m_logon.Server);
            else
                cbServer.SelectedIndex = -1;
            cbServer.SelectedIndexChanged += cbServer_SelectedIndexChanged;
            cbServer.TextChanged += Component_Changed;
            lbServer.Focus();
        }
        */
        public override void ComponentChanged(object sender)
        {
            base.ComponentChanged(sender);
            if (sender == cbServer)
            {
                if (cbServer.SelectedIndex + 1 != cbServer.Items.Count)
                {
                    m_logon.Server = cbServer.Text;
                    FillCB_DB(null);
                    if (LoginChangeServer != null)
                        LoginChangeServer(m_logon.Server, edPassword);
                    //lbServer.Focus();
                }
            }
            if (sender == cbDB)
            {
                if (cbDB.SelectedIndex + 1 != cbDB.Items.Count)
                    m_logon.DataBaseName = cbDB.Text.Trim();
            }
        }

        private void cbServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbServer.SelectedIndex + 1 != cbServer.Items.Count)
                return;
            this.Enabled = false;
            try
            {
                string sss = frmListSevers.Execute(this.pnServer, m_logon.Server);
                if (!string.IsNullOrWhiteSpace(sss) && sss != m_logon.Server)
                {
                    m_logon.Server = sss;
                    FillCB_DB(null);
                    cbServerAddItems(sss);
                    if (LoginChangeServer != null)
                        LoginChangeServer(m_logon.Server, edPassword);
                }
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void cbDBSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDB.SelectedIndex + 1 != cbDB.Items.Count)
                return;
            List<string> aDataList = null;
            if (!string.IsNullOrWhiteSpace(m_logon.Server)
                && ((!string.IsNullOrWhiteSpace(m_logon.User) && !string.IsNullOrWhiteSpace(m_logon.Password)) || m_logon.AuthenticationType == AuthenticationType.Windows))
            {
                string currDB = m_logon.DataBaseName;
                m_logon.DataBaseName = string.Empty;
                this.Enabled = false;
                try
                {
                    if (m_logon.ValidateConnect(true))
                    {
                        aDataList = new List<string>();
                        DB_Services.GetServerDB(aDataList, true, true);

                    }                                     
                }
                finally
                {
                    m_logon.DataBaseName = currDB; 
                    this.Enabled = true;
                }                
            }
            FillCB_DB(aDataList);
        }

    }
}
