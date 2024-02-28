namespace BookReaderConst
{
    partial class FormMark
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMark));
            pnTop = new Panel();
            edFile = new TextBox();
            lbFile = new Label();
            btnAdd = new Button();
            pnMarks = new Panel();
            toolTip1 = new ToolTip(components);
            pnTop.SuspendLayout();
            SuspendLayout();
            // 
            // pnTop
            // 
            pnTop.Controls.Add(edFile);
            pnTop.Controls.Add(lbFile);
            pnTop.Controls.Add(btnAdd);
            pnTop.Dock = DockStyle.Top;
            pnTop.Location = new Point(0, 0);
            pnTop.Name = "pnTop";
            pnTop.Size = new Size(480, 46);
            pnTop.TabIndex = 0;
            // 
            // edFile
            // 
            edFile.Location = new Point(53, 13);
            edFile.Name = "edFile";
            edFile.ReadOnly = true;
            edFile.Size = new Size(384, 23);
            edFile.TabIndex = 3;
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
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAdd.Image = (Image)resources.GetObject("btnAdd.Image");
            btnAdd.Location = new Point(441, 7);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(36, 33);
            btnAdd.TabIndex = 1;
            btnAdd.TextImageRelation = TextImageRelation.ImageBeforeText;
            toolTip1.SetToolTip(btnAdd, "Добавить закладку (Shift+M)");
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // pnMarks
            // 
            pnMarks.Dock = DockStyle.Fill;
            pnMarks.Location = new Point(0, 46);
            pnMarks.Name = "pnMarks";
            pnMarks.Size = new Size(480, 500);
            pnMarks.TabIndex = 2;
            // 
            // FormMark
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(480, 546);
            Controls.Add(pnMarks);
            Controls.Add(pnTop);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormMark";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Закладки";
            pnTop.ResumeLayout(false);
            pnTop.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnTop;
        public Button btnAdd;
        private Panel pnMarks;
        private TextBox edFile;
        private Label lbFile;
        private ToolTip toolTip1;
    }
}