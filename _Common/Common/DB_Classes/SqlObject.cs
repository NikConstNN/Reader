using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

//using DB_Classes.Data.ExtType;
//using Sda.Common;
//using Sda.Tools;

namespace Common.DB_Classes
{
	/// <summary>
	/// Выполнение запроса или sp SQL Server database.
	/// </summary>
	public class SqlObject: IDisposable
	{

		/// <summary>
		/// Формат данных типа дата/время
		/// </summary>
		public static string SQLDateTimeFormat = "yyyyMMdd HH:mm:ss.fff";

		/// <summary>
        /// Формат бинарных данных
		/// </summary>
		public static string SQLBinaryFormat = "x2";


		#region Class members

		private SqlConnection m_connection;
        private List<SqlParameter> m_parameters;
        //DbParameterCollection m_parameters;
		private string m_connectionString;
		private bool m_isDisposed;
        private string m_QueryText;
        private int m_ConnectTimeout;

		#endregion

        /// <summary>
        /// Конструктор SqlObject class.
        /// </summary>
        public SqlObject() : this(DBConnector.Connection) { }

		/// <summary>
		/// Конструктор SqlObject class.
		/// </summary>
        /// <param name="aConnection">SqlConnection для подключение к экземпляру SQL</param>
		public SqlObject(SqlConnection aConnection)
		{
			m_connectionString = null;
			m_connection = aConnection;
			m_isDisposed = false;
            m_QueryText = string.Empty;
            m_ConnectTimeout = -1;
		}

		/// <summary>
        /// Конструктор SqlObject class.
		/// </summary>
        /// <param name="aConnectionString">Строка, используемая для открытия источника данных</param>
		public SqlObject(string aConnectionString): this((SqlConnection)null) //DBConnector.SetConnection(aConnectionString))
		{
			m_connectionString = aConnectionString;
		}

        /// <summary>
        /// Текст запроса
        /// </summary>
        public string QueryText
        {
            get
            {
                return m_QueryText;
            }
            set
            {
                m_QueryText = value;
            }
        }
        /// <summary>
        /// Connection Timeout.
        /// </summary>
        public int ConnectTimeout
        {
            get
            {
                return m_ConnectTimeout;
            }
            set
            {
                m_ConnectTimeout = value;
            }
        }

		/// <summary>
        /// SqlConnection, используемый этим экземпляром SQLObject
		/// </summary>
		protected SqlConnection Connection
		{
			get
			{
				if (m_connection == null && isDefineConnectionString)
				{
                    m_connection = DBConnector.SetConnection(m_connectionString);
				}
				return m_connection;
			}
		}

		/// <summary>
        /// Используется ли ConnectionString
		/// </summary>
		protected bool isDefineConnectionString
		{
			get
			{
				return m_connectionString != null;
			}
		}

		/// <summary>
        /// ConnectionString
		/// </summary>
		protected string ConnectionString
		{
			get
			{
				return m_connectionString == null ? string.Empty : m_connectionString;
			}
		}

        #region Параметры запроса
        /// <summary>
        /// Параметры запроса
        /// </summary>
        protected List<SqlParameter> Parameters
        //protected DbParameterCollection Parameters            
        {
            get
            {
                if (m_parameters == null)
                {
                    m_parameters = new List<SqlParameter>();
                    //m_parameters = new SqlParameterCollection();
                }
                return m_parameters;
            }
        }

        public virtual void AddParameter(string aName, object aValue, Type aDataType)
        {
            string _name = ParamName(aName, false);
            if (!_name.Equals(string.Empty))
            {
                DeleteParameter(aName);

                DbType tp;
                if (aDataType == null)
                    tp = DbType.String;
                else if (aDataType == typeof(DateTime))
                {
                    tp = DbType.DateTime;
                }
                else if (aDataType == typeof(int))
                {
                    tp = DbType.Int32;
                }
                else if (aDataType == typeof(Int16))
                {
                    tp = DbType.Int16;
                }
                else if (aDataType == typeof(Byte))
                {
                    tp = DbType.Byte;
                }
                else if (aDataType == typeof(Int64) || aDataType == typeof(long))
                {
                    tp = DbType.Int64;
                }
                else if (aDataType == typeof(decimal))
                {
                    tp = DbType.Decimal;
                }
                else if (aDataType == typeof(double) || aDataType == typeof(float))
                {
                    tp = DbType.Double;
                }
                else if (aDataType == typeof(byte[]))
                {
                    tp = DbType.Binary;
                }
                else if (aDataType == typeof(string))
                //else if (aValue is string)
                {
                    tp = DbType.String;
                }
                else
                    tp = DbType.Object;
                //tp = SqlDbType.VarChar;
                //tp = SqlDbType.Variant;
                SqlParameter par = new SqlParameter();
                par.ParameterName = _name;
                //string val = GetParamObjectAsString(aValue);
                //if (val != "null")
                //  par.Value = val;
                par.DbType = tp;
                par.Value = aValue;
                /*
                if (aValue == null || aValue == DBNull.Value)
                {
                    par.Value = null;
                    par.SqlValue = DBNull.Value;
                    //par.DbType = DbType.Object;
                }
                else
                {
                    par.Value = aValue;
                   // par.SqlDbType = tp;
                }
                 */ 
                //par.DbType .UdtTypeName
                //Parameters.Add(new SqlParameter(_name, Common.ExtType.GetObjectValueAsString(aValue, null)));
                Parameters.Add(par);
                //Parameters[aName].Value = aValue; //  .Add(par);
            }       
        }
        /// <summary>
        /// Добавить параметр запроса. Если уже есть такой параметр, он удаляется и заменяется новым
        /// </summary>
        /// <param name="aName">Имя параметра</param>
        /// <param name="aValue">Значение параметра</param>
        public virtual void AddParameter(string aName, object aValue)
        {
            if (aValue != null && aValue != DBNull.Value)
                AddParameter(aName, aValue, aValue.GetType());
            else
                AddParameter(aName, aValue, null);
        }
        /// <summary>
        /// Изменить значение параметра запроса
        /// </summary>
        /// <param name="ParameterName">Имя параметра</param>
        /// <param name="ParameterValue">Значение параметра</param>
        public virtual void ChangeParameterValue(string ParameterName, object ParameterValue)
        {
            AddParameter(ParameterName, ParameterValue);
        }
        /// <summary>
        /// Удалить параметр запроса
        /// </summary>
        /// <param name="ParameterName">Имя параметра</param>
        public virtual void DeleteParameter(string ParameterName)
        {
            string _name = ParamName(ParameterName, true);
            if (!_name.Equals(string.Empty))
            {
                SqlParameter parList = null;
                foreach (SqlParameter par in Parameters)
                    if (par.ParameterName.ToUpper() == _name)
                    {
                        parList = par;
                        break;
                    }
                if (parList != null)
                {
                    Parameters.Remove(parList);
                    parList = null;
                }
            }
        }
        /// <summary>
        /// Удалить все параметры запроса
        /// </summary>
        public virtual void ClearParameter()
        {
            Parameters.Clear();
        }

        private string ParamName(string OrigName, bool ToUpper)
        {
            if (string.IsNullOrWhiteSpace(OrigName))
                return string.Empty;
            string res = OrigName.Trim();
            if (ToUpper)
                res = res.ToUpper();
            if (res[0] != '@')
                res = "@" + res;
            return res;
        }
        #endregion

        /// <summary>
		/// Разбор запроса.
		/// </summary>
		/// <param name="aSqlText">Текст запроса</param>
		/// <param name="aParams">Array parameters for the statement</param>
		/// <returns></returns>
		protected virtual string PrepareSqlText(string aSqlText, object[] aParams)
		{
			//string res = aSqlText.Replace(':','@');
            string res = aSqlText;
			if (aParams != null)
			{
				if (aSqlText.IndexOf("{") > 0)
				{
					// used string.Format function
					object[] tmpParams = new object[aParams.Length];
					for (int i = 0; i < aParams.Length; i++)
					{
						//tmpParams[i] = DB_Classes.Data.ExtType. .GetParamObjectAsString(aParams[i]);
					}

					res = string.Format(aSqlText, tmpParams);
				}
				else
				{
					res += " ";
					for (int i = 0; i < aParams.Length; i++)
					{
                        string val = ""; //GetParamObjectAsString(aParams[i]);
						res += val + ",";
					}

					if (res[res.Length - 1] == ',')
					{
						res = res.Substring(0, res.Length - 1);
					}
				}
			}
			return res;
		}
		/// <summary>
		/// Gets the parameter value as string.
		/// </summary>
		/// <param name="aValue">A parameter</param>
		/// <returns>Parameter value as string</returns>
		protected virtual string GetParamObjectAsString(object aValue)
		{
			string format = null;
			if (aValue is DateTime)
			{
				format = SQLDateTimeFormat;
			}
			else
				if (aValue is byte[])
				{
					format = SQLBinaryFormat;
				}
			string strVal = Common.ExtType.GetObjectValueAsString(aValue, "null", format);
			//if ((aValue is string || aValue is DateTime) && strVal != "null")
			//{
			//	strVal = StringTools.QuotedStr(strVal);
			//}
			return strVal;
		}
		/// <summary>
		/// Закрыть SqlObject.
		/// </summary>
		public virtual void Close()
		{
            m_connection.Close();
			if (isDefineConnectionString)
			{
				m_connection.Dispose();
				m_connection = null;
			}
            m_isDisposed = true;
		}

        /// <summary>
        /// Дата/время sql server.
        /// </summary>
        /// <param name="aConnection">Соединение</param>
        /// <returns>Текущие дата/время</returns>
        public static DateTime GetDateTimeFromServer(SqlConnection aConnection)
        {
            return GetDateTimeFromServer(aConnection, false);
        }

		/// <summary>
		/// Дата/время sql server.
		/// </summary>
		/// <param name="aConnection">Соединение</param>
        /// <param name="aOnlyDate">Дата без времени</param>
        /// <returns>Текущие дата/время или дата без времени</returns>
		public static DateTime GetDateTimeFromServer(SqlConnection aConnection, bool aOnlyDate)
		{
			SqlCommand command = new SqlCommand("select getdate() as CurrDate", aConnection);
			if (aConnection.State == ConnectionState.Closed)
			{
				aConnection.Open();
			}
            DateTime dt = (DateTime)command.ExecuteScalar();
            if (aOnlyDate)
                dt = dt.Date;
			return dt;
		}

		#region Init Params
/*
		/// <summary>
		/// Gets null if value equal DATE_NULL 
		/// </summary>
		/// <param name="aValue">A DateTime value</param>
		/// <returns>New parameter value</returns>
		public static object DateAsParam(DateTime aValue)
		{
			if (aValue == ExtType.DATE_NULL)
			{
				return null;
			}
			else
			{
				return new DateTime(aValue.Year, aValue.Month, aValue.Day);
			}
		}

		/// <summary>
		/// Gets parameter value without time
		/// </summary>
		/// <param name="aValue">A DateTime value</param>
		/// <returns>New parameter value</returns>
		public static object DateTimeAsParam(DateTime aValue)
		{
			if (aValue == ExtType.DATE_NULL)
			{
				return null;
			}
			else
			{
				return aValue;
			}
		}

		/// <summary>
		/// Extract time only from DataTime value
		/// </summary>
		/// <param name="aValue">A DateTime value</param>
		/// <returns>New parameter value</returns>
		public static object TimeAsParam(DateTime aValue)
		{
			if (aValue == ExtType.DATE_NULL)
			{
				return null;
			}
			else
			{
				return DateTimeTools.TimeWithoutDate(aValue);
			}
		}

		/// <summary>
		/// Gets null if parameter value less zero
		/// </summary>
		/// <param name="aValue">A int parameter value</param>
		/// <returns>New parameter value</returns>
		public static object IDAsParam(int aValue)
		{
			if (aValue <= 0)
			{
				return null;
			}
			else
			{
				return aValue;
			}
		}
 */ 
		#endregion

		#region IDisposable Members

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		public virtual void Dispose()
		{
			if (!m_isDisposed)
			{
				Close();
				m_isDisposed = true;
			}
            if (!isDefineConnectionString)
            {
                m_connection.Dispose();
                m_connection = null;
            }
			//System.GC.SuppressFinalize(this);			
		}

		#endregion
	}
}
