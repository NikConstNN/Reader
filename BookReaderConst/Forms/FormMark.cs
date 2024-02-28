using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using VisualWinForm.Visual.Components;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace BookReaderConst
{
    public partial class FormMark : Form
    {
        FileParam m_File;
        RichTextBoxEx m_TextBox;
        List<Panel> m_Marks;
        ComponentResourceManager m_resources;
        public FormMark(FileParam pFile, RichTextBoxEx pTextBox)
        {
            InitializeComponent();
            m_resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMark));
            m_File = pFile;
            m_TextBox = pTextBox;
            m_Marks = new();
            SeparatorLine slTop = new()
            {
                Parent = pnTop,
                Dock = DockStyle.Bottom
            };
            //KeyUp += FormMark_KeyUp;
            KeyDown += FormMark_KeyDown;
            edFile.Text = pFile.FileNameShort;
            pnMarks.AutoScroll = true;
            ShowInTaskbar = false;
            InitMarks();
        }

        private void FormMark_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.M:
                    if (e.Shift)
                    {
                        btnAdd_Click(null, new EventArgs());
                        e.Handled = true;
                    }
                    break;
                case Keys.Escape:
                    Close();
                    e.Handled = true;
                    break;
            }
        }

        void InitMarks()
        {
            foreach (Panel item in m_Marks)
            {
                item.Hide();
                pnMarks.Controls.Remove(item);
                item.Dispose();
            }
            m_Marks.Clear();
            var res = ComparerByValueProperty.SortDictionaryByKey(m_File.Marks, ListSortDirection.Descending);
            if (res != null)
                foreach (var item in res)
                {
                    Panel pn = new()
                    {
                        Height = 50,
                        Parent = pnMarks,
                        Dock = DockStyle.Top
                    };
                    SeparatorLine slTop = new()
                    {
                        Parent = pn,
                        Dock = DockStyle.Bottom
                    };

                    TextBox ed = new()
                    {
                        AutoSize = false,
                        ReadOnly = true,
                        Parent = pn,
                        Multiline = true,
                        BorderStyle = BorderStyle.None,
                        Dock = DockStyle.Fill,
                        Text = item.Value,
                        Margin = new Padding(2, 0, 0, 0),
                        TextAlign = HorizontalAlignment.Left,
                        Font = new Font(Font, FontStyle.Bold),
                        Tag = item.Key
                    };
                    ed.Click += GoClick;
                    ed.Cursor = Cursors.Hand; // .Default;

                    Panel pnBtn = new()
                    {
                        Width = 80,
                        Parent = pn,
                        Dock = DockStyle.Right
                    };
                    SeparatorLine slBtn = new()
                    {
                        Parent = pnBtn,
                        Dock = DockStyle.Bottom
                    };
                    Panel pnLeft = new()
                    {
                        Width = 5,
                        Parent = pn,
                        Dock = DockStyle.Left
                    };
                    SeparatorLine slLeft = new()
                    {
                        Parent = pnLeft,
                        Dock = DockStyle.Bottom
                    };
                    Button btnG = new()
                    {
                        Parent = pnBtn,
                        Image = (Image)m_resources.GetObject("bookmark_blue"),
                        Location = new Point(1, 7),
                        Size = new Size(36, 33),
                        UseVisualStyleBackColor = true,
                        Tag = item.Key
                    };
                    toolTip1.SetToolTip(btnG, "Перейти на закладку");
                    btnG.Click += GoClick;

                    Button btnD = new()
                    {
                        Parent = pnBtn,
                        Image = (Image)m_resources.GetObject("bookmark_blue_delete"),
                        Location = new Point(40, 7),
                        Size = new Size(36, 33),
                        UseVisualStyleBackColor = true,
                        Tag = item.Key
                    };
                    toolTip1.SetToolTip(btnD, "Удалить на закладку");
                    btnD.Click += btnDel_Click;

                    //ed.Tr


                    //Label lb = new()
                    //{
                    //    AutoSize = false,
                    //    Parent = pn,
                    //    Dock = DockStyle.Fill,
                    //    Text = item.Value,
                    //    TextAlign = ContentAlignment.MiddleLeft,
                    //    Font = new Font(Font, FontStyle.Bold)
                    //};
                    m_Marks.Add(pn);
                }
        }
        private void btnAdd_Click(object? sender, EventArgs e)
        {
            int pos = m_TextBox.SelectionStart;
            string markTxt = $"{pos}  {(pos + 160 > m_TextBox.Text.Length ? m_TextBox.Text.Substring(pos) : m_TextBox.Text.Substring(pos, 160) + "...")}";
            markTxt = markTxt.Replace("\n", " ");
            markTxt = markTxt.Replace("\r", " ");
            if (markTxt.Trim().Length == 0)
                markTxt = "пустая строка";
            m_File.AddMark(pos, markTxt);
            Close();
        }

        private void GoClick(object? sender, EventArgs e)
        {
            if (sender != null && sender is Control cn && cn.Tag != null && long.TryParse(cn.Tag.ToString(), out long mark))
            {               
                m_TextBox.SelectionStart = (int)mark;
                m_TextBox.ScrollToCaret();
                m_File.MarkLast = mark;
                Close();
            }
        }

        private void btnDel_Click(object? sender, EventArgs e)
        {
            if (sender != null && sender is Button bt && bt.Tag != null && long.TryParse(bt.Tag.ToString(), out long mark))
            {
                if (m_File.DeleteMark(mark))
                    InitMarks();
            }
        }
    }
}
