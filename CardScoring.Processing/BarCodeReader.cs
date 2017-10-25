using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using ZXing;
using System.IO;
using CardScoring.Logging;

namespace CardScoring.Processing
{
    public class BarcodeReaderWrapper : BarcodeReader
    {
        public BarcodeReaderWrapper()
        { }

        public string Decode(string path)
        {
            if (File.Exists(path))
            {
                if (!path.EndsWith(".png")) return "";
                var barcodeBitMap = (Bitmap)Image.FromFile(path);
                var result = Decode(barcodeBitMap);
                if(result != null)
                {
                    if (string.IsNullOrEmpty(result.Text))
                    {
                        Logger.LogErrorFormat("BarCodeReader: No Euid found in file at {0}", path);
                    }
                    else
                    {
                        Logger.LogInfoFormat("Found EUID: {0}", result.Text);
                        return result.Text;
                    }
                }
            }
            else
            {
                Logger.LogErrorFormat("BarCodeReader: Path does not exist: {0}", path);
            }
            return "";
        }
    }
}
