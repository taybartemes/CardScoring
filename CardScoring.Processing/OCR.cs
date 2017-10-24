using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace CardScoring.Processing
{
    //TODO this whole thing
    public class OCRWrapper
    {
        public OCRWrapper()
        {
        }

        public string Read(string path)
        {
            if (!File.Exists(path))
            {
                Logging.Logger.LogErrorFormat("OCRWrapper: Path does not exist: {0}", path);
                return "";
            }
            var ocr = new TesseractEngine("./tessdata", "eng");
            var img = Pix.LoadFromFile(path);
            var page = ocr.Process(img);
            var text = page.GetText();
            return text;
        }
    }
}
