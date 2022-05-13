using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using XRayAnalyzer.MVVM.Model;
using XRayAnalyzer.Objects.Enums;

namespace XRayAnalyzer.Services
{
    class LogingService
    {
        private static readonly Lazy<LogingService> lazySingleton = new(() => new LogingService());
        public static LogingService Instance { get { return lazySingleton.Value; } }

        private TextBox? LogDisplay { get; set; }
        private int MaxLength { get; set; } = 15000;
        private string? LogingPath { get; set; }
        private string LogFileName { get; set; } = string.Empty;

        private LogingService() { }

        public LogingService SetTextBox(TextBox logDisplay)
        {
            LogDisplay = logDisplay;
            return this;
        }

        private void SaveLogToFile(Log log)
        {
            if (LogingPath == null || LogingPath != Properties.Settings.Default.LogingPath)
            {
                LogingPath = Properties.Settings.Default.LogingPath;

                DirectoryInfo di = new DirectoryInfo(LogingPath);
                if (di.Exists)
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                }

                LogFileName = Path.Combine(LogingPath, DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".log");
                if (!Directory.Exists(LogingPath)) Directory.CreateDirectory(LogingPath);
            }

            try
            {
                using StreamWriter sw = File.AppendText(LogFileName);
                sw.WriteLine(string.Format("{0} [{1}] - {2}", log.TimeStamp.ToString("dd.MM.yyyy HH:mm:ss"), log.Type, log.Message));
            }
            catch (Exception ex)
            {
                ShowInLogDisplay(new Log() { Message = ex.Message, Type = LogType.Error, TimeStamp = DateTime.Now });
            }
        }

        private void ShowInLogDisplay(Log log)
        {
            if (LogDisplay == null)
            {
                return;
            }

            string text = LogDisplay.Text;
            text += string.Format("{0} [{1}] - {2}", log.TimeStamp.ToString("dd.MM.yyyy HH:mm:ss"), log.Type, log.Message) + Environment.NewLine;
            if (text.Length > MaxLength) text = text.Remove(0, text.Length - MaxLength);
            LogDisplay.Text = text;
            LogDisplay.ScrollToEnd();
        }

        public void AddMessage(string message, LogType type = LogType.Info)
        {
            Log logMessage = new()
            {
                Message = message,
                Type = type,
                TimeStamp = DateTime.Now
            };
            ShowInLogDisplay(logMessage);
            SaveLogToFile(logMessage);
        }
    }
}
