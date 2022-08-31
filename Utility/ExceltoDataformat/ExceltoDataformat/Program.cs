using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExceltoJson
{
    class Program
    {
        public static string JsonText;
        public static string XmlText;
        [STAThread]
        static void Main(string[] args)
        {
            string readfilePath = "";
            string readfilename = System.IO.Directory.GetCurrentDirectory() + "\\table.xlsx";
            string outfilePath = "";

            
            if (args.Length > 2)
            {
                readfilePath = args[0];
                readfilename = readfilePath + args[1];
                outfilePath = args[2];
            }
            else
            {
                using(OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "C:\\";
                    openFileDialog.RestoreDirectory = true;
                    if(openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        readfilename = openFileDialog.FileName;
                    }
                }
            }
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    outfilePath = folderBrowserDialog.SelectedPath + "\\";
                }
            }

            Excel.Application excel = new Excel.Application();
            Workbook workbook = excel.Workbooks.Open(readfilename);
            Worksheet worksheet = null;
            Console.WriteLine(workbook.Name);
            for (int i = 1; i < workbook.Worksheets.Count+1; i++)
            {
                worksheet = workbook.Worksheets.Item[i];
                string worksheetname = worksheet.Name;
                if (worksheetname.Contains("Table"))
                {
                    string column = null;
                    string type = null;
                    string data = null;
                    List<string> columns = new List<string>();
                    List<string> datas = new List<string>();
                    Excel.Range range = worksheet.UsedRange;
                    // 데이터 스키마 가져옴
                    // xml 데이터 작성
                    for (int j = 1; j < range.Columns.Count + 1; j++)
                    {
                        if((range.Cells[1, j] as Excel.Range).Value2 != null)
                            column = (range.Cells[1, j] as Excel.Range).Value2.ToString();
                        if ((range.Cells[2, j] as Excel.Range).Value2 != null) 
                            type = (range.Cells[2, j] as Excel.Range).Value2.ToString();
                        if (column != null & type != null) 
                            XmlText += string.Format(StringFormat.XmlDataFormat, type, column);

                        columns.Add(column);
                        column = null;
                        type = null;
                    }
                    string jsondata = "";
                    // 데이터 레코드 가져옴
                    // json데이터 작성
                    for (int j = 3; j < range.Rows.Count + 1; j++)
                    {
                        if (j != 3)
                        {
                            JsonText += ",\n";
                        }
                        // 데이터 한줄
                        for (int k = 1; k < range.Columns.Count + 1; k++)
                        {
                            // 데이터 하나
                            if ((range.Cells[j, k] as Excel.Range).Value2 != null)
                            {
                                if (k != 1)
                                {
                                    jsondata += ", \n";
                                }
                                data = (range.Cells[j, k] as Excel.Range).Value2.ToString();
                                jsondata += string.Format(StringFormat.JsonDataFormat, columns[k - 1], data);
                            }
                                
                            datas.Add(data);
                        }
                        JsonText += string.Format(StringFormat.JsonDatasFormat, jsondata);
                        jsondata = "";
                    }

                    //데이터 완성
                    System.IO.File.WriteAllText(outfilePath + worksheetname + ".json", string.Format(StringFormat.JsonFormat,JsonText));
                    System.IO.File.WriteAllText(outfilePath + worksheetname + ".xml", string.Format(StringFormat.XmlFormat,worksheetname,XmlText));
                    JsonText = "";
                    XmlText = "";
                }
            }

            excel.Quit();
            //Marshal.ReleaseComObject();
        }

       
    }
}
