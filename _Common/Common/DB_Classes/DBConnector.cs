using System.Data.SqlClient;

namespace Common.DB_Classes
{
    /// <summary>
    /// Тип авторизации
    /// </summary>
    public enum AuthenticationType
    {
        /// <summary>
        /// SQL Server
        /// </summary>
        SQLServer = 0,
        /// <summary>
        /// Windows
        /// </summary>
        Windows = 1,
        /// <summary>
        /// Только SQL Server
        /// </summary>
        OnlySQLServer = 3,
        /// <summary>
        /// Только Windows
        /// </summary>
        OnlyWindows = 4
    }
    /// <summary>
    /// Соединение с сервером
    /// </summary>
    public class DBConnector
    {
        private static string m_ConnectionString;
        private static int m_ConnectTimeout = 0;
        private static DBList m_ListDB;
        private static string m_CurrentDBName;

        /// <summary>
        /// Установить строку соединения.
        /// </summary>
        /// <param name="pConnectionString"></param>
        public static void SetConnectionString(string pConnectionString)
        {
            m_ConnectionString = pConnectionString;
        }

        /// <summary>
        /// Сформировать строку соединения.
        /// </summary>
        /// <param name="pServer">Сервер</param>
        /// <param name="pDataBaseName">База данных</param>
        /// <param name="pPassword">password</param>
        /// <param name="pUser">login</param>
        /// <param name="pType">Тип авторизации</param>
        /// <param name="pTimeOut">TimeOut</param>
        public static void RebuildConnectionString(string pServer, string pDataBaseName, string pPassword, string pUser, AuthenticationType pType, int pTimeOut)
        {

            if (string.IsNullOrEmpty(pServer) || ((pType == AuthenticationType.SQLServer || pType == AuthenticationType.OnlySQLServer) && (string.IsNullOrEmpty(pPassword) || string.IsNullOrEmpty(pUser))))
            {
                m_ConnectionString = string.Empty;
                return;
            }
            m_ConnectTimeout = pTimeOut;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            try
            {
                builder.DataSource = pServer;
                if (!string.IsNullOrWhiteSpace(pDataBaseName))
                {
                    builder.InitialCatalog = pDataBaseName;
                    m_CurrentDBName = pDataBaseName;
                }
                else
                    m_CurrentDBName = string.Empty;
                //builder.AsynchronousProcessing = true;
                builder.ConnectTimeout = m_ConnectTimeout;
                if (pType == AuthenticationType.SQLServer || pType == AuthenticationType.OnlySQLServer)
                {
                    builder.UserID = pUser;
                    builder.Password = pPassword;
                }
                else
                    builder.IntegratedSecurity = true;
                m_ConnectionString = builder.ConnectionString;
            }
            finally
            {
                builder.Clear();
                builder = null;
            }
        }

        /// <summary>
        /// Соединение с текущей ConnectionString.
        /// </summary>
        public static SqlConnection Connection
        {
            get
            {
                return new SqlConnection(m_ConnectionString);
            }
        }

        /// <summary>
        /// Новое соединение с возможно другой ConnectionString.
        /// </summary>
        public static SqlConnection SetConnection(string pConnectionString)
        {
            if (string.IsNullOrWhiteSpace(pConnectionString))
                pConnectionString = m_ConnectionString;
            return new SqlConnection(pConnectionString);
        }

        /// <summary>
        /// Connection Timeout.
        /// </summary>
        public static int ConnectTimeout
        {
            get
            {
                return m_ConnectTimeout;
            }
            set
            {
                if (value >= 0)
                {
                    m_ConnectionString = ChangeItemValueConnectionString("Connection Timeout", value);
                    m_ConnectTimeout = value;
                }
            }
        }

        /// <summary>
        /// Изменить значение элемента ConnectionString
        /// </summary>
        /// <param name="pItemName">Имя элемента</param>
        /// <param name="pItemValue">Новое значение элемента</param>
        /// <returns>Новая ConnectionString</returns>
        public static string ChangeItemValueConnectionString(string pItemName, object pItemValue)
        {
            string str = string.Empty;
            if (!string.IsNullOrWhiteSpace(m_ConnectionString))
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(m_ConnectionString);
                try
                {
                    builder[pItemName] = pItemValue;
                    str = builder.ConnectionString;
                }
                catch
                {
                    str = m_ConnectionString;
                }
                finally
                {
                    builder.Clear();
                    builder = null;
                }
            }
            return str;
        }


        #region Базы данных соединения
        /// <summary>
        /// Список баз данных использованных в текущем сеансе
        /// </summary>
        public static DBList ListDB
        {
            get
            {
                if (m_ListDB == null)
                    m_ListDB = new DBList();
                return m_ListDB;
            }
        }

        /// <summary>
        /// Текущая БД
        /// </summary>
        public static DB_Data CurrentDB
        {
            get
            {
                return ListDB.CurrentDB;
            }
        }

        /// <summary>
        /// Имя текущей БД. Если БД еще не было в списке, добавляет ее. 
        /// Если БД есть на сервере и активна, изменяет 'Initial Catalog' в строке соединения.
        /// </summary>
        public static string CurrentDBName
        {
            get
            {
                return m_CurrentDBName;
            }
            set
            {
                string s = string.Empty;
                if (!string.IsNullOrWhiteSpace(value))
                    s = value;
                ListDB.CurrentDBName = s;
                m_CurrentDBName = s;
                if (ListDB.CurrentDBIsActive || s == string.Empty)
                    m_ConnectionString = ChangeItemValueConnectionString("Initial Catalog", (s == string.Empty ? null : s));
            }
        }
        #endregion

    }
}
