using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;
//using SdaWin.Visual;
using EX = Microsoft.Office.Interop.Excel;

namespace VisualWinForm.Visual.Tools
{
    /// <summary>
    /// Excel Application. Export to Excel
    /// </summary>
    class ExcelApplic
    {
        #region Class Members

        private EX.Application m_excelApp;
        //private EX.Worksheet m_CurrentWorksheet;
        //private EX.Workbook m_CurrentWorkbook;
        private string m_sheetName;
        private object[,] m_excelArray;
        private DataGridView m_grid;
        private int[,] m_dataPropertyTypesInfo;
        private CultureInfo m_cultureInfo;
        private IWin32Window m_Owner;
        #endregion
        /// <summary>
        /// Creates a new instance of the ExcelApplic class.
        /// </summary>
        /// <param name="aOwner">Form the owner of the grid</param>
        public ExcelApplic(IWin32Window aOwner)
        {
            m_Owner = aOwner;
        }
        /// <summary>
        /// Creates a new instance of the ExcelApplic class.
        /// </summary>
        public ExcelApplic() : this(null) { }

        /// <summary>
        /// Get or set Worksheet name
        /// </summary>
        public string SheetName
        {
            get
            {
                return m_sheetName;
            }
            set
            {
                m_sheetName = value;
                if (!string.IsNullOrEmpty(m_sheetName))
                {
                    if (m_sheetName.Length > 31)
                        m_sheetName = m_sheetName.Substring(0, 31);
                    m_sheetName = m_sheetName.Replace(@"\", "_");
                    m_sheetName = m_sheetName.Replace("/", "_");
                    m_sheetName = m_sheetName.Replace("?", "_");
                    m_sheetName = m_sheetName.Replace("*", "_");
                    m_sheetName = m_sheetName.Replace("[", "_");
                    m_sheetName = m_sheetName.Replace("]", "_");
                }
            }
        }

        /// <summary>
        /// Run export to Excel
        /// </summary>
        public void RunExportToExcel(DataGridView aGrid)
        {
            m_grid = aGrid;
            if (m_grid == null || m_grid.RowCount == 0 || m_grid.ColumnCount == 0) //?????
            {
                ControlTools.ShowMessageExt(m_Owner, "[I]Пустая сетка. Операция не выполняется.", "Экспорт данных в Excel", string.Empty, 0);
                return;
            }
            Cursor cursor = m_grid.Cursor;
            m_grid.Cursor = Cursors.WaitCursor;
            m_cultureInfo = CultureInfo.CurrentCulture;
            m_excelApp = new EX.Application();
            try
            {
                string[] titles = new string[m_grid.ColumnCount];
                m_dataPropertyTypesInfo = new int[m_grid.ColumnCount, 2];
                //PropertyInfo[] prop = ((IList)datasource)[0].GetType().GetProperties();
                EX.Workbook wb = m_excelApp.Workbooks.Add(Type.Missing);
                if (m_excelApp.Workbooks.Count == 0) return;
                EX.Worksheet ws = (EX.Worksheet)wb.Worksheets.get_Item(1);
                if (m_grid.RowCount > ws.Rows.Count || m_grid.ColumnCount > ws.Columns.Count)
                {
                    ControlTools.ShowMessageExt(m_Owner, string.Format("[W]Слишком много записей ({0}) или колонок ({1}) в сетке.{2}Excel ver {3}.{4}Записей должно быть меньше {5}{6}Колонок должно быть меньше {7}"
                        , new object[] { m_grid.RowCount
                                        ,m_grid.ColumnCount 
                                        ,Environment.NewLine 
                                        ,m_excelApp.Version 
                                        ,Environment.NewLine 
                                        ,ws.Rows.Count + 1
                                        ,Environment.NewLine 
                                        ,ws.Columns.Count + 1 
                                       }), "Экспорт данных в Excel", string.Empty, 0);
                    return;
                }
                for (int i = 0; i < m_grid.ColumnCount; i++)
                {
                    titles[i] = m_grid.Columns[i].HeaderText;
                    m_dataPropertyTypesInfo[i, 0] = -1;
                    m_dataPropertyTypesInfo[i, 1] = -1;
                }
                ArrayCreate();
                FormatWorksheets(ws);
                //ws.get_Range(ws.Cells[1, 1], ws.Cells[1, m_grid.ColumnCount]).Value2 = titles;
                ws.Range[ws.Cells[1, 1], ws.Cells[1, m_grid.ColumnCount]].Value2 = titles;
                //ws.get_Range(ws.Cells[2, 1], ws.Cells[m_grid.RowCount + 1, m_grid.ColumnCount]).Value2 = m_excelArray;
                ws.Range[ws.Cells[2, 1], ws.Cells[m_grid.RowCount + 1, m_grid.ColumnCount]].Value2 = m_excelArray;
                ws.Columns.AutoFit();
                m_excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                ControlTools.ShowMessageExt(m_Owner, string.Format("[E]Ошибка экспорта в Excel.{0}{1}", Environment.NewLine, ex.Message)
                    , "Экспорт данных в Excel", string.Empty, 0);
                m_excelApp.Quit();
            }
            finally
            {
                m_grid.Cursor = cursor;
                Free();
            }                       
        }

        private void SetSheetName()
        {
            if (string.IsNullOrEmpty(m_sheetName))
            {
                string sheelname = string.Empty;
                object lParent = m_grid;
                while (string.IsNullOrEmpty(sheelname) && lParent != null && lParent is Control)
                {
                    sheelname = ((Control)lParent).Text;
                    lParent = ((Control)lParent).Parent;
                }
                SheetName = sheelname;
            }
        }

        private void FormatWorksheets(EX.Worksheet ws)
        {
            int savedCult = Thread.CurrentThread.CurrentCulture.LCID;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(0x0409, false); //en-US
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(0x0409, false);
                SetSheetName();
                if (!string.IsNullOrEmpty(m_sheetName))
                    ws.Name = m_sheetName;                   
                //ws.get_Range(ws.Cells[1, 1], ws.Cells[1, m_grid.ColumnCount]).Font.FontStyle = "Bold";
                ws.Range[ws.Cells[1, 1], ws.Cells[1, m_grid.ColumnCount]].Font.FontStyle = "Bold";
                //ws.get_Range(ws.Cells[1, 1], ws.Cells[1, m_grid.ColumnCount]).WrapText = true;
                ws.Range[ws.Cells[1, 1], ws.Cells[1, m_grid.ColumnCount]].WrapText = true;
                m_excelApp.ActiveWindow.SplitRow = 1;
                //m_excelApp.ActiveWindow.SplitColumn = 1;
                m_excelApp.ActiveWindow.FreezePanes = true;

                for (int i = 0; i < m_grid.ColumnCount; i++)
                {
                    string lFormat = string.Empty;
                    switch (m_dataPropertyTypesInfo[i, 0])
                    {
                        case 0: //тип не определен, возможно объект
                        case 1: //string
                        case 2: //bool ???
                        case 3: //Enum ???
                            lFormat = "@";
                            break;
                        //case 2: //bool
                        //case 3: //Enum
                        case 4: //int, Int16, Int64, long
                            lFormat = "0";
                            break;
                        case 5: //DateTime
                            lFormat = m_cultureInfo.DateTimeFormat.ShortDatePattern;
                            //lFormat = "MM/dd/yyyy";
                            //lFormat = "dd.MM.YYYY";
                            if (m_dataPropertyTypesInfo[i, 1] > 0)
                                lFormat += " " + m_cultureInfo.DateTimeFormat.ShortTimePattern; 
                            //    lFormat += " hh:mm:ss";
                            //    lFormat += " hh:mm:ss";
                            break;
                        case 6: //float, double, decimal
                            lFormat = "0."; // + m_cultureInfo.NumberFormat.NumberDecimalSeparator;
                            int len = m_dataPropertyTypesInfo[i, 1] > 0 ? m_dataPropertyTypesInfo[i, 1] : m_cultureInfo.NumberFormat.NumberDecimalDigits;
                            for (int k = 0; k < len; k++)
                                lFormat += "0";
                            break;
                    }
                    if (!string.IsNullOrEmpty(lFormat))
                        ws.Range[ws.Cells[2, i + 1], ws.Cells[m_grid.RowCount + 1, i + 1]].NumberFormat = lFormat;
                        //ws.get_Range(ws.Cells[2, i + 1], ws.Cells[m_grid.RowCount + 1, i + 1]).NumberFormat = lFormat;
                }
                object begData = ws.Cells[2, 1];
                object endData = ws.Cells[m_grid.RowCount + 1, m_grid.ColumnCount];
                /*
                ws.get_Range(begData, endData).Font.FontStyle = m_grid.Font.Style;
                ws.get_Range(begData, endData).Font.Size = m_grid.Font.Size;
                ws.get_Range(begData, endData).Font.Name = m_grid.Font.FontFamily.Name;
                */
                ws.Range[begData, endData].Font.FontStyle = m_grid.Font.Style;
                ws.Range[begData, endData].Font.Size = m_grid.Font.Size;
                ws.Range[begData, endData].Font.Name = m_grid.Font.FontFamily.Name;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(savedCult, true);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(savedCult, true);
                Application.DoEvents();
            }
        }

        private void ArrayCreate()
        {
            m_excelArray = (object[,])Array.CreateInstance(typeof(object)
                , new int[2] { m_grid.RowCount, m_grid.ColumnCount }
                , new int[2] { 1, 1 });
            for (int i = 0; i < m_grid.Rows.Count; i++)
            {
                for (int j = 0; j < m_grid.Columns.Count; j++)
                {
                    object val = m_grid.Rows[i].Cells[j].Value;
                    if (val != null)
                    {
                        if (m_dataPropertyTypesInfo[j, 0] < 0)
                        {
                            int lType = 0;
                            if (val.GetType() == typeof(string))
                                lType = 1;
                            else if (val.GetType() == typeof(bool))
                                lType = 2;
                            else if (val.GetType().BaseType == typeof(Enum))
                                lType = 3;
                            else if (val.GetType() == typeof(int)
                                    || val.GetType() == typeof(Int16)
                                    || val.GetType() == typeof(Int64)
                                    || val.GetType() == typeof(long))
                                lType = 4;
                            else if (val.GetType() == typeof(DateTime))
                                lType = 5;
                            else if (val.GetType() == typeof(float)
                                    || val.GetType() == typeof(double)
                                    || val.GetType() == typeof(decimal))
                                lType = 6;
                            m_dataPropertyTypesInfo[j, 0] = lType;
                        }
                        switch (m_dataPropertyTypesInfo[j, 0])
                        {
                            case 0: //тип не определен, возможно объект
                                val = val.ToString();
                                break;
                            //case 2: //bool
                            //    val = (bool)val ? 1 : 0; // "Yes" : "No"; //???
                            //    break;
                            case 3: //Enum
                                val = val.ToString(); //???
                                break;
                            case 5: //DateTime
                                if (m_dataPropertyTypesInfo[j, 1] < 0)
                                {
                                    DateTime dt = (DateTime)val;
                                    DateTime dtn = new DateTime(dt.Year, dt.Month, dt.Day);
                                    if (dt.Equals(dtn))
                                        m_dataPropertyTypesInfo[j, 1] = 0;
                                    else
                                        m_dataPropertyTypesInfo[j, 1] = 1;
                                }
                                break;
                            case 6: //float, double, decimal
                                if (m_dataPropertyTypesInfo[j, 1] < 0)
                                {
                                    string s = val.ToString();
                                    int pos = (int)s.IndexOf(m_cultureInfo.NumberFormat.NumberDecimalSeparator);
                                    int len = 0;
                                    if (pos >= 0)
                                        len = s.Substring(pos + 1).Length;
                                    if (len > m_dataPropertyTypesInfo[j, 1])
                                        m_dataPropertyTypesInfo[j, 1] = len;
                                }
                                break;
                        }
                        m_excelArray[i + 1, j + 1] = val;
                    }
                }
            }
        }

        private void Free()
        {
            m_excelApp = null;
            if (m_excelArray != null)
                m_excelArray = null;
            GC.GetTotalMemory(true);
        }
        /*
        #region
        public ContextMenuStrip CreateGridContextMenu() 
        {
            return null;
        }

        #endregion
         */ 
    }
}
