using System;
using System.ComponentModel;
using System.Windows.Forms;
//using Sda.Common;
using System.Drawing;
using System.Collections;
using System.Text.RegularExpressions;
//using SdaWin.Visual.Tools;

namespace VisualWinForm.Visual.Components
{
	/// <summary>
	/// Extended class of the DataGrid class.
	/// </summary>
	public class ExDataGrid : DataGridView
	{
		private bool m_allowFind = true;
        private bool m_allowExportToExcel = true; //08/22/13 SDA

		/// <summary>
		/// Raises the CellFormatting event. 
		/// </summary>
		/// <param name="e">A DataGridViewCellFormattingEventArgs that contains the event data.</param>
		protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
		{
			if (e.Value != null)
			{
				Type type = e.Value.GetType();
				if (type == typeof(int) || type == typeof(Int16) || type == typeof(Int64) || type == typeof(long) ||
					type == typeof(float) || type == typeof(double) || type == typeof(decimal))
				{
					e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				}
                
				if (type == typeof(bool))
				{
					if (this.Columns[e.ColumnIndex].CellType == typeof(DataGridViewTextBoxCell))
					{
						e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
						e.Value = (bool)e.Value ? "Да" : "Нет";
						e.FormattingApplied = true;
					}
				}
                 
			}
            
			//if (ExtType.IsNull(e.Value))
			//{
			//	e.Value = null;
			//}

			base.OnCellFormatting(e);
		}

		/// <summary>
		/// Get or set allow find.
		/// </summary>
		public bool AllowFind
		{
			get
			{
				return m_allowFind;
			}
			set
			{
				m_allowFind = value;
			}
		}

        //08/22/13 SDA
        /// <summary>
        /// Get or set allow export to Excel.
        /// </summary>
        public bool AllowExportToExcel
        {
            get
            {
                return m_allowExportToExcel;
            }
            set
            {
                m_allowExportToExcel = value;
            }
        }

		/// <summary>
		/// Raises the KeyDown event.
		/// </summary>
		/// <param name="e">A KeyEventArgs that contains the event data</param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
            /*
			if (m_allowFind && e.Control && e.KeyCode == Keys.F && this.CurrentCell != null && this.CurrentCell.ColumnIndex >= 0)
			{
				if (DataSource is IBindingList)
				{
					string name = this.Columns[this.CurrentCell.ColumnIndex].HeaderText;

					FrmFind frm = new FrmFind();
					frm.Text = "Find by '" + name + "' column";
					frm.StartPosition = FormStartPosition.Manual;
					Rectangle rect = RectangleToScreen(this.Bounds);

					int x = (rect.Right - rect.Left) / 2 + frm.Width / 2;
					int y = rect.Bottom - frm.Height;
					frm.Location = new Point(x, y);
					
					frm.FindTextChanged += new FrmFind.FindTextHandler(frm_FindTextChanged);
					frm.NextFindText += new FrmFind.FindTextHandler(frm_NextFindText);
					frm.ShowDialog(this.FindForm());
				}
			}
            //08/22/13 SDA
            else if (m_allowExportToExcel && e.Control && e.KeyCode == Keys.X && this.CurrentCell.ColumnIndex >= 0)
            {
                ExcelApplic _Excel = new ExcelApplic();
                _Excel.RunExportToExcel(this);
            }
            else
            {
                base.OnKeyDown(e);
            }
             */
            base.OnKeyDown(e);
		}

		void frm_NextFindText(object sender, string aText, bool aIgnoreCase)
		{
			if (DataSource is IBindingList && DataSource is ITypedList)
			{
				IBindingList list = DataSource as IBindingList;
				if (this.CurrentCell.ColumnIndex >= 0)
				{
					PropertyDescriptorCollection propList = (list as ITypedList).GetItemProperties(null);
					string pName = this.Columns[this.CurrentCell.ColumnIndex].DataPropertyName;
					PropertyDescriptor prop = propList.Find(pName, true);

					int start = (DataSource as BindingSource).Position + 1;
					int row = Find(list, prop, aText, aIgnoreCase, start);
					if (row < 0)
					{
                        if (VisualWinForm.Visual.Tools.ControlTools.ShowMessageExt(this, 
                            "[W]Поиск закончена. Хотите, чтобы начать поиск с начала сетке?"
                            , "Да;Нет") == 0)
						{
							row = Find(list, prop, aText, aIgnoreCase, 0);
						}
					}

					if (row >= 0)
					{
						(DataSource as BindingSource).Position = row;
					}
				}
			}
		}

		void frm_FindTextChanged(object sender, string aText, bool aIgnoreCase)
		{
			if (DataSource is IBindingList && DataSource is ITypedList)
			{
				IBindingList list = DataSource as IBindingList;
				if (this.CurrentCell.ColumnIndex >= 0)
				{
					PropertyDescriptorCollection propList = (list as ITypedList).GetItemProperties(null);
					string pName = this.Columns[this.CurrentCell.ColumnIndex].DataPropertyName;
					PropertyDescriptor prop = propList.Find(pName, true);

					int row = Find(list, prop, aText, aIgnoreCase, 0);
					if (row >= 0)
					{
						(DataSource as BindingSource).Position = row;
					}
				}
			}
		}

		private int Find(IList aList, PropertyDescriptor property, string key, bool aIgnoreCase, int aStartRow)
		{
			int row = -1;
			string strKey = key == null ? string.Empty : key;

			strKey = "^" + strKey.Replace(@"\", @"\\");
			strKey = strKey.Replace("*", ".*");
			strKey = strKey.Replace("%", ".*");

			try
			{
				Regex reg = new Regex(strKey, aIgnoreCase ? RegexOptions.Compiled | RegexOptions.IgnoreCase : RegexOptions.Compiled);

				for (int i = aStartRow; i < aList.Count; i++)
				{
					object obj = aList[i];
					object value = property.GetValue(obj);
					string strValue = value == null ? string.Empty : value.ToString();

					if (reg.IsMatch(strValue))//string.Compare(strValue, 0, strKey, 0, strKey.Length, aIgnoreCase) == 0)
					{
						row = i;
						break;
					}
				}
			}
			catch(Exception err)
			{
                VisualWinForm.Visual.Tools.ControlTools.ShowMessageExt(this.FindForm(), "[E]" + err.Message);
			}
			return row;
		}

		/// <summary>
		/// Sorts the contents of the DataGridView control in ascending or descending order based on the contents of the specified column. 
		/// </summary>
		/// <param name="dataGridViewColumn">The column by which to sort the contents of the DataGridView. </param>
		/// <param name="direction">One of the ListSortDirection values.</param>
		public override void Sort(DataGridViewColumn dataGridViewColumn, ListSortDirection direction)
		{
			Cursor cursor = this.Cursor;
			this.Cursor = Cursors.WaitCursor;
			try
			{
				base.Sort(dataGridViewColumn, direction);
			}
			finally
			{
				this.Cursor = cursor;
			}
		}

        public override void Sort(IComparer comparer)
        {
            Cursor cursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                base.Sort(comparer);
            }
            finally
            {
                this.Cursor = cursor;
            }               
        }
	}
}