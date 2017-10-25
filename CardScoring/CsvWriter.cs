using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;

namespace CardScoring
{
    public static class CsvHelperWrapper
    {
        public static void WriteCsv<T>(string path, IEnumerable<T> objectsToWrite)
        {
            try
            {
                using (TextWriter writer = File.CreateText(path))
                {
                    var csv = new CsvWriter(writer);
                    csv.Configuration.HasHeaderRecord = true;
                    //csv.WriteHeader(typeof(T));
                    csv.WriteRecords(objectsToWrite);
                }
            }
            catch(Exception ex)
            {
                Logging.Logger.LogError(ex);
            }
        }
    }
}
