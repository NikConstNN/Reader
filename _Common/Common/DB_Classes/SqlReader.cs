using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Common;


//using Sda.Common;

namespace Common.DB_Classes
{
	/// <summary>
    /// Предоставляет средство чтения однонаправленного потока строк из базы данных SQL Server.
	/// </summary>
	public class SqlReader : SqlObject, IDataReader, IDataRecord
	{
		#region Class Members

		private SqlDataReader m_dataReader;
		private SqlCommand m_command;
		private bool m_isOpened;
		private int m_currentRowIndex;

		#endregion

		/// <summary>
        /// Конструктор SqlReader class.
		/// </summary>
        /// <param name="aConnection">SqlConnection, представляющий соединение с экземпляром SQL</param>
		public SqlReader(SqlConnection aConnection) : base(aConnection) { }

		/// <summary>
        /// Конструктор SqlReader class.
		/// </summary>
		/// <param name="aConnectionString">Строка соединения с источником данных</param>
		public SqlReader(string aConnectionString) : base(aConnectionString) { }

        /// <summary>
        /// Конструктор SqlReader class.
        /// </summary>
        public SqlReader() : base() { }

        /// <summary>
        /// Извлечь первый столбец первой строки результирующего набора, возвращаемого запросом
        /// </summary>
        /// <returns>Значение первого столбца первой строки</returns>
        public virtual object ExecuteScalar()
        {
            if (string.IsNullOrWhiteSpace(this.QueryText))
                return null;
            if (m_isOpened)
                this.Close();
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
            m_command = new SqlCommand(this.QueryText, Connection);
            m_command.CommandTimeout = this.ConnectTimeout >= 0 ? this.ConnectTimeout : Connection.ConnectionTimeout;
            foreach (SqlParameter par in Parameters)
            {
                m_command.Parameters.Add(par);
            }
            m_isOpened = true;
            return m_command.ExecuteScalar();
        }

		/// <summary>
		/// Открыть запрос или SP
		/// </summary>
		/// <param name="aSqlText">Текст запроса.</param>
		/// <param name="aParams">Параметры запроса.</param>
		/// <param name="aTransaction">Транзакция, в которой выполняется запрос.</param>
		public virtual void OpenSQL(string aSqlText, object[] aParams, SqlTransaction aTransaction)
		{
            if (string.IsNullOrWhiteSpace(aSqlText) && string.IsNullOrWhiteSpace(this.QueryText))
                return;
            string sql;
            if (string.IsNullOrWhiteSpace(aSqlText))
            {
                sql = PrepareSqlText(this.QueryText, aParams);
            }
            else
            {
                this.QueryText = aSqlText;
                sql = PrepareSqlText(aSqlText, aParams);                            
            }
            if (m_isOpened)
                this.Close();
			if (Connection.State == ConnectionState.Closed)
			{
				Connection.Open();
			}

            m_command = new SqlCommand(sql, Connection, aTransaction);
            m_command.CommandTimeout = this.ConnectTimeout >= 0 ? this.ConnectTimeout : Connection.ConnectionTimeout;
            foreach (SqlParameter par in Parameters)
            {
                m_command.Parameters.Add(par);
            }
			m_dataReader = m_command.ExecuteReader();
			m_isOpened = true;
		}

		/// <summary>
        /// Открыть запрос или SP
		/// </summary>
        /// <param name="aSqlText">Текст запроса.</param>
        /// <param name="aParams">Параметры запроса.</param>
		public void OpenSQL(string aSqlText, params object[] aParams)
		{
			this.OpenSQL(aSqlText, aParams, null);
		}

		/// <summary>
        ///  Открыть запрос или SP
		/// </summary>
        /// <param name="aSqlText">Текст запроса</param>
		public void OpenSQL(string aSqlText)
		{
			this.OpenSQL(aSqlText, null);
		}

        /// <summary>
        ///  Открыть запрос или SP. Текст запроса берется из с-ва QueryText объекта
        /// </summary>
        public void OpenSQL()
        {
            this.OpenSQL(string.Empty);
        }

		/// <summary>
		/// Поле is null
		/// </summary>
		/// <param name="aFieldName">Имя поля</param>
        /// <returns>true если указанное значение столбца эквивалентно DBNull, в противном случае false.</returns>
		public bool IsDBNull(string aFieldName)
		{
			return Reader.IsDBNull(Reader.GetOrdinal(aFieldName));
		}

		/// <summary>
		/// Номер текущей записи выборки
		/// </summary>
		public int CurrentRowIndex
		{
			get
			{
				return m_currentRowIndex;
			}
		}

		/// <summary>
        /// Установить номер текущей записи выборки
		/// </summary>
		/// <param name="aRowIndex">Текущий номер</param>
		protected virtual void SetCurrentRowIndex(int aRowIndex)
		{
			m_currentRowIndex = aRowIndex;
		}

		/// <summary>
		/// Выбока не открыта
		/// </summary>
		public bool IsClosed
		{
			get
			{
				return !m_isOpened;
			}
		}

		/// <summary>
        /// Заполнить результатами выборки ArrayList
		/// </summary>
        /// <param name="aDataList">ArrayList для заполнения</param>
        /// <returns>Кол-во записей добавленых в ArrayList.</returns>
		public int Fill(ArrayList aDataList)
		{
			int i = 0;			
			while (Reader.Read())
			{
                object[] rowValue = new object[Reader.FieldCount];
				Reader.GetValues(rowValue);
				aDataList.Add(rowValue);
				i++;
			}
			return i;
		}

		/// <summary>
        /// Заполнить результатами выборки DataTable.
		/// </summary>
        /// <param name="aTable">DataTable для заполнения</param>
        /// <returns>Кол-во записей добавленых в DataTable.</returns>
		public int Fill(DataTable aTable)
		{
			int fieldCount = Reader.FieldCount;
			for (int i = 0; i < fieldCount; i++)
			{
				aTable.Columns.Add(Reader.GetName(i), Reader.GetFieldType(i));
			}
			int k = 0;
			while (Reader.Read())
			{
				DataRow row = aTable.NewRow();
				for (int i = 0; i < fieldCount; i++)
				{
					row[i] = Reader.GetValue(i);
				}
				aTable.Rows.Add(row);
				k++;
			}
			return k;
		}

		/// <summary>
		/// Есть ли записи в выборке
		/// </summary>
		public bool HasRows
		{
			get
			{
				return Reader.HasRows;
			}
		}

		#region Значения полей выборки

		/// <summary>
		/// Значение поля as string.
		/// </summary>
		/// <param name="aFieldName">Имя поля</param>
		/// <returns>Значение</returns>
		public string ValueAsString(string aFieldName)
		{
			int fid = Reader.GetOrdinal(aFieldName);
			if (Reader.IsDBNull(fid))
			{
				return null;
			}
			else
			{
				string value = Reader.GetValue(fid).ToString();
				//if (Settings.MakeRightTrimInDBReader)
				//{
				//	value = value.TrimEnd(new char[]{' '});
				//}
				return value;
			}
		}

		/// <summary>
        /// Значение поля as int.
		/// </summary>
        /// <param name="aFieldName">Имя поля</param>
        /// <returns>Значение</returns>
		public int ValueAsInteger(string aFieldName)
		{
			int fid = Reader.GetOrdinal(aFieldName);
			if (Reader.IsDBNull(fid))
			{
				return ExtType.INT_NULL;
			}
			else
			{
				if (Reader.GetFieldType(fid) == typeof(string))
				{
					return Convert.ToInt32(Reader.GetString(fid));
				}
				else if (Reader.GetFieldType(fid) == typeof(int))
				{
					return Reader.GetInt32(fid);
				}
				else if (Reader.GetFieldType(fid) == typeof(byte))
				{
					return Reader.GetByte(fid);
				}
				else if (Reader.GetFieldType(fid) == typeof(short))
				{
					return Reader.GetInt16(fid);
				}
				else if (Reader.GetFieldType(fid) == typeof(bool))
				{
					return (Reader.GetBoolean(fid) ? 1 : 0);
				}
				else
				{
					return (int)Reader.GetValue(fid);
				}
			}
		}

		/// <summary>
        /// Значение поля as Int64.
		/// </summary>
        /// <param name="aFieldName">Имя поля.</param>
        /// <returns>Значение</returns>
		public Int64 ValueAsInt64(string aFieldName)
		{
			int fid = Reader.GetOrdinal(aFieldName);
			if (Reader.IsDBNull(fid))
			{
				return ExtType.INT64_NULL;
			}
			else
			{
				if (Reader.GetFieldType(fid) == typeof(string))
				{
					return Convert.ToInt64(Reader.GetString(fid));
				}
				else if (Reader.GetFieldType(fid) == typeof(int))
				{
					return Reader.GetInt32(fid);
				}
				else if (Reader.GetFieldType(fid) == typeof(Int64))
				{
					return Reader.GetInt64(fid);
				}
				else if (Reader.GetFieldType(fid) == typeof(byte))
				{
					return Reader.GetByte(fid);
				}
				else if (Reader.GetFieldType(fid) == typeof(short))
				{
					return Reader.GetInt16(fid);
				}
				else if (Reader.GetFieldType(fid) == typeof(bool))
				{
					return (Reader.GetBoolean(fid) ? 1 : 0);
				}
				else
				{
					return (Int64)Reader.GetValue(fid);
				}
			}
		}

		/// <summary>
        /// Значение поля as DataTime.
		/// </summary>
        /// <param name="aFieldName">Имя поля</param>
        /// <returns>Значение</returns>
		public DateTime ValueAsDateTime(string aFieldName)
		{
			int fid = Reader.GetOrdinal(aFieldName);
			if (Reader.IsDBNull(fid))
			{
				return ExtType.DATE_NULL;
			}
			else
			{
				Type type = Reader.GetFieldType(fid);
				if (type == typeof(string))
				{
					return Convert.ToDateTime(Reader.GetString(fid));
				}
				return Reader.GetDateTime(fid);
			}
		}

		/// <summary>
        /// Значение поля as bool.
		/// </summary>
        /// <param name="aFieldName">Имя поля</param>
        /// <returns>Значение</returns>
		public bool ValueAsBoolean(string aFieldName)
		{
			int fid = Reader.GetOrdinal(aFieldName);
			if (Reader.IsDBNull(fid))
			{
				return false;
			}
			else
			{
				if (Reader.GetFieldType(fid) == typeof(bool))
				{
					return Reader.GetBoolean(fid);
				}
				else if (Reader.GetFieldType(fid) == typeof(string))
				{
					string val = Reader.GetString(fid);
					return ExtType.StringToBool(val);
				}
				else if (Reader.GetFieldType(fid) == typeof(int))
				{
					return Reader.GetInt32(fid) > 0;
				}
				else if (Reader.GetFieldType(fid) == typeof(byte))
				{
					return Reader.GetByte(fid) > 0;
				}
				else if (Reader.GetFieldType(fid) == typeof(short))
				{
					return Reader.GetInt16(fid) > 0;
				}
				else
				{
					return (bool)Reader.GetValue(fid);
				}
			}
		}

		/// <summary>
        /// Поток байтов из поля в byte массив.
		/// </summary>
        /// <param name="aFieldName">Имя поля</param>
        /// <returns>Значение</returns>
		public byte[] ValueAsBinary(string aFieldName)
		{
			int fid = Reader.GetOrdinal(aFieldName);
			if (Reader.IsDBNull(fid))
			{
				return null;
			}
			else
			{
				return Reader.GetSqlBinary(fid).Value;
			}
		}

		/// <summary>
        /// Значение поля as decimal.
		/// </summary>
        /// <param name="aFieldName">Имя поля</param>
        /// <returns>Значение</returns>
		public decimal ValueAsDecimal(string aFieldName)
		{
			int fid = Reader.GetOrdinal(aFieldName);
			if (Reader.IsDBNull(fid))
			{
				return ExtType.DECIMAL_NULL;
			}
			else
			{
				Type type = Reader.GetFieldType(fid);
				if (type == typeof(float))
				{
					if(Reader.IsDBNull(fid))
					{
						return ExtType.DECIMAL_NULL;
					}
					else
					{
						return Convert.ToDecimal(Reader.GetFloat(fid));
					}
				}
				else if (type == typeof(int))
				{
					return Reader.GetInt32(fid);
				}
				else if (type == typeof(string))
				{
					return Convert.ToDecimal(Reader.GetString(fid));
				}
				else if (type == typeof(double))
				{
					return Convert.ToDecimal(Reader.GetDouble(fid));
				}
				return Reader.GetDecimal(fid);
			}
		}

		/// <summary>
        /// Значение поля as double.
		/// </summary>
        /// <param name="aFieldName">Имя поля</param>
        /// <returns>Значение</returns>
		public double ValueAsDouble(string aFieldName)
		{
			int fid = Reader.GetOrdinal(aFieldName);
			if (Reader.IsDBNull(fid))
			{
				return ExtType.DOUBLE_NULL;
			}
			else
			{
				Type type = Reader.GetFieldType(fid);
				if (type == typeof(float))
				{
					return Convert.ToDouble(Reader.GetFloat(fid));
				}
				else if (type == typeof(int))
				{
					return Reader.GetInt32(fid);
				}
				else if (type == typeof(decimal))
				{
					return Convert.ToDouble(Reader.GetDecimal(fid));
				}
				else if (type == typeof(string))
				{
					return Convert.ToDouble(Reader.GetString(fid));
				}
				return Reader.GetDouble(fid);
			}
		}

		#endregion

		/// <summary>
		/// SqlDataReader экземпляра.
		/// </summary>
		protected SqlDataReader Reader
		{
			get
			{
				if (m_dataReader == null)
				{
					throw new Exception("Data reader not opened");
				}
				return m_dataReader;
			}
		}

		#region IDataReader Members

		/// <summary>
		/// Gets a value indicating the depth of nesting for the current row.
		/// </summary>
		public int Depth
		{
			get
			{
				return Reader.Depth;
			}
		}

		/// <summary>
        /// Возвращает объект DataTable, описывающий метаданные столбца из IDataReader.
		/// </summary>
		/// <returns></returns>
		public DataTable GetSchemaTable()
		{
			return Reader.GetSchemaTable();
		}

		/// <summary>
        /// Количество строк были изменены, вставлены или удалены при выполнении SQL запроса.
		/// </summary>
		public int RecordsAffected
		{
			get
			{
				return Reader.RecordsAffected;
			}
		}

		/// <summary>
		/// Переход к следующей записи.
		/// </summary>
		/// <returns>true если следующая запись есть; иначе, false.</returns>
		public virtual bool Read()
		{
			bool res = Reader.Read();
			if (res)
			{
				m_currentRowIndex++;
			}
			return res;
		}

		/// <summary>
		/// Закрыть DataReader
		/// </summary>
		public override void Close()
		{
			if (!IsClosed)
			{
                if (m_dataReader != null)
                {
                    m_dataReader.Close();
                    m_dataReader.Dispose();
                    m_dataReader = null;
                }
                m_command.Parameters.Clear();
                m_command.Dispose();
                m_command = null;

				base.Close();

				m_isOpened = false;
			}
		}

		/// <summary>
        /// Переход чтения данных к следующему результату, если запрос возвращает несколько выборок.
		/// </summary>
		/// <returns>true если в следуещей выбоке есть записи; иначе, false.</returns>
		public bool NextResult()
		{
			m_currentRowIndex = 0;
			return Reader.NextResult();
		}

		#endregion

		#region IDataRecord Members

		int IDataRecord.GetInt32(int i)
		{
			return Reader.GetInt32(i);
		}

		/// <summary>
        /// Получает поле с указанным именем. 
		/// </summary>
        /// <param name="name">Имя столбца.</param>
        /// <returns>поле с указанным именем as Object.</returns>
		public object this[string name]
		{
			get
			{
				return Reader[name];
			}
		}

		/// <summary>
        /// Получает поле с указанным индексом. 
		/// </summary>
		/// <param name="i">Индекс столбца. </param>
        /// <returns>поле с указанным индексом as Object.</returns>
		object IDataRecord.this[int i]
		{
			get
			{
				return Reader[i];
			}
		}

		/// <summary>
		/// Значение поля по индексу.
		/// </summary>
        /// <param name="i">Индекс поля.</param>
        /// <returns>значение поля as object.</returns>
		public object GetValue(int i)
		{
			return Reader.GetValue(i);
		}

		/// <summary>
		/// Значение указанного поля null 
		/// </summary>
        /// <param name="i">Индекс поля.</param>
        /// <returns>true если значение указанного поля null. Иначе, false.</returns>
		public bool IsDBNull(int i)
		{
			return Reader.IsDBNull(i);
		}

		long IDataRecord.GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return Reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
		}

		byte IDataRecord.GetByte(int i)
		{
			return Reader.GetByte(i);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public Type GetFieldType(int i)
		{
			return Reader.GetFieldType(i);
		}

		decimal IDataRecord.GetDecimal(int i)
		{
			return Reader.GetDecimal(i);
		}

		/// <summary>
		/// Значения всех полей текущей записи. 
		/// </summary>
		/// <param name="values">Массив object, в который копируются значения полей.</param>
		/// <returns>К-во значений в массиве. </returns>
		public int GetValues(object[] values)
		{
			return Reader.GetValues(values);
		}

		/// <summary>
		/// Имя поля по индексу. 
		/// </summary>
		/// <param name="i">Индекс поля. </param>
        /// <returns>Имя поля или пустая строка (""), если поле с указаным индексом не найдено.</returns>
		public string GetName(int i)
		{
			return Reader.GetName(i);
		}

		/// <summary>
		/// К-во полей в текущей записи. 
		/// </summary>
		public int FieldCount
		{
			get
			{
				return Reader.FieldCount;
			}
		}

		long IDataRecord.GetInt64(int i)
		{
			return Reader.GetInt64(i);
		}

		double IDataRecord.GetDouble(int i)
		{
			return Reader.GetDouble(i);
		}

		bool IDataRecord.GetBoolean(int i)
		{
			return Reader.GetBoolean(i);
		}

		Guid IDataRecord.GetGuid(int i)
		{
			return Reader.GetGuid(i);
		}

		DateTime IDataRecord.GetDateTime(int i)
		{
			return Reader.GetDateTime(i);
		}

		/// <summary>
		/// Индекс поля по имени. 
		/// </summary>
		/// <param name="name">Имя поля. </param>
        /// <returns>Индекс поля.</returns>
		public int GetOrdinal(string name)
		{
			return Reader.GetOrdinal(name);
		}

		string IDataRecord.GetDataTypeName(int i)
		{
			return Reader.GetDataTypeName(i);
		}

		float IDataRecord.GetFloat(int i)
		{
			return Reader.GetFloat(i);
		}

		IDataReader IDataRecord.GetData(int i)
		{
			return Reader.GetData(i);
		}

		long IDataRecord.GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return Reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
		}

		string IDataRecord.GetString(int i)
		{
			return Reader.GetString(i);
		}

		char IDataRecord.GetChar(int i)
		{
			return Reader.GetChar(i);
		}

		short IDataRecord.GetInt16(int i)
		{
			return Reader.GetInt16(i);
		}

		#endregion
	}
}
