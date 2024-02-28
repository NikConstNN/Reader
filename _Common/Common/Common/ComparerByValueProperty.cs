using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;

namespace Common
{
	/// <summary>
    /// Сравнение значения свойства двух объектов. Метод объекта класса 'Compare' используется в качестве компаратора для сортировки 
    /// списков или массивов произвольных обектов по значению свойства объекта. Если объект = null, то считается минимальным.
	/// </summary>
	public class ComparerByValueProperty: IComparer
	{
		private string m_propertyName;
		private Comparer m_comparer;
        private System.Reflection.PropertyInfo m_propertyInfo;
		private int m_directionIndex;
        private bool m_StringIgnoreCase;
        private bool m_IsStringType;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="aPropertyName">Имя свойства сравнения</param>
        /// <param name="aDirection">Направление сравнения</param>
        /// <param name="aStringIgnoreCase">Не учитывать регистр при сравнения строк. Для типов не string параметр игнорируется</param>
        public ComparerByValueProperty(string aPropertyName, ListSortDirection aDirection, bool aStringIgnoreCase)
        {
            m_propertyName = aPropertyName;
            m_IsStringType = false;
            m_propertyInfo = null;
            m_comparer = new Comparer(System.Globalization.CultureInfo.CurrentCulture);
            m_directionIndex = (aDirection == ListSortDirection.Ascending) ? 1 : -1;
            m_StringIgnoreCase = aStringIgnoreCase;
        }
        /// <summary>
        /// Конструктор. Направление сравнения - возрастание.
        /// </summary>
        /// <param name="aPropertyName">Имя свойства сравнения</param>
        /// <param name="aStringIgnoreCase">Не учитывать регистр при сравнения строк. Для типов не string параметр игнорируется</param>
        public ComparerByValueProperty(string aPropertyName, bool aStringIgnoreCase) : this(aPropertyName, ListSortDirection.Ascending, aStringIgnoreCase)
        {
        }
		/// <summary>
        /// Конструктор. Строки сравниваются без учета регистра
		/// </summary>
		/// <param name="aPropertyName">Имя свойства сравнения</param>
        /// <param name="aDirection">Направление сравнения</param>
		public ComparerByValueProperty(string aPropertyName, ListSortDirection aDirection) : this(aPropertyName, aDirection, true)
		{
		}
        /// <summary>
        /// Конструктор. Направление сравнения - возрастание. Строки сравниваются без учета регистра
        /// </summary>
        /// <param name="aPropertyName">Имя свойства сравнения</param>
        public ComparerByValueProperty(string aPropertyName) : this(aPropertyName, ListSortDirection.Ascending, true)
        {
        }

		#region IComparer метод

		/// <summary>
        /// Выполняет сравнение значения свойства двух объектов одного и того же типа.
		/// </summary>
        /// <param name="obj1">1-й объект для сравнения</param>
        /// <param name="obj2">2-й объект для сравнения</param>
		/// <returns></returns>
		public int Compare(object obj1, object obj2)
		{
			object val1, val2;
            if (obj1 == null && obj2 == null)
                return 0;
            else if (obj1 != null && obj2 == null)
                return 1 * m_directionIndex;
            else if (obj1 == null && obj2 != null)
                return -1 * m_directionIndex;
            try
            {
                if (m_propertyInfo == null)
                {
                    m_propertyInfo = obj1.GetType().GetProperty(m_propertyName);
                    m_IsStringType = (m_propertyInfo.PropertyType == typeof(String) || m_propertyInfo.PropertyType == typeof(string));
                }
                val1 = m_propertyInfo.GetValue(obj1, null);
                val2 = m_propertyInfo.GetValue(obj2, null);             
            }
            catch
            {
                throw new ArgumentException(string.Format("Сравнение значения свойства двух объектов. У объекта типа '{0}' нет свойства с именем '{1}' ", obj1.GetType().Name, m_propertyName));
            }

            if (m_IsStringType && m_StringIgnoreCase)
                return string.Compare(val1 == null ? null : val1.ToString(), val2 == null ? null : val2.ToString(), m_StringIgnoreCase, System.Globalization.CultureInfo.CurrentCulture) * m_directionIndex;
            else
            {
                if ((val1 is IComparable) || (val2 is IComparable))
                    return m_comparer.Compare(val1, val2) * m_directionIndex;
                else
                    return 0;
            }
		}

        #endregion

        public static void SortLinked<T>(List<T> lst, object[] linked_lists, Comparison<T> comparison)
        {
            List<T> basestore = new List<T>();
            basestore.AddRange(lst);
            lst.Clear();
            List<object>[] store = new List<object>[linked_lists.Length];
            for (int i = linked_lists.GetLowerBound(0); i <= linked_lists.GetUpperBound(0); i++)
            {
                if (linked_lists[i].GetType().IsGenericType && linked_lists[i] is IList)
                {
                    store[i] = new List<object>();
                    IList lstx = (IList)linked_lists[i];
                    for (int j = 0; j < lstx.Count; j++)
                        store[i].Add(lstx[j]);
                    lstx.Clear();
                }
                else
                    throw new InvalidCastException();
            }
            List<int> index = new List<int>();
            index.AddRange(Enumerable.Range(0, basestore.Count()));
            index.Sort(delegate (int x, int y)
            {
                return comparison(basestore[x], basestore[y]);
            });
            for (int i = 0; i < index.Count; i++)
            {
                lst.Add(basestore[index[i]]);
                for (int j = linked_lists.GetLowerBound(0); j <= linked_lists.GetUpperBound(0); j++)
                {
                    IList lstx = (IList)linked_lists[j];
                    lstx.Add(store[j][index[i]]);
                }
            }
        } //this 


        public static Dictionary<TKey, TValue>? SortDictionaryByKey<TKey, TValue>(Dictionary<TKey, TValue> pDictionary, ListSortDirection pDirection = ListSortDirection.Ascending) where TKey : notnull
        {
            if (pDictionary != null && pDictionary.Count > 1)
            {
                List<TKey> keys = pDictionary.Keys.ToList();
                //ComparerByValueProperty comparer = new ComparerByValueProperty("", pDirection);
                keys.Sort();
                Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();
                if (pDirection == ListSortDirection.Ascending)
                {
                    for (int i = 0; i < keys.Count; i++)
                        dic.Add(keys[i], pDictionary[keys[i]]);
                }
                else
                {
                    for (int i = keys.Count - 1; i >= 0; i--)
                        dic.Add(keys[i], pDictionary[keys[i]]);
                }
                return dic;
            }
            else
                return pDictionary;
        }
	}

    public static class Sorts
    {

    }
}
