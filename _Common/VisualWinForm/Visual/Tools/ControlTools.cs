using System;
using System.Windows.Forms;
using System.Text;
using VisualWinForm.Visual.Forms;

namespace VisualWinForm.Visual.Tools
{
	/// <summary>
    /// ������ ��� �����������, ��������� ������������
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
        /// ��������� �������� Enabled ��� ����������
		/// </summary>
        /// <param name="aParent">���������</param>
		/// <param name="aValue">����� �������� enable property</param>
		/// <param name="aRecursice">���������� ��� ���� �������� �����������</param>
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
        /// ���������� �������� FlatStyle ��� �����������.
		/// </summary>
        /// <param name="aParent">���������</param>
        /// <param name="aStyle">����� �������� FlatStyle</param>
        /// <param name="aRecursice">���������� ��� ���� �������� �����������</param>
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
        /// ������ �������� �������� ��� ���������� �����. ������������ ��� ���������� ������� KeyPress
		/// </summary>
        /// <param name="sender">�������� �������.</param>
		/// <param name="e">KeyPressEventArgs - ������ � �������.</param>
		public static void OnlyDigitFilter(object sender, KeyPressEventArgs e)
		{
            //edTraceNumber7.KeyPress += new KeyPressEventHandler(VisualWinForm.Visual.Tools.ControlTools.OnlyDigitFilter);
			if (!(char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
			{
				e.Handled = true;
			}
		}

        /// <summary>
        /// ������ �������� �������� ��� ���������� �����. ����������� ���� �����, ����� "-" � ������ ������� � ����������� �����������. ������������ ��� ���������� ������� KeyPress
        /// </summary>
        /// <param name="sender">�������� �������.</param>
        /// <param name="e">KeyPressEventArgs - ������ � �������.</param>
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
            bool flagOK = char.IsDigit(e.KeyChar) //�����
                || (text == string.Empty && e.KeyChar.Equals('-')) //���� "-" � ������ �������
                || e.KeyChar == (char)Keys.Back //BACKSPACE key
                || (e.KeyChar.ToString().Equals(DecimalSeparator) && text.IndexOf(DecimalSeparator) < 0); //������������ ����������� ������� �����
            e.Handled = !flagOK;
        }

		/// <summary>
        /// ���������� true ���� �������� ���������� �����. ������� ���� ��������� ������ ���������
		/// </summary>
        /// <param name="aControl">����������� ���������.</param>
        /// <returns>true ���� �������� ���������� �����.</returns>
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
        /// ���� �������� ���������� �����, ������������ �� ���� ����� � ������ ��������� (���� ������� ���� ���������).
		/// </summary>
        /// <param name="aControl">����������� ���������.</param>
        /// <param name="aOwner">���� ����� ������� ���������� ���� ���������.</param>
		/// <param name="aMessage">���� ���������</param>
        /// <returns>true ���� �������� ���������� �����.</returns>
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
		/// ���������� ����� �� ���������.
		/// </summary>
        /// <param name="aControl">���������</param>
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

        #region ��������� � ������������ MessageBox
        /// <summary>
        /// �������� ���������-������������� � ������������ MessageBox
        /// </summary>
        /// <param name="aOwner">���� ����� ������� ���������� ���� ���������.</param>
        /// <param name="aMessage">����� ���������.</param>
        /// <param name="aButtons">������, ������������ � ���������.</param>
        /// <returns>�������� DialogResult.</returns>
        public static DialogResult ShowQuestionMessage(IWin32Window aOwner, string aMessage, MessageBoxButtons aButtons)
        {
            return MessageBox.Show(aOwner, aMessage, string.Empty, aButtons, MessageBoxIcon.Question);
        }

        /// <summary>
        /// �������� ��������� �� ������ � ����������� MessageBox.
        /// </summary>
        /// <param name="aOwner">���� ����� ������� ���������� ���� ���������.</param>
        /// <param name="aMessage">����� ���������.</param>
        public static void ShowErrorMessage(IWin32Window aOwner, string aMessage)
        {
            MessageBox.Show(aOwner, aMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        ///  �������� �������������� ��������� � ����������� MessageBox.
        /// </summary>
        /// <param name="aOwner">���� ����� ������� ���������� ���� ���������.</param>
        /// <param name="aMessage">����� ���������.</param>
        public static void ShowInfoMessage(IWin32Window aOwner, string aMessage)
        {
            MessageBox.Show(aOwner, aMessage, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// �������� ��������� � ������������ MessageBox.
        /// </summary>
        /// <param name="aOwner">���� ����� ������� ���������� ���� ���������</param>
        /// <param name="aMessages">������ ���������</param>
        /// <param name="aTitle">��������� ���������</param>
        /// <param name="aIcon">������ ���������</param>
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

        #region ������ ������� ShowMessageExt - ����� ��������� � ������������ ���. ������ � ���������� ���������
        /// <summary>
        /// �������� ����� ���������. ����������� ���� ������ "OK"
        /// </summary>
        /// <param name="aMessage">����� ���������</param>
        /// <returns>0 ��� -1</returns>
        public static int ShowMessageExt(string aMessage)
        {
            return ShowMessageExt(null, aMessage, string.Empty, string.Empty, 0);
        }

        /// <summary>
        /// �������� ����� ���������. ����������� ���������� ������
        /// </summary>
        /// <param name="aMessage">����� ���������</param>
        /// <param name="aButtons">������ �������� ������ (������ ����� ';')</param>
        /// <returns>���������� ����� ������� ������ ��� -1, ���� ���� ������� �� 'X' ��� 'Esc'</returns>
        public static int ShowMessageExt(string aMessage, string aButtons)
        {
            return ShowMessageExt(null, aMessage, string.Empty, aButtons, 0);
        }

        /// <summary>
        /// �������� ����� ���������. ����������� ���������� ������
        /// </summary>
        /// <param name="aMessage">����� ���������</param>
        /// <param name="aTitle">��������� �����</param>
        /// <param name="aButtons">������ �������� ������ (������ ����� ';')</param>
        /// <returns>���������� ����� ������� ������ ��� -1, ���� ���� ������� �� 'X' ��� 'Esc'</returns>
        public static int ShowMessageExt(string aMessage, string aTitle, string aButtons)
        {
            return ShowMessageExt(null, aMessage, aTitle, aButtons, 0);
        }

        /// <summary>
        /// �������� ����� ���������. ����������� ���� ������ "OK"
        /// </summary>
        /// <param name="aOwner">��������� ��� ������� ������������� �����</param>
        /// <param name="aMessage">����� ���������</param>
        /// <returns>0 ��� -1</returns>
        public static int ShowMessageExt(IWin32Window? aOwner, string aMessage)
        {
            return ShowMessageExt(aOwner, aMessage, string.Empty, string.Empty, 0);
        }

        /// <summary>
        /// �������� ����� ���������. ����������� ���������� ������
        /// </summary>
        /// <param name="aOwner">��������� ��� ������� ������������� �����</param>
        /// <param name="aMessage">����� ���������</param>
        /// <param name="aButtons">������ �������� ������ (������ ����� ';')</param>
        /// <returns>���������� ����� ������� ������ ��� -1, ���� ���� ������� 'X' ��� 'Esc'</returns>
        public static int ShowMessageExt(IWin32Window? aOwner, string aMessage, string aButtons)
        {
            return ShowMessageExt(aOwner, aMessage, string.Empty, aButtons, 0);
        }

        /// <summary>
        /// �������� ����� ��������� � ������������ ���. ������ � ���������� ���������
        /// </summary>
        /// <param name="aOwner">��������� ��� ������� ������������� �����</param>
        /// <param name="aMessage">����� ���������</param>
        /// <param name="aTitle">��������� �����</param>
        /// <param name="aButtons">������ �������� ������ (������ ����� ';')</param>
        /// <param name="aDefaultIndex">����� ������, ������������ �� ���������</param>
        /// <returns>���������� ����� ������� ������ ��� -1, ���� ���� ������� 'X' ��� 'Esc'</returns>
        public static int ShowMessageExt(IWin32Window? aOwner, string aMessage, string aTitle, string aButtons, int aDefaultIndex)
        {
            //���� aOwner �� null � Control, � ��� ������� Text (���� �� �������� ���������� aTitle), ���� Form, ��� � Icon. 
            //������ ������� ��������� ���������� [...], �� ��������� ����� ���������. ��������� ������ � [...] ��������� ������� �������� �� �����:
            //E - Error, W - Warning, Q - Question, I - Info. ���� � [...] ��� ������������ ������, [...] � ��������� �� ����������
            //������ ���������� ����������� �� ����� 8-10 � ��������� �����������.
            //���������� ������� ��� ����� ������ ���� ��������� ������ � ����� �������:
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
