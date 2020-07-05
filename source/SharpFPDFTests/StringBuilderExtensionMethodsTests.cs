using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpFPDF;
using System.Text;

namespace SharpFPDFTests
{
    [TestClass]
    public class StringBuilderExtensionMethodsTests
    {

        [TestMethod]
        public void AppendNewLine()
        {
            // arrange
            var sut = new StringBuilder();
            var expected = "Hello Pdf\n";

            // act
            sut.AppendNewLine("Hello Pdf");

            // assert
            Assert.AreEqual(expected, sut.ToString());
        }
    }
}
