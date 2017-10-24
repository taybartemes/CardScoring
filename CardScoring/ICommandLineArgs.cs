using System;
using System.Collections.Generic;

namespace CardScoring
{
    public interface ICommandLineArgs
    {
        IEnumerable<string> FilesToProcess { get; set; }
        string WorkingDir { get; set; }
        string OutputLocation { get; set; }
    }
    public class CommandLineArgs : ICommandLineArgs
    {
        public IEnumerable<string> FilesToProcess { get; set; }
        public string WorkingDir {get;set;}
        public string OutputLocation {get;set;}

        public CommandLineArgs()
        {
            FilesToProcess = new List<string>();
            OutputLocation = "";
            WorkingDir = "";
        }
        
    }
}