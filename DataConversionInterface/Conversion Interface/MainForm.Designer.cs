namespace MainWindow
{
    partial class fileConversionInterface
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridViewGeneral = new System.Windows.Forms.DataGridView();
            this.conversionTablesList = new System.Windows.Forms.ComboBox();
            this.tablesListLabel = new System.Windows.Forms.Label();
            this.dataGridViewTables = new System.Windows.Forms.DataGridView();
            this.tablesSelectLabel = new System.Windows.Forms.Label();
            this.generalDataViewLabel = new System.Windows.Forms.Label();
            this.buttonOpenDataFile = new System.Windows.Forms.Button();
            this.buttonLoadDataFile = new System.Windows.Forms.Button();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.labelFilePath = new System.Windows.Forms.Label();
            this.panelConversionTable = new System.Windows.Forms.Panel();
            this.buttonSaveTable = new System.Windows.Forms.Button();
            this.progressBarConversion = new System.Windows.Forms.ProgressBar();
            this.labelConversionStatus = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxStatusMessages = new System.Windows.Forms.TextBox();
            this.buttonStartConversion = new System.Windows.Forms.Button();
            this.timerConvertProgress = new System.Windows.Forms.Timer(this.components);
            this.buttonViewReport = new System.Windows.Forms.Button();
            this.buttonDeclineReport = new System.Windows.Forms.Button();
            this.buttonAcceptReport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGeneral)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTables)).BeginInit();
            this.panelConversionTable.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewGeneral
            // 
            this.dataGridViewGeneral.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGeneral.Location = new System.Drawing.Point(12, 32);
            this.dataGridViewGeneral.Name = "dataGridViewGeneral";
            this.dataGridViewGeneral.Size = new System.Drawing.Size(1107, 205);
            this.dataGridViewGeneral.TabIndex = 0;
            // 
            // conversionTablesList
            // 
            this.conversionTablesList.FormattingEnabled = true;
            this.conversionTablesList.Location = new System.Drawing.Point(30, 329);
            this.conversionTablesList.Name = "conversionTablesList";
            this.conversionTablesList.Size = new System.Drawing.Size(159, 21);
            this.conversionTablesList.TabIndex = 2;
            this.conversionTablesList.SelectedIndexChanged += new System.EventHandler(this.conversionTablesList_SelectedIndexChanged);
            // 
            // tablesListLabel
            // 
            this.tablesListLabel.AutoSize = true;
            this.tablesListLabel.Location = new System.Drawing.Point(120, 10);
            this.tablesListLabel.Name = "tablesListLabel";
            this.tablesListLabel.Size = new System.Drawing.Size(95, 13);
            this.tablesListLabel.TabIndex = 3;
            this.tablesListLabel.Text = "Conversion Tables";
            // 
            // dataGridViewTables
            // 
            this.dataGridViewTables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTables.Location = new System.Drawing.Point(30, 26);
            this.dataGridViewTables.Name = "dataGridViewTables";
            this.dataGridViewTables.Size = new System.Drawing.Size(295, 282);
            this.dataGridViewTables.TabIndex = 4;
            // 
            // tablesSelectLabel
            // 
            this.tablesSelectLabel.AutoSize = true;
            this.tablesSelectLabel.Location = new System.Drawing.Point(62, 312);
            this.tablesSelectLabel.Name = "tablesSelectLabel";
            this.tablesSelectLabel.Size = new System.Drawing.Size(95, 13);
            this.tablesSelectLabel.TabIndex = 5;
            this.tablesSelectLabel.Text = "Choose your table.";
            // 
            // generalDataViewLabel
            // 
            this.generalDataViewLabel.AutoSize = true;
            this.generalDataViewLabel.Location = new System.Drawing.Point(9, 16);
            this.generalDataViewLabel.Name = "generalDataViewLabel";
            this.generalDataViewLabel.Size = new System.Drawing.Size(65, 13);
            this.generalDataViewLabel.TabIndex = 6;
            this.generalDataViewLabel.Text = "Data Viewer";
            // 
            // buttonOpenDataFile
            // 
            this.buttonOpenDataFile.Location = new System.Drawing.Point(941, 243);
            this.buttonOpenDataFile.Name = "buttonOpenDataFile";
            this.buttonOpenDataFile.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenDataFile.TabIndex = 7;
            this.buttonOpenDataFile.Text = "Select File";
            this.buttonOpenDataFile.UseVisualStyleBackColor = true;
            this.buttonOpenDataFile.Click += new System.EventHandler(this.buttonOpenDataFile_Click);
            // 
            // buttonLoadDataFile
            // 
            this.buttonLoadDataFile.Location = new System.Drawing.Point(1022, 243);
            this.buttonLoadDataFile.Name = "buttonLoadDataFile";
            this.buttonLoadDataFile.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadDataFile.TabIndex = 8;
            this.buttonLoadDataFile.Text = "Analyze";
            this.buttonLoadDataFile.UseVisualStyleBackColor = true;
            this.buttonLoadDataFile.Click += new System.EventHandler(this.buttonLoadDataFile_Click);
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Location = new System.Drawing.Point(490, 246);
            this.textBoxFileName.MaxLength = 256;
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(445, 20);
            this.textBoxFileName.TabIndex = 9;
            // 
            // labelFilePath
            // 
            this.labelFilePath.AutoSize = true;
            this.labelFilePath.Location = new System.Drawing.Point(436, 249);
            this.labelFilePath.Name = "labelFilePath";
            this.labelFilePath.Size = new System.Drawing.Size(48, 13);
            this.labelFilePath.TabIndex = 10;
            this.labelFilePath.Text = "File Path";
            // 
            // panelConversionTable
            // 
            this.panelConversionTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelConversionTable.Controls.Add(this.buttonSaveTable);
            this.panelConversionTable.Controls.Add(this.dataGridViewTables);
            this.panelConversionTable.Controls.Add(this.conversionTablesList);
            this.panelConversionTable.Controls.Add(this.tablesListLabel);
            this.panelConversionTable.Controls.Add(this.tablesSelectLabel);
            this.panelConversionTable.Location = new System.Drawing.Point(12, 276);
            this.panelConversionTable.Name = "panelConversionTable";
            this.panelConversionTable.Size = new System.Drawing.Size(353, 357);
            this.panelConversionTable.TabIndex = 11;
            // 
            // buttonSaveTable
            // 
            this.buttonSaveTable.Location = new System.Drawing.Point(250, 327);
            this.buttonSaveTable.Name = "buttonSaveTable";
            this.buttonSaveTable.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveTable.TabIndex = 15;
            this.buttonSaveTable.Text = "Save Table";
            this.buttonSaveTable.UseVisualStyleBackColor = true;
            // 
            // progressBarConversion
            // 
            this.progressBarConversion.Location = new System.Drawing.Point(5, 27);
            this.progressBarConversion.Maximum = 9;
            this.progressBarConversion.Name = "progressBarConversion";
            this.progressBarConversion.Size = new System.Drawing.Size(282, 23);
            this.progressBarConversion.Step = 1;
            this.progressBarConversion.TabIndex = 12;
            // 
            // labelConversionStatus
            // 
            this.labelConversionStatus.AutoSize = true;
            this.labelConversionStatus.Location = new System.Drawing.Point(2, 2);
            this.labelConversionStatus.Name = "labelConversionStatus";
            this.labelConversionStatus.Size = new System.Drawing.Size(110, 13);
            this.labelConversionStatus.TabIndex = 13;
            this.labelConversionStatus.Text = "Redpoint Status Feed";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBoxStatusMessages);
            this.panel1.Controls.Add(this.buttonStartConversion);
            this.panel1.Controls.Add(this.labelConversionStatus);
            this.panel1.Controls.Add(this.progressBarConversion);
            this.panel1.Location = new System.Drawing.Point(371, 276);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(291, 110);
            this.panel1.TabIndex = 14;
            // 
            // textBoxStatusMessages
            // 
            this.textBoxStatusMessages.Location = new System.Drawing.Point(5, 74);
            this.textBoxStatusMessages.Multiline = true;
            this.textBoxStatusMessages.Name = "textBoxStatusMessages";
            this.textBoxStatusMessages.ReadOnly = true;
            this.textBoxStatusMessages.Size = new System.Drawing.Size(282, 28);
            this.textBoxStatusMessages.TabIndex = 16;
            // 
            // buttonStartConversion
            // 
            this.buttonStartConversion.Location = new System.Drawing.Point(212, 2);
            this.buttonStartConversion.Name = "buttonStartConversion";
            this.buttonStartConversion.Size = new System.Drawing.Size(75, 23);
            this.buttonStartConversion.TabIndex = 15;
            this.buttonStartConversion.Text = "Convert";
            this.buttonStartConversion.UseVisualStyleBackColor = true;
            this.buttonStartConversion.Click += new System.EventHandler(this.buttonStartConversion_Click);
            // 
            // timerConvertProgress
            // 
            this.timerConvertProgress.Tick += new System.EventHandler(this.timerConvertProgress_Tick);
            // 
            // buttonViewReport
            // 
            this.buttonViewReport.Location = new System.Drawing.Point(371, 392);
            this.buttonViewReport.Name = "buttonViewReport";
            this.buttonViewReport.Size = new System.Drawing.Size(75, 23);
            this.buttonViewReport.TabIndex = 15;
            this.buttonViewReport.Text = "View Report";
            this.buttonViewReport.UseVisualStyleBackColor = true;
            // 
            // buttonDeclineReport
            // 
            this.buttonDeclineReport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OrangeRed;
            this.buttonDeclineReport.Location = new System.Drawing.Point(587, 392);
            this.buttonDeclineReport.Name = "buttonDeclineReport";
            this.buttonDeclineReport.Size = new System.Drawing.Size(75, 23);
            this.buttonDeclineReport.TabIndex = 16;
            this.buttonDeclineReport.Text = "Decline";
            this.buttonDeclineReport.UseVisualStyleBackColor = true;
            this.buttonDeclineReport.MouseEnter += new System.EventHandler(this.buttonDeclineReport_MouseEnter);
            this.buttonDeclineReport.MouseLeave += new System.EventHandler(this.buttonDeclineReport_MouseLeave);
            // 
            // buttonAcceptReport
            // 
            this.buttonAcceptReport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGreen;
            this.buttonAcceptReport.Location = new System.Drawing.Point(480, 392);
            this.buttonAcceptReport.Name = "buttonAcceptReport";
            this.buttonAcceptReport.Size = new System.Drawing.Size(75, 23);
            this.buttonAcceptReport.TabIndex = 17;
            this.buttonAcceptReport.Text = "Accept";
            this.buttonAcceptReport.UseVisualStyleBackColor = true;
            this.buttonAcceptReport.MouseEnter += new System.EventHandler(this.buttonAcceptReport_MouseEnter);
            this.buttonAcceptReport.MouseLeave += new System.EventHandler(this.buttonAcceptReport_MouseLeave);
            // 
            // fileConversionInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1148, 636);
            this.Controls.Add(this.buttonAcceptReport);
            this.Controls.Add(this.buttonDeclineReport);
            this.Controls.Add(this.buttonViewReport);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelConversionTable);
            this.Controls.Add(this.labelFilePath);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.buttonLoadDataFile);
            this.Controls.Add(this.buttonOpenDataFile);
            this.Controls.Add(this.generalDataViewLabel);
            this.Controls.Add(this.dataGridViewGeneral);
            this.KeyPreview = true;
            this.Name = "fileConversionInterface";
            this.Text = "A";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGeneral)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTables)).EndInit();
            this.panelConversionTable.ResumeLayout(false);
            this.panelConversionTable.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataGridView dataGridViewGeneral;
        private System.Windows.Forms.ComboBox conversionTablesList;
        private System.Windows.Forms.Label tablesListLabel;
        private System.Windows.Forms.DataGridView dataGridViewTables;
        private System.Windows.Forms.Label tablesSelectLabel;
        private System.Windows.Forms.Label generalDataViewLabel;
        private System.Windows.Forms.Button buttonOpenDataFile;
        private System.Windows.Forms.Button buttonLoadDataFile;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Label labelFilePath;
        private System.Windows.Forms.Panel panelConversionTable;
        private System.Windows.Forms.ProgressBar progressBarConversion;
        private System.Windows.Forms.Label labelConversionStatus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonSaveTable;
        private System.Windows.Forms.Button buttonStartConversion;
        private System.Windows.Forms.Timer timerConvertProgress;
        private System.Windows.Forms.TextBox textBoxStatusMessages;
        private System.Windows.Forms.Button buttonViewReport;
        private System.Windows.Forms.Button buttonDeclineReport;
        private System.Windows.Forms.Button buttonAcceptReport;
    }
}

