using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainWindow
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitialSetup.Start();
        }

        private void readConfig_Click(object sender, EventArgs e)
        {
            // Set up the data grid view.
            dataGridViewGeneral.ColumnCount = 1;
            dataGridViewGeneral.Columns[0].HeaderCell.Value = "Config";

            // Read the config file into a list line by line.
            string configFile = AppDomain.CurrentDomain.BaseDirectory + @"config\users.cfg";
            List<string> configLines = new List<string>();
            using (StreamReader configRead = new StreamReader(configFile))
            {
                while (configRead.Peek() > -1)
                {
                    dataGridViewGeneral.Rows.Add(configRead.ReadLine());
                }
            }

        }
    }

    public class InitialSetup
    {
        public static void Start()
        {
            // Log in to the application.
            string userName = UserName();
            byte[] finalPassword = Password();

            // Create config file if it does not exist.
            string configPath = @"config\";
            string configName = "users.cfg";
            bool configExists = FileManagement.CheckFileExists(configName, configPath);
            if (configExists == false)
            {
                FileManagement.CreateNewConfig();
            }

            // Check for user in the config file.


            // Program setup is now complete.

        }

        public static string UserName()
        {
            string userName = null;
            string userFormCaption = "Login";
            string userFormText = "Welcome to the file conversion interface." + '\n' + "Please enter your first and last name.";
            userName = Prompt.ShowDialog(userFormText, userFormCaption);
            return userName;
        }

        public static byte[] Password()
        {
            // Hash the password.
            string userPassword = null;
            MD5 md5Hash = MD5.Create();
            string userFormCaption = "Password";
            string userFormText = "Please enter your password.";
            userPassword = Prompt.ShowDialog(userFormText, userFormCaption);
            byte[] finalPassword = PasswordManagement.HashPassword(userPassword, md5Hash);
            return finalPassword;
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

                while (currentLine != "Users")
                {
                    currentLine = nextLine;
                    nextLine = userRead.ReadLine();
                }

                while (currentLine != null)
                {
                    if (currentLine == userName)
                    {
                        userExists = true;
                    }
                    currentLine = nextLine;
                    nextLine = userRead.ReadLine();
                }

                if (currentLine == "#1")
                {
                    userRead.Close();
                }

                return userExists;
            }
        }


    }


}