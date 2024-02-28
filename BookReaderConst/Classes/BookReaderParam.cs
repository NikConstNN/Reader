using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Common;

namespace BookReaderConst
{
    /// <summary>
    /// Параметры BookReaderConst
    /// </summary>
    public class BookReaderParam
    {
        /// <summary>
        /// Рабочая папка
        /// </summary>
        public string WorkDir { get; set; } = "";

        Font? m_DefFont;
        [JsonIgnore]
        /// <summary>
        /// Шрифт по умолчанию
        /// </summary>
        public Font DefFont
        {
            get
            {
                if (m_DefFont == null) 
                {
                    m_DefFont = new Font(DefFontName, DefFontSize, DefFontStyle, DefUnit, DefFontCharset);
                }
                return m_DefFont;
            }
            set
            { 
                if (m_DefFont != value) 
                { 
                    if (m_DefFont != null)
                        m_DefFont.Dispose();
                    m_DefFont = value;
                    if (m_DefFont != null)
                    {
                        DefFontCharset = m_DefFont.GdiCharSet;
                        DefFontName = m_DefFont.Name;
                        DefFontSize = m_DefFont.Size;
                        DefFontStyle = m_DefFont.Style;
                        DefUnit = m_DefFont.Unit;
                    }
                }
            } 
        }

        #region Составные шрифта
        public byte DefFontCharset { get; set; } = 204;
        public string DefFontName { get; set; } = "Times New Roman";
        public float DefFontSize { get; set; } = 16;
        public FontStyle DefFontStyle { get; set; } = FontStyle.Regular;

        public GraphicsUnit DefUnit { get; set; } = GraphicsUnit.Pixel;
        #endregion

        Color m_DefColor = Color.Empty;
        [JsonIgnore]
        /// <summary>
        /// Цвет фона по умолчанию
        /// </summary>
        public Color DefColor
        {
            get
            {
                if (m_DefColor.IsEmpty)
                {
                    m_DefColor = Color.FromArgb(A, R, G, B);
                }
                return m_DefColor;
            }
            set
            {
                if (m_DefColor.IsEmpty || !m_DefColor.Equals(value))
                {
                    m_DefColor = value;
                    if (!m_DefColor.IsEmpty)
                    {
                        A = m_DefColor.A;
                        R = m_DefColor.R;
                        G = m_DefColor.G;
                        B = m_DefColor.B;
                    }
                }
            }
        }
        #region Составные цвета
        public int R { get; set; } = 255;
        public int G { get; set; } = 255;
        public int B { get; set; } = 255;
        public int A { get; set; } = 255;
        #endregion

        /// <summary>
        /// Повторное открытие открытого файла. true - открыть заново, false - просто перейти на закладку. По ум. false.
        /// </summary>
        public bool OpenFileAgain { get; set; } = false;

        public bool SaveWorkDir { get; set; } = true;
        /// <summary>
        /// При старте открывать последний файл
        /// </summary>
        public bool OpenLastFile { get; set; } = true;
        /// <summary>
        /// Что-то с кодировкой ???
        /// </summary>
        public bool SaveDecode { get; set; } = true;
        /// <summary>
        /// При открытии файла переходить на последнюю закладку
        /// </summary>
        public bool GoMark { get; set; } = true;
        /// <summary>
        /// При старте форму открыть на весь экран
        /// </summary>
        public bool FormMax { get; set; } = false;
        /// <summary>
        /// Показывать поля Дата/Время
        /// </summary>
        public bool VisDateTime { get; set; } = true;
        /// <summary>
        /// Показавать StatusBar
        /// </summary>
        public bool VisStatusBar { get; set; } = true;
        /// <summary>
        /// Показавать ToolBar
        /// </summary>
        public bool VisToolBar { get; set; } = true;
        /// <summary>
        /// При закрытии файла установить закладку
        /// </summary>
        public bool SetCloseMark { get; set; } = true;
        /// <summary>
        /// Итнервал очистки устаревших данных (дней). По ум. 3, 0 - данные очищаются при каждом старте, -1 данные не очищаются
        /// </summary>
        public int ClearInt { get; set; } = 3;
        /// <summary>
        /// Дата последней очистка
        /// </summary>
        public DateTime LastClear { get; set; } = DateTime.Now;
        /// <summary>
        /// Помнить последнии файлы. Ум. 10, 0 - не запоминаем.
        /// </summary>
        public int LastFilesCount { get; set; } = 10;
        /// <summary>
        /// Позиция формы при старте
        /// </summary>
        public Point FormLocation { get; set; } = new Point(20, 20);
        /// <summary>
        /// Размер формы при старте
        /// </summary>
        public Size FormSize { get; set; } = new Size(1000, 800);

        #region Файлы приложения
        /// <summary>
        /// Файлы, которые были окрыты приложением
        /// </summary>
        public Dictionary<string, FileParam> LastFiles  { get; set; }  = new Dictionary<string, FileParam>();
        [JsonIgnore]
        /// <summary>
        /// Id фaйлов открытых в данный момент
        /// </summary>
        public List<string> CurrentFiles { get; set; } = new List<string>();
        /// <summary>
        /// Id текущего файла
        /// </summary>
        public string LastFileID { get; set; } = "";
        [JsonIgnore]
        /// <summary>
        /// Текущий файл
        /// </summary>
        public FileParam? LastFile { get; set; }
        #endregion

        #region Форма FormFind
        /// <summary>
        /// Фрагменты текста для поиска, не более 30
        /// </summary>
        public List<string> FindText { get; set; } = new List<string>();
        /// <summary>
        /// Позиция формы
        /// </summary>
        public Point FormFindLocation { get; set; } = new Point(0, 0);
        /// <summary>
        /// Слово целиком
        /// </summary>
        public bool FlagFullWord { get; set; } = false;
        /// <summary>
        /// Учет регистра
        /// </summary>
        public bool FlagCase { get; set; } = false;
        /// <summary>
        /// Зациклить поиск
        /// </summary>
        public bool FlagCh { get; set; } = false;
        #endregion

        [JsonIgnore]
        /// <summary>
        /// Параметры новые 
        /// </summary>
        public bool IsNew { get; set; }
        /// <summary>
        /// Конструктор
        /// </summary>
        public BookReaderParam() 
        {
            IsNew = true;
        }

        /// <summary>
        /// Прочитать сохраненные параметры. Если нет или неудачное чтение создает новый объект.
        /// </summary>
        /// <returns>Обект Параметры. Если не найден, создается новый.</returns>
        public static BookReaderParam ReadParams()
        {
            BookReaderParam? res = null;
            try
            {
                if (File.Exists("BookReaderParam.json"))
                {
                    string param = File.ReadAllText("BookReaderParam.json");
                    res = JsonSerializer.Deserialize<BookReaderParam>(param);
                }
            }
            catch (Exception ex)
            {
                res = null;
            }
            if (res == null)
                res = new BookReaderParam();
            else
                res.IsNew = false;
            return res;
        }
        /// <summary>
        /// Сохранить параметры
        /// </summary>
        /// <param name="pParam">параметры</param>
        /// <returns>ОК/не ОК</returns>
        public static bool  WriteParams(BookReaderParam pParam)
        {
            bool res = false;
            if (pParam != null)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string paramJson = JsonSerializer.Serialize(pParam, typeof(BookReaderParam), options);
                StreamWriter file = File.CreateText("BookReaderParam.json");
                file.WriteLine(paramJson);
                file.Close();
                file.Dispose();
            }
            return res;
        }

        /// <summary>
        /// Добавить в список новый файл или если есть просто сделать его текущим
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FileOpenStaus AddFile(string fileName)
        {
            FileOpenStaus res = FileOpenStaus.AlreadyOpened;
            if (File.Exists(fileName))
            {
                FileParam? curr = FindFileByName(fileName);
                if (curr == null)
                {
                    res = FileOpenStaus.FirstOpen;
                    curr = new FileParam(fileName);

                    List<FileParam> fileParams = LastFiles.Values.ToList();
                    fileParams.Insert(0, curr);
                    LastFiles.Clear();
                    for (int i = 0; i < fileParams.Count; i++)
                        LastFiles.Add(fileParams[i].ID, fileParams[i]);
                    //LastFiles.Add(curr.ID, curr);
                }
                if (CurrentFiles.Contains(curr.ID))
                {
                    res = FileOpenStaus.Opened;
                }
                else
                    CurrentFiles.Insert(0, curr.ID);
                //SetCurrentFileByID(curr.ID);
                LastFile = curr;
                LastFileID = curr.ID;
            }
            else
                res = FileOpenStaus.ErrorNotFind;
            return res;
        }

        public void SetCurrentFileByID(string fileID)
        {
            if (!string.IsNullOrWhiteSpace(fileID))
            {
                if (!fileID.Equals(LastFileID))
                {
                    if (LastFiles.TryGetValue(fileID, out FileParam? curr0) && curr0 != null)
                    {
                        LastFile = curr0;
                        LastFileID = fileID;
                    }
                    else
                    {
                        LastFile = null;
                        LastFileID = "";
                    }
                }
            }
            else 
            {
                LastFile = null;
                LastFileID = "";
            }
        }


        /// <summary>
        /// Удалить файл из списка по имени файла
        /// </summary>
        /// <param name="pFileName"></param>
        public void DeleteFile(string pFileName)
        {
            FileParam? file = FindFileByName(pFileName);
            if (file != null)
                DeleteFileById(file.ID);
        }
        /// <summary>
        /// Удалить файл из списка
        /// </summary>
        /// <param name="pFile"></param>
        public void DeleteFile(FileParam pFile)
        {
            if (pFile != null)
                DeleteFileById(pFile.ID);
        }
        /// <summary>
        /// Удалить файл из списка по ID файла
        /// </summary>
        /// <param name="pFileID"></param>
        public void DeleteFileById(string pFileID)
        {
            if (!string.IsNullOrWhiteSpace(pFileID) && LastFiles.Keys.Contains(pFileID))
            {
                LastFiles.Remove(pFileID);
                if (CurrentFiles.Contains(pFileID))
                    CurrentFiles.Remove(pFileID);
            }
            if (LastFile != null && LastFile.ID.Equals(pFileID))
            {
                LastFile = null;
                LastFileID = "";
            }
        }
        /// <summary>
        /// Ревизия списка. Удаляет несуществующие файлы
        /// </summary>
        public void RevisionList()
        {
            if (ClearInt >= 0)
            {
                int inr = ClearInt;
                DateTime dt = DateTime.Now;
                if (ClearInt > 0)
                    inr = (dt - LastClear).Days;
                if (inr >= ClearInt)
                {
                    List<string> ldel = new();
                    foreach (FileParam item in LastFiles.Values)
                    {
                        if (!File.Exists(item.FileName))
                            ldel.Add(item.ID);
                    }
                    foreach (var item in ldel)
                    {
                        DeleteFileById(item);
                    }
                    LastClear = dt;
                }
            }
        }

        public FileParam? FindFileByName(string fileName)
        {
            FileParam? curr = null;
            foreach (FileParam item in LastFiles.Values)
            {
                if (item.FileName.Equals(fileName))
                {
                    curr = item;
                    break;
                }
            }
            return curr;
        }

        public FileParam? FindFileByID(string fileID = "")
        {
            if (string.IsNullOrWhiteSpace(fileID))
                fileID = LastFileID;
            LastFiles.TryGetValue(fileID, out FileParam? curr0);
            return curr0;
        }
    }

    /// <summary>
    /// Параметры книги
    /// </summary>
    public class FileParam
    {
        public string FileName { get; set; }
        public string FileNameShort { get; set; }
        public string ID { get; set; } //= Guid.NewGuid().ToString();

        /// <summary>
        /// Строк на странице книги
        /// </summary>
        [JsonIgnore]
        public double LinesOfPage { get; set; } = 0.0;

        Font? m_FileFont;
        [JsonIgnore]
        /// <summary>
        /// Шрифт файла
        /// </summary>
        public Font? FileFont
        {
            get
            {
                if (m_FileFont == null && FileFontName.Length > 0)
                {
                    m_FileFont = new Font(FileFontName, FileFontSize, FileFontStyle, FileUnit, FileFontCharset);
                }
                return m_FileFont;
            }
            set
            {
                if (m_FileFont == null || !m_FileFont.Equals(value))
                {
                    if (m_FileFont != null)
                        m_FileFont.Dispose();
                    m_FileFont = value;
                    if (m_FileFont != null)
                    {
                        FileFontCharset = m_FileFont.GdiCharSet;
                        FileFontName = m_FileFont.Name;
                        FileFontSize = m_FileFont.Size;
                        FileFontStyle = m_FileFont.Style;
                        FileUnit = m_FileFont.Unit;
                    }
                    else
                        FileFontName = "";
                }
            }
        }

        #region Составные шрифта
        public byte FileFontCharset { get; set; } 
        public string FileFontName { get; set; } 
        public float FileFontSize { get; set; }
        public FontStyle FileFontStyle { get; set; }
        public GraphicsUnit FileUnit { get; set; }
        #endregion


        Color m_FileColor = Color.Empty;
        [JsonIgnore]
        /// <summary>
        /// Цвет фона по умолчанию
        /// </summary>
        public Color FileColor
        {
            get
            {
                if (m_FileColor.IsEmpty && FA > 0)
                {
                    m_FileColor = Color.FromArgb(FA, FR, FG, FB);
                }
                return m_FileColor;
            }
            set
            {
                if (m_FileColor.IsEmpty || !m_FileColor.Equals(value))
                {
                    m_FileColor = value;
                    if (!m_FileColor.IsEmpty)
                    {
                        FA = m_FileColor.A;
                        FR = m_FileColor.R;
                        FG = m_FileColor.G;
                        FB = m_FileColor.B;
                    }
                    else
                        FA = 0;
                }
            }
        }
        #region Составные цвета
        public int FR { get; set; } = 0;
        public int FG { get; set; } = 0;
        public int FB { get; set; } = 0;
        public int FA { get; set; } = 0;
        #endregion

        /// <summary>
        /// Закладки
        /// </summary>
        public Dictionary<long, string> Marks { get; set; }
        /// <summary>
        /// Закладка - позиция закрытия файла
        /// </summary>
        public long MarkClose { get; set; }
        /// <summary>
        /// Последняя добавленная закладка
        /// </summary>
        public long MarkLast { get; set; }

        public FileParam(string fileName)
        {
            FileName = fileName;
            FileNameShort = Path.GetFileName(fileName);
            FileFontName = "";
            Marks = new Dictionary<long, string>();
            ID = Guid.NewGuid().ToString();
            MarkClose = -1;
            MarkLast = -1;
        }

        public void AddMark(long pPos, string pText)
        {
            MarkLast = pPos;
            if (!Marks.ContainsKey(pPos))
            {
                Marks.Add(pPos, pText);
                var res = ComparerByValueProperty.SortDictionaryByKey(Marks);
                if (res != null)
                    Marks = res;
                //if (Marks.Count > 1)
                //{
                //    var list = Marks.Keys.ToList();
                //    list.Sort();
                //    Dictionary<long, string>? marks = new();
                //    for (int i = 0; i < list.Count; i++)
                //        marks.Add(list[i], Marks[list[i]]);
                //    Marks.Clear();
                //    foreach (var item in marks)
                //        Marks.Add(item.Key, item.Value);
                //    marks.Clear();
                //    marks = null;
                //}
            }
        }

        public bool DeleteMark(long pKey, bool pAll = false)
        {
            bool res = false;
            if (pAll)
            {
                res = Marks.Count > 0;
                MarkLast = -1;
                Marks.Clear();
            }
            else if (Marks.ContainsKey(pKey))
            {
                Marks.Remove(pKey);
                res = true;
                if (pKey == MarkLast)
                {
                    if (Marks.Count > 0)
                        MarkLast = Marks.FirstOrDefault().Key;
                    else
                        MarkLast = -1;
                }
            }
            return res;
        }
    }

    /// <summary>
    /// Возможные варианты открытия файла
    /// </summary>
    public enum FileOpenStaus
    {
        /// <summary>
        /// Ошибка, файл не найден
        /// </summary>
        ErrorNotFind = -2,
        /// <summary>
        /// Ошибка чтения файла
        /// </summary>
        ErrorRead = -1,
        /// <summary>
        /// Файл открывается впервые
        /// </summary>
        FirstOpen = 0,
        /// <summary>
        /// Файл уже открывался в других сеансах
        /// </summary>
        AlreadyOpened = 1,
        /// <summary>
        /// Файл уже открыт
        /// </summary>
        Opened = 2
    }
}
