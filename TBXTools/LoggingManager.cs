using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TBXTools
{
    public static class LoggingManager
    {
        public delegate void OutputFunc(string message, params object[] args);

        public enum LoggingLevels
        {
            None,
            ErrorsOnly,
            Info,
            Verbose
        }

        public static LoggingLevels LoggingLevel { get; private set; } = LoggingLevels.None;

        private static OutputFunc output = (message, args) => { if (LoggingLevel > LoggingLevels.None) Console.WriteLine(message); };
        private static OutputFunc outputError = (message, args) => { if (LoggingLevel >= LoggingLevels.ErrorsOnly) Output(message, args); };
        private static OutputFunc outputInfo = (message, args) => { if (LoggingLevel >= LoggingLevels.Info) Output(message, args); };
        private static OutputFunc outputVerbose = (message, args) => { if (LoggingLevel >= LoggingLevels.Verbose) Output(message, args); };

        private static OutputFunc SetInternalOutputFunc(OutputFunc func, LoggingLevels level) => (message, args) => { if (LoggingLevel >= level) func(message, args); };

        public static OutputFunc Output {
            get => output;
            private set => output = SetInternalOutputFunc(value, LoggingLevels.None);
        } 
        public static OutputFunc OutputError {
            get => outputError;
            private set => outputError = SetInternalOutputFunc(value, LoggingLevels.ErrorsOnly);
        } 
        public static OutputFunc OutputInfo {
            get => outputInfo;
            private set => outputInfo = SetInternalOutputFunc(value, LoggingLevels.Info);
        }
        public static OutputFunc OutputVerbose {
            get => outputVerbose;
            private set => outputVerbose = SetInternalOutputFunc(value, LoggingLevels.Verbose);
        } 

        public static void SetLoggingLevel(LoggingLevels level) => LoggingLevel = level;
        /// <summary>
        /// Set Output Function. Will be invoked only if LoggingLevel >= LoggingLevels.None.
        /// </summary>
        /// <param name="function"></param>
        public static void SetOutputFunction(OutputFunc function) => Output = function;
        /// <summary>
        /// Set OutputError Function. Will be invoked only if LoggingLevel >= LoggingLevels.ErrorsOnly.
        /// </summary>
        /// <param name="function"></param>
        public static void SetOutputErrorFunction(OutputFunc function) => OutputError = function;
        /// <summary>
        /// Set OutputInfo Function. Will be invoked only if LoggingLevel >= LoggingLevels.Info.
        /// </summary>
        /// <param name="function"></param>
        public static void SetOutputInfoFunction(OutputFunc function) => OutputInfo = function;
        /// <summary>
        /// Set OutputVerbose Function. Will be invoked only if LoggingLevel >= LoggingLevels.Verbose.
        /// </summary>
        /// <param name="function"></param>
        public static void SetOutputVerboseFunction(OutputFunc function) => OutputVerbose = function;
    }
}
