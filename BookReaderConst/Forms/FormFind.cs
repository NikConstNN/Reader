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
using VisualWinForm.Visual.Tools;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace BookReaderConst
{
    public partial class FormFind : Form
    {
        FileParam? m_FileBook = null;
        /// <summary>
        /// Форму нужно закрыть
        /// </summary>
        public bool NeedClose { get; set; } = false;

        int m_CurrentPos;

        int m_FirstPos = -1;

        int m_FindStart;

        int m_LastPos = -1;

        /// <summary>
        /// Позиция начала поиска
        /// </summary>
        public int FindStart
        { get { return m_FindStart; } set { if (m_FindStart != value) { m_FindStart = value; if (m_FindStart < 0) m_FindStart = 0; m_CurrentPos = m_FindStart; } } }

        /// <summary>
        /// Параметры текущей книги
        /// </summary>
        public FileParam? FileBook
        {
            get { return m_FileBook; }
            set
            {
                if ((m_FileBook == null && value != null) || (m_FileBook != null && value == null) || (m_FileBook != null && value != null && !m_FileBook.Equals(value)))
                {
                    m_FileBook = value;
                    if (m_FileBook != null)
                        edFile.Text = m_FileBook.FileNameShort;
                    else
                        edFile.Text = "";
                    m_LastPos = -1;
                    m_FirstPos = -1;
                }
            }
        }
        /// <summary>
        /// RichTextBox в котором выполняется поиск
        /// </summary>
        public RichTextBoxEx? TextBoxBook { get; set; } = null;
        //ComponentResourceManager m_resources;
        /// <summary>
        /// Конструктор
        /// </summary>
        public FormFind()
        {
            InitializeComponent();
            TopMost = true;
            FindStart = TextBoxBook != null ? TextBoxBook.SelectionStart : 0;
            InitForm();
            ShowInTaskbar = false;
            chCh.Enabled = false;
        }

        private void FormFind_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F3:
                    if (e.Shift)
                        Find(false);
                    else
                        Find(true);
                    e.Handled = true;
                    break;
                case Keys.Escape:
                    Close();
                    e.Handled = true;
                    break;
            }
        }

        void InitForm()
        {
            SeparatorLine slTop = new()
            {
                Parent = pnTop,
                Dock = DockStyle.Bottom
            };
            KeyDown += FormFind_KeyDown;
        }

        private void btnFindNext_Click(object? sender, EventArgs e)
        {
            Find(true);
        }

        private void btnFindPrev_Click(object? sender, EventArgs e)
        {
            Find(false);
        }

        public void Find(bool pNext = true, bool pVis = true)
        {
            if (TextBoxBook != null && cbFind.Text != null && !string.IsNullOrWhiteSpace(cbFind.Text))
            {
                InitInfo();
                TextBoxBook.SelectionBackColor = TextBoxBook.BackColor;
                TextBoxBook.Select(0, 0);
                RichTextBoxFinds optoin = RichTextBoxFinds.None;
                if (!pNext)
                    optoin = RichTextBoxFinds.Reverse;
                if (chFullWord.Checked)
                {
                    if (optoin == RichTextBoxFinds.None)
                        optoin = RichTextBoxFinds.WholeWord;
                    else
                        optoin |= RichTextBoxFinds.WholeWord;
                }
                if (chCase.Checked)
                {
                    if (optoin == RichTextBoxFinds.None)
                        optoin = RichTextBoxFinds.MatchCase;
                    else
                        optoin |= RichTextBoxFinds.MatchCase;
                }
                if ((m_CurrentPos < TextBoxBook.Text.Length && pNext) || (m_CurrentPos > 0 && !pNext))
                {
                    m_CurrentPos = TextBoxBook.Find(cbFind.Text, m_CurrentPos < 0 ? 0 : m_CurrentPos, pNext ? TextBoxBook.Text.Length : -1, optoin); // + (pNext ? 1 : -1);
                    if (m_CurrentPos >= 0)
                    {
                        IsFind();
                        /*
                        TextBoxBook.ScrollToCaret();
                        m_LastPos = m_CurrentPos;
                        if (pVis)
                        {
                            TextBoxBook.SelectionBackColor = SystemColors.ActiveCaption;
                            if (m_LastPos == m_FirstPos)
                                ControlTools.ShowMessageExt($"[I]Текст '{cbFind.Text}', файл '{edFile.Text}'. Достигнут конец поиска.");
                        }
                        else
                            TextBoxBook.SelectionStart = m_CurrentPos;
                        if (m_FirstPos < 0)
                            m_FirstPos = m_CurrentPos;
                        m_CurrentPos += (pNext ? 1 : -1);
                        /**/
                    }
                    else
                    {
                        if (m_LastPos >= 0)
                        {
                            m_CurrentPos = TextBoxBook.Find(cbFind.Text, 0, pNext ? TextBoxBook.Text.Length : -1, optoin);
                            if (m_CurrentPos >= 0)
                                IsFind();
                            /*
                            {
                                TextBoxBook.ScrollToCaret();
                                m_LastPos = m_CurrentPos;
                                if (pVis)
                                {
                                    TextBoxBook.SelectionBackColor = SystemColors.ActiveCaption;
                                }
                                else
                                    TextBoxBook.SelectionStart = m_LastPos;
                                m_CurrentPos += (pNext ? 1 : -1);
                            }
                            /**/
                        }
                        else
                        {
                            PageInfo.ForeColor = Color.Red;
                            PageInfo.Text = $"Текст '{cbFind.Text}' не найден.";
                        }
                    }
                }
            }
            void IsFind()
            {
                if (TextBoxBook != null)
                {
                    TextBoxBook.ScrollToCaret();
                    m_LastPos = m_CurrentPos;
                    if (pVis)
                    {
                        TextBoxBook.SelectionBackColor = SystemColors.ActiveCaption;
                    }
                    else
                        TextBoxBook.SelectionStart = m_CurrentPos;
                    if (m_FirstPos < 0)
                        m_FirstPos = m_CurrentPos;
                    m_CurrentPos += (pNext ? 1 : -1);
                    if (m_LastPos == m_FirstPos)
                    {
                        PageInfo.Text = $"Текст '{cbFind.Text}'. Достигнут конец поиска.";
                    }
                }
            }
        }

        private void cbFind_TextChanged(object sender, EventArgs e)
        {
            SetBtn();
            m_LastPos = -1;
            m_FirstPos = -1;
            InitInfo();
        }
        void SetBtn()
        {
            btnFindNext.Enabled = TextBoxBook != null && cbFind.Text != null && !string.IsNullOrWhiteSpace(cbFind.Text);
            btnFindPrev.Enabled = false;
        }

        private void FormFind_Activated(object sender, EventArgs e)
        {
            if (cbFind.Text != null && !string.IsNullOrWhiteSpace(cbFind.Text))
                btnFindNext.Focus();
            else
                cbFind.Focus();
            SetBtn();
            InitInfo();
        }

        private void FormFind_FormClosing(object sender, FormClosingEventArgs e)
        {
            cbFind_Leave(null, null);
            if (TextBoxBook != null)
            {
                TextBoxBook.SelectionBackColor = TextBoxBook.BackColor;
                TextBoxBook.Select(0, 0);
                if (m_LastPos >= 0)
                {
                    TextBoxBook.SelectionStart = m_LastPos > 0 ? m_LastPos : 0;
                    //TextBoxBook.ScrollToCaret();
                }
            }
            m_FirstPos = -1;
            m_LastPos = -1;
            e.Cancel = !NeedClose;
            Hide();
        }

        private void btnFindExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        void InitInfo()
        {
            PageInfo.ForeColor = SystemColors.ControlText;
            PageInfo.Text = "";
        }

        private void cbFind_Leave(object? sender, EventArgs? e)
        {
            if (cbFind.Text != null && !string.IsNullOrWhiteSpace(cbFind.Text) && !cbFind.Items.Contains(cbFind.Text))
                cbFind.Items.Insert(0, cbFind.Text);
        }

        private void chFullWord_Click(object sender, EventArgs e)
        {
            m_FirstPos = -1;
            m_LastPos = -1;
        }
    }
}
