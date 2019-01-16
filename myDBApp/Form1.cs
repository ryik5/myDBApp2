using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace DBAppIntellect
{
    public partial class Form1 : Form
    {
        //http://www.cyberforum.ru/windows-forms/thread110436.html#post629892
        //http://www.realcoding.net/article/view/2611

        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private System.Diagnostics.FileVersionInfo myFileVersionInfo;
        private string strVersion;

        private string UserLogin =  "";
        private string UserPassword = "";
        private string UserWindowsAuthorization = ";" + "Persist Security Info=True";
        private string ServerName = "";
        private string ServerType = "MSSQL";
        private string connection = "";
        private string serverDB = "";
        private string serverDbTable = "";
        private string serverDbTableColumn = "";


        public Form1()
        { InitializeComponent(); }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Log test block
            logger.Trace("Test trace message");
            logger.Debug("Test debug message");
            logger.Info("Test info message");
            logger.Warn("Test warn message");
            logger.Error("Test error message");
            logger.Fatal("Test fatal message");

            myFileVersionInfo = myFileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
            strVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            toolTip1.SetToolTip(textBoxQuery, "Введите параметр для фильтра выводимых данных");
            StatusLabel1.Alignment = ToolStripItemAlignment.Right;
            StatusLabel1.Text = myFileVersionInfo.Comments + " ver." + myFileVersionInfo.FileVersion + " b." + strVersion + " " + myFileVersionInfo.LegalCopyright;
            StatusLabel1.Alignment = ToolStripItemAlignment.Right;

            comboBoxTables.Enabled = false;
            button1.Enabled = false;
            comboBoxColumns.Enabled = false;
            textBoxQuery.Enabled = false;
            GetRows.Enabled = false;

            string checkServerName = "";
            string checkServerType = "";
            //The Start of The Block. for transfer any data between Form1 and Form2
            this.Hide();
            Form2 f = new Form2(this);
            f.buttonSave.Focus();
            f.ShowDialog();
            this.Show();

            if (f.UserButtonOK)
            {
                if (f.checkBoxAuthorize.Checked)
                {
                    UserWindowsAuthorization = ";Integrated Sercurity = true";
                    UserLogin = null;
                    UserPassword = null;
                }
                else
                {
                    UserWindowsAuthorization = ";Persist Security Info=True";
                    UserLogin = f.textBoxUserName.Text.Trim();
                    UserPassword =  f.textBoxPassword.Text.Trim();
                }
                checkServerName = f.textBoxServerName.Text.Trim();
                checkServerType = f.comboBoxTypeDB.Text.Trim();

                if (checkServerName.Length > 3)
                {
                    ServerName = checkServerName;
                    ServerType = checkServerType;
                    comboBoxServers.Items.Add(ServerName);
                    comboBoxServers.SelectedIndex = comboBoxServers.FindString(ServerName);
                    ComboBoxServers_SelectedIndexChanged();
                }
            }
            else
            {
                StatusLabel2.Text = "Выберите сервер для получения данных";
            }
            //The End of The Block. for transfer any data between Form1 and Form2
        }


        private void SetStartStatus()
        {
            comboBoxServers.SelectedIndex = 0;
            comboBoxDBs.SelectedIndex = 0;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        { ComboBoxServers_SelectedIndexChanged(); }

        private void ComboBoxServers_SelectedIndexChanged()
        {
            HashSet<string> list = new HashSet<string>();
            DataTable dt = new DataTable();

            //Start Timer //https://msdn.microsoft.com/ru-ru/library/system.data.sqlclient.sqlconnection.connectionstring(v=vs.110).aspx
            timer2.Enabled = true;

            if (ServerName.Length == 0)
            { ServerName = comboBoxServers.Text; }
            StatusLabel2.Text = "Подключаюсь к серверу " + ServerName;
            logger.Info("Подключаюсь к серверу " + ServerName);

            try
            {
                if (ServerName.ToLower().Contains("po-sql-01") || ServerName.ToLower().Contains("tfactura"))
                {
                    connection = @"Data Source=" + ServerName + ";Initial Catalog=EBP" + UserWindowsAuthorization + ";User ID=" + UserLogin + ";Password=" + UserPassword + ";Connect Timeout=5";
                    using (System.Data.SqlClient.SqlConnection dbCon = new System.Data.SqlClient.SqlConnection(connection))
                    {
                        dbCon.Open();

                        dt = dbCon.GetSchema("Databases");
                        foreach (DataRow r in dt.Rows)
                        { list.Add(r[0].ToString()); }

                        dbCon.Close();
                    }
                }
                else if (ServerType == "MSSQL")
                {
                    connection = @"Data Source=" + ServerName + UserWindowsAuthorization + ";User ID=" + UserLogin + ";Password=" + UserPassword + ";Connect Timeout=5";
                    using (System.Data.SqlClient.SqlConnection dbCon = new System.Data.SqlClient.SqlConnection(connection))
                    {
                        dbCon.Open();

                        dt = dbCon.GetSchema("Databases");
                        foreach (DataRow r in dt.Rows)
                        { list.Add(r[0].ToString()); }

                        dbCon.Close();
                    }
                }
                else if (ServerType == "SQLEXPRESS")
                {
                    connection = @"Data Source=" + ServerName + "\\SQLEXPRESS" + UserWindowsAuthorization + ";User ID=" + UserLogin + ";Password=" + UserPassword + ";Connect Timeout=5";
                    StatusLabel2.Text = "Подключаюсь к серверу " + ServerName;
                    using (System.Data.SqlClient.SqlConnection dbCon = new System.Data.SqlClient.SqlConnection(connection))
                    {
                        dbCon.Open();

                        dt = dbCon.GetSchema("Databases");
                        foreach (DataRow r in dt.Rows)
                        { list.Add(r[0].ToString()); }

                        dbCon.Close();
                    }
                }
                else if (ServerType == "MySQL")
                {
                    //change connection to MySQL
                    connection = @"server=" + ServerName + ";User=" + UserLogin + ";Password=" + UserPassword + ";database=mysql;Connect Timeout=5";
                    StatusLabel2.Text = "Подключаюсь к серверу " + ServerName;
                    string query = "SELECT * FROM db where user='" + UserLogin + "' and select_priv='Y'";

                    using (MySql.Data.MySqlClient.MySqlConnection dbCon = new MySql.Data.MySqlClient.MySqlConnection(connection))
                    {
                        dbCon.Open();

                        using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, dbCon))
                        {
                            using (MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (reader.GetString(0) != null && reader.GetString(0).Length > 0)
                                    { list.Add(reader.GetString(1)); }
                                }
                            }
                        }
                        dbCon.Close();
                    }
                }

                logger.Info("Список баз сервера " + ServerName + " успешно получено");
                StatusLabel2.Text = "Список баз сервера " + ServerName + " успешно получено";

                comboBoxDBs.Items.AddRange(list.ToArray());
                comboBoxDBs.SelectedIndex = 0;

                GetTables.Enabled = false;
                comboBoxTables.Enabled = false;
                button1.Enabled = false;
                comboBoxColumns.Enabled = false;
                textBoxQuery.Enabled = false;
                GetRows.Enabled = false;
            } catch (Exception expt)
            {
                logger.Warn("Проверьте правильность - " + ServerName + ", " + ServerType + ", Логин - " + UserLogin + ", Пароль - " + UserPassword);
                StatusLabel2.Text = "Ошибка доступа к" + ServerName + " " + ServerType + "|Логин - " + UserLogin + "|Пароль - " + UserPassword;
                MessageBox.Show("Проверьте имя сервера/пользователя/пароль:\nСервер и его тип - " +
                  ServerName + " " + ServerType + "\nЛогин - " + UserLogin + "\nПароль - " + UserPassword +
                  "\n-----------------------" + "\n" + expt.ToString());
            }

            /*
               using (MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd))
               {
               DataTable dt = new DataTable();
             da.Fill(dt);
             foreach (DataRow r in dt.Rows)
          { list.Add(r[1].ToString()); }
        }*/

        }

        private void GetTables_Click(object sender, EventArgs e)
        {
            HashSet<string> list = new HashSet<string>();
            DataTable dt = new DataTable();
            serverDB = comboBoxDBs.SelectedItem.ToString();
            logger.Info("Запрашиваю список таблиц с базы " + serverDB + " сервера " + ServerName);

            comboBoxTables.Enabled = true;
            button1.Enabled = false;
            comboBoxColumns.Enabled = false;
            textBoxQuery.Enabled = false;
            GetRows.Enabled = false;
            comboBoxServers.Enabled = false;
            comboBoxDBs.Enabled = false;

            comboBoxTables.Items.Clear();
            //string conn = DBAppIntellect.Properties.Settings.Default.intellectConnection;  // Native Connection
            //https://msdn.microsoft.com/ru-ru/library/system.data.sqlclient.sqlconnection.connectionstring(v=vs.110).aspx
            try
            {
                if (ServerName.ToLower().Contains("po-sql-01") || ServerName.ToLower().Contains("tfactura") || ServerType == "MSSQL2005")
                {
                    connection = @"Data Source=" + ServerName + ";Initial Catalog=" + serverDB + UserWindowsAuthorization + ";User ID=" + UserLogin + ";Password=" + UserPassword + ";Connect Timeout=60";
                    using (System.Data.SqlClient.SqlConnection dbCon = new System.Data.SqlClient.SqlConnection(connection))
                    {
                        dbCon.Open();

                        dt = dbCon.GetSchema("Tables");
                        foreach (DataRow row in dt.Rows)
                        { list.Add(row["TABLE_NAME"].ToString()); }

                        dbCon.Close();
                    }
                }
                else if (ServerType == "SQLEXPRESS")
                {
                    connection = @"Data Source=" + ServerName + "\\SQLEXPRESS" + ";Initial Catalog=" + serverDB + UserWindowsAuthorization + ";User ID=" + UserLogin + ";Password=" + UserPassword + ";Connect Timeout=60";
                    using (System.Data.SqlClient.SqlConnection dbCon = new System.Data.SqlClient.SqlConnection(connection))
                    {
                        dbCon.Open();

                        dt = dbCon.GetSchema("Tables");
                        foreach (DataRow row in dt.Rows)
                        { list.Add(row["TABLE_NAME"].ToString()); }

                        dbCon.Close();
                    }
                }
                else if (ServerType == "MySQL")
                {
                    connection = @"server=" + ServerName + ";User=" + UserLogin + ";Password=" + UserPassword + ";database=" + serverDB + ";Connect Timeout=5";
                    string query = "SHOW TABLES";
                    //string query = "SHOW TABLES from serverDB";
                    using (MySql.Data.MySqlClient.MySqlConnection dbCon = new MySql.Data.MySqlClient.MySqlConnection(connection))
                    {
                        dbCon.Open();

                        using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, dbCon))
                        {
                            using (MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (reader.GetString(0) != null && reader.GetString(0).Length > 0)
                                        list.Add(reader.GetString(0));
                                }
                            }
                        }
                        dbCon.Close();
                    }
                }
                logger.Info("Список таблиц c базы " + serverDB + " сервера " + ServerName + " успешно получены");
                StatusLabel2.Text = "Список таблиц c базы " + serverDB + " сервера " + ServerName + " успешно получены";
            } catch (Exception expt)
            {
                logger.Warn("Проверьте доступ к чтению списка таблиц: " + ServerName + ", " + ServerType + ", Логин - " + UserLogin + ", Пароль - " + UserPassword);

                MessageBox.Show("Проверьте доступ к чтению списка таблиц:\nСервер - " +
                  ServerName + " " + ServerType + "\nЛогин - " + UserLogin + "\nПароль - " + UserPassword +
                  "\n-----------------------" + "\n" + expt.ToString());
            }

            comboBoxTables.Items.AddRange(list.ToArray());
            comboBoxTables.SelectedIndex = 0;
        }

        private void GetColumns_Click(object sender, EventArgs e)
        {
            comboBoxColumns.Enabled = true;
            textBoxQuery.Enabled = false;
            GetRows.Enabled = false;
            comboBoxColumns.Items.Clear();

            serverDbTable = comboBoxTables.SelectedItem.ToString();
            HashSet<string> list = new HashSet<string>();
            DataTable dt = new DataTable();

            logger.Info("Запрашиваю список столбцов таблицы " + serverDbTable + " базы " + serverDB + " сервера " + ServerName);
            StatusLabel2.Text = "Запрашиваю список столбцов таблицы " + serverDbTable + " базы " + serverDB + " сервера " + ServerName;

            try
            {
                if (ServerName.ToLower().Contains("po-sql-01") || ServerName.ToLower().Contains("tfactura") || ServerType == "MSSQL2005")
                {
                    connection = @"Data Source=" + ServerName + ";Initial Catalog=" + serverDB + ";Type System Version=SQL Server 2005" + UserWindowsAuthorization + ";User ID=" + UserLogin + ";Password=" + UserPassword + ";Connect Timeout=60";

                    using (System.Data.SqlClient.SqlConnection dbCon = new System.Data.SqlClient.SqlConnection(connection))
                    {
                        dbCon.Open();

                        dt = dbCon.GetSchema("Columns", new[] { null, null, serverDbTable });
                        foreach (DataRow row in dt.Rows)
                        { list.Add(row["COLUMN_NAME"].ToString()); }

                        dbCon.Close();
                    }
                }
                else if (ServerType == "SQLEXPRESS")
                {
                    connection = @"Data Source=" + ServerName + "\\SQLEXPRESS" + ";Initial Catalog=" + serverDB + UserWindowsAuthorization + ";User ID=" + UserLogin + ";Password=" + UserPassword + ";Connect Timeout=60";
                    StatusLabel2.Text = "Подключаюсь к серверу " + ServerName;

                    using (System.Data.SqlClient.SqlConnection dbCon = new System.Data.SqlClient.SqlConnection(connection))
                    {
                        dbCon.Open();

                        dt = dbCon.GetSchema("Columns", new[] { null, null, serverDbTable });
                        foreach (DataRow row in dt.Rows)
                        { list.Add(row["COLUMN_NAME"].ToString()); }

                        dbCon.Close();
                    }
                }
                else if (ServerType == "MySQL")
                {
                    connection = @"server=" + ServerName + ";User=" + UserLogin + ";Password=" + UserPassword + ";database=" + serverDB + ";Connect Timeout=5";
                    string query = "SHOW COLUMNS FROM " + serverDbTable;
                    //string query = "SHOW TABLES from serverDB";
                    using (MySql.Data.MySqlClient.MySqlConnection dbCon = new MySql.Data.MySqlClient.MySqlConnection(connection))
                    {
                        dbCon.Open();

                        using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, dbCon))
                        {
                            using (MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (reader.GetString(0) != null && reader.GetString(0).Length > 0)
                                    { list.Add(reader.GetString(0)); }
                                }
                            }
                        }
                        dbCon.Close();
                    }
                }

                logger.Info("Список столбцов таблицы " + serverDbTable + " для базы " + serverDB + " сервера " + ServerName + " получен");
                StatusLabel2.Text = "Список столбцов таблицы " + serverDbTable + " для базы " + serverDB + " сервера " + ServerName + " получен";

                comboBoxColumns.Items.AddRange(list.ToArray());
                comboBoxColumns.SelectedIndex = 0;
            } catch (Exception expt)
            {
                logger.Warn("Проверьте доступ к чтению списка столбцов: " + ServerName + ", " + ServerType + ", Логин - " + UserLogin + ", Пароль - " + UserPassword);

                MessageBox.Show("Проверьте доступ к чтению списка столбцов:\nСервер - " +
                  ServerName + " " + ServerType + "\nЛогин - " + UserLogin + "\nПароль - " + UserPassword +
                  "\n-----------------------" + "\n" + expt.ToString());
            }
        }

        private void GetInfo_Click(object sender, EventArgs e)
        {
            timer2.Enabled = true;

            serverDbTableColumn = comboBoxColumns.SelectedItem.ToString();
            DataTable dt = new DataTable();
            string query = "";

            logger.Info("Запрашиваю данные таблицы " + serverDbTable + " базы " + serverDB + " сервера " + ServerName);
            StatusLabel2.Text = "Запрашиваю данные таблицы " + serverDbTable + " базы " + serverDB + " сервера " + ServerName;

            try
            {
                //Next string - work!!!!
                //string conn = DBAppIntellect.Properties.Settings.Default.intellectConnection; //Native Connection
                //string conn = "Data Source=KV-BASE\\SQLEXPRESS;Initial Catalog=intellect;Persist Security Info=True;User ID=xcvxcvx;Password=sdfsf;Connect Timeout=60";
                //string conn = "Data Source=" + comboBox3.SelectedItem.ToString() + "\\SQLEXPRESS;Persist Security Info=True;User ID=sdsdfsdf;Password=ghghjgh;Connect Timeout=60";

                if (ServerName.ToLower().Contains("po-sql-01") || ServerName.ToLower().Contains("tfactura") || ServerType == "MSSQL2005")
                {
                    connection = @"Data Source=" + ServerName + ";Initial Catalog=" + serverDB + ";Type System Version=SQL Server 2005" + UserWindowsAuthorization + ";User ID=" + UserLogin + ";Password=" + UserPassword + ";Connect Timeout=60";
                    query = "SELECT TOP 100 * FROM " + serverDbTable + " WHERE " + serverDbTableColumn + " LIKE '%" + textBoxQuery.Text + "%'";
                    textBox2.AppendText("\n");
                    textBox2.AppendText("\nconn:" + connection);
                    textBox2.AppendText("\nquery: " + query);
                    textBox2.AppendText("\n");

                    using (System.Data.SqlClient.SqlConnection dbCon = new System.Data.SqlClient.SqlConnection(connection))
                    {

                        dbCon.Open();
                        using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, dbCon))
                        {
                            using (System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            { da.Fill(dt); }
                        }
                    }
                }
                else if (ServerType == "SQLEXPRESS")
                {
                    connection = @"Data Source=" + ServerName + "\\SQLEXPRESS" + ";Initial Catalog=" + serverDB + UserWindowsAuthorization + ";User ID=" + UserLogin + ";Password=" + UserPassword + ";Connect Timeout=60";
                    query = "SELECT TOP 100 * FROM " + serverDbTable + " WHERE " + serverDbTableColumn + " LIKE '%" + textBoxQuery.Text + "%'";
                    textBox2.AppendText("\n");
                    textBox2.AppendText("\nconn:" + connection);
                    textBox2.AppendText("\nquery: " + query);
                    textBox2.AppendText("\n");

                    using (var dbCon = new System.Data.SqlClient.SqlConnection(connection))
                    {
                        dbCon.Open();

                        using (var cmd = new System.Data.SqlClient.SqlCommand(query, dbCon))
                        {
                            using (var da = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            { da.Fill(dt); }
                        }

                        dbCon.Close();
                    }
                }
                else if (ServerType == "MySQL")
                {
                    connection = @"server=" + ServerName + ";User=" + UserLogin + ";Password=" + UserPassword + ";database=" + serverDB + ";Connect Timeout=5";
                    query = "SELECT TOP 100 * FROM " + serverDbTable + " WHERE " + serverDbTableColumn + " LIKE '%" + textBoxQuery.Text + "%'";
                    textBox2.AppendText("\n");
                    textBox2.AppendText("\nconn:" + connection);
                    textBox2.AppendText("\nquery: " + query);
                    textBox2.AppendText("\n");

                    using (var dbCon = new MySql.Data.MySqlClient.MySqlConnection(connection))
                    {
                        dbCon.Open();

                        using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, dbCon))
                        {
                            using (var da = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd))
                            { da.Fill(dt); }
                        }

                        dbCon.Close();
                    }
                }

                dataGridView1.DataSource = dt;

                logger.Info("Выполнен запрос " + query + " для базы " + serverDB + " сервера " + ServerName);
                StatusLabel2.Text = "Данные с " + ServerName + " успешно получены";
            } catch (Exception Expt)
            {
                logger.Warn("Проверьте доступ к чтению данных таблицы: " + serverDbTable + ", " + ServerName + ", " + ServerType + ", Логин - " + UserLogin + ", Пароль - " + UserPassword);
                MessageBox.Show(Expt.Message, comboBoxServers.SelectedItem.ToString() + " не доступен или неправильная авторизация", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            timer2.Enabled = false; //Stop Timer
        }

        private void buttonGettFullInfo_Click(object sender, EventArgs e)
        {
            timer2.Enabled = true;

            serverDbTableColumn = comboBoxColumns.SelectedItem.ToString();
            DataTable dt = new DataTable();
            string txtbox = textBoxQuery.Text;
            string query = "";

            logger.Info("Запрашиваю данные таблицы " + serverDbTable + " базы " + serverDB + " сервера " + ServerName);
            StatusLabel2.Text = "Запрашиваю данные таблицы " + serverDbTable + " базы " + serverDB + " сервера " + ServerName;

            try
            {
                //Next string - work!!!!
                //string conn = DBAppIntellect.Properties.Settings.Default.intellectConnection; //Native Connection
                //string conn = "Data Source=KV-BASE\\SQLEXPRESS;Initial Catalog=intellect;Persist Security Info=True;User ID=xcvxcvx;Password=sdfsf;Connect Timeout=60";
                //string conn = "Data Source=" + comboBox3.SelectedItem.ToString() + "\\SQLEXPRESS;Persist Security Info=True;User ID=sdsdfsdf;Password=ghghjgh;Connect Timeout=60";

                if (ServerName.ToLower().Contains("po-sql-01") || ServerName.ToLower().Contains("tfactura") || ServerType == "MSSQL2005")
                {
                    connection = @"Data Source=" + ServerName + ";Initial Catalog=" + serverDB + ";Type System Version=SQL Server 2005" + UserWindowsAuthorization + ";User ID=" + UserLogin + ";Password=" + UserPassword + ";Connect Timeout=60";
                    query = "SELECT TOP 10000 * FROM " + serverDbTable + " WHERE " + serverDbTableColumn + " LIKE '%" + txtbox + "%'";
                    textBox2.AppendText("\n");
                    textBox2.AppendText("\nconn:" + connection);
                    textBox2.AppendText("\nquery: " + query);
                    textBox2.AppendText("\n");

                    using (System.Data.SqlClient.SqlConnection dbCon = new System.Data.SqlClient.SqlConnection(connection))
                    {

                        dbCon.Open();
                        using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, dbCon))
                        {
                            using (System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            { da.Fill(dt);
                            }
                        }
                    }
                }
                else if (ServerType == "SQLEXPRESS")
                {
                    connection = @"Data Source=" + ServerName + "\\SQLEXPRESS" + ";Initial Catalog=" + serverDB + UserWindowsAuthorization + ";User ID=" + UserLogin + ";Password=" + UserPassword + ";Connect Timeout=60";
                    query = "SELECT TOP 10000 * FROM " + serverDbTable + " WHERE " + serverDbTableColumn + " LIKE '%" + txtbox + "%'";
                    textBox2.AppendText("\n");
                    textBox2.AppendText("\nconn:" + connection);
                    textBox2.AppendText("\nquery: " + query);
                    textBox2.AppendText("\n");

                    using (var dbCon = new System.Data.SqlClient.SqlConnection(connection))
                    {
                        dbCon.Open();

                        using (var cmd = new System.Data.SqlClient.SqlCommand(query, dbCon))
                        {
                            using (var da = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            { da.Fill(dt);
                            }
                        }

                        dbCon.Close();
                    }
                }
                else if (ServerType == "MySQL")
                {
                    connection = @"server=" + ServerName + ";User=" + UserLogin + ";Password=" + UserPassword + ";database=" + serverDB + ";Connect Timeout=5";
                    query = "SELECT * FROM " + serverDbTable + " WHERE " + serverDbTableColumn + " LIKE '%" + txtbox + "%' LIMIT 10000";
                    textBox2.AppendText("\n");
                    textBox2.AppendText("\nconn:" + connection);
                    textBox2.AppendText("\nquery: " + query);
                    textBox2.AppendText("\n");

                    using (var dbCon = new MySql.Data.MySqlClient.MySqlConnection(connection))
                    {
                        dbCon.Open();

                        using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, dbCon))
                        {
                            using (var da = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd))
                            { da.Fill(dt);
                            }
                        }

                        dbCon.Close();
                    }
                }

                dataGridView1.DataSource = dt;

                logger.Info("Выполнен запрос " + query + " для базы " + serverDB + " сервера " + ServerName);
                StatusLabel2.Text = "Данные с " + ServerName + " успешно получены";
            } catch (Exception Expt)
            {
                logger.Warn("Проверьте доступ к чтению данных таблицы: " + serverDbTable + ", " + ServerName + ", " + ServerType + ", Логин - " + UserLogin + ", Пароль - " + UserPassword);
                MessageBox.Show(Expt.Message, comboBoxServers.SelectedItem.ToString() + " не доступен или неправильная авторизация", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            timer2.Enabled = false; //Stop Timer
        }

            private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            comboBoxColumns.Enabled = false;
            textBoxQuery.Enabled = false;
            GetRows.Enabled = false;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxQuery.Enabled = true;
            GetRows.Enabled = true;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            timer2.Enabled = false; //Stop Timer2
            GetTables.Enabled = true;
            comboBoxTables.Enabled = false;
            button1.Enabled = false;
            comboBoxColumns.Enabled = false;
            textBoxQuery.Enabled = false;
            GetRows.Enabled = false;
            timer1.Enabled = true; //Start Timer
            StatusLabel2.Text = "Выбран " + ServerName;
        }

        private void timer1_Tick(object sender, EventArgs e) //Change a Color of the Font on Status by the Timer
        {
            if (this.StatusLabel1.ForeColor == System.Drawing.Color.Black)
                this.StatusLabel1.ForeColor = System.Drawing.Color.Red;
            else this.StatusLabel1.ForeColor = System.Drawing.Color.Black;
        }
        private void timer2_Tick(object sender, EventArgs e) //Change a Color of the Font on Status by the Timer
        {
            if (this.StatusLabel2.ForeColor == System.Drawing.Color.Black)
                this.StatusLabel2.ForeColor = System.Drawing.Color.Red;
            else this.StatusLabel2.ForeColor = System.Drawing.Color.Black;
        }

        private void comboBox3_Click(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e) //Кнопка "О программе"
        {
            DialogResult result = MessageBox.Show(
                myFileVersionInfo.Comments + "\n\nВерсия: " + myFileVersionInfo.FileVersion + "\nBuild: " +
                strVersion + "\n" + myFileVersionInfo.LegalCopyright + "\n" + "\nЛокальный ПК:\n" +
                Environment.MachineName + "\n" +
                Environment.OSVersion + "\n" +
                Environment.WorkingSet,
                "Информация о программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
            if (result == DialogResult.OK)
                this.buttonAbout.BackColor = System.Drawing.Color.Khaki;
        }

        private void Form1_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Инструмент для подключения к БД\nи nросмотра содержимого этих баз данных ",
                "Информация о программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        
        private void buttonReset_Click(object sender, EventArgs e)
        {
            comboBoxServers.Enabled = true;
            comboBoxDBs.Enabled = true;
        }
    }
}
