# SharpFPDF
My C# port of the famous FPDF library, originally written in PHP by Olivier Plathey.

Starting from FPDF Version: 1.82, 2019-12-07


## Acutal work

Methods from FPDF 1.82 Reference Manual

## ported
- AcceptPageBreak - accept or not automatic page break
- AliasNbPages - define an alias for number of pages
- Error - fatal error (=> not implemented, we throw exceptions)
- Footer - page footer
- GetPageHeight - get current page height
- GetPageWidth - get current page width
- Header - page header
- Ln - line break
- PageNo - page number
- SetAuthor - set the document author
- SetAutoPageBreak - set the automatic page breaking mode
- SetCreator - set document creator
- SetCompression - turn compression on or off
- SetDrawColor - set drawing color
- SetDisplayMode - set display mode
- SetFillColor - set filling color
- SetKeywords - associate keywords with document
- SetLeftMargin - set left margin
- SetLineWidth - set line width
- SetMargins - set margins
- SetRightMargin - set right margin
- SetSubject - set document subject
- SetTextColor - set text color
- SetTitle - set document title
- SetTopMargin - set top margin

## in progress
- __construct - constructor  (font settings)
- AddPage - add a new page  (Need good solution for mixed type size)

## todo
- AddFont - add a new font
- AddLink - create an internal link
- Cell - print a cell
- Close - terminate the document
- GetStringWidth - compute string length
- GetX - get current x position
- GetY - get current y position
- Image - output an image
- Line - draw a line
- Link - put a link
- Ln - line break
- MultiCell - print text with line breaks
- Output - save or send the document
- Rect - draw a rectangle
- SetFont - set font
- SetFontSize - set font size
- SetLink - set internal link destination
- SetX - set current x position
- SetXY - set current x and y positions
- SetY - set current y position and optionally reset x
- Text - print a string
- Write - print flowing text
