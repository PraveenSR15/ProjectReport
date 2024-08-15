﻿using Models;
using Utilities;

namespace Actions
{
    [ActionName("MonthlyReportInOutEntry")]
    internal class MonthlyReportInOutEntryAction(string inputFolder) : Action
    {
        private List<string> _monthlyReports = [];
        private List<string> _musterOptionsReports = [];

        public override void Init()
        {
            _monthlyReports = Helper.GetReports(inputFolder, Constants.MonthlyReportPattern).ToList();

            _musterOptionsReports = Helper.GetReports(inputFolder, Constants.MusterOptionsPattern).ToList();
        }

        public override bool Run()
        {
            Logger.LogFileNames(_musterOptionsReports, "Muster Options files found found:");

            Logger.LogFileNames(_monthlyReports, "Monthly reports found:");

            var monthlyReportsData = _monthlyReports.Select(x => (Services.DataService.ExtractEmployeeIdFromFileName(x), x)).ToList();

            var musterOptionsDatas = ReadService.ReadMusterOptions(_musterOptionsReports);

            if (musterOptionsDatas != null && musterOptionsDatas.Datas.Count > 0)
            {
                return WriteService.WriteMonthlyReportInOutEntry(monthlyReportsData, musterOptionsDatas);
            }
            else
            {
                Logger.LogWarning($"Muster options data is empty, check if data is present, otherwise report application error to {Constants.ApplicationAdmin}", 2);
                return false;
            }
        }

        public override bool Validate()
        {
            bool res = ValidateDirectory(inputFolder);

            res = res && ValidateReports(_monthlyReports, $"No Monthly Report files with naming pattern {Constants.MonthlyReportPattern} found on {inputFolder}.");

            res = res && ValidateReports(_musterOptionsReports, $"No Muster Options files with naming pattern {Constants.MusterOptionsPattern} found on {inputFolder}.");

            return res;
        }
    }
}