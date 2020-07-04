using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpFPDF;
using System;
using System.Diagnostics;
using System.Globalization;

namespace SharpFPDFTests
{
    [TestClass]
    public class PdfTests
    {
        public Pdf _sut = new Pdf();


        [TestMethod]
        public void Constructor()
        {
            var sut = new Pdf();
        }

        [TestMethod]
        public void SPrintF()
        {
            // sprintf('%.2F w',$lw *$this->k)
            double number = 0.5703;

            var actual = number.ToString("0.00 w", CultureInfo.InvariantCulture);

            Assert.AreEqual("0.57 w", actual);
        }

        [TestMethod]
        public void SetColors()
        {
            _sut.SetDrawColor(14);

            _sut.SetFillColor(0, 0, 0);

            _sut.SetTextColor(50, 128, 30);

        }

        //protected void BeginPage<T?>(Orientation? orientation, T? size, Rotation? rotation)
        //{
        //    PageSize? pageSize = null;
        //    Size? dimension = null;
        //    if (size is PageSize foundPageSize) { pageSize = foundPageSize; }
        //    else if (size is Size foundDimension) { dimension = foundDimension; }
        //    else { throw new ArgumentException("The wrong type", nameof(size)); }

        //    if (pageSize != null) Debug.WriteLine($"Found a page size {pageSize}");
        //    if (dimension != null) Debug.WriteLine($"Found a dimension {dimension.Width} | {dimension.Height}");

        //}

        //[TestMethod]
        //public void TestBeginPage()
        //{
        //    BeginPage(Orientation.Portrait, PageSize.A4, Rotation.Degree90);
        //    BeginPage(Orientation.Portrait, new Size(1, 2), Rotation.Degree90);

        //}


        [TestMethod]
        public void PossibleAddPageOverloads()
        {
            _sut.AddPage();

            _sut.AddPage(Orientation.Portrait);
            _sut.AddPage(rotation: Rotation.Degree180);
            _sut.AddPage(PageSize.A5);
            _sut.AddPage(size: new Size(4, 5));

            _sut.AddPage(Orientation.Landscape, Rotation.Degree180);
            _sut.AddPage(Orientation.Landscape, PageSize.A5);
            _sut.AddPage(Orientation.Landscape, size: new Size(4, 5));

            _sut.AddPage(Rotation.Degree180, PageSize.A5);
            _sut.AddPage(Rotation.Degree180, new Size(4, 5));

            _sut.AddPage(Orientation.Landscape, Rotation.Degree180, PageSize.A4);
            _sut.AddPage(Orientation.Landscape, Rotation.Degree180, new Size(56,43));
        }
    }
}
