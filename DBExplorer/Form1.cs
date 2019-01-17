using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace DBExplorer
{
    public partial class Form1 : Form
    {
        //http://www.cyberforum.ru/windows-forms/thread110436.html#post629892
        //http://www.realcoding.net/article/view/2611

        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private System.Diagnostics.FileVersionInfo myFileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath);

        private string strVersion;
        private ContextMenu contextMenu1;

        public string ServerName = "";
        public string ServerType = "MSSQL";

        private string UserLogin = "";
        private string UserPassword = "";
        private string UserWindowsAuthorization = ";" + "Persist Security Info=True";
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

            //start point of datagrid
            dataGridView1.DataSource = new System.Collections.ArrayList(); 

            myFileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
            strVersion = myFileVersionInfo.Comments + " ver." + myFileVersionInfo.FileVersion + " " + myFileVersionInfo.LegalCopyright;
            StatusLabel1.Text = myFileVersionInfo.ProductName + " ver." + myFileVersionInfo.FileVersion + " " + myFileVersionInfo.LegalCopyright;
            StatusLabel1.Alignment = ToolStripItemAlignment.Right;
            DBParameters.productName = myFileVersionInfo.ProductName;
            contextMenu1 = new ContextMenu();  //Context Menu on notify Icon
            notifyIcon1.ContextMenu = contextMenu1;
            contextMenu1.MenuItems.Add("About", AboutSoft);
            contextMenu1.MenuItems.Add("Exit", ApplicationExit);
            notifyIcon1.Text = myFileVersionInfo.ProductName + "\nv." + myFileVersionInfo.FileVersion + "\n" + myFileVersionInfo.CompanyName;
            this.Text = myFileVersionInfo.Comments;

            toolTip1.SetToolTip(textBoxQuery, "Введите параметр для фильтра выводимых данных");


            using (Microsoft.Win32.RegistryKey EvUserKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(DBParameters.myRegKey, Microsoft.Win32.RegistryKeyPermissionCheck.ReadSubTree, System.Security.AccessControl.RegistryRights.ReadKey))
            {
                try { DBParameters.ServerName = EvUserKey.GetValue("ServerName").ToString().Trim(); } catch { }
                try { DBParameters.ServerType = EvUserKey.GetValue("ServerType").ToString().Trim(); } catch { }
                try { DBParameters.UserLogin = EvUserKey.GetValue("UserLogin").ToString().Trim(); } catch { }
            }

            using (Microsoft.Win32.RegistryKey EvUserKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(DBParameters.myRegKey))
            {
                try { EvUserKey.SetValue("productName", DBParameters.productName, Microsoft.Win32.RegistryValueKind.String); } catch { }
            }

            comboBoxTables.Enabled = false;
            buttonColumns.Enabled = false;
            comboBoxColumns.Enabled = false;
            textBoxQuery.Enabled = false;
            GetRows.Enabled = false;

            buttonSort.Text = "Nothing";
            toolTip1.SetToolTip(buttonSort, "Сортировка по упомлчанию");
            buttonSort.BackColor = System.Drawing.SystemColors.Control;

            //The Start of The Block. for transfer any data between Form1 and Form2
            this.Hide();
            DBParameters.Inputed = false;

            Form2 f = new Form2();
            f.ShowDialog();
            this.Show();

            if (DBParameters.Inputed)
            {
                if (DBParameters.CheckBoxAuthorize)
                {
                    UserWindowsAuthorization = ";Integrated Sercurity = true";
                    UserLogin = null;
                    UserPassword = null;
                }
                else
                {
                    UserWindowsAuthorization = ";Persist Security Info=True";
                    UserLogin = DBParameters.UserLogin;
                    UserPassword = DBParameters.UserPassword;
                }

                if (DBParameters.ServerName.Length > 0)
                {
                    ServerName = DBParameters.ServerName;
                    ServerType = DBParameters.ServerType;
                    comboBoxServers.Items.Add(ServerName);
                    comboBoxServers.SelectedIndex = comboBoxServers.FindString(ServerName);

                    GetTables.Enabled = true;
                    comboBoxTables.Enabled = false;
                    buttonColumns.Enabled = false;
                    comboBoxColumns.Enabled = false;
                    textBoxQuery.Enabled = false;
                    GetRows.Enabled = false;
                }
                logger.Info("Выбран " + ServerName);
                StatusLabel2.Text = "Выбран " + ServerName;

                using (Microsoft.Win32.RegistryKey EvUserKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(DBParameters.myRegKey))
                {
                    try { EvUserKey.SetValue("ServerName", DBParameters.ServerName, Microsoft.Win32.RegistryValueKind.String); } catch { }
                    try { EvUserKey.SetValue("ServerType", DBParameters.ServerType, Microsoft.Win32.RegistryValueKind.String); } catch { }
                    try { EvUserKey.SetValue("UserLogin", DBParameters.UserLogin, Microsoft.Win32.RegistryValueKind.String); } catch { }
                }
            }
            else
            {
                logger.Info("Сервер не выбран");
                StatusLabel2.Text = "Выберите сервер для получения данных";
            }
            //The End of The Block. for transfer any data between Form1 and Form2
        }


        private void SetStartStatus()
        {
            comboBoxServers.SelectedIndex = 0;
            comboBoxDBs.SelectedIndex = 0;
        }

        private void GetDBNames_SelectedIndexChanged(object sender, EventArgs e)// ComboBoxServers_SelectedIndexChanged()
        {
            comboBoxDBs.Items.Clear();
            GetDBNames();
        }

        private void GetDBNames()
        {
            HashSet<string> list = new HashSet<string>();
            string[] str = new string[1];
            DataTable dt = new DataTable();

            if (ServerName.Length == 0)
            { ServerName = comboBoxServers.SelectedItem.ToString(); }
            StatusLabel2.Text = "Подключаюсь к серверу " + ServerName;
            logger.Info("Подключаюсь к серверу " + ServerName);

            try
            {
                if (ServerName.ToLower().Contains("po-sql-01") || ServerName.ToLower().Contains("tfactura") || ServerType == "MSSQL")
                {
                    connection = @"Data Source=" + ServerName;
                    if (ServerType == "SQLEXPRESS")
                    { connection += "\\SQLEXPRESS"; }
                    else { connection += ";Initial Catalog=EBP"; }
                    connection += UserWindowsAuthorization + ";User ID=" + UserLogin + ";Password=" + UserPassword + ";Connect Timeout=10";

                    list = GetSchemaMSSQLDB(connection, "Databases", "database_name");
                }
                else if (ServerType == "MySQL")
                {
                    connection = @"server=" + ServerName + ";User=" + UserLogin + ";Password=" + UserPassword + ";database=mysql;Connect Timeout=5";
                    string query = "SELECT * FROM db where user='" + UserLogin + "' and select_priv='Y'";
                    list = GetSchemaMySqlDB(connection, query, 1);
                }

                comboBoxDBs.Items.AddRange(list.ToArray());

                if (ServerName.ToLower().Contains("po-sql-01") || ServerName.ToLower().Contains("tfactura"))
                { comboBoxDBs.SelectedIndex = comboBoxDBs.FindString("EBP"); }
                else { comboBoxDBs.SelectedIndex = 0; }


                logger.Info("Список баз сервера " + ServerName + " успешно получено");
                StatusLabel2.Text = "Список баз сервера " + ServerName + " успешно получено";

                GetTables.Enabled = false;
                comboBoxTables.Enabled = false;
                buttonColumns.Enabled = false;
                comboBoxColumns.Enabled = false;
                textBoxQuery.Enabled = false;
                GetRows.Enabled = false;
            }
            catch (Exception expt)
            {
                logger.Warn("Проверьте правильность - " + ServerName + ", " + ServerType + ", Логин - " + UserLogin + ", Пароль - " + UserPassword);
                StatusLabel2.Text = "Ошибка доступа к" + ServerName + " " + ServerType + "|Логин - " + UserLogin + "|Пароль - " + UserPassword;
                MessageBox.Show("Проверьте имя сервера/пользователя/пароль:\nСервер и его тип - " +
                  ServerName + " " + ServerType + "\nЛогин - " + UserLogin + "\nПароль - " + UserPassword +
                  "\n-----------------------" + "\n" + expt.ToString());
            }
        }

        private HashSet<string> GetSchemaColumnsMSSQLDB(string connectionString, string nameTable)
        {
            HashSet<string> list = new HashSet<string>();
            DataTable dt = new DataTable();
            logger.Info("nameTable: " + nameTable + " connectionString: " + connectionString);

            using (System.Data.SqlClient.SqlConnection dbCon = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                dbCon.Open();
                dt = dbCon.GetSchema("Columns", new[] { null, null, nameTable });
                foreach (DataRow r in dt.Rows)
                { list.Add(r["COLUMN_NAME"].ToString()); }
                dbCon.Close();
            }
            dt = null;
            return list;
        }

        private HashSet<string> GetSchemaMSSQLDB(string connectionString, string nameSchema, string nameColumn)
        {
            HashSet<string> list = new HashSet<string>();
            DataTable dt = new DataTable();
            logger.Info("nameSchema: " + nameSchema + " nameColumn: " + nameColumn + " connectionString: " + connectionString);

            using (System.Data.SqlClient.SqlConnection dbCon = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                dbCon.Open();
                dt = dbCon.GetSchema(nameSchema);
                foreach (DataRow r in dt.Rows)
                { list.Add(r[nameColumn].ToString()); }
                dbCon.Close();
            }
            dt = null;
            return list;
        }

        private HashSet<string> GetSchemaMySqlDB(string connectionString, string query, int indexColumn)
        {
            HashSet<string> list = new HashSet<string>();
            DataTable dt = new DataTable();
            logger.Info("query: " + query + " indexColumn: " + indexColumn + " connectionString: " + connectionString);

            using (MySql.Data.MySqlClient.MySqlConnection dbCon = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                dbCon.Open();
                using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, dbCon))
                {
                    using (MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetString(indexColumn) != null && reader.GetString(indexColumn).Length > 0)
                                list.Add(reader.GetString(indexColumn));
                        }
                    }
                }
                dbCon.Close();
            }

            dt = null;
            return list;
        }

        private void GetTables_Click(object sender, EventArgs e)
        {
            comboBoxTables.Items.Clear();
            GetTablesDb();
        }

        private void GetTablesDb()
        {
            HashSet<string> list = new HashSet<string>();
            DataTable dt = new DataTable();
            serverDB = comboBoxDBs.SelectedItem.ToString();
            logger.Info("Запрашиваю список таблиц с базы " + serverDB + " сервера " + ServerName);

            comboBoxTables.Enabled = true;
            buttonColumns.Enabled = false;
            comboBoxColumns.Enabled = false;
            textBoxQuery.Enabled = false;
            GetRows.Enabled = false;
            comboBoxServers.Enabled = false;
            //comboBoxDBs.Enabled = false;

            //string conn = DBAppIntellect.Properties.Settings.Default.intellectConnection;  // Native Connection
            //https://msdn.microsoft.com/ru-ru/library/system.data.sqlclient.sqlconnection.connectionstring(v=vs.110).aspx
            try
            {
                if (ServerName.ToLower().Contains("po-sql-01") || ServerName.ToLower().Contains("tfactura") || ServerType == "MSSQL" || ServerType == "SQLEXPRESS")
                {
                    connection = @"Data Source=" + ServerName;
                    if (ServerType == "SQLEXPRESS")
                    { connection += "\\SQLEXPRESS"; }
                    connection += ";Initial Catalog=" + serverDB + UserWindowsAuthorization + ";User ID=" + UserLogin + ";Password=" + UserPassword + ";Connect Timeout=20";

                    list = GetSchemaMSSQLDB(connection, "Tables", "TABLE_NAME");
                }
                else if (ServerType == "MySQL")
                {
                    connection = @"server=" + ServerName + ";User=" + UserLogin + ";Password=" + UserPassword + ";database=" + serverDB + ";Connect Timeout=5";
                    string query = "SHOW TABLES";

                    list = GetSchemaMySqlDB(connection, query, 0);
                }

                comboBoxTables.Items.AddRange(list.ToArray());

                logger.Info("Список таблиц c базы " + serverDB + " сервера " + ServerName + " успешно получены");
                StatusLabel2.Text = "Список таблиц c базы " + serverDB + " сервера " + ServerName + " успешно получены";
            }
            catch (Exception expt)
            {
                logger.Warn("Проверьте доступ к чтению списка таблиц: " + ServerName + ", " + ServerType + ", Логин - " + UserLogin + ", Пароль - " + UserPassword);

                MessageBox.Show("Проверьте доступ к чтению списка таблиц:\nСервер - " +
                  ServerName + " " + ServerType + "\nЛогин - " + UserLogin + "\nПароль - " + UserPassword +
                  "\n-----------------------" + "\n" + expt.ToString());
            }

            comboBoxTables.SelectedIndex = 0;
        }

        private void GetColumns_Click(object sender, EventArgs e)
        {
            comboBoxColumns.Items.Clear();
            GetColumnsDb();
        }

        private void GetColumnsDb()
        {
            comboBoxColumns.Enabled = true;
            textBoxQuery.Enabled = false;
            GetRows.Enabled = false;

            serverDbTable = comboBoxTables.SelectedItem.ToString();
            HashSet<string> list = new HashSet<string>();
            DataTable dt = new DataTable();

            logger.Info("Запрашиваю список столбцов таблицы " + serverDbTable + " базы " + serverDB + " сервера " + ServerName);
            StatusLabel2.Text = "Запрашиваю список столбцов таблицы " + serverDbTable + " базы " + serverDB + " сервера " + ServerName;

            try
            {
                if (ServerName.ToLower().Contains("po-sql-01") || ServerName.ToLower().Contains("tfactura") || ServerType == "MSSQL" || ServerType == "SQLEXPRESS")
                {
                    connection = @"Data Source=" + ServerName;
                    if (ServerType == "SQLEXPRESS")
                    { connection += "\\SQLEXPRESS"; }
                    connection += ";Initial Catalog=" + serverDB + UserWindowsAuthorization + ";User ID=" + UserLogin + ";Password=" + UserPassword + ";Connect Timeout=20";

                    list = GetSchemaColumnsMSSQLDB(connection, serverDbTable);
                }
                else if (ServerType == "MySQL")
                {
                    connection = @"server=" + ServerName + ";User=" + UserLogin + ";Password=" + UserPassword + ";database=" + serverDB + ";Connect Timeout=5";
                    string query = "SHOW COLUMNS FROM " + serverDbTable;

                    list = GetSchemaMySqlDB(connection, query, 0);
                }

                comboBoxColumns.Items.AddRange(list.ToArray());
                comboBoxColumns.SelectedIndex = 0;

                logger.Info("Список столбцов таблицы " + serverDbTable + " для базы " + serverDB + " сервера " + ServerName + " получен");
                StatusLabel2.Text = "Список столбцов таблицы " + serverDbTable + " для базы " + serverDB + " сервера " + ServerName + " получен";

                //  comboBoxColumns.SelectedIndex = 0;
            }
            catch (Exception expt)
            {
                logger.Warn("Проверьте доступ к чтению списка столбцов: " + ServerName + ", " + ServerType + ", Логин - " + UserLogin + ", Пароль - " + UserPassword);

                MessageBox.Show("Проверьте доступ к чтению списка столбцов:\nСервер - " +
                  ServerName + " " + ServerType + "\nЛогин - " + UserLogin + "\nПароль - " + UserPassword +
                  "\n-----------------------" + "\n" + expt.ToString());
            }
        }

        private void GetInfo_Click(object sender, EventArgs e)
        { GetInfoDb(); }

        private void GetInfoDb()
        {
            serverDbTableColumn = comboBoxColumns.SelectedItem.ToString();
            DataTable dt = new DataTable();
            string txtbox = textBoxQuery.Text.Trim();
            string query = "";

            logger.Info("Запрашиваю данные таблицы " + serverDbTable + " базы " + serverDB + " сервера " + ServerName);
            StatusLabel2.Text = "Запрашиваю данные таблицы " + serverDbTable + " базы " + serverDB + " сервера " + ServerName;

            try
            {
                if (ServerName.ToLower().Contains("po-sql-01") || ServerName.ToLower().Contains("tfactura") || ServerType == "MSSQL" || ServerType == "SQLEXPRESS")
                {
                    //connection
                    connection = @"Data Source=" + ServerName;
                    if (ServerType == "SQLEXPRESS")
                    { connection += "\\SQLEXPRESS"; }
                    connection += ";Initial Catalog=" + serverDB + UserWindowsAuthorization + "; User ID=" + UserLogin + ";Password=" + UserPassword + "; Connect Timeout=60"; //;Type System Version=SQL Server 2005 

                    //query
                    query = "SELECT TOP 100 * FROM " + serverDbTable;

                    if (txtbox.Length > 0)
                    { query += " WHERE " + serverDbTableColumn + " LIKE '%" + txtbox + "%'"; }

                    if (buttonSort.Text == "ASC")
                    { query += " ORDER BY " + serverDbTableColumn + " ASC"; }
                    else if (buttonSort.Text == "DESC")
                    { query += " ORDER BY " + serverDbTableColumn + " DESC"; }

                    //logger
                    logger.Info("connection " + connection);
                    logger.Info("query " + query);

                    textBox2.AppendText("\n");
                    textBox2.AppendText("\nconn:" + connection);
                    textBox2.AppendText("\nquery: " + query);
                    textBox2.AppendText("\n");

                    //get data
                    using (System.Data.SqlClient.SqlConnection dbCon = new System.Data.SqlClient.SqlConnection(connection))
                    {
                        dbCon.Open();
                        using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, dbCon))
                        {
                            using (System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            { da.Fill(dt); }
                        }
                        dbCon.Close();
                    }
                }
                else if (ServerType == "MySQL")
                {
                    //connection
                    connection = @"server=" + ServerName + ";User=" + UserLogin + ";Password=" + UserPassword + ";database=" + serverDB + ";pooling = false; convert zero datetime=True; Connect Timeout=30";

                    //query
                    query = "SELECT * FROM " + serverDbTable;

                    if (txtbox.Length > 0)
                    { query += " WHERE " + serverDbTableColumn + " LIKE '%" + txtbox + "%'"; }

                    if (buttonSort.Text == "ASC")
                    { query += " ORDER BY " + serverDbTableColumn + " ASC"; }
                    else if (buttonSort.Text == "DESC")
                    { query += " ORDER BY " + serverDbTableColumn + " DESC"; }

                    query += " LIMIT 100";

                    //logger
                    logger.Info("connection " + connection);
                    logger.Info("query " + query);

                    textBox2.AppendText("\n");
                    textBox2.AppendText("\nconn:" + connection);
                    textBox2.AppendText("\nquery: " + query);
                    textBox2.AppendText("\n");

                    //get data
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
            }
            catch (Exception Expt)
            {
                logger.Warn("Проверьте доступ к чтению данных таблицы: " + serverDbTable + ", " + ServerName + ", " + ServerType + ", Логин - " + UserLogin + ", Пароль - " + UserPassword);
                MessageBox.Show(Expt.Message, comboBoxServers.SelectedItem.ToString() + " не доступен или неправильная авторизация", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonGettFullInfo_Click(object sender, EventArgs e)
        { GetFullInfoDb(); }

        private void GetFullInfoDb()
        {
            serverDbTableColumn = comboBoxColumns.SelectedItem.ToString();
            DataTable dt = new DataTable();
            string txtbox = textBoxQuery.Text.Trim();
            string query = "";

            logger.Info("Запрашиваю данные таблицы " + serverDbTable + " базы " + serverDB + " сервера " + ServerName);
            StatusLabel2.Text = "Запрашиваю данные таблицы " + serverDbTable + " базы " + serverDB + " сервера " + ServerName;

            try
            {
                if (ServerName.ToLower().Contains("po-sql-01") || ServerName.ToLower().Contains("tfactura") || ServerType == "MSSQL" || ServerType == "SQLEXPRESS")
                {
                    //connection
                    connection = @"Data Source=" + ServerName;
                    if (ServerType == "SQLEXPRESS")
                    { connection += "\\SQLEXPRESS"; }
                    connection += ";Initial Catalog=" + serverDB + UserWindowsAuthorization + "; User ID=" + UserLogin + ";Password=" + UserPassword + "; Connect Timeout=30";  //;Type System Version=SQL Server 2005                    

                    //query
                    query = "SELECT TOP 5000 * FROM " + serverDbTable;

                    if (txtbox.Length > 0)
                    { query += " WHERE " + serverDbTableColumn + " LIKE '%" + txtbox + "%'"; }

                    if (buttonSort.Text == "ASC")
                    { query += " ORDER BY " + serverDbTableColumn + " ASC"; }
                    else if (buttonSort.Text == "DESC")
                    { query += " ORDER BY " + serverDbTableColumn + " DESC"; }

                    //logger
                    logger.Info("connection " + connection);
                    logger.Info("query " + query);

                    textBox2.AppendText("\n");
                    textBox2.AppendText("\nconn:" + connection);
                    textBox2.AppendText("\nquery: " + query);
                    textBox2.AppendText("\n");

                    //get data
                    using (System.Data.SqlClient.SqlConnection dbCon = new System.Data.SqlClient.SqlConnection(connection))
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
                    //connection
                    connection = @"server=" + ServerName + ";User=" + UserLogin + ";Password=" + UserPassword + ";database=" + serverDB + ";pooling = false; convert zero datetime=True; Connect Timeout=30";

                    //query
                    query = "SELECT * FROM " + serverDbTable;

                    if (txtbox.Length > 0)
                    { query += " WHERE " + serverDbTableColumn + " LIKE '%" + txtbox + "%'"; }

                    if (buttonSort.Text == "ASC")
                    { query += " ORDER BY " + serverDbTableColumn + " ASC"; }
                    else if (buttonSort.Text == "DESC")
                    { query += " ORDER BY " + serverDbTableColumn + " DESC"; }

                    query += " LIMIT 5000";

                    //logger
                    logger.Info("connection " + connection);
                    logger.Info("query " + query);

                    textBox2.AppendText("\n");
                    textBox2.AppendText("\nconn:" + connection);
                    textBox2.AppendText("\nquery: " + query);
                    textBox2.AppendText("\n");

                    //get data
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
            }
            catch (Exception Expt)
            {
                logger.Warn("Проверьте доступ к чтению данных таблицы: " + serverDbTable + ", " + ServerName + ", " + ServerType + ", Логин - " + UserLogin + ", Пароль - " + UserPassword);
                MessageBox.Show(Expt.Message, comboBoxServers.SelectedItem.ToString() + " не доступен или неправильная авторизация", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonColumns.Enabled = true;
            comboBoxColumns.Enabled = false;
            textBoxQuery.Enabled = false;
            GetRows.Enabled = false;

            comboBoxColumns.Items.Clear();
            GetColumnsDb();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxQuery.Enabled = true;
            GetRows.Enabled = true;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTables.Enabled = true;
            comboBoxTables.Enabled = false;
            buttonColumns.Enabled = false;
            comboBoxColumns.Enabled = false;
            textBoxQuery.Enabled = false;
            GetRows.Enabled = false;
            StatusLabel2.Text = "Выбран " + ServerName;

            comboBoxTables.Items.Clear();
            GetTablesDb();
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

        private void AboutSoft(object sender, EventArgs e) //Кнопка "О программе"
        { AboutSoft(); }

        private void AboutSoft()
        {
            AboutBox1 aboutBox = new AboutBox1();
            aboutBox.Show();
        }

        private void ApplicationExit(object sender, EventArgs e)
        { ApplicationExit(); }

        private void ApplicationExit()
        { Application.Exit(); }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        { Application.Exit(); }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            comboBoxServers.Enabled = true;
            comboBoxDBs.Enabled = true;
        }

        bool sortDirectionASC = true;
        private void buttonSort_Click(object sender, EventArgs e)
        {
            if (sortDirectionASC && buttonSort.Text == "ASC")
            {
                buttonSort.Text = "Nothing";
                toolTip1.SetToolTip(buttonSort, "Сортировка по упомлчанию");

                buttonSort.BackColor = System.Drawing.SystemColors.Control;
            }
            else if (sortDirectionASC)
            {
                buttonSort.Text = "DESC";
                toolTip1.SetToolTip(buttonSort, "Сортировка по убыванию");

                sortDirectionASC = false;
                buttonSort.BackColor = System.Drawing.Color.DarkOrange;
            }
            else
            {
                buttonSort.Text = "ASC";
                toolTip1.SetToolTip(buttonSort, "Сортировка по возрастанию");

                sortDirectionASC = true;
                buttonSort.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
    }

    public static class DBParameters
    {
        public static string productName = "";
        public static string ServerName = "";
        public static string ServerType = "";
        public static string UserLogin = "";
        public static string UserPassword = "";
        public static bool CheckBoxAuthorize = false;
        public static bool Inputed = false;
        public static readonly string myRegKey = @"SOFTWARE\RYIK\DBExplorer";
    }
}
