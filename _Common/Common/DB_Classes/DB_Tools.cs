using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common.DB_Classes
{
    /// <summary>
    /// Сервисные методы баз данных
    /// </summary>
    public class DB_Tools
    {
        /// <summary>
        /// Наличие таблицы с заданным имененем в текущем соединении.
        /// </summary>
        /// <param name="pTableName">Имя таблицы</param>
        /// <returns>true - есть, false - нет</returns>
        public static bool TableExist(string pTableName)
        {
            //прежде всего смотрим БД текущего соединения
            bool res = DBConnector.ListDB.TableExist(pTableName);
            //если не нашли, пробуем просто запросом (это скорее всего будет бд master)
            if (!res && string.IsNullOrEmpty(DBConnector.CurrentDBName))
            {
                using (SqlReader reader = new SqlReader())
                {
                    reader.QueryText = "SELECT count(*) FROM sys.objects o join sys.schemas s on s.schema_id = o.schema_id where o.type = 'U' and UPPER(o.name) = UPPER(@TAB_NAME)";
                    //reader.QueryText = "SELECT count(*) FROM INFORMATION_SCHEMA.TABLES where UPPER(TABLE_NAME) = UPPER(@TAB_NAME)";
                    reader.AddParameter("TAB_NAME", pTableName);
                    res = (int)reader.ExecuteScalar() > 0;
                }
            }
            return res;
        }

        /// <summary>
        /// Полное имя таблицы.
        /// </summary>
        /// <param name="pTableName">Имя таблицы</param>
        /// <returns>полное имя</returns>
        public static string FullTableName(string pTableName)
        {
            //прежде всего смотрим БД текущего соединения
            return DBConnector.ListDB.FullTableName(pTableName);
        }

        /// <summary>
        /// Выгрузить значение blob поля в файл.
        /// </summary>
        /// <param name="pTableName">Таблица</param>
        /// <param name="pFieldName">Поле</param>
        /// <param name="pKeys">Список 'Имя поля ID'+'Значение поля ID'</param>
        /// <param name="pFileName">Имя файла, куда выгружаем, если файл уже есть, перезаписывается</param>
        /// <returns></returns>
        public static bool SaveBlobFieldToFile(string pTableName, string pFieldName, NameObjects pKeys, string pFileName)
        {
            using (SqlReader reader = new SqlReader())
            {
                reader.QueryText = "SELECT " + pFieldName + " FROM " + pTableName + " where ID_DD_OBJECT_DDOB = @ID";
                reader.AddParameter("ID", 519014802);
                reader.OpenSQL();
                if (reader.Read())
                {
                    if (File.Exists(pFieldName))
                        File.Delete(pFieldName);
                    byte[] blobContent = reader.ValueAsBinary(pFieldName);
                    using (FileStream fs = new FileStream(pFileName, FileMode.CreateNew, FileAccess.Write))
                    {
                        fs.Write(blobContent, 0, blobContent.Length);
                    }
                }
                else
                    return false;
            }
            return true;
        }

    }
}
