using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// Именованный список обьектов
    /// </summary>
    public class NameObjects : DictionaryBase, IDisposable
    {
        List<string> _NamesList;

        /// <summary>
        /// Конструктор. Создает пустой объект
        /// </summary>
        public NameObjects() : this(new string[] { }, new object[] { }) { }
        /// <summary>
        /// Конструктор. Создает объект и заполняет его значениями
        /// </summary>
        /// <param name="pNames">Строка-список имен через ';'</param>
        /// <param name="pObjects">Массив значений</param>
        public NameObjects(string pNames, object[] pObjects)
        {
            _NamesList = new List<string>();
            FillObject(pNames, pObjects);
        }
        /// <summary>
        /// Конструктор. Создает объект и заполняет его значениями
        /// </summary>
        /// <param name="pNames">Массив имен</param>
        /// <param name="pObjects">Массив значений</param>
        public NameObjects(string[] pNames, object [] pObjects)
        {
            _NamesList = new List<string>();
            FillObject(pNames, pObjects);
        }
        private void FillObject(string pNames, object[] pObjects)
        {
            string[]? _Names = null;
            if (!string.IsNullOrWhiteSpace(pNames))
            {
                _Names = pNames.Split(new char[] { ';' });
            }
            FillObject(_Names, pObjects);
        }

        private void FillObject(string[]? pNames, object[] pObjects)
        {
            if (pNames != null && pNames.Length > 0)
            {
                int _CountObj = pObjects == null ? 0 : pObjects.Length;
                for (int i = 0; i < pNames.Length; i++)
                    this.Add(pNames[i], i >= _CountObj ? null : pObjects[i]);
            }
        }

        public new IEnumerator GetEnumerator()
        {
            return new NameObjectsEnum(Dictionary, _NamesList);
        }
        /// <summary>
        /// Объект по имени
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <returns>Объект</returns>
        public object this[string Name]
        {
            get
            {
                return Dictionary[Name];
            }
            set
            {
                if (Contains(Name))                    
                    Dictionary[Name] = value;
                else
                    this.Add(Name, value);
            }
        }
        /// <summary>
        /// Объект по индексу
        /// </summary>
        /// <param name="Index">Индекс</param>
        /// <returns>Объект</returns>
        public object this[int Index]
        {
            get
            {
                if (Contains(Index))
                    return Dictionary[_NamesList[Index]];
                else
                    return null;
            }
            set
            {
                if (Contains(Index))
                    Dictionary[_NamesList[Index]] = value;
                else
                    throw new ArgumentException(string.Format("В списке отсутствует запись с индексом '{0}'", Index), "Index");
            }
        }
        /*
        /// <summary>
        /// Имена списка в виде коллекции
        /// </summary>
        public ICollection Names
        {
            get
            {
                return (Dictionary.Keys);
            }
        }
        /// <summary>
        /// Объекты списка в виде коллекции
        /// </summary>
        public ICollection Objects
        {
            get
            {
                return (Dictionary.Values);
            }
        }
         */ 
        /// <summary>
        /// Cписок имен
        /// </summary>
        public List<string> NamesList
        {
            get
            {                
                return _NamesList;
            }
        }
        /// <summary>
        /// Добавить запись
        /// </summary>
        /// <param name="pName">Имя</param>
        /// <param name="pObject">Объект</param>
        public void Add(string pName, object pObject)
        {
            Dictionary.Add(pName, pObject);
            _NamesList.Add(pName);
        }
        /// <summary>
        /// Добавить записи
        /// </summary>
        /// <param name="pNames">Массив имен</param>
        /// <param name="pObjects">Массив значений</param>
        public void AddRange(string[] pNames, object[] pObjects)
        {
            FillObject(pNames, pObjects);
        }
        /// <summary>
        /// Добавить записи
        /// </summary>
        /// <param name="pNames">Строка-список имен через ';'</param>
        /// <param name="pObjects">Массив значений</param>
        public void AddRange(string pNames, object[] pObjects)
        {
            FillObject(pNames, pObjects);
        }
        /// <summary>
        /// Имеется ли в списке элемент с указаным именем
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <returns></returns>
        public bool Contains(string Name)
        {
            return (Dictionary.Contains(Name));
        }
        /// <summary>
        /// Имеется ли в списке элемент с указаным индексом
        /// </summary>
        /// <param name="Index">Индекс</param>
        /// <returns></returns>
        public bool Contains(int Index)
        {
            if (Index >= 0 && Index < _NamesList.Count)
            {
                return Dictionary.Contains(_NamesList[Index]);
            }
            else 
                return false;
        }
        /// <summary>
        /// удалить элемент по имени
        /// </summary>
        /// <param name="Name"></param>
        public void Remove(string Name)
        {
            Dictionary.Remove(Name);
            _NamesList.Remove(Name);
        }
        /// <summary>
        /// Удалить элемент по индексу
        /// </summary>
        /// <param name="Index"></param>
        public void Remove(int Index)
        {
            if (Contains(Index))
            {
                Dictionary.Remove(_NamesList[Index]);
                _NamesList.Remove(_NamesList[Index]);
            }
        }
        /// <summary>
        /// Объкты списка в виде массива
        /// </summary>
        /// <returns></returns>
        public object[] ToArrayObjects()
        {
            object[] result = new object[_NamesList.Count];
            for (int i = 0; i < _NamesList.Count; i++)
                result[i] = Dictionary[_NamesList[i]];
            return result;
        }
        /// <summary>
        /// Имена списка в виде массива
        /// </summary>
        /// <returns></returns>
        public string[] ToArrayNames()
        {
            return _NamesList.ToArray();
        }
        /// <summary>
        /// Индекс по имени
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <returns></returns>
        public int IndexOfName(string Name)
        {
            return _NamesList.IndexOf(Name);
        }
        /// <summary>
        /// Индекс по объекту
        /// </summary>
        /// <param name="Object">Объект</param>
        /// <returns></returns>
        public int IndexOfObject(object Object)
        {
            object[] arr = this.ToArrayObjects();
            for (int i = 0; i < arr.Length; i++)
                if (arr[i].Equals(Object))
                    return i;
            return -1;
        }
        /// <summary>
        /// Содержит ли список заданный объект.
        /// </summary>
        /// <param name="Object">объект</param>
        /// <returns></returns>
        public bool ContainsObject(object Object)
        {
            //ICollection vals = this.Objects;
            foreach (object ob in Dictionary.Values)
                if (ob.Equals(Object))
                    return true;
            return false;
        }
        /// <summary>
        /// Сортировать по именам
        /// </summary>
        public void SortByNames()
        {
            _NamesList.Sort();
        }
        protected override void OnClear()
        {
            base.OnClear();
            _NamesList.Clear();
        }

        #region интерфейс IDisposable

        /// <summary>
        /// Очистка.
        /// </summary>
        public virtual void Dispose()
        {
            if (_NamesList != null)
            {
                _NamesList.Clear();
                _NamesList = null;
            }
            Dictionary.Clear();
            System.GC.SuppressFinalize(this);			
        }

        #endregion
    }
    /// <summary>
    /// Перечислитель
    /// </summary>
    public class NameObjectsEnum : IEnumerator
    {
        public IDictionary _Dictionary;
        public List<string> _ListNames;

        int position = -1;

        public NameObjectsEnum(IDictionary Dictionary, List<string> ListNames)
        {
            _Dictionary = Dictionary;
            _ListNames = ListNames;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _ListNames.Count);
        }

        public void Reset()
        {
            position = -1;
        }

        public object Current
        {
            get
            {
                try
                {
                    string _key = _ListNames[position];
                    object _val = _Dictionary[_key];
                    return new KeyValuePair<string,object>(_key, _val);
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }

}
