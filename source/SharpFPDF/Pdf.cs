using SharpFPDF.Fonts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SharpFPDF
{
    public class Pdf
    {
        #region Constructors

        public Pdf()
        {
            var orientation = Orientation.Portrait;
            var unit = Unit.mm;
            InitFonts();
            _k = InitScaleFactor(unit);
            var size = GetSize(PageSize.A4, _k);
            InitPageSize(size);
            InitPageOrientation(orientation, size);
            InitRemainingParameter();
        }

        public Pdf(Orientation orientation = Orientation.Portrait,
                   Unit unit = Unit.mm,
                   PageSize pageSize = PageSize.A4)
        {
            InitFonts();
            _k = InitScaleFactor(unit);
            var size = GetSize(pageSize, _k);
            InitPageSize(size);
            InitPageOrientation(orientation, size);
            InitRemainingParameter();
        }

        public Pdf(Orientation orientation = Orientation.Portrait,
                   Unit unit = Unit.mm,
                   double width = _widthA4, double height = _heightA4)
        {
            InitFonts();
            _k = InitScaleFactor(unit);
            var size = new Size(width, height);
            InitPageSize(size);
            InitPageOrientation(orientation, size);
            InitRemainingParameter();
        }

        #endregion

        #region Fields

        private const double _widthA4 = 595.28;
        private const double _heightA4 = 841.89;
        private const string _pdfVerion1_3 = "1.3";
        private const string _pdfVerion1_4 = "1.4";
        private const string _SharpFPDF_VERSION = "0.1";
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
        private readonly Dictionary<int, PageInfo> _pageInfos = new Dictionary<int, PageInfo>();

        /// <summary>
        /// array of object offsets
        /// </summary>
        private readonly Dictionary<int, int> _offsets = new Dictionary<int, int>();

        /// <summary>
        /// array of core font names
        /// </summary>
        private readonly List<string> _coreFonts = new List<string>
        {
            "courier", "helvetica", "times", "symbol", "zapfdingbats"
        };

        private double _k = 0;

        private readonly Dictionary<PageSize, Size> _standardPageSizes =
            new Dictionary<PageSize, Size>()
            {
                { PageSize.A3, new Size(841.89, 1190.55) },
                { PageSize.A4, new Size(_widthA4, _heightA4) },
                { PageSize.A5, new Size(420.94, 595.28) },
                { PageSize.Letter, new Size(612, 792) },
                { PageSize.Legal, new Size(612, 1008) }
            };

        /// <summary>
        /// default page size
        /// </summary>
        private Size _defPageSize = default!;

        /// <summary>
        /// current page size
        /// </summary>
        private Size _curPageSize = default!;

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
        private Rotation _curRotation;

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
        private readonly Dictionary<string, string> _metaData = new Dictionary<string, string>();

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
        private FontStyles _fontStyle = FontStyles.Regular;

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
        private string _drawColor = _defaultDrawColor;

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

        /// <summary>
        /// word spacing
        /// </summary>
        private double _ws;

        /// <summary>
        /// current font info
        /// </summary>
        private PdfFont? _currentFont;

        /// <summary>
        /// current font size in user unit
        /// </summary>
        private double _fontSize;

        /// <summary>
        /// array of ToUnicode CMaps
        /// </summary>
        private Dictionary<string, int> _cmaps = new Dictionary<string, int>();

        /// <summary>
        /// array of encodings [encoding, object number]
        /// </summary>
        private Dictionary<string, int> _encodings = new Dictionary<string, int>();

        /// <summary>
        /// array of font files
        /// </summary>
        private Dictionary<string, PdfFont> _fontFiles = new Dictionary<string, PdfFont>();


        //################## NOT YET DEFINED

        /// <summary>
        /// array of used fonts
        /// </summary>
        private readonly Dictionary<string, PdfFont> _fonts = new Dictionary<string, PdfFont>();

        //#########################################

        #endregion



        #region FPDF public Methods

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
            _metaData.AddOrUpdate("Title", title);
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
            _metaData.AddOrUpdate("Author", author);
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
            _metaData.AddOrUpdate("Subject", subject);
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
            _metaData.AddOrUpdate("Keywords", keywords);
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
            _metaData.AddOrUpdate("Creator", creator);
        }

        /// <summary>
        /// Defines an alias for the total number of pages. It will be substituted as the document is closed. 
        /// </summary>
        /// <param name="alias">The alias. Default value: {nb}.</param>
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

        #region AddPage

        #region AddPage overloads

        public void AddPage()
        {
            AddPage(null, null, (Size?)null);
        }

        public void AddPage(PageSize pageSize)
        {
            AddPage(null, null, GetSize(pageSize, _k));
        }
        public void AddPage(Orientation orientation)
        {
            AddPage(orientation, null, (Size?)null);
        }

        public void AddPage(Orientation orientation, Rotation rotation)
        {
            AddPage(orientation, rotation, (Size?)null);
        }

        public void AddPage(Orientation orientation, PageSize pageSize)
        {
            AddPage(orientation, null, GetSize(pageSize, _k));
        }

        public void AddPage(Rotation rotation, PageSize pageSize)
        {
            AddPage(null, rotation, GetSize(pageSize, _k));
        }
        public void AddPage(Rotation rotation, Size size)
        {
            AddPage(null, rotation, size);
        }

        public void AddPage(Orientation orientation, Rotation rotation, PageSize pageSize)
        {
            AddPage(orientation, rotation, GetSize(pageSize, _k));
        }

        #endregion

        public void AddPage(Orientation? orientation = null, Rotation? rotation = null, Size? size = null)
        {
            if (rotation == null) rotation = Rotation.Degree0;

            // Start a new page
            if (_state == State.Closed) throw new InvalidOperationException("The document is closed");

            var family = _fontFamily;
            var style = _fontStyle; // we leave the underline in the font style
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
            BeginPage(orientation, size, (Rotation)rotation);
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

        #endregion

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
            const bool isDrawColor = true;
            _drawColor = ConvertColor(isDrawColor, r, g, b);
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
            const bool isDrawColor = false;
            _fillColor = ConvertColor(isDrawColor, r, g, b);
            _colorFlag = _fillColor != _textColor;
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
            const bool isDrawColor = false;
            _textColor = ConvertColor(isDrawColor, r, g, b);
            _colorFlag = _fillColor != _textColor;
        }

        /// <summary>
        /// Returns the length of a string in user unit. A font must be selected. 
        /// </summary>
        /// <param name="s">The string whose length is to be computed. </param>
        /// <returns>The length in user units,</returns>
        public double GetStringWidth(string s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s), "The string must not be NULL.");
            if (_currentFont == null) throw new InvalidOperationException("No font has been set");

            var cw = _currentFont.CharacterWidths;
            var w = 0.0;
            var l = s.Length;
            for (var i = 0; i < l; i++)
            {
                w += cw[s[i]];
            }
            return w * _fontSize / 1000;

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
            string op = style switch
            {
                RectStyle.Draw => "S",
                RectStyle.Fill => "f",
                RectStyle.DrawAndFill => "B",
                _ => throw new NotImplementedException($"No implementation for rect style: {style}"),
            };
            Out(string.Format(
                CultureInfo.InvariantCulture,
                "{0:0.00} {1:0.00} {2:0.00} {3:0.00} re {4}", x * _k, (_h - y) * _k, w * _k, -h * _k, op));
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

        public virtual void Cell(
            double w, double h = 0, string txt = "", Borders border = Borders.NoBorder,
            LineBehavior ln = LineBehavior.ToTheRight, Align align = Align.Left, bool fill = false, string link = "")
        {
            AssertValidBorderCombination(border);

            // Output a cell
            var k = _k;
            if (_y + h > _pageBreakTrigger && !_inHeader && !_inFooter && AcceptPageBreak())
            {
                // Automatic page break
                var x = _x;
                var ws = _ws;
                if (ws > 0)
                {
                    _ws = 0;
                    Out("0 Tw");
                }
                AddPage(_curOrientation, _curRotation, _curPageSize);
                _x = x;
                if (ws > 0)
                {
                    _ws = ws;
                    Out(string.Format(CultureInfo.InvariantCulture, "{0:0.000} Tw", ws * k));
                }
            }

            if (w == 0)
            {
                w = _w - _rMargin - _x;
            }
            var sb = new StringBuilder();
            if (fill || border == Borders.Frame)
            {
                string op;
                if (fill)
                {
                    op = (border == Borders.Frame) ? "B" : "f";
                }
                else
                {
                    op = "S";
                }
                sb.Append(string.Format(
                            CultureInfo.InvariantCulture,
                            "{0:0.00} {1:0.00} {2:0.00} {3:0.00} re {4} ",
                            _x * k, (_h - _y) * k, w * k, -h * k, op));
            }
            var hasSideBorders = border.HasFlag(Borders.Left) | border.HasFlag(Borders.Right) | border.HasFlag(Borders.Top) | border.HasFlag(Borders.Bottom);
            if (hasSideBorders)
            {
                var x = _x;
                var y = _y;
                var format = "{0:0.00} {1:0.00} m {2:0.00} {3:0.00} l S ";
                if (border.HasFlag(Borders.Left))
                {
                    sb.Append(string.Format(CultureInfo.InvariantCulture, format, x * k, (_h - y) * k, x * k, (_h - (y + h)) * k));
                }
                if (border.HasFlag(Borders.Top))
                {
                    sb.Append(string.Format(CultureInfo.InvariantCulture, format, x * k, (_h - y) * k, (x + w) * k, (_h - y) * k));
                }
                if (border.HasFlag(Borders.Right))
                {
                    sb.Append(string.Format(CultureInfo.InvariantCulture, format, (x + w) * k, (_h - y) * k, (x + w) * k, (_h - (y + h)) * k));
                }
                if (border.HasFlag(Borders.Bottom))
                {
                    sb.Append(string.Format(CultureInfo.InvariantCulture, format, x * k, (_h - (y + h)) * k, (x + w) * k, (_h - (y + h)) * k));
                }
            }

            if (!string.IsNullOrEmpty(txt))
            {
                if (_currentFont == null)
                {
                    throw new InvalidOperationException("No font has been set");
                }
                double dx;
                if (align == Align.Right)
                {
                    dx = w - _cMargin - GetStringWidth(txt);
                }
                else if (align == Align.Center)
                {
                    dx = (w - GetStringWidth(txt)) / 2;
                }
                else
                {
                    dx = _cMargin;
                }
                if (_colorFlag)
                {
                    sb.Append($"q {_textColor} ");
                }
                sb.Append(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "BT {0:0.00} {1:0.00} Td ({2}) Tj ET",
                        (_x + dx) * k, (_h - (_y + .5 * h + .3 * _fontSize)) * k, Escape(txt)));

                if (_underline)
                {
                    sb.Append($" {DoUnderline(_currentFont, _x + dx, _y + .5 * h + .3 * _fontSize, txt)}");
                }
                if (_colorFlag)
                {
                    sb.Append(" Q");
                }
                if (!string.IsNullOrEmpty(link))
                {
                    // TODO implement LINK
                    //Link(_x + dx, _y + .5 * h - .5 * _fontSize, GetStringWidth(txt), _fontSize, link);
                }
            }

            if (sb.Length > 0)
            {
                Out(sb.ToString());
            }
            _lasth = h;
            if (ln != LineBehavior.ToTheRight)
            {
                // Go to next line
                _y += h;
                if (ln == LineBehavior.ToTheBeginningOfTheNextLine)
                {
                    _x = _lMargin;
                }
            }
            else
            {
                _x += w;
            }
        }

        public void SetFont(string family, FontStyles style = FontStyles.Regular, double fontSize = 0)
        {
            // Select a font; size given in points
            if (string.IsNullOrWhiteSpace(family))
            {
                family = _fontFamily;
            }
            else
            {
                family = family.ToLower();
            }

            _underline = style.HasFlag(FontStyles.Underline);

            if (fontSize == 0)
            {
                fontSize = _fontSizePt;
            }

            // Test if font is already selected
            if (_fontFamily == family && _fontStyle == style && _fontSizePt == fontSize)
                return;

            // Test if font is already loaded
            var fontKey = GetFontKey(family, style);
            if (!_fonts.ContainsKey(fontKey))
            {
                // Test if one of the core fonts
                if (family == "arial")
                    family = "helvetica";
                if (_coreFonts.Contains(family))
                {
                    if (family == "symbol" || family == "zapfdingbats")
                    {
                        style = FontStyles.Regular;
                    }
                    fontKey = GetFontKey(family, style);
                    if (!_fonts.ContainsKey(fontKey))
                        AddFont(family, style);
                }
                else
                {
                    throw new InvalidOperationException($"Undefined font key: {fontKey}");
                }
            }

            // Select it
            _fontFamily = family;
            _fontStyle = style;
            _fontSizePt = fontSize;
            _fontSize = fontSize / _k;
            _currentFont = _fonts[fontKey];
            if (_page > 0)
            {
                Out(string.Format(
                        CultureInfo.InvariantCulture,
                        "BT /F{0} {1:0.00} Tf ET",
                        _currentFont.I, _fontSizePt));
            }

        }

        public void AddFont(string family, FontStyles style = FontStyles.Regular)
        {
            // TODO 1. attempt: The original font files (php) are ported into C# classes which implement PdfFont

            family = family.ToLower().Replace(" ", "");
            var fontKey = GetFontKey(family, style);
            if (_fonts.ContainsKey(fontKey))
            {
                return;
            }

            var fontName = GetAvailableFonts().Where(f => f.ToLower() == fontKey.ToLower()).FirstOrDefault();
            if (fontName == null) throw new ArgumentException($"The given family and style leads to font, which doesn't exist: {fontKey}");
            var type = Type.GetType($"SharpFPDF.Fonts.{fontName}");
            var font = (PdfFont)Activator.CreateInstance(type);
            font.I = _fonts.Count + 1;

            // TODO implement Embedded Font
            //if (!empty($info['file']))
            //{
            //    // Embedded font
            //    if ($info['type'] == 'TrueType')
            //        $this->FontFiles[$info['file']] = array('length1'=>$info['originalsize']);
            //    else
            //        $this->FontFiles[$info['file']] = array('length1'=>$info['size1'], 'length2'=>$info['size2']);
            //}

            _fonts.Add(fontKey, font);
        }

        public static ICollection<string> GetAvailableFonts()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.UnderlyingSystemType.BaseType == typeof(PdfFont));
            var typeNames = new List<string>();
            foreach (var type in types)
            {
                typeNames.Add(type.Name);
            }
            return typeNames;
        }

        public void OutputToFile(string filename)
        {
            Close();
            if (string.IsNullOrWhiteSpace(filename)) filename = "doc.pdf";
            File.WriteAllText(filename, _buffer.ToString());
        }


        #endregion

        #region InitMethods

        // TODO implement
        private void InitFonts()
        {
            /*
             * In order to handle cp1252 codepages, we need the System.Text.Encoding.CodePages NuGet package
             * and the code line below.
             * https://stackoverflow.com/questions/50858209/system-notsupportedexception-no-data-is-available-for-encoding-1252/58074654#58074654             * 
             */
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


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
            return unit switch
            {
                Unit.pt => 1,
                Unit.mm => 72 / 25.4,
                Unit.cm => 72 / 2.54,
                Unit.inch => 72,
                _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, "Unknown unit"),
            };
        }

        private void InitPageSize(Size size)
        {
            _curPageSize = size;
            _defPageSize = size;
        }

        private void InitPageOrientation(Orientation orientation, Size size)
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

        #region protected methods


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

        protected void BeginPage(Orientation? orientation, Size? size, Rotation rotation)
        {
            _page++;
            _pages.Add(_page, new StringBuilder());
            _pageInfos.Add(_page, new PageInfo());
            _state = State.BeginPage;
            _x = _lMargin;
            _y = _tMargin;
            _fontFamily = string.Empty;

            // Check page size and orientation
            if (orientation == null) orientation = _defOrientation;
            if (size == null) size = _defPageSize;
            if (orientation != _curOrientation || size != _curPageSize)
            {
                // New size or orientation
                if (orientation == Orientation.Portrait)
                {
                    _w = size.Width;
                    _h = size.Height;
                }
                else
                {
                    _w = size.Height;
                    _h = size.Width;
                }
                _wPt = _w * _k;
                _hPt = _h * _k;
                _pageBreakTrigger = _h - _bMargin;
                _curOrientation = (Orientation)orientation;
                _curPageSize = size;
            }
            if (orientation != _defOrientation || size != _defPageSize)
            {
                _pageInfos.Add(_page, new PageInfo() { Size = new Size(_wPt, _hPt) });
            }
            if (rotation != Rotation.Degree0)
            {
                _curRotation = rotation;
                _pageInfos[_page].Rotation = rotation;
            }
        }

        private void EndPage()
        {
            _state = State.EndPage;
        }

        private void EndDoc()
        {
            PutHeader();
            PutPages();
            PutResources();
            // Info
            NewObj();
            Put("<<");
            PutInfo();
            Put(">>");
            Put("endobj");
            // Catalog
            NewObj();
            Put("<<");
            PutCatalog();
            Put(">>");
            Put("endobj");
            // Cross-ref
            var offset = GetOffset();
            Put("xref");
            Put($"0 {_n + 1}");
            Put("0000000000 65535 f ");
            for (var i = 1; i <= _n; i++)
            {
                Put(string.Format(CultureInfo.InvariantCulture, "{0:0000000000} 00000 n ", _offsets[i]));
            }
            // Trailer
            Put("trailer");
            Put("<<");
            PutTrailer();
            Put(">>");
            Put("startxref");
            Put(offset.ToString());
            Put("%%EOF");
            _state = State.Closed;
        }

        private void PutResources()
        {
            // TODO implement PutFonts, ...
            PutFonts();
            //PutImages();
            // Resource dictionary
            NewObj(2);
            Put("<<");
            PutResourceDict();
            Put(">>");
            Put("endobj");
        }

        private void PutResourceDict()
        {
            Put("/ProcSet [/PDF /Text /ImageB /ImageC /ImageI]");
            Put("/Font <<");

            foreach (var font in _fonts)
            {
                Put($"/F{font.Value.I} {font.Value.N} 0 R");
            }
            Put(">>");
            Put("/XObject <<");
            Putxobjectdict();
            Put(">>");
        }

        private void Putxobjectdict()
        {
            // TODO: Implement images

            //foreach ($this->images as $image)
            //$this->_put('/I'.$image['i'].' '.$image['n'].' 0 R');
        }

        private void PutInfo()
        {
            _metaData.AddOrUpdate("Producer", $"SharpFPDF {_SharpFPDF_VERSION}");
            _metaData.AddOrUpdate("CreationDate", $"D:{DateTime.Now:yyyyMMddHHmmss}");
            foreach (var metadata in _metaData)
            {
                Put($"/{metadata.Key} {TextString(metadata.Value)}");
            }
        }

        private void PutCatalog()
        {
            var n = _pageInfos[1].N;
            Put("/Type /Catalog");
            Put("/Pages 1 0 R");
            if (_zoomMode == ZoomMode.Fullpage)
                Put($"/OpenAction [{n} 0 R /Fit]");
            else if (_zoomMode == ZoomMode.Fullwidth)
                Put($"/OpenAction [{n} 0 R /FitH null]");
            else if (_zoomMode == ZoomMode.Real)
                Put($"/OpenAction [{n} 0 R /XYZ null null 1]");
            else if (_zoomMode == null && _zoomFactor != null)
                Put($"/OpenAction [{n} 0 R /XYZ null null {(_zoomFactor / 100):0.00}]");
            switch (_layoutMode)
            {
                case LayoutMode.Default:
                    break;
                case LayoutMode.Single:
                    Put("/PageLayout /SinglePage");
                    break;
                case LayoutMode.Continuous:
                    Put("/PageLayout /OneColumn");
                    break;
                case LayoutMode.Two:
                    Put("/PageLayout /TwoColumnLeft");
                    break;
                default:
                    throw new NotImplementedException($"No implementation for layoutMode: {_layoutMode}");
            }
        }

        private void PutTrailer()
        {
            Put($"/Size {_n + 1}");
            Put($"/Root {_n} 0 R");
            Put($"/Info {_n - 1} 0 R");
        }

        private static string TextString(string value)
        {
            // Format a text string
            // We still have UTF16 strings
            return $"({Escape(value)})";
        }

        private static string Escape(string s)
        {
            // Escape special characters
            if (s.Contains("(") || s.Contains(")") || s.Contains("\\") || s.Contains("\r"))
            {
                return s.Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)").Replace("\r", "\\r");
            }
            else
            {
                return s;
            }
        }

        private void PutHeader()
        {
            Put($"%PDF-{_pdfVersion}");
        }

        private void PutPages()
        {
            var nb = _page;
            for (int n = 1; n <= nb; n++)
            {
                _pageInfos[n].N = _n + 1 + 2 * (n - 1);
            }
            for (int n = 1; n <= nb; n++)
            {
                PutPage(n);
            }
            // Pages root
            NewObj(1);
            Put("<</Type /Pages");
            var kids = new StringBuilder();
            kids.Append("/Kids [");
            for (int n = 1; n <= nb; n++)
            {
                kids.Append($"{_pageInfos[n].N} 0 R ");
            }
            kids.Append("]");
            Put(kids.ToString());
            Put($"/Count {nb}");
            if (_defOrientation == Orientation.Portrait)
            {
                _w = _defPageSize.Width;
                _h = _defPageSize.Height;
            }
            else
            {
                _w = _defPageSize.Height;
                _h = _defPageSize.Width;
            }
            Put(string.Format(CultureInfo.InvariantCulture, "/MediaBox [0 0 {0:0.00} {1:0.00}]", _w * _k, _h * _k));
            Put(">>");
            Put("endobj");
        }

        private void PutPage(int n)
        {
            NewObj();
            Put("<</Type /Page");
            Put("/Parent 1 0 R");

            if (_pageInfos[n].Size != null)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Put(string.Format(CultureInfo.InvariantCulture, "/MediaBox [0 0 {0:0.00} {1:0.00}]", _pageInfos[n].Size.Width, _pageInfos[n].Size.Height));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            if (_pageInfos[n].Rotation != null)
            {
                Put($"/Rotate {_pageInfos[n].Rotation}");
            }
            Put("/Resources 2 0 R");

            // TODO implement Links
            /*
             * 
                if(isset($this->PageLinks[$n]))
                {
                    // Links
                    $annots = '/Annots [';
                    foreach($this->PageLinks[$n] as $pl)
                    {
                        $rect = sprintf('%.2F %.2F %.2F %.2F',$pl[0],$pl[1],$pl[0]+$pl[2],$pl[1]-$pl[3]);
                        $annots .= '<</Type /Annot /Subtype /Link /Rect ['.$rect.'] /Border [0 0 0] ';
                        if(is_string($pl[4]))
                            $annots .= '/A <</S /URI /URI '.$this->_textstring($pl[4]).'>>>>';
                        else
                        {
                            $l = $this->links[$pl[4]];
                            if(isset($this->PageInfo[$l[0]]['size']))
                                $h = $this->PageInfo[$l[0]]['size'][1];
                            else
                                $h = ($this->DefOrientation=='P') ? $this->DefPageSize[1]*$this->k : $this->DefPageSize[0]*$this->k;
                            $annots .= sprintf('/Dest [%d 0 R /XYZ 0 %.2F null]>>',$this->PageInfo[$l[0]]['n'],$h-$l[1]*$this->k);
                        }
                    }
                    $this->_put($annots.']');
                }
             */

            if (_withAlpha)
            {
                Put("/Group <</Type /Group /S /Transparency /CS /DeviceRGB>>");
            }
            Put($"/Contents {_n + 1} 0 R>>");
            Put("endobj");
            // Page content
            if (!string.IsNullOrEmpty(_aliasNbPages))
            {
                _pages[n] = _pages[n].Replace(_aliasNbPages, _page.ToString());
            }
            PutStreamObject(_pages[n]);
        }

        private void PutStreamObject(StringBuilder sb)
        {

            if (_compress)
            {
                // TODO implement compression

                throw new NotImplementedException("Compression is not implemented yet.");
                //$entries = '/Filter /FlateDecode ';
                //$data = gzcompress($data);
            }
            else
            {
                NewObj();
                Put($"<</Length {sb.Length}>>");
                PutStream(sb);
                Put("endobj");
            }
        }

        private void PutStream(StringBuilder sb)
        {
            Put("stream");
            Put(sb.ToString());
            Put("endstream");
        }


        private void PutFonts()
        {
            // TODO   Implement FontFiles

            foreach (var keyValuePair in _fonts)
            {
                var k = keyValuePair.Key;
                var font = keyValuePair.Value;

                // TODO The "diff" part?
                // Encoding
                if (!string.IsNullOrEmpty(font.Diff))
                {
                    if (font.Encoding != null && !_encodings.ContainsKey((string)font.Encoding))
                    {
                        NewObj();
                        Put($"<</Type /Encoding /BaseEncoding /WinAnsiEncoding /Differences [{font.Diff}]>>");
                        Put("endobj");
                        _encodings.Add((string)font.Encoding, _n);
                    }
                }
                // ToUnicode CMap
                string cmapKey = string.Empty;
                if (font.Uv != null)
                {
                    if (font.Encoding != null)
                    {
                        cmapKey = font.Encoding;
                    }
                    else
                    {
                        cmapKey = font.Name;
                    }
                    if (!_cmaps.ContainsKey(cmapKey))
                    {
                        var cmap = ToUnicodeCmap(font.Uv);
                        PutStreamObject(cmap);
                        _cmaps.Add(cmapKey, _n);
                    }
                }
                // Font object
                _fonts[k].N = _n + 1;
                var type = font.Type;
                var name = font.Name;
                if (font.Subsetted)
                    name = $"AAAAAA+{name}";
                if (type == "Core")
                {
                    // Core font
                    NewObj();
                    Put("<</Type /Font");
                    Put($"/BaseFont /{name}");
                    Put("/Subtype /Type1");
                    if (name != "Symbol" && name != "ZapfDingbats")
                    {
                        Put("/Encoding /WinAnsiEncoding");
                    }
                    if (font.Uv != null)
                    {
                        Put($"/ToUnicode {_cmaps[cmapKey]} 0 R");
                    }
                    Put(">>");
                    Put("endobj");
                }
                else if (type == "Type1" || type == "TrueType")
                {
                    // Additional Type1 or TrueType/OpenType font
                    NewObj();
                    Put("<</Type /Font");
                    Put($"/BaseFont /{name}");
                    Put($"/Subtype /{type}");
                    Put("/FirstChar 32 /LastChar 255");
                    Put($"/Widths {_n + 1} 0 R");
                    Put($"/FontDescriptor {_n + 2} 0 R");
                    if (font.Diff != null && font.Encoding != null) // 
                    {
                        Put($"/Encoding {_encodings[font.Encoding]} 0 R");
                    }
                    else
                    {
                        Put("/Encoding /WinAnsiEncoding");
                    }
                    if (font.Uv != null)
                    {
                        Put($"/ToUnicode {_cmaps[cmapKey]} 0 R");
                    }
                    Put(">>");
                    Put("endobj");
                    // Widths
                    NewObj();
                    var cw = font.CharacterWidths;
                    var sb = new StringBuilder();
                    sb.Append("[");
                    for (var i = 32; i <= 255; i++)
                        sb.Append($"{cw[(char)i]} ");
                    Put(sb.Append("]").ToString());
                    Put("endobj");
                    // Descriptor
                    NewObj();
                    sb.Clear();
                    sb.Append($"<</Type /FontDescriptor /FontName /{name}");
                    foreach (var kvp in font.Desc)
                    {
                        sb.Append($" /{kvp.Key} {kvp.Value}");
                    }
                    if (!string.IsNullOrEmpty(font.File))
                    {
                        sb.Append($" /FontFile{(type == "Type1" ? "" : "2")} {_fontFiles[font.File].N} 0 R");
                    }
                    Put(sb.Append(">>").ToString());
                    Put("endobj");
                }
                else
                {

                    throw new NotImplementedException($"The implementation for {type} is not yet implemented.");
                    //// Allow for additional types
                    //$mtd = '_put'.strtolower($type);
                    //if (!method_exists($this,$mtd))
                    //    $this->Error('Unsupported font type: '.$type);
                    //$this->$mtd($font);
                }
            }
        }

        private void NewObj(int? n = null)
        {
            // Begin a new object
            if (n == null)
            {
                n = ++_n;
            }

            _offsets.Add((int)n, GetOffset());
            Put($"{n} 0 obj");
        }


        #endregion

        #region Helper

        protected Size GetSize(PageSize pageSize, double k)
        {
            if (_standardPageSizes.ContainsKey(pageSize))
            {
                var uncorrectedsize = _standardPageSizes[pageSize];
                var size = new Size(uncorrectedsize.Width / k, uncorrectedsize.Height / k);
                return size;
            }

            throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "The page size has an unexpected value.");
        }

        /// <summary>
        /// Convert the RGB byte values into the needed string.
        /// </summary>
        /// <param name="r">red</param>
        /// <param name="g">green</param>
        /// <param name="b">blue</param>
        /// <returns>The color string.</returns>
        private static string ConvertColor(bool isDrawColor, byte r, byte? g, byte? b)
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

            return isDrawColor ? color : color.ToLowerInvariant();
        }


        private static void AssertValidBorderCombination(Borders border)
        {
            Borders couldBeCombined = Borders.Left | Borders.Right | Borders.Top | Borders.Bottom;
            Borders couldNotBeCombined = Borders.NoBorder | Borders.Frame;
            if (border.HasFlag(couldNotBeCombined) && border.HasFlag(couldBeCombined))
            {
                throw new ArgumentException("Invalid border combination. Only left, right, top and bottom can be combined.", nameof(border));
            }
            if (border.HasFlag(Borders.NoBorder) && border.HasFlag(Borders.Frame))
            {
                throw new ArgumentException("Invalid border combination. You can not combine  'no border' and 'frame'.", nameof(border));
            }
        }

        private string DoUnderline(PdfFont currentFont, double x, double y, string txt)
        {
            // Underline text
            var up = currentFont.UnderlinePosition;
            var ut = currentFont.UnderlineThickness;
            var spaceCount = Regex.Matches(string.Empty, Regex.Escape(txt)).Count;
            var w = GetStringWidth(txt) + _ws * spaceCount;

            return string.Format(
                        CultureInfo.InvariantCulture,
                        "{0:0.00} {1:0.00} {2:0.00} {3:0.00} re f",
                        x * _k, (_h - (y - up / 1000 * _fontSize)) * _k, w * _k, -ut / 1000 * _fontSizePt);
        }

        private static string GetFontKey(string family, FontStyles style)
        {
            var sb = new StringBuilder();
            sb.Append(family);
            if (style.HasFlag(FontStyles.Bold)) sb.Append("B");
            if (style.HasFlag(FontStyles.Italic)) sb.Append("I");
            return sb.ToString();
        }

        #endregion

        private static StringBuilder ToUnicodeCmap(Dictionary<int, int[]> uv)
        {
            var ranges = new StringBuilder();
            var nbr = 0;
            var chars = new StringBuilder();
            var nbc = 0;

            foreach (var keyValuePair in uv)
            {
                if (keyValuePair.Value.Length > 1)
                {
                    // 			$ranges .= sprintf("<%02X> <%02X> <%04X>\n",$c,$c+$v[1]-1,$v[0]);

                    ranges.AppendNewLine(string.Format("<{0:X2}> <{1:X2}> <{2:X4}>", keyValuePair.Key, keyValuePair.Key + keyValuePair.Value[1] - 1, keyValuePair.Value[0]));
                    nbr++;
                }
                else
                {
                    chars.AppendNewLine(string.Format("<{0:X2}> <{1:X4}>", keyValuePair.Key, keyValuePair.Value[0]));
                    nbc++;
                }
            }
            var sb = new StringBuilder();
            sb.AppendNewLine("/CIDInit /ProcSet findresource begin");
            sb.AppendNewLine("12 dict begin");
            sb.AppendNewLine("begincmap");
            sb.AppendNewLine("/CIDSystemInfo");
            sb.AppendNewLine("<</Registry (Adobe)");
            sb.AppendNewLine("/Ordering (UCS)");
            sb.AppendNewLine("/Supplement 0");
            sb.AppendNewLine(">> def");
            sb.AppendNewLine("/CMapName /Adobe-Identity-UCS def");
            sb.AppendNewLine("/CMapType 2 def");
            sb.AppendNewLine("1 begincodespacerange");
            sb.AppendNewLine("<00> <FF>");
            sb.AppendNewLine("endcodespacerange");
            if (nbr > 0)
            {
                sb.AppendNewLine($"{nbr} beginbfrange");
                sb.Append(ranges);
                sb.AppendNewLine("endbfrange");
            }
            if (nbc > 0)
            {
                sb.AppendNewLine($"{nbc} beginbfchar");
                sb.Append(chars);
                sb.AppendNewLine("endbfchar");
            }
            sb.AppendNewLine("endcmap");
            sb.AppendNewLine("CMapName currentdict /CMap defineresource pop");
            sb.AppendNewLine("end");
            sb.AppendNewLine("end");
            return sb;
        }
    }
}
