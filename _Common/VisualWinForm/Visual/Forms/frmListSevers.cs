/*
 * Copyright (c) 11.2010, 2010 Information & Management Ltd. 
 *
 * NAME  frmListSevers.cs namespace BackupRestoreDB      SCOPE  
 * VERSION: 01.00.00.001                                   
 * DESCRIPTION:                                            
 *    Резервное копирование баз данных (MS SQL Server)
 *    Форма для показа и выбора существующих в сети серверов.
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
using System.Data.Sql;
using System.Data.SqlClient;

namespace VisualWinForm.Visual.Forms
{
    public partial class frmListSevers : Form
    {
        public frmListSevers()
        {
            InitializeComponent();
        }
        public static string Execute() 
        {
            return Execute(null, string.Empty);
        }
        public static string Execute(string pOldServer)
        {
            return Execute(null, pOldServer);
        }
        public static string Execute(IWin32Window owner)
        {
            return Execute(owner, string.Empty);
        }

        public static string Execute(IWin32Window owner, string pOldServer)
        {
            /*
            frmListSevers frm = new frmListSevers();
            try
            {
                frm.lboxServers.Items.Clear();
                SqlDataSourceEnumerator lSqlServ = SqlDataSourceEnumerator.Instance; //???
                System.Data.DataTable table = lSqlServ.GetDataSources();
                foreach (DataRow Row in table.Rows)
                    frm.lboxServers.Items.Add(Row.ItemArray[0].ToString() + (Row.ItemArray[1].ToString().Length > 0 ? "\\" + Row.ItemArray[1].ToString() : ""));
                frm.lboxServers.Sorted = true;
                frm.lboxServers.SelectedIndex = -1;
                if (owner != null && owner is Control)
                {
                    Control co = ((Control)owner);
                    Point po;
                    if (owner is Form)
                    {
                        frm.Icon = ((Form)owner).Icon;
                        po = new Point(co.Left + co.Size.Width + 6, co.Top);
                    }
                    else
                    {
                        po = co.PointToScreen(new Point(0, 0));
                        po.X += co.Size.Width + 3;
                        po.Y += 3;
                    }
                    frm.Font = co.Font;
                    frm.Top = po.Y;
                    frm.Left = po.X;
                    if (frm.Top < 0)
                        frm.Top = 0;
                    if (frm.Top + frm.Height + 3 > Screen.PrimaryScreen.WorkingArea.Height)
                        frm.Top = Screen.PrimaryScreen.WorkingArea.Height - (frm.Height + 3);
                    if (frm.Left < 0)
                        frm.Left = 0;
                    if (frm.Left + frm.Width + 3 > Screen.PrimaryScreen.WorkingArea.Width)
                        frm.Left = Screen.PrimaryScreen.WorkingArea.Width - (frm.Width + 3);
                }
                else
                    frm.StartPosition = FormStartPosition.CenterScreen;
                if (!string.IsNullOrWhiteSpace(pOldServer))
                { 
                    int ind = frm.lboxServers.Items.IndexOf(pOldServer);
                    frm.lboxServers.SelectedIndex = ind;
                }
                if (frm.ShowDialog() == DialogResult.OK && frm.lboxServers.SelectedIndex >= 0)
                    return frm.lboxServers.Items[frm.lboxServers.SelectedIndex].ToString();
                else
                    return string.Empty;
            }
            finally
            {
                frm.Dispose();
            }
            /**/
            return string.Empty;
        }


    }
}
