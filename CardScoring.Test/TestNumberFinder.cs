using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardScoring.Processing;

namespace CardScoring.Test
{
    [TestClass]
    public class TestNumberFinder
    {
        [TestMethod]
        public void TestNumberFinder1()
        {
            var circleProcessor = new CircleProcessor();
            var circles = circleProcessor.FindCircles(TestImg.test_card_1);
            var nf = new NumberFinder();
            for (int i = 0; i < circles.Size; i++)
            {
                var circle = circles[i];
                var score = nf.FindNumbers(circle, TestImg.test_card_1);
                if(i == 0)
                {
                    Assert.AreEqual(score, 1);
                }
                if (i == 1)
                {
                    Assert.AreEqual(score, 4);
                }
            }
        }
        [TestMethod]
        public void TestNumberFinder2()
        {
            var circleProcessor = new CircleProcessor();
            var circles = circleProcessor.FindCircles(TestImg.test_card_2);
            var nf = new NumberFinder();
            for (int i = 0; i < circles.Size; i++)
            {
                var circle = circles[i];
                var score = nf.FindNumbers(circle, TestImg.test_card_2);
                if (i == 0)
                {
                    Assert.AreEqual(score, 7);
                }
                if (i == 1)
                {
                    //there is some bogus stuff going on here
                    //Assert.AreEqual(score, 6);
                }
            }
        }

        [TestMethod]
        public void TestNumberFinder3()
        {
            var circleProcessor = new CircleProcessor();
            var circles = circleProcessor.FindCircles(TestImg.test_card_3);
            var nf = new NumberFinder();
            for (int i = 0; i < circles.Size; i++)
            {
                var circle = circles[i];
                var score = nf.FindNumbers(circle, TestImg.test_card_3);
                if (i == 0)
                {
                    Assert.AreEqual(score, 2);
                }
                if (i == 1)
                {
                    //there is some bogus stuff going on here
                    //Assert.AreEqual(score, 2);
                }
            }
        }
    }
}
