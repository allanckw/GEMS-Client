using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows;
using evmsService.entities;

namespace Gems.UIWPF
{
    class Exporter
    {
        public static void PrintToCell(Excel._Worksheet oSheet, int x, int y, string data)
        {
            oSheet.Cells[y, x] = data;
        }

        public static void BoldCellsRange(Excel._Worksheet oSheet, string startX, int startY, string endX, int endY)
        {
            Excel.Range range;
            range = oSheet.Cells.get_Range(startX + startY.ToString(), endX + endY.ToString());
            range.Font.Bold = true;
        }

        public static void PrintExcel(ExportData data, Boolean NeedFacilities, Boolean NeedPrograms, Boolean NeedIncome, Boolean NeedOptItems, Boolean NeedTasks
        , Boolean NeedGuest, Boolean NeedParticipant)
        {

            try
            {

                //myHashtable = excelprocess.CheckExcellProcesses(myHashtable);

                Excel.Application oXL;
                oXL = new Excel.Application();



                Excel.Workbook excelWorkbook = oXL.Workbooks.Add();


                //  oXL.Workbooks.Open(workbookPath, false, true, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);

                int needextra = 0;

                if (NeedFacilities)
                    needextra++;
                if (NeedPrograms)
                    needextra++;
                if (NeedIncome)
                    needextra++;
                if (NeedOptItems)
                    needextra++;
                if (NeedTasks)
                    needextra++;
                if (NeedGuest)
                    needextra++;
                if (NeedParticipant)
                    needextra++;
                //prep
                //Excel._Worksheet eventSheet = (Excel._Worksheet)(excelWorkbook.Sheets[1]);

                while (excelWorkbook.Sheets.Count != (needextra + 1))
                {
                    if (excelWorkbook.Sheets.Count < (needextra + 1))
                    {
                        excelWorkbook.Sheets.Add(excelWorkbook.Sheets[excelWorkbook.Sheets.Count]);
                        //++
                    }
                    else if (excelWorkbook.Sheets.Count > (needextra + 1))
                    {

                        ((Excel._Worksheet)(excelWorkbook.Sheets[excelWorkbook.Sheets.Count - 1])).Delete();
                        //--
                    }
                }
                Excel._Worksheet sheetEvent = null;
                Excel._Worksheet sheetFacility = null;
                Excel._Worksheet sheetProgram = null;
                Excel._Worksheet sheetIncome = null;
                Excel._Worksheet sheetOptItem = null;
                Excel._Worksheet sheetTask = null;
                Excel._Worksheet sheetGuest = null;
                Excel._Worksheet sheetParticipant = null;

                sheetEvent = (Excel._Worksheet)(excelWorkbook.Sheets[1]);
                int sheetcount = 2;

                //
                //populate event
                //
                sheetEvent.Activate();

                sheetEvent.Name = "Event";
                PrintToCell(sheetEvent, 1, 1, "EventName:");
                PrintToCell(sheetEvent, 2, 1, data.evnts.Name);

                PrintToCell(sheetEvent, 1, 2, "Organizer Name:");
                PrintToCell(sheetEvent, 2, 2, data.evnts.Organizer.Name);

                PrintToCell(sheetEvent, 1, 3, "Start:");
                PrintToCell(sheetEvent, 2, 3, data.evnts.StartDateTime.ToString());

                PrintToCell(sheetEvent, 1, 4, "End:");
                PrintToCell(sheetEvent, 2, 4, data.evnts.EndDateTime.ToString());

                PrintToCell(sheetEvent, 1, 5, "WebSite:");
                PrintToCell(sheetEvent, 2, 5, data.evnts.Website);

                PrintToCell(sheetEvent, 1, 6, "Description:");
                PrintToCell(sheetEvent, 2, 6, data.evnts.Description);

                PrintToCell(sheetEvent, 1, 7, "Tag:");
                PrintToCell(sheetEvent, 2, 7, data.evnts.Tag);


                BoldCellsRange(sheetEvent, "A", 1, "A", 7);

                EventDay[] days = data.days;

                //populate days

                //PrintToCell(sheetEvent, 1, 9, "Days:");
                //for (int i = 0; i < days.Count(); i++)
                //{

                //}




                sheetEvent.Columns.AutoFit();
                //sheetEvent.Rows.AutoFit();
                if (NeedFacilities)
                {
                    sheetFacility = (Excel._Worksheet)(excelWorkbook.Sheets[sheetcount]);
                    sheetcount++;
                    sheetFacility.Name = "Facilities";
                    if (data.facilities != null && data.facilities.Count() != 0)
                    {
                        PrintToCell(sheetEvent, 1, 1, "StartTime");
                        PrintToCell(sheetEvent, 2, 1, "EndTime");
                        PrintToCell(sheetEvent, 3, 1, "Venue");
                        PrintToCell(sheetEvent, 4, 1, "Purpose");
                        PrintToCell(sheetEvent, 5, 1, "Remark");

                        int row = 2;
                        for (int i = 0; i < data.facilities.Count(); i++)
                        {
                            PrintToCell(sheetEvent, 1, row, "Date:");
                            row++;
                            PrintToCell(sheetEvent, 1, row, data.days[i].StartDateTime.Date.ToShortDateString());
                            row++;
                            PrintToCell(sheetEvent, 1, row, "Day");
                            row++;
                            PrintToCell(sheetEvent, 1, row, data.days[i].DayNumber.ToString());
                            row++;

                            for (int j = 0; j < data.facilities[i].Count(); j++)
                            {
                                PrintToCell(sheetEvent, 1, row, data.facilities[i][j].RequestStartDateTime.ToShortTimeString());
                                PrintToCell(sheetEvent, 2, row, data.facilities[i][j].RequestEndDateTime.ToShortTimeString());
                                PrintToCell(sheetEvent, 3, row, data.facilities[i][j].Venue);
                                PrintToCell(sheetEvent, 4, row, data.facilities[i][j].Purpose);
                                PrintToCell(sheetEvent, 5, row, data.facilities[i][j].Remarks);
                            }
                        }

                    }
                    else
                    {
                        PrintToCell(sheetEvent, 1, 1, "No Facilities added");
                    }
                }

                if (NeedPrograms)
                {
                    sheetProgram = (Excel._Worksheet)(excelWorkbook.Sheets[sheetcount]);
                    sheetcount++;

                    sheetProgram.Name = "Programmes";
                    if (data.programs != null && data.programs.Count() != 0)
                    {


                    }
                    else
                    {
                        PrintToCell(sheetProgram, 1, 1, "No Program added");
                    }
                }
                if (NeedIncome)
                {
                    sheetIncome = (Excel._Worksheet)(excelWorkbook.Sheets[sheetcount]);
                    sheetcount++;

                    sheetIncome.Name = "Income";
                    if (data.budgetincomes != null && data.budgetincomes.Count() != 0)
                    {


                    }
                    else
                    {
                        PrintToCell(sheetIncome, 1, 1, "No Income added");
                    }
                }
                if (NeedOptItems)
                {
                    sheetOptItem = (Excel._Worksheet)(excelWorkbook.Sheets[sheetcount]);
                    sheetcount++;

                    sheetOptItem.Name = "OptimizedItems";
                    if (data.optitems != null && data.optitems.BudgetItemsList.Count() != 0)
                    {


                    }
                    else
                    {
                        PrintToCell(sheetOptItem, 1, 1, "No Optimized Items added");
                    }
                }
                if (NeedTasks)
                {
                    sheetTask = (Excel._Worksheet)(excelWorkbook.Sheets[sheetcount]);
                    sheetcount++;

                    sheetTask.Name = "Task";
                    if (data.tasks != null && data.tasks.Count() != 0)
                    {


                    }
                    else
                    {
                        PrintToCell(sheetTask, 1, 1, "No Task added");
                    }
                }
                if (NeedGuest)
                {
                    sheetGuest = (Excel._Worksheet)(excelWorkbook.Sheets[sheetcount]);
                    sheetcount++;

                    sheetGuest.Name = "Guest";
                    if (data.guests != null && data.guests.Count() != 0)
                    {


                    }
                    else
                    {
                        PrintToCell(sheetGuest, 1, 1, "No Guest added");
                    }
                }
                if (NeedParticipant)
                {
                    sheetParticipant = (Excel._Worksheet)(excelWorkbook.Sheets[sheetcount]);
                    sheetcount++;

                    sheetParticipant.Name = "Participant";
                    if (data.participants != null && data.participants.Count() != 0)
                    {


                    }
                    else
                    {
                        PrintToCell(sheetParticipant, 1, 1, "No Participant added");
                    }
                }


                //PrintToCell(oSheet, 1, 1, "1");
                //PrintToCell(oSheet, 1, 2, "1");
                //PrintToCell(oSheet, 1, 3, "1");
                //PrintToCell(oSheet, 1, 4, "1");

                //BoldCellsRange(oSheet,"A",1,"A",1);

                oXL.Visible = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                //excelprocess.KillExcel(myHashtable);
                //return;
            }
        }
    }
}
