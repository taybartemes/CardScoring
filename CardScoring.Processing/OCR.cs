using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            path = Upscale(path);
            var img = Pix.LoadFromFile(path);
            img = img.Scale(2, 1);
            var page = ocr.Process(img);
            var text = page.GetText();
            return text;
        }

        public string Upscale(string path)
        {
            Bitmap newImage = new Bitmap(300, 300);
            var img = Image.FromFile(path);
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(img, new Rectangle(0, 0, 300, 300));
            }
            return path;
        }
    }
}
