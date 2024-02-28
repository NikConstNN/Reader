using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace VisualWinForm.Visual.Frames
{
	/// <summary>
	/// ������ frames
	/// </summary>
	public class FrameBase : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
        /// ����������� FrameBase class.
		/// </summary>
        public FrameBase()
		{
			InitializeComponent();
		}

		/// <summary>
        /// true - ������ � ������ �������.
		/// </summary>
		public bool IsDesignMode
		{
			get
			{
                return VisualWinForm.Visual.DisignerDetector.IsComponentInDesignMode(this);
			}
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
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			// 
            // FrameBase
			// 
            this.Name = "FrameBase";
			this.Size = new System.Drawing.Size(400, 300);
		}
	}
}
