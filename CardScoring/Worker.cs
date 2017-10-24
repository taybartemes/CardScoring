using CardScoring.Processing;
using CardScoring.Models;
using System;
using System.Collections.Generic;

namespace CardScoring
{
    public interface IWorker
    {
        void Work();
    }
    public class Worker : IWorker
    {
        private ICommandLineArgs commandLineArgs;
        private List<PlotPOLCNTOutput> csvRows;

        public Worker(ICommandLineArgs commandLineArgs)
        {
            this.commandLineArgs = commandLineArgs;
            csvRows = new List<PlotPOLCNTOutput>();
        }

        public void Work()
        {
            if (string.IsNullOrEmpty(commandLineArgs.WorkingDir))
            {
                Logging.Logger.LogInfo("Checking File");
                //TODO Verify file
                //var validFiles = FileVerifier.Verify();
                foreach (var file in commandLineArgs.FilesToProcess)
                {
                    var entry = new PlotPOLCNTOutput();
                    Logging.Logger.LogInfo(string.Format("Processing File: {0}", file));
                    int euid = 0;
                    var bc = ReadBarcode(file);
                    if(!int.TryParse(bc, out euid))
                    {
                        Logging.Logger.LogErrorFormat("Expected Euid in barcode, got: {0}", bc);
                        Logging.Logger.LogInfo("Moving on to next file...");
                        continue;
                    }
                    else
                    {
                        entry.EUID = euid;
                    }
                    //TODO Process File
                }
            }
        }

        private string ReadBarcode(string file)
        {
            Logging.Logger.LogInfoFormat("Reading barcode on: {0}", file);
            var bcodeReader = new BarcodeReaderWrapper();
            var euid = bcodeReader.Decode(file);
            return euid;
        }
    }
}