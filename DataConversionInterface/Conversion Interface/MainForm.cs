using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainWindow
{
    /* TO-DO List:
     * Re-write variables as constants wherever possible.
     * Add in enough elegant error handling and log reporting for stability.
     * Write networking functions to communicate with other users on LAN using the same software.
     * !FIX! = marker in comments for me to attend to in the future.
     */
    public partial class fileConversionInterface : Form
    {
        // Replace this with a config file later.
        const string tablesFinderFiles = @"\\engagests1\Elements\Prospect Jobs\Conversions\01-File Conversions\Redpoint Finder\Downloaded\Tables\";
        const string tablesCreditCards = @"\\engagests1\Elements\Prospect Jobs\Conversions\01-File Conversions\Credit Cards\Tables\";
        const string statusFinderFiles = @"\\engagests1\Elements\Prospect Jobs\Conversions\01-File Conversions\Redpoint Finder\Downloaded\Layout\Status Files\";
        const string statusCreditCards = @"\\engagests1\Elements\Prospect Jobs\Conversions\01-File Conversions\Redpoint Finder\Downloaded\Layout\Status Files\";
        const string reportPath = @"\\engagests1\Elements\Prospect Jobs\Conversions\01-File Conversions\Redpoint Finder\Downloaded\Staging\";
        const string dataPath = @"\\engagests1\Elements\Prospect Jobs\Conversions\01-File Conversions\Redpoint Finder\Downloaded\";
        int linesViewCount = 1000;
        bool linesViewAll = false;

        // Variables for the global timer and message box.
        List<string> statusMessages = new List<string>();

        // Start methods.

        public fileConversionInterface()
        {
            InitializeComponent();
            InitialSetup.Start();
            InitializeTableList();
            InitializeContextMenu();

            // Double buffer the data grid view to prevent flickering.
            dataGridViewGeneral.DoubleBuffered(true);
        }

        // Set up the context menu for header cells.
        private void InitializeContextMenu()
        {
            String[] houseFields = new string[] { "Finder_Number", "Keycode", "Title", "First_Name", "Middle_Name", "Last_Name", "Suffix", "Gender", "Company", "Primary_Addr", "Secondary_Addr", "City", "State", "Zip", "Zip4", "Occupation", "Employer", "Work_Phone", "Home_Phone", "Cell_Phone", "Fax", "Email", "Client_Source", "Entity", "LCV_Account_ID", "Contact_ID", "EML_Appeal_ID", "Package_Type", "Client_Code", "Spouse_Title", "Spouse_First", "Spouse_Mid", "Spouse_Last", "Spouse_Suffix", "Address3", "Country", "Import_Id", "Address_Import_Id" };
            foreach (string item in houseFields)
            {
                contextMenuHeaders.Items.Add(item);
            }
        }

        // Set up the table list and remove the definition file from an array of existing files.
        private void InitializeTableList()
        {
            ConversionTable displayTables = new ConversionTable();

            // Get a list of available tables and clean up path data for the combo box.
            string[] cleanedTablesContent = displayTables.GetTableList(tablesFinderFiles).Select(s => s.Replace(tablesFinderFiles, "")).ToArray();
            // Remove the house fields reference table.
            cleanedTablesContent = cleanedTablesContent.Where(s => s != "HOUSE_FIELDS.txt").ToArray();
            conversionTablesList.DataSource = cleanedTablesContent;
        }

        // This event fires whenever a new lookup table is selected by the user.
        private void conversionTablesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Start up the grid view for this selected table.
            DataTable selectedTable = new DataTable();
            selectedTable.Columns.Add("CLIENT");
            selectedTable.Columns.Add("HOUSE");
            string tableLocation = null;
            if (checkCreditCards.Checked == true)
            {
                // Return the credit card tables.
                tableLocation = tablesCreditCards + conversionTablesList.SelectedValue.ToString();
            }
            else
            {
                // Select the finder file tables.
                tableLocation = tablesFinderFiles + conversionTablesList.SelectedValue.ToString();
            }



            // Fill in the gridviewer rows with the new conversion table.
            StreamReader tableReader = new StreamReader(tableLocation);
            // Get rid of the header.
            tableReader.ReadLine();
            string[] tableContent = new string[File.ReadAllLines(tableLocation).Length];
            try
            {
                while (!tableReader.EndOfStream)
                {
                    tableContent = tableReader.ReadLine().Split(',');
                    selectedTable.Rows.Add(tableContent[0], tableContent[1]);
                }
            }
            catch (Exception)
            {
                // !FIX! make this more detailed.
                MessageBox.Show("Not enough columns in your table.", "Table Error");
            }


            // Display the data table.
            dataGridViewTables.DataSource = selectedTable;
            tableReader.Close();

            // Color the header cells.
            colorHeaderCellsByComparison();
        }

        // Open a new file selection window when a user select Load File.
        private void buttonOpenDataFile_Click(object sender, EventArgs e)
        {
            string dataFilePath = null;
            OpenFileDialog selectFile = new OpenFileDialog();
            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                dataFilePath = selectFile.FileName;
            }

            textBoxFileName.Text = dataFilePath;
        }

        private void buttonLoadDataFile_Click(object sender, EventArgs e)
        {
            // Make sure that the file exists before opening it. Also check that it's a valid format.
            string[] validFormats = new string[] { "csv", "tab" };
            if (File.Exists(textBoxFileName.Text.ToString())
                && validFormats.Contains(
                    textBoxFileName.Text.Substring(
                    textBoxFileName.Text.Length - 3, 3)
                    )
                // End contains statement.
                )
            // End if statement.
            {
                // Store the file selection textbox as a string.
                string dataFilePath = textBoxFileName.Text.ToString();
                FileInfo fileLengthCheck = new FileInfo(dataFilePath);
                // Check to make sure the user isn't about to overload memory due to the data structures I am using.
                // !FIX!
                if (fileLengthCheck.Length > 30000000 && linesViewAll == true)
                {
                    MessageBox.Show("The file is too large. Only 2,000 lines will be displayed, but you can try more. \r\n This will be fixed soon.", "File Size");
                    linesViewAll = false;
                    linesViewCount = 2000;
                }
                // Ensure that we aren't loading an empty space.

                if (textBoxFileName.Text.ToString() != "")
                {
                    // Set up the line count variable.
                    int displayTotalLines = 0;
                    // Set up variables to read the data file.
                    StreamReader dataFileReader = new StreamReader(dataFilePath);
                    List<string> dataFileContent = new List<string>();
                    string regexMatch = null;
                    // Counter for header array
                    int u = 0;
                    int i = 0;
                    // Regex for parsing.
                    Regex regexSplitFinal = new Regex("");
                    // Delimiter character.
                    char finalDelimiterChar = ',';

                    // Determine which type of file we are reading.
                    string fileTabOrCSV = FileManagement.fileTABorCSV(dataFilePath);

                    // Time to read the file into memory for display to grab header record.
                    // Load Regex to parse the strings.
                    if (fileTabOrCSV == "TAB")
                    {
                        regexSplitFinal = new Regex("(?:^|\t)(\"(?:[^\"]+|\"\")*\"|[^\t]*)", RegexOptions.Compiled);
                        foreach (Match match in regexSplitFinal.Matches(dataFileReader.ReadLine()))
                        {
                            regexMatch = match.Value;
                            if (0 == regexMatch.Length)
                            {
                                dataFileContent.Add("");
                                u++;
                            }

                            dataFileContent.Add(regexMatch.ToString().TrimStart('\t'));
                            u++;
                        }
                        finalDelimiterChar = '\t';
                    }
                    else if (fileTabOrCSV == "CSV")
                    {
                        regexSplitFinal = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)", RegexOptions.Compiled);
                        foreach (Match match in regexSplitFinal.Matches(dataFileReader.ReadLine()))
                        {
                            regexMatch = match.Value;
                            if (0 == regexMatch.Length)
                            {
                                dataFileContent.Add("");
                                u++;
                            }

                            dataFileContent.Add(regexMatch.ToString().TrimStart(','));
                            u++;
                        }
                        finalDelimiterChar = ',';
                    }

                    // Build the data grid view with a variable number of columns set as the header record.
                    DataTable dataTableGeneral = DataViewerControls.DataFileVariableGrid(dataFileContent.Count(), dataFileContent.ToArray());

                    // Clear out the dataFileContent array.
                    dataFileContent.Clear();

                    // Load all lines into a list of string arrays to make a data structure with columns and rows.
                    // Skip over the header using int i = 1 as starting point.
                    int sizeArray = dataFileContent.Count();
                    List<string[]> dataFileLines = new List<string[]>(sizeArray);

                    // Load a number of arrays where each value is a column, one at a time into the list dataFileLines.
                    // First, find out if there are less than 60 lines.
                    var lineCounter = File.ReadLines(dataFilePath).Count();

                    // Display the line count for the file.
                    labelRecordCount.Text = "Record Count: " + lineCounter.ToString();

                    // Initialize a variable to determine line count for display.
                    if (linesViewAll == false) { displayTotalLines = linesViewCount; }
                    else { displayTotalLines = lineCounter; }
                    if (displayTotalLines > lineCounter) { displayTotalLines = lineCounter; }

                    // Build the data to display.
                    for (i = 1; i < displayTotalLines; i++)
                    {
                        foreach (Match match in regexSplitFinal.Matches(dataFileReader.ReadLine()))
                        {
                            regexMatch = match.Value;
                            if (0 == regexMatch.Length)
                            {
                                dataFileContent.Add("");
                                u++;
                            }
                            else
                            {
                                dataFileContent.Add(regexMatch.ToString().TrimStart(finalDelimiterChar));
                            }
                            u++;
                        }

                        dataFileLines.Add(dataFileContent.ToArray());
                        dataFileContent.Clear();
                    }

                    // Close the file.
                    dataFileReader.Close();

                    // Add the rows to the data grid viewer.
                    try
                    {
                        foreach (string[] rows in dataFileLines)
                        {
                            dataTableGeneral.Rows.Add(rows);
                        }
                        dataGridViewGeneral.DataSource = dataTableGeneral;
                    }
                    catch (NullReferenceException)
                    {
                        MessageBox.Show("The file you are trying to open does not have enough columns.", "Not a table!");
                    }
                    catch (ArgumentException)
                    {
                        MessageBox.Show("This program can't view that type of file yet.", "Bad file!");
                    }

                    // Color the header cells.
                    colorHeaderCellsByComparison();

                    // Disable column sorting.
                    DisableColumnSorting();
                }

            }
            else
            {
                MessageBox.Show("The file you have selected does not exist, or is not a CSV/TAB file.", "Missing File");
            }

        }

        private void DisableColumnSorting()
        {
            foreach (DataGridViewColumn column in dataGridViewGeneral.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void colorHeaderCellsByComparison()
        {
            try
            {
                // Strings will be sent .ToUpper() so that they can be compared.
                // Gather an array of headers that are defined on the conversion table.
                int tableRowCount = dataGridViewTables.Rows.Count;
                string[] tableRowText = new string[tableRowCount];
                // Counter for the foreach iteration.
                int i = 0;

                // Build the array.
                foreach (DataGridViewRow dataRow in dataGridViewTables.Rows)
                {
                    // Catch a null exception and add an empty string instead.
                    try { tableRowText[i] = dataRow.Cells[0].Value.ToString().ToUpper(); }
                    catch (NullReferenceException) { tableRowText[i] = string.Empty; }
                    // !FIX! add more exceptions.
                    catch (Exception) {; }
                    i++;
                }

                // Check to see if the header on the data table is in the conversion definitions.
                // Improve the speed on this... Use caching? !FIX!
                foreach (DataGridViewColumn dataCol in dataGridViewGeneral.Columns)
                {
                    if (tableRowText
                        .Contains(dataCol.HeaderText.ToString().ToUpper()
                        .Trim('"'))
                        )
                    { dataCol.HeaderCell.Style.BackColor = Color.LightGreen; }
                    else { dataCol.HeaderCell.Style.BackColor = Color.LightSalmon; }
                }

                // Disable the default visual styles.
                dataGridViewGeneral.EnableHeadersVisualStyles = false;
            }

            catch (Exception)
            {
                // !FIX!
                MessageBox.Show("The header cell colors failed. Please let me know about this.", "Header Color Failure");
            }

        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                buttonOpenDataFile_Click(sender, e);
            }

            if (e.Control && e.KeyCode == Keys.A)
            {
                buttonLoadDataFile_Click(sender, e);
            }

            if (e.Control && e.KeyCode == Keys.C)
            {
                buttonStartConversion_Click(sender, e);
            }

            if (e.Control && e.KeyCode == Keys.E)
            {
                buttonSaveTable_Click(sender, e);
            }
        }

        private void buttonStartConversion_Click(object sender, EventArgs e)
        {
            // Safety check!
            if ((MessageBox.Show("Are you sure you want to start conversion using "
                + conversionTablesList.Text.ToString()
                + "?", "Safety Check!", MessageBoxButtons.YesNo)) == DialogResult.Yes)
            {
                string dataFilePath = textBoxFileName.Text.ToString();

                // Open select file window if one has not been chosen.
                if (dataFilePath == "") { buttonOpenDataFile_Click(sender, e); }

                // Start the timer which handles the progress bar and status messages.
                timerConvertProgress.Enabled = true;

                // Start the conversion process by moving the selected file into Redpoint's automation folders.
                string dataFileFormat = Path.GetExtension(dataFilePath);
                string dataClientCode = conversionTablesList.Text.ToString();
                dataClientCode = dataClientCode.Substring(0, 2);
                bool boolConversionSuccesss = ConversionUtilities.StartConversion(dataFilePath, dataClientCode, dataFileFormat);
            }
            else
            {

            }
        }

        private void buttonConvertCredit_Click(object sender, EventArgs e)
        {

            // Safety check!
            if ((MessageBox.Show("Are you sure you want to start conversion using "
                + conversionTablesList.Text.ToString()
                + "?", "Safety Check!", MessageBoxButtons.YesNo)) == DialogResult.Yes)
            {
                string dataFilePath = textBoxFileName.Text.ToString();

                // Open select file window if one has not been chosen.
                if (dataFilePath == "") { buttonOpenDataFile_Click(sender, e); }

                // Start the timer which handles the progress bar and status messages.
                timerConvertProgress.Enabled = true;

                // Start the conversion process by moving the selected file into Redpoint's automation folders.
                string dataFileFormat = Path.GetExtension(dataFilePath);
                string dataClientCode = conversionTablesList.Text.ToString();
                dataClientCode = dataClientCode.Substring(0, 2);
                bool boolConversionSuccesss = ConversionUtilities.StartConversion(dataFilePath, dataClientCode, dataFileFormat);
            }
            else
            {

            }
        }

    // This timer handles the update of the progress bar and status message below it.
    private void timerConvertProgress_Tick(object sender, EventArgs e)
        {
            // The status messages file is 60 byte fixed width.
            // Check to see if file size has changed.
            using (FileStream redPointLogStream = File.Open(statusFinderFiles + "CurrentStatus.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // Go to the end of the file and roll back 60 KB.
                if (redPointLogStream.Length > 59)
                {
                    redPointLogStream.Seek(-60, SeekOrigin.End);
                    // Read the bytes that were rolled back.
                    byte[] bytesNew = new byte[60];
                    redPointLogStream.Read(bytesNew, 0, 60);
                    // Convert the bytes to a string.
                    string newLogData = Encoding.Default.GetString(bytesNew);
                    textBoxStatusMessages.Text = newLogData.ToString();
                    // Run a switch based on the first byte of the line we're reading, which is numbered by Redpoint's conversion step.
                    // I bet I could write this much smaller. !FIX!
                    switch (newLogData.ToString()[0])
                    {
                        case '1':
                            progressBarConversion.Value = 1;
                            break;
                        case '2':
                            progressBarConversion.Value = 2;
                            break;
                        case '3':
                            progressBarConversion.Value = 3;
                            break;
                        case '4':
                            progressBarConversion.Value = 4;
                            break;
                        case '5':
                            progressBarConversion.Value = 5;
                            break;
                        case '6':
                            progressBarConversion.Value = 6;
                            break;
                        case '7':
                            progressBarConversion.Value = 7;
                            break;
                        case '8':
                            progressBarConversion.Value = 8;
                            break;
                        case '9':
                            progressBarConversion.Value = 9;
                            break;
                        case '0':
                            Thread.Sleep(2250);
                            System.IO.File.WriteAllText(statusFinderFiles + "CurrentStatus.txt", string.Empty);
                            timerConvertProgress.Enabled = false;
                            textBoxStatusMessages.Text = "";
                            progressBarConversion.Value = 0;
                            break;
                    }

                }
                else
                {
                    textBoxStatusMessages.Text = "0: Conversion has not started.";
                    progressBarConversion.Value = 0;
                }
            }
        }

        // This timer handles the credit card conversion status messages.
        private void timerCreditCards_Tick(object sender, EventArgs e)
        {
            // The status messages file is 60 byte fixed width.
            // Check to see if file size has changed.
            using (FileStream redPointLogStream = File.Open(statusCreditCards + "CurrentStatus.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // Go to the end of the file and roll back 60 KB.
                if (redPointLogStream.Length > 59)
                {
                    redPointLogStream.Seek(-60, SeekOrigin.End);
                    // Read the bytes that were rolled back.
                    byte[] bytesNew = new byte[60];
                    redPointLogStream.Read(bytesNew, 0, 60);
                    // Convert the bytes to a string.
                    string newLogData = Encoding.Default.GetString(bytesNew);
                    textBoxStatusMessages.Text = newLogData.ToString();
                    // Run a switch based on the first byte of the line we're reading, which is numbered by Redpoint's conversion step.
                    // I bet I could write this much smaller. !FIX!
                    switch (newLogData.ToString()[0])
                    {
                        case '1':
                            progressBarConversion.Value = 1;
                            break;
                        case '2':
                            progressBarConversion.Value = 2;
                            break;
                        case '3':
                            progressBarConversion.Value = 3;
                            break;
                        case '4':
                            progressBarConversion.Value = 4;
                            break;
                        case '5':
                            progressBarConversion.Value = 5;
                            break;
                        case '6':
                            progressBarConversion.Value = 6;
                            break;
                        case '7':
                            progressBarConversion.Value = 7;
                            break;
                        case '8':
                            progressBarConversion.Value = 8;
                            break;
                        case '9':
                            progressBarConversion.Value = 9;
                            break;
                        case '0':
                            Thread.Sleep(2250);
                            System.IO.File.WriteAllText(statusFinderFiles + "CurrentStatus.txt", string.Empty);
                            timerConvertProgress.Enabled = false;
                            textBoxStatusMessages.Text = "";
                            progressBarConversion.Value = 0;
                            break;
                    }

                }
                else
                {
                    textBoxStatusMessages.Text = "0: Conversion has not started.";
                    progressBarConversion.Value = 0;
                }
            }
        }

        // Basic button controls and design below this comment.
        private void buttonAcceptReport_MouseEnter(object sender, EventArgs e)
        {
            buttonAcceptReport.BackColor = Color.LightGreen;
        }

        private void buttonAcceptReport_MouseLeave(object sender, EventArgs e)
        {
            buttonAcceptReport.BackColor = default(Color);
        }

        private void buttonDeclineReport_MouseEnter(object sender, EventArgs e)
        {
            buttonDeclineReport.BackColor = Color.Tomato;
        }

        private void buttonDeclineReport_MouseLeave(object sender, EventArgs e)
        {
            buttonDeclineReport.BackColor = default(Color);
        }

        private void buttonViewReport_Click(object sender, EventArgs e)
        {
            // Load the report file path into the selected file box and then initiate a click event.
            textBoxFileName.Text = reportPath + "ACCEPTED.csv";
            buttonLoadDataFile.PerformClick();
        }

        private void buttonAcceptReport_Click(object sender, EventArgs e)
        {
            File.Move(reportPath + "ACCEPTED.csv", reportPath + @"Report\ACCEPTED.csv");
        }

        private void buttonDeclineReport_Click(object sender, EventArgs e)
        {
            File.Move(reportPath + "DECLINED.csv", reportPath + @"Report\DECLINED.csv");
        }

        private void buttonViewOriginalFile_Click(object sender, EventArgs e)
        {
            /*if (File.Exists(dataPath + "*.csv"))
            {
                textBoxFileName.Text = System
            }*/

            MessageBox.Show("This function isn't working yet.", "Woops!");
        }

        private void buttonViewAllLines_Click(object sender, EventArgs e)
        {
            linesViewAll = true;
            textBoxViewLines.Text = "";
            buttonLoadDataFile.PerformClick();

        }

        private void textBoxViewLines_TextChanged(object sender, EventArgs e)
        {
            try
            {
                linesViewCount = Convert.ToInt32(textBoxViewLines.Text.ToString());
                buttonLoadDataFile.PerformClick();
            }
            // Catch just so that nothing weird happens. No need to alert the user.
            catch (Exception)
            {
            }
        }

        private void dataGridViewGeneral_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView gridView = sender as DataGridView;
            if (null != gridView)
            {
                foreach (DataGridViewRow r in gridView.Rows)
                {
                    gridView.Rows[r.Index].HeaderCell.Value = (r.Index + 1).ToString();
                }
            }
            dataGridViewGeneral.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewGeneral.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //dataGridViewGeneral.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllHeaders;
        }

        private void buttonSaveTable_Click(object sender, EventArgs e)
        {
            try
            {
                // Write the data table to a file using stream writer, iterating over rows and columns.
                StreamWriter tablesWriter = new System.IO.StreamWriter(tablesFinderFiles + conversionTablesList.Text.ToString());
                int count = dataGridViewTables.Rows.Count;
                tablesWriter.WriteLine("CLIENT,HOUSE");

                foreach (DataGridViewRow dataRow in dataGridViewTables.Rows)
                {
                    if (dataRow.Cells[0].Value != null)
                    {
                        // Join the columns for a specific row together by commas.
                        tablesWriter.WriteLine(
                            string.Join(",", dataRow.Cells
                                .Cast<DataGridViewCell>()
                                .Where(c => c.Value != null)
                                .Select(c => c.Value.ToString()).ToArray())
                                );
                    }
                }

                // Close the writer.
                tablesWriter.Close();

                // Color cells with the new change.
                colorHeaderCellsByComparison();
            }
            catch (Exception)
            {
                // !FIX!
                MessageBox.Show("Table failed to save. Let me know about this.", "Save Error");
            }
        }

        private void buttonExcelConvert_Click(object sender, EventArgs e)
        {
            // Get the selected file, its name, and extension.
            string dataFilePath = textBoxFileName.Text.ToString();
            string dataFileName = Path.GetFileNameWithoutExtension(dataFilePath);
            string dataFileFormat = Path.GetExtension(dataFilePath);
            // Finally, get just the directory name. Add a \ character to the end for re-construction later.
            dataFilePath = Path.GetDirectoryName(dataFilePath) + @"\";

            // Pass these variables to Excel Convert.
            bool fileConverted = ExcelConvert.ConversionTools.TablesToCSV(dataFilePath, dataFileName, dataFileFormat);
            if (fileConverted == true)
            {
                // Change the selected file to the new csv.
                textBoxFileName.Text = dataFilePath + dataFileName + ".csv";
                // Send a click event to load the new csv.
                buttonLoadDataFile.PerformClick();
            }
            // End buttonExcelConvert_Click method.
        }

        private void buttonChopHeader_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure that a data file is loaded and exists.
                if (File.Exists(textBoxFileName.Text.ToString()) == true)
                {
                    // Prepare a list since we are just appending strings in order.
                    List<string> dataFileContent = new List<string>();

                    // Get the file as a string.
                    string dataFilePath = textBoxFileName.Text.ToString();
                    StreamReader dataFileStream = new StreamReader(dataFilePath);
                    // Skip one line to remove the current header.
                    dataFileStream.ReadLine();

                    // Iterate over the file into a list. 
                    // Save the while as a variable so that it can be used to test and then save if not null.
                    string nextLine = null;
                    while ((nextLine = dataFileStream.ReadLine()) != null)
                    {
                        dataFileContent.Add(nextLine);
                    }
                    // Close the stream reader.
                    dataFileStream.Close();

                    // Save the file out.
                    File.WriteAllLines(dataFilePath, dataFileContent);

                    // Send a click event to load the new csv or tab file.
                    buttonLoadDataFile.PerformClick();
                }
                else
                {
                    MessageBox.Show("Please load a file before attempting to use Chop Header.", "Data Missing!");
                }
                // End try statement.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chop Header was unable to run on this file.\r\n" + ex.ToString(), "File Error!");
            }
            // End buttonChopHeader method onClick.
        }

        private void buttonAddSourceCode_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure that a data file is loaded and exists.
                if (File.Exists(textBoxFileName.Text.ToString()) == true)
                {
                    // Ask the user which source code they wish to add.
                    string sourceCode = null;

                    // Make sure user has entered something.
                    if (textBoxSourceCode.Text.ToString() != null && textBoxSourceCode.Text.ToString() != "Type your source here.")
                    {
                        // Accept the source code.
                        sourceCode = textBoxSourceCode.Text.ToString();

                        // Prepare a list since we are just appending strings in order.
                        List<string> dataFileContent = new List<string>();

                        // Get the file as a string.
                        string dataFilePath = textBoxFileName.Text.ToString();

                        // Get the extension to determine delimiting character.
                        char charDelimiter = ',';
                        switch (Path.GetExtension(dataFilePath).ToUpper())
                        {
                            case ".CSV":
                                // Already set it to CSV as default.
                                break;
                            case ".TAB":
                                charDelimiter = '\t';
                                break;
                        }

                        // Open a stream reader for the file.
                        StreamReader dataFileStream = new StreamReader(dataFilePath);

                        // Add the header record.
                        dataFileContent.Add(dataFileStream.ReadLine() + charDelimiter + "Engage Source");

                        // Iterate over the file into a list. 
                        // Save the while as a variable so that it can be used to test and then save if not null.
                        string nextLine = null;
                        while ((nextLine = dataFileStream.ReadLine()) != null)
                        {
                            // Add the new line with a delimiter and the new source code.
                            dataFileContent.Add(nextLine + charDelimiter + sourceCode);
                        }
                        // Close the stream reader.
                        dataFileStream.Close();

                        // Save the file out.
                        File.WriteAllLines(dataFilePath, dataFileContent);

                        // Send a click event to load the new csv or tab file.
                        buttonLoadDataFile.PerformClick();
                    }
                    else
                    {
                        MessageBox.Show("Enter your source code into the text box first.", "Missing Source");
                    }
                }
                else
                {
                    MessageBox.Show("Please load a file before attempting to add a source code.", "Data Missing!");
                }
                // End try statement.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add source code.\r\n" + ex.ToString(), "Add Source");
            }
            // End buttonAddSourceCode method with click event.
        }

        // This will allow a user to select new fields and add them to the table.
        private void dataGridViewGeneral_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Get the column name for later.
            String dataColumnName = dataGridViewGeneral.Columns[e.ColumnIndex].Name.ToString();

            // Select a menu item and add it as a newrow with the selected item being column index 1 on the table.
            contextMenuHeaders.Show(Cursor.Position);
            // Do not move on until an item is selected.
            while (contextMenuHeaders.Visible == true) Application.DoEvents();

            // Call up the data table.
            var newItemAdded = (dataGridViewTables.DataSource as DataTable);

            // The count of rows will equal the last row in the table off by one, and we need to add our selected header to index 0.
            int countRows = newItemAdded.Rows.Count;
            DataRow dr = newItemAdded.Rows[countRows - 1];
            dr[0] = dataColumnName.ToString();

            // Set the datasource once more for our table view.
            dataGridViewTables.DataSource = newItemAdded;
        }

        private void contextMenuHeaders_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var newItemAdded = (dataGridViewTables.DataSource as DataTable);
            DataRow dr = newItemAdded.NewRow();
            dr[0] = "";
            dr[1] = e.ClickedItem.ToString();
            newItemAdded.Rows.Add(dr);
            dataGridViewTables.DataSource = newItemAdded;
        }

        private void checkCreditCards_CheckStateChanged(object sender, EventArgs e)
        {
            // Initialize a ConversionTable to hold the new values.
            ConversionTable displayTables = new ConversionTable();

            // This checkbox will determine which conversion tables we use, and where the files end up later.
            if (checkCreditCards.Checked == true)
            {
                // Get a list of available tables and clean up path data for the combo box.
                string[] cleanedTablesContent = displayTables.GetTableList(tablesCreditCards).Select(s => s.Replace(tablesCreditCards, "")).ToArray();
                // Remove the house fields reference table.
                cleanedTablesContent = cleanedTablesContent.Where(s => s != "HOUSE_FIELDS.txt").ToArray();
                conversionTablesList.DataSource = cleanedTablesContent;
            }
            else
            {
                // Get a list of available tables and clean up path data for the combo box.
                string[] cleanedTablesContent = displayTables.GetTableList(tablesFinderFiles).Select(s => s.Replace(tablesFinderFiles, "")).ToArray();
                // Remove the house fields reference table.
                cleanedTablesContent = cleanedTablesContent.Where(s => s != "HOUSE_FIELDS.txt").ToArray();
                conversionTablesList.DataSource = cleanedTablesContent;
            }
        }


        // End fileConversionInterface class.
    }

    public class InitialSetup
    {
        /* Issues to consider:
         * The login prompt can be brute forced by throwing the wrong answers and closing it.
         * The config file management isn't particularly elegant.
         */
        public static void Start()
        {
            // Log in to the application.
            bool validLogin = false;
            string userName = UserName();
            string userPass = Password();
            MD5 md5Hash = MD5.Create();
            byte[] finalPassword = PasswordManagement.HashPassword(userPass, md5Hash);

            // Create config file if it does not exist.
            string configPath = @"config\";
            string configName = "users.cfg";
            // Re-inventing the wheel. !FIX!
            bool configExists = FileManagement.CheckFileExists(configName, configPath);
            if (configExists == false)
            {
                FileManagement.CreateNewConfig();
            }

            // Check for user in the config file.
            bool existingUser = FileManagement.UserExistsConfig(userName);

            // Add the user to the config file if they do not exist.
            if (existingUser == false)
            {
                // Pass the username and hash-salt password by converting the byte array into a string.
                FileManagement.NewUserCreate(userName, System.Text.Encoding.Default.GetString(finalPassword));
                validLogin = true;
            }
            else
            {
                validLogin = FileManagement.CheckUserLogin(userName, userPass);
            }

            // Exit the application if the login is false.
            if (validLogin == false &&
                MessageBox.Show("Invalid login, try again?", "Login Attempt",
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                // !FIX!
                Start();
            }
            else if (validLogin == false)
            {
                Environment.Exit(1);
            }
            // Program setup is now complete.
        }

        public static string UserName()
        {
            string userName = null;
            string userFormCaption = "Login";
            string userFormText = "Welcome to the file conversion interface." + '\n' + "Please enter your first and last name.";
            userName = Prompt.ShowDialog(userFormText, userFormCaption);
            if (userName == "") { Environment.Exit(2); }
            return userName;
        }

        public static string Password()
        {
            // Hash the password.
            string userPassword = null;
            string userFormCaption = "Password";
            string userFormText = "Please enter your password.";
            userPassword = Prompt.ShowDialogPass(userFormText, userFormCaption);
            return userPassword;
        }

        // End the start class.
    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            textLabel.AutoSize = true;
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            System.Windows.Forms.Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;


            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        public static string ShowDialogPass(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            textLabel.AutoSize = true;
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            textBox.PasswordChar = '*';
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;


            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
        // End Prompt class.
    }

    /* 
     This password hashing block is still being studied. Once I am finished, it will be re-written from scratch.  
    */
    public static class PasswordManagement
    {
        /// <summary>
        /// Creates a byte array containing the hashed combination of
        /// a GUID-generated salt value prepended to the password, hashed
        /// according to the provided algorithm.
        /// </summary>
        /// <param name="password">The plaintext password to be hashed</param>
        /// <param name="hashAlg">The HashAlgorithm to use in hashing the data</param>
        /// <returns>A byte array of the concatenated salt &
        /// hashed salt/password combination</returns>
        public static byte[] HashPassword(string password, HashAlgorithm hashAlg)
        {
            // Convert the password to a byte array
            byte[] passwordAsByteArray = UTF8Encoding.UTF8.GetBytes(password);

            // Generate a salt for the password
            byte[] saltAsByteArray = Guid.NewGuid().ToByteArray();

            // Hash the salt/password combination
            byte[] hashedPassAndSalt = HashPassword(
                saltAsByteArray, passwordAsByteArray, hashAlg);

            // Prepend the salt to the hashed data
            byte[] finalOutput = new byte[saltAsByteArray.Length + hashedPassAndSalt.Length];
            Array.Copy(saltAsByteArray, finalOutput, saltAsByteArray.Length);
            Array.Copy(hashedPassAndSalt, 0, finalOutput,
                saltAsByteArray.Length, hashedPassAndSalt.Length);

            return finalOutput;
        }

        /// <summary>
        /// Prepends the provided salt to the provided password using the provided
        /// HashAlgorithm
        /// </summary>
        /// <param name="salt">The salt to prepend</param>
        /// <param name="password">The password</param>
        /// <param name="hashAlg">The HashAlgorithm to use</param>
        /// <returns>A byte array containing the hashed salt/password combination.</returns>
        private static byte[] HashPassword(byte[] salt, byte[] password, HashAlgorithm hashAlg)
        {
            // Combine the salt and password into a single byte array
            byte[] passAndSaltForHashing = new byte[salt.Length + password.Length];
            Array.Copy(salt, passAndSaltForHashing, salt.Length);
            Array.Copy(password, 0, passAndSaltForHashing,
                salt.Length, password.Length);

            // Hash the salt/password combination
            return hashAlg.ComputeHash(passAndSaltForHashing);
        }

        /// <summary>
        /// Compares a plaintext password provided by the user to the
        /// password stored in a hashed salt/password byte array.
        /// </summary>
        /// <param name="password">The user-provided plaintext password for comparison</param>
        /// <param name="storedPassAndSalt">The stored hashed salt/password combination</param>
        /// <param name="hashAlg">The hash algorithm to use</param>
        /// <returns>True if the password provided matches the stored pass, otherwise
        /// false.</returns>
        public static bool ComparePassword(string password,
            byte[] storedPassAndSalt, HashAlgorithm hashAlg)
        {
            // Get salt from start of storedPassAndSalt
            int hashSize = hashAlg.HashSize / 8;

            // Deduce the size of the salt from the hash length
            int saltSize = storedPassAndSalt.Length - hashSize;

            // Extract salt from storedPassAndSalt
            byte[] salt = new byte[storedPassAndSalt.Length - hashSize];
            Array.Copy(storedPassAndSalt, salt, saltSize);

            // Extract hash from storedPassAndSalt
            byte[] hashedPasswordFromFile = new byte[storedPassAndSalt.Length - salt.Length];
            Array.Copy(storedPassAndSalt, salt.Length, hashedPasswordFromFile, 0, hashSize);

            // Using the salt extracted from the storeed password,
            // hash the password we received from the user.
            byte[] hashedPasswordFromUser = HashPassword(
                salt, UTF8Encoding.UTF8.GetBytes(password), hashAlg);

            // Compare the stored and provided hashes
            return hashedPasswordFromFile.SequenceEqual(hashedPasswordFromUser);
        }

        /// <summary>
        /// Create a human-readable hexadecimal string from the
        /// byte array by walking the array and converting each byte
        /// into a 2-digit hexadecimal value.
        /// </summary>
        /// <param name="data">The byte array to make human-readable</param>
        /// <returns>The human-readable string</returns>
        public static string CreateTextString(byte[] data)
        {
            // Create a human-readable hexadecimal string from the
            // byte array by walking the array and converting each byte
            // into a 2-digit hexadecimal value.
            StringBuilder sb = new StringBuilder(data.Length * 2);
            for (int i = 0; i < data.Length; ++i)
            {
                sb.AppendFormat("{0:x2}", data[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Transform the provided human-readable hexadecimal string to
        /// an array of bytes.
        /// </summary>
        /// <param name="data">The string to transform</param>
        /// <returns>The byte array representation of the hexadecimal string</returns>
        public static byte[] CreateByteArray(string data)
        {
            // Since each byte is represented by a 2-digit hex number,
            // we know that the length of the resulting byte array is
            // half the length of the passed-in data.
            byte[] binData = new byte[data.Length / 2];
            for (int i = 0; i < data.Length; i += 2)
            {
                binData[i / 2] = Convert.ToByte(data.Substring(i, 2), 16);
            }

            return binData;
        }
    }

    public static class FileManagement
    {
        public static bool CheckFileExists(string fileName, string subFolder)
        {
            bool reportFileExists = false;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, subFolder, fileName);
            reportFileExists = File.Exists(path);

            return reportFileExists;
        }

        public static bool CreateNewConfig()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string folder = @"config\";
            string configName = "users.cfg";
            // #\d lines are to call when a section of the config ends.
            string[] emptyContentStructure = { "Users:", "", "#1", "Passwords:", "", "#2", "Flags:", "", "#3" };
            bool configFolderCreated = false;

            // Check to see if the config folder exists. !FIX!
            bool configFolderExists = Directory.Exists(path + folder);

            // Handle folder creation and return a value depending on what happens.
            if (configFolderExists == false)
            {
                configFolderCreated = true;
                Directory.CreateDirectory(path + folder);
            }
            else
            {
                configFolderCreated = false;
            }

            // Write to our new config file using the emptyContentStructure variable.
            System.IO.File.WriteAllLines(path + folder + configName, emptyContentStructure);

            return configFolderCreated;
        }

        public static bool UserExistsConfig(string userName)
        {
            bool userExists = false;
            string configFile = AppDomain.CurrentDomain.BaseDirectory + @"config\users.cfg";
            using (StreamReader userRead = new StreamReader(configFile))
            {
                string currentLine = userRead.ReadLine();
                string nextLine = userRead.ReadLine();

                while (currentLine != "Users:")
                {
                    currentLine = nextLine;
                    nextLine = userRead.ReadLine();
                }

                while (currentLine != "")
                {
                    if (currentLine == userName)
                    {
                        userExists = true;
                    }
                    currentLine = nextLine;
                    nextLine = userRead.ReadLine();
                }

                if (nextLine == "#1")
                {
                    userRead.Close();
                }

                return userExists;
            }
        }

        public static void NewUserCreate(string userName, string userPassword)
        {
            int userAdded = 0;
            int i = 0;
            string configFile = AppDomain.CurrentDomain.BaseDirectory + @"config\users.cfg";
            List<string> configContent = File.ReadAllLines(configFile).ToList();

            while (userAdded != 1)
            {
                if (configContent[i] == "" && configContent[i + 1] == "#1")
                {
                    configContent[i] = userName + "\r\n";
                    userAdded = 1;
                }
                i++;

                if (i > configContent.Count)
                {
                    Prompt.ShowDialog("Failed to add new user to the config (NewUserCreate).", "ERROR 0001");
                    break;
                }
            }

            if (userAdded == 1)
            {
                // Each config block has a footer which equals 1 lines. Each block is the same length.
                // Multiply userName position by the config block number, and add 1 for foooter to get next data point.
                configContent[i * 2] = userPassword;
                configContent[i * 2 + 1] = "\r\n" + configContent[i * 2 + 1];
            }

            // Write the altered config file.
            System.IO.File.WriteAllLines(configFile, configContent);
        }

        public static bool CheckUserLogin(string userName, string userPass)
        {
            bool validLogin = false;

            int i = 0;
            MD5 md5Hash = MD5.Create();
            byte[] finalPass = null;
            string configFile = AppDomain.CurrentDomain.BaseDirectory + @"config\users.cfg";
            List<string> configContent = File.ReadAllLines(configFile).ToList();

            while (i < configContent.Count)
            {
                if (configContent[i] == userName)
                {
                    // Get the index of "Passwords:" and add the index of username to receive password.
                    finalPass = Encoding.Default.GetBytes(configContent[configContent.IndexOf("Passwords:") + i]);
                    break;
                }
                i++;
            }

            validLogin = PasswordManagement.ComparePassword(userPass, finalPass, md5Hash);

            return validLogin;

        }

        public static string fileTABorCSV(string dataFilePath)
        {
            string finalResult = null;
            int resultIsFileCSV = 0;
            int resultIsFileTAB = 0;

            // Set up the header record into a string.
            StreamReader dataFileReader = new StreamReader(dataFilePath);
            string dataFileContent = dataFileReader.ReadLine();

            // Set up the list of integers to store count of commas and tabs.
            List<int> fileTypeChooser = new List<int>();
            for (int i = dataFileContent.IndexOf(','); i > -1; i = dataFileContent.IndexOf(',', i + 1))
            {
                // If no more of the character is found, then -1 will return and end it.
                fileTypeChooser.Add(i);
            }

            // Store the CSV count into a variable for later.
            if (fileTypeChooser != null)
            {
                resultIsFileCSV = fileTypeChooser.Count;
            }
            fileTypeChooser.Clear();

            // Determine if the file is tab delimited now.
            for (int i = dataFileContent.IndexOf('\t'); i > -1; i = dataFileContent.IndexOf('\t', i + 1))
            {
                // If no more of the character is found, then -1 will return and end it.
                fileTypeChooser.Add(i);
            }

            // Store the TAB count into a variable for soon.
            if (fileTypeChooser != null)
            {
                resultIsFileTAB = fileTypeChooser.Count;
            }

            if (resultIsFileTAB > resultIsFileCSV)
            {
                finalResult = "TAB";
            }

            else if (resultIsFileCSV > resultIsFileTAB)
            {
                finalResult = "CSV";
            }

            else if (resultIsFileTAB + resultIsFileCSV == 0)
            {
                finalResult = "UNKNOWN";
            }

            dataFileReader.Close();

            return finalResult;
        }

    }

    public class ConversionTable
    {
        public string[] GetTableList(string tablesPath)
        {
            string[] tableFiles = Directory.GetFiles(tablesPath);
            return tableFiles;
        }
    }

    public class DataViewerControls
    {
        public static DataTable DataFileVariableGrid(int numberOfColumns, string[] headerRecord)
        {
            // Define a new data grid view to build.
            DataTable dataVariableGrid = new DataTable();
            for (int i = 0; i < numberOfColumns; i++)
            {
                try { dataVariableGrid.Columns.Add(headerRecord[i]); }
                // This is a serious exception and needs to be handled more gracefully. !FIX!
                catch (DuplicateNameException) { MessageBox.Show("Your table header contains duplicate column names.", "Header Error!"); break; }
            }
            return dataVariableGrid;
        }
    }

    static class DataGridViewExtensioncs
    {

        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            var dgvType = dgv.GetType();
            var pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }

    static class ConversionUtilities
    {
        public static bool StartConversion(string dataFilePath, string dataClientCode, string dataFileFormat)
        {
            // Replace conversionFolder with a config file. !FIX!
            const string conversionFolder = @"\\engagests1\Elements\Prospect Jobs\Conversions\01-File Conversions\Redpoint Finder\Downloaded\";

            // Move the data file to the conversion folder and change the name to Client Code + Extension.
            try
            {
                File.Copy(dataFilePath, conversionFolder + dataClientCode + dataFileFormat);
                return true;
            }
            catch (IOException)
            {
                MessageBox.Show("There is already a file in the conversion folder, or the file you selected was not found.", "File Error");
                return false;
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Either you do not have a file selected, or a client table isn't selected.", "File Error");
                return false;
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Your filename contains invalid characters.", "Name Error");
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("You do not have access to move this file into conversion.", "Permissions Error");
                return false;
            }
        }

    }

}