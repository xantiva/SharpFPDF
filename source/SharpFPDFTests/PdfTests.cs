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
        [TestMethod]
        public void TestMethod1()
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
            var sut = new Pdf();
            sut.SetDrawColor(14);

            sut.SetFillColor(0, 0, 0);

            sut.SetTextColor(50, 128, 30);

        }

        protected void BeginPage<T?>(Orientation? orientation, T? size, Rotation? rotation)
        {
            PageSize? pageSize = null;
            Size? dimension = null;
            if (size is PageSize foundPageSize) { pageSize = foundPageSize; }
            else if (size is Size foundDimension) { dimension = foundDimension; }
            else { throw new ArgumentException("The wrong type", nameof(size)); }

            if (pageSize != null) Debug.WriteLine($"Found a page size {pageSize}");
            if (dimension != null) Debug.WriteLine($"Found a dimension {dimension.Width} | {dimension.Height}");

        }

        [TestMethod]
        public void TestBeginPage()
        {
            BeginPage(Orientation.Portrait, PageSize.A4, Rotation.Degree90);
            BeginPage(Orientation.Portrait, new Size(1, 2), Rotation.Degree90);

        }

        protected void AddPage(Orientation? orientation = null, Rotation? rotation = null, Size? dimension = null)
        {
            // Do everything
        }
        public void AddPage()
        {
            AddPage(null, null, (Size?)null);
        }

        public void AddPage(PageSize pageSize)
        {
            // convert PageSize into Dimension
            var dimension = new Size(1, 2);
            AddPage(null, null, dimension);
        }
        public void AddPage(Orientation orientation)
        {
            AddPage(orientation, null, (Size?)null);
        }

        public void AddPage(Orientation orientation, Rotation rotation)
        {
            AddPage(orientation, rotation, (Size?)null);
        }

        public void AddPage(Orientation orientation, Rotation rotation, PageSize pageSize)
        {
            // convert PageSize into Dimension
            var dimension = new Size(1,2);
            AddPage(orientation, rotation, dimension);
        }

        [TestMethod]
        public void TestAddPage()
        {
            AddPage();
            AddPage(Orientation.Portrait);
            AddPage(Orientation.Landscape, Rotation.Degree180);
            AddPage(Orientation.Landscape, Rotation.Degree180, PageSize.A4);
            AddPage(Orientation.Landscape, Rotation.Degree180, new Size(56,43));
            AddPage(rotation: Rotation.Degree180);
            //AddPage(PageSize.A5);
            //AddPage(Rotation.Degree180, PageSize.A5);
            //AddPage(Orientation.Landscape, PageSize.A5);
            //AddPage(new Dimension(4,5));

        }
    }
}
