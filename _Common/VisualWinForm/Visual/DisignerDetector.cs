using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace VisualWinForm.Visual
{
	/// <summary>
    /// Проверка находится или Control в режиме дизайна.
	/// </summary>
	public sealed class DisignerDetector
	{
		private const int START_FRAME = 4;

		/// <summary>
        /// Проверить находится или Control в режиме дизайна.
		/// </summary>
        /// <param name="aControl">Control, для которого режим определяется.</param>
        /// <returns>true - control в режиме дизайна.</returns>
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
