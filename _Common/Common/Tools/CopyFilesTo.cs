/*
 * Copyright (c) 11.2010, 2010 Information & Management Ltd. 
 *
 * NAME  cService.cs  namespace InuSys  SCOPE  
 * VERSION: 01.00.00.001                                   
 * DESCRIPTION:                                            
 *   Сервисные классы  
 *                                                          
 * AUTHOR: 
 *                                                          
 * РАЗРАБОТАН НА ОСНОВЕ ПОСТАНОВОК И ЗАМЕЧАНИЙ:            
  *
 * MODIFIED
 * 
*/
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
//using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;

namespace Common.Tools
{
    /// <summary>
    /// Копирование файлов
    /// </summary>
    public class CopyFilesTo 
    {
        private string m_Message;
        private bool m_CreateDir;
        private bool m_CopyCorrectFile;

        /// <summary>
        /// Сообщения. При успешном завершении копирования, пустая строка
        /// </summary>
        public string Message
        {
            get
            {
                return m_Message;
            }
        }

        /// <summary>
        /// Создавать папку, если отсутствует (по умолчанию true)
        /// </summary>
        public bool CreateDir
        {
            get
            {
                return m_CreateDir;
            }
            set
            {
                m_CreateDir = value;
            }

        }

        /// <summary>
        /// Копировать корректные файлы, если в списке окажутся некорректные (по умолчанию true)
        /// </summary>
        public bool CopyCorrectFile
        {
            get
            {
                return m_CopyCorrectFile;
            }
            set
            {
                m_CopyCorrectFile = value;
            }

        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public CopyFilesTo()
        {
            m_Message = string.Empty;
            m_CreateDir = true; 
            m_CopyCorrectFile = true; 
        }

        /// <summary>
        /// Копирование файлов 
        /// </summary>
        /// <param name="pPathsFileNamesFrom">Файл (путь\имя)</param>
        /// <param name="pPathTo">Путь копирования</param>
        /// <returns></returns>
        public bool RunCopyFiles(string pPathFileNameFrom, string pPathTo)
        {
            if (String.IsNullOrWhiteSpace(pPathFileNameFrom) || String.IsNullOrWhiteSpace(pPathTo))
                return false;
            List<string> lPathsFileNamesFrom = new List<string>(); 
            lPathsFileNamesFrom.Add(pPathFileNameFrom);
            return RunCopyFiles(lPathsFileNamesFrom, pPathTo); 
        }
        /// <summary>
        /// Копирование файлов 
        /// </summary>
        /// <param name="pPathsFileNamesFrom">Список файлов (путь\имя)</param>
        /// <param name="pPathTo">Путь копирования</param>
        /// <returns></returns>
        public bool RunCopyFiles(List<string> pPathsFileNamesFrom, string pPathTo)
        {
            List<string> lPathsFileNamesFrom = CheckPathAndFiles(pPathsFileNamesFrom, pPathTo);
            pPathTo = pPathTo.TrimEnd(new char[1] { '\\' });
            bool res = (lPathsFileNamesFrom.Count > 0);
            if (res)
                foreach (string s in lPathsFileNamesFrom) 
                {
                    FileInfo fi = new FileInfo(s);
                    try
                    {
                        File.Copy(s, pPathTo + "\\" + fi.Name, true);
                    }
                    catch (Exception ex)
                    {
                        m_Message = string.Format("Не удалось скопировать файл - '{0}'{1}{2}", s, Environment.NewLine, ex.Message);
                        res = false;
                    }
                }
            return res;
        }
        /// <summary>
        /// Проверка наличия файла и пути для копирование. Оценка свободного пространтства на приемнике
        /// </summary>
        /// <param name="pPathsFileNamesFrom">Файл (путь\имя)</param>
        /// <param name="pPathTo">Путь копирования</param>
        /// <returns></returns>
        public bool CheckPathAndFiles(string pPathFileNameFrom, string pPathTo)
        {
            List<string> st = new List<string>();
            st.Add(pPathFileNameFrom);
            return CheckPathAndFiles(st, pPathTo).Count > 0;
        }
        /// <summary>
        /// Проверка наличия файлов и пути для копирование. Оценка свободного пространтства на приемнике
        /// </summary>
        /// <param name="pPathsFileNamesFrom">Список файлов (путь\имя)</param>
        /// <param name="pPathTo">Путь копирования</param>
        /// <returns>Список корректных файлов или пустой если ошибка/отказ</returns>
        public List<string> CheckPathAndFiles(List<string> pPathsFileNamesFrom, string pPathTo)
        {
            List<string> res = new List<string>();
            if (pPathsFileNamesFrom == null || pPathsFileNamesFrom.Count == 0 || String.IsNullOrWhiteSpace(pPathTo))
                return res;
            StringBuilder sb = new StringBuilder();
            long DrFreeSpace = 0;
            pPathTo = pPathTo.TrimEnd(new char[1] { '\\' });
            DriveInfo di = new DriveInfo(pPathTo);
            if (di.IsReady)
                DrFreeSpace = di.TotalFreeSpace;
            else
            {
                sb.Append("Задано не существующее устройство - '" + di.Name + "'");
                sb.Append(Environment.NewLine);
                sb.Append("Возможно устройство не готово или не подключено.");
                m_Message = sb.ToString();
                return res;
            }
            long FileSpace = 0;
            sb.Append("Ошибочные файлы:");
            foreach (string s in pPathsFileNamesFrom)
            {
                if (File.Exists(s))
                {
                    FileInfo fi = new FileInfo(s);
                    if (String.Compare(pPathTo, fi.DirectoryName, true) == 0)
                    {
                        sb.Append(Environment.NewLine);
                        sb.Append(s + " - копирование в себя");
                    }
                    else //ok
                    {
                        res.Add(s);
                        FileSpace = FileSpace + fi.Length;
                    }
                }
                else
                {
                    sb.Append(Environment.NewLine);
                    sb.Append(s + " - файла не существует");
                }
            }
            if (res.Count == 0)
            {
                m_Message = sb.ToString();
                return res;
            }
            else
            {
                if (res.Count != pPathsFileNamesFrom.Count)
                {
                    m_Message = sb.ToString();
                    if (!m_CopyCorrectFile)
                    {
                        res.Clear();
                        return res;
                    }
                }
                //Оценка свободного места
                if (DrFreeSpace < FileSpace + 1024)
                {
                    Double free = (DrFreeSpace / 1024) / 1024;
                    Double a = (FileSpace / 1024) / 1024;
                    m_Message += Environment.NewLine + string.Format("Недостаточно места для копирования, устройство - '{0}'{1}Требуется - {2} MБ{3}Свободно  - {4} MБ"
                        , new object[] { di.Name, Environment.NewLine, a.ToString("#.##"), Environment.NewLine, free.ToString("#.##") });
                    res.Clear();
                    return res;
                }
            }
            if (!Directory.Exists(pPathTo))
            {

                if (m_CreateDir)
                {
                    try
                    {
                        Directory.CreateDirectory(pPathTo);
                    }
                    catch (Exception ex)
                    {
                        m_Message += Environment.NewLine + string.Format("Не удалось создать папку - '{0}'{1}{2}", pPathTo, Environment.NewLine, ex.Message);
                        res.Clear();
                    }
                }
                else
                {
                    res.Clear();
                    m_Message += string.Format("Заданного пути - '{0}' не существует", pPathTo);
                }
            }
            return res;        
        }
    }
}