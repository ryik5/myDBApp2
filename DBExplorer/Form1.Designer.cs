namespace DBExplorer
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonExit = new System.Windows.Forms.Button();
            this.textBoxQuery = new System.Windows.Forms.TextBox();
            this.GetRows = new System.Windows.Forms.Button();
            this.GetTables = new System.Windows.Forms.Button();
            this.comboBoxTables = new System.Windows.Forms.ComboBox();
            this.comboBoxColumns = new System.Windows.Forms.ComboBox();
            this.buttonColumns = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonAbout = new System.Windows.Forms.Button();
            this.buttonAuthorize = new System.Windows.Forms.Button();
            this.buttonGetAllRows = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonSort = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.SplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.StatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.comboBoxServers = new System.Windows.Forms.ComboBox();
            this.comboBoxDBs = new System.Windows.Forms.ComboBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.labelDebugger = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(2, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(832, 551);
            this.dataGridView1.TabIndex = 0;
            // 
            // buttonExit
            // 
            this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SandyBrown;
            this.buttonExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExit.Location = new System.Drawing.Point(1023, 12);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(60, 23);
            this.buttonExit.TabIndex = 1;
            this.buttonExit.Text = "Exit";
            this.toolTip1.SetToolTip(this.buttonExit, "Завершить работу с программой");
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.ApplicationExit);
            // 
            // textBoxQuery
            // 
            this.textBoxQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxQuery.Location = new System.Drawing.Point(924, 162);
            this.textBoxQuery.Name = "textBoxQuery";
            this.textBoxQuery.Size = new System.Drawing.Size(159, 20);
            this.textBoxQuery.TabIndex = 3;
            // 
            // GetRows
            // 
            this.GetRows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GetRows.FlatAppearance.MouseOverBackColor = System.Drawing.Color.PaleGreen;
            this.GetRows.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GetRows.Location = new System.Drawing.Point(843, 159);
            this.GetRows.Name = "GetRows";
            this.GetRows.Size = new System.Drawing.Size(75, 23);
            this.GetRows.TabIndex = 4;
            this.GetRows.Text = "Get Rows";
            this.toolTip1.SetToolTip(this.GetRows, "Получить отфильтрованный список таблиз выбранной базы выбранной таблицы");
            this.GetRows.UseVisualStyleBackColor = true;
            this.GetRows.Click += new System.EventHandler(this.GetInfo_Click);
            // 
            // GetTables
            // 
            this.GetTables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GetTables.FlatAppearance.MouseOverBackColor = System.Drawing.Color.PaleGreen;
            this.GetTables.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GetTables.Location = new System.Drawing.Point(843, 92);
            this.GetTables.Name = "GetTables";
            this.GetTables.Size = new System.Drawing.Size(65, 23);
            this.GetTables.TabIndex = 6;
            this.GetTables.Text = "GetTables";
            this.toolTip1.SetToolTip(this.GetTables, "Получить список таблиз выбранной базы");
            this.GetTables.UseVisualStyleBackColor = true;
            this.GetTables.Click += new System.EventHandler(this.GetTables_Click);
            // 
            // comboBoxTables
            // 
            this.comboBoxTables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTables.FormattingEnabled = true;
            this.comboBoxTables.Location = new System.Drawing.Point(917, 93);
            this.comboBoxTables.Name = "comboBoxTables";
            this.comboBoxTables.Size = new System.Drawing.Size(166, 21);
            this.comboBoxTables.Sorted = true;
            this.comboBoxTables.TabIndex = 7;
            this.comboBoxTables.Text = "Таблица:";
            this.comboBoxTables.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // comboBoxColumns
            // 
            this.comboBoxColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxColumns.FormattingEnabled = true;
            this.comboBoxColumns.Location = new System.Drawing.Point(939, 126);
            this.comboBoxColumns.Name = "comboBoxColumns";
            this.comboBoxColumns.Size = new System.Drawing.Size(144, 21);
            this.comboBoxColumns.Sorted = true;
            this.comboBoxColumns.TabIndex = 9;
            this.comboBoxColumns.Text = "Колонки:";
            this.comboBoxColumns.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // buttonColumns
            // 
            this.buttonColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonColumns.FlatAppearance.MouseOverBackColor = System.Drawing.Color.PaleGreen;
            this.buttonColumns.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonColumns.Location = new System.Drawing.Point(843, 126);
            this.buttonColumns.Name = "buttonColumns";
            this.buttonColumns.Size = new System.Drawing.Size(84, 23);
            this.buttonColumns.TabIndex = 8;
            this.buttonColumns.Text = "Get Columns";
            this.toolTip1.SetToolTip(this.buttonColumns, "Получить список колонок выбранной базы");
            this.buttonColumns.UseVisualStyleBackColor = true;
            this.buttonColumns.Click += new System.EventHandler(this.GetColumns_Click);
            // 
            // buttonAbout
            // 
            this.buttonAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.PaleGreen;
            this.buttonAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAbout.Location = new System.Drawing.Point(939, 12);
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size(60, 23);
            this.buttonAbout.TabIndex = 14;
            this.buttonAbout.Text = "About";
            this.toolTip1.SetToolTip(this.buttonAbout, "О программе");
            this.buttonAbout.UseVisualStyleBackColor = true;
            this.buttonAbout.Click += new System.EventHandler(this.AboutSoft);
            // 
            // buttonAuthorize
            // 
            this.buttonAuthorize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAuthorize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.PaleGreen;
            this.buttonAuthorize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAuthorize.Location = new System.Drawing.Point(843, 12);
            this.buttonAuthorize.Name = "buttonAuthorize";
            this.buttonAuthorize.Size = new System.Drawing.Size(75, 23);
            this.buttonAuthorize.TabIndex = 15;
            this.buttonAuthorize.Text = "Authorize";
            this.toolTip1.SetToolTip(this.buttonAuthorize, "Авторизоваться с другими учетными данными");
            this.buttonAuthorize.UseVisualStyleBackColor = true;
            this.buttonAuthorize.Click += new System.EventHandler(this.Form1_Load);
            // 
            // buttonGetAllRows
            // 
            this.buttonGetAllRows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGetAllRows.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SandyBrown;
            this.buttonGetAllRows.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGetAllRows.Location = new System.Drawing.Point(843, 225);
            this.buttonGetAllRows.Name = "buttonGetAllRows";
            this.buttonGetAllRows.Size = new System.Drawing.Size(106, 23);
            this.buttonGetAllRows.TabIndex = 18;
            this.buttonGetAllRows.Text = "Get whole raws";
            this.toolTip1.SetToolTip(this.buttonGetAllRows, "Получить отфильтрованные данные выбранной базы выбранной таблицы");
            this.buttonGetAllRows.UseVisualStyleBackColor = true;
            this.buttonGetAllRows.Click += new System.EventHandler(this.buttonGettFullInfo_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SandyBrown;
            this.buttonReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReset.Location = new System.Drawing.Point(975, 225);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(108, 23);
            this.buttonReset.TabIndex = 19;
            this.buttonReset.Text = "Reset connection";
            this.toolTip1.SetToolTip(this.buttonReset, "Сбросить подключение к серверу");
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonSort
            // 
            this.buttonSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSort.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.buttonSort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSort.Location = new System.Drawing.Point(843, 192);
            this.buttonSort.Name = "buttonSort";
            this.buttonSort.Size = new System.Drawing.Size(240, 23);
            this.buttonSort.TabIndex = 20;
            this.buttonSort.Text = "Data sorting direction in the table";
            this.toolTip1.SetToolTip(this.buttonSort, "Изменить направление сортировки по выбранному столбцу");
            this.buttonSort.UseVisualStyleBackColor = true;
            this.buttonSort.Click += new System.EventHandler(this.buttonSort_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "ПО \r\nдля просмотра баз данных";
            this.notifyIcon1.BalloonTipTitle = "©Yuri Ryabchenko 2016 - 2017";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "ПО для просмотра баз данных DBApp";
            this.notifyIcon1.Visible = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel1,
            this.toolStripProgressBar1,
            this.SplitButton1,
            this.StatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 552);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1088, 26);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel1
            // 
            this.StatusLabel1.Name = "StatusLabel1";
            this.StatusLabel1.Size = new System.Drawing.Size(73, 21);
            this.StatusLabel1.Text = "StatusLabel1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(75, 20);
            // 
            // SplitButton1
            // 
            this.SplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SplitButton1.Image = global::DBExplorer.Properties.Resources.ryik;
            this.SplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SplitButton1.Name = "SplitButton1";
            this.SplitButton1.Size = new System.Drawing.Size(36, 24);
            this.SplitButton1.Text = "SplitButton1";
            // 
            // StatusLabel2
            // 
            this.StatusLabel2.Name = "StatusLabel2";
            this.StatusLabel2.Size = new System.Drawing.Size(73, 21);
            this.StatusLabel2.Text = "StatusLabel2";
            this.StatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxServers
            // 
            this.comboBoxServers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxServers.FormattingEnabled = true;
            this.comboBoxServers.Location = new System.Drawing.Point(843, 60);
            this.comboBoxServers.Name = "comboBoxServers";
            this.comboBoxServers.Size = new System.Drawing.Size(126, 21);
            this.comboBoxServers.TabIndex = 12;
            this.comboBoxServers.Text = "Сервер:";
            this.comboBoxServers.SelectedIndexChanged += new System.EventHandler(this.GetDBNames_SelectedIndexChanged);
            // 
            // comboBoxDBs
            // 
            this.comboBoxDBs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDBs.FormattingEnabled = true;
            this.comboBoxDBs.Location = new System.Drawing.Point(975, 60);
            this.comboBoxDBs.Name = "comboBoxDBs";
            this.comboBoxDBs.Size = new System.Drawing.Size(108, 21);
            this.comboBoxDBs.TabIndex = 13;
            this.comboBoxDBs.Text = "База:";
            this.comboBoxDBs.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(843, 282);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(240, 271);
            this.textBox2.TabIndex = 16;
            // 
            // labelDebugger
            // 
            this.labelDebugger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDebugger.AutoSize = true;
            this.labelDebugger.Location = new System.Drawing.Point(840, 259);
            this.labelDebugger.Name = "labelDebugger";
            this.labelDebugger.Size = new System.Drawing.Size(57, 13);
            this.labelDebugger.TabIndex = 17;
            this.labelDebugger.Text = "Debugger:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1088, 578);
            this.Controls.Add(this.buttonSort);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonGetAllRows);
            this.Controls.Add(this.labelDebugger);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.buttonAuthorize);
            this.Controls.Add(this.buttonAbout);
            this.Controls.Add(this.comboBoxDBs);
            this.Controls.Add(this.comboBoxServers);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.comboBoxColumns);
            this.Controls.Add(this.buttonColumns);
            this.Controls.Add(this.comboBoxTables);
            this.Controls.Add(this.GetTables);
            this.Controls.Add(this.GetRows);
            this.Controls.Add(this.textBoxQuery);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "DBApp";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.TextBox textBoxQuery;
        private System.Windows.Forms.Button GetRows;
        private System.Windows.Forms.Button GetTables;
        private System.Windows.Forms.ComboBox comboBoxTables;
        private System.Windows.Forms.ComboBox comboBoxColumns;
        private System.Windows.Forms.Button buttonColumns;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel1;
        private System.Windows.Forms.ComboBox comboBoxServers;
        private System.Windows.Forms.ComboBox comboBoxDBs;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel2;
        private System.Windows.Forms.Button buttonAbout;
        private System.Windows.Forms.Button buttonAuthorize;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label labelDebugger;
        private System.Windows.Forms.ToolStripSplitButton SplitButton1;
        private System.Windows.Forms.Button buttonGetAllRows;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonSort;
    }
}

