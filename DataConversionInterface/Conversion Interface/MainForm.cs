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
        const string tablesPath = @"\\engagests1\Elements\Prospect Jobs\Conversions\01-File Conversions\Redpoint Finder\Downloaded\Tables\";
        const string statusPath = @"\\engagests1\Elements\Prospect Jobs\Conversions\01-File Conversions\Redpoint Finder\Downloaded\Layout\Status Files\";
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

            // Double buffer the data grid view to prevent flickering.
            dataGridViewGeneral.DoubleBuffered(true);
        }

        // Set up the table list and remove the definition file from an array of existing files.
        private void InitializeTableList()
        {
            ConversionTable displayTables = new ConversionTable();

            // Get a list of available tables and clean up path data for the combo box.
            string[] cleanedTablesContent = displayTables.GetTableList(tablesPath).Select(s => s.Replace(tablesPath, "")).ToArray();
            // Remove the house fields reference table.
            cleanedTablesContent = cleanedTablesContent.Where(s => s!= "HOUSE_FIELDS.txt").ToArray();
            conversionTablesList.DataSource = cleanedTablesContent;
        }

        // This event fires whenever a new lookup table is selected by the user.
        private void conversionTablesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Start up the grid view for this selected table.
            DataTable selectedTable = new DataTable();
            selectedTable.Columns.Add("CLIENT");
            selectedTable.Columns.Add("HOUSE");
            string tableLocation = tablesPath + conversionTablesList.SelectedValue.ToString();

            // Fill in the gridviewer rows with the new conversion table.
            StreamReader tableReader = new StreamReader(tableLocation);
            // Get rid of the header.
            tableReader.ReadLine();
            string[] tableContent = new string[File.ReadAllLines(tableLocation).Length];
            while (!tableReader.EndOfStream)
            {
                tableContent = tableReader.ReadLine().Split(',');
                selectedTable.Rows.Add(tableContent[0], tableContent[1]);
            }

            // Display the data table.
            dataGridViewTables.DataSource = selectedTable;
            tableReader.Close();
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
            // Ensure that we aren't loading an empty space.
            if (File.Exists(textBoxFileName.Text.ToString()))
            {
                if (textBoxFileName.Text.ToString() != "")
                {
                    // Set up the line count variable.
                    int displayTotalLines = 0;
                    // Set up variables to read the data file.
                    string dataFilePath = textBoxFileName.Text.ToString();
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
                    labelRecordCount.Text = lineCounter.ToString();

                    // Initialize a variable to determine line count for display.
                    if (linesViewAll == false) { displayTotalLines = linesViewCount; }
                    else { displayTotalLines = lineCounter; }
                    if ( displayTotalLines > lineCounter ) { displayTotalLines = lineCounter; }
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

                            dataFileContent.Add(regexMatch.ToString().TrimStart(finalDelimiterChar));
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
                    catch (NullReferenceException r)
                    {
                        MessageBox.Show("The file you are trying to open does not have enough columns.", "Not a table!");
                    }
                    catch (ArgumentException a)
                    {
                        MessageBox.Show("This program can't view that type of file yet.", "Bad file!");
                    }
                }

            }
            else
            {
                MessageBox.Show("The file you have selected doesA not exist.", "Missing File");
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
        }

        private void buttonStartConversion_Click(object sender, EventArgs e)
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

        // This timer handles the update of the progress bar and status message below it.
        private void timerConvertProgress_Tick(object sender, EventArgs e)
        {
            // The status messages file is 60 byte fixed width.
            // Check to see if file size has changed.
            using (FileStream redPointLogStream = File.Open(statusPath + "CurrentStatus.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
                            System.IO.File.WriteAllText(statusPath + "CurrentStatus.txt", string.Empty);
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
        }

        private void textBoxViewLines_TextChanged(object sender, EventArgs e)
        {
            try
            {
                linesViewCount = Convert.ToInt32(textBoxViewLines.Text.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("That is not a valid number of lines.", "Invalid Number");
                textBoxViewLines.Text = "1000";
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
            dataGridViewGeneral.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
    }

    public class InitialSetup
    {
        /* Issues to consider:
         * The login prompt can be brute forced by throwing the wrong answers and closing it.
         * The config file management isn't particularly elegant.
         * I still need to fully study up on how the salt is extracted from the hashed pass.
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
            if ( validLogin == false &&
                MessageBox.Show("Invalid login, try again?", "Login Attempt",
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
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
            userPassword = Prompt.ShowDialog(userFormText, userFormCaption);
            return userPassword;
        }
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
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;


            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }

    /* 
     This password hashing block is still being studied. Once I am finished, it will be re-written from scratch.  
     http://www.dreamincode.net/forums/topic/196519-basic-login-system-part-i-password-handling/
     I'm saving this link for personal study this weekend.
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

            // Check to see if the config folder exists.
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
                if (configContent[i] == "" && configContent[i+1] == "#1")
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
            for (int i = dataFileContent.IndexOf(','); i > -1; i = dataFileContent.IndexOf(',', i + 1) )
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
        public static DataTable DataFileVariableGrid(int numberOfColumns, string[]headerRecord)
        {
            // Define a new data grid view to build.
            DataTable dataVariableGrid = new DataTable();
            for (int i = 0; i < numberOfColumns; i++)
            {
                try { dataVariableGrid.Columns.Add(headerRecord[i]); }
                // This is a serious exception and needs to be handled more gracefully. !FIX!
                catch (DuplicateNameException ev) { MessageBox.Show("Your table header contains duplicate column names.", "Header Error!"); break; }
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
                File.Move(dataFilePath, conversionFolder + dataClientCode + dataFileFormat);
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

        public static bool ExcelConvert(string dataFilePath, string dataFileName, string dataFileFormat)
        {
            // Replace this constant with a config file at some point. !FIX!
            const string excelConvertPath = @"\\engagests1\Elements\Prospect Jobs\Conversions\01-File Conversions\Redpoint Finder\Excel Convert\";
            string dataFileFullName = dataFileName + dataFileFormat;

            // Now that we know the file path and name, it's time to execute a powershell script to run an excel conversion from DBF, XLS, or XLSX.
            try
            {
                using (PowerShell instanceExcelConvert = PowerShell.Create())
                {
                    // Move the original file into the conversion folder.
                    File.Move(dataFilePath + dataFileFullName, excelConvertPath + dataFileFullName);
                    instanceExcelConvert.AddScript(excelConvertPath + "Excel Convert.ps1");
                    instanceExcelConvert.Invoke();
                    // Move the original file back to the original folder.
                    File.Move(excelConvertPath + dataFileFullName, dataFilePath + dataFileFullName);
                    // Move the converted file back to original folder.
                    File.Move(excelConvertPath + dataFileName + ".csv", dataFilePath + dataFileName + ".csv");
                    return true;
                }
            }
            // This is a sloppy catch. Study up on the powershell class. !FIX!
            catch (Exception pshellException)
            {
                MessageBox.Show("The Excel Convert script failed because " + pshellException, "Powershell Error");
                return false;
            }
            
        }
    }

}