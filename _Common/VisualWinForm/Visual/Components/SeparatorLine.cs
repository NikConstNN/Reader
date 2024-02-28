using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace VisualWinForm.Visual.Components
{
	/// <summary>
	/// Разделительная линия.
	/// </summary>
	public class SeparatorLine : UserControl
	{
		private const int MA_NOACTIVATEANDEAT = 4;
		private const int WM_MOUSEACTIVATE = 0x0021;

		private Border3DStyle m_borderStyle = Border3DStyle.Etched;
		private Border3DSide m_borderSide = Border3DSide.Top;

		/// <summary>
		/// Конструктор
		/// </summary>
        public SeparatorLine()
		{
			this.ResizeRedraw = true;
			this.Height = SystemInformation.Border3DSize.Height;
			this.SetStyle(ControlStyles.Selectable, false);
			this.TabStop = false;
		}

		/// <summary>
		/// Запрет реакции на мышь в режиме выполнения.
		/// </summary>
        /// <param name="aMessage">The Windows <see cref="Message"/> to process.</param>
		protected override void WndProc(ref Message aMessage)
		{
			if (aMessage.Msg == WM_MOUSEACTIVATE && !DesignMode)
			{
				aMessage.Result = (IntPtr)MA_NOACTIVATEANDEAT;
			}
			else
			{
				base.WndProc(ref aMessage);
			}
		}

		/// <summary>
        /// К каким сторонам SeparatorLine применять трехмерную границу (по умолчению Border3DSide.Top).
		/// </summary>
		public Border3DSide Border3DSide
		{
			get
			{
				return m_borderSide;
			}
			set
			{
				if (m_borderSide != value)
				{
					m_borderSide = value;
					this.Refresh();
				}
			}
		}

		/// <summary>
        /// Стиль границы (по умолчению Border3DStyle.Etched).
		/// </summary>
		public Border3DStyle Border3DStyle
		{
			get
			{
				return m_borderStyle;
			}
			set
			{
				if (m_borderStyle != value)
				{
					m_borderStyle = value;
					this.Refresh();
				}
			}
		}

		/// <summary>
        /// Перерисовка.
		/// </summary>
        /// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e) 
		{
			Rectangle rect;
			rect = new Rectangle(0, 0, this.Width, this.Height);
			ControlPaint.DrawBorder3D(e.Graphics, rect,	m_borderStyle, m_borderSide);
		}
	}
}
