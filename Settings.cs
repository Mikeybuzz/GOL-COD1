using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GOLStartUp
{
    public class Settings
    {
        public int TimerIntervalMS { get; set; } = defaultTimerInterval;
        public int NumRows { get; set; } = defaultRows;
        public int NumColumns { get; set; } = defaultColumns;

        const int defaultTimerInterval = 100;
        const int defaultRows = 5;
        const int defaultColumns = 5;

        private string filePath;

        public Settings(string path)
        {
            filePath = path;

            if (!File.Exists(filePath))
            {
                Save();
                return;
            }

            string settingsText = File.ReadAllText(filePath);
            if(string.IsNullOrEmpty(settingsText))
            {
                Save();
                return;
            }

            SetValuesFromString(settingsText);
        }

        public void ResetToDefaults()
        {
            TimerIntervalMS = defaultTimerInterval;
            NumRows = defaultRows;
            NumColumns = defaultColumns;
        }

        public void Save()
        {
            string settingsAsText = GetSettingsAsString();
            File.WriteAllText(filePath, settingsAsText);
        }

        public string GetSettingsAsString()
        {
            return $"NumRows={NumRows},NumColumns={NumColumns},TimerIntervalMS={TimerIntervalMS}";
        }

        private void SetValuesFromString(string values)
        {
            if (string.IsNullOrEmpty(values))
                return;

            var split = values.Split(',');

            // get the rows
            var rows = split[0];
            var rowsSplit = rows.Split('=');
            int.TryParse(rowsSplit[1], out int numRows);
            NumRows = numRows;

            // get the columns
            var cols = split[1];
            var colsSplit = cols.Split('=');
            int.TryParse(colsSplit[1], out int numCols);
            NumColumns = numCols;

            // get the timer interval
            var timer = split[2];
            var timerSplit = timer.Split('=');
            int.TryParse(timerSplit[1], out int timerInterval);
            TimerIntervalMS = timerInterval;
        }
    }
}
