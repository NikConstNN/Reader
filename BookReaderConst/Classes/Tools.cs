using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualWinForm.Visual.Components;

namespace BookReaderConst
{

    public class BookReaderTools
    {
        BookReaderParam m_CurrentParams;
        public BookReaderTools(BookReaderParam pBookReaderParam) 
        {
            m_CurrentParams = pBookReaderParam;
        }

        public void Find(bool pNext, bool pPrev, RichTextBoxEx pFile = null)
        {
            /*
            if (pFile == null)
                pFile = m_CurrentParams.LastFile;
            if (pFile != null && m_ListTextBox.TryGetValue(pFile.ID, out List<Control>? textBox) && textBox != null)
            {
                RichTextBoxEx re = (RichTextBoxEx)textBox[0];
                if (re.Lines.Length > 0)
                {
                    if (pGoMark)
                    {
                        if (pMark < 0)
                        {
                            if (pFile.MarkLast >= 0)
                                pMark = pFile.MarkLast;
                            else
                                pMark = pFile.Marks.FirstOrDefault().Key;
                        }
                        if (pMark >= 0)
                            re.SelectionStart = (int)pMark;
                    }
                    else if (pSetMark)
                    {
                        int pos = re.SelectionStart;
                        string markTxt = $"{pos}  {(pos + 160 > re.Text.Length ? re.Text.Substring(pos) : re.Text.Substring(pos, 160) + "...")}";
                        //string markTxt = pos + 160 > re.Text.Length ? re.Text.Substring(pos) : re.Text.Substring(pos, 160) + "...";
                        markTxt = markTxt.Replace("\n", " ");
                        markTxt = markTxt.Replace("\r", " ");
                        if (markTxt.Trim().Length == 0)
                            markTxt = "пустая строка";
                        pFile.AddMark(pos, markTxt);
                    }
                    else
                    {
                        using FormMark fMark = new(pFile, re);
                        fMark.ShowDialog();
                    }
                }
            }
            */
        }
    }
}
