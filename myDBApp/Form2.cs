using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DBAppIntellect
{

//http://www.cyberforum.ru/windows-forms/thread110436.html#post629892
//
//Data.Value = "111";
//

    public partial class Form2 : Form
    {
        public bool UserButtonOK = false;

        public Form2(Form1 f1) //for transfer any data between Form1 and Form12
        //public Form2()
        {
            InitializeComponent();
            checkBoxAuthorize.Checked = false;
        }

        private void checkBox1_CheckedChanged()
        {
            textBoxUserName.Enabled = false;
            textBoxPassword.Enabled = false;
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (checkBoxAuthorize.Checked == Enabled)
            {
                labelUser.Enabled = false;
                labelPassword.Enabled = false;
                textBoxUserName.Enabled = false;
                textBoxPassword.Enabled = false;
            }
            if (checkBoxAuthorize.Checked != Enabled)
            {
                textBoxUserName.Enabled = true;
                textBoxPassword.Enabled = true;
                labelUser.Enabled = true;
                labelPassword.Enabled = true;
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBoxUserName.Clear();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBoxPassword.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserLogin.Value = textBoxUserName.Text.Trim();
            UserPassword.Value = textBoxPassword.Text.Trim();
            UserButtonOK = true;
            Form2.ActiveForm.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            List<string> typeDB = new List<string>();
            typeDB.Add("TFactura");
            typeDB.Add("MSSQL");
            typeDB.Add("SQLEXPRESS");
            typeDB.Add("MySQL");
            
            comboBoxTypeDB.DataSource = typeDB;
            textBoxUserName.Text = "";
            textBoxPassword.Text = "";
            textBoxServerName.Text = "po-sql-01.corp.ais".ToUpper();
            buttonSave.Focus();
            UserLogin.Value = null;
            UserPassword.Value = null;
        }
    }
}
