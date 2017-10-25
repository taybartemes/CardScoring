using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardScoring.Processing
{
    public class NumberFinder
    {
        public NumberFinder()
        { }

        //public int FindNearestNumber(VectorOfPoint v, Bitmap bm)
        //{
        //    var numberCandidates = FindNumbers(v, bm);
        //    var bmBoundingBox = CvInvoke.BoundingRectangle(v);
        //    var center = new Point(bmBoundingBox.X + bmBoundingBox.Width / 2, bmBoundingBox.Y - bmBoundingBox.Height / 2);
        //    var minDistance = double.MaxValue;
        //    IInputArray contour = null;
        //    foreach (var item in numberCandidates)
        //    {
        //        var itemBB = CvInvoke.BoundingRectangle(item);
        //        var itemCenter = new Point(itemBB.X + itemBB.Width / 2, itemBB.Y - itemBB.Height / 2);
        //        var distance = Distance(itemCenter, center);
        //        if (distance < minDistance)
        //        {
        //            minDistance = distance;
        //            contour = item;
        //        }
        //    }
        //    if(contour != null)
        //    {
        //        var ocrWrapper = new OCRWrapper();
        //        var text = ocrWrapper.Read("tmp");
        //        text.Trim();
        //        if (CleanText(text))
        //            return int.Parse(text);
        //    }
        //    return -1;
        //}

        public int FindNumbers(VectorOfPoint v, Bitmap bm)
        {
            Logging.Logger.LogInfo("Looking for numbers");
            bm.Save("tmp");
            var inMap = CvInvoke.Imread("tmp");
            var gray = inMap.Clone();
            CvInvoke.CvtColor(inMap, gray, ColorConversion.Bgr2Gray);
            CvInvoke.PyrUp(gray, gray);
            CvInvoke.GaussianBlur(gray, inMap, new Size(13, 13), 1.5);
            CvInvoke.Canny(gray, inMap, 75, 200);
            var output = new VectorOfVectorOfPoint();
            var img = new Image<Bgr, byte>(bm);
            var refRect = CvInvoke.BoundingRectangle(v);
            img.ROI = refRect;
            //img.Draw(refRect, new Bgr(255, 255, 255), 1);
            var center = new Point(refRect.X + refRect.Width / 2, refRect.Y + refRect.Height / 2);
            try
            {
                File.Delete("tmp");
            }
            catch(Exception ex)
            {

            }
            //take advantage of the size of the image, dont' get me wrong, i hate this
            return (int)Math.Round((center.Y - 36) / 19.7);
        }

        private double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        private List<IInputArray> FindNumbers(Bitmap bm)
        {
            Logging.Logger.LogInfo("Looking for numbers");
            bm.Save("tmp");
            var inMap = CvInvoke.Imread("tmp");
            var gray = inMap.Clone();
            CvInvoke.CvtColor(inMap, gray, ColorConversion.Bgr2Gray);
            CvInvoke.PyrUp(gray, gray);
            CvInvoke.GaussianBlur(gray, inMap, new Size(13, 13), 1.5);
            CvInvoke.Canny(gray, inMap, 75, 200);
            var output = new VectorOfVectorOfPoint();
            var copy = inMap.Clone();
            var opt = inMap.Clone();
            var contours = new List<IInputArray>();
            CvInvoke.FindContours(copy, output, opt, RetrType.Tree, ChainApproxMethod.ChainApproxSimple);

            var k = new Image<Bgr, byte>(inMap.Size);
            k.Draw(output, -1, new Bgr(255, 255, 255));
            var ocrWrapper = new OCRWrapper();
            if (output.Size > 0)
            {
                for (int o = 0; o < output.Size; o++)
                {
                    //CvInvoke.ApproxPolyDP(output[o], curr, CvInvoke.ArcLength(output[o], true) * 0.05, true);
                    var rect = CvInvoke.BoundingRectangle(output[o]);

                    //curr.Draw(rect, new Bgr(255, 255, 255));
                    var ar = rect.Width / (float)rect.Height;
                    //It's not circle if it's irregular

                    if (rect.Width > 15 && rect.Height > 15 && ar >= .9 && ar <= 1.1) { }
                    else //if(rect.Width < 20 && rect.Height < 30)
                    {
                        //contours.Add(output[o]);
                        //var fileName = "temp" + o + ".bmp";
                        //curr.ROI = 
                        //var vvp = new VectorOfVectorOfPoint(output[o]);
                        //var curr = new Image<Bgr, byte>(inMap.Size);
                        //curr.Draw((IInputArrayOfArrays)vvp, -1, new Bgr(255, 255, 255));
                        //curr.Save(fileName);
                        //var text = ocrWrapper.Read(fileName);
                        //text.Trim();
                        //if (CleanText(text))
                        //    contours.Add(curr);
                        //DeleteFile(fileName);
                    }
                }
                var vv = new VectorOfVectorOfPoint(contours.Select(x => (VectorOfPoint)x).ToArray());
                var bb = new Image<Bgr, byte>(inMap.Size);
                bb.Draw((IInputArrayOfArrays)vv, -1, new Bgr(255, 255, 255));
                //bb.Save(@"E:\src\CardScoring\bb.png");
            }
            Logging.Logger.LogInfoFormat("Found {0} numbers", contours.Count);
            DeleteFile("tmp");
            return contours;
        }
        private void DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch { }

        }

        private bool CleanText(string text)
        {
            int ret = -1;
            //I guess i don't care if it fails
            return int.TryParse(text, out ret);
        }

        //I hate this
    }
}
