using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelConvert
{
   public class ConversionTools
    {
        public static bool Entry()
        {
            // The purpose of this method is to act as an entry-point for the Excel tools.
            // This enforces a safe way to make sure that Excel is installed on the user system.

            Type excelInstalled = Type.GetTypeFromProgID("Excel.Application");
            bool continueStep = false;

            if (excelInstalled == null)
            {
                // Report the issue.
                MessageBox.Show("This program is requesting a version of Excel which is not present on your system.", "Excel Missing!");
                continueStep = false;
            }
            else
            {
                // User system passed the test and can run these methods.
                continueStep = true;
            }
            return continueStep;
        }
        public static bool TablesToCSV(string dataFilePath, string dataFileName, string dataFileFormat)
        {
            // Request validation to continue from Entry.
            bool continueStep = Entry();
            if (continueStep == true)
            {
                // Replace this constant with a config file at some point. !FIX!
                string dataFileFullName = dataFileName + dataFileFormat;

                // Now that we know the file path and name, it's time to execute a powershell script to run an excel conversion from DBF, XLS, or XLSX.

            }
            return false;
            

        }
    }
}
