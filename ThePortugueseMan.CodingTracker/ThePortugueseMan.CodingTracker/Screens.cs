﻿using ConsoleTableExt;

namespace ThePortugueseMan.CodingTracker;

internal class Screens
{
    AskInput askInput = new();
    DbCommands dbCmds = new();
    ListOperations listOp = new();
    public void MainMenu()
    {
        bool exitMenu = false;
        List<string> optionsString = new List<string> { 
            "1 - View Logs", 
            "2 - Insert Log", 
            "3 - Update Log", 
            "4 - Delete Log",
            "5 - View Reports",
            "6 - Goals",
            "0 - Exit App"};

        while (!exitMenu) 
        {
            Console.Clear();
            ConsoleTableBuilder.From(optionsString)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Coding Tracker")
                .ExportAndWriteLine();
            Console.Write("\n");
            switch (askInput.PositiveNumber("Please select an option or press 0 to exit"))
            {
                case 0: exitMenu = true; break;
                case 1: ViewLogsMenu(); break;
                case 2: InsertLogsMenu(); break;
                case 3: UpdateLogMenu(); break;
                case 4: DeleteLogScreen(); break;
                case 5: ReportsMenu(); break;
                case 6: GoalsMenu(); break;
                default: break;

            }
        }
        return;
    }

    private void GoalsMenu()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            List<string> optionsString = new List<string> {
                "1 - View Goals",
                "2 - Set Goals",
                "0 - Return"};

            ConsoleTableBuilder.From(optionsString)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Goals")
                .ExportAndWriteLine();
            Console.Write("\n");

            switch (askInput.PositiveNumber("Please select an option or press 0 to exit"))
            {
                case 0: exit = true; continue;
                case 1:
                    Console.Clear();
                    ReportsView(dbCmds.ReturnAllLogsInTable());
                    break;
                case 2:
                    DateTime[] interval = askInput.DateInterval("Insert a start date.", "Insert an end date.");
                    Console.Clear();
                    ReportsView(listOp.ReturnLogsBetweenDates(dbCmds.ReturnAllLogsInTable(),
                        interval[0], interval[1]));

                    break;
                case 3: break;
                default: continue;
            }
            askInput.AnyKeyToContinue("\n");
        }
    }

    private void ReportsMenu()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            List<string> optionsString = new List<string> {
                "1 - Overview",
                "2 - Date interval",
                "3 - Last X",
                "0 - Return"};

            ConsoleTableBuilder.From(optionsString)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("View")
                .ExportAndWriteLine();
            Console.Write("\n");

            switch (askInput.PositiveNumber("Please select an option or press 0 to exit"))
            {
                case 0: exit = true; continue;
                case 1:
                    Console.Clear();
                    ReportsView(dbCmds.ReturnAllLogsInTable());
                    break;
                case 2:
                    DateTime[] interval = askInput.DateInterval("Insert a start date.", "Insert an end date.");
                    Console.Clear();
                    ReportsView(listOp.ReturnLogsBetweenDates(dbCmds.ReturnAllLogsInTable(),
                        interval[0], interval[1]));
                    
                    break;
                case 3: break;
                default: continue;
            }
            askInput.AnyKeyToContinue("\n");
        }
    }

    private void ReportsView(List<CodingSession> listToReport)
    {
        if (listToReport == null)
        {
            Console.WriteLine("Nothing to report on");
            return;
        }
        else 
        {
            string totalTime = listOp.TotalTimeInList(listToReport).ToString("hh\\:mm");
            string totalSessions = listOp.NumberOfSessionsInList(listToReport).ToString();
            string averageTime = listOp.AverageTimeInList(listToReport).ToString("hh\\:mm");
            string firstDate = listOp.FirstDateInList(listToReport).ToString("dd-MM-yy");
            string lastDate = listOp.LastDateInList(listToReport).ToString("dd-MM-yy");
            string diffBetweenFirstAndLast = listOp.DifferenceBetweenFirsAndLastDates(listToReport).Days.ToString();

            var tableList = new List<List<object>>
            {
                new List<object>{"Total time spent", totalTime},
                new List<object>{"Number of sessions", totalSessions},
                new List<object>{"Number of days", diffBetweenFirstAndLast},
                new List<object>{"Average time per session", averageTime},
            };

            ConsoleTableBuilder.From(tableList)
                .WithTitle($"{firstDate} to {lastDate}")
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();
        }
        return;
    }
    private void ViewLogsMenu()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            List<string> optionsString = new List<string> {
                "1 - View All Logs",
                "2 - View Ordered Logs",
                "3 - View Logs in date interval",
                "0 - Return"};

            ConsoleTableBuilder.From(optionsString)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("View")
                .ExportAndWriteLine();
            Console.Write("\n");

            switch (askInput.PositiveNumber("Please select an option or press 0 to exit"))
            {
                case 0: exit = true; break;
                case 1:
                    Console.Clear();
                    DisplaySessions(dbCmds.ReturnAllLogsInTable(), "VIEW ALL");
                    askInput.AnyKeyToContinue("\nPress any key to return");
                    break;
                case 2:
                    ViewOrderedLogsMenu();
                    break;
                case 3: ViewLogsInDateInterval(); break;
                default: break;
            }
        }
    }
    private void ViewLogsInDateInterval()
    {
        CodingSession codingSession = new();
        List<CodingSession> listToDisplay = new();

        DateTime[] interval = askInput.DateInterval("Insert the start date", "Insert the end date");
        listToDisplay = listOp.ReturnLogsBetweenDates(dbCmds.ReturnAllLogsInTable(), interval[0], interval[1]);

        Console.Clear();
        Console.Write('\n');
        DisplaySessions(listToDisplay, $"{interval[0].ToString("dd-MM-yy")} to " +
            $"{interval[1].ToString("dd-MM-yy")}");
        askInput.AnyKeyToContinue();
        return;
    }
    private void ViewOrderedLogsMenu()
    {
        bool exitMenu = false;
        List<string> optionsString = new List<string> {
            "1 - Ascending date",
            "2 - Descending date",
            "0 - Return"};


        while (!exitMenu)
        {
            Console.Clear();
            ConsoleTableBuilder.From(optionsString)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("View")
                .ExportAndWriteLine();
            Console.Write("\n");
            switch (askInput.PositiveNumber("Please select an option or press 0 to exit"))
            {
                case 0: exitMenu = true; break;
                case 1:
                    ViewOrderedLogs("ASCENDING");
                    askInput.AnyKeyToContinue("\nPress any key to return");
                    break;
                case 2:
                    ViewOrderedLogs("DESCENDING");
                    askInput.AnyKeyToContinue("\nPress any key to return");
                    break;
                case 3: break;
                default: break;
            }
        }
    }
    private void ViewOrderedLogs (string? order)
    {
        List<CodingSession> listToDisplay;
        Console.Clear();
        if (order == "ASCENDING") listToDisplay = listOp.ReturnOrderedByAscendingDate(dbCmds.ReturnAllLogsInTable());

        else if (order == "DESCENDING") listToDisplay = listOp.ReturnOrderedByDescendingDate(dbCmds.ReturnAllLogsInTable());
        else throw new Exception($"ViewOrderedLogs order not valid: {order}");
        DisplaySessions(listToDisplay, order);
        return;
    }
    private void DisplaySessions(List<CodingSession> listToDisplay, string title)
    {
        var tableDataDisplay = new List<List<object>>();
        if (listToDisplay is not null)
        {
            foreach (CodingSession session in listToDisplay)
            {
                tableDataDisplay.Add(
                    new List<object>
                    {
                    session.Id,
                    session.StartDateTime.ToString("dd-MM-yy"), session.StartDateTime.ToString("HH:mm"),
                    session.EndDateTime.ToString("dd-MM-yy"), session.EndDateTime.ToString("HH:mm"),
                    session.Duration.ToString("hh\\:mm")
                    });
            }
            ConsoleTableBuilder.From(tableDataDisplay)
                .WithTitle(title)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Id", "Start date", "Start time", "End date", "End time", "Duration")
                .ExportAndWriteLine();
        }
        else 
        {
            tableDataDisplay.Add(new List<object> { "", "", "", "", "", "" });
            ConsoleTableBuilder.From(tableDataDisplay)
                .WithTitle("EMPTY")
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Id", "Start date", "Start time", "End date", "End time", "Duration")
                .ExportAndWriteLine();
        }
        return;
    }
    private void InsertLogsMenu()
    {
        bool exit = false;
        while(!exit)
        {
            Console.Clear();
            List<string> optionsString = new List<string> {
                "1 - Manual Insert",
                "2 - StopWatch",
                "0 - Return"};

            ConsoleTableBuilder.From(optionsString)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Insert")
                .ExportAndWriteLine();
            Console.Write("\n");

            switch (askInput.PositiveNumber("Please select an option or press 0 to exit"))
            {
                case 0: exit = true; break;
                case 1: ManualLogInsert(); break;
                case 2: StopWatchInsert(); break;
                default: break;
            }
        }
    }
    private void ManualLogInsert()
    {
        CodingSession codingSession = new();
        bool validEntry;

        askInput.DateInterval("Insert the start date.", "Insert the end date.");
        do
        {
            Console.Write("\n");
            codingSession.StartDateTime = askInput.AskForDateWithHours("Insert the start date.");
            if (codingSession.StartDateTime != DateTime.MinValue)
            {
                codingSession.EndDateTime = askInput.AskForDateWithHours("Insert the end date.");
                if (codingSession.EndDateTime == DateTime.MinValue) return;

                codingSession.Duration =
                    codingSession.EndDateTime.Subtract(codingSession.StartDateTime);

                if (codingSession.Duration > TimeSpan.Zero)
                {
                    validEntry = true;
                    dbCmds.Insert(codingSession);
                }
                else
                {
                    Console.WriteLine("End date is earlier than the start date.");
                    askInput.AnyKeyToContinue();
                    validEntry = false;
                }
            }
            else return;
        } while (!validEntry);
        Console.WriteLine("Entry was logged successfully");
        askInput.AnyKeyToContinue();
        Console.Clear();
        return;
    }
    private void StopWatchInsert()
    {
        bool exit = false;
        exit = askInput.ZeroOrOtherAnyKeyToContinue(
            "Press any key to start the stopwatch. Or press 0 to return");

        if(!exit)
        {
            CodingSession sessionToInsert = new();
            sessionToInsert.StartDateTime = DateTime.Now;
            askInput.AnyKeyToContinue("Press any key to stop the session");
            sessionToInsert.EndDateTime = DateTime.Now;
            sessionToInsert.Duration =
                sessionToInsert.EndDateTime.Subtract(sessionToInsert.StartDateTime);
            Console.WriteLine($"\nThis session was {sessionToInsert.Duration.ToString("hh\\:mm")}");
            dbCmds.Insert(sessionToInsert);
            askInput.AnyKeyToContinue();
        }
        return;
    }
    private void UpdateLogMenu()
    {
        int index;
        bool showError = false, exit = false, validUpdate = false;
        CodingSession codingSession = new();

        while (!exit)
        {
            Console.Clear();
            DisplaySessions(dbCmds.ReturnAllLogsInTable(), "UPDATE");
            do
            {
                Console.Write("\n");
                if (!showError) index = askInput.PositiveNumber("Select the index you want to update. Or press 0 to return");
                else index = askInput.PositiveNumber("Please select a valid index");
                if (index == 0) exit = true;
            } while (!dbCmds.CheckIfIndexExistsInTable(index) && exit == false);

            if (!exit)
            {
                validUpdate = false;
                codingSession.StartDateTime = askInput.AskForDateWithHours("Insert the updated start date.");
                if (codingSession.StartDateTime != DateTime.MinValue)
                {
                    codingSession.EndDateTime = askInput.AskForDateWithHours("Insert the updated end date.");
                    if (codingSession.EndDateTime == DateTime.MinValue) break; ;

                    codingSession.Duration =
                        codingSession.EndDateTime.Subtract(codingSession.StartDateTime);

                    if (codingSession.Duration > TimeSpan.Zero)
                    {
                        validUpdate = dbCmds.Update(index, codingSession);
                    }
                    else
                    {
                        Console.WriteLine("End date is earlier than the start date.");
                        askInput.AnyKeyToContinue();
                    }
                }
            }
        }
        return;
    }
    private void DeleteLogScreen()
    {
        int index;
        bool showError = false, exit = false;

        while(!exit)
        {
            Console.Clear();
            DisplaySessions(dbCmds.ReturnAllLogsInTable(), "DELETE");
            do
            {
                Console.Write("\n");
                if (!showError) index = askInput.PositiveNumber("Select the index you want to delete. Or press 0 to return");
                else index = askInput.PositiveNumber("Please select a valid index");
                if (index == 0) exit = true;
            } while (!dbCmds.CheckIfIndexExistsInTable(index) && exit == false);

            if (!exit) 
            {
                if (dbCmds.DeleteByIndex(index)) Console.WriteLine("Log successfully deleted");
                else Console.WriteLine("Couldn't delete log...");
                askInput.AnyKeyToContinue();
            }
        }
        return;
    }
}
