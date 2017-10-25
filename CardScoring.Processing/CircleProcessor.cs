using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing;
using System.IO;
using Emgu.CV.Util;

namespace CardScoring.Processing
{
    //TODO MAKE THIS WORK
    public class CircleProcessor
    {
        public CircleProcessor()
        {

        }

        public VectorOfVectorOfPoint FindCircles(Bitmap map, double cannyThreshold = 280, double circleAccumulatedThreshold = 120)
        {
            map.Save("tmp");
            var inMap = CvInvoke.Imread("tmp");
            var gray = inMap.Clone();
            CvInvoke.CvtColor(inMap, gray, ColorConversion.Bgr2Gray);
            CvInvoke.PyrUp(gray, gray);
            CvInvoke.PyrDown(gray, gray);
            CvInvoke.GaussianBlur(gray, inMap, new Size(13, 13), 1.5);
            CvInvoke.Canny(gray, inMap, 75, 200);
            var output = new VectorOfVectorOfPoint();
            var copy = inMap.Clone();
            var opt = inMap.Clone();
            var circleList = new List<IInputArray>();
            CvInvoke.FindContours(copy, output, opt, RetrType.Tree, ChainApproxMethod.ChainApproxSimple);

            if (output.Size > 0)
            {
                for (int o = 0; o < output.Size; o++)
                {
                    var rect = CvInvoke.BoundingRectangle(output[o]);
                    //circleList.Add(output[o]);
                    var ar = rect.Width / (float)rect.Height;
                    //It's a circle if...
                    if (rect.Width > 15 && rect.Height > 15 && ar >= .9 && ar <= 1.1)
                    {
                        circleList.Add(output[o]);
                    }
                }
            }
            var vv = new VectorOfVectorOfPoint(circleList.Select(x => (VectorOfPoint)x).ToArray());
            var bb = new Image<Bgr, byte>(inMap.Size);
            bb.Draw((IInputArrayOfArrays)vv, -1, new Bgr(255, 255, 255));
            //bb.Save(@"E:\src\CardScoring\bb.png");


            var thresh = bb.Clone();
            CvInvoke.Threshold(gray, thresh, 0, 255, ThresholdType.BinaryInv | ThresholdType.Otsu);
           
            var leftLargest = new Tuple<int, VectorOfPoint>(0, new VectorOfPoint());
            var rightLargest = new Tuple<int, VectorOfPoint>(0, new VectorOfPoint());
            var midPoint = map.Width / 2;
            var clean = thresh;
            for (var idx = 0; idx < vv.Size; idx++)
            {
                thresh = clean;
                var mask = new Image<Gray, byte>(thresh.Size);
                var dest = new Image<Gray, byte>(thresh.Size);
                var clone = new Image<Gray, byte>(thresh.Bitmap);
                var other = new Image<Gray, byte>(thresh.Bitmap);
                var item = vv[idx];
                //use an assumption to chop off bottom
                if (item.ToArray().Any(x => x.Y > 225))
                {
                    continue;
                }

                var oneContour = new VectorOfVectorOfPoint(item);
                var g = new Gray(255);
                mask.Draw(oneContour, -1, g, -1);
                CvInvoke.BitwiseAnd(thresh, thresh, dest, mask);
                var total = CvInvoke.CountNonZero(dest);
                if (item.ToArray().All(x => x.X > midPoint))
                {
                    if (total > rightLargest.Item1)
                    {
                        rightLargest = new Tuple<int, VectorOfPoint>(total, item);
                    }
                }
                else
                {
                    if (total > leftLargest.Item1)
                    {
                        leftLargest = new Tuple<int, VectorOfPoint>(total, item);
                    }
                }
            }
            vv = new VectorOfVectorOfPoint(leftLargest.Item2, rightLargest.Item2);
            //var found = new Image<Gray, byte>(thresh.Size);
            //found.Draw(vv, -1, new Gray(255), 1);
            //found.Save(@"E:\src\CardScoring\found.png");
            try
            {
                File.Delete("tmp");
            }
            catch(Exception ex)
            {
            }

            return vv;
        }
    }
}
