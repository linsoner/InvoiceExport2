﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiwangExport
{
    public partial class FormCredence : BaseForm
    {
        string ConnString { get; set; }

        public int Credtype { get; set; }
        public int Sub_C { get; set; }
        public int Sub_Tax { get; set; }
        public DateTime CredDate { get; set; }
        public string BillMaker { get; set; }
   
        public FormCredence()
        {
            InitializeComponent();
            cboAccount.SelectedIndexChanged += CboAccount_SelectedIndexChanged;
        }

        public void InitialDataSource(string connString)
        {
            ConnString = connString;
            try
            {
                GetAccounts();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace + "\r\nInitialDataSource");
            }
        }

        /// <summary>
        /// 获取速达账套
        /// </summary>
        public void GetAccounts()
        {
            DataTable table = SD3000.GetAccounts(ConnString);
            if (table != null)
            {
                cboAccount.DataSource = table;
                cboAccount.DisplayMember = "corpname";
                cboAccount.ValueMember = "accsetname";
            }
        }

        private void CboAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region 根据选择的账套修改数据库连接字符串
            string dbSuffix = string.Empty;
            dbSuffix = cboAccount.SelectedValue.ToString();
            if (string.IsNullOrWhiteSpace(dbSuffix)) return;

            string[] s = ConnString.Split(';');
            if (s.Length < 4) return;
            string[] s2 = s[1].Split('=');
            if (s2.Length < 2) return;

            string dbName = s2[1].Substring(0, s2[1].LastIndexOf('_')+1) + dbSuffix;

            ConnString = s[0] + ";" + s2[0] + "=" + dbName + ";" + s[2] + ";" + s[3];
            #endregion 根据选择的账套修改数据库连接字符串

            try
            {
                GetSubjects();
                GetCredTypes();
                GetOperator();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetSubjects()
        {
            if (string.IsNullOrWhiteSpace(ConnString))
                throw new ArgumentNullException("数据库链接字符串不能为空！");

            DataTable subjects = SD3000.GetSubjects(ConnString);
            if(subjects!=null)
            {
                DataTable table = subjects.Copy();

                cboTaxSSubject.DataSource = table;
                cboTaxSSubject.DisplayMember = "displayname";
                cboTaxSSubject.ValueMember = "subid";
                //DataRow[] rows = table.Select("")
                cboTaxSSubject.SelectedItem = table.Select("subcode='2221001'").First();

                cboIncomeSubject.DataSource = subjects;
                cboIncomeSubject.DisplayMember = "displayname";
                cboIncomeSubject.ValueMember = "subid";
                cboIncomeSubject.SelectedItem = table.Select("subcode='5001'").First();
            }
        }
        public void GetCredTypes()
        {
            DataTable table = SD3000.GetCredTypes(ConnString);
            if(table !=null)
            {
                cboCredType.DataSource = table;
                cboCredType.DisplayMember = "name";
                cboCredType.ValueMember = "id";
            }
        }

        public void GetOperator()
        {
            DataTable table = SD3000.GetOperators(ConnString);
            if (table != null)
            {
                cboBiller.DataSource = table;
                cboBiller.DisplayMember = "opname";
                cboBiller.ValueMember = "opname";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cboAccount.SelectedIndex == -1)
            {
                MessageBox.Show("请选择要导入的账套");
                cboAccount.Focus();
                return;
            }
            if (cboCredType.SelectedIndex == -1)
            {
                MessageBox.Show("请选择凭证字");
                cboCredType.Focus();
                return;
            }
            if (cboBiller.SelectedIndex == -1)
            {
                MessageBox.Show("请选择制单人");
                cboBiller.Focus();
                return;
            }
            if (cboIncomeSubject.SelectedIndex == -1)
            {
                MessageBox.Show("请选择主营业务收入科目");
                cboIncomeSubject.Focus();
                return;
            }
            if (cboTaxSSubject.SelectedIndex == -1)
            {
                MessageBox.Show("请选择应交税费科目");
                cboTaxSSubject.Focus();
                return;
            }

            int credtype = -1;
            if(int.TryParse(cboCredType.SelectedValue.ToString(),out credtype))
                Credtype = credtype;

            int sub_C = -1;
            if (int.TryParse(cboIncomeSubject.SelectedValue.ToString(), out sub_C))
                Sub_C = sub_C;

            int sub_Tax = -1;
            if (int.TryParse(cboTaxSSubject.SelectedValue.ToString(), out sub_Tax))
                Sub_Tax = sub_Tax;

            BillMaker= cboBiller.SelectedValue.ToString();

            CredDate = dteCredDate.Value;
    }

        private void btnGetSub_Click(object sender, EventArgs e)
        {
            if (cboAccount.SelectedIndex == -1)
            {
                if (cboAccount.SelectedIndex == -1)
                {
                    MessageBox.Show("请选择要导入的账套");
                    cboAccount.Focus();
                    return;
                }
            }

            DataTable table = dataGridView1.DataSource as DataTable;
            if(table == null)
            {
                MessageBox.Show("细表资料为空，无法获取科目");
                return;
            }

            foreach(DataRow row in table.Rows)
            {
                D
            }
        }
    }
}
