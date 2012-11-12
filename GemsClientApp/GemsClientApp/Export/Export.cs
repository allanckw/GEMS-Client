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
                AdminHelper client = new AdminHelper();

                PrintToCell(sheetEvent, 2, 2, client.GetUserName(data.evnts.Organizerid));

                client.Close();

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

                    int facicount = 0;

                    foreach (FacilityBookingConfirmed[] fbc in data.facilities)
                    {
                        facicount = facicount + fbc.Count();
                    }


                    if (data.facilities != null && facicount != 0)
                    {
                        int col = 1;
                        for (int i = 0; i < data.days.Count(); i++)
                        {
                            PrintToCell(sheetFacility, col, 2, "Date:");
                            PrintToCell(sheetFacility, col + 1, 2, data.days[i].StartDateTime.Date.ToShortDateString());

                            Excel.Range range2;

                            Excel.Range r12 = sheetFacility.Cells[2, col];
                            Excel.Range r22 = sheetFacility.Cells[2, col + 1];
                            range2 = sheetFacility.get_Range(r12.Address, r22.Address);
                            range2.Font.Bold = true;


                            int row = 3;

                            if (data.programs[i].Count() == 0)
                            {
                                PrintToCell(sheetFacility, col, row, "No Programs Yet");
                            }

                            for (int j = 0; j < data.facilities[i].Count(); j++)
                            {
                                PrintToCell(sheetFacility, col, row, "StartTime");
                                PrintToCell(sheetFacility, col, row + 1, "EndTime");
                                PrintToCell(sheetFacility, col, row + 2, "Venue");
                                PrintToCell(sheetFacility, col, row + 3, "Purpose");
                                PrintToCell(sheetFacility, col, row + 4, "Remark");

                            

                                PrintToCell(sheetFacility, col + 1, row, data.facilities[i][j].RequestStartDateTime.ToShortTimeString());
                                PrintToCell(sheetFacility, col + 1, row + 1, data.facilities[i][j].RequestEndDateTime.ToShortTimeString());
                                PrintToCell(sheetFacility, col + 1, row + 2, data.facilities[i][j].Venue);
                                PrintToCell(sheetFacility, col + 1, row + 3, data.facilities[i][j].Purpose);
                                PrintToCell(sheetFacility, col + 1, row + 4, data.facilities[i][j].Remarks);

                                Excel.Range range;

                                Excel.Range r1 = sheetFacility.Cells[1, col];
                                Excel.Range r2 = sheetFacility.Cells[row + 6, col];
                                range = sheetFacility.get_Range(r1.Address, r2.Address);
                                range.Font.Bold = true;

                                row = row + 6;
                            }

                            col = col + 3;

                            BoldCellsRange(sheetFacility, "A", 1, "A", 1);

                        }


                        //PrintToCell(sheetFacility, 1, 1, "StartTime");
                        //PrintToCell(sheetFacility, 2, 1, "EndTime");
                        //PrintToCell(sheetFacility, 3, 1, "Venue");
                        //PrintToCell(sheetFacility, 4, 1, "Purpose");
                        //PrintToCell(sheetFacility, 5, 1, "Remark");

                        //BoldCellsRange(sheetFacility, "A", 1, "E", 1);

                        //int row = 2;
                        //for (int i = 0; i < data.facilities.Count(); i++)
                        //{
                        //    PrintToCell(sheetFacility, 1, row, "Date:");

                        //    PrintToCell(sheetFacility, 2, row, data.days[i].StartDateTime.Date.ToShortDateString());
                        //    row++;
                        //    PrintToCell(sheetFacility, 1, row, "Day");

                        //    PrintToCell(sheetFacility, 2, row, data.days[i].DayNumber.ToString());
                        //    row++;

                        //    if (data.facilities[i].Count() == 0)
                        //    {
                        //        PrintToCell(sheetFacility, 1, row, "There are no comfirmed booking facilities on this day");
                        //        row++;
                        //    }
                        //    for (int j = 0; j < data.facilities[i].Count(); j++)
                        //    {
                        //        PrintToCell(sheetFacility, 1, row, data.facilities[i][j].RequestStartDateTime.ToShortTimeString());
                        //        PrintToCell(sheetFacility, 2, row, data.facilities[i][j].RequestEndDateTime.ToShortTimeString());
                        //        PrintToCell(sheetFacility, 3, row, data.facilities[i][j].Venue);
                        //        PrintToCell(sheetFacility, 4, row, data.facilities[i][j].Purpose);
                        //        PrintToCell(sheetFacility, 5, row, data.facilities[i][j].Remarks);
                        //        row++;
                        //    }
                        //}

                    }
                    else
                    {
                        PrintToCell(sheetFacility, 1, 1, "No Facilities Booking Comfirmed");
                    }
                }

                if (NeedPrograms)
                {
                    sheetProgram = (Excel._Worksheet)(excelWorkbook.Sheets[sheetcount]);
                    sheetcount++;

                    int progcount = 0;

                    foreach (Program[] ps in data.programs)
                    {
                        progcount = progcount + ps.Count();
                    }

                    sheetProgram.Name = "Programmes";
                    if (data.programs != null && progcount != 0)
                    {

                        int col = 1;
                        for(int i=0;i< data.days.Count();i++)
                        {
                            PrintToCell(sheetProgram, col, 2, "Date:");
                            PrintToCell(sheetProgram, col+1, 2, data.days[i].StartDateTime.Date.ToShortDateString());

                            Excel.Range range2;

                            Excel.Range r12 = sheetProgram.Cells[2, col];
                            Excel.Range r22 = sheetProgram.Cells[2, col+1];
                            range2 = sheetProgram.get_Range(r12.Address, r22.Address);
                            range2.Font.Bold = true;


                            int row = 3;

                            if (data.programs[i].Count() == 0)
                            {
                                PrintToCell(sheetProgram, col, row, "No Programs Yet");
                            }

                            for(int j=0;j< data.programs[i].Count();j++)
                            {
                                PrintToCell(sheetProgram, col, row, "StartTime");
                                PrintToCell(sheetProgram, col, row+1, "EndTime");
                                PrintToCell(sheetProgram, col, row+2, "Name");
                                PrintToCell(sheetProgram, col, row+3, "Description");
                                PrintToCell(sheetProgram, col, row+4, "Venue");

                                PrintToCell(sheetProgram, col+1, row, data.programs[i][j].StartDateTime.ToShortTimeString());
                                PrintToCell(sheetProgram, col + 1, row + 1, data.programs[i][j].EndDateTime.ToShortTimeString());
                                PrintToCell(sheetProgram, col+1, row + 2, data.programs[i][j].Name);
                                PrintToCell(sheetProgram, col + 1, row + 3, data.programs[i][j].Description);
                                PrintToCell(sheetProgram, col + 1, row + 4, data.programs[i][j].Location);

                                Excel.Range range;

                                Excel.Range r1 = sheetProgram.Cells[1, col];
                                Excel.Range r2 = sheetProgram.Cells[row + 6, col];
                                range = sheetProgram.get_Range(r1.Address,r2.Address);
                                range.Font.Bold = true;

                                row = row + 6;
                            }

                            col = col + 3;

                            BoldCellsRange(sheetProgram, "A", 1, "A", 1);

                        }



                        //PrintToCell(sheetProgram, 1, 1, "StartTime");
                        //PrintToCell(sheetProgram, 2, 1, "EndTime");
                        //PrintToCell(sheetProgram, 3, 1, "Name");
                        //PrintToCell(sheetProgram, 4, 1, "Location");
                        //PrintToCell(sheetProgram, 5, 1, "Description");

                        //BoldCellsRange(sheetProgram, "A", 1, "E", 1);

                        //int row = 2;
                        //for (int i = 0; i < data.programs.Count(); i++)
                        //{
                        //    PrintToCell(sheetProgram, 1, row, "Date:");

                        //    PrintToCell(sheetProgram, 2, row, data.days[i].StartDateTime.Date.ToShortDateString());
                        //    row++;
                        //    PrintToCell(sheetProgram, 1, row, "Day");

                        //    PrintToCell(sheetProgram, 2, row, data.days[i].DayNumber.ToString());
                        //    row++;

                        //    if (data.programs[i].Count() == 0)
                        //    {
                        //        PrintToCell(sheetProgram, 1, row, "There are no Programs Added on this day");
                        //        row++;
                        //    }
                        //    for (int j = 0; j < data.programs[i].Count(); j++)
                        //    {
                        //        PrintToCell(sheetProgram, 1, row, data.programs[i][j].StartDateTime.ToShortTimeString());
                        //        PrintToCell(sheetProgram, 2, row, data.programs[i][j].EndDateTime.ToShortTimeString());
                        //        PrintToCell(sheetProgram, 3, row, data.programs[i][j].Name);
                        //        PrintToCell(sheetProgram, 4, row, data.programs[i][j].Location);
                        //        PrintToCell(sheetProgram, 5, row, data.programs[i][j].Description);


                        //        row++;
                        //    }


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
                        PrintToCell(sheetIncome, 1, 1, "Name");
                        PrintToCell(sheetIncome, 2, 1, "Source");
                        PrintToCell(sheetIncome, 3, 1, "Date Received");
                        PrintToCell(sheetIncome, 4, 1, "GST Liable");
                        PrintToCell(sheetIncome, 5, 1, "Before GST");
                        PrintToCell(sheetIncome, 6, 1, "GST Value");
                        PrintToCell(sheetIncome, 7, 1, "After GST");

                        BoldCellsRange(sheetProgram, "A", 1, "G", 1);


                        decimal total = 0;
                        int row = 2;
                        for (int i = 0; i < data.budgetincomes.Count(); i++)
                        {
                            PrintToCell(sheetIncome, 1, row, data.budgetincomes[i].Name);
                            PrintToCell(sheetIncome, 2, row, data.budgetincomes[i].Source);
                            PrintToCell(sheetIncome, 3, row, data.budgetincomes[i].DateReceived.ToShortDateString());
                            PrintToCell(sheetIncome, 4, row, data.budgetincomes[i].IsGstLiable.ToString());
                            PrintToCell(sheetIncome, 5, row, data.budgetincomes[i].IncomeBeforeGST.ToString());
                            PrintToCell(sheetIncome, 6, row, data.budgetincomes[i].GstValue.ToString());
                            PrintToCell(sheetIncome, 7, row, data.budgetincomes[i].IncomeAfterGST.ToString());
                            total = data.budgetincomes[i].IncomeAfterGST + total;
                            row++;
                        }
                        PrintToCell(sheetIncome, 6, row, "total:");
                        PrintToCell(sheetIncome, 7, row, total.ToString());


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


                        PrintToCell(sheetOptItem, 1, 1, "Generated Date:");
                        PrintToCell(sheetOptItem, 2, 1, data.optitems.GeneratedDate.ToString());

                        PrintToCell(sheetOptItem, 1, 2, "Total Estmiated Price:");
                        PrintToCell(sheetOptItem, 2, 2, data.optitems.TotalEstimatedPrice.ToString());

                        PrintToCell(sheetOptItem, 1, 3, "Total Satisfaction:");
                        PrintToCell(sheetOptItem, 2, 3, data.optitems.TotalSatisfaction.ToString());

                        PrintToCell(sheetOptItem, 1, 4, "Name");
                        PrintToCell(sheetOptItem, 2, 4, "Type");
                        PrintToCell(sheetOptItem, 3, 4, "Is Bought");
                        BoldCellsRange(sheetProgram, "A", 4, "C", 4);



                        int row = 5;
                        for (int i = 0; i < data.optitems.BudgetItemsList.Count(); i++)
                        {
                            PrintToCell(sheetOptItem, 1, row, data.optitems.BudgetItemsList[i].ItemName);
                            PrintToCell(sheetOptItem, 2, row, data.optitems.BudgetItemsList[i].typeString);
                            PrintToCell(sheetOptItem, 3, row, data.optitems.BudgetItemsList[i].IsBought.ToString());
                        }

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
                        PrintToCell(sheetTask, 1, 1, "name");
                        PrintToCell(sheetTask, 2, 1, "Description");
                        PrintToCell(sheetTask, 3, 1, "DueDate");
                        PrintToCell(sheetTask, 4, 1, "Completion(%)");
                        int row = 2;

                        for (int i = 0; i < data.tasks.Count(); i++)
                        {
                            PrintToCell(sheetTask, 1, row, data.tasks[i].TaskName);
                            PrintToCell(sheetTask, 2, row, data.tasks[i].TaskDesc);
                            PrintToCell(sheetTask, 3, row, data.tasks[i].DueDate.ToString());
                            PrintToCell(sheetTask, 4, row, data.tasks[i].PercentageCompletion.ToString() + "%");
                            row++;
                            for (int j = 0; j < data.tasks[i].TasksAssignments.Count(); j++)
                            {
                                TaskAssignment t = data.tasks[i].TasksAssignments[j];

                                PrintToCell(sheetTask, 1, row, t.RoleName);//shld be role name
                                PrintToCell(sheetTask, 2, row, t.RoleUserID);//shld be role assigned name
                                PrintToCell(sheetTask, 3, row, t.Remarks);
                                PrintToCell(sheetTask, 4, row, t.IsRead.ToString());
                                PrintToCell(sheetTask, 5, row, t.IsCompleted.ToString());

                            }


                        }

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

                    int guestcount = 0;

                    foreach (Guest[] gs in data.guests)
                    {
                        guestcount = guestcount + gs.Count();
                    }

                    sheetGuest.Name = "Guest";
                    if (data.guests != null && guestcount != 0)
                    {
                        int col = 1;
                        for (int i = 0; i < data.days.Count(); i++)
                        {
                            PrintToCell(sheetGuest, col, 2, "Date:");
                            PrintToCell(sheetGuest, col + 1, 2, data.days[i].StartDateTime.Date.ToShortDateString());

                            Excel.Range range2;

                            Excel.Range r12 = sheetGuest.Cells[2, col];
                            Excel.Range r22 = sheetGuest.Cells[2, col + 1];
                            range2 = sheetGuest.get_Range(r12.Address, r22.Address);
                            range2.Font.Bold = true;


                            int row = 3;

                            if (data.guests[i].Count() == 0)
                            {
                                PrintToCell(sheetGuest, col, row, "No Guests Yet");
                            }

                            for (int j = 0; j < data.guests[i].Count(); j++)
                            {
                                PrintToCell(sheetGuest, col, row, "Name");
                                PrintToCell(sheetGuest, col, row + 1, "Description");
                                PrintToCell(sheetGuest, col, row + 2, "Contact");
                            
                                PrintToCell(sheetGuest, col + 1, row, data.guests[i][j].Name);
                                PrintToCell(sheetGuest, col + 1, row + 1, data.guests[i][j].Description);
                                PrintToCell(sheetGuest, col + 1, row + 2, data.guests[i][j].Contact);
                               
                                Excel.Range range;

                                Excel.Range r1 = sheetGuest.Cells[1, col];
                                Excel.Range r2 = sheetGuest.Cells[row + 3, col];
                                range = sheetGuest.get_Range(r1.Address, r2.Address);
                                range.Font.Bold = true;

                                row = row + 4;
                            }

                            col = col + 3;

                            BoldCellsRange(sheetGuest, "A", 1, "A", 1);

                        }




                        //PrintToCell(sheetGuest, 1, 1, "Name");
                        //PrintToCell(sheetGuest, 2, 1, "Description");
                        //PrintToCell(sheetGuest, 3, 1, "Contact");

                        //BoldCellsRange(sheetGuest, "A", 1, "C", 1);

                        //int row = 2;
                        //for (int i = 0; i < data.programs.Count(); i++)
                        //{
                        //    PrintToCell(sheetGuest, 1, row, "Date:");

                        //    PrintToCell(sheetGuest, 2, row, data.days[i].StartDateTime.Date.ToShortDateString());
                        //    row++;
                        //    PrintToCell(sheetGuest, 1, row, "Day");

                        //    PrintToCell(sheetGuest, 2, row, data.days[i].DayNumber.ToString());
                        //    row++;

                        //    if (data.guests.Count() == 0)
                        //    {
                        //        PrintToCell(sheetGuest, 1, row, "There are no Guests Added on this day");
                        //        row++;
                        //    }
                        //    for (int j = 0; j < data.guests[i].Count(); j++)
                        //    {
                        //        PrintToCell(sheetGuest, 1, row, data.guests[i][j].Name);
                        //        PrintToCell(sheetGuest, 2, row, data.guests[i][j].Description);
                        //        PrintToCell(sheetGuest, 3, row, data.guests[i][j].Contact);

                        //        row++;
                        //    }
                        //}

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
                    if (data.participants != null && data.participants.Count() != 0 && data.field != null)
                    {

                        int col = 1;
                        for (int j = 0; j < data.field.Count(); j++)
                        {
                            PrintToCell(sheetParticipant, col, 1, data.field[j].FieldName);
                            col++;
                        }


                        int row = 2;
                        for (int i = 0; i < data.participants.Count(); i++)
                        {
                            for (int j = 0; j < data.field.Count(); j++)
                            {

                                for (int k = 0; k < data.participants[i].Answer.Count(); k++)
                                {
                                    if (data.participants[i].Answer[k].FieldID == data.field[j].FieldID)
                                    {
                                        PrintToCell(sheetParticipant, j + 1, row, data.participants[i].Answer[k].Answer);

                                        break;
                                    }

                                }
                            }
                            PrintToCell(sheetParticipant, 1, row, data.participants[i].ParticipantID.ToString());//
                            row++;
                        }

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
