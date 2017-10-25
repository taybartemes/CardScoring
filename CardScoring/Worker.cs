using CardScoring.Processing;
using CardScoring.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Linq;

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
            //TODO Verify file
            var results = new List<IPlotPOLCNTOutput>();
            var imgProcessor = new CardScoring.Processing.CircleProcessor();
            if (string.IsNullOrEmpty(commandLineArgs.WorkingDir) && commandLineArgs.FilesToProcess.Any())
            {
                foreach (var file in commandLineArgs.FilesToProcess)
                {
                    var ret = ProcessFile(file, imgProcessor);
                    if(ret != null)
                    {
                        results.Add(ret);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(commandLineArgs.WorkingDir))
            {
                var files = Directory.GetFiles(commandLineArgs.WorkingDir);
                foreach (var file in files)
                {
                    var ret = ProcessFile(file, imgProcessor);
                    if (ret != null)
                    {
                        results.Add(ret);
                    }
                }
            }
            else
            {
                //working dir is current dir
                var files = Directory.GetFiles(".");
                foreach (var file in files)
                {
                    var ret = ProcessFile(file, imgProcessor);
                    if (ret != null)
                    {
                        results.Add(ret);
                    }
                }
            }

            if (!string.IsNullOrEmpty(commandLineArgs.OutputLocation))
            {
                CsvHelperWrapper.WriteCsv(commandLineArgs.OutputLocation, results);
            }
            else
            {
                CsvHelperWrapper.WriteCsv("results.csv", results);
            }

        }

        private IPlotPOLCNTOutput ProcessFile(string path, CircleProcessor imgProcessor)
        {
            if (!File.Exists(path))
            {
                Logging.Logger.LogErrorFormat("File does not exist: {0}", path);
                return null;
            }
            try {
                if (path.EndsWith(".zip")) return null;
                if (path.EndsWith(".gz"))
                {
                    var fs = new FileStream(path, FileMode.Open);
                    var finfo = new FileInfo(path);
                    var currFile = Path.GetFullPath(path);
                    var newFile = currFile.Remove(currFile.Length - finfo.Extension.Length);
                    using (var newFs = new FileStream(newFile, FileMode.OpenOrCreate))
                    using (var stream = new System.IO.Compression.GZipStream(fs, System.IO.Compression.CompressionMode.Decompress))
                    {
                        stream.CopyTo(newFs);
                        Logging.Logger.LogInfoFormat("Decompressed {0} to {1}", path, newFs.Name);
                    }
                    if (Directory.Exists(newFile))
                    {
                        var newFiles = Directory.GetFiles(newFile);
                        foreach (var i in newFiles)
                        {
                            return ProcessFile(i, imgProcessor);
                        }
                    }
                    else
                    {
                        return ProcessFile(newFile, imgProcessor);
                    }
                }
            }
            catch(Exception ex)
            {
                Logging.Logger.LogError("Error while decompressing", ex);
            }
            var entry = new PlotPOLCNTOutput();
            Logging.Logger.LogInfo(string.Format("Processing File: {0}", path));
            int euid = 0;
            var bc = ReadBarcode(path);
            if (!int.TryParse(bc, out euid))
            {
                Logging.Logger.LogErrorFormat("Expected Euid in barcode, got: {0}", bc);
                Logging.Logger.LogInfo("Moving on to next file...");
                return null;
            }
            else
            {
                entry.EUID = euid;
            }

            Logging.Logger.LogInfo(string.Format("Finding circles:", path));
            var bm = new Bitmap(path);
            var circles = imgProcessor.FindCircles(bm);
            var midPoint = bm.Width / 2;
            Logging.Logger.LogInfo(string.Format("Finding scores:", path));
            var numFinder = new NumberFinder();
            for (int i = 0; i < circles.Size; i++)
            {
                var circle = circles[i];
                var score = numFinder.FindNumbers(circle, bm);
                if (circle.ToArray().All(x => x.X > midPoint))
                {
                    entry.POLCNT = score;
                }
                else
                {
                    entry.Plot = score;
                }
            }
            return entry;
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