using CardScoring.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardScoring
{
    class Program
    {
        private static ICommandLineArgs commandLineArgs;
        static void Main(string[] args)
        {
            InitErrorHandler();
            InitLogging();
            HandleArgs(args);
            DoWork();
        }

        private static void DoWork()
        {
            var worker = new Worker(commandLineArgs);
            worker.Work();
        }

        private static void InitLogging()
        {
            CardScoring.Logging.Logger.LogInfo("Logger Init");
        }

        private static void InitErrorHandler()
        {
            Logger.LogInfo("Init Exception Handler");
            System.AppDomain.CurrentDomain.UnhandledException += UnHandledExceptionHandler;
        }

        private static void UnHandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.LogError("Caught Unhandled Exception", (Exception)e.ExceptionObject);
            Environment.Exit(1);
        }

        private static void HandleArgs(string[] args)
        {
            //TODO parse args
            Logger.LogInfo("Parsing CommandLine Args");
            commandLineArgs = new CommandLineArgs();
            if (!args.Any())
            {
            }
            else if (args.First().Contains("help"))
            {
                Logger.LogInfo(HelpText);
            }
        }
        static string HelpText = @"
The syntax of this command is:
    CardScoring 
        [FileNames | Help | WorkingDir]";
    }
}
