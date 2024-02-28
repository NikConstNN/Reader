using System;
using System.Data;
using System.Data.SqlClient;

namespace Common.DB_Classes
{
	/// <summary>
    /// ��������� Transact-SQL ���������� � ���������� ���������� �����, ���������� ����������
	/// </summary>
	public class SqlExecutor : SqlObject
	{
		private SqlCommand m_asyncCommand = null;


		/// <summary>
		/// ����������� SqlExecutor ������.
		/// </summary>
        /// <param name="aConnection">SqlConnection, �������������� ���������� � ����������� SQL</param>
		public SqlExecutor(SqlConnection aConnection) : base(aConnection) { }
		/// <summary>
        /// ����������� SqlExecutor ������.
		/// </summary>
        /// <param name="aConnectionString">������ ���������� � ���������� ������</param>
		public SqlExecutor(string aConnectionString) : base(aConnectionString) { }
        /// <summary>
        /// ����������� SqlExecutor ������.
        /// </summary>
        public SqlExecutor() : base() { }
		/// <summary>
        /// ��������� ���������� Transact-SQL � ������� ���������� �����, ���������� ����������
		/// </summary>
        /// <param name="aSqlText">����� ����������.</param>
        /// <param name="aParams">��������� ����������.</param>
        /// <param name="aTransaction">����������, � ������� ����������� ����������.</param>
        /// <returns>���������� �����, ���������� ����������</returns>
		public int ExecSQL(string aSqlText, object[] aParams, SqlTransaction aTransaction)
		{
            int result = -1;
            if (string.IsNullOrWhiteSpace(aSqlText) && string.IsNullOrWhiteSpace(this.QueryText))
                return result;

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

            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }

            SqlCommand command = new SqlCommand(sql, Connection, aTransaction);
            command.CommandTimeout = this.ConnectTimeout >= 0 ? this.ConnectTimeout : Connection.ConnectionTimeout;
            foreach (SqlParameter par in Parameters)
            {
                command.Parameters.Add(par);
            }

			try
			{
				result = command.ExecuteNonQuery();
			}
			finally
			{
				command.Dispose();
			}

			return result;
		}

		/// <summary>
        /// ��������� ���������� Transact-SQL � ������� ���������� �����, ���������� ����������
		/// </summary>
        /// <param name="aSqlText">����� ����������.</param>
        /// <param name="aParams">��������� ����������.</param>
        /// <returns>���������� �����, ���������� ����������</returns>
		public int ExecSQL(string aSqlText, params object[] aParams)
		{
			return this.ExecSQL(aSqlText, aParams, null);
		}

		/// <summary>
        /// ��������� ���������� Transact-SQL � ������� ���������� �����, ���������� ����������
		/// </summary>
        /// <param name="aSqlText">����� ����������.</param>
        /// <returns>���������� �����, ���������� ����������</returns>
		public int ExecSQL(string aSqlText)
		{
			return this.ExecSQL(aSqlText, null, null);
		}
        /// <summary>
        /// ��������� ���������� Transact-SQL � ������� ���������� �����, ���������� ����������
        /// </summary>
        /// <returns>���������� �����, ���������� ����������</returns>
        public int ExecSQL()
        {
            return this.ExecSQL(string.Empty, null, null);
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		public override void Dispose()
		{
			if (m_asyncCommand != null)
			{
				m_asyncCommand.Dispose();
			}
			base.Dispose();
		}
	}
}
