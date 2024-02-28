using System;
using System.Configuration;
using System.Windows.Forms;
//using System.Collections.Specialized;

//using Sda.DB;
//using SdaWin.Visual;
//using System.Security.Principal;
using Common.DB_Classes;
using System.Data.SqlClient;

namespace VisualWinForm.Visual.Tools
{
	/// <summary>
    /// ���� � �������
	/// </summary>
	public class Logon
	{
		/// <summary>
        /// ������������ ���������� ������� ���������� � ����� ������.
		/// </summary>
		public const int MAX_ATTEMPT = 3;

		#region Class Members
        private AuthenticationType m_authenticationType;
        private string m_Server;
        private string m_DataBaseName;
		private string m_User;
		private string m_Password;
        private int m_TimeOut;
        IWin32Window m_owner;

		#endregion


        /// <summary>
        /// ������
        /// </summary>
        public string Server
        {
            get
            {
                return m_Server;
            }
            set
            {
                m_Server = value;
            }
        }

        /// <summary>
        /// ���� ������
        /// </summary>
        public string DataBaseName
        {
            get
            {
                return m_DataBaseName;
            }
            set
            {
                m_DataBaseName = value;
            }
        }
		/// <summary>
		/// ������������.
		/// </summary>
		public string User
		{
			get
			{
				return m_User;
			}
			set
			{
				m_User = value;
			}
		}

		/// <summary>
		/// ��� �����������
		/// </summary>
		public AuthenticationType AuthenticationType
		{
			get
			{
				return m_authenticationType;
			}
			set
			{
				m_authenticationType = value;
			}
		}

		/// <summary>
		/// ������
		/// </summary>
		public string Password
		{
			get
			{
				return m_Password;
			}
			set
			{
				m_Password = value;
			}
		}
        
		/// <summary>
        /// TimeOut
		/// </summary>
        public int TimeOut
		{
			get
			{
                return m_TimeOut;
			}
			set
			{
                m_TimeOut = value;
			}
		}

        /// <summary>
        /// ���������� Logon class.
        /// </summary>
        public Logon() : this(null) {}

		/// <summary>
		/// ���������� Logon class.
		/// </summary>
        public Logon(IWin32Window owner)
		{
            m_owner = owner;
            SetValues();
		}
		/// <summary>
		/// ������� ���������� � ��������.
		/// </summary>
		public void LogonUser()
		{
            SqlConnection coOld = DBConnector.Connection;
            string s = coOld.ConnectionString;
            DBConnector.RebuildConnectionString(m_Server, m_DataBaseName, m_Password, m_User, m_authenticationType, 30);
            SqlConnection co = DBConnector.Connection;
            try
            {
                if (string.IsNullOrWhiteSpace(co.ConnectionString))
                    throw new Exception("�� ���������������� ������ ����������." 
                        + " �������� �� ������ ��� ������� ��� ��� ���� ����������� 'SQL Server'"
                        + " �� ������ ���/������ ������������. ");
                co.Open();
                DBConnector.CurrentDBName = m_DataBaseName;
            }
            catch (Exception ex)
            {
                //m_Password = string.Empty;
                DBConnector.SetConnectionString(s);
                throw new Exception(ex.Message);
            }
            finally
            {
                coOld.Dispose();
                coOld = null;
                if (co.State == System.Data.ConnectionState.Open)
                    co.Close();
                co.Dispose();
                co = null;
            }
		}
        /// <summary>
        /// �������� ���������� ������������
        /// </summary>
        /// <param name="pShowMess">���������� ��������� �� ������ ����������</param>
        /// <returns></returns>
        public bool ValidateConnect(bool pShowMess)
        {
            try
            {
                LogonUser();
            }
            catch (Exception ex)
            {
                if (pShowMess)
                    ControlTools.ShowMessageExt(m_owner, string.Format("[E]�� ������� ���������� ����������.{0}C����� - '{1}'{2}���� ������ - '{3}'{4}{5}{6}{7}"
                        , Environment.NewLine, m_Server
                        , Environment.NewLine, m_DataBaseName
                        , Environment.NewLine, m_authenticationType == AuthenticationType.Windows || m_authenticationType == AuthenticationType.OnlyWindows ? "����������� Windows" : "������������ - '" + m_User + "'"
                        , Environment.NewLine, ex.Message)
                        , "���������� � ��", string.Empty, 0);
                return false;
            }
            return true;
        }

		/// <summary>
		/// Logoff user.
		/// </summary>
		public void LogoffUser()
		{
            /*
			SqlExecutor sqlexec = Global.CreateSqlExecutor(Global.Connection);
			try
			{
				sqlexec.ExecSQL("Log_off");
			}
			catch (Exception ex)
			{
				ControlTools.ShowErrorMessage(Global.ApplicationContext.MainForm, ex.Message);
			}
             */ 
		}

		/// <summary>
		/// Save user name in a registry.
		/// </summary>
		public void SaveUserName()
		{
			//Application.UserAppDataRegistry.SetValue("User", Global.UserName);
		}

        private void SetValues()
        {
            m_authenticationType = AuthenticationType.SQLServer;
            m_Server = string.Empty;
            m_DataBaseName = string.Empty;
            m_User = string.Empty;
            m_Password = string.Empty;
            m_TimeOut = 30;
            //������ ��������� �� ����� app.config, ������ appSettings
            m_Server = ConfigurationManager.AppSettings["SQLServer"];
            m_DataBaseName = ConfigurationManager.AppSettings["DataBase"];
            m_User = ConfigurationManager.AppSettings["User"];
            m_Password = ConfigurationManager.AppSettings["Password"];
            int.TryParse(ConfigurationManager.AppSettings["TimeOut"], out m_TimeOut);
            //������ ��������� �� ��������� ������
            string[] args = Environment.GetCommandLineArgs();
            string ls;
            foreach (string s in args)
            {
                ls = s.Substring(2).Trim();
                if (!string.IsNullOrEmpty(ls))
                {
                    if (s.StartsWith("-A", StringComparison.OrdinalIgnoreCase))
                        m_Server = ls.ToUpper();
                    else if (s.StartsWith("-U", StringComparison.OrdinalIgnoreCase))
                    {
                        int i = (ls.IndexOf("/"));
                        if (i > 0)
                        {
                            m_User = ls.Substring(0, i);
                            m_Password = ls.Substring(i + 1);
                        }
                        else if (i == 0)
                            m_Password = ls.Substring(i + 1);
                        else
                            m_User = ls;
                    }
                    else if (s.StartsWith("-I", StringComparison.OrdinalIgnoreCase))
                        m_DataBaseName = ls.ToUpper() + "_DATA";
                }
            }
            if (String.IsNullOrWhiteSpace(m_User))
                m_authenticationType = AuthenticationType.Windows;
        }
	}
}