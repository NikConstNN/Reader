using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Common.Tools;

namespace Common.DB_Classes
{
    /// <summary>
    /// Состояние базы данных. С 0 по 6 - стандартные статусы БД MS SQL, остальные дополнительные
    /// </summary>
    public enum StateDB : short
    {
        /// <summary>
        /// Доступна
        /// </summary>
        Online = 0,
        /// <summary>
        /// Восстановление
        /// </summary>
        Restoring = 1,
        /// <summary>
        /// Восстанавливается
        /// </summary>
        Recovering = 2,
        /// <summary>
        /// Ожидает восстановления
        /// </summary>
        Recovery_Pending = 3,
        /// <summary>
        /// Файл БД поврежден
        /// </summary>
        Suspect = 4,
        /// <summary>
        /// Аварийная (файл БД поврежден)
        /// </summary>
        Emergency = 5,
        /// <summary>
        /// В автономном режиме
        /// </summary>
        Offline = 6,
        /// <summary>
        /// Ошибка восстановления
        /// </summary>
        Error_Restore = 7,
        /// <summary>
        /// Восстановлена
        /// </summary>
        Restored = 8,
        /// <summary>
        /// Не определен
        /// </summary>
        Unknown = 9,
        /// <summary>
        /// Базы данных не существует (новая)
        /// </summary>
        NoDB = 10
    }
    /// <summary>
    /// Коллекция объектов DB_Data, базы данных
    /// </summary>
    public class DBList : CollectionBase //, IBindingList
    {
        #region Поля класса DB_Data
        private string[] _StateDBNames;
        private string _CurrentDBName;
        #endregion

        /// <summary>
        /// Русские имена статусов БД
        /// </summary>
        public string[] StateDBNames
        {
            get
            {
                if (_StateDBNames == null)
                {
                    _StateDBNames = new string[]
				    {
                        "Доступна"
                        , "Восстановление"
                        , "Восстанавливается"
                        , "Выбрана"
                        , "Аварийная"
                        , "Аварийная"
                        , "Автономно"
                        , "Ошибка"
                        , "Восстановлена"
                        , "Не определен"
                        , "Новая" 
				    };
                }
                return _StateDBNames;
            }
        }

        /// <summary>
        /// Имя текущей базы данных. Если БД с таким именем нет, то добавляется в список.
        /// </summary>
        public string CurrentDBName
        {
            get
            {
                return _CurrentDBName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _CurrentDBName = string.Empty;
                else 
                {
                    _CurrentDBName = value;
                    if (this.IndexOfNameDB(_CurrentDBName) < 0)
                    {
                        this.Add(new DB_Data(_CurrentDBName));
                    }
                }
            }
        }
        /// <summary>
        /// Текущая БД. Если не задано имя CurrentDBName - возвращает null
        /// </summary>
        public DB_Data CurrentDB
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CurrentDBName))
                    return null;
                else
                    return this[_CurrentDBName];
            }
        }

        /// <summary>
        /// Активна ли текущая БД. Если нет текущей БД, false.
        /// </summary>
        public bool CurrentDBIsActive
        {
            get
            {
                if (CurrentDB != null)
                    return CurrentDB.IsActive;
                else
                    return false;
            }
        }

        /// <summary>
        /// БД по имени
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <returns>БД</returns>
        public DB_Data this[string Name]
        {
            get
            {
                int ind = this.IndexOfNameDB(Name);
                if (ind < 0)
                    return null;
                else
                    return (DB_Data)this.InnerList[ind];
            }
        }
        /// <summary>
        /// БД по индексу
        /// </summary>
        /// <param name="Index">Индекс</param>
        /// <returns>БД</returns>
        public DB_Data this[int Index]
        {
            get
			{
				return (DB_Data)this.InnerList[Index];
			}
			set
			{
				this.InnerList[Index] = value;
			}
        }
        /// <summary>
        /// Вставить объект в коллекцию по индексу.
        /// </summary>
        /// <param name="aIndex">Индекс.</param>
        /// <param name="aValue">объект типа DB_Data</param>
        public void Insert(int aIndex, DB_Data aValue)
        {
            this.InnerList.Insert(aIndex, aValue);
        }

        /// <summary>
        /// Удалить по значению
        /// </summary>
        /// <param name="aValue">Значение, объект типа DB_Data</param>
        public void Remove(DB_Data aValue)
        {
            this.InnerList.Remove(aValue);
        }

        /// <summary>
        /// Удалить по имени БД
        /// </summary>
        /// <param name="aName">Имя БД</param>
        public void RemoveOfNameDB(string aName)
        {
            int ind = this.IndexOfNameDB(aName);
            if (ind >=0)
                this.InnerList.Remove(this.InnerList[ind]);
        }

        /// <summary>
        /// Индекс по значению
        /// </summary>
        /// <param name="aValue">Значение, объект типа DB_Data</param>
        /// <returns>Индекс первого элемента с указаным значением или -1</returns>
        public int IndexOf(DB_Data aValue)
        {
            return this.InnerList.IndexOf(aValue);
        }
        /// <summary>
        /// Индекс по имени БД
        /// </summary>
        /// <param name="aName">Имя БД</param>
        /// <returns>Индекс первого элемента с указаным именем БД или -1</returns>
        public int IndexOfNameDB(string aName)
        {
            int res = -1;
            if (!string.IsNullOrWhiteSpace(aName))
            { 
                aName = aName.ToUpper();
                for (int i = 0; i < this.InnerList.Count; i++)
                    if (((DB_Data)this.InnerList[i]).Name.ToUpper().CompareTo(aName) == 0)
                    {
                        res = i;
                        break;
                    }
            }
            return res;
        }
        /// <summary>
        /// Добавить объект в коллекцию
        /// </summary>
        /// <param name="aValue">объект типа DB_Data</param>
        /// <returns>Индекс</returns>
        public int Add(DB_Data aValue)
        {
            int index = this.InnerList.Add(aValue);
            return index;
        }
        #region Поиск таблицы с заданным именем в списке БД
        /// <summary>
        /// Имеется ли таблица с именем pTableName хоть в одной БД из этого списка
        /// </summary>
        /// <param name="pTableName">Имя таблицы</param>
        /// <returns></returns>
        public bool TableExist(string pTableName)
        {
            return TableExist(string.Empty, pTableName);
        }
        /// <summary>
        /// Имеется ли таблица с именем pTableName  
        /// </summary>
        /// <param name="pDBName">Имя БД</param>
        /// <param name="pTableName">Имя таблицы</param>
        /// <returns></returns>
        public bool TableExist(string pDBName, string pTableName)
        {
            string res = FindFullTableName(pDBName, pTableName);
            return !string.IsNullOrEmpty(res);
        }

        /// <summary>
        /// Полное имя таблицы
        /// </summary>
        /// <param name="pTableName">Имя таблицы</param>
        /// <returns></returns>
        public string FullTableName(string pTableName)
        {
            return FullTableName(string.Empty, pTableName);
        }
        /// <summary>
        /// Полное имя таблицы
        /// </summary>
        /// <param name="pDBName">Имя БД</param>
        /// <param name="pTableName">Имя таблицы</param>
        /// <returns></returns>
        public string FullTableName(string pDBName, string pTableName)
        {
            string res = FindFullTableName(pDBName, pTableName);
            if (string.IsNullOrEmpty(res))
                return pTableName;
            else
                return res;
        }

        private string FindFullTableName(string pDBName, string pTableName)
        {
            if (string.IsNullOrEmpty(pTableName))
                return string.Empty;
            string table = pTableName.ToShortTableName();
            foreach (DB_Data db in this)
            {
                if ((string.IsNullOrWhiteSpace(pDBName) || pDBName.Equals(db.Name)) && db.Tables.IndexOfName(table) >= 0)
                    return db.Tables[table].tostring();
            }
            foreach (DB_Data db in this)
            {
                if ((string.IsNullOrWhiteSpace(pDBName) || pDBName.Equals(db.Name)) && db.TableExist(table))
                {
                    return db.Tables[table].tostring();
                }
            }
            return string.Empty;        
        }

        #endregion
        /*
        #region IBindingList Members

        /// <summary>
        /// Adds the PropertyDescriptor to the indexes used for searching.
        /// </summary>
        /// <param name="property">The PropertyDescriptor to add to the indexes used for searching.</param>
        public virtual void AddIndex(PropertyDescriptor property)
        {

        }

        /// <summary>
        /// Gets whether you can add items to the list using AddNew.
        /// </summary>
        public virtual bool AllowNew
        {
            get
            {
                return false;
            }
        }

        private ListSortDirection m_sortDirection = ListSortDirection.Ascending;
        private PropertyDescriptor m_currentSortProperty;

        /// <summary>
        /// Sorts the list based on a PropertyDescriptor and a ListSortDirection.
        /// </summary>
        /// <param name="aProperty">The PropertyDescriptor to sort by.</param>
        /// <param name="aDirection">One of the ListSortDirection values.</param>
        public virtual void ApplySort(PropertyDescriptor aProperty, ListSortDirection aDirection)
        {
            m_currentSortProperty = aProperty;
            m_sortDirection = aDirection;
            Common.ComparerByValueProperty comparer = new Common.ComparerByValueProperty(aProperty.Name, m_sortDirection);
            this.InnerList.Sort(comparer);
        }

        /// <summary>
        /// Gets the PropertyDescriptor that is being used for sorting.
        /// </summary>
        public PropertyDescriptor SortProperty
        {
            get
            {
                return m_currentSortProperty;
            }
        }

        /// <summary>
        /// Gets the direction of the sort.
        /// </summary>
        public ListSortDirection SortDirection
        {
            get
            {
                return m_sortDirection;
            }
        }

        /// <summary>
        /// Returns the index of the row that has the given PropertyDescriptor.
        /// </summary>
        /// <param name="property">The PropertyDescriptor to search on.</param>
        /// <param name="key">The value of the property parameter to search for.</param>
        /// <returns>The index of the row that has the given PropertyDescriptor.</returns>
        public virtual int Find(PropertyDescriptor property, object key)
        {
            int row = -1;
            string strKey = key == null ? string.Empty : key.ToString();
            for (int i = 0; i < InnerList.Count; i++)
            {
                object obj = InnerList[i];
                object value = property.GetValue(obj);
                string strValue = value == null ? string.Empty : value.ToString();
                if (string.Compare(strValue, 0, strKey, 0, strKey.Length) == 0)
                {
                    row = i;
                    break;
                }
            }
            return row;
        }

        /// <summary>
        /// Gets whether the list supports sorting.
        /// </summary>
        public virtual bool SupportsSorting
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets whether the items in the list are sorted.
        /// </summary>
        public virtual bool IsSorted
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets whether you can remove items from the list, using Remove or RemoveAt.
        /// </summary>
        public virtual bool AllowRemove
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets whether the list supports searching using the Find method.
        /// </summary>
        public virtual bool SupportsSearching
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// Gets whether a ListChanged event is raised when the list changes or an item in the list changes
        /// </summary>
        public virtual bool SupportsChangeNotification
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Removes any sort applied using ApplySort.
        /// </summary>
        public virtual void RemoveSort()
        {

        }

        /// <summary>
        /// Adds a new item to the list.
        /// </summary>
        /// <returns>The item added to the list.</returns>
        public virtual object AddNew()
        {
            return null;
        }

        /// <summary>
        /// Gets whether you can update items in the list.
        /// </summary>
        public virtual bool AllowEdit
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the PropertyDescriptor from the indexes used for searching.
        /// </summary>
        /// <param name="property">The PropertyDescriptor to remove from the indexes used for searching.</param>
        public virtual void RemoveIndex(PropertyDescriptor property)
        {
        }
        /// <summary>
        /// Occurs when the list changes or an item in the list changes.
        /// </summary>
        public event ListChangedEventHandler ListChanged;

        /// <summary>
        /// Raises the Change event. OnChanged is called when change are object properties
        /// </summary>
        /// <param name="aEventArgs">A ModelObjectEventArgs that contains the event data</param>
        protected void OnListChanged(ListChangedEventArgs aEventArgs)
        {
            if (ListChanged != null)
            {
                ListChanged(this, aEventArgs);
            }
        }

        #endregion
        */
    }
}
