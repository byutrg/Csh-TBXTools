using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TBXTools
{
    public static class LoggingManager
    {
        public enum LoggingLevels
        {
            None,
            ErrorsOnly,
            Info,
            Verbose
        }

        public static LoggingLevels LoggingLevel { get; set; } = LoggingLevels.Info;

        public delegate void OutputFunc(string message, params object[] args);
        public static OutputFunc Output { get; set; } = (message, args) => { if (LoggingLevel > LoggingLevels.None) Console.WriteLine(message); };

        public static OutputFunc OutputError { get; set; } = (message, args) => { if (LoggingLevel >= LoggingLevels.ErrorsOnly) Output(message, args); };
        public static OutputFunc OutputInfo { get; set; } = (message, args) => { if (LoggingLevel >= LoggingLevels.Info) Output(message, args); };
        public static OutputFunc OutputVerbose { get; set; } = (message, args) => { if (LoggingLevel >= LoggingLevels.Verbose) Output(message, args); };
    }
}
