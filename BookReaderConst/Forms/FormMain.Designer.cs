namespace BookReaderConst
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            MainToolStrip = new ToolStrip();
            tbOpen = new ToolStripSplitButton();
            spOpenClose = new ToolStripSeparator();
            tbClose = new ToolStripButton();
            spCloseSearch = new ToolStripSeparator();
            tbSearch = new ToolStripButton();
            spSearchMark = new ToolStripSeparator();
            tbMark = new ToolStripSplitButton();
            spMarkFont = new ToolStripSeparator();
            tbFont = new ToolStripSplitButton();
            ddItemFontDefault = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            ddItemFont = new ToolStripMenuItem();
            spFontColor = new ToolStripSeparator();
            tbColor = new ToolStripSplitButton();
            ddColorDefault = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            ddColor = new ToolStripMenuItem();
            spColorExit = new ToolStripSeparator();
            tbExit = new ToolStripButton();
            MainStatusStrip = new StatusStrip();
            PageInfo = new ToolStripStatusLabel();
            LineInfo = new ToolStripStatusLabel();
            CodeInfo = new ToolStripStatusLabel();
            DateInfo = new ToolStripStatusLabel();
            TimeInfo = new ToolStripStatusLabel();
            tcBooks = new TabControl();
            MainToolStrip.SuspendLayout();
            MainStatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // MainToolStrip
            // 
            MainToolStrip.ImageScalingSize = new Size(32, 32);
            MainToolStrip.Items.AddRange(new ToolStripItem[] { tbOpen, spOpenClose, tbClose, spCloseSearch, tbSearch, spSearchMark, tbMark, spMarkFont, tbFont, spFontColor, tbColor, spColorExit, tbExit });
            MainToolStrip.Location = new Point(0, 0);
            MainToolStrip.Name = "MainToolStrip";
            MainToolStrip.Size = new Size(854, 39);
            MainToolStrip.TabIndex = 0;
            // 
            // tbOpen
            // 
            tbOpen.AutoSize = false;
            tbOpen.Image = (Image)resources.GetObject("tbOpen.Image");
            tbOpen.ImageAlign = ContentAlignment.MiddleLeft;
            tbOpen.ImageTransparentColor = Color.Magenta;
            tbOpen.Name = "tbOpen";
            tbOpen.Size = new Size(110, 36);
            tbOpen.Text = "Открыть";
            tbOpen.TextAlign = ContentAlignment.MiddleLeft;
            tbOpen.ToolTipText = "Открыть файл (Ctrl+O)";
            // 
            // spOpenClose
            // 
            spOpenClose.AutoSize = false;
            spOpenClose.Name = "spOpenClose";
            spOpenClose.Size = new Size(10, 39);
            // 
            // tbClose
            // 
            tbClose.AutoSize = false;
            tbClose.Image = (Image)resources.GetObject("tbClose.Image");
            tbClose.ImageAlign = ContentAlignment.MiddleLeft;
            tbClose.ImageTransparentColor = Color.Magenta;
            tbClose.Name = "tbClose";
            tbClose.Size = new Size(110, 36);
            tbClose.Text = "Закрыть";
            tbClose.TextAlign = ContentAlignment.MiddleLeft;
            tbClose.ToolTipText = "Закрыть текущий файл (Alt+C)";
            // 
            // spCloseSearch
            // 
            spCloseSearch.AutoSize = false;
            spCloseSearch.Name = "spCloseSearch";
            spCloseSearch.Size = new Size(10, 39);
            // 
            // tbSearch
            // 
            tbSearch.AutoSize = false;
            tbSearch.Image = (Image)resources.GetObject("tbSearch.Image");
            tbSearch.ImageAlign = ContentAlignment.MiddleLeft;
            tbSearch.ImageTransparentColor = Color.Magenta;
            tbSearch.Name = "tbSearch";
            tbSearch.Size = new Size(110, 36);
            tbSearch.Text = "Поиск";
            tbSearch.TextAlign = ContentAlignment.MiddleLeft;
            tbSearch.ToolTipText = "Поиск фрагмента текста в текущем файле (Ctrl+F)";
            tbSearch.Click += tbSearch_Click;
            // 
            // spSearchMark
            // 
            spSearchMark.AutoSize = false;
            spSearchMark.Name = "spSearchMark";
            spSearchMark.Size = new Size(10, 39);
            // 
            // tbMark
            // 
            tbMark.AutoSize = false;
            tbMark.Image = (Image)resources.GetObject("tbMark.Image");
            tbMark.ImageAlign = ContentAlignment.MiddleLeft;
            tbMark.ImageTransparentColor = Color.Magenta;
            tbMark.Name = "tbMark";
            tbMark.Size = new Size(110, 36);
            tbMark.Text = "Закладки";
            tbMark.TextAlign = ContentAlignment.MiddleLeft;
            tbMark.ToolTipText = "Закладки. Ctrl+M - открыть форму закладок, Alt+M - добавить закладку, Shift+M - перейти на последнюю закладку";
            // 
            // spMarkFont
            // 
            spMarkFont.AutoSize = false;
            spMarkFont.Name = "spMarkFont";
            spMarkFont.Size = new Size(10, 39);
            // 
            // tbFont
            // 
            tbFont.AutoSize = false;
            tbFont.DropDownItems.AddRange(new ToolStripItem[] { ddItemFontDefault, toolStripSeparator1, ddItemFont });
            tbFont.Image = (Image)resources.GetObject("tbFont.Image");
            tbFont.ImageAlign = ContentAlignment.MiddleLeft;
            tbFont.ImageTransparentColor = Color.Magenta;
            tbFont.Name = "tbFont";
            tbFont.Size = new Size(110, 36);
            tbFont.Text = "Шрифт";
            tbFont.TextAlign = ContentAlignment.MiddleLeft;
            tbFont.ToolTipText = "Изменение шрифта для текущего файла (Ctrl+A)";
            // 
            // ddItemFontDefault
            // 
            ddItemFontDefault.Image = (Image)resources.GetObject("ddItemFontDefault.Image");
            ddItemFontDefault.ImageAlign = ContentAlignment.MiddleLeft;
            ddItemFontDefault.ImageScaling = ToolStripItemImageScaling.None;
            ddItemFontDefault.Name = "ddItemFontDefault";
            ddItemFontDefault.Size = new Size(318, 22);
            ddItemFontDefault.Text = "Установить шрифт \"По умолчанию\" (Alt+A)";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(315, 6);
            // 
            // ddItemFont
            // 
            ddItemFont.Image = (Image)resources.GetObject("ddItemFont.Image");
            ddItemFont.ImageScaling = ToolStripItemImageScaling.None;
            ddItemFont.Name = "ddItemFont";
            ddItemFont.Size = new Size(318, 22);
            ddItemFont.Text = "Шрифт (Ctrl+A)";
            // 
            // spFontColor
            // 
            spFontColor.AutoSize = false;
            spFontColor.Name = "spFontColor";
            spFontColor.Size = new Size(10, 39);
            // 
            // tbColor
            // 
            tbColor.AutoSize = false;
            tbColor.DropDownItems.AddRange(new ToolStripItem[] { ddColorDefault, toolStripSeparator2, ddColor });
            tbColor.Image = (Image)resources.GetObject("tbColor.Image");
            tbColor.ImageAlign = ContentAlignment.MiddleLeft;
            tbColor.ImageTransparentColor = Color.Magenta;
            tbColor.Name = "tbColor";
            tbColor.Size = new Size(110, 36);
            tbColor.Text = "Цвет фона";
            tbColor.TextAlign = ContentAlignment.MiddleLeft;
            tbColor.ToolTipText = "Изменение цвета фона для текущего файла (Ctrl+K)";
            // 
            // ddColorDefault
            // 
            ddColorDefault.Image = (Image)resources.GetObject("ddColorDefault.Image");
            ddColorDefault.ImageScaling = ToolStripItemImageScaling.None;
            ddColorDefault.Name = "ddColorDefault";
            ddColorDefault.Size = new Size(334, 22);
            ddColorDefault.Text = "Установить цвет фона \"По умолчанию\" (Alt+K)";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(331, 6);
            // 
            // ddColor
            // 
            ddColor.Image = (Image)resources.GetObject("ddColor.Image");
            ddColor.ImageScaling = ToolStripItemImageScaling.None;
            ddColor.Name = "ddColor";
            ddColor.Size = new Size(334, 22);
            ddColor.Text = "Цвет фона (Ctrl+K)";
            // 
            // spColorExit
            // 
            spColorExit.AutoSize = false;
            spColorExit.Name = "spColorExit";
            spColorExit.Size = new Size(10, 39);
            // 
            // tbExit
            // 
            tbExit.AutoSize = false;
            tbExit.Image = (Image)resources.GetObject("tbExit.Image");
            tbExit.ImageAlign = ContentAlignment.MiddleLeft;
            tbExit.ImageTransparentColor = Color.Magenta;
            tbExit.Name = "tbExit";
            tbExit.Size = new Size(110, 36);
            tbExit.Text = "Выход";
            tbExit.TextAlign = ContentAlignment.MiddleLeft;
            tbExit.ToolTipText = "Завершить работу (Alt+F4)";
            tbExit.Click += tbExit_Click;
            // 
            // MainStatusStrip
            // 
            MainStatusStrip.Items.AddRange(new ToolStripItem[] { PageInfo, LineInfo, CodeInfo, DateInfo, TimeInfo });
            MainStatusStrip.Location = new Point(0, 137);
            MainStatusStrip.Name = "MainStatusStrip";
            MainStatusStrip.Size = new Size(854, 24);
            MainStatusStrip.TabIndex = 1;
            // 
            // PageInfo
            // 
            PageInfo.AutoSize = false;
            PageInfo.BorderSides = ToolStripStatusLabelBorderSides.Right;
            PageInfo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            PageInfo.Name = "PageInfo";
            PageInfo.Size = new Size(140, 19);
            PageInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // LineInfo
            // 
            LineInfo.AutoSize = false;
            LineInfo.BorderSides = ToolStripStatusLabelBorderSides.Right;
            LineInfo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            LineInfo.Name = "LineInfo";
            LineInfo.Size = new Size(90, 19);
            // 
            // CodeInfo
            // 
            CodeInfo.AutoSize = false;
            CodeInfo.BorderSides = ToolStripStatusLabelBorderSides.Right;
            CodeInfo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            CodeInfo.Name = "CodeInfo";
            CodeInfo.Size = new Size(200, 19);
            CodeInfo.Text = "Информация о кодировке файла";
            CodeInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // DateInfo
            // 
            DateInfo.AutoSize = false;
            DateInfo.BorderSides = ToolStripStatusLabelBorderSides.Right;
            DateInfo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            DateInfo.Name = "DateInfo";
            DateInfo.Size = new Size(195, 19);
            DateInfo.Text = "20 сентября 2022г.  Воскресенье";
            DateInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TimeInfo
            // 
            TimeInfo.AutoSize = false;
            TimeInfo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            TimeInfo.Name = "TimeInfo";
            TimeInfo.Size = new Size(45, 19);
            TimeInfo.Text = "24:24";
            // 
            // tcBooks
            // 
            tcBooks.Dock = DockStyle.Fill;
            tcBooks.Location = new Point(0, 39);
            tcBooks.Name = "tcBooks";
            tcBooks.SelectedIndex = 0;
            tcBooks.Size = new Size(854, 98);
            tcBooks.TabIndex = 2;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(854, 161);
            Controls.Add(tcBooks);
            Controls.Add(MainStatusStrip);
            Controls.Add(MainToolStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MinimumSize = new Size(870, 200);
            Name = "MainForm";
            StartPosition = FormStartPosition.Manual;
            Text = "Просмотр текстовых и RTF файлов";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            MainToolStrip.ResumeLayout(false);
            MainToolStrip.PerformLayout();
            MainStatusStrip.ResumeLayout(false);
            MainStatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip MainToolStrip;
        private ToolStripSplitButton tbOpen;
        private ToolStripSeparator spOpenClose;
        private ToolStripButton tbClose;
        private ToolStripSeparator spCloseSearch;
        private ToolStripButton tbSearch;
        private ToolStripSeparator spSearchMark;
        private ToolStripSplitButton tbMark;
        private ToolStripSeparator spMarkFont;
        private ToolStripSplitButton tbFont;
        private ToolStripSeparator spFontColor;
        private ToolStripSplitButton tbColor;
        private ToolStripSeparator spColorExit;
        private ToolStripButton tbExit;

        private StatusStrip MainStatusStrip;
        private ToolStripStatusLabel PageInfo;
        private ToolStripStatusLabel LineInfo;
        private ToolStripStatusLabel CodeInfo;
        private ToolStripStatusLabel DateInfo;
        private ToolStripStatusLabel TimeInfo;
        private TabControl tcBooks;
        private ToolStripMenuItem ddItemFontDefault;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem ddItemFont;
        private ToolStripMenuItem ddColorDefault;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem ddColor;
    }
}