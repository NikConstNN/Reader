namespace BookReaderConst
{
    partial class FormFind
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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

        //System.ComponentModel.ComponentResourceManager m_resources;
        //System.ComponentModel.ComponentResourceManager resources;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFind));
            pnTop = new Panel();
            edFile = new TextBox();
            lbFile = new Label();
            pnMarks = new Panel();
            chCase = new CheckBox();
            chCh = new CheckBox();
            chFullWord = new CheckBox();
            btnFindExit = new Button();
            btnFindPrev = new Button();
            cbFind = new ComboBox();
            lbFind = new Label();
            btnFindNext = new Button();
            toolTip1 = new ToolTip(components);
            MainStatusStrip = new StatusStrip();
            PageInfo = new ToolStripStatusLabel();
            pnTop.SuspendLayout();
            pnMarks.SuspendLayout();
            MainStatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // pnTop
            // 
            pnTop.Controls.Add(edFile);
            pnTop.Controls.Add(lbFile);
            pnTop.Dock = DockStyle.Top;
            pnTop.Location = new Point(0, 0);
            pnTop.Name = "pnTop";
            pnTop.Size = new Size(378, 46);
            pnTop.TabIndex = 8;
            pnTop.TabStop = true;
            // 
            // edFile
            // 
            edFile.Location = new Point(53, 13);
            edFile.Name = "edFile";
            edFile.ReadOnly = true;
            edFile.Size = new Size(313, 23);
            edFile.TabIndex = 7;
            edFile.TabStop = false;
            // 
            // lbFile
            // 
            lbFile.AutoSize = true;
            lbFile.Location = new Point(3, 16);
            lbFile.Name = "lbFile";
            lbFile.Size = new Size(44, 15);
            lbFile.TabIndex = 2;
            lbFile.Text = "Файл -";
            // 
            // pnMarks
            // 
            pnMarks.Controls.Add(chCase);
            pnMarks.Controls.Add(chCh);
            pnMarks.Controls.Add(chFullWord);
            pnMarks.Controls.Add(btnFindExit);
            pnMarks.Controls.Add(btnFindPrev);
            pnMarks.Controls.Add(cbFind);
            pnMarks.Controls.Add(lbFind);
            pnMarks.Controls.Add(btnFindNext);
            pnMarks.Dock = DockStyle.Fill;
            pnMarks.Location = new Point(0, 46);
            pnMarks.Name = "pnMarks";
            pnMarks.Size = new Size(378, 175);
            pnMarks.TabIndex = 2;
            // 
            // chCase
            // 
            chCase.AutoSize = true;
            chCase.Location = new Point(12, 87);
            chCase.Name = "chCase";
            chCase.Size = new Size(130, 19);
            chCase.TabIndex = 3;
            chCase.Text = "Учитывать регистр";
            chCase.UseVisualStyleBackColor = true;
            chCase.Click += chFullWord_Click;
            // 
            // chCh
            // 
            chCh.AutoSize = true;
            chCh.Checked = true;
            chCh.CheckState = CheckState.Checked;
            chCh.Location = new Point(12, 119);
            chCh.Name = "chCh";
            chCh.Size = new Size(120, 19);
            chCh.TabIndex = 4;
            chCh.Text = "Зациклить поиск";
            chCh.UseVisualStyleBackColor = true;
            // 
            // chFullWord
            // 
            chFullWord.AutoSize = true;
            chFullWord.Location = new Point(12, 56);
            chFullWord.Name = "chFullWord";
            chFullWord.Size = new Size(153, 19);
            chFullWord.TabIndex = 2;
            chFullWord.Text = "Только слово целиком";
            chFullWord.UseVisualStyleBackColor = true;
            chFullWord.Click += chFullWord_Click;
            // 
            // btnFindExit
            // 
            btnFindExit.DialogResult = DialogResult.Cancel;
            btnFindExit.Image = (Image)resources.GetObject("btnFindExit.Image");
            btnFindExit.ImageAlign = ContentAlignment.MiddleLeft;
            btnFindExit.Location = new Point(195, 116);
            btnFindExit.Name = "btnFindExit";
            btnFindExit.Size = new Size(171, 26);
            btnFindExit.TabIndex = 7;
            btnFindExit.Text = "Закрыть (Esc)";
            toolTip1.SetToolTip(btnFindExit, "Закрыть (Esc)");
            btnFindExit.UseVisualStyleBackColor = true;
            btnFindExit.Click += btnFindExit_Click;
            // 
            // btnFindPrev
            // 
            btnFindPrev.Image = (Image)resources.GetObject("btnFindPrev.Image");
            btnFindPrev.ImageAlign = ContentAlignment.MiddleLeft;
            btnFindPrev.Location = new Point(195, 84);
            btnFindPrev.Name = "btnFindPrev";
            btnFindPrev.Size = new Size(171, 26);
            btnFindPrev.TabIndex = 6;
            btnFindPrev.Text = "Найти предыдущее";
            btnFindPrev.TextAlign = ContentAlignment.MiddleRight;
            toolTip1.SetToolTip(btnFindPrev, "Найти предыдущее совпадение (Shift+F3)");
            btnFindPrev.UseVisualStyleBackColor = true;
            btnFindPrev.Click += btnFindPrev_Click;
            // 
            // cbFind
            // 
            cbFind.FormattingEnabled = true;
            cbFind.Location = new Point(53, 15);
            cbFind.Name = "cbFind";
            cbFind.Size = new Size(313, 23);
            cbFind.TabIndex = 1;
            cbFind.TextChanged += cbFind_TextChanged;
            cbFind.Leave += cbFind_Leave;
            // 
            // lbFind
            // 
            lbFind.AutoSize = true;
            lbFind.Location = new Point(3, 18);
            lbFind.Name = "lbFind";
            lbFind.Size = new Size(44, 15);
            lbFind.TabIndex = 0;
            lbFind.Text = "Найти:";
            // 
            // btnFindNext
            // 
            btnFindNext.Image = (Image)resources.GetObject("btnFindNext.Image");
            btnFindNext.ImageAlign = ContentAlignment.MiddleLeft;
            btnFindNext.Location = new Point(195, 52);
            btnFindNext.Name = "btnFindNext";
            btnFindNext.Size = new Size(171, 26);
            btnFindNext.TabIndex = 5;
            btnFindNext.Text = "Найти далее";
            toolTip1.SetToolTip(btnFindNext, "Найти следующее совпадение (F3)");
            btnFindNext.UseVisualStyleBackColor = true;
            btnFindNext.Click += btnFindNext_Click;
            // 
            // MainStatusStrip
            // 
            MainStatusStrip.Items.AddRange(new ToolStripItem[] { PageInfo });
            MainStatusStrip.Location = new Point(0, 197);
            MainStatusStrip.Name = "MainStatusStrip";
            MainStatusStrip.Size = new Size(378, 24);
            MainStatusStrip.TabIndex = 9;
            // 
            // PageInfo
            // 
            PageInfo.AutoSize = false;
            PageInfo.BorderSides = ToolStripStatusLabelBorderSides.Right;
            PageInfo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            PageInfo.Name = "PageInfo";
            PageInfo.Size = new Size(360, 19);
            PageInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FormFind
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnFindExit;
            ClientSize = new Size(378, 221);
            Controls.Add(MainStatusStrip);
            Controls.Add(pnMarks);
            Controls.Add(pnTop);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormFind";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Найти";
            Activated += FormFind_Activated;
            FormClosing += FormFind_FormClosing;
            pnTop.ResumeLayout(false);
            pnTop.PerformLayout();
            pnMarks.ResumeLayout(false);
            pnMarks.PerformLayout();
            MainStatusStrip.ResumeLayout(false);
            MainStatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnTop;
        private Panel pnMarks;
        private ToolTip toolTip1;
        private TextBox edFile;
        private Label lbFile;
        public Button btnFindExit;
        public Button btnFindPrev;
        private ComboBox cbFind;
        private Label lbFind;
        public Button btnFindNext;
        private CheckBox chCase;
        private CheckBox chCh;
        private CheckBox chFullWord;
        private StatusStrip MainStatusStrip;
        private ToolStripStatusLabel PageInfo;
    }
}