using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SharpFPDF
{
    public class Pdf
    {
        #region Constructors

        public Pdf()
        {
            var orientation = Orientation.Landscape;
            var unit = Unit.mm;
            InitFonts();
            _k = InitScaleFactor(unit);
            var dimension = GetPageSize(PageSize.A4, _k);
            InitPageSize(dimension);
            InitPageOrientation(orientation, dimension);
            InitRemainingParameter();
        }

        public Pdf(Orientation orientation = Orientation.Landscape,
                   Unit unit = Unit.mm,
                   PageSize pageSize = PageSize.A4)
        {
            InitFonts();
            _k = InitScaleFactor(unit);
            var dimension = GetPageSize(pageSize, _k);
            InitPageSize(dimension);
            InitPageOrientation(orientation, dimension);
            InitRemainingParameter();
        }

        public Pdf(Orientation orientation = Orientation.Landscape,
                   Unit unit = Unit.mm,
                   double width = _widthA4, double height = _heightA4)
        {
            InitFonts();
            _k = InitScaleFactor(unit);
            var dimension = new Dimension(width, height);
            InitPageSize(dimension);
            InitPageOrientation(orientation, dimension);
            InitRemainingParameter();
        }


        #endregion

        #region Fields

        private const double _widthA4 = 595.28;
        private const double _heightA4 = 841.89;
        private const string _pdfVerion1_3 = "1.3";
        private const string _pdfVerion1_4 = "1.4";
        private const string _defaultDrawColor = "0 G";
        private const string _defaultFillColor = "0 g";
        private const string _defaultTextColor = "0 g";

        /// <summary>
        /// current document state
        /// </summary>
        private State _state = State.Init;

        /// <summary>
        /// current page number
        /// </summary>
        private int _page;

        /// <summary>
        /// current object number
        /// </summary>
        private int _n = 2;

        /// <summary>
        /// buffer holding in-memory PDF
        /// </summary>
        private StringBuilder _buffer = new StringBuilder();

        /// <summary>
        /// (array) list containing pages
        /// </summary>
        private readonly Dictionary<int, StringBuilder> _pages = new Dictionary<int, StringBuilder>();

        /// <summary>
        /// page-related data
        /// </summary>
        private readonly List<PageInfo> _pageInfos = new List<PageInfo>();

        /// <summary>
        /// array of core font names
        /// </summary>
        private readonly List<string> _coreFonts = new List<string>
        {
            "courier", "helvetica", "times", "symbol", "zapfdingbats"
        };

        private double _k = 0;

        private readonly Dictionary<PageSize, Dimension> _standardPageSizes =
            new Dictionary<PageSize, Dimension>()
            {
                { PageSize.A3, new Dimension(841.89, 1190.55) },
                { PageSize.A4, new Dimension(_widthA4, _heightA4) },
                { PageSize.A5, new Dimension(420.94, 595.28) },
                { PageSize.Letter, new Dimension(612, 792) },
                { PageSize.Legal, new Dimension(612, 1008) }
            };

        /// <summary>
        /// default page size
        /// </summary>
        private Dimension _defPageSize = default!;

        /// <summary>
        /// current page size
        /// </summary>
        private Dimension _curPageSize = default!;

        /// <summary>
        /// default orientation
        /// </summary>
        private Orientation _defOrientation = Orientation.Portrait;
        /// <summary>
        /// current orientation
        /// </summary>
        private Orientation _curOrientation = Orientation.Portrait;

        /// <summary>
        /// width of current page in user unit
        /// </summary>
        private double _w;

        /// <summary>
        /// height of current page in user unit
        /// </summary>
        private double _h;

        /// <summary>
        /// width of current page in points
        /// </summary>
        private double _wPt;

        /// <summary>
        /// height of current page in points
        /// </summary>
        private double _hPt;

        /// <summary>
        /// current page rotation
        /// </summary>
        private double _curRotation;

        /// <summary>
        /// left margin
        /// </summary>
        private double _lMargin;

        /// <summary>
        /// top margin
        /// </summary>
        private double _tMargin;

        /// <summary>
        /// right margin
        /// </summary>
        private double _rMargin;

        /// <summary>
        /// cell margin
        /// </summary>
        private double _cMargin;

        /// <summary>
        /// page break margin
        /// </summary>
        private double _bMargin;

        /// <summary>
        /// current x position in user unit
        /// </summary>
        private double _x;

        /// <summary>
        /// current y position in user unit
        /// </summary>
        private double _y;

        /// <summary>
        /// line width in user unit
        /// </summary>
        private double _lineWidth;

        /// <summary>
        /// height of last printed cell
        /// </summary>
        private double _lasth;

        /// <summary>
        /// automatic page breaking
        /// </summary>
        private bool _autoPageBreak;

        /// <summary>
        /// threshold used to trigger page breaks
        /// </summary>
        private double _pageBreakTrigger;

        /// <summary>
        /// zoom display mode
        /// </summary>
        /// <remarks>We need also the _zoomFactor because the zoomMode in FPDF is a mixed type.</remarks>
        private ZoomMode? _zoomMode = null;

        /// <summary>
        /// zoom display factor
        /// </summary>
        /// <remarks>We need the _zoomFactor because the zoomMode in FPDF is a mixed type.</remarks>
        private double? _zoomFactor = null;

        /// <summary>
        /// layout display mode
        /// </summary>
        private LayoutMode _layoutMode;

        /// <summary>
        /// compression flag
        /// </summary>
        private bool _compress;

        /// <summary>
        /// PDF version number
        /// </summary>
        private string _pdfVersion = _pdfVerion1_3;

        /// <summary>
        /// document properties
        /// </summary>
        private readonly MetaData _metaData = new MetaData();

        /// <summary>
        /// alias for total number of pages
        /// </summary>
        private string _aliasNbPages = string.Empty;

        /// <summary>
        /// flag set when processing footer
        /// </summary>
        private bool _inFooter;

        /// <summary>
        /// flag set when processing header
        /// </summary>
        private bool _inHeader;

        /// <summary>
        /// current font family
        /// </summary>
        private string _fontFamily = string.Empty;

        /// <summary>
        /// current font style
        /// </summary>
        private string _fontStyle = string.Empty;

        /// <summary>
        /// current font size in points
        /// </summary>
        private double _fontSizePt = 12;

        /// <summary>
        /// underlining flag
        /// </summary>
        private bool _underline;

        /// <summary>
        /// commands for drawing color
        /// </summary>
        private string _drawColor = _defaultFillColor;

        /// <summary>
        /// commands for filling color
        /// </summary>
        private string _fillColor = _defaultFillColor;

        /// <summary>
        /// commands for text color
        /// </summary>
        private string _textColor = _defaultTextColor;

        /// <summary>
        /// indicates whether fill and text colors are different
        /// </summary>
        private bool _colorFlag;

        /// <summary>
        /// indicates whether alpha channel is used
        /// </summary>
        private bool _withAlpha;


        //################## NOT YET DEFINED

        /// <summary>
        /// array of used fonts
        /// </summary>
        private readonly List<int> _fonts = default!;

        //#########################################

        #endregion



        #region FPDF Methods

        /// <summary>
        /// Returns the current page width.
        /// </summary>
        /// <seealso cref="GetPageHeight"/>
        public double GetPageWidth => _w;

        /// <summary>
        /// Returns the current page height.
        /// </summary>
        /// <seealso cref="GetPageWidth"/>
        public double GetPageHeight => _h;

        /// <summary>
        /// Defines the left, top and right margins. By default, they equal 1 cm. Call this method to change them.
        /// </summary>
        /// <param name="left">Left margin.</param>
        /// <param name="top">Top margin.</param>
        /// <param name="right">Right margin. Default value is the left one.</param>
        /// <seealso cref="SetLeftMargin(double)"/>
        /// <seealso cref="SetTopMargin(double)"/>
        /// <seealso cref="SetRightMargin(double)"/>
        /// <seealso cref="SetAutoPageBreak(bool, double)"/>
        public void SetMargins(double left, double top, double? right = null)
        {
            _lMargin = left;
            _tMargin = top;
            _rMargin = right ?? left;
        }

        /// <summary>
        /// Defines the left margin. The method can be called before creating the first page.
        /// If the current abscissa gets out of page, it is brought back to the margin.
        /// </summary>
        /// <param name="margin">The margin.</param>
        /// <seealso cref="SetTopMargin(double)"/>
        /// <seealso cref="SetRightMargin(double)"/>
        /// <seealso cref="SetAutoPageBreak(bool, double)"/>        
        /// <seealso cref="SetMargins(double, double, double?)"/>
        public void SetLeftMargin(double margin)
        {
            _lMargin = margin;

            if (_page > 0 && _x < margin)
            {
                _x = margin;
            }
        }

        /// <summary>
        /// Defines the top margin. The method can be called before creating the first page. 
        /// </summary>
        /// <param name="margin">The margin.</param>
        /// <seealso cref="SetLeftMargin(double)"/>
        /// <seealso cref="SetRightMargin(double)"/>
        /// <seealso cref="SetAutoPageBreak(bool, double)"/>        
        /// <seealso cref="SetMargins(double, double, double?)"/>
        public void SetTopMargin(double margin)
        {
            _tMargin = margin;
        }

        /// <summary>
        /// Defines the right  margin. The method can be called before creating the first page. 
        /// </summary>
        /// <param name="margin">The margin.</param>
        /// <seealso cref="SetLeftMargin(double)"/>
        /// <seealso cref="SetTopMargin(double)"/>
        /// <seealso cref="SetAutoPageBreak(bool, double)"/>        
        /// <seealso cref="SetMargins(double, double, double?)"/>
        public void SetRightMargin(double margin)
        {
            _rMargin = margin;

            if (_page > 0 && _x < margin)
            {
                _x = margin;
            }
        }

        /// <summary>
        /// Enables or disables the automatic page breaking mode.
        /// When enabling, the second parameter is the distance from the bottom of the page
        /// that defines the triggering limit. By default, the mode is on and the margin is 2 cm.
        /// </summary>
        /// <param name="auto">Boolean indicating if mode should be on or off.</param>
        /// <param name="margin">Distance from the bottom of the page.</param>
        // TODO SeeAlso
        public void SetAutoPageBreak(bool auto, double margin = 0)
        {
            _autoPageBreak = auto;
            _bMargin = margin;
            _pageBreakTrigger = _h - margin;
        }

        /// <summary>
        /// Defines the way the document is to be displayed by the viewer. 
        /// The zoom level can be set: pages can be displayed entirely on screen, 
        /// occupy the full width of the window, use real size, 
        /// be scaled by a specific zooming factor or use viewer default
        /// (configured in the Preferences menu of Adobe Reader).
        /// The page layout can be specified too: single at once, continuous display, two columns or viewer default. 
        /// </summary>
        /// <param name="zoomMode">The zoom to use. <see cref="ZoomMode"/></param>
        /// <param name="layout">The page layout.</param>
        public void SetDisplayMode(ZoomMode zoomMode, LayoutMode layout = LayoutMode.Default)
        {
            _zoomMode = zoomMode;
            _zoomFactor = null;
            _layoutMode = layout;
        }

        /// <summary>
        /// Defines the way the document is to be displayed by the viewer. 
        /// The zoom level can be set: pages can be displayed entirely on screen, 
        /// occupy the full width of the window, use real size, 
        /// be scaled by a specific zooming factor or use viewer default
        /// (configured in the Preferences menu of Adobe Reader).
        /// The page layout can be specified too: single at once, continuous display, two columns or viewer default. 
        /// </summary>
        /// <param name="zoom">The zoom factor to use.</param>
        /// <param name="layout">The page layout.</param>
        public void SetDisplayMode(double zoom, LayoutMode layout = LayoutMode.Default)
        {
            // TODO: Validation of valid zoom factor
            _zoomFactor = zoom;
            _zoomMode = null;
            _layoutMode = layout;
        }

        /// <summary>
        /// Activates or deactivates page compression.
        /// When activated, the internal representation of each page is compressed,
        /// which leads to a compression ratio of about 2 for the resulting document.
        /// Compression is on by default. 
        /// </summary>
        /// <param name="useCompression">Boolean indicating if compression must be enabled.</param>
        public void SetCompression(bool useCompression)
        {
            _compress = useCompression;
        }

        /// <summary>
        /// Defines the title of the document.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <seealso cref="SetAuthor(string)"/>
        /// <seealso cref="SetSubject(string)"/>
        /// <seealso cref="SetKeywords(string)"/>
        /// <seealso cref="SetCreator(string)"/>
        public void SetTitle(string title)
        {
            _metaData.Title = title;
        }

        /// <summary>
        /// Defines the author of the document.
        /// </summary>
        /// <param name="author">The author.</param>
        /// <seealso cref="SetTitle(string)"/>
        /// <seealso cref="SetSubject(string)"/>
        /// <seealso cref="SetKeywords(string)"/>
        /// <seealso cref="SetCreator(string)"/>
        public void SetAuthor(string author)
        {
            _metaData.Author = author;
        }

        /// <summary>
        /// Defines the subject of the document.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <seealso cref="SetTitle(string)"/>
        /// <seealso cref="SetAuthor(string)"/>
        /// <seealso cref="SetKeywords(string)"/>
        /// <seealso cref="SetCreator(string)"/>
        public void SetSubject(string subject)
        {
            _metaData.Subject = subject;
        }

        /// <summary>
        /// Defines the keywords of the document.
        /// </summary>
        /// <param name="keywords">The keywords.</param>
        /// <seealso cref="SetTitle(string)"/>
        /// <seealso cref="SetAuthor(string)"/>
        /// <seealso cref="SetSubject(string)"/>
        /// <seealso cref="SetCreator(string)"/>
        public void SetKeywords(string keywords)
        {
            _metaData.Keywords = keywords;
        }

        /// <summary>
        /// Defines the creator of the document.
        /// </summary>
        /// <param name="creator">The creator.</param>
        /// <seealso cref="SetTitle(string)"/>
        /// <seealso cref="SetAuthor(string)"/>
        /// <seealso cref="SetSubject(string)"/>
        /// <seealso cref="SetKeywords(string)"/>
        public void SetCreator(string creator)
        {
            _metaData.Creator = creator;
        }

        /// <summary>
        /// Defines an alias for the total number of pages. It will be substituted as the document is closed. 
        /// </summary>
        /// <param name="alias">The alias. Default value: {nb}.</param>
        //  TODO: SeeAlso
        public void AliasNbPages(string alias = "{nb}")
        {
            _aliasNbPages = alias;
        }

        /// <summary>
        /// Terminates the PDF document. It is not necessary to call this method 
        /// explicitly because Output() does it automatically.
        /// If the document contains no page, AddPage() is called to prevent from getting an invalid document.
        /// </summary>
        public void Close()
        {
            if (_state == State.Closed) return;
            if (_page == 0) AddPage();
            _inFooter = true;
            Footer();
            _inFooter = false;
            // Close page
            EndPage();
            // Close document
            EndDoc();

        }

        public void AddPage(Orientation? orientation = null, PageSize? pageSize = null, Rotation? rotation = null)
        {
            // Start a new page
            if (_state == State.Closed) throw new InvalidOperationException("The document is closed");

            var family = _fontFamily;
            var style = string.Format("{0}{1}", _fontStyle, _underline ? "U" : "");
            var fontSize = _fontSizePt;
            var lw = _lineWidth;
            var dc = _drawColor;
            var fc = _fillColor;
            var tc = _textColor;
            var cf = _colorFlag;

            if (_page > 0)
            {
                // Page footer
                _inFooter = true;
                Footer();
                _inFooter = false;
                // Close page
                EndPage();
            }
            // Start new page
            BeginPage(orientation, pageSize, rotation);
            // Set line cap style to square
            Out("2 J");
            // Set line width
            _lineWidth = lw;
            Out((lw * _k).ToString("0.00 w", CultureInfo.InvariantCulture));
            // Set font
            if (!string.IsNullOrWhiteSpace(family))
            {
                SetFont(family, style, fontSize);
            }
            // Set colors
            _drawColor = dc;
            if (dc != _defaultDrawColor)
            {
                Out(dc);
            }
            _fillColor = fc;
            if (fc != _defaultFillColor)
            {
                Out(fc);
            }
            _textColor = tc;
            _colorFlag = cf;
            // Page header
            _inHeader = true;
            Header();
            _inHeader = false;
            // Restore line width
            if (_lineWidth != lw)
            {
                _lineWidth = lw;
                Out((lw * _k).ToString("0.00 w", CultureInfo.InvariantCulture));
            }
            // Restore font
            if (!string.IsNullOrWhiteSpace(family))
            {
                SetFont(family, style, fontSize);
            }
            // Restore colors
            if (_drawColor != dc)
            {
                _drawColor = dc;
                Out(dc);
            }
            if (_fillColor != fc)
            {
                _fillColor = fc;
                Out(fc);
            }
            _textColor = tc;
            _colorFlag = cf;
        }

        /// <summary>
        /// Returns the current page number. 
        /// </summary>
        /// <returns>page number</returns>
        public int PageNo() => _page;

        /// <summary>
        /// Defines the color used for all drawing operations (lines, rectangles and cell borders). 
        /// It can be expressed in RGB components or gray scale. The method can be called before
        /// the first page is created and the value is retained from page to page. 
        /// </summary>
        /// <param name="r">If g and b are given, red component; if not, indicates the gray level. Value between 0 and 255.</param>
        /// <param name="g">Green component (between 0 and 255).</param>
        /// <param name="b">Blue component (between 0 and 255).</param>
        public void SetDrawColor(byte r, byte? g = null, byte? b = null)
        {
            _drawColor = ConverColor(r, g, b);
            if (_page > 0)
            {
                Out(_drawColor);
            }
        }

        /// <summary>
        /// Defines the color used for all filling operations (filled rectangles and cell backgrounds). 
        /// It can be expressed in RGB components or gray scale. The method can be called before 
        /// the first page is created and the value is retained from page to page. 
        /// </summary>
        /// <param name="r">If g and b are given, red component; if not, indicates the gray level. Value between 0 and 255.</param>
        /// <param name="g">Green component (between 0 and 255).</param>
        /// <param name="b">Blue component (between 0 and 255).</param>
        public void SetFillColor(byte r, byte? g = null, byte? b = null)
        {
            _fillColor = ConverColor(r, g, b);
            if (_page > 0)
            {
                Out(_fillColor);
            }
        }

        /// <summary>
        /// Defines the color used for text. It can be expressed in RGB components or gray scale. 
        /// The method can be called before the first page is created and the value is retained 
        /// from page to page. 
        /// </summary>
        /// <param name="r">If g and b are given, red component; if not, indicates the gray level. Value between 0 and 255.</param>
        /// <param name="g">Green component (between 0 and 255).</param>
        /// <param name="b">Blue component (between 0 and 255).</param>
        public void SetTextColor(byte r, byte? g = null, byte? b = null)
        {
            _textColor = ConverColor(r, g, b);
            if (_page > 0)
            {
                Out(_textColor);
            }
        }

        /// <summary>
        /// Convert the RGB byte values into the needed string.
        /// </summary>
        /// <param name="r">red</param>
        /// <param name="g">green</param>
        /// <param name="b">blue</param>
        /// <returns>The color string.</returns>
        private static string ConverColor(byte r, byte? g, byte? b)
        {
            const double max = 255.0;
            string color;

            if (g == null || b == null || (r == 0 && g == 0 && b == 0))
            {
                color = (r / max).ToString("0.000 G", CultureInfo.InvariantCulture);
            }
            else
            {
                color = string.Format(CultureInfo.InvariantCulture, "{0:0.000} {1:0.000} {2:0.000} RG", r / max, g / max, b / max);
            }

            return color;
        }

        /// <summary>
        /// Defines the line width. By default, the value equals 0.2 mm.
        /// The method can be called before the first page is created and 
        /// the value is retained from page to page. 
        /// </summary>
        /// <param name="width">The width.</param>
        public void SetLineWidth(double width)
        {
            // Set line width
            _lineWidth = width;
            if (_page > 0)
            {
                Out((width * _k).ToString("0.00 w", CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Draws a line between two points.
        /// </summary>
        /// <param name="x1">Abscissa of first point.</param>
        /// <param name="y1">Ordinate of first point.</param>
        /// <param name="x2">Abscissa of second point.</param>
        /// <param name="y2">Ordinate of second point.</param>
        public void Line(double x1, double y1, double x2, double y2)
        {
            // Draw a line
            Out(string.Format(
                CultureInfo.InvariantCulture,
                "{0:0.00} {1:0.00} m {2:0.00} {3:0.00} l S", x1 * _k, (_h - y1) * _k, x2 * _k, (_h - y2) * _k));
        }

        /// <summary>
        /// Outputs a rectangle. It can be drawn (border only), filled (with no border) or both. 
        /// </summary>
        /// <param name="x">Abscissa of upper-left corner.</param>
        /// <param name="y">Ordinate of upper-left corner.</param>
        /// <param name="w">Width.</param>
        /// <param name="h">Height.</param>
        /// <param name="style">Style of rendering. Possible values see <see cref="RectStyle"/>.</param>
        public void Rect(double x, double y, double w, double h, RectStyle style = RectStyle.Draw)
        {
            // Draw a rectangle
            string op;
            switch (style)
            {
                case RectStyle.Draw:
                    op = "S";
                    break;
                case RectStyle.Fill:
                    op = "f";
                    break;
                case RectStyle.DrawAndFill:
                    op = "B";
                    break;
                default:
                    throw new NotImplementedException($"No implementation for rect style: {style}");
            }
            Out(string.Format(
                CultureInfo.InvariantCulture,
                "{0:0.00} {1:0.00} {2:0.00} {3:0.00} re {4}", x * _k, (_h - y) * _k, x * _k, -h * _k, op));
        }


        /// <summary>
        /// Whenever a page break condition is met, the method is called, and the break is issued or not
        /// depending on the returned value. The default implementation returns a value according to the mode
        /// selected by SetAutoPageBreak().
        /// This method is called automatically and should not be called directly by the application.
        /// </summary>
        /// <returns>Is a page break accepted?</returns>
        public virtual bool AcceptPageBreak() => _autoPageBreak;

        /// <summary>
        /// Performs a line break. The current abscissa goes back to the left margin and 
        /// the ordinate increases by the amount passed in parameter. 
        /// </summary>
        /// <param name="h">The height of the break. By default, the value equals the height of the last printed cell.</param>
        public void Ln(double? h = null)
        {
            // Line feed; default value is the last cell height
            _x = _lMargin;
            if (h == null)
            {
                _y += _lasth;
            }
            else
            {
                _y += (double)h;
            }
        }

        /// <summary>
        /// Returns the abscissa of the current position.
        /// </summary>
        /// <returns></returns>
        public double GetX() => _x;

        /// <summary>
        /// Defines the abscissa of the current position. If the passed value is negative, 
        /// it is relative to the right of the page.
        /// </summary>
        /// <param name="x">The value of the abscissa.</param>
        public void SetX(double x)
        {
            // Set x position
            if (x >= 0)
            {
                _x = x;
            }
            else
            {
                _x = _w + x;
            }
        }

        /// <summary>
        /// Returns the ordinate of the current position. 
        /// </summary>
        /// <returns></returns>
        public double GetY() => _y;

        /// <summary>
        /// Sets the ordinate and optionally moves the current abscissa back
        /// to the left margin. If the value is negative, it is relative to the bottom of the page. 
        /// </summary>
        /// <param name="y">The value of the ordinate. </param>
        /// <param name="resetX">Whether to reset the abscissa. Default value: <c>true</c>. </param>
        public void SetY(double y, bool resetX = true)
        {
            // Set y position and optionally reset x
            if (y >= 0)
            {
                _y = y;
            }
            else
            {
                _y = _h + y;
            }

            if (resetX) _x = _lMargin;
        }

        /// <summary>
        /// Defines the abscissa and ordinate of the current position. If the passed values are negative,
        /// they are relative respectively to the right and bottom of the page. 
        /// </summary>
        /// <param name="x">The value of the abscissa.</param>
        /// <param name="y">The value of the ordinate.</param>
        public void SetXY(double x, double y)
        {
            // Set x and y positions
            SetX(x);
            SetY(y, false);
        }



        /*




         */



        /// <summary>
        /// This method is used to render the page footer. It is automatically called by AddPage()
        /// and Close() and should not be called directly by the application. The implementation 
        /// in SharpFPDF is empty, so you have to subclass it and override the method if you want
        /// a specific processing. 
        /// </summary>
        public virtual void Footer()
        {
            // To be implemented in your own inherited class
        }

        /// <summary>
        /// This method is used to render the page header. It is automatically called by AddPage() 
        /// and should not be called directly by the application. The implementation in FPDF is empty, 
        /// so you have to subclass it and override the method if you want a specific processing. 
        /// </summary>
        public virtual void Header()
        {
            // To be implemented in your own inherited class
        }

        protected void Out(string s)
        {
            switch (_state)
            {
                case State.Init:
                    throw new InvalidOperationException("No page has been added yet.");
                case State.EndPage:
                    Put(s);
                    break;
                case State.BeginPage:
                    _pages[_page].AppendNewLine(s);
                    break;
                case State.Closed:
                    throw new InvalidOperationException("No page has been added yet.");
                default:
                    throw new NotImplementedException($"No implementation for state: {_state}");
            }
        }

        protected void Put(string s)
        {
            _buffer.AppendNewLine(s);
        }

        protected int GetOffset()
        {
            return _buffer.Length;
        }

        protected void BeginPage(Orientation? orientation, PageSize? pageSize, Rotation? rotation)
        {
            throw new NotImplementedException();
        }


        private void EndPage()
        {
            throw new NotImplementedException();
        }
        private void EndDoc()
        {
            throw new NotImplementedException();
        }


        private void SetFont(string family, string style, double fontSize)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region InitMethods

        // TODO implement
        private void InitFonts()
        {


            /*
             * From Constructor ...
             * 
             * 	// Font path
                if(defined('FPDF_FONTPATH'))
                {
                    $this->fontpath = FPDF_FONTPATH;
                    if(substr($this->fontpath,-1)!='/' && substr($this->fontpath,-1)!='\\')
                        $this->fontpath .= '/';
                }
                elseif(is_dir(dirname(__FILE__).'/font'))
                    $this->fontpath = dirname(__FILE__).'/font/';
                else
                    $this->fontpath = '';

             */
        }

        private static double InitScaleFactor(Unit unit)
        {
            switch (unit)
            {
                case Unit.pt:
                    return 1;
                case Unit.mm:
                    return 72 / 25.4;
                case Unit.cm:
                    return 72 / 2.54;
                case Unit.inch:
                    return 72;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, "Unknown unit");
            }
        }

        private void InitPageSize(Dimension dimension)
        {
            _curPageSize = dimension;
            _defPageSize = dimension;
        }

        private void InitPageOrientation(Orientation orientation, Dimension size)
        {
            switch (orientation)
            {
                case Orientation.Portrait:
                    _w = size.Width;
                    _h = size.Height;
                    break;
                case Orientation.Landscape:
                    _w = size.Height;
                    _h = size.Width;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(orientation), orientation, "Unknown orientation");
            }

            _wPt = _w * _k;
            _hPt = _h * _k;
        }


        private void InitRemainingParameter()
        {
            const double oneCm72Dpi = 28.35; // 72 dpi / 2.54 cm / inch
            // Page margins (1 cm)
            var margin = oneCm72Dpi / _k;
            SetMargins(margin, margin);
            // Interior cell margin (1 mm)
            _cMargin = margin / 10;
            // Line width (0.2 mm)
            _lineWidth = .567 / _k;
            // Automatic page break
            SetAutoPageBreak(true, 2 * margin);
            // Default display mode
            SetDisplayMode(ZoomMode.Default);
            // Enable compression
            SetCompression(true);
        }

        #endregion

        #region Helper

        private Dimension GetPageSize(PageSize size, double k)
        {
            if (_standardPageSizes.ContainsKey(size))
            {
                var uncorrectedDimension = _standardPageSizes[size];
                var dimension = new Dimension(uncorrectedDimension.Width / k, uncorrectedDimension.Height / k);
                return dimension;
            }

            throw new ArgumentOutOfRangeException(nameof(size), size, "The size has an unexpected value.");
        }

        #endregion
    }
}
