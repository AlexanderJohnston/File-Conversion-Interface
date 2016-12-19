namespace MainWindow
{
    partial class MainForm
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
            this.readConfig = new System.Windows.Forms.Button();
            this.conversionTablesList = new System.Windows.Forms.ComboBox();
            this.tablesListLabel = new System.Windows.Forms.Label();
            this.dataGridViewTables = new System.Windows.Forms.DataGridView();
            this.tablesSelectLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGeneral)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTables)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewGeneral
            // 
            this.dataGridViewGeneral.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGeneral.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewGeneral.Name = "dataGridViewGeneral";
            this.dataGridViewGeneral.Size = new System.Drawing.Size(849, 150);
            this.dataGridViewGeneral.TabIndex = 0;
            // 
            // readConfig
            // 
            this.readConfig.Location = new System.Drawing.Point(517, 376);
            this.readConfig.Name = "readConfig";
            this.readConfig.Size = new System.Drawing.Size(75, 23);
            this.readConfig.TabIndex = 1;
            this.readConfig.Text = "View Config";
            this.readConfig.UseVisualStyleBackColor = true;
            this.readConfig.Click += new System.EventHandler(this.readConfig_Click);
            // 
            // conversionTablesList
            // 
            this.conversionTablesList.FormattingEnabled = true;
            this.conversionTablesList.Location = new System.Drawing.Point(12, 495);
            this.conversionTablesList.Name = "conversionTablesList";
            this.conversionTablesList.Size = new System.Drawing.Size(159, 21);
            this.conversionTablesList.TabIndex = 2;
            this.conversionTablesList.SelectedIndexChanged += new System.EventHandler(this.conversionTablesList_SelectedIndexChanged);
            // 
            // tablesListLabel
            // 
            this.tablesListLabel.AutoSize = true;
            this.tablesListLabel.Location = new System.Drawing.Point(13, 170);
            this.tablesListLabel.Name = "tablesListLabel";
            this.tablesListLabel.Size = new System.Drawing.Size(95, 13);
            this.tablesListLabel.TabIndex = 3;
            this.tablesListLabel.Text = "Conversion Tables";
            // 
            // dataGridViewTables
            // 
            this.dataGridViewTables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTables.Location = new System.Drawing.Point(12, 187);
            this.dataGridViewTables.Name = "dataGridViewTables";
            this.dataGridViewTables.Size = new System.Drawing.Size(295, 282);
            this.dataGridViewTables.TabIndex = 4;
            // 
            // tablesSelectLabel
            // 
            this.tablesSelectLabel.AutoSize = true;
            this.tablesSelectLabel.Location = new System.Drawing.Point(14, 478);
            this.tablesSelectLabel.Name = "tablesSelectLabel";
            this.tablesSelectLabel.Size = new System.Drawing.Size(95, 13);
            this.tablesSelectLabel.TabIndex = 5;
            this.tablesSelectLabel.Text = "Choose your table.";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 528);
            this.Controls.Add(this.tablesSelectLabel);
            this.Controls.Add(this.dataGridViewTables);
            this.Controls.Add(this.tablesListLabel);
            this.Controls.Add(this.conversionTablesList);
            this.Controls.Add(this.readConfig);
            this.Controls.Add(this.dataGridViewGeneral);
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGeneral)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTables)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataGridView dataGridViewGeneral;
        private System.Windows.Forms.Button readConfig;
        private System.Windows.Forms.ComboBox conversionTablesList;
        private System.Windows.Forms.Label tablesListLabel;
        private System.Windows.Forms.DataGridView dataGridViewTables;
        private System.Windows.Forms.Label tablesSelectLabel;
    }
}

