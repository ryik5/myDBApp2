using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DBExplorer
{
    public partial class Form2 : Form
    {
        public bool UserButtonOK = false;

        public Form2 ()
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
            DBParameters.UserLogin = textBoxUserName.Text.Trim();
            DBParameters.UserPassword = textBoxPassword.Text.Trim();
            DBParameters.ServerName = textBoxServerName.Text.Trim();
            DBParameters.ServerType = comboBoxTypeDB.Text;
            DBParameters.Inputed = true;
            DBParameters.CheckBoxAuthorize = checkBoxAuthorize.Checked;
            Form2.ActiveForm.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            List<string> typeDB = new List<string>();
            typeDB.Add("MySQL");
            typeDB.Add("MSSQL");
            typeDB.Add("SQLEXPRESS");
            typeDB.Add("TFactura");

            comboBoxTypeDB.Items.AddRange(typeDB.ToArray());

            if (DBParameters.ServerType.Length > 0)
            { try { comboBoxTypeDB.SelectedIndex = comboBoxTypeDB.FindString(DBParameters.ServerType); } catch { } };
            textBoxUserName.Text = DBParameters.UserLogin;
            textBoxPassword.Text = DBParameters.UserPassword;
            textBoxServerName.Text = DBParameters.ServerName;
            buttonSave.Focus();
        }
    }
}
