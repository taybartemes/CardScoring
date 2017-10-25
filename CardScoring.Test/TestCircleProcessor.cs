using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CardScoring.Test
{
    [TestClass]
    public class TestCircleProcessor
    {
        [TestMethod]
        public void FindCirclesTest1()
        {
            var imgProcessor = new CardScoring.Processing.CircleProcessor();
            var circles = imgProcessor.FindCircles(TestImg.test_card_1);
            Assert.AreEqual(circles.Size, 2);
        }

        [TestMethod]
        public void FindCirclesTest2()
        {
            var imgProcessor = new CardScoring.Processing.CircleProcessor();
            var circles = imgProcessor.FindCircles(TestImg.test_card_2);
            Assert.AreEqual(circles.Size, 2);
        }

        [TestMethod]
        public void FindCirclesTest3()
        {
            var imgProcessor = new CardScoring.Processing.CircleProcessor();
            var circles = imgProcessor.FindCircles(TestImg.test_card_3);
            Assert.AreEqual(circles.Size, 2);
        }


    }
}
