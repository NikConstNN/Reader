using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Common;


//using Sda.Common;

namespace Common.DB_Classes
{
	/// <summary>
    /// ������������� �������� ������ ����������������� ������ ����� �� ���� ������ SQL Server.
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
        /// ����������� SqlReader class.
		/// </summary>
        /// <param name="aConnection">SqlConnection, �������������� ���������� � ����������� SQL</param>
		public SqlReader(SqlConnection aConnection) : base(aConnection) { }

		/// <summary>
        /// ����������� SqlReader class.
		/// </summary>
		/// <param name="aConnectionString">������ ���������� � ���������� ������</param>
		public SqlReader(string aConnectionString) : base(aConnectionString) { }

        /// <summary>
        /// ����������� SqlReader class.
        /// </summary>
        public SqlReader() : base() { }

        /// <summary>
        /// ������� ������ ������� ������ ������ ��������������� ������, ������������� ��������
        /// </summary>
        /// <returns>�������� ������� ������� ������ ������</returns>
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
		/// ������� ������ ��� SP
		/// </summary>
		/// <param name="aSqlText">����� �������.</param>
		/// <param name="aParams">��������� �������.</param>
		/// <param name="aTransaction">����������, � ������� ����������� ������.</param>
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
        /// ������� ������ ��� SP
		/// </summary>
        /// <param name="aSqlText">����� �������.</param>
        /// <param name="aParams">��������� �������.</param>
		public void OpenSQL(string aSqlText, params object[] aParams)
		{
			this.OpenSQL(aSqlText, aParams, null);
		}

		/// <summary>
        ///  ������� ������ ��� SP
		/// </summary>
        /// <param name="aSqlText">����� �������</param>
		public void OpenSQL(string aSqlText)
		{
			this.OpenSQL(aSqlText, null);
		}

        /// <summary>
        ///  ������� ������ ��� SP. ����� ������� ������� �� �-�� QueryText �������
        /// </summary>
        public void OpenSQL()
        {
            this.OpenSQL(string.Empty);
        }

		/// <summary>
		/// ���� is null
		/// </summary>
		/// <param name="aFieldName">��� ����</param>
        /// <returns>true ���� ��������� �������� ������� ������������ DBNull, � ��������� ������ false.</returns>
		public bool IsDBNull(string aFieldName)
		{
			return Reader.IsDBNull(Reader.GetOrdinal(aFieldName));
		}

		/// <summary>
		/// ����� ������� ������ �������
		/// </summary>
		public int CurrentRowIndex
		{
			get
			{
				return m_currentRowIndex;
			}
		}

		/// <summary>
        /// ���������� ����� ������� ������ �������
		/// </summary>
		/// <param name="aRowIndex">������� �����</param>
		protected virtual void SetCurrentRowIndex(int aRowIndex)
		{
			m_currentRowIndex = aRowIndex;
		}

		/// <summary>
		/// ������ �� �������
		/// </summary>
		public bool IsClosed
		{
			get
			{
				return !m_isOpened;
			}
		}

		/// <summary>
        /// ��������� ������������ ������� ArrayList
		/// </summary>
        /// <param name="aDataList">ArrayList ��� ����������</param>
        /// <returns>���-�� ������� ���������� � ArrayList.</returns>
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
        /// ��������� ������������ ������� DataTable.
		/// </summary>
        /// <param name="aTable">DataTable ��� ����������</param>
        /// <returns>���-�� ������� ���������� � DataTable.</returns>
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
		/// ���� �� ������ � �������
		/// </summary>
		public bool HasRows
		{
			get
			{
				return Reader.HasRows;
			}
		}

		#region �������� ����� �������

		/// <summary>
		/// �������� ���� as string.
		/// </summary>
		/// <param name="aFieldName">��� ����</param>
		/// <returns>��������</returns>
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
        /// �������� ���� as int.
		/// </summary>
        /// <param name="aFieldName">��� ����</param>
        /// <returns>��������</returns>
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
        /// �������� ���� as Int64.
		/// </summary>
        /// <param name="aFieldName">��� ����.</param>
        /// <returns>��������</returns>
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
        /// �������� ���� as DataTime.
		/// </summary>
        /// <param name="aFieldName">��� ����</param>
        /// <returns>��������</returns>
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
        /// �������� ���� as bool.
		/// </summary>
        /// <param name="aFieldName">��� ����</param>
        /// <returns>��������</returns>
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
        /// ����� ������ �� ���� � byte ������.
		/// </summary>
        /// <param name="aFieldName">��� ����</param>
        /// <returns>��������</returns>
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
        /// �������� ���� as decimal.
		/// </summary>
        /// <param name="aFieldName">��� ����</param>
        /// <returns>��������</returns>
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
        /// �������� ���� as double.
		/// </summary>
        /// <param name="aFieldName">��� ����</param>
        /// <returns>��������</returns>
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
		/// SqlDataReader ����������.
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
        /// ���������� ������ DataTable, ����������� ���������� ������� �� IDataReader.
		/// </summary>
		/// <returns></returns>
		public DataTable GetSchemaTable()
		{
			return Reader.GetSchemaTable();
		}

		/// <summary>
        /// ���������� ����� ���� ��������, ��������� ��� ������� ��� ���������� SQL �������.
		/// </summary>
		public int RecordsAffected
		{
			get
			{
				return Reader.RecordsAffected;
			}
		}

		/// <summary>
		/// ������� � ��������� ������.
		/// </summary>
		/// <returns>true ���� ��������� ������ ����; �����, false.</returns>
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
		/// ������� DataReader
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
        /// ������� ������ ������ � ���������� ����������, ���� ������ ���������� ��������� �������.
		/// </summary>
		/// <returns>true ���� � ��������� ������ ���� ������; �����, false.</returns>
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
        /// �������� ���� � ��������� ������. 
		/// </summary>
        /// <param name="name">��� �������.</param>
        /// <returns>���� � ��������� ������ as Object.</returns>
		public object this[string name]
		{
			get
			{
				return Reader[name];
			}
		}

		/// <summary>
        /// �������� ���� � ��������� ��������. 
		/// </summary>
		/// <param name="i">������ �������. </param>
        /// <returns>���� � ��������� �������� as Object.</returns>
		object IDataRecord.this[int i]
		{
			get
			{
				return Reader[i];
			}
		}

		/// <summary>
		/// �������� ���� �� �������.
		/// </summary>
        /// <param name="i">������ ����.</param>
        /// <returns>�������� ���� as object.</returns>
		public object GetValue(int i)
		{
			return Reader.GetValue(i);
		}

		/// <summary>
		/// �������� ���������� ���� null 
		/// </summary>
        /// <param name="i">������ ����.</param>
        /// <returns>true ���� �������� ���������� ���� null. �����, false.</returns>
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
		/// �������� ���� ����� ������� ������. 
		/// </summary>
		/// <param name="values">������ object, � ������� ���������� �������� �����.</param>
		/// <returns>�-�� �������� � �������. </returns>
		public int GetValues(object[] values)
		{
			return Reader.GetValues(values);
		}

		/// <summary>
		/// ��� ���� �� �������. 
		/// </summary>
		/// <param name="i">������ ����. </param>
        /// <returns>��� ���� ��� ������ ������ (""), ���� ���� � �������� �������� �� �������.</returns>
		public string GetName(int i)
		{
			return Reader.GetName(i);
		}

		/// <summary>
		/// �-�� ����� � ������� ������. 
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
		/// ������ ���� �� �����. 
		/// </summary>
		/// <param name="name">��� ����. </param>
        /// <returns>������ ����.</returns>
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
