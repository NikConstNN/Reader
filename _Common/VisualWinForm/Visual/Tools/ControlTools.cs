using System;
using System.Windows.Forms;
using System.Text;
using VisualWinForm.Visual.Forms;

namespace VisualWinForm.Visual.Tools
{
	/// <summary>
    /// Методы для компонентов, сообщения пользователю
	/// </summary>
	public sealed class ControlTools
	{
		private static void SetEnableControl(Control aControl, bool aValue)
		{
			if (!(aControl is TabControl || aControl is GroupBox
				|| aControl is TabPage || aControl is Label || aControl is Panel))
			{
				aControl.Enabled = aValue;
			}
		}

		/// <summary>
        /// Установка значения Enabled для компонента
		/// </summary>
        /// <param name="aParent">Компонент</param>
		/// <param name="aValue">Новое значение enable property</param>
		/// <param name="aRecursice">Рекурсивно для всех дочерних компонентов</param>
		public static void SetEnableForControls(Control aParent, bool aValue, bool aRecursice)
		{
			SetEnableControl(aParent, aValue);
			if (aRecursice)
			{
				foreach (Control ctrl in aParent.Controls)
				{
					SetEnableForControls(ctrl, aValue, aRecursice);
				}
			}
		}

        private static void SetFlatStyle(Control aControl, FlatStyle aStyle)
		{

			if (aControl is ButtonBase)
			{
				((ButtonBase)aControl).FlatStyle = aStyle;
			}

			if (aControl is Label)
			{
				((Label)aControl).FlatStyle = aStyle;
			}

			if (aControl is GroupBox)
			{
				((GroupBox)aControl).FlatStyle = aStyle;
			}
		}

		/// <summary>
        /// Установить значение FlatStyle для компонентов.
		/// </summary>
        /// <param name="aParent">Компонент</param>
        /// <param name="aStyle">Новое значение FlatStyle</param>
        /// <param name="aRecursice">Рекурсивно для всех дочерних компонентов</param>
		public static void SetFlatStyleForControls(Control aParent, FlatStyle aStyle, bool aRecursive)
		{
			SetFlatStyle(aParent, aStyle);
			foreach(Control ctrl in aParent.Controls)
			{
				SetFlatStyle(ctrl, aStyle);
				if (aRecursive && ctrl.Controls.Count > 0)
				{
					SetFlatStyleForControls(ctrl, aStyle, aRecursive);
				}
			}
		}

        /// <summary>
        /// Фильтр цифровых значений для компонента ввода. Используется как обработчик события KeyPress
		/// </summary>
        /// <param name="sender">Источник события.</param>
		/// <param name="e">KeyPressEventArgs - данные о событии.</param>
		public static void OnlyDigitFilter(object sender, KeyPressEventArgs e)
		{
            //edTraceNumber7.KeyPress += new KeyPressEventHandler(VisualWinForm.Visual.Tools.ControlTools.OnlyDigitFilter);
			if (!(char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
			{
				e.Handled = true;
			}
		}

        /// <summary>
        /// Фильтр числовых значений для компонента ввода. Допускается ввод цифры, знака "-" в первой позиции и десятичного разделителя. Используется как обработчик события KeyPress
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">KeyPressEventArgs - данные о событии.</param>
        public static void OnlyNumberFilter(object sender, KeyPressEventArgs e)
        {
            //edTraceNumber7.KeyPress += new KeyPressEventHandler(VisualWinForm.Visual.Tools.ControlTools.OnlyNumberFilter);
            string text = string.Empty;
            string DecimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (sender is TextBox)
            {
                text = ((TextBox)sender).Text;
                if (text == null)
                    text = string.Empty;
            }
            bool flagOK = char.IsDigit(e.KeyChar) //цифра
                || (text == string.Empty && e.KeyChar.Equals('-')) //знак "-" в первой позиции
                || e.KeyChar == (char)Keys.Back //BACKSPACE key
                || (e.KeyChar.ToString().Equals(DecimalSeparator) && text.IndexOf(DecimalSeparator) < 0); //единственный разделитель дробной части
            e.Handled = !flagOK;
        }

		/// <summary>
        /// Возвращает true если значение компонента пусто. Пробелы тоже считаются пустым значением
		/// </summary>
        /// <param name="aControl">Проверяемый компонент.</param>
        /// <returns>true если значение компонента пусто.</returns>
		public static bool IsControlEmpty(Control aControl)
		{
			bool isEmpty = false;
            /*
			if (aControl is NumericBox)
			{
				isEmpty = (aControl as NumericBox).ValueIsNull;
			}
			else 
             */
                if (aControl is ComboBox)
			{
				isEmpty = (aControl as ComboBox).SelectedIndex < 0;
				if (isEmpty && ((aControl as ComboBox).DropDownStyle == ComboBoxStyle.Simple || (aControl as ComboBox).DropDownStyle == ComboBoxStyle.DropDown))
				{
					isEmpty = (aControl as ComboBox).Text == string.Empty;
				}
			}
			else if (aControl is TextBox)
			{
				string s = (aControl as TextBox).Text;
				isEmpty = s == null || s.Trim() == string.Empty;
			}
			return isEmpty;
		}

		/// <summary>
        /// Если значение компонента пусто, устанавливет не него фокус и выдает сообщение (если передан тект сообщения).
		/// </summary>
        /// <param name="aControl">Проверяемый компонент.</param>
        /// <param name="aOwner">Окно перед которым отображать окно сообщения.</param>
		/// <param name="aMessage">Тект сообщения</param>
        /// <returns>true если значение компонента пусто.</returns>
		public static bool IsControlEmpty(Control aControl, IWin32Window aOwner, string aMessage)
		{
			bool result = IsControlEmpty(aControl);
			if (result)
			{
				ForceFocus(aControl);
                if (!string.IsNullOrWhiteSpace(aMessage))
				    ShowInfoMessage(aOwner, aMessage);
			}
			return result;
		}

		/// <summary>
		/// Установить фокус на компонент.
		/// </summary>
        /// <param name="aControl">компонент</param>
		public static void ForceFocus(Control aControl)
		{
			Control aParent = aControl.Parent;
			while (aParent != null)
			{
				if (aParent is TabPage && aParent.Parent is TabControl)
				{
					(aParent.Parent as TabControl).SelectedTab = (aParent as TabPage);
				}
				aParent = aParent.Parent;
			}
			aControl.Focus();
		}

        #region сообщения в стандатртном MessageBox
        /// <summary>
        /// Показать сообщение-подтверждение в стандатртном MessageBox
        /// </summary>
        /// <param name="aOwner">Окно перед которым отображать окно сообщения.</param>
        /// <param name="aMessage">Текст сообщения.</param>
        /// <param name="aButtons">Кнопки, отображаемые в сообщении.</param>
        /// <returns>Значение DialogResult.</returns>
        public static DialogResult ShowQuestionMessage(IWin32Window aOwner, string aMessage, MessageBoxButtons aButtons)
        {
            return MessageBox.Show(aOwner, aMessage, string.Empty, aButtons, MessageBoxIcon.Question);
        }

        /// <summary>
        /// Показать сообщение об ошибке в стандартном MessageBox.
        /// </summary>
        /// <param name="aOwner">Окно перед которым отображать окно сообщения.</param>
        /// <param name="aMessage">Текст сообщения.</param>
        public static void ShowErrorMessage(IWin32Window aOwner, string aMessage)
        {
            MessageBox.Show(aOwner, aMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        ///  Показать информационное сообщение в стандартном MessageBox.
        /// </summary>
        /// <param name="aOwner">Окно перед которым отображать окно сообщения.</param>
        /// <param name="aMessage">Текст сообщения.</param>
        public static void ShowInfoMessage(IWin32Window aOwner, string aMessage)
        {
            MessageBox.Show(aOwner, aMessage, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Показать сообщение в стандатртном MessageBox.
        /// </summary>
        /// <param name="aOwner">Окно перед которым отображать окно сообщения</param>
        /// <param name="aMessages">Массив сообщений</param>
        /// <param name="aTitle">Заголовок сообщения</param>
        /// <param name="aIcon">Иконка сообщения</param>
        public static void ShowMessage(IWin32Window aOwner, string[] aMessages, string aTitle, MessageBoxIcon aIcon)
        {
            if (aMessages.Length > 0)
            {
                StringBuilder sbMess = new StringBuilder();
                foreach (string s in aMessages)
                {
                    if (sbMess.Length > 0)
                        sbMess.Append(Environment.NewLine);
                    sbMess.Append(s);
                }
                MessageBox.Show(aOwner, sbMess.ToString(), aTitle, MessageBoxButtons.OK, aIcon);
            }
        }
        #endregion

        #region Группа методов ShowMessageExt - показ сообщения с возможностью доп. кнопок и сохранения сообщения
        /// <summary>
        /// Показать форму сообщения. Формируется одна кнопка "OK"
        /// </summary>
        /// <param name="aMessage">Текст сообщения</param>
        /// <returns>0 или -1</returns>
        public static int ShowMessageExt(string aMessage)
        {
            return ShowMessageExt(null, aMessage, string.Empty, string.Empty, 0);
        }

        /// <summary>
        /// Показать форму сообщения. Формируются переданные кнопки
        /// </summary>
        /// <param name="aMessage">Текст сообщения</param>
        /// <param name="aButtons">Список названий кнопок (строка через ';')</param>
        /// <returns>Возвращает номер нажатой кнопки или -1, если окно закрыто по 'X' или 'Esc'</returns>
        public static int ShowMessageExt(string aMessage, string aButtons)
        {
            return ShowMessageExt(null, aMessage, string.Empty, aButtons, 0);
        }

        /// <summary>
        /// Показать форму сообщения. Формируются переданные кнопки
        /// </summary>
        /// <param name="aMessage">Текст сообщения</param>
        /// <param name="aTitle">Заголовок формы</param>
        /// <param name="aButtons">Список названий кнопок (строка через ';')</param>
        /// <returns>Возвращает номер нажатой кнопки или -1, если окно закрыто по 'X' или 'Esc'</returns>
        public static int ShowMessageExt(string aMessage, string aTitle, string aButtons)
        {
            return ShowMessageExt(null, aMessage, aTitle, aButtons, 0);
        }

        /// <summary>
        /// Показать форму сообщения. Формируется одна кнопка "OK"
        /// </summary>
        /// <param name="aOwner">Компонент над которым отображаетсся форма</param>
        /// <param name="aMessage">Текст сообщения</param>
        /// <returns>0 или -1</returns>
        public static int ShowMessageExt(IWin32Window? aOwner, string aMessage)
        {
            return ShowMessageExt(aOwner, aMessage, string.Empty, string.Empty, 0);
        }

        /// <summary>
        /// Показать форму сообщения. Формируются переданные кнопки
        /// </summary>
        /// <param name="aOwner">Компонент над которым отображаетсся форма</param>
        /// <param name="aMessage">Текст сообщения</param>
        /// <param name="aButtons">Список названий кнопок (строка через ';')</param>
        /// <returns>Возвращает номер нажатой кнопки или -1, если окно закрыто 'X' или 'Esc'</returns>
        public static int ShowMessageExt(IWin32Window? aOwner, string aMessage, string aButtons)
        {
            return ShowMessageExt(aOwner, aMessage, string.Empty, aButtons, 0);
        }

        /// <summary>
        /// Показать форму сообщения с возможностью доп. кнопок и сохранения сообщения
        /// </summary>
        /// <param name="aOwner">Компонент над которым отображаетсся форма</param>
        /// <param name="aMessage">Текст сообщения</param>
        /// <param name="aTitle">Заголовок формы</param>
        /// <param name="aButtons">Список названий кнопок (строка через ';')</param>
        /// <param name="aDefaultIndex">Номер кнопка, используемой по умолчанию</param>
        /// <returns>Возвращает номер нажатой кнопки или -1, если окно закрыто 'X' или 'Esc'</returns>
        public static int ShowMessageExt(IWin32Window? aOwner, string aMessage, string aTitle, string aButtons, int aDefaultIndex)
        {
            //Если aOwner не null и Control, с нее берется Text (если не передано параметром aTitle), если Form, еще и Icon. 
            //Первые символы сообщения окруженные [...], то считаются кодом сообщения. Последний символ в [...] управляет показом картинки на форме:
            //E - Error, W - Warning, Q - Question, I - Info. Если в [...] это единственный символ, [...] в сообщение не включается
            //Кнопок желательно формировать не более 8-10 с короткими заголовками.
            //Назначение свойств для формы должно быть выполнено именно в таком порядке:
            //1. Text
            //2. DialogOwner
            //3. TextButtons
            //4. Message
            //5. DefaultButton
            frmMessageDialog frm = new frmMessageDialog();
            frm.Text = aTitle;
            frm.DialogOwner = aOwner;
            frm.TextButtons = aButtons;
            frm.Message = aMessage;
            frm.DefaultButton = aDefaultIndex;
            return frm.ShowDialog();
        }

        #endregion
	}
}
