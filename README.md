# SharpFPDF
My C# port of the famous FPDF library, originally written in PHP by Olivier Plathey.

Starting from FPDF Version: 1.82, 2019-12-07


## Acutal work

Methods from FPDF 1.82 Reference Manual

## ported
- AcceptPageBreak - accept or not automatic page break
- AddPage - add a new page
- AliasNbPages - define an alias for number of pages
- Close - terminate the document
- Error - fatal error (=> not implemented, we throw exceptions)
- Footer - page footer
- GetPageHeight - get current page height
- GetPageWidth - get current page width
- GetStringWidth - compute string length
- GetX - get current x position
- GetY - get current y position
- Header - page header
- Line - draw a line
- Ln - line break
- PageNo - page number
- Rect - draw a rectangle
- SetAuthor - set the document author
- SetAutoPageBreak - set the automatic page breaking mode
- SetCreator - set document creator
- SetCompression - turn compression on or off
- SetDrawColor - set drawing color
- SetDisplayMode - set display mode
- SetFillColor - set filling color
- SetFont - set font
- SetKeywords - associate keywords with document
- SetLeftMargin - set left margin
- SetLineWidth - set line width
- SetMargins - set margins
- SetRightMargin - set right margin
- SetSubject - set document subject
- SetTextColor - set text color
- SetTitle - set document title
- SetTopMargin - set top margin
- SetX - set current x position
- SetXY - set current x and y positions
- SetY - set current y position and optionally reset x


## in progress
- __construct - constructor  (font settings)
- Cell - print a cell  (link support missing)

## todo
- AddFont - add a new font
- AddLink - create an internal link
- Image - output an image
- Link - put a link
- MultiCell - print text with line breaks
- Output - save or send the document
- SetFontSize - set font size
- SetLink - set internal link destination
- Text - print a string
- Write - print flowing text
