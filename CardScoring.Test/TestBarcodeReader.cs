using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZXing;
using System.Drawing;
using CardScoring.Processing;

namespace CardScoring.Test
{
    [TestClass]
    public class TestBarcodeReader
    {
        [TestMethod]
        public void TestQRRead1()
        {
            var reader = new BarcodeReaderWrapper();
            var bmap = TestImg.test_card_1;
            var result = reader.Decode(bmap);
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Text));
        }

        [TestMethod]
        public void TestQRRead2()
        {
            var reader = new BarcodeReaderWrapper();
            var bmap = TestImg.test_card_2;
            var result = reader.Decode(bmap);
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Text));
        }

        [TestMethod]
        public void TestQRRead3()
        {
            var reader = new BarcodeReaderWrapper();
            var bmap = TestImg.test_card_3;
            var result = reader.Decode(bmap);
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Text));
        }
    }
}

