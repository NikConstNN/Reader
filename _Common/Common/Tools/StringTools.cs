using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Tools
{
    /// <summary>
    /// Служебные методы работы со строками
    /// </summary>
    public static class StringTools
    {
        #region Методы поиска вхождений текста, окруженного двумя символами
        /// <summary>
        /// Вернуть первое вхождение текста, окруженные символом (не включая этот символ)
        /// </summary>
        /// <param name="pText">Исходная строка</param>
        /// <param name="pChar">символ-ограничитель</param>
        /// <returns>Строка или string.Empty если не найдено</returns>
        public static string SurroundedTextFirst(string pText, char pChar)
        {
            return SurroundedTextFirst(pText, pChar, pChar);
        }
        /// <summary>
        /// Вернуть первое вхождение текста, окруженные двумя символами (не включая эти символы)
        /// </summary>
        /// <param name="pText">Исходная строка</param>
        /// <param name="pLeftChar">Левый символ-ограничитель</param>
        /// <param name="pRightChar">Правый символ-ограничитель</param>
        /// <returns>Строка или string.Empty если не найдено</returns>
        public static string SurroundedTextFirst(string pText, char pLeftChar, char pRightChar)
        {
            List<string> list = SurroundedText(pText, pLeftChar, pRightChar, 0, 1);
            if (list.Count == 0)
                return string.Empty;
            else
                return list[0];
        }
        /// <summary>
        /// Вернуть все вхождения текста, окруженные символом (не включая этот символ)
        /// </summary>
        /// <param name="pText">Исходная строка</param>
        /// <param name="pChar">символ-ограничитель</param>
        /// <returns>Список всех вхождений</returns>
        public static List<string> SurroundedText(string pText, char pChar)
        {
            return SurroundedText(pText, pChar, pChar, 0, -1);
        }
        /// <summary>
        /// Вернуть все вхождения текста, окруженные двумя символами (не включая эти символы)
        /// </summary>
        /// <param name="pText">Исходная строка</param>
        /// <param name="pLeftChar">Левый символ-ограничитель</param>
        /// <param name="pRightChar">Правый символ-ограничитель</param>
        /// <param name="pBegPos">Позиция, с которой начинается поиск</param>
        /// <param name="pCount">Количество вхождений, 0 или меньше - все</param>
        /// <returns>Список вхождений</returns>
        public static List<string> SurroundedText(string pText, char pLeftChar, char pRightChar, int pBegPos, int pCount)
        {
            List<string> res = new List<string> { };
            if (!string.IsNullOrEmpty(pText) && !string.IsNullOrEmpty(pLeftChar.ToString()))
            {
                if (string.IsNullOrEmpty(pRightChar.ToString()))
                    pRightChar = pLeftChar;
                if (pBegPos < 0)
                    pBegPos = 0;
                bool flagBeg = false;
                string ss = string.Empty;
                for (int i = pBegPos; i < pText.Length; i++)
                {
                    if (pText[i].Equals(pLeftChar))
                    {
                        flagBeg = true;
                        continue;
                    }
                    if (flagBeg)
                    {
                        if (pText[i].Equals(pRightChar))
                        {
                            res.Add(ss);
                            if (pCount > 0 && res.Count == pCount)
                                break;
                            ss = string.Empty;
                            flagBeg = false;
                        }
                        else
                            ss += pText[i].ToString();
                    }
                }
            }
            return res;        
        }
        #endregion

        /// <summary>
        /// то же самое, что ToString, но если null, то результат string.Empty
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string tostring(this object s)
        {
            if (s == null) 
                return string.Empty;
            return s.ToString();
        }

        /// <summary>
        /// Полное имя таблицы
        /// </summary>
        /// <param name="pTableName">Имя таблицы</param>
        /// <returns>Полное имя таблицы - бд.DBO.имя таблицы</returns>
        public static string ToFullTableName(this string pTableName)
        {
            if (string.IsNullOrWhiteSpace(pTableName))
                return string.Empty;
            //pTableName = pTableName.Trim();
            //string[] arr = pTableName.Split(new char[] {'.'});
            //if (arr.Length == 3)
            //    return pTableName;
            //else
            //    pTableName = arr[arr.Length - 1];
            else
                return DB_Classes.DB_Tools.FullTableName(pTableName);
        }

        /// <summary>
        /// Краткое имя таблицы
        /// </summary>
        /// <param name="pTableName">Имя таблицы</param>
        /// <returns>Краткое имя таблицы</returns>
        public static string ToShortTableName(this string pTableName)
        {
            string res = string.Empty;
            if (!string.IsNullOrWhiteSpace(pTableName))
            {
                string[] arr = pTableName.Trim().Split(new char[] { '.' });
                if (arr.Length > 0)
                {
                    res = arr[arr.Length - 1];
                }
            }
            return res;
        }
    }
}
