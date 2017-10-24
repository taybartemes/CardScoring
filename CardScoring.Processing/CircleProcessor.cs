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

namespace CardScoring.Processing
{
    public class CircleProcessor
    {
        public CircleProcessor()
        {

        }

        public CircleF[] FindCircles(Bitmap map, double cannyThreshold = 280, double circleAccumulatedThreshold = 120)
        {
            var image = new Image<Gray, byte>(map);
            var color = new Image<Bgr, byte>(map);
            var cleanedImage = image.Not();
            cleanedImage = RemoveNoise(cleanedImage);
            var circles = CvInvoke.HoughCircles(cleanedImage, HoughType.Gradient, 8.0, 5.0, cannyThreshold, circleAccumulatedThreshold, 0, image.Width/5);
            return circles;
        }

        public IEnumerable<CircleF> GetFilledCircles(IEnumerable<CircleF> circles, Image<Gray, byte> cleanedImage)
        {
            var filledCircles = new List<CircleF>();
            var bm = cleanedImage.ToBitmap();
            foreach (var item in circles)
            {
                //var centerPixel = bm.GetPi
                //var circlePixels = GetPixelsInRadiius(centerPixel, cleanedImage, item);

            }
            return filledCircles;
        }

        private Image<Gray, byte> RemoveNoise(Image<Gray, byte> image)
        {
            UMat tmp = new UMat();
            CvInvoke.PyrDown(image, tmp);
            CvInvoke.PyrUp(image, tmp);
            CvInvoke.Canny(image, image, 120, 120);
            return image;
        }
    }
}
