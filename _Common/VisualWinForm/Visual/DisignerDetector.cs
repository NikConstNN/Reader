using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace VisualWinForm.Visual
{
	/// <summary>
    /// �������� ��������� ��� Control � ������ �������.
	/// </summary>
	public sealed class DisignerDetector
	{
		private const int START_FRAME = 4;

		/// <summary>
        /// ��������� ��������� ��� Control � ������ �������.
		/// </summary>
        /// <param name="aControl">Control, ��� �������� ����� ������������.</param>
        /// <returns>true - control � ������ �������.</returns>
		public static bool IsComponentInDesignMode(IComponent aControl)
		{
			if(aControl.Site != null )
			{
				return aControl.Site.DesignMode;
			}
			else
			{
				StackTrace stack = new StackTrace();
				int frameCount = stack.FrameCount - 1;

				for(int i = START_FRAME; i < frameCount; i++)
				{
					Type typeStack = stack.GetFrame(i).GetMethod().DeclaringType;
					if((typeof(IDesignerHost)).IsAssignableFrom(typeStack))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
