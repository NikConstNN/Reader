/*
 * Copyright (c) 02.2014, 2014 Information & Management Ltd. 
 *
 * NAME  fm_AskValues.cs       SCOPE  
 * VERSION: 01.00.00.001                                   
 * DESCRIPTION:                                            
 * 
 *     Форма запроса произвольных значений.
 *     
 *     Обрабатывает значения типа: 
 *     "S" - текст, "N" - число со знаком и разделителем дес. части, "I" - беззнаковое целое число,
 *     "D" - дата, "DT" - дата-время, "T" - время,  "B" - bool,
 *     "C" - Выборка из ComboBox, "RG" - группа RadioButton
 *     По умолчанию "S" - текст.
 *     Описание полей ввода задается в списке FieldsValies с использованием класса DescriptionField (находится здесь же).
 *     В соответствии с описанием на форме строятся нужные компоненты.
 *     
 *     У формы есть 4 события:
 *     1) onAfterInitComponent - возникает после создания всех Control, перед визуализацией. 
 *        Сдесь можно порулить компонентами, например присвоить новые нач. значения или сделать какие-то недоступными
 *     2) onFieldChanged - изменилось значение поля
 *        Что-то можно сделать дополнительно, например порулить доступностью других компонентов, записать доп. значение.
 *     3) onSelectClick - нажата кнопка выбора. 
 *        Если используются кнопки выбора, обработчик для них должен быть определен в потомке или делегате (см. пример в методе Select_Click)
 *     4) onCheckingData - Проверка данных. Вызывается при нажатии ОК. Здесь проверяется только заполнение полей заданых как обязательные.
 *        Остальные проверки можно организовать по своему вкусу. Если поверка вернула false, остаемся в форме и ждем либо корректных значений, либо "Отмена"
 *        
 *     Выбранные/введенные значения будут записаны в Values списка ReturnValies, аналогному списку FieldsValies.
 *     
 *     Примеры вызова можно посмотреть в COMMON_VS\HierarchyForms\Forms\WORKFORMS\fm_SEQList.cs, метод tb_UpdateSEQ_Click или
 *     MR_VS\Forms\VS_Main.cs метод ac_import_grid (здесь попроще)
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
using System.IO;
using System.Windows.Forms;
using Common;
using VisualWinForm.Visual.Tools;

namespace VisualWinForm.Visual.Forms
{
    /// <summary>
    /// Форма запроса значений
    /// </summary>
    public partial class FormInputValues : Form
    {
        private NameObjects m_FieldsValies;
        private NameObjects m_ReturnValies;
        bool m_SetMaxWidth;

        #region События и делегаты

        /// <summary>
        /// Делегат события - нажата одна из кнопок выбора
        /// </summary>
        /// <param name="sender">Объект описания поля DescriptionField, на котором нажата кнопка</param>
        /// <param name="pFieldsValies">Список всех полей</param>
        public delegate void SelectClick(DescriptionField sender, NameObjects pFieldsValies);
        /// <summary>
        /// Событие - нажата одна из кнопок выбора
        /// </summary>
        public event SelectClick onSelectClick;

        /// <summary>
        /// Делегат события - изменилось значение поля
        /// </summary>
        /// <param name="sender">Объект описания поля DescriptionField, у которго изменилось значение. В CurrentValue будет новое значение</param>
        /// <param name="pFieldsValies">Список всех полей</param>
        public delegate void FieldChanged(DescriptionField sender, NameObjects pFieldsValies);
        /// <summary>
        /// Событие - изменилось значение поля
        /// </summary>
        public event FieldChanged onFieldChanged;

        /// <summary>
        /// Делегат события - после создания всех Control и присвоении им начальных значений
        /// </summary>
        /// <param name="pFieldsValies">Список всех полей</param>
        /// <param name="pThisForm"></param>
        public delegate void AfterInitComponent(NameObjects pFieldsValies, Form pThisForm);
        /// <summary>
        /// Событие - возникает после создания всех Control и присвоении им начальных значений
        /// </summary>
        public event AfterInitComponent onAfterInitComponent;

        /// <summary>
        /// Проверка данных. Вызывается при нажатии ОК, после проверки заполнения обязательных полей
        /// </summary>
        /// <param name="pFieldsValies">Список всех полей</param>
        /// <returns>Если возвращает false проверка прерывается, остаемся в форме</returns>
        public delegate bool CheckingData(NameObjects pFieldsValies);
        /// <summary>
        /// Событие - Проверка данных
        /// </summary>
        public event CheckingData onCheckingData;

        #endregion

        /// <summary>
        /// Для полей типа TextBox, GroupBox, и ComboBox установить Width по ширине максимального поля (по умолчанию false)
        /// </summary>
        public bool SetMaxWidth
        {
            get
            {
                return m_SetMaxWidth;
            }
            set
            {
                m_SetMaxWidth = value;
            }
        }

        /// <summary>
        /// Список отображаемых полей (имя поля, объект DescriptionField)
        /// </summary>
        public NameObjects FieldsValies
        {
            get
            {
                if (m_FieldsValies == null)
                    m_FieldsValies = new NameObjects();
                return m_FieldsValies;
            }
        }

        /// <summary>
        /// Список выбранных/введеных значений полей. Анологичен списку FieldsValies, заполняется в методе SetValues (при нажитии "OK")
        /// </summary>
        public NameObjects ReturnValies
        {
            get
            {
                if (m_ReturnValies == null)
                    m_ReturnValies = new NameObjects();
                return m_ReturnValies;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public FormInputValues() : this(null, string.Empty) { }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="pCaption">Заголовок</param>
        public FormInputValues(string pCaption) : this(null, pCaption) 
        {
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="owner">Вызывающая форма</param>
        /// <param name="pCaption">Заголовок</param>
        public FormInputValues(IWin32Window owner, string pCaption)
        {
            InitializeComponent();
            if (owner != null && owner is Form)
                Icon = ((Form)owner).Icon;
            if (!string.IsNullOrWhiteSpace(pCaption))
                Text = pCaption;
            else
                Text = "Запрос данных";
            btnOK.Text = "OK";
            btnCancel.Text = "Отмена";
            m_SetMaxWidth = false;
            this.ShowInTaskbar = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (OK_Click())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                //FieldsValies.Clear();
            }
            else
                this.DialogResult = System.Windows.Forms.DialogResult.None;
        }


        /// <summary>
        /// Инициализация компонентов
        /// </summary>
        protected virtual void InitComponent()
        {
            SetSeparatorLine();
            if (FieldsValies.Count == 0)
                return;
            DescriptionField field;
            int maxLabWidth = 0;
            int maxControlAndLabWidth = 0;
            //bool isButton = false;
            //дорабатываем переданные параметры полей и определяем max Width метки
            for (int i = 0; i < FieldsValies.Count; i++)
            {
                //Описание поля не задано. Создаем объект DescriptionField с параметрами по умолчанию. Это будет тип S, 200px, без метки
                if (FieldsValies[i] == null) 
                {
                    FieldsValies[i] = new DescriptionField(FieldsValies.NamesList[i]);
                }
                field = (DescriptionField)FieldsValies[i];
                //имена полей в DescriptionField балансируются с именами в FieldsValies
                if (!field.FieldName.Contains(FieldsValies.NamesList[i]))
                    field.FieldName = FieldsValies.NamesList[i];
                //есть метка и тип не RG
                if (!string.IsNullOrWhiteSpace(field.FieldLabel) && !field.FieldType.Equals("RG")) 
                {
                    int w = TextRenderer.MeasureText(field.FieldLabel, pnData.Font).Width;
                    if (maxLabWidth < w)
                        maxLabWidth = w;
                }
                //if (field.CreateSelectButton)
                //    isButton = true;
            }
            //Строим Control-s
            int CurrentLocY = 6;
            for (int i = 0; i < FieldsValies.Count; i++)
            {
                Control lComponent = null;
                field = (DescriptionField)FieldsValies[i];
                //Метка (заголовок поля). Для типа "RG" заголовок поля поместится в GroupBox.Text 
                if (!string.IsNullOrWhiteSpace(field.FieldLabel) && !field.FieldType.Equals("RG"))
                {
                    Label lb = new Label();
                    lb.AutoSize = true;
                    lb.Name = "lb_" + field.FieldName;
                    lb.Text = field.FieldLabel;
                    field.VisLabel = lb;
                    this.pnData.Controls.Add(lb);
                    lb.Location = new System.Drawing.Point(5, CurrentLocY + 3);
                }
                switch (field.FieldType)
                {
                    case "C" :
                        if (field.ListValues != null && field.ListValues.Count > 0)
                        {
                            ComboBox cb = new ComboBox();
                            cb.DropDownStyle = ComboBoxStyle.DropDownList;
                            cb.FormattingEnabled = true;
                            cb.Items.AddRange(field.ListValues.ToArray());
                            if (field.FieldMaxLenOrInitInd < cb.Items.Count)
                                cb.SelectedIndex = field.FieldMaxLenOrInitInd;
                            else
                            {
                                cb.SelectedIndex = -1;
                                field.FieldMaxLenOrInitInd = -1;
                            }
                            cb.SelectedIndexChanged += edChanged;
                            lComponent = cb;
                        }
                        break;
                    case "RG":
                        if (field.ListValues != null && field.ListValues.Count > 0)
                        {
                            field.AddValue = -1;
                            GroupBox gb = new GroupBox();
                            gb.AutoSize = true;
                            gb.Padding = new System.Windows.Forms.Padding(0);
                            if (!string.IsNullOrWhiteSpace(field.FieldLabel))
                                gb.Text = field.FieldLabel;
                            bool ch = false;
                            for (int k = 0; k < field.ListValues.Count; k++)
                            {
                                RadioButton rb = new RadioButton();
                                rb.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
                                rb.AutoSize = true;
                                rb.Location = new System.Drawing.Point(10, (k * 28) + 20);
                                rb.Name = "rb#" + field.FieldName + "#" + k.ToString();
                                rb.Text = field.ListValues[k].ToString();
                                if (k == field.FieldMaxLenOrInitInd)
                                {
                                    rb.Checked = true;
                                    ch = true;
                                }
                                rb.CheckedChanged += edChanged;
                                gb.Controls.Add(rb);
                            }
                            if (!ch)
                                field.FieldMaxLenOrInitInd = -1;
                            lComponent = gb;
                        }
                        break;
                    case "S":
                    case "N":
                    case "I":
                        TextBox ed = new TextBox();
                        if (field.ListValues != null && field.ListValues.Count > 0 && field.ListValues[0] != null)
                        {
                            string s = field.ListValues[0].ToString();
                            if (field.FieldMaxLenOrInitInd > 0 && s.Length > field.FieldMaxLenOrInitInd)
                                ed.Text = s.Substring(0, field.FieldMaxLenOrInitInd);
                            else
                                ed.Text = s;
                        }
                        if (field.FieldMaxLenOrInitInd > 0)
                            ed.MaxLength = field.FieldMaxLenOrInitInd;
                        if (field.FieldType.Equals("N"))
                            ed.KeyPress += new KeyPressEventHandler(VisualWinForm.Visual.Tools.ControlTools.OnlyNumberFilter);
                        if (field.FieldType.Equals("I"))
                            ed.KeyPress += new KeyPressEventHandler(VisualWinForm.Visual.Tools.ControlTools.OnlyDigitFilter);
                        ed.TextChanged += edChanged;
                        lComponent = ed;
                        break;
                    case "B":
                        CheckBox chb = new CheckBox();
                        chb.Checked = false;
                        if (field.ListValues != null && field.ListValues.Count > 0 && field.ListValues[0] != null)
                        {
                            bool b = false;
                            if (bool.TryParse(field.ListValues[0].ToString(), out b))
                                chb.Checked = b;
                        }
                        chb.CheckedChanged += edChanged;
                        lComponent = chb;
                        break;
                    case "DT":
                    case "D":
                    case "T":
                        DateTimePicker dtDate = new DateTimePicker();
                        if (field.IsRequired)
                        {
                            dtDate.Checked = true;
                            dtDate.ShowCheckBox = false;
                        }
                        else
                        {
                            dtDate.Checked = true;
                            dtDate.ShowCheckBox = true;
                        }                        
                        DateTime dt;
                        if (field.FieldType.Equals("D"))
                        {
                            dtDate.Format = DateTimePickerFormat.Short;
                            dt = DateTime.Today;
                        }
                        else
                        {
                            dtDate.Format = DateTimePickerFormat.Custom;
                            dt = DateTime.Now;
                            if (field.FieldType.Equals("T"))
                                dtDate.CustomFormat = "HH:mm:ss";
                            else
                                dtDate.CustomFormat = "dd.MM.yyyy HH:mm:ss";
                        }
                        if (field.ListValues != null && field.ListValues.Count > 0)
                        {
                            if (!DateTime.TryParse(field.ListValues[0].ToString(), out dt))
                            {
                                if (field.FieldType.Equals("D"))
                                    dt = DateTime.Today;
                                else
                                    dt = DateTime.Now;
                            }
                        }
                        dtDate.Value = dt;
                        dtDate.ValueChanged += edChanged;
                        lComponent = dtDate;
                        break;
                }
                //Кидаем Control на панель, устанавливаем его размеры и позицию 
                if (lComponent != null)
                {
                    bool IsLb = !string.IsNullOrWhiteSpace(field.FieldLabel) && !field.FieldType.Equals("RG");
                    lComponent.Name = field.FieldName;
                    lComponent.TabIndex = i;
                    this.pnData.Controls.Add(lComponent);
                    int locW;
                    if (!field.FieldType.Equals("RG"))
                        lComponent.Size = new System.Drawing.Size(field.FieldLength, 21);
                    lComponent.Location = new System.Drawing.Point((IsLb ? maxLabWidth + 8 : 5), CurrentLocY);
                    if (IsLb)
                        locW = lComponent.Size.Width + maxLabWidth + 3;
                    else
                        locW = lComponent.Size.Width; 
                    CurrentLocY = CurrentLocY + lComponent.Size.Height + 6;
                    field.VisControl = lComponent;
                    //кнопка, если надо
                    CreateSelectBtn(field);
                    if (locW > maxControlAndLabWidth)
                        maxControlAndLabWidth = locW;
                    field.CurrentValue = GetValueField(FieldsValies.NamesList[i]);
                }
            }
            if (onAfterInitComponent != null)
                onAfterInitComponent(FieldsValies, this);
            //Задано выравнивание по ширине максимального поля
            if (m_SetMaxWidth)
            {
                for (int i = 0; i < FieldsValies.Count; i++)
                {
                    field = (DescriptionField)FieldsValies[i];
                    if (field.VisControl is GroupBox || field.VisControl is TextBox || field.VisControl is ComboBox)
                    {
                        int h = field.VisControl.Size.Height;
                        int w = field.VisControl.Width;
                        if (field.VisLabel != null)
                            w += maxLabWidth + 3;
                        if (w < maxControlAndLabWidth)
                        {
                            //контрол с меткой
                            if (field.VisLabel != null)
                                w = maxControlAndLabWidth - maxLabWidth - 3;
                            else
                            {
                                w = maxControlAndLabWidth;
                                //GroupBox растянем на всю ширину + ширину кнопок 24 + 3 (если есть) ???
                                //if (field.VisControl is GroupBox && isButton)
                                //    w += 27;
                            }
                            field.VisControl.Size = new Size(w, h);
                            //двигаем кнопку
                            if (field.VisButton !=null)
                                field.VisButton.Location = new Point(field.VisControl.Location.X + field.VisControl.Size.Width + 3, field.VisControl.Location.Y - 2); 
                        }
                    }
                }
            }
            //кооректируем если надо размер (высоту) и выводим форму в центр экрана
            if (this.Size.Height > Screen.PrimaryScreen.WorkingArea.Height - 50)
            {
                int w = this.Size.Width + 20;
                this.AutoSize = false;
                this.Size = new Size(w, Screen.PrimaryScreen.WorkingArea.Height - 30);
                this.pnData.AutoScroll = true;
            }
            int x = (int)((Screen.PrimaryScreen.WorkingArea.Width - this.Width)/2);
            if (x < 0)
                x = 0;
            int y = (int)((Screen.PrimaryScreen.WorkingArea.Height - this.Height)/2);
            if (y < 0)
                y = 0;
            this.Location = new Point(x, y);
        }

        /// <summary>
        /// Создать рядом с переданным pField.VisControl кнопку выбора значения. Имя кнопки будет "btn_"+имя компонета(имя поля ввода). 
        /// </summary>
        /// <param name="pField">Объект класса описания для поля</param>
        private void CreateSelectBtn(DescriptionField pField)
        {
            if (pField.CreateSelectButton)
            {
                Button btn = new Button();
                btn.AutoSize = false;
                btn.Name = "btn_" + pField.VisControl.Name;
                btn.Size = new System.Drawing.Size(24, 24);
                System.Resources.ResourceManager rm = new System.Resources.ResourceManager(typeof(FormInputValues));
                btn.Image = (Image)rm.GetObject("Begin_SELECT_2"); 
                if (!string.IsNullOrWhiteSpace(pField.FieldLabel))
                    toolTip1.SetToolTip(btn, string.Format("Выбор '{0}'", pField.FieldLabel));
                btn.Click += btnSelect_Click;
                pField.VisButton = btn;
                pField.VisControl.Parent.Controls.Add(btn);
                btn.Location = new Point(pField.VisControl.Location.X + pField.VisControl.Size.Width + 3, pField.VisControl.Location.Y - 2);
            }
        }

        /// <summary>
        /// Нажата кнопка OK
        /// </summary>
        protected virtual bool OK_Click()
        {
            if (CheckData())
            {
                SetValues();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Проверка и возможное автоматическое изменение введенных данных. Стандартно вызывается из метода OK_Click().
        /// Здесь делается проверка только полей обязательных к заполнению. Дополнительные проверки реальзуется в потомке (если надо)
        /// </summary>
        /// <returns>Если true, при вызове из OK_Click(), заполняется список Valies и форма завершает работу.</returns>
        protected virtual bool CheckData()
        {
            for (int i = 0; i < FieldsValies.Count; i++)
            {
                DescriptionField fd = (DescriptionField)FieldsValies[i];
                if (fd.IsRequired) //поле обязательно к заполннению
                {
                    object val = GetValueField(FieldsValies.NamesList[i]);
                    if (val == null)
                    {
                        //ControlTools.ShowMessageExt(string.Format(lErr, "Не указан или указан несуществующий или недоступный", 1, AppParams.Path_Save_1));
                        ControlTools.ShowMessageExt(string.Format("[E]Поле '{0}' не может быть пустым", fd.FieldLabel));
                        return false;
                    }
                }
            }
            if (onCheckingData != null)
                return onCheckingData(FieldsValies);
            else
                return true;
        }

        /// <summary>
        /// Заполнить список ReturnValies возвращаемыми значениями. Стандартно вызывается из метода OK_Click(), после метода CheckData().
        /// </summary>
        protected virtual void SetValues()
        {
            ReturnValies.Clear();
            for (int i = 0; i < FieldsValies.Count; i++)
            {
                string lName = FieldsValies.NamesList[i];
                ReturnValies[lName] = GetValueField(lName);
            }
        }

        /// <summary>
        /// Введенное (выбранное) значение поля. Если поля с таким именем нет или значение не введено (не выбрано) - null
        /// </summary>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        protected virtual object GetValueField(string FieldName)
        { 
            object res = null;
            DescriptionField fd = (DescriptionField)FieldsValies[FieldName];
            if (fd != null)
            {
                Control ctrl = fd.VisControl;
                if (ctrl is TextBox)
                {
                    if (!string.IsNullOrWhiteSpace(ctrl.Text.Trim()))
                    {
                        if (fd.FieldType.Equals("I"))
                        {
                            int val;
                            if (int.TryParse(ctrl.Text.Trim(), out val))
                                res = val;
                        }
                        else if (fd.FieldType.Equals("N"))
                        {
                            double d;
                            if (double.TryParse(ctrl.Text.Trim(), out d))
                                res = d;
                        }
                        else
                            res = ctrl.Text.Trim();
                    }
                }
                else if (ctrl is ComboBox)
                {
                    if (((ComboBox)ctrl).SelectedIndex >= 0)
                        res = ((ComboBox)ctrl).Items[((ComboBox)ctrl).SelectedIndex];
                }
                else if (ctrl is GroupBox)
                {
                    int ind = GetIndexRadioGroup(FieldName);
                    if (ind >= 0)
                        res = fd.ListValues[ind];
                }
                else if (ctrl is CheckBox)
                {
                    res = (ctrl as CheckBox).Checked;
                }
                else if (ctrl is DateTimePicker)
                {
                    if (((DateTimePicker)ctrl).Checked)
                        res = (ctrl as DateTimePicker).Value;
                    //else
                    //    res = DateTime.MinValue;
                }
            }
            return res;
        }

        /// <summary>
        /// Вернуть индекс выбранного значения поля типа RG. Если поле с таким именем отсутствует или не RG или значение не выбрано возвращает -1
        /// </summary>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        protected virtual int GetIndexRadioGroup(string FieldName)
        {
            int res = -1;
            DescriptionField df = (DescriptionField)FieldsValies[FieldName];
            if (df != null && df.FieldType == "RG")
            {
                for (int i = 0; i < df.VisControl.Controls.Count; i++)
                {
                    if (((RadioButton)df.VisControl.Controls[i]).Checked)
                    {
                        string ss = df.VisControl.Controls[i].Name.Substring(df.VisControl.Controls[i].Name.LastIndexOf('#') + 1);
                        if (string.IsNullOrWhiteSpace(ss) || !int.TryParse(ss, out res))
                        {
                            res = -1;
                        }
                        break;
                    }
                }
            }
            return res;
        }


        /// <summary>
        /// Метод вызывается из события Load формы. Здесь выполняется инициализация компонентов по переданным параметрам 
        /// </summary>
        /// <returns></returns>
        protected virtual void LoadForm()
        {
            InitComponent();
        }

        /// <summary>
        /// Реализация обработчика кнопок выбора значений в потомке
        /// </summary>
        /// <param name="sender">Поле ввода, связаное с кнопкой</param>
        protected virtual void Select_Click(DescriptionField sender)
        {
            /* Пример реализации в потомке или в делегате
            switch (sender.Name) //имя поля ввода
			{
                //если нажата кнопка <btn_To> делаем... (тут вызывам класс-справочник методом s_View.RefExec и выбор записывам в TextBox и в AddValue)
				case "To": 
                    s_NamedObjects lLoc = null;
                    int id = To.Tag.obj_to_ID();
                    if (id > 0)
                        lLoc = new s_NamedObjects(TuningExchNmPrEntity.ColumnIdTuningExchNmPr, new object[] { id });
                    s_WinApplication.SelectionList.Clear();
                    s_View.RefExec("TuningExchNmPrData", lLoc, null, 1, true, string.Empty, null);
                    if (s_WinApplication.SelectionDone)
                    {
                        if (!s_WinApplication.EmptySelection)
                        {
                            //название записываем в виз. поле
                            sender.VisControl.Text = s_WinApplication.SelectionList[0][TuningExchNmPrEntity.ColumnNamePartner].obj_to_str();
                            //ID (если нужно) помещаем в AddValue
                            sender.AddValue = s_WinApplication.SelectionList[0][TuningExchNmPrEntity.ColumnIdTuningExchNmPr].obj_to_ID();
                        }
                    }
                    break;
                //если нажата кнопка <btn_Path> делаем... (тут вызывам форму выбора папки и выбор записывем в TextBox Path)
				case "Path":
                    folderDialog.SelectedPath = Const.LastOpenFolder;
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        sender.Text = folderDialog.SelectedPath;
                        Const.LastOpenFolder = folderDialog.SelectedPath;
                    }
                    break;
            //и т.д.
			}
             */
        }

        /// <summary>
        /// Обработчик кнопок выбора значений. Назначается всем автоматически сформированым кнопкам выбора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            //на каком поле нажата кнопка
            DescriptionField fd = (DescriptionField)FieldsValies[((Button)sender).Name.Substring(4)];
            if (fd != null && fd.VisControl is TextBox)
            {
                if (onSelectClick != null)
                    onSelectClick(fd, FieldsValies);
                Select_Click(fd);
            }
        }

        /// <summary>
        /// Поле редактируется
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edChanged(object sender, EventArgs e)
        {
            string fName = ((Control)sender).Name;
            int lInitInd = -100;
            if (sender is RadioButton)
            {
                string[] arr = fName.Split(new char[] {'#'});
                if (arr.Length < 3)
                    return;
                fName = arr[1];
                lInitInd = GetIndexRadioGroup(fName);
            }
            if (sender is ComboBox)
                lInitInd = ((ComboBox)sender).SelectedIndex;
            DescriptionField fd = (DescriptionField)FieldsValies[fName];
            if (fd != null)
            {
                fd.CurrentValue = GetValueField(fName);
                if (lInitInd >= -1)
                    fd.FieldMaxLenOrInitInd = lInitInd;
                if (onFieldChanged != null)
                    onFieldChanged(fd, FieldsValies);
            }
        }

        /// <summary>
        /// На Load формы формируем компоненты из списка FieldsValies, если они переданы 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fm_AskValues_Load(object sender, EventArgs e)
        {
            LoadForm();
        }

        #region Separator Line

        UserControl SeparatorLine;

        private void SetSeparatorLine()
        {
            SeparatorLine = new UserControl();
            SeparatorLine.Paint += OnPaint;
            SeparatorLine.TabStop = false;
            SeparatorLine.Name = "SeparatorLine";
            SeparatorLine.Size = new System.Drawing.Size(pnButton.Width, 3);
            pnButton.Controls.Add(SeparatorLine);
            SeparatorLine.Dock = DockStyle.Top;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Rectangle rect;
            rect = new Rectangle(0, 0, SeparatorLine.Width, SeparatorLine.Height);
            ControlPaint.DrawBorder3D(e.Graphics, rect, Border3DStyle.Etched, Border3DSide.All);
        }

        #endregion

    }

    /// <summary>
    /// Класс описания поля для формы
    /// </summary>
    public class DescriptionField
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="pFieldName">Имя поля</param>
        /// <param name="pFieldLabel">Заголовок поля</param>
        /// <param name="pFieldType">Тип поля. "S" - текст, "C" - ComboBox, "N" - число со знаком и разделителем дес. части, "I" - беззнаковое целое число, "D" - дата, "DT" - дата-время, "T" - время, "B" - bool, "RG" - группа RadioButton (по умолчанию "S" - текст)</param>
        /// <param name="pListValues">Список начальных значений. Тип "C" заполняется из списка, тип "RG" формируется на основании списка, для остальных типов берется 1-е значение</param>
        /// <param name="pCreateSelectButton">Формировать кнопку выбора. Актуально для типов "S", "N" и "I", для остальных игнорируется</param>
        /// <param name="pFieldLength">Длинна поля в px (по умолчанию 200 или если поле без заголовка 200 + длинна max заголовка)</param>
        /// <param name="pFieldMaxLenOrInitInd">Для типов "S", "N" и "I" Max длинна поля в символах. Для типов "C" и "RG" начальный выбраный индекс. Для остальных игнорирутся</param>
        /// <param name="pIsRequired">Поле обязательно к заполнению. Если установлен этот признак, делается проверка после нажатия кнопки ОК (метод CheckData)</param>
        public DescriptionField(string pFieldName, string pFieldLabel = null, string pFieldType = null, List<object> pListValues = null, bool pCreateSelectButton = false, int pFieldLength = -1, int pFieldMaxLenOrInitInd = 0, bool pIsRequired = false)
        {
            if (string.IsNullOrWhiteSpace(pFieldName))
                FieldName = string.Empty;
            else
                FieldName = pFieldName;

            if (string.IsNullOrWhiteSpace(pFieldLabel))
                FieldLabel = string.Empty;
            else
                FieldLabel = pFieldLabel;
            if (string.IsNullOrWhiteSpace(pFieldType))
                pFieldType = "S";
            pFieldType = pFieldType.ToUpper();
            if (!pFieldType.Equals("S") && !pFieldType.Equals("C") && !pFieldType.Equals("N") && !pFieldType.Equals("I")
                    && !pFieldType.Equals("D") && !pFieldType.Equals("DT") && !pFieldType.Equals("T") && !pFieldType.Equals("B") && !pFieldType.Equals("RG"))
                FieldType = "S";
            else
                FieldType = pFieldType;

            if (pFieldLength < 0)
            {
                if (FieldType.Equals("B"))
                    FieldLength = 30;
                else
                    FieldLength = 200;
            }
            else
                FieldLength = pFieldLength;
            FieldMaxLenOrInitInd = pFieldMaxLenOrInitInd;
            ListValues = pListValues;
            if (pCreateSelectButton && (FieldType.Equals("S") || FieldType.Equals("N") || FieldType.Equals("I")))
                CreateSelectButton = true;
            else
                CreateSelectButton = false;
            IsRequired = pIsRequired;
        }
        /// <summary>
        /// Имя поля. 
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Заголовок поля. Используется как Text соответствующей Label. Если пусто или null, Label не формируется
        /// </summary>
        public string FieldLabel { get; set; }
        /// <summary>
        /// Тип поля. "S" - текст, "C" - ComboBox, "N" - число со знаком и разделителем дес. части, "I" - беззнаковое целое число, "D" - дата, "DT" - дата-время, "T" - время, "B" - bool, "RG" - группа RadioButton
        /// </summary>
        public string FieldType { get; set; }
        /// <summary>
        /// Список начальных значений. Тип ComboBox заполняется из списка, тип RG формируется на основании списка, для остальных типов берется 1-е значение
        /// </summary>
        public List<object> ListValues { get; set; }
        /// <summary>
        /// Формировать кнопку выбора. Актуально для типов "S", "N" и "I", для остальных игнорируется
        /// </summary>
        public bool CreateSelectButton { get; set; }
        /// <summary>
        /// Длинна поля ввода в px (по умолчанию 200 или 30 для поля "B")
        /// </summary> 
        public int FieldLength { get; set; }
        /// <summary>
        /// Для типов "S", "N" и "I" Max длинна поля в символах. Для типов "C" и "RG" начальный выбраный индекс. Для остальных игнорирутся.
        /// Для типов "C" и "RG" в процессе выбора содержит индекс текущего выбранного Item
        /// </summary>
        public int FieldMaxLenOrInitInd { get; set; }
        /// <summary>
        /// Поле обязательно к заполнению.
        /// </summary>
        public bool IsRequired { get; set; }
        /// <summary>
        /// Компонент для ввода поля
        /// </summary>
        public Control VisControl { get; set; }
        /// <summary>
        /// Label заголовка поля
        /// </summary>
        public Label VisLabel { get; set; }
        /// <summary>
        /// Button кнопка для выбора значения
        /// </summary>
        public Button VisButton { get; set; }
        /// <summary>
        /// Текущее значение поля
        /// </summary>
        public object CurrentValue { get; set; }
        /// <summary>
        /// Текущее дополнительное значение поля. Сюда можно записать доп. значения, если для поля предполагается выбор не одного зачения (например ID + наименование)
        /// Заполнение надо организовать в обработчике onSelectClick или onFieldChanged.
        /// </summary>
        public object AddValue { get; set; }
    }
}
