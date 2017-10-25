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
            try {
                if (!args.Any())
                {
                }
                else if (args.Any(x => x.StartsWith("-h")))
                {
                    Logger.LogInfo(HelpText);
                }
                if (args.Any(x => x.StartsWith("-f")))
                {
                    var files = args.FirstOrDefault(x => x.Contains("-f"))?.Split('=', ',');
                    files = files.Where(x => x != "-f" && x != "=").ToArray();
                    commandLineArgs.FilesToProcess = files;
                }
                if (args.Any(x => x.StartsWith("-w")))
                {
                    var wDir = args.FirstOrDefault(x => x.Contains("-w"))?.Split('=');
                    var dir = wDir.FirstOrDefault(x => x != "-w" && x != "=");
                    commandLineArgs.WorkingDir = dir;
                }
                if (args.Any(x => x.StartsWith("-o")))
                {
                    var oDir = args.FirstOrDefault(x => x.Contains("-o"))?.Split('=');
                    var dir = oDir.FirstOrDefault(x => x != "-o" && x != "=");
                    commandLineArgs.OutputLocation = dir;
                }

            }
            catch (Exception ex)
            {
                Logger.LogError("Got an invalid Command Line argument", ex);
            }
        }
        static string HelpText = @"
The syntax of this command is:
    CardScoring 
        [-f FileName| -h Help | -w WorkingDir | -o OutputFile]";
    }
}
