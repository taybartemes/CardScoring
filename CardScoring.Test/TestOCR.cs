using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardScoring.Processing;
using System.IO;

namespace CardScoring.Test
{
    [TestClass]
    public class TestOCR
    {
        //TODO not file I/O in this test
        [TestMethod]
        public void TestOCR1()
        {
            var reader = new OCRWrapper();
            TestImg.test_card_1.Save("tmp");
            var text = reader.Read("tmp");
            File.Delete("tmp");
            Assert.IsFalse(string.IsNullOrEmpty(text));

        }
    }
}
