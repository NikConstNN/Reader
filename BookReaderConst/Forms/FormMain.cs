using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using VisualWinForm.Visual.Tools;
using VisualWinForm.Visual.Components;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;
using System.Xml.Linq;
using System;
using System.ComponentModel;
//using Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Interop.Excel;
//using static System.Net.Mime.MediaTypeNames;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//using System. .Documents;

namespace BookReaderConst
{
    public partial class MainForm : Form
    {
        System.Windows.Forms.Timer? m_timer;

        System.Windows.Forms.Timer? m_timerInfo;

        Dictionary<string, List<Control>> m_ListTextBox;
        private System.Drawing.Drawing2D.GraphicsPath m_mousePath;
        ContextMenuStrip m_ContextMenuStrip;

        BookReaderParam? m_CurrentParams;
        BookReaderParam CurrentParams
        {
            get
            {
                if (m_CurrentParams == null)
                {
                    m_CurrentParams = BookReaderParam.ReadParams();
                }
                return m_CurrentParams;
            }
        }

        FormFind m_FormFind;
        BookReaderTools? m_Tools;
        //double m_LinesOfPage = 0.0;

        public MainForm()
        {
            InitializeComponent();
            m_ListTextBox = new Dictionary<string, List<Control>>();
            m_mousePath = new System.Drawing.Drawing2D.GraphicsPath();
            m_ContextMenuStrip = new ContextMenuStrip();
            m_FormFind = new FormFind();
        }

        void Init()
        {
            //m_mousePath = new System.Drawing.Drawing2D.GraphicsPath();
            CurrentParams.CurrentFiles.Clear();
            CurrentParams.RevisionList();
            if (CurrentParams.FormMax)
                WindowState = FormWindowState.Maximized;
            else
            {
                WindowState = FormWindowState.Normal;
                Size = new Size(CurrentParams.FormSize.Width, CurrentParams.FormSize.Height);
                if (CurrentParams.IsNew)
                    CurrentParams.FormLocation = new Point((Screen.PrimaryScreen.Bounds.Width - Size.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - Size.Height) / 2);
                Location = new Point(CurrentParams.FormLocation.X, CurrentParams.FormLocation.Y);
            }
            MainToolStrip.Visible = CurrentParams.VisToolBar;
            MainStatusStrip.Visible = CurrentParams.VisStatusBar;
            DateInfo.Visible = CurrentParams.VisStatusBar && CurrentParams.VisDateTime;
            TimeInfo.Visible = CurrentParams.VisStatusBar && CurrentParams.VisDateTime;
            ReCalcControlsSize();
            SetDateTime();
            CreateDropDownMenu();
            Resize += OnResize;
            //ResizeEnd += OnResize;
            KeyDown += MainForm_KeyDown;
            tbOpen.ButtonClick += TbOpen_Click;
            tbOpen.DropDown = m_ContextMenuStrip;
            tbClose.Click += TbClose_Click;

            tbMark.ButtonClick += TbMark_ButtonClick;
            tbMark.DropDownItems.Add(new ToolStripMenuItem("Открыть форму закладок (Ctrl+M)", null, TbMark_ButtonClick));
            tbMark.DropDownItems.Add(new ToolStripMenuItem("Добавить закладку (Shift+M)", null, TbMark_ButtonClick));
            tbMark.DropDownItems.Add(new ToolStripMenuItem("Перейти на последнюю закладку (Alt+M)", null, TbMark_ButtonClick));

            tbFont.ButtonClick += TbFont_Click;
            foreach (var item in tbFont.DropDown.Items)
            {
                if (item is ToolStripMenuItem ts)
                    ts.Click += TbFont_Click;
            }
            tbColor.ButtonClick += TbColor_Click;
            foreach (var item in tbColor.DropDown.Items)
            {
                if (item is ToolStripMenuItem ts)
                    ts.Click += TbColor_Click;
            }
            //tcBooks.Selected += TcBooks_Selected;
            //tcBooks.Selecting += TcBooks_Selecting;
            tcBooks.Deselecting += TcBooks_Deselecting;
            tcBooks.SelectedIndexChanged += TcBooks_SelectedIndexChanged;
            if (CurrentParams.OpenLastFile && !string.IsNullOrWhiteSpace(CurrentParams.LastFileID))
            {
                FileParam? file = CurrentParams.FindFileByID(CurrentParams.LastFileID);
                if (file != null)
                    OpenFileData(file.FileName);
            }
            ContextMenuStrip = m_ContextMenuStrip;
            m_Tools = new BookReaderTools(CurrentParams);
        }

        private void TcBooks_Deselecting(object? sender, TabControlCancelEventArgs e)
        {
            //throw new NotImplementedException();
            SetCurrentMark(e.TabPage == null ? "" : e.TabPage.Name);
        }

        private void TcBooks_Selecting(object? sender, TabControlCancelEventArgs e)
        {
            //throw new NotImplementedException();
            //e.
        }

        private void TbMark_ButtonClick(object? sender, EventArgs e)
        {
            if (sender == null || sender is ToolStripSplitButton)
                Marks(false, false);
            else if (sender is ToolStripItem mi2)
            {
                if (mi2.Text.Contains("Ctrl"))
                    Marks(false, false);
                else if (mi2.Text.Contains("Shift"))
                    Marks(true, false);
                else if (mi2.Text.Contains("Alt"))
                    Marks(false, true);
            }
        }

        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.O:
                    if (e.Control)
                    {
                        TbOpen_Click(null, new EventArgs());
                        e.Handled = true;
                    }
                    break;
                case Keys.C:
                    if (e.Alt)
                    {
                        TbClose_Click(null, new EventArgs());
                        e.Handled = true;
                    }
                    break;
                case Keys.M:
                    if (e.Control)
                    {
                        Marks(false, false);
                        e.Handled = true;
                    }
                    else if (e.Shift)
                    {
                        Marks(true, false);
                        e.Handled = true;
                    }
                    else if (e.Alt)
                    {
                        Marks(false, true);
                        e.Handled = true;
                    }
                    break;
                case Keys.A:
                    if (e.Control)
                    {
                        TbFont_Click(null, new EventArgs());
                        e.Handled = true;
                    }
                    else if (e.Alt)
                    {
                        TbFont_Click(ddItemFontDefault, new EventArgs());
                        e.Handled = true;
                    }
                    break;
                case Keys.K:
                    if (e.Control)
                    {
                        TbColor_Click(null, new EventArgs());
                        e.Handled = true;
                    }
                    else if (e.Alt)
                    {
                        TbColor_Click(ddColorDefault, new EventArgs());
                        e.Handled = true;
                    }
                    break;
                case Keys.F4:
                    if (e.Alt)
                    {
                        tbExit_Click(this, new EventArgs());
                        e.Handled = true;
                    }
                    break;
                case Keys.F:
                    if (e.Control)
                    {
                        tbSearch_Click(this, new EventArgs());
                        e.Handled = true;
                    }
                    break;
                case Keys.F3:
                    if (PrepareFormFind())
                        m_FormFind.Find(true, false);
                    e.Handled = true;
                    break;
                    //case Keys.PageDown:
                    //case Keys.PageUp:
                    //    SetPageInfo(null, null);
                    //    e.Handled = true;
                    //    break;
            }
        }

        void Marks(bool pSetMark, bool pGoMark, FileParam? pFile = null, long pMark = -1)
        {
            if (pFile == null)
                pFile = CurrentParams.LastFile;
            if (pFile != null && m_ListTextBox.TryGetValue(pFile.ID, out List<Control>? textBox) && textBox != null)
            {
                RichTextBoxEx re = (RichTextBoxEx)textBox[0];
                if (re.Lines.Length > 0)
                {
                    if (pGoMark)
                    {
                        if (pMark < 0)
                        {
                            if (pFile.MarkLast >= 0)
                                pMark = pFile.MarkLast;
                            else
                                pMark = pFile.Marks.FirstOrDefault().Key;
                        }
                        if (pMark >= 0)
                            re.SelectionStart = (int)pMark;
                    }
                    else if (pSetMark)
                    {
                        int pos = re.SelectionStart;
                        string markTxt = $"{pos}  {(pos + 160 > re.Text.Length ? re.Text.Substring(pos) : re.Text.Substring(pos, 160) + "...")}";
                        //string markTxt = pos + 160 > re.Text.Length ? re.Text.Substring(pos) : re.Text.Substring(pos, 160) + "...";
                        markTxt = markTxt.Replace("\n", " ");
                        markTxt = markTxt.Replace("\r", " ");
                        if (markTxt.Trim().Length == 0)
                            markTxt = "пустая строка";
                        pFile.AddMark(pos, markTxt);
                    }
                    else
                    {
                        using FormMark fMark = new(pFile, re);
                        fMark.ShowDialog();
                    }
                }
            }
        }

        private void TbColor_Click(object? sender, EventArgs e)
        {
            if (CurrentParams.LastFile != null && m_ListTextBox.TryGetValue(CurrentParams.LastFile.ID, out List<Control>? textBox) && textBox != null)
            {
                if (sender == null || sender is ToolStripSplitButton || (sender is ToolStripItem mi && !mi.Name.Contains("Default")))
                {
                    Color fnt = Color.Empty;
                    using ColorDialog cd = new();
                    cd.Color = textBox[0].BackColor;
                    if (cd.ShowDialog() == DialogResult.OK)
                        fnt = cd.Color;
                    if (!fnt.IsEmpty)
                    {
                        foreach (var item in textBox)
                            item.BackColor = fnt;
                        CurrentParams.LastFile.FileColor = fnt;
                    }
                }
                else
                {
                    foreach (var item in textBox)
                        item.BackColor = CurrentParams.DefColor;
                    CurrentParams.LastFile.FileColor = Color.Empty;
                }
            }
        }

        private void TbFont_Click(object? sender, EventArgs e)
        {
            if (CurrentParams.LastFile != null && m_ListTextBox.TryGetValue(CurrentParams.LastFile.ID, out List<Control>? textBox) && textBox != null)
            {
                if (sender == null || sender is ToolStripSplitButton || (sender is ToolStripItem mi && !mi.Name.Contains("Default")))
                {
                    Font? fnt = null;
                    using FontDialog fd = new();
                    fd.Font = textBox[0].Font;
                    if (fd.ShowDialog() == DialogResult.OK)
                    {
                        fnt = fd.Font; // new Font(fd.Font.FontFamily, fd.Font.Size, fd.Font.Style, GraphicsUnit.Pixel);
                    }
                    if (fnt != null)
                    {
                        textBox[0].Font = fnt;
                        CurrentParams.LastFile.FileFont = fnt;
                        GetPageInfo(CurrentParams.LastFile, (RichTextBoxEx)textBox[0]);
                    }
                }
                else
                {
                    textBox[0].Font = CurrentParams.DefFont;
                    CurrentParams.LastFile.FileFont = null;
                    GetPageInfo(CurrentParams.LastFile, (RichTextBoxEx)textBox[0]);
                }
            }
        }

        private void TcBooks_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (tcBooks.TabCount > 0 && tcBooks.SelectedTab != null)
            {
                CurrentParams.SetCurrentFileByID(tcBooks.SelectedTab.Name);
                if (m_ListTextBox.TryGetValue(tcBooks.SelectedTab.Name, out List<Control>? textBox) && textBox != null)
                    textBox[0].Focus();
            }
            else
                CurrentParams.SetCurrentFileByID("");
        }

        private void TcBooks_Selected(object? sender, TabControlEventArgs e)
        {
        }

        private void TbClose_Click(object? sender, EventArgs e)
        {
            if (tcBooks.SelectedTab != null)
            {
                int ind = tcBooks.TabPages.Count == 1 ? -1 : tcBooks.SelectedIndex > 0 ? tcBooks.SelectedIndex - 1 : 0;
                DeletePages(new List<string> { tcBooks.SelectedTab.Name });
                tcBooks.SelectedIndex = ind;
            }
        }

        void DeletePages(List<string> pPageNames)
        {
            foreach (string item in pPageNames)
            {
                TabPage? tp = tcBooks.TabPages[item];
                if (tp != null)
                {
                    tp.Parent = null;
                    FileParam? file = CurrentParams.FindFileByID(tp.Name);
                    if (file != null && CurrentParams.CurrentFiles.Contains(file.ID))
                    {
                        CurrentParams.CurrentFiles.Remove(file.ID);
                        if (m_ListTextBox.TryGetValue(file.ID, out List<Control>? rb) && rb != null)
                        {                           
                            RichTextBoxEx re = (RichTextBoxEx)rb[0];
                            //SetCurrentMark(file, re);
                            re.Clear();
                            foreach (var item2 in rb)
                                item2.Dispose();
                            m_ListTextBox.Remove(file.ID);
                        }
                    }
                    tp.Dispose();
                }
            }
        }

        void CreateDropDownMenu()
        {
            m_ContextMenuStrip.Items.Clear();
            if (CurrentParams.LastFilesCount > 0)
            {
                int i = 1;
                foreach (FileParam item in CurrentParams.LastFiles.Values)
                {
                    if (i > CurrentParams.LastFilesCount)
                        break;
                    ToolStripMenuItem tc = new ToolStripMenuItem(item.FileName, null, TbOpen_Click);
                    tc.Name = item.ID;
                    m_ContextMenuStrip.Items.Add(tc);
                    i++;
                }
            }
        }

        private void TbOpen_Click(object? sender, EventArgs e)
        {
            string fileName = "";
            if (sender == null || sender is ToolStripSplitButton)
            {
                using OpenFileDialog od = new OpenFileDialog();
                od.Filter = "Текстовые файлы (*.txt)|*.txt|Rtf файлы (*.rtf)|*.rtf|Все файлы (*.*)|*.*";
                od.Title = "Открыть файл.";
                if (CurrentParams.WorkDir.Length > 0 && Directory.Exists(CurrentParams.WorkDir))
                    od.InitialDirectory = CurrentParams.WorkDir;
                if (od.ShowDialog() == DialogResult.OK)
                {
                    fileName = od.FileName;
                }
                else
                    return;
            }
            else if (sender is ToolStripItem mi)
            {
                fileName = mi.Text;
            }
            if (OpenFileData(fileName) && CurrentParams.SaveWorkDir)
                CurrentParams.WorkDir = Path.GetDirectoryName(fileName) ?? "";
        }

        bool OpenFileData(string fileName, bool pMess = true)
        {
            bool res = true;
            switch (CurrentParams.AddFile(fileName))
            {
                case FileOpenStaus.FirstOpen:
                    CreatePage(CurrentParams.LastFile);
                    CreateDropDownMenu();
                    break;
                case FileOpenStaus.AlreadyOpened:
                    CreatePage(CurrentParams.LastFile);
                    break;
                case FileOpenStaus.Opened:
                    if (CurrentParams.LastFile != null)
                    {
                        if (CurrentParams.OpenFileAgain)
                            OpenFile(CurrentParams.LastFile, false);
                        int ind = tcBooks.TabPages.IndexOfKey(CurrentParams.LastFile.ID);
                        tcBooks.SelectedIndex = ind >= 0 ? ind : 0;
                    }
                    break;
                case FileOpenStaus.ErrorNotFind:
                    DeleteFileData(fileName);
                    CreateDropDownMenu();
                    res = false;
                    break;
            }
            return res;
        }

        void DeleteFileData(string fileName, bool pMess = true)
        {
            FileParam? file = CurrentParams.FindFileByName(fileName);
            if (file == null)
            {
                if (pMess)
                    ControlTools.ShowMessageExt($"[W]Файл '{fileName}' не найден. Возможно он удален или перемещен.");
            }
            else if (pMess && ControlTools.ShowMessageExt($"[W]Файл '{fileName}' не найден. Возможно он удален или перемещен.{Environment.NewLine}Удалить сведения о файле из системы?", "Да;Нет") == 0)
            {
                CurrentParams.DeleteFile(file);
            }
        }

        void CreatePage(FileParam? file, bool pOpenFile = true)
        {
            if (file != null && !m_ListTextBox.ContainsKey(file.ID))
            {
                TabPage tp = new TabPage(file.FileNameShort);
                tp.Name = file.ID;
                tp.BorderStyle = BorderStyle.FixedSingle;
                RichTextBoxEx textBox = new RichTextBoxEx();
                tp.Controls.Add(textBox);
                //textBox.Dock = DockStyle.Fill;
                textBox.Font = file.FileFont == null ? CurrentParams.DefFont : file.FileFont;
                textBox.BackColor = file.FileColor == Color.Empty ? CurrentParams.DefColor : file.FileColor;
                tp.BackColor = textBox.BackColor;
                textBox.WordWrap = true;
                textBox.Multiline = true;
                //textBox.ScrollBars = RichTextBoxScrollBars.None;
                textBox.ScrollBars = RichTextBoxScrollBars.Vertical;
                textBox.BorderStyle = BorderStyle.None;
                textBox.ReadOnly = true;
                //textBox.Cursor = Cursors.Default;
                textBox.ScrollToCaret();
                textBox.ContextMenuStrip = m_ContextMenuStrip;
                textBox.KeyDown += TextBox_KeyDown;
                //textBox.MouseWheel += TextBox_MouseWheel;
                //textBox.MouseMove += TextBox_MouseMove;
                //textBox.MouseLeave += TextBox_MouseLeave;
                //tp.Pa
                //textBox.Pai .Pa
                //textBox.SelectionBullet = true;
                //textBox.
                //textBox.Find()
                //Panel pnL = new();
                //pnL.Width = 35;
                //tp.Controls.Add(pnL);
                //pnL.Dock = DockStyle.Left;
                //pnL.BackColor = textBox.BackColor;
                //pnL.BorderStyle = BorderStyle.None;
                /*
                                Panel pnT = new();
                                pnT.Height = 15;
                                tp.Controls.Add(pnT);
                                pnT.Dock = DockStyle.Top;
                                pnT.BackColor = textBox.BackColor;
                                pnT.BorderStyle = BorderStyle.None;

                                Panel pnR = new();
                                pnR.Width = 15;
                                tp.Controls.Add(pnR);
                                pnR.Dock = DockStyle.Right;
                                pnR.BackColor = textBox.BackColor;
                                pnR.BorderStyle = BorderStyle.None;

                                Panel pnB = new();
                                pnB.Height = 15;
                                tp.Controls.Add(pnB);
                                pnB.Dock = DockStyle.Bottom;
                                pnB.BackColor = textBox.BackColor;
                                pnB.BorderStyle = BorderStyle.None;
                /**/
                tcBooks.TabPages.Add(tp);
                //textBox.SetBounds(35, 15, tp.Width - 35 - 15, tp.Height - 15 - 15);
                textBox.SetBounds(45, 0, tp.Width - 45, tp.Height);
                //textBox.
                m_ListTextBox.Add(file.ID, new List<Control> { textBox, tp }); //, pnL, pnT, pnR, pnB
                CreateDropDownMenu();
                if (pOpenFile)
                {
                    OpenFile(file, CurrentParams.GoMark);
                    GetPageInfo(file, textBox);
                }
                tcBooks.SelectedIndex = tcBooks.TabCount - 1;
            }
        }

        private void TextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            /*
            if (e.KeyCode == Keys.PageDown 
                || e.KeyCode == Keys.PageUp 
                || e.KeyCode == Keys.Down 
                || e.KeyCode == Keys.Up
                || e.KeyCode == Keys.End
                || e.KeyCode == Keys.Home)
               SetPageInfo(null, null);
            /**/
        }
        #region эксперементы с мишью
        private void TextBox_MouseLeave(object? sender, EventArgs e)
        {
            m_mousePath.Dispose();
            m_mousePath = new System.Drawing.Drawing2D.GraphicsPath();
        }

        private void TextBox_MouseMove(object? sender, MouseEventArgs e)
        {
            int mouseX = e.X;
            int mouseY = e.Y;
            m_mousePath.AddLine(mouseX, mouseY, mouseX, mouseY);
        }

        private void TextBox_MouseWheel(object? sender, MouseEventArgs e)
        {
            //System.Windows.Forms.TextBox textBox0 = new System.Windows.Forms.TextBox();
            //textBox0.Pa
            if (sender != null && sender is RichTextBoxEx textBox)
            {
                int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
                int numberOfPixelsToMove = (int)(numberOfTextLinesToMove * textBox.Font.Size);
                if (numberOfPixelsToMove != 0)
                {
                    //textBox.CreateGraphics(). .Set .Set
                    System.Drawing.Drawing2D.Matrix translateMatrix = new System.Drawing.Drawing2D.Matrix();
                    translateMatrix.Translate(0, numberOfPixelsToMove);
                    m_mousePath.Transform(translateMatrix);
                }
                textBox.CreateGraphics().DrawPath(System.Drawing.Pens.DarkRed, m_mousePath);
                //textBox.Invalidate();
            }

            //throw new NotImplementedException();
        }
        #endregion
        void OpenFile(FileParam? file, bool pGoToMark)
        {
            if (file != null && m_ListTextBox.TryGetValue(file.ID, out List<Control>? textBox) && textBox != null)
            {
                //Stream fs = new FileStream(file.FileName, FileMode.Open);
                //using (StreamReader sr = new StreamReader(fs, true))
                //{
                //    Encoding encoding = sr.CurrentEncoding; //.ToString()
                //}
                RichTextBoxEx rtb = (RichTextBoxEx)textBox[0];
                //rtb.Doc
                //TextRange doc = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                //using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
                if (Path.GetExtension(file.FileName).ToLower() == ".rtf")
                    rtb.LoadFile(file.FileName);
                else
                    rtb.Text = File.ReadAllText(file.FileName); // .GetEncoding("windows-1251")  .UTF8 //.LoadFile(file.FileName); // 
                if (pGoToMark && file.MarkClose >= 0)
                {
                    rtb.SelectionStart = (int)file.MarkClose;
                    rtb.ScrollToCaret();
                }
                //rtb.SelectionCharOffset = (int)file.MarkClose;
                rtb.Focus();
            }
        }

        void GetPageInfo(FileParam? file, RichTextBoxEx? rtb)
        {
            bool isData = false;
            if (file == null)
                file = m_CurrentParams?.LastFile ?? null;
            if (m_CurrentParams != null && file != null)
            {
                if (rtb == null && m_ListTextBox.TryGetValue(file.ID, out List<Control>? textBox) && textBox != null)
                    rtb = (RichTextBoxEx)textBox[0];
                if (rtb != null && rtb.Text.Length > 0)
                {
                    using Graphics gr = rtb.CreateGraphics();
                    gr.MeasureString(rtb.Text.Length > 5000 ? rtb.Text.Substring(0, 5000) : rtb.Text, rtb.Font, rtb.Size, null, out int ch1, out int ln1);
                    file.LinesOfPage = (double)ln1;
                    isData = true;
                    SetPageInfo(file, rtb);
                }
            }
            if (!isData && file != null)
                file.LinesOfPage = 0.0;
        }

        void SetPageInfo(FileParam? file, RichTextBoxEx? rtb)
        {
            if (file == null)
                file = m_CurrentParams?.LastFile ?? null;
            if (rtb == null && file != null && m_ListTextBox.TryGetValue(file.ID, out List<Control>? textBox) && textBox != null)
                rtb = (RichTextBoxEx)textBox[0];
            if (file != null && file.LinesOfPage > 0 && rtb != null)
            {
                if (m_timerInfo == null)
                {
                    m_timerInfo = new System.Windows.Forms.Timer();
                    m_timerInfo.Tick += M_timerInfo_Tick;
                    m_timerInfo.Interval = 100;
                }
                else
                    m_timerInfo.Stop();
                double lineEnd = rtb.GetLineFromCharIndex(rtb.Text.Length - 1) + 1;
                double pageCount = lineEnd / file.LinesOfPage;
                int pageCountI = (int)pageCount;
                if (pageCount > pageCountI)
                    pageCountI++;
                int line = rtb.GetLineFromCharIndex(rtb.SelectionStart);
                double current = line / file.LinesOfPage;
                int currentI = (int)current;
                if (current > currentI || currentI == 0)
                    currentI++;
                PageInfo.Text = $"Страница {currentI} из {pageCountI}";
                LineInfo.Text = "";
                m_timerInfo.Start();
            }
            else
            {
                PageInfo.Text = ""; //"Страница 4000 из 10000";
                LineInfo.Text = ""; //"Ст 100000"
            }
        }

        private void M_timerInfo_Tick(object? sender, EventArgs e)
        {
            SetPageInfo(null, null);
        }

        public int CountDisplayedLines(RichTextBox rtb)
        {
            Point pos = new Point(0, 0);
            int firstIndex = rtb.GetCharIndexFromPosition(pos);
            int firstLine = rtb.GetLineFromCharIndex(firstIndex);

            // now we get index of last visible char 
            // and number of last visible line
            pos.X = rtb.ClientRectangle.Width;
            pos.Y = rtb.ClientRectangle.Height;
            int lastIndex = rtb.GetCharIndexFromPosition(pos);
            int lastLine = rtb.GetLineFromCharIndex(lastIndex);

            pos = rtb.GetPositionFromCharIndex(lastIndex);

            // finally, renumber label
            //Label1.Text = "";
            int lineCharIndex = 0;
            int selPos = rtb.SelectionStart;
            int lineNum = 1;
            int i = firstLine;
            while (i <= lastLine)
            {
                lineCharIndex = rtb.GetFirstCharIndexFromLine(i);
                rtb.SelectionStart = lineCharIndex;

                if (rtb.SelectionIndent < 1 & !rtb.SelectionBullet)
                {
                    //Label1.Text += lineNum + Environment.NewLine;
                    lineNum += 1;
                }
                //else
                //    Label1.Text += Environment.NewLine;

                System.Math.Max(System.Threading.Interlocked.Increment(ref i), i - 1);
            }
            //TextPointer position = rtb.Selection.End;
            //rtb.CaretPosition = rtb.Document.ContentEnd;
            //rtb.CaretPosition.GetLineStartPosition(-int.MaxValue, out int lineNumber);
            //rtb.CaretPosition = position;
            return lineNum; //     lineNumber * -1 + 2;
        }

        private void OnResize(object? sender, EventArgs e)
        {
            ReCalcControlsSize();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            SetDateTime();
        }

        void SetDateTime()
        {
            if (CurrentParams.VisStatusBar && CurrentParams.VisDateTime)
            {
                DateTime dateTime = DateTime.Now;
                if (m_timer == null)
                {
                    m_timer = new System.Windows.Forms.Timer();
                    m_timer.Tick += Timer_Tick;
                    m_timer.Interval = 60000 - dateTime.Second * 1000;
                }
                else
                {
                    m_timer.Stop();
                    m_timer.Interval = 60000;
                }
                m_timer.Start();
                TimeInfo.Text = dateTime.ToShortTimeString();
                string dd = dateTime.ToString("dddd");
                dd = dd.Substring(0, 1).ToUpper() + dd.Substring(1);
                DateInfo.Text = $"{dateTime.ToLongDateString()}  {dd}";
            }
        }

        private void tbExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Init();
        }
        void ReCalcControlsSize()
        {
            int w = 0;
            ToolStripItem? cnt = null;
            if (CurrentParams.VisToolBar)
            {
                foreach (ToolStripItem item in MainToolStrip.Items)
                {
                    //if (item.Visible)
                    {
                        if (item.Name == "spColorExit")
                            cnt = item;
                        else
                            w += item.Width;
                    }
                }
                if (cnt != null)
                    cnt.Width = Size.Width - w - 30;
                cnt = null;
                w = 0;
            }
            if (CurrentParams.VisStatusBar)
            {
                foreach (ToolStripItem item in MainStatusStrip.Items)
                {
                    //if (item.Visible)
                    {
                        if (item.Name == "CodeInfo")
                            cnt = item;
                        else
                            w += item.Width;
                    }
                }
                if (cnt != null)
                    cnt.Width = Size.Width - w - 30; // - 10;
            }
            if (CurrentParams.LastFile != null && m_ListTextBox.TryGetValue(CurrentParams.LastFile.ID, out List<Control>? textBox) && textBox != null)
            {
                //textBox[0].SetBounds(35, 15, textBox[1].Width - 35 - 15, textBox[1].Height - 15 - 15);
                textBox[0].SetBounds(45, 0, textBox[1].Width - 45, textBox[1].Height); // - 15 - 15
                GetPageInfo(CurrentParams.LastFile, (RichTextBoxEx)textBox[0]);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!e.Cancel)
            {
                if (m_timerInfo != null)
                {
                    m_timerInfo.Stop();
                    m_timerInfo.Dispose();
                }
                if (m_timer != null)
                {
                    m_timer.Stop();
                    m_timer.Dispose();
                }
                if (m_FormFind != null)
                {
                    m_FormFind.NeedClose = true;
                    m_FormFind.Close();
                    m_FormFind.Dispose();
                }
                SetCurrentMark(tcBooks.SelectedTab == null ? "" : tcBooks.SelectedTab.Name);
                CurrentParams.FormLocation = new Point(Left, Top);
                CurrentParams.FormSize = new Size(Width, Height);
                BookReaderParam.WriteParams(CurrentParams);
                tcBooks.Deselecting -= TcBooks_Deselecting;
                tcBooks.SelectedIndexChanged -= TcBooks_SelectedIndexChanged;
                //if (tcBooks.TabCount > 0 && tcBooks.SelectedTab != null)
                //    CurrentParams.SetCurrentFileByID(tcBooks.SelectedTab.Name);
                //else
                //    CurrentParams.SetCurrentFileByID("");
                List<string> list = new();
                foreach (TabPage item in tcBooks.TabPages)
                    list.Add(item.Name);
                DeletePages(list);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Init();
        }
       
        void SetCurrentMark(string pNamePage)
        {
            if (!string.IsNullOrWhiteSpace(pNamePage))
            {
                FileParam? file = CurrentParams.FindFileByID(pNamePage);
                if (file != null && CurrentParams.CurrentFiles.Contains(file.ID))
                {
                    if (m_ListTextBox.TryGetValue(file.ID, out List<Control>? rb) && rb != null)
                        SetCurrentMark(file, (RichTextBoxEx)rb[0]);
                }
            }
        }

        void SetCurrentMark(FileParam? pFile, RichTextBoxEx? pRE)
        {
            if (pFile != null && pRE != null)
            {
                if (pRE.Lines.Length > 0)
                    pFile.MarkClose = pRE.SelectionStart;
                else
                    pFile.MarkClose = -1;
            }
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            //Init();
        }

        private void tbSearch_Click(object sender, EventArgs e)
        {
            if (PrepareFormFind())
                m_FormFind.Show();
        }
        bool PrepareFormFind()
        {
            m_FormFind.FileBook = CurrentParams?.LastFile;
            if (m_FormFind.FileBook != null && m_ListTextBox.TryGetValue(m_FormFind.FileBook.ID, out List<Control>? textBox) && textBox != null)
            {
                m_FormFind.TextBoxBook = (RichTextBoxEx)textBox[0];
                m_FormFind.FindStart = m_FormFind.TextBoxBook.SelectionStart + 1;
            }
            else
            {
                m_FormFind.TextBoxBook = null;
                m_FormFind.FindStart = 0;
            }
            return m_FormFind.TextBoxBook != null;
        }
    }
}