using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace Wpf_пробное_страницы
{
    class ThreatList
    {
        public class Threat
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Source { get; set; }
            public string ImpactObj { get; set; }
            public bool Confidentiality { get; set; }
            public bool Integrity { get; set; }
            public bool Availability { get; set; }
        }
        /*public class ThreatShort
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }*/

        public IList<Threat> GetData()
        {
            if (!File.Exists(@".\thrlist.xlsx"))
            {
                if (MessageBox.Show("На компьютере нет файла с базой данных угроз безопасности информации! Скачать файл?", "В противном случае приложение закроется.", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFile(@"https://bdu.fstec.ru/files/documents/thrlist.xlsx", @".\thrlist.xlsx");
                    }
                }
            }
            //Create COM Objects. Create a COM object for everything that is referenced
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbooks xlWorkbookS = xlApp.Workbooks;
            string path = Directory.GetCurrentDirectory() + @"\thrlist.xlsx";
            Excel.Workbook xlWorkbook = xlWorkbookS.Open(path);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            IList<Threat> result = new List<Threat>();

            int rowCount = xlRange.Rows.Count;

            for (int i = 3; i <= rowCount; i++)
            {
                Threat threat = new Threat();
                //ThreatShort threatShort = new ThreatShort();
                int id;
                bool isint = Int32.TryParse(xlRange.Rows[i].Cells[1].Value2.ToString(), out id);
                if (isint)
                {
                    if (id < 10)
                    {
                        threat.Id = "УБИ.00" + xlRange.Rows[i].Cells[1].Value2.ToString();
                    }
                    else if (id < 100)
                    {
                        threat.Id = "УБИ.0" + xlRange.Rows[i].Cells[1].Value2.ToString();
                    }
                    else threat.Id = "УБИ." + xlRange.Rows[i].Cells[1].Value2.ToString();
                }
                else threat.Id = xlRange.Rows[i].Cells[1].Value2.ToString();
                threat.Name = xlRange.Rows[i].Cells[2].Value2.ToString();
                threat.Description = xlRange.Rows[i].Cells[3].Value2.ToString();
                threat.Source = xlRange.Rows[i].Cells[4].Value2.ToString();
                threat.ImpactObj = xlRange.Rows[i].Cells[5].Value2.ToString();
                if (xlRange.Rows[i].Cells[6].Value2.ToString() == "1")
                {
                    threat.Confidentiality = true;
                }
                else threat.Confidentiality = false;
                if (xlRange.Rows[i].Cells[7].Value2.ToString() == "1")
                {
                    threat.Integrity = true;
                }
                else threat.Integrity = false;
                if (xlRange.Rows[i].Cells[8].Value2.ToString() == "1")
                {
                    threat.Availability = true;
                }
                else threat.Availability = false;
                result.Add(threat);
            }

            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlWorkbookS);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);

            return result;
        }
        public IList<Threat> GetNewData()
        {
            using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(@"https://bdu.fstec.ru/files/documents/thrlist.xlsx", @".\thrlist.xlsx");
                }
            //Create COM Objects. Create a COM object for everything that is referenced
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbooks xlWorkbookS = xlApp.Workbooks;
            string path = Directory.GetCurrentDirectory() + @"\thrlist.xlsx";
            Excel.Workbook xlWorkbook = xlWorkbookS.Open(path);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            IList<Threat> result = new List<Threat>();

            int rowCount = xlRange.Rows.Count;

            for (int i = 3; i <= rowCount; i++)
            {
                Threat threat = new Threat();
                //ThreatShort threatShort = new ThreatShort();
                int id;
                bool isint = Int32.TryParse(xlRange.Rows[i].Cells[1].Value2.ToString(), out id);
                if (isint)
                {
                    if (id < 10)
                    {
                        threat.Id = "УБИ.00" + xlRange.Rows[i].Cells[1].Value2.ToString();
                    }
                    else if (id < 100)
                    {
                        threat.Id = "УБИ.0" + xlRange.Rows[i].Cells[1].Value2.ToString();
                    }
                    else threat.Id = "УБИ." + xlRange.Rows[i].Cells[1].Value2.ToString();
                }
                else threat.Id = xlRange.Rows[i].Cells[1].Value2.ToString();
                threat.Name = xlRange.Rows[i].Cells[2].Value2.ToString();
                threat.Description = xlRange.Rows[i].Cells[3].Value2.ToString();
                threat.Source = xlRange.Rows[i].Cells[4].Value2.ToString();
                threat.ImpactObj = xlRange.Rows[i].Cells[5].Value2.ToString();
                if (xlRange.Rows[i].Cells[6].Value2.ToString() == "1")
                {
                    threat.Confidentiality = true;
                }
                else threat.Confidentiality = false;
                if (xlRange.Rows[i].Cells[7].Value2.ToString() == "1")
                {
                    threat.Integrity = true;
                }
                else threat.Integrity = false;
                if (xlRange.Rows[i].Cells[8].Value2.ToString() == "1")
                {
                    threat.Availability = true;
                }
                else threat.Availability = false;
                result.Add(threat);
            }

            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlWorkbookS);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);

            return result;
        }
    }
}