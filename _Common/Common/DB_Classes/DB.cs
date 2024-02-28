using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Common;
using Common.Tools;

namespace Common.DB_Classes
{
    /// <summary>
    /// База данных MS SQL
    /// </summary>
    public class DB_Data
    {
        #region Поля класса DB_Data
        private DB_Files _DATA_Files;
        private string _Name;
        private bool _IsBackUping;
        private bool _IsBackUped;
        private bool _IsRestored;
        private bool _IsRestoring;
        private StateDB _CurrentStateDB;
        private NameObjects _Tables;
        #endregion
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="pName">Имя БД</param>
        public DB_Data(string pName) //, StateDB pStateDB
        {
            _IsBackUped = false;
            _IsBackUping = false;
            _IsRestored = false;
            _IsRestoring = false;
            _Name = pName;
            _CurrentStateDB = StateDB.Unknown;
            ReadDBFiles();
        }
        /*
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="pName">Имя БД</param>
        public DB_Data(string pName) : this(pName, StateDB.Unknown) { }
        /**/
        /// <summary>
        /// Файлы БД
        /// </summary>
        public DB_Files DATA_Files 
        { 
            get 
            { 
                if (_DATA_Files == null) 
                    _DATA_Files = new DB_Files(); 
                return _DATA_Files; 
            } 
        }

        /// <summary>
        /// БД допустима для использования
        /// </summary>
        public bool IsActive
        {
            get
            {
                return (_CurrentStateDB == StateDB.Online
                    || _CurrentStateDB == StateDB.Recovery_Pending
                    || _CurrentStateDB == StateDB.Restored); 
            }
        }

        /// <summary>
        /// Таблицы БД
        /// </summary>
        public NameObjects Tables
        {
            get
            {
                if (_Tables == null)
                    _Tables = new NameObjects();
                return _Tables;
            }
        }

        /// <summary>
        /// Имя БД
        /// </summary>
        public string Name 
        { 
            get 
            { 
                return _Name; 
            } 
        }
        /// <summary>
        /// Текущий статус БД
        /// </summary>
        public StateDB CurrentStateDB 
        { 
            get 
            {
                return _CurrentStateDB; 
            } 
            set 
            {
                _CurrentStateDB = value; 
            } 
        }
        /// <summary>
        /// База отобрана для резервного копирования
        /// </summary>
        public bool IsBackUping { get { return _IsBackUping; } set { _IsBackUping = value; } }
        /// <summary>
        /// База успешно скопирована
        /// </summary>
        public bool IsBackUped { get { return _IsBackUped; } set { _IsBackUped = value; } }
        /// <summary>
        /// База отобрана для восстановления
        /// </summary>
        public bool IsRestoring { get { return _IsRestoring; } set { _IsRestoring = value; } }
        /// <summary>
        /// База успешно восстановлена
        /// </summary>
        public bool IsRestored { get { return _IsRestored; } set { _IsRestored = value; } }
        /// <summary>
        /// Файл резервной копии для восстановления
        /// </summary>
        public string BackUpFile { get; set; }

        //public DateTime DATE_1 { get; set; }

        //public DateTime DATE_2 { get; set; }

        //public double Double_1 { get; set; }

        //double Double0 = 15.1234;
        /// <summary>
        /// Определить статус и файлы БД
        /// </summary>
        public void ReadDBFiles()
        {
            ArrayList _List = new ArrayList();
            SqlReader reader = new SqlReader();
            try
            {
                reader.OpenSQL(string.Format("SELECT name, state FROM sys.databases where upper(name) = '{0}'", _Name.ToUpper()));
                reader.Fill(_List);
                if (_List.Count == 0)
                {
                    _CurrentStateDB = StateDB.NoDB;
                    reader.Close();
                    reader.Dispose();
                    return;
                }
                else
                {
                    int stat = -1;
                    if (!Int32.TryParse(((object[])_List[0])[1].ToString(), out stat) || (stat < 0 && stat > 6))
                        stat = 9;
                    _CurrentStateDB = (StateDB)stat;
                }
                reader.Close();
                reader.OpenSQL("use " + _Name + " select type, name, physical_name, state from sys.database_files ");
                reader.Fill(_List);
                reader.Close();
                reader.Dispose();
                for (int i = 0; i < _List.Count; i++)
                {
                    int typ = -1;
                    if (!Int32.TryParse(((object[])_List[i])[0].ToString(), out typ))
                        typ = -1;
                    if (typ == 0)
                    {
                        this.DATA_Files.RowsLogicalName = ((object[])_List[i])[1].ToString();
                        this.DATA_Files.RowsPhysicalName = ((object[])_List[i])[2].ToString();
                    }
                    else if (typ == 1)
                    {
                        this.DATA_Files.LogLogicalName = ((object[])_List[i])[1].ToString();
                        this.DATA_Files.LogPhysicalName = ((object[])_List[i])[2].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось определить файлы баз данных сервера"
                    + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Имеется ли в БД таблица с именем pTableName 
        /// </summary>
        /// <param name="pTableName">Имя таблицы</param>
        /// <returns>да, нет</returns>
        public bool TableExist(string pTableName)
        {
            bool res = false;
            if (!string.IsNullOrWhiteSpace(pTableName))
            {
                pTableName = pTableName.ToShortTableName();
                res = this.Tables.IndexOfName(pTableName) >= 0;
                if (!res && this.IsActive)
                {
                    using (SqlReader reader = new SqlReader())
                    {
                        reader.QueryText = "use " + _Name + " SELECT '.' + s.name + '.' + o.name FROM sys.objects o join sys.schemas s on s.schema_id = o.schema_id where o.type = 'U' and UPPER(o.name) = UPPER(@TAB_NAME)";
                        reader.AddParameter("TAB_NAME", pTableName);
                        string res0 = (string)reader.ExecuteScalar();
                        if (!string.IsNullOrWhiteSpace(res0))
                        {
                            res0 = _Name + res0;
                            this.Tables.Add(pTableName, res0);
                            res = true;
                        }
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Все таблицы БД. Заполняется список Tables 
        /// </summary>
        /// <returns>Количество таблиц в БД</returns>
        public int GetAllTables()
        {
            this.Tables.Clear();
            if (this.IsActive)
                using (SqlReader reader = new SqlReader())
                {
                    reader.QueryText = "use " + _Name + " SELECT upper(o.name) TabName, '.'+ upper(s.name) +'.'+ upper(o.name) FullName FROM sys.objects o join sys.schemas s on s.schema_id = o.schema_id where o.type = 'U' order by o.name";
                    reader.OpenSQL();
                    while (reader.Read())
                        this.Tables.Add(reader.ValueAsString("TabName"), _Name + reader.ValueAsString("FullName"));
                }
            return this.Tables.Count;
        }
    }
    /// <summary>
    /// Файлы базы данных MS SQL
    /// </summary>
    public class DB_Files
    {
        /// <summary>
        /// Файл данных. Логическое имя 
        /// </summary>
        public string RowsLogicalName { get; set; }
        /// <summary>
        /// Файл данных. Физическое имя 
        /// </summary>
        public string RowsPhysicalName { get; set; }
        /// <summary>
        /// Log файл. Логическое имя 
        /// </summary>
        public string LogLogicalName { get; set; }
        /// <summary>
        /// Log файл. Физическое имя
        /// </summary>
        public string LogPhysicalName { get; set; }
    }
}
