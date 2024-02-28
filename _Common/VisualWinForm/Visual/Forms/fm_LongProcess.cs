/*
 * Copyright (c) 12.2011, 2011 Information & Management Ltd. 
 *
 * NAME  fm_LongProcess.cs namespace Ierarchi.WORKFORMS 
 * 
 * VERSION: 01.00.00.001                                   
 * 
 * DESCRIPTION:                                            
 *    Класс FormLongProcess для отражения длительных процессов, для 
 *    которых невозможно установить прогресс (чтоб юзер не скучал). 
 *    Форма НЕ модальная, запускается в доп. потоке.
 *    Вызов      - FormLongProcess.StartFormLongProcess
 *    Завершение - FormLongProcess.StopFormLongProcess
 *    
 *    Пока одновременно возможен запуск только ОДНОЙ формы.
 * 
 * AUTHOR: Николенко К.Н.                                  
 *                                                          
 * РАЗРАБОТАН НА ОСНОВЕ ПОСТАНОВОК И ЗАМЕЧАНИЙ:            
 *
 * MODIFIED
 * 
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Resources;

namespace VisualWinForm.Visual.Forms
{

    partial class frmLongProcess : Form
    {
        public frmLongProcess()
        {
            InitializeComponent();
            cbStop.Visible = false;
            this.ShowInTaskbar = false;
            Application.DoEvents();
        }

        private void cbStop_CheckedChanged(object sender, EventArgs e)
        {
            if (cbStop.Checked)
                Close();
        }
    }

    public class FormLongProcess
    {
        FormLongProcessParams _Params;
        bool _StopProcess = false;
        Thread _WorkerThread;
        static FormLongProcess Process;
        /// <summary>
        /// Создание формы в рабочем потоке, ее визуализация, обновление и удаление после остановки потока
        /// </summary>
        void DoWork()
        {
            frmLongProcess frm = new frmLongProcess();
            if (_Params.Text == null)
                frm.lbCaption.Text = string.Empty;
            else
                frm.lbCaption.Text = _Params.Text;
            if (_Params.Font != null)
                frm.lbCaption.Font = _Params.Font;
            frm.lbCaption.ForeColor = _Params.ColorText;
            if (_Params.Image == null) //стандартная анимация
            {
                ResourceManager rm = new ResourceManager(frm.GetType());
                frm.picBox.Image = (Image)rm.GetObject("wait_2"); 
            }
            else
                frm.picBox.Image = _Params.Image;
            frm.picBox.Size = frm.picBox.Image.Size;
            int h = frm.lbCaption.Height + 10 > frm.picBox.Height + 2 ? frm.lbCaption.Height + 10 : frm.picBox.Height + 2;
            h = ((int)(h / 2)) * 2;
            frm.Width = frm.picBox.Size.Width + frm.lbCaption.Width + (frm.lbCaption.Width == 0 ? 2 : 12);
            frm.picBox.Location = new Point(0, frm.lbCaption.Height + 10 > frm.picBox.Height + 2 ? (int)((h - frm.picBox.Height - 2) / 2) : 0);
            frm.lbCaption.Location = new Point(frm.picBox.Location.X + frm.picBox.Width + 5, (int)((h - frm.lbCaption.Height) / 2));
            frm.Show();
            frm.Height = h;
            if (!frm.CanFocus)
                frm.Focus();
            while (!_StopProcess)
                Application.DoEvents();
            frm.Hide();
            //Две строки ниже в оригинальном commom закомментированы, вызывающая форма иногда оставалась минимизированной
            //Возможно новые манипуляции с активизацией вызывающей формы помогут избежать эту проблему.
            frm.Close();
            frm.Dispose();
            //*********************************
            frm = null;
            Application.DoEvents();
        }
        /// <summary>
        /// Показать форму длительного процесса. 
        /// </summary>
        /// <param name="pText">Название процесса, отображаемое в форме</param>
        public static void StartFormLongProcess(string pText)
        {
            FormLongProcessParams par = new FormLongProcessParams(pText);
            StartFormLongProcess(par);
        }
        /// <summary>
        /// Показать форму длительного процесса.
        /// </summary>
        /// <param name="owner">Форма-родитель</param>
        /// <param name="pText">Название процесса, отображаемое в форме</param>
        /// <param name="DisableOwner">Сделать форму-родитель недоступной</param>
        public static void StartFormLongProcess(Form owner, string pText, bool DisableOwner)
        {
            FormLongProcessParams par = new FormLongProcessParams(pText, null, null, Color.Black, owner, DisableOwner);
            StartFormLongProcess(par);
        }
        /// <summary>
        /// Показать форму длительного процесса.
        /// </summary>
        /// <param name="pParams">Параметры формы</param>
        public static void StartFormLongProcess(FormLongProcessParams pParams)
        {
            if (Process != null || pParams == null) //не закрыт предыдущий процесс
                return;
            Process = new FormLongProcess();
            Process._Params = pParams;
            if (Process._Params.DisableOwner)
            {
                Process._Params.oldEnabled = Process._Params.Owner.Enabled;
                Process._Params.Owner.Enabled = false;
            }
            Process._WorkerThread = new Thread(Process.DoWork);
            Process._WorkerThread.IsBackground = true;
            Process._WorkerThread.Start();
            while (!Process._WorkerThread.IsAlive) ;
        }
        /// <summary>
        /// Закрыть форму длительного процесса. 
        /// </summary>
        public static void StopFormLongProcess()
        {
            if (Process == null || Process._WorkerThread == null) //процесс не запущен
                return;
            Process._StopProcess = true;
            Process._WorkerThread.Join();
            Process._WorkerThread = null;
            Form _frm = null; //передаем фокус вызывающей форме или активной или последней форме приложения
            if (Process._Params.Owner != null)
                _frm = Process._Params.Owner;
            else if (Form.ActiveForm != null)
                _frm = Form.ActiveForm;
            else if (Application.OpenForms.Count > 0)
                _frm = Application.OpenForms[Application.OpenForms.Count - 1];
            if (_frm != null && _frm.Visible)
                _frm.Activate();
            if (Process._Params.DisableOwner)
                Process._Params.Owner.Enabled = Process._Params.oldEnabled;
            Process = null;
            Application.DoEvents();
        }
    }
    /// <summary>
    /// Класс для передачи параметров форме LongProcess - Text, Картинка анимации, Font и цвет текста
    /// Параметры могут быть заданы в произвольной комбинации, в случае отсутствия используются стандартные значения.
    /// </summary>
    public class FormLongProcessParams
    {
        public FormLongProcessParams(string pText, Image pImage, Font pFont, Color pColorText, Form owner, bool disableOwner)
        {
            Font = pFont;
            Text = pText;
            Image = pImage;
            ColorText = pColorText;
            Owner = owner;
            DisableOwner = owner == null ? false : disableOwner;
        }
        public FormLongProcessParams(string pText) : this(pText, null, null, Color.Black, null, false) { }
        public FormLongProcessParams(string pText, Image pImage) : this(pText, pImage, null, Color.Black, null, false) { }
        public FormLongProcessParams(string pText, Image pImage, Font pFont) : this(pText, pImage, pFont, Color.Black, null, false) { }
        public FormLongProcessParams(string pText, Image pImage, Color pColorText) : this(pText, pImage, null, pColorText, null, false) { }
        public FormLongProcessParams(string pText, Font pFont) : this(pText, null, pFont, Color.Black, null, false) { }
        public FormLongProcessParams(string pText, Font pFont, Color pColorText) : this(pText, null, pFont, pColorText, null, false) { }
        public FormLongProcessParams(string pText, Color pColorText) : this(pText, null, null, pColorText, null, false) { }
        public Font Font { get; set; }
        public string Text { get; set; }
        public Image Image { get; set; }
        public Color ColorText { get; set; }
        public Form Owner { get; set; }
        public bool DisableOwner { get; set; }
        public bool oldEnabled { get; set; }
    }
}
