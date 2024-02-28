using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualWinForm.Visual.Components
{
    /// <summary>
    /// RichTextBox с расширенными возможностями
    /// </summary>
    public class RichTextBoxEx : RichTextBox
    {
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, Int32 msg, Int32 wParam, ref PARAFORMAT2 lParam);

        private const int SCF_SELECTION = 1;
        public const int PFM_LINESPACING = 256;
        public const int EM_SETPARAFORMAT = 1095;
        public const int EM_GETPARAFORMAT = 1085;
        /* https://www.eiffel.org/files/doc/static/22.05/libraries/wel/wel_rich_edit_message_constants_flatshort.html#f_em_getcharformat
    Em_getcharformat: INTEGER_32 = 1082
			-- Declared in Windows as EM_GETCHARFORMAT
	Em_geteventmask: INTEGER_32 = 1083
			-- Declared in Windows as EM_GETEVENTMASK
	Em_getimecolor: INTEGER_32 = 1129
			-- Declared in Windows as EM_GETIMECOLOR
	Em_getimeoptions: INTEGER_32 = 1131
			-- Declared in Windows as EM_GETIMEOPTIONS
	Em_getoptions: INTEGER_32 = 1102
			-- Declared in Windows as EM_GETOPTIONS
	Em_getparaformat: INTEGER_32 = 1085
			-- Declared in Windows as EM_GETPARAFORMAT
	Em_getpunctuation: INTEGER_32 = 1125
			-- Declared in Windows as EM_GETPUNCTUATION
	Em_getseltext: INTEGER_32 = 1086
			-- Declared in Windows as EM_GETSELTEXT
	Em_gettextrange: INTEGER_32 = 1099
			-- Declared in Windows as EM_GETTEXTRANGE
	Em_getwordbreakprocex: INTEGER_32 = 1104
			-- Declared in Windows as EM_GETWORDBREAKPROCEX
	Em_getwordwrapmode: INTEGER_32 = 1127
			-- Declared in Windows as EM_GETWORDWRAPMODE 
        
    Em_selectiontype: INTEGER_32 = 1090
			-- Declared in Windows as EM_SELECTIONTYPE
	Em_setbkgndcolor: INTEGER_32 = 1091
			-- Declared in Windows as EM_SETBKGNDCOLOR
	Em_setcharformat: INTEGER_32 = 1092
			-- Declared in Windows as EM_SETCHARFORMAT
	Em_seteventmask: INTEGER_32 = 1093
			-- Declared in Windows as EM_SETEVENTMASK
	Em_setimecolor: INTEGER_32 = 1128
			-- Declared in Windows as EM_SETIMECOLOR
	Em_setimeoptions: INTEGER_32 = 1130
			-- Declared in Windows as EM_SETIMEOPTIONS
	Em_setlimittext: INTEGER_32 = 197
			-- Declared in Windows as EM_SETLIMITTEXT
	Em_setoptions: INTEGER_32 = 1101
			-- Declared in Windows as EM_SETOPTIONS
	Em_setparaformat: INTEGER_32 = 1095
			-- Declared in Windows as EM_SETPARAFORMAT
	Em_setpunctuation: INTEGER_32 = 1124
			-- Declared in Windows as EM_SETPUNCTUATION
	Em_settargetdevice: INTEGER_32 = 1096
			-- Declared in Windows as EM_SETTARGETDEVICE
	Em_setwordbreakprocex: INTEGER_32 = 1105
			-- Declared in Windows as EM_SETWORDBREAKPROCEX
	Em_setwordwrapmode: INTEGER_32 = 1126
			-- Declared in Windows as EM_SETWORDWRAPMODE
	Em_streamin: INTEGER_32 = 1097
			-- Declared in Windows as EM_STREAMIN
	Em_streamout: INTEGER_32 = 1098
			-- Declared in Windows as EM_STREAMOUT
	En_selchange: INTEGER_32 = 1794
			-- Declared in Windows as EN_SELCHANGE

         */

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct PARAFORMAT2
        {
            public int cbSize;
            public uint dwMask;
            public Int16 wNumbering;
            public Int16 wReserved;
            public int dxStartIndent;
            public int dxRightIndent;
            public int dxOffset;
            public Int16 wAlignment;
            public Int16 cTabCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public int[] rgxTabs;
            public int dySpaceBefore;
            public int dySpaceAfter;
            public int dyLineSpacing;
            public Int16 sStyle;
            public byte bLineSpacingRule;
            public byte bOutlineLevel;
            public Int16 wShadingWeight;
            public Int16 wShadingStyle;
            public Int16 wNumberingStart;
            public Int16 wNumberingStyle;
            public Int16 wNumberingTab;
            public Int16 wBorderSpace;
            public Int16 wBorderWidth;
            public Int16 wBorders;
        }
        /// <summary>
        /// Установить Интервал между линиями
        /// </summary>
        /// <param name="bLineSpacingRule"></param>
        /// <param name="dyLineSpacing"></param>
        public void SetSelectionLineSpacing(byte bLineSpacingRule, int dyLineSpacing)
        {
            PARAFORMAT2 format = new PARAFORMAT2();
            format.cbSize = Marshal.SizeOf(format);
            format.dwMask = PFM_LINESPACING;
            format.dyLineSpacing = dyLineSpacing;
            format.bLineSpacingRule = bLineSpacingRule;
            SendMessage(this.Handle, EM_SETPARAFORMAT, SCF_SELECTION, ref format);
            //Invalidate();
        }

        /// <summary>
        /// Прочитать Интервал между линиями
        /// </summary>
        /// <returns></returns>
        public int GetSelectionLineSpacing()
        {
            PARAFORMAT2 format = new PARAFORMAT2();
            format.cbSize = Marshal.SizeOf(format);
            format.dwMask = PFM_LINESPACING;
            //format.dyLineSpacing = dyLineSpacing;
            //format.bLineSpacingRule = bLineSpacingRule;
            SendMessage(this.Handle, EM_GETPARAFORMAT, SCF_SELECTION, ref format);
            return format.dyLineSpacing;
        }
    }
}
