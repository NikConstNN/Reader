using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VisualWinForm.Visual.Components;
using VisualWinForm.Visual.Tools;


namespace VisualWinForm.Visual.Frames
{
	/// <summary>
    /// ������������ ������ �� DataGridView.
	/// </summary>
	public class FrameDataGrid : FrameBase
	{
		private IContainer components = null;
        private ExDataGrid m_dataGrid;
		private bool m_allowSort;
		private BindingSource m_bindingSource;

		/// <summary>
		/// ����������� FrameDataGrid.
		/// </summary>
		public FrameDataGrid()
		{
			InitializeComponent();
			m_allowSort = true;
            CreateContextMenu();
			if (!DesignMode)
			{
				InitGridStyle(m_dataGrid);
			}
		}
		/// <summary>
		/// Get or set allow find.
		/// </summary>
		public bool AllowFind
		{
			get
			{
				return m_dataGrid.AllowFind;
			}
			set
			{
				m_dataGrid.AllowFind = value;
			}
		}
        /// <summary>
        /// Get or set allow export to Excel.
        /// </summary>
        public bool AllowExportToExcel
        {
            get
            {
                return m_dataGrid.AllowExportToExcel;
            }
            set
            {
                m_dataGrid.AllowExportToExcel = value;
            }
        }
		/// <summary>
		/// Gets or sets the sort enable for the column.
		/// </summary>
		public bool AllowSort
		{
			get
			{
				return m_allowSort;
			}
			set
			{
				m_allowSort = value;
				foreach (DataGridViewColumn column in m_dataGrid.Columns)
				{
					if (m_allowSort)
					{
						column.SortMode = DataGridViewColumnSortMode.Automatic;
					}
					else
					{
						column.SortMode = DataGridViewColumnSortMode.NotSortable;
					}
				}
			}
		}

		/// <summary>
		/// ������������� ������ grid
		/// </summary>
		/// <param name="aDataGrid">DataGridView</param>
        protected virtual void InitGridStyle(DataGridView aDataGrid)
		{
			aDataGrid.AutoGenerateColumns = false;
			aDataGrid.AllowUserToOrderColumns = true;
			aDataGrid.AllowUserToAddRows = false;
			aDataGrid.AllowUserToDeleteRows = false;
			aDataGrid.AllowUserToResizeRows = false;
			aDataGrid.ReadOnly = true;
			aDataGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
			aDataGrid.MultiSelect = false;
			aDataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			//aDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font(aDataGrid.ColumnHeadersDefaultCellStyle.Font, FontStyle.Bold);
			aDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			aDataGrid.AllowUserToResizeRows = false;
			aDataGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			aDataGrid.RowHeadersWidth = 25;
			aDataGrid.RowTemplate.Height = 18;
			aDataGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
		}

		/// <summary>
        /// DataGridView control �� frame.
		/// </summary>
        public DataGridView DataGrid
		{
			get
			{
				return m_dataGrid;
			}
		}

		/// <summary>
		/// ���������� ���������� ������ grid
		/// </summary>
		public virtual BindingSource CurrencyManager
		{
			get
			{
				return m_bindingSource;
			}
		}

		/// <summary>
		/// ���������� true ���� �������� ������ �� ��������� ��� ������.
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return !(m_bindingSource != null && m_bindingSource.Count > 0);
			}
		}

		/// <summary>
        /// Gets ��� sets �������� ������
		/// </summary>
		public object DataSource
		{
			get
			{
				return m_dataGrid.DataSource;
			}
			set
			{
				m_bindingSource = new BindingSource();
				m_bindingSource.DataSource = value;
				m_dataGrid.DataSource = m_bindingSource;
				if (value != null)
				{
					//m_bindingSource.Ref
				}
			}
		}

		/// <summary>
        /// ���������� ������� � Grid 
		/// </summary>
		/// <param name="aPropertyName">��� �������� ����������������� �������.</param>
		/// <param name="aDisplayName">��������� �������.</param>
        /// <param name="aWidht">������ �������.</param>
		/// <param name="aFormat">������ ��������������.</param>
        /// <param name="aPropertyType">��� �������� ���������������� �������.</param>
        /// <param name="aAlignment">�������� Alignment ��� DefaultCellStyle.</param>
        /// <returns>����������� �������.</returns>
		public virtual DataGridViewColumn MappingColumn(string aPropertyName,
			string aDisplayName,
			int aWidht,
			string aFormat,
			Type aPropertyType,
			DataGridViewContentAlignment aAlignment)
		{
			DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
			DataGridViewColumn column = new DataGridViewColumn(cell);
			column.HeaderText = aDisplayName;
			column.DataPropertyName = aPropertyName;
			column.ValueType = aPropertyType;
			column.ReadOnly = true;
			column.DefaultCellStyle.Format = aFormat;
			column.DefaultCellStyle.Alignment = aAlignment;
			column.SortMode = DataGridViewColumnSortMode.Automatic;
			if (aWidht != 0)
			{
				column.Width = aWidht;
			}

			m_dataGrid.Columns.Add(column);
			return column;
		}

		/// <summary>
        /// �������� �������� � Grid.
		/// </summary>
		/// <param name="aColumn">�������.</param>
		/// <returns>������ ����������� �������.</returns>
		public int AddColumn(DataGridViewColumn aColumn)
		{
			return m_dataGrid.Columns.Add(aColumn);
		}

        /// <summary>
        /// ���������� ������� � Grid 
        /// </summary>
        /// <param name="aPropertyName">��� �������� ���������������� �������.</param>
        /// <param name="aDisplayName">��������� �������.</param>
        /// <param name="aWidht">������ �������.</param>
        /// <param name="aFormat">������ ��������������.</param>
        /// <returns>����������� �������.</returns>
		public DataGridViewColumn MappingColumn(string aPropertyName, string aDisplayName, int aWidht, string aFormat)
		{
			return MappingColumn(aPropertyName, aDisplayName, aWidht, aFormat, null, DataGridViewContentAlignment.MiddleLeft);
		}

        /// <summary>
        /// ���������� ������� � Grid 
        /// </summary>
        /// <param name="aPropertyName">��� �������� ���������������� �������.</param>
        /// <param name="aDisplayName">��������� �������.</param>
        /// <param name="aWidht">������ �������.</param>
        /// <param name="aAlignment">�������� Alignment ��� DefaultCellStyle.</param>
        /// <returns>����������� �������.</returns>
		public DataGridViewColumn MappingColumn(string aPropertyName, string aDisplayName,	int aWidht,	DataGridViewContentAlignment aAlignment)
		{
			return MappingColumn(aPropertyName, aDisplayName, aWidht, "", null, aAlignment);
		}

        /// <summary>
        /// ���������� ������� � Grid 
        /// </summary>
        /// <param name="aPropertyName">��� �������� ���������������� �������.</param>
        /// <param name="aDisplayName">��������� �������.</param>
        /// <param name="aWidht">������ �������.</param>
        /// <returns>����������� �������.</returns>
		public DataGridViewColumn MappingColumn(string aPropertyName, string aDisplayName, int aWidht)
		{
			return MappingColumn(aPropertyName, aDisplayName, aWidht, "", null, DataGridViewContentAlignment.MiddleLeft);
		}

		/// <summary>
        /// Refresh. 
		/// </summary>
		public override void Refresh()
		{
			base.Refresh();
			DataGrid.Refresh();
		}

		/// <summary>
        /// Invalidate.
		/// </summary>
		public new void Invalidate()
		{
			base.Invalidate();
			DataGrid.Invalidate();
		}

		/// <summary>
        /// Update.
		/// </summary>
		public new void Update()
		{
			base.Update();
			DataGrid.Update();
		}

		/// <summary>
		/// ������� ��������.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
                if (m_dataGrid != null && m_dataGrid.ContextMenuStrip != null)
                    m_dataGrid.ContextMenuStrip.Dispose();
			}
			base.Dispose(disposing);
		}
        /// <summary>
        /// �������� ������������ ���� ��� grid (���� ���� ����� "������� ������ � Excel")
        /// </summary>
        private void CreateContextMenu()
        {
            if (AllowExportToExcel) 
            {
                m_dataGrid.ContextMenuStrip = new ContextMenuStrip();
                m_dataGrid.ContextMenuStrip.Items.Add("������� ������ � Excel", null, cmContextMenu_Click).Tag = "EXCEL";
            }
        }
        /// <summary>
        /// ����������� Click ������������ ���� ��� grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmContextMenu_Click(object sender, EventArgs e)
        {
            string sTag = ((ToolStripItem)sender).Tag.ToString();
            switch (sTag)
            { 
                case "EXCEL" : 
                    {
                        ExcelApplic _Excel = new ExcelApplic();
                        _Excel.RunExportToExcel(this.m_dataGrid);
                    }
                    break;
            }
        }


		#region ��� ��������������� ����������

		private void InitializeComponent()
		{
            this.m_dataGrid = new ExDataGrid();
            ((System.ComponentModel.ISupportInitialize)(this.m_dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // m_dataGrid
            // 
            this.m_dataGrid.AllowExportToExcel = true;
            this.m_dataGrid.AllowFind = true;
            this.m_dataGrid.AllowUserToAddRows = false;
            this.m_dataGrid.AllowUserToDeleteRows = false;
            this.m_dataGrid.AllowUserToOrderColumns = true;
            this.m_dataGrid.AllowUserToResizeRows = false;
            this.m_dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dataGrid.Location = new System.Drawing.Point(0, 0);
            this.m_dataGrid.Name = "m_dataGrid";
            this.m_dataGrid.ReadOnly = true;
            this.m_dataGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dataGrid.Size = new System.Drawing.Size(400, 300);
            this.m_dataGrid.TabIndex = 0;
            // 
            // FrameDataGrid
            // 
            this.Controls.Add(this.m_dataGrid);
            this.Name = "FrameDataGrid";
            ((System.ComponentModel.ISupportInitialize)(this.m_dataGrid)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion
	}
}