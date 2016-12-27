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
            try
            {
                // Request validation to continue from Entry.
                bool continueStep = Entry();
                if (continueStep == true)
                {
                    // This feels lazy. !FIX!
                    string dataFileFullName = dataFileName + dataFileFormat;

                    // Open a new Excel application.
                    Microsoft.Office.Interop.Excel.Application instanceExcel = new Microsoft.Office.Interop.Excel.Application();

                    // Run silently.
                    instanceExcel.DisplayAlerts = false;

                    // Load in the selected file as a new workbook in Excel.
                    Microsoft.Office.Interop.Excel.Workbook instanceWorkBook = instanceExcel.Workbooks.Open(dataFilePath + dataFileFullName);
                    instanceWorkBook.SaveAs(dataFilePath + dataFileName + ".csv", Microsoft.Office.Interop.Excel.XlFileFormat.xlCSV);

                    // Close the WorkBook and then close Excel. Return the bool if successful.
                    instanceWorkBook.Close();
                    instanceExcel.Quit();
                    return true;
                }
                // End try statement.
                // Send a false bool if the continueStep conditional was false.
                else
                {
                    return false;
                }
            }
            // Expand these exceptions !FIX!.
            catch (Exception ex)
            {
                MessageBox.Show("The Excel conversion failed between validation and actual conversion.\r\n" + ex.ToString(), "TablesToCSV Method");
                // Return false because our code failed by exception.
                return false;
            }
            // End TablesToCSV method.
        }
        // End ConversionTools class.
    }
    // End ExcelConvert namespace.
}
