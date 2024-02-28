using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VisualWinForm.Visual.Components;
using VisualWinForm.Visual.Tools;


namespace VisualWinForm.Visual.Frames
{
	/// <summary>
    /// Визуализация данных на DataGridView.
	/// </summary>
	public class FrameDataGrid : FrameBase
	{
		private IContainer components = null;
        private ExDataGrid m_dataGrid;
		private bool m_allowSort;
		private BindingSource m_bindingSource;

		/// <summary>
		/// Конструктор FrameDataGrid.
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
		/// Инициализация стилей grid
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
        /// DataGridView control на frame.
		/// </summary>
        public DataGridView DataGrid
		{
			get
			{
				return m_dataGrid;
			}
		}

		/// <summary>
		/// Управление источником данных grid
		/// </summary>
		public virtual BindingSource CurrencyManager
		{
			get
			{
				return m_bindingSource;
			}
		}

		/// <summary>
		/// Возвращает true если источник данных не определен или пустой.
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return !(m_bindingSource != null && m_bindingSource.Count > 0);
			}
		}

		/// <summary>
        /// Gets или sets источник данных
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
        /// Добавление колонок в Grid 
		/// </summary>
		/// <param name="aPropertyName">Имя свойства визуализируетмого объекта.</param>
		/// <param name="aDisplayName">Заголовок колонки.</param>
        /// <param name="aWidht">Ширина колонки.</param>
		/// <param name="aFormat">Строка форматирования.</param>
        /// <param name="aPropertyType">Тип свойства визуализируемого объекта.</param>
        /// <param name="aAlignment">Значение Alignment для DefaultCellStyle.</param>
        /// <returns>Добавленная колонка.</returns>
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
        /// Добавить колоноку в Grid.
		/// </summary>
		/// <param name="aColumn">Колонка.</param>
		/// <returns>Индекс добавленной колонки.</returns>
		public int AddColumn(DataGridViewColumn aColumn)
		{
			return m_dataGrid.Columns.Add(aColumn);
		}

        /// <summary>
        /// Добавление колонок в Grid 
        /// </summary>
        /// <param name="aPropertyName">Имя свойства визуализируемого объекта.</param>
        /// <param name="aDisplayName">Заголовок колонки.</param>
        /// <param name="aWidht">Ширина колонки.</param>
        /// <param name="aFormat">Строка форматирования.</param>
        /// <returns>Добавленная колонка.</returns>
		public DataGridViewColumn MappingColumn(string aPropertyName, string aDisplayName, int aWidht, string aFormat)
		{
			return MappingColumn(aPropertyName, aDisplayName, aWidht, aFormat, null, DataGridViewContentAlignment.MiddleLeft);
		}

        /// <summary>
        /// Добавление колонок в Grid 
        /// </summary>
        /// <param name="aPropertyName">Имя свойства визуализируемого объекта.</param>
        /// <param name="aDisplayName">Заголовок колонки.</param>
        /// <param name="aWidht">Ширина колонки.</param>
        /// <param name="aAlignment">Значение Alignment для DefaultCellStyle.</param>
        /// <returns>Добавленная колонка.</returns>
		public DataGridViewColumn MappingColumn(string aPropertyName, string aDisplayName,	int aWidht,	DataGridViewContentAlignment aAlignment)
		{
			return MappingColumn(aPropertyName, aDisplayName, aWidht, "", null, aAlignment);
		}

        /// <summary>
        /// Добавление колонок в Grid 
        /// </summary>
        /// <param name="aPropertyName">Имя свойства визуализируемого объекта.</param>
        /// <param name="aDisplayName">Заголовок колонки.</param>
        /// <param name="aWidht">Ширина колонки.</param>
        /// <returns>Добавленная колонка.</returns>
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
		/// Очистка ресурсов.
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
        /// Создание контекстного меню для grid (пока один пункт "Экспорт данных в Excel")
        /// </summary>
        private void CreateContextMenu()
        {
            if (AllowExportToExcel) 
            {
                m_dataGrid.ContextMenuStrip = new ContextMenuStrip();
                m_dataGrid.ContextMenuStrip.Items.Add("Экспорт данных в Excel", null, cmContextMenu_Click).Tag = "EXCEL";
            }
        }
        /// <summary>
        /// Обрабратчик Click контекстного меню для grid
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


		#region Код сгенерированный дизайнером

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