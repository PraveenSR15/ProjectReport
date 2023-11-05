﻿using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace Services
{
    public static class WriteService
    {
    }

    public class ExcelFile
    {
        private string excelFilePath = string.Empty;
        private int rowNumber = 1; // define first row number to enter data in excel

        private Excel.Application myExcelApplication;
        private Excel.Workbook myExcelWorkbook;
        private Excel.Worksheet myExcelWorkSheet;

        public string ExcelFilePath
        {
            get { return excelFilePath; }
            set { excelFilePath = value; }
        }

        public int Rownumber
        {
            get { return rowNumber; }
            set { rowNumber = value; }
        }

        public void openExcel()
        {
            myExcelApplication = null;

            myExcelApplication = new Excel.Application(); // create Excell App
            myExcelApplication.DisplayAlerts = false; // turn off alerts

            myExcelWorkbook = myExcelApplication.Workbooks._Open(excelFilePath, Missing.Value,
               Missing.Value, Missing.Value, Missing.Value,
               Missing.Value, Missing.Value, Missing.Value,
               Missing.Value, Missing.Value, Missing.Value,
               Missing.Value, Missing.Value); // open the existing excel file

            int numberOfWorkbooks = myExcelApplication.Workbooks.Count; // get number of workbooks (optional)

            myExcelWorkSheet = (Excel.Worksheet)myExcelWorkbook.Worksheets[1]; // define in which worksheet, do you want to add data
            myExcelWorkSheet.Name = "WorkSheet 1"; // define a name for the worksheet (optinal)

            int numberOfSheets = myExcelWorkbook.Worksheets.Count; // get number of worksheets (optional)
        }

        public void addDataToExcel(string firstname, string lastname, string language, string email, string company)
        {
            myExcelWorkSheet.Cells[rowNumber, "H"] = firstname;
            myExcelWorkSheet.Cells[rowNumber, "J"] = lastname;
            myExcelWorkSheet.Cells[rowNumber, "Q"] = language;
            myExcelWorkSheet.Cells[rowNumber, "BH"] = email;
            myExcelWorkSheet.Cells[rowNumber, "CH"] = company;
            rowNumber++;  // if you put this method inside a loop, you should increase rownumber by one or wat ever is your logic
        }

        public void closeExcel()
        {
            try
            {
                myExcelWorkbook.SaveAs(excelFilePath, Missing.Value, Missing.Value, Missing.Value,
                                               Missing.Value, Missing.Value, Excel.XlSaveAsAccessMode.xlNoChange,
                                               Missing.Value, Missing.Value, Missing.Value,
                                               Missing.Value, Missing.Value); // Save data in excel

                myExcelWorkbook.Close(true, excelFilePath, Missing.Value); // close the worksheet
            }
            finally
            {
                if (myExcelApplication != null)
                {
                    myExcelApplication.Quit(); // close the excel application
                }
            }
        }
    }
}