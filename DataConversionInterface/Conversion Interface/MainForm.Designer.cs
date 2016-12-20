namespace MainWindow
{
    partial class textBoxSelectedFile
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGeneral)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTables)).BeginInit();
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
            this.conversionTablesList.Location = new System.Drawing.Point(12, 608);
            this.conversionTablesList.Name = "conversionTablesList";
            this.conversionTablesList.Size = new System.Drawing.Size(159, 21);
            this.conversionTablesList.TabIndex = 2;
            this.conversionTablesList.SelectedIndexChanged += new System.EventHandler(this.conversionTablesList_SelectedIndexChanged);
            // 
            // tablesListLabel
            // 
            this.tablesListLabel.AutoSize = true;
            this.tablesListLabel.Location = new System.Drawing.Point(9, 284);
            this.tablesListLabel.Name = "tablesListLabel";
            this.tablesListLabel.Size = new System.Drawing.Size(95, 13);
            this.tablesListLabel.TabIndex = 3;
            this.tablesListLabel.Text = "Conversion Tables";
            // 
            // dataGridViewTables
            // 
            this.dataGridViewTables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTables.Location = new System.Drawing.Point(12, 300);
            this.dataGridViewTables.Name = "dataGridViewTables";
            this.dataGridViewTables.Size = new System.Drawing.Size(295, 282);
            this.dataGridViewTables.TabIndex = 4;
            // 
            // tablesSelectLabel
            // 
            this.tablesSelectLabel.AutoSize = true;
            this.tablesSelectLabel.Location = new System.Drawing.Point(14, 591);
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
            // textBoxSelectedFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1148, 636);
            this.Controls.Add(this.labelFilePath);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.buttonLoadDataFile);
            this.Controls.Add(this.buttonOpenDataFile);
            this.Controls.Add(this.generalDataViewLabel);
            this.Controls.Add(this.tablesSelectLabel);
            this.Controls.Add(this.dataGridViewTables);
            this.Controls.Add(this.tablesListLabel);
            this.Controls.Add(this.conversionTablesList);
            this.Controls.Add(this.dataGridViewGeneral);
            this.KeyPreview = true;
            this.Name = "textBoxSelectedFile";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGeneral)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTables)).EndInit();
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
    }
}

