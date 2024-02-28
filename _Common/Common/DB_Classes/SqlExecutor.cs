using System;
using System.Data;
using System.Data.SqlClient;

namespace Common.DB_Classes
{
	/// <summary>
    /// Выполняет Transact-SQL инструкцию и возвращает количество строк, затронутых изменением
	/// </summary>
	public class SqlExecutor : SqlObject
	{
		private SqlCommand m_asyncCommand = null;


		/// <summary>
		/// Конструктор SqlExecutor класса.
		/// </summary>
        /// <param name="aConnection">SqlConnection, представляющий соединение с экземпляром SQL</param>
		public SqlExecutor(SqlConnection aConnection) : base(aConnection) { }
		/// <summary>
        /// Конструктор SqlExecutor класса.
		/// </summary>
        /// <param name="aConnectionString">Строка соединения с источником данных</param>
		public SqlExecutor(string aConnectionString) : base(aConnectionString) { }
        /// <summary>
        /// Конструктор SqlExecutor класса.
        /// </summary>
        public SqlExecutor() : base() { }
		/// <summary>
        /// Выполнить инструкцию Transact-SQL и вернуть количество строк, затронутых изменением
		/// </summary>
        /// <param name="aSqlText">Текст инструкции.</param>
        /// <param name="aParams">Параметры инструкции.</param>
        /// <param name="aTransaction">Транзакция, в которой выполняется инструкция.</param>
        /// <returns>Количество строк, затронутых изменением</returns>
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
        /// Выполнить инструкцию Transact-SQL и вернуть количество строк, затронутых изменением
		/// </summary>
        /// <param name="aSqlText">Текст инструкции.</param>
        /// <param name="aParams">Параметры инструкции.</param>
        /// <returns>Количество строк, затронутых изменением</returns>
		public int ExecSQL(string aSqlText, params object[] aParams)
		{
			return this.ExecSQL(aSqlText, aParams, null);
		}

		/// <summary>
        /// Выполнить инструкцию Transact-SQL и вернуть количество строк, затронутых изменением
		/// </summary>
        /// <param name="aSqlText">Текст инструкции.</param>
        /// <returns>Количество строк, затронутых изменением</returns>
		public int ExecSQL(string aSqlText)
		{
			return this.ExecSQL(aSqlText, null, null);
		}
        /// <summary>
        /// Выполнить инструкцию Transact-SQL и вернуть количество строк, затронутых изменением
        /// </summary>
        /// <returns>Количество строк, затронутых изменением</returns>
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
