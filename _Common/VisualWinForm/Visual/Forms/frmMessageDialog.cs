using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace VisualWinForm.Visual.Forms
{
    /// <summary>
    /// Форма сообщения
    /// </summary>
	internal partial class frmMessageDialog : Form
    {

        #region Поля класса
        protected int SPACE_SIZE = 5;
		private string[] m_textButtons;
		private List<Button> m_buttons;
		private int m_defaultButton;
		private int m_result;
        private int m_widthAllButton;
        private bool m_Save;
        private bool m_ViewLabel;
        private IWin32Window? m_DialogOwner;
        TextBox m_edMessage;
        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
		public frmMessageDialog()
		{
			InitializeComponent();
            btnSave.Image = VisualWinForm.Properties.Resources.save;
            btnBuffer.Image = VisualWinForm.Properties.Resources.documents_text;
			m_buttons = new List<Button>();
			m_defaultButton = 0;
            m_widthAllButton = 0;
            m_Save = false;
            m_ViewLabel = true;
		}
        /// <summary>
        /// Компонент над котором отображаетсся форма
        /// </summary>
        public IWin32Window? DialogOwner
        {
            get
            {
                return m_DialogOwner;
            }
            set
            {
                m_DialogOwner = value;
                if (m_DialogOwner != null)
                {
                    if (m_DialogOwner is Control && string.IsNullOrWhiteSpace(this.Text))
                        this.Text = ((Control)m_DialogOwner).Text;
                    if (m_DialogOwner is Form && ((Form)m_DialogOwner).Icon != null)
                    {
                        this.ShowIcon = true;
                        this.Icon = ((Form)m_DialogOwner).Icon;
                    }
                }
                else
                {
                    if (Form.ActiveForm != null)
                        DialogOwner = Form.ActiveForm;
                    else if (Application.OpenForms.Count > 0)
                        DialogOwner = Application.OpenForms[Application.OpenForms.Count - 1];
                }
            }
        }
        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Message
        {
            get
            {
                if (m_ViewLabel)
                    return lbMessage.Text;
                else
                    return m_edMessage.Text;
            }
            set
            {
                lbMessage.Text = SetMessage(value);
            }
        }
        /// <summary>
        /// Список названий кнопок (строка через ';')
        /// </summary>
        public string TextButtons
        {
            set
            {
                string str;
                if (string.IsNullOrWhiteSpace(value))
                    str = "OK";
                else
                    str = value;
                this.m_textButtons = str.Split(new char[] { ';' });
                btnBuffer.Click += new EventHandler(btnBuffer_Click);
                btnSave.Click += new EventHandler(btnSave_Click);
                RefreshButtons();
            }
        }
        /// <summary>
        /// Номер кнопка, используемой по умолчанию
        /// </summary>
        public int DefaultButton
        {
            get
            {
                return m_defaultButton;
            }
            set
            {
                m_defaultButton = value;
                pnButton.TabIndex = 0;
                if (m_defaultButton < m_buttons.Count)
                {
                    m_buttons[m_defaultButton].TabIndex = 1;
                    m_buttons[m_defaultButton].Focus();
                }
            }
        }

        private string SetMessage(string pMess)
        {
            string res = pMess;
            if (string.IsNullOrWhiteSpace(res)) //сообщение null или пустое, отображаем 10 пробелов
                res = " ".PadRight(10, ' ');
            //проверка необходимости и установка картинки
            string imagCode = string.Empty;
            if (res[0].Equals('['))
            {
                string messCode = Common.Tools.StringTools.SurroundedTextFirst(res, '[', ']');
                if (!string.IsNullOrWhiteSpace(messCode))
                {
                    int l = messCode.Length + 2;
                    messCode = messCode.Trim().ToUpper();
                    imagCode = messCode.Substring(messCode.Length - 1);
                    if ((imagCode.Equals("W")
                        //|| imagCode.Equals("A")
                        //|| imagCode.Equals("S")
                        || imagCode.Equals("E")
                        || imagCode.Equals("I")
                        || imagCode.Equals("Q"))
                        && messCode.Length == 1)
                    {
                        res = res.Substring(l); //Если в коде сообщения только код картинки, удаляем его из сообщения
                    }
                }
            }
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(this.GetType()); 
            switch (imagCode)
            {
                case "E":
                //case "S":
                    picBox.Image = (Image)rm.GetObject("Mess_Error"); 
                    break;
                case "W":
                //case "A":
                    picBox.Image = (Image)rm.GetObject("Mess_Warning");
                    break;
                case "Q":
                    picBox.Image = (Image)rm.GetObject("Mess_Question"); 
                    break;
                case "I":
                    picBox.Image = (Image)rm.GetObject("Mess_Info"); 
                    break;
                default :
                    picBox.Visible = false;
                    pnLeft.Width = pnLeft.Width - picBox.Width - 10;
                    break;
            } 
            //препарируем сообщение
            m_edMessage = new TextBox();
            m_edMessage.Font = this.Font;
            m_edMessage.Multiline = true;
            m_edMessage.Size = new System.Drawing.Size(lbMessage.Width, lbMessage.Height);
            m_edMessage.Text = res;
            Graphics g = Graphics.FromHwnd(m_edMessage.Handle);
            int width = 0;
            int height = 0;
            foreach (string s in m_edMessage.Lines)
            {
                SizeF size = g.MeasureString(s, m_edMessage.Font);
                int w = (int)size.Width + 25;
                int h = (int)size.Height + 2;
                if (w > width)
                    width = w;
                if (h > height)
                    height = h;
            }
            width = width - m_edMessage.Width;
            if (width > 0)
            {
                if ((this.Width + width) > (Screen.PrimaryScreen.WorkingArea.Width - 80))
                {
                    this.Width = Screen.PrimaryScreen.WorkingArea.Width - 80;
                }
                else
                    this.Width += width;
                m_edMessage.Text = null;
                m_edMessage.Size = new System.Drawing.Size(lbMessage.Width, lbMessage.Height);
                m_edMessage.Text = res;
            }
            int linesCount = m_edMessage.GetLineFromCharIndex(m_edMessage.TextLength - 1);
            if (linesCount > 1)
            {
                linesCount--;
                if ((this.Height + (height * linesCount)) > (Screen.PrimaryScreen.WorkingArea.Height - 40))
                {
                    this.Height = Screen.PrimaryScreen.WorkingArea.Height - 40;
                    m_ViewLabel = false;                    
                }
                else
                    this.Height = this.Height + (height * linesCount);
            }
            if (m_ViewLabel)
            {
                m_edMessage.Text = string.Empty;
                m_edMessage.Dispose();
                m_edMessage = null;
            }
            //Не уложились в габариты. Вместо lbMessage показываем m_edMessage 
            else
            {
                lbMessage.Text = string.Empty;
                lbMessage.Visible = false;
                m_edMessage.ReadOnly = true;
                m_edMessage.BorderStyle = BorderStyle.FixedSingle;
                m_edMessage.ScrollBars = ScrollBars.Vertical;
                this.pnMessage.Controls.Add(m_edMessage);
                m_edMessage.Dock = DockStyle.Fill;
            }
            //выставляем кнопки по центру
            ReLocButton(); 
            return res;
        }

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			DefaultButton = m_defaultButton;
		}

		private void DeleteButtons()
		{
			foreach (Button control in m_buttons)
			{
				this.Controls.Remove(control);
				control.Dispose();
			}
			m_buttons.Clear();
		}

		private void RefreshButtons()
		{
			DeleteButtons();
			int right = this.pnButton.ClientRectangle.Right - SPACE_SIZE;
            int w = btnBuffer.Width;
            int l = right - w;
            btnBuffer.Location = new Point(l, 9);
            btnBuffer.TabIndex = this.m_textButtons.Length + 3;
            right = right - (w + SPACE_SIZE);
            m_widthAllButton += (w + SPACE_SIZE);
            w = btnSave.Width;
            l = right - w;
            btnSave.Location = new Point(l, 9);
            btnSave.TabIndex = this.m_textButtons.Length + 2;
            right = right - (w + SPACE_SIZE + 3);
            m_widthAllButton += (w + SPACE_SIZE + 3);

			for (int i = this.m_textButtons.Length - 1; i >= 0; i--)
			{
				Button button = new Button();
                button.Font = this.Font;
				Graphics g = Graphics.FromHwnd(button.Handle);
				int width = (int) g.MeasureString(this.m_textButtons[i], button.Font).Width + 20;
                if (width < 50)
                    width = 50;
                int left = right - width;

				button.Size = new Size(width, 25);
				//button.Location = new Point(left, this.ClientRectangle.Bottom - 10 - button.Size.Height);
                button.Location = new Point(left, 9);
				button.Name = "button" + i.ToString();
				button.Text = this.m_textButtons[i];
				button.TabIndex = i + 2;
				button.Tag = i;
				button.Click += new EventHandler(OnButtonClick);
                button.TextAlign = ContentAlignment.MiddleCenter;
				this.pnButton.Controls.Add(button);
				m_buttons.Insert(0, button);
                right = right - (width + SPACE_SIZE);
                m_widthAllButton += (width + SPACE_SIZE);
			}
            m_widthAllButton -= SPACE_SIZE;
            if (right < 0)
            {
                //Слишком много кнопок, не уложились в ширину экрана
                if ((this.Width - right + SPACE_SIZE) > (Screen.PrimaryScreen.WorkingArea.Width - 80))
                    this.Width = Screen.PrimaryScreen.WorkingArea.Width - 80;
                else
                    this.Width = this.Width - right + SPACE_SIZE;
            }
		}

        private void ReLocButton()
        {
            if (m_widthAllButton > 0 && m_buttons.Count > 0)
            { 
                int lLeft = (int)((this.ClientRectangle.Right + this.Padding.Right) - m_widthAllButton)/2;
                if (lLeft > 0)
                {
                    for (int i = 0; i < m_buttons.Count; i++)
                    {
                        Button btn = m_buttons[i];
                        btn.Location = new Point(lLeft, btn.Location.Y);
                        lLeft += (btn.Width + SPACE_SIZE);
                    }
                    lLeft++;
                    btnSave.Location = new Point(lLeft, btnSave.Location.Y);
                    lLeft += (btnSave.Width + SPACE_SIZE);
                    btnBuffer.Location = new Point(lLeft, btnBuffer.Location.Y);
                }
            }
        }

		void OnButtonClick(object sender, EventArgs e)
		{
			m_result = (int) ((Button) sender).Tag;
			DialogResult = DialogResult.OK;
		}

		public new int ShowDialog()
		{
			m_result = -1;
			base.ShowDialog(m_DialogOwner);
			return m_result;
		}

        private void btnSave_Click(object sender, EventArgs e)
        {
            using SaveFileDialog sd = new SaveFileDialog();
            if (sd.ShowDialog() == DialogResult.OK)
                File.WriteAllText(sd.FileName, Message);
            m_Save = true;
        }

        private void btnBuffer_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(Message);
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) 
            {
                if (!m_Save)
                {
                    m_result = -1;
                    DialogResult = DialogResult.OK;
                }
            }
            else if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control) btnSave_Click(btnSave, new EventArgs());
            else if (e.KeyCode == Keys.B && e.Modifiers == Keys.Control) btnBuffer_Click(btnBuffer, new EventArgs());
            e.Handled = true;
            m_Save = false;
        }

	}
}