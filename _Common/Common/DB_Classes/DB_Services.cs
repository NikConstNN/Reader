using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Common.DB_Classes
{
    /// <summary>
    /// Содержит методы обслуживания сервера
    /// </summary>
    public class DB_Services
    {
        /// <summary>
        /// Базы данных текущего сервера. Все БД
        /// </summary>
        /// <param name="aListDB">Список баз данных</param>
        public static void GetServerDB(List<string> aListDB)
        {
            GetServerDB(aListDB, false, false);
        }

        /// <summary>
        /// Базы данных сервера
        /// </summary>
        /// <param name="aListDB">Список баз данных</param>
        /// <param name="aOnlyUser">Исключить системные БД</param>
        /// <param name="aOnlyOnline">Только БД со статусом ONLINE</param>
        public static void GetServerDB(List<string> aListDB, bool aOnlyUser, bool aOnlyOnline)
        {
            if (aListDB == null)
                aListDB = new List<string>();
            else 
                aListDB.Clear();
            SqlReader reader = new SqlReader();
            reader.OpenSQL(string.Format("SELECT name, state FROM sys.databases {0} {1} {2} {3} order by name"
                , aOnlyUser || aOnlyOnline ? "where" : string.Empty
                , aOnlyUser ? "UPPER(name) not in ('MASTER','MODEL','MSDB','TEMPDB', 'RESOURCE')" : string.Empty
                , aOnlyOnline && aOnlyUser ? "and" : string.Empty
                , aOnlyOnline ? "state = 0" : string.Empty));
            ArrayList _ListDB = new ArrayList();
            reader.Fill(_ListDB);
            reader.Close();
            reader.Dispose();
            for (int i = 0; i < _ListDB.Count; i++)
                aListDB.Add(((object[])_ListDB[i])[0].ToString());
            _ListDB.Clear();
            _ListDB = null;
        }
        /// <summary>
        /// Заполнить список БД текущего сервера
        /// </summary>
        /// <param name="aDBList">Список БД</param>
        /// <param name="aOnlyUser">Исключить системные БД</param>
        /// <param name="aOnlyOnline">Только БД со статусом ONLINE</param>
        /// <returns></returns>
        public static bool FillDBList(DBList aDBList, bool aOnlyUser, bool aOnlyOnline)
        {
            bool res = true;
            aDBList.Clear();
            List<string> _ListDB = new List<string>();
            GetServerDB(_ListDB, aOnlyUser, aOnlyOnline);            
            for (int i = 0; i < _ListDB.Count; i++)
            {
                
                DB_Data db = new DB_Data(_ListDB[i]);
                if (db != null && db.CurrentStateDB != StateDB.NoDB)
                {
                    aDBList.Add(db);
                }
                else
                    res = false;
            }
            return res;
        }
    }
}
