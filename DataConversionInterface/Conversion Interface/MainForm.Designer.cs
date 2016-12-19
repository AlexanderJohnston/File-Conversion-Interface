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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGeneral)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewGeneral
            // 
            this.dataGridViewGeneral.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGeneral.Location = new System.Drawing.Point(64, 117);
            this.dataGridViewGeneral.Name = "dataGridViewGeneral";
            this.dataGridViewGeneral.Size = new System.Drawing.Size(250, 208);
            this.dataGridViewGeneral.TabIndex = 0;
            // 
            // readConfig
            // 
            this.readConfig.Location = new System.Drawing.Point(428, 168);
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
            this.conversionTablesList.Location = new System.Drawing.Point(64, 346);
            this.conversionTablesList.Name = "conversionTablesList";
            this.conversionTablesList.Size = new System.Drawing.Size(159, 21);
            this.conversionTablesList.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 528);
            this.Controls.Add(this.conversionTablesList);
            this.Controls.Add(this.readConfig);
            this.Controls.Add(this.dataGridViewGeneral);
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGeneral)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dataGridViewGeneral;
        private System.Windows.Forms.Button readConfig;
        private System.Windows.Forms.ComboBox conversionTablesList;
    }
}

