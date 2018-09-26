﻿using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using System;

namespace convert
{
    class Program
    {
        static List<List<string>> txtPatentlist = new List<List<string>>();
        static void Main(string[] args)
        {
            string path = "C:/Users/12773/Desktop/DemoHtml.html";
            Program.ParseHtml();
            ProcessTranslate();

            string document = @"C:\Users\12773\Desktop\demo.docx";
        }

        

        public static void SetTitle(string filepath)
        {
            WordprocessingDocument wordDocument = WordprocessingDocument.Open(filepath, true);

            // Add a main document part. 
            // Create the document structure and add some text.
            Body body = wordDocument.MainDocumentPart.Document.Body;
            Paragraph para = body.AppendChild(new Paragraph());

            Run run = para.AppendChild(new Run());
            run.AppendChild(new Text("Executive Summary"));
            if (para.Elements<ParagraphProperties>().Count() == 0)
                para.PrependChild<ParagraphProperties>(new ParagraphProperties());

            // Get the ParagraphProperties element of the paragraph.
            ParagraphProperties pPr = para.Elements<ParagraphProperties>().First();

            // Set the value of ParagraphStyleId to "Heading3".
            pPr.ParagraphStyleId = new ParagraphStyleId() { Val = "Heading1" };
            wordDocument.Close();

        }

        // Create a new paragraph style with the specified style ID, primary style name, and aliases and 
        // add it to the specified style definitions part.
        public static void CreateAndAddParagraphStyle(StyleDefinitionsPart styleDefinitionsPart,
            string styleid, string stylename, string aliases = "")
        {
            // Access the root element of the styles part.
            Styles styles = styleDefinitionsPart.Styles;
            if (styles == null)
            {
                styleDefinitionsPart.Styles = new Styles();
                styleDefinitionsPart.Styles.Save();
            }

            // Create a new paragraph style element and specify some of the attributes.
            Style style = new Style()
            {
                Type = StyleValues.Paragraph,
                StyleId = styleid,
                CustomStyle = true,
                Default = false
            };

            // Create and add the child elements (properties of the style).
            Aliases aliases1 = new Aliases() { Val = aliases };
            AutoRedefine autoredefine1 = new AutoRedefine() { Val = OnOffOnlyValues.Off };
            BasedOn basedon1 = new BasedOn() { Val = "Normal" };
            LinkedStyle linkedStyle1 = new LinkedStyle() { Val = "OverdueAmountChar" };
            Locked locked1 = new Locked() { Val = OnOffOnlyValues.Off };
            PrimaryStyle primarystyle1 = new PrimaryStyle() { Val = OnOffOnlyValues.On };
            StyleHidden stylehidden1 = new StyleHidden() { Val = OnOffOnlyValues.Off };
            SemiHidden semihidden1 = new SemiHidden() { Val = OnOffOnlyValues.Off };
            StyleName styleName1 = new StyleName() { Val = stylename };
            NextParagraphStyle nextParagraphStyle1 = new NextParagraphStyle() { Val = "Normal" };
            UIPriority uipriority1 = new UIPriority() { Val = 1 };
            UnhideWhenUsed unhidewhenused1 = new UnhideWhenUsed() { Val = OnOffOnlyValues.On };
            if (aliases != "")
                style.Append(aliases1);
            style.Append(autoredefine1);
            style.Append(basedon1);
            style.Append(linkedStyle1);
            style.Append(locked1);
            style.Append(primarystyle1);
            style.Append(stylehidden1);
            style.Append(semihidden1);
            style.Append(styleName1);
            style.Append(nextParagraphStyle1);
            style.Append(uipriority1);
            style.Append(unhidewhenused1);

            // Create the StyleRunProperties object and specify some of the run properties.
            StyleRunProperties styleRunProperties1 = new StyleRunProperties();
            Bold bold1 = new Bold();
            Color color1 = new Color() { ThemeColor = ThemeColorValues.Accent2 };
            RunFonts font1 = new RunFonts() { Ascii = "Lucida Console" };
            Italic italic1 = new Italic();
            ParagraphBorders pb = new ParagraphBorders(new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 24, Color = "FF0000" } ,
                new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 24, Color = "FF0000" },
                new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 24, Color = "FF0000" },
                new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 24, Color = "FF0000" });
            // Specify a 12 point size.
            FontSize fontSize1 = new FontSize() { Val = "24" };
            styleRunProperties1.Append(bold1);
            styleRunProperties1.Append(color1);
            styleRunProperties1.Append(font1);
            styleRunProperties1.Append(fontSize1);
            styleRunProperties1.Append(italic1);
            styleRunProperties1.Append(pb);

            // Add the run properties to the style.
            style.Append(styleRunProperties1);

            // Add the style to the styles part.
            styles.Append(style);
        }

        // Add a StylesDefinitionsPart to the document.  Returns a reference to it.
        public static StyleDefinitionsPart AddStylesPartToPackage(WordprocessingDocument doc)
        {
            StyleDefinitionsPart part;
            part = doc.MainDocumentPart.AddNewPart<StyleDefinitionsPart>();
            Styles root = new Styles();
            root.Save(part);
            return part;
        }

        public static void BookMark(string filepath)
        {
            WordprocessingDocument doc =
                WordprocessingDocument.Open(filepath, true);
            Body body = doc.MainDocumentPart.Document.Body;
            Paragraph paragraph1 = doc.MainDocumentPart.Document.Body.Elements<Paragraph>().First();
            BookmarkStart bookmarkStart = new BookmarkStart() { Name = "p1", Id = "1" };
            BookmarkEnd bookmarkEnd = new BookmarkEnd() { Id = "1" };
            body.InsertBefore<BookmarkStart>(bookmarkStart, paragraph1);
            body.InsertAfter<BookmarkEnd>(bookmarkEnd, paragraph1);

            Paragraph paragraph3 = doc.MainDocumentPart.Document.Body.Elements<Paragraph>().ElementAt(2);
            Run run2 = new Run() { RsidRunAddition = "009B0519" };
            FieldChar fieldChar1 = new FieldChar() { FieldCharType = FieldCharValues.Begin };
            run2.Append(fieldChar1);
            Run run3 = new Run() { RsidRunAddition = "009B0519" };
            FieldCode fieldCode1 = new FieldCode() { Space = SpaceProcessingModeValues.Preserve };
            fieldCode1.Text = " REF p1 \\h "; //Link to bookmark p1
            run3.Append(fieldCode1);
            Run run4 = new Run() { RsidRunAddition = "009B0519" };
            FieldChar fieldChar2 = new FieldChar() { FieldCharType = FieldCharValues.Separate };
            run4.Append(fieldChar2);
            Run run5 = new Run() { RsidRunAddition = "009B0519" };
            Text text2 = new Text();
            text2.Text = "Link To Paragraph1";
            run5.Append(text2);
            Run run6 = new Run();
            FieldChar fieldChar3 = new FieldChar() { FieldCharType = FieldCharValues.End };
            run6.Append(fieldChar3);
            paragraph3.Append(run2);
            paragraph3.Append(run3);
            paragraph3.Append(run4);
            paragraph3.Append(run5);
            paragraph3.Append(run6);

            string parastyleid = "OverdueAmountPara";
            if (paragraph3.Elements<ParagraphProperties>().Count() == 0)
            {
                paragraph3.PrependChild<ParagraphProperties>(new ParagraphProperties());
            }

            // Get a reference to the ParagraphProperties object.
            ParagraphProperties pPr = paragraph3.ParagraphProperties;

            // If a ParagraphStyleId object doesn't exist, create one.
            if (pPr.ParagraphStyleId == null)
                pPr.ParagraphStyleId = new ParagraphStyleId();

            // Set the style of the paragraph.
            pPr.ParagraphStyleId.Val = parastyleid;
            doc.Close();
        }

        public static void ParseHtml(/*string filepath*/)
        {
            //HtmlDocument doc = new HtmlDocument();
            //doc.Load(filepath);
            string document = @"C:\Users\12773\Desktop\demo.docx";
            var html =
        @"<body><h1><b>bold</b> heading</h1><p>This is <u>italic</u> paragraph</p><img src='C:\Users\12773\Desktop\aa.png'/><table><thead><tr><th>Item</th><th style='text-align:right'>Value</th></tr></thead><tbody><tr><td>Computer</td><td style='text-align:right'>$1600</td></tr><tr><td>Phone</td><td style='text-align:right'>$12</td></tr><tr><td>Pipe</td><td style='text-align:right'>$1</td></tr></tbody></table><ul><li>List1</li><li>List2<ul><li>List2-1</li></ul></li><li>List3</li></ul><pre><code class='lang - javascript'>function test() {console.log('Hello world!');}</code></pre></body>";

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//body");
            Node n1 = new Node();
            n1.setNodename("body");
            traverse(node, n1);
            HtmlConvert(n1);
        }

        public static void traverse(HtmlNode node, Node pnode)
        {
            foreach (var n in node.ChildNodes)
            {
                Node tnode = new Node();
                var nodename = n.Name;
                tnode.setNodename(nodename);
                tnode.setParent(pnode);
                pnode.AddChild(tnode);
                if (nodename == "h1" || nodename == "p" || nodename == "img" || nodename == "table" || (nodename == "ul" && tnode.getParent().getNodename() != "li" || nodename == "pre"))
                {
                    Node teminal = new Node();
                    teminal.setNodename("\n");
                    pnode.AddChild(teminal);
                }
                if (nodename == "img")
                {
                    tnode.setSrc(n.Attributes["src"].Value);
                }
                if (nodename == "#text")
                    tnode.setText(n.InnerHtml);
                else
                    traverse(n, tnode);
            }
        }

        public static void HtmlConvert(Node n1)
        {
            string document = @"C:\Users\12773\Desktop\demo.docx";
            string nodename = n1.getNodename();
            List<Node> nlist = n1.getChilds();
            int count = nlist.Count;
            switch (nodename)
            {
                case "body":
                    for (int i = 0; i < count; i++)
                        HtmlConvert(nlist[i]);
                    break;

                case "h1":
                    for (int i = 0; i < count; i++)
                        HtmlConvert(nlist[i]);
                    break;

                case "b":
                    for (int i = 0; i < count; i++)
                        HtmlConvert(nlist[i]);
                    break;

                case "#text":
                    string txt = n1.getText();
                    GetTxtParentList(n1);
                    break;

                case "img":
                    string src = n1.getSrc();
                    List<string> imglist = new List<string>();
                    imglist.Add("img");
                    imglist.Add(src);
                    txtPatentlist.Add(imglist);
                    break;

                case "\n":
                    List<string> taglist = new List<string>();
                    taglist.Add("endtag");
                    txtPatentlist.Add(taglist);
                    break;

                default:
                    for (int i = 0; i < count; i++)
                        HtmlConvert(nlist[i]);
                    break;
            }            
        }

        public static void GetTxtParentList(Node n)
        {
            List<string> plist = new List<string>();
            plist.Add(n.getText());
            while (n.getParent().getNodename() != "body")
            {
                plist.Add(n.getParent().getNodename());
                if (n.getParent().getNodename() == "tr")
                {
                    int colcount = n.getParent().getChilds().Count();
                    plist.Add(colcount.ToString());
                }
                
                n = n.getParent();
            }
            txtPatentlist.Add(plist);
        }

        public static void ProcessTranslate()
        {
            string document = @"C:\Users\12773\Desktop\demo.docx";
            int begintag = 0;
            int endtag = 0;
            int listcount = 1;
            while (begintag < txtPatentlist.Count())
            {
                endtag = GetInterval(begintag);

                if (txtPatentlist[begintag][0] == "img")
                {
                    InsertAPicture(document, txtPatentlist[begintag][1]);
                }

                else if (txtPatentlist[begintag].Contains("table"))
                {
                    int trindex = txtPatentlist[begintag].IndexOf("tr") + 1;
                    int colcount = Int32.Parse(txtPatentlist[begintag][trindex]);
                    int rowcount = (endtag - begintag) / colcount;
                    CreateTable(document, colcount, rowcount, begintag);
                }

                else if (txtPatentlist[begintag].Contains("ul"))
                {
                    WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(document, true);
                    Body body = wordprocessingDocument.MainDocumentPart.Document.Body;

                    for (; begintag < endtag; begintag++)
                    {
                        int level = -1;
                        string content = txtPatentlist[begintag][0];
                        foreach (string s in txtPatentlist[begintag])
                        {
                            if (s == "ul")
                                level += 1;
                        }
                        AppendListItem(body, content, level, listcount, 0);
                        wordprocessingDocument.MainDocumentPart.Document.Save();
                    }
                    listcount++;
                    wordprocessingDocument.Close();                                 
                }

                else if (txtPatentlist[begintag].Contains("pre"))
                {
                    WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(document, true);
                    Body body = wordprocessingDocument.MainDocumentPart.Document.Body;
                    Paragraph para = body.AppendChild(new Paragraph());

                    Run run = para.AppendChild(new Run());
                    run.AppendChild(new Text(txtPatentlist[begintag][0]));

                    //Border border = new Border();
                    //ParagraphProperties ppr = new ParagraphProperties();
                    //ParagraphBorders pbdr = new ParagraphBorders();
                    //TopBorder tb = new TopBorder() { Size = 24, Space = 1, Color = "FF0000" };
                    //BottomBorder bb = new BottomBorder() { Size = 24, Space = 1, Color = "FF0000" };
                    //LeftBorder lb = new LeftBorder() { Size = 24, Space = 1, Color = "FF0000" };
                    //RightBorder rb = new RightBorder() { Size = 24, Space = 1, Color = "FF0000" };
                    //pbdr.AppendChild(tb);
                    //pbdr.AppendChild(bb);
                    //pbdr.AppendChild(lb);
                    //pbdr.AppendChild(rb);
                    //ppr.AppendChild(pbdr);

                    //para.AppendChild(ppr);
                    //ApplyStyleToParagraph(wordprocessingDocument, "code", "code", para);
                }

                else
                {
                    WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(document, true);
                    Body body = wordprocessingDocument.MainDocumentPart.Document.Body;
                    Paragraph para = body.AppendChild(new Paragraph());

                    for (; begintag < endtag; begintag++)
                    {
                        Run run = para.AppendChild(new Run());
                        if (txtPatentlist[begintag][0] != "endtag")
                        {
                            run.AppendChild(new Text(txtPatentlist[begintag][0] + " "));
                        }
                        foreach (string tag in txtPatentlist[begintag])
                        {
                            switch (tag)
                            {
                                case "b":
                                    SetBoldFont(run, wordprocessingDocument);
                                    break;
                                case "u":
                                    SetItalic(run, wordprocessingDocument);
                                    break;
                                case "strong":
                                    SetBoldFont(run, wordprocessingDocument);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    wordprocessingDocument.Close();
                }
                                
                begintag = endtag + 1;
            }
          
        }

        public static int GetInterval(int i)
        {
            int endtag = 0;
            for (; i < txtPatentlist.Count(); i++)
            {
                if (txtPatentlist[i].Contains("endtag"))
                {
                    endtag = i;
                    return endtag;
                }

            }
            return endtag;
        }

        public static void ApplyStyleToParagraph(WordprocessingDocument doc, string styleid, string stylename, Paragraph p)
        {
            if (p.Elements<ParagraphProperties>().Count() == 0)
            {
                p.PrependChild<ParagraphProperties>(new ParagraphProperties());
            }
            ParagraphProperties pPr = p.Elements<ParagraphProperties>().First();

            StyleDefinitionsPart part = doc.MainDocumentPart.StyleDefinitionsPart;
            if (part == null)
            {
                part = AddStylesPartToPackage(doc);
                AddNewStyle(part, styleid, stylename);
            }
            else
            {
                // If the style is not in the document, add it.
                if (IsStyleIdInDocument(doc, styleid) != true)
                {
                    // No match on styleid, so let's try style name.
                    string styleidFromName = GetStyleIdFromStyleName(doc, stylename);
                    if (styleidFromName == null)
                    {
                        AddNewStyle(part, styleid, stylename);
                    }
                    else
                        styleid = styleidFromName;
                }
            }

            pPr.ParagraphStyleId = new ParagraphStyleId() { Val = styleid };
        }

        public static bool IsStyleIdInDocument(WordprocessingDocument doc, string styleid)
        {
            // Get access to the Styles element for this document.
            Styles s = doc.MainDocumentPart.StyleDefinitionsPart.Styles;

            // Check that there are styles and how many.
            int n = s.Elements<Style>().Count();
            if (n == 0)
                return false;

            // Look for a match on styleid.
            Style style = s.Elements<Style>()
                .Where(st => (st.StyleId == styleid) && (st.Type == StyleValues.Paragraph))
                .FirstOrDefault();
            if (style == null)
                return false;

            return true;
        }

        public static string GetStyleIdFromStyleName(WordprocessingDocument doc, string styleName)
        {
            StyleDefinitionsPart stylePart = doc.MainDocumentPart.StyleDefinitionsPart;
            string styleId = stylePart.Styles.Descendants<StyleName>()
                .Where(s => s.Val.Value.Equals(styleName) &&
                    (((Style)s.Parent).Type == StyleValues.Paragraph))
                .Select(n => ((Style)n.Parent).StyleId).FirstOrDefault();
            return styleId;
        }

        public static void AddNewStyle(StyleDefinitionsPart styleDefinitionsPart, string styleid, string stylename)
        {
            Styles styles = styleDefinitionsPart.Styles;
            Style style = new Style() {
                Type = StyleValues.Paragraph,
                StyleId = styleid,
                CustomStyle = true
            };
            StyleName styname = new StyleName() { Val = stylename };
            BasedOn basedOn1 = new BasedOn() { Val = "Normal" };
            style.Append(styname);
            style.Append(basedOn1);

            

        }

        public static void OpenAndAddTextToWordDocument(string filepath, string txt1)
        {
            // Open a WordprocessingDocument for editing using the filepath.
            WordprocessingDocument wordprocessingDocument =
                WordprocessingDocument.Open(filepath, true);

            // Assign a reference to the existing document body.
            Body body = wordprocessingDocument.MainDocumentPart.Document.Body;

            // Add new text.
            Paragraph para = body.AppendChild(new Paragraph());
            Run run = para.AppendChild(new Run());
            run.AppendChild(new Text(txt1));

            wordprocessingDocument.Close();
        }

        public static void SetBoldFont(Run r, WordprocessingDocument wordprocessingDocument)
        {           
            RunProperties rPr = new RunProperties();
            Bold bd = new Bold();
            rPr.AppendChild(bd);

            r.PrependChild<RunProperties>(rPr);
            wordprocessingDocument.MainDocumentPart.Document.Save();
        }

        public static void SetItalic(Run r, WordprocessingDocument wordprocessingDocument)
        {
            RunProperties rPr = new RunProperties();
            Italic it = new Italic();
            rPr.AppendChild(it);

            r.PrependChild<RunProperties>(rPr);
            wordprocessingDocument.MainDocumentPart.Document.Save();
        }

        public static void SetUnderline(Run r, WordprocessingDocument wordprocessingDocument)
        {
            RunProperties rPr = new RunProperties();
            Underline ul = new Underline();
            rPr.AppendChild(ul);

            r.PrependChild<RunProperties>(rPr);
            wordprocessingDocument.MainDocumentPart.Document.Save();             
        }

        public static void InsertAPicture(string document, string fileName)
        {
            using (WordprocessingDocument wordprocessingDocument =
                WordprocessingDocument.Open(document, true))
            {
                MainDocumentPart mainPart = wordprocessingDocument.MainDocumentPart;

                ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

                using (FileStream stream = new FileStream(fileName, FileMode.Open))
                {
                    imagePart.FeedData(stream);
                }

                AddImageToBody(wordprocessingDocument, mainPart.GetIdOfPart(imagePart));
            }
        }

        private static void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId)
        {
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = 990000L, Cy = 792000L },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = "New Bitmap Image.jpg"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.Print
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = 9900000L, Cy = 7920000L }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         EditId = "50D07946"
                     });

            // Append the reference to body, the element should be in a Run.
            wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(element)));
        }

        public static void CreateTable(string fileName, int colcount, int rowcount, int begintag)
        {
            WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(fileName, true);
            Body body = wordprocessingDocument.MainDocumentPart.Document.Body;

            Table tb = new Table();

            //Table properties
            TableProperties tableProp = new TableProperties();
            TableStyle tableStyle = new TableStyle() { Val = "TableGrid" };
            TableWidth tableWidth = new TableWidth() { Width = "2300", Type = TableWidthUnitValues.Pct };
            tableProp.Append(tableStyle, tableWidth);
            tb.AppendChild(tableProp);

            //Table column
            TableGrid tg = new TableGrid();
            for (int i = 0; i < colcount; i++)
            {
                GridColumn gc = new GridColumn();
                tg.AppendChild(gc);
            }
            tb.AppendChild(tg);

            //Table row
            for (int j = 0; j < rowcount; j++)
            {
                TableRow tr = new TableRow();
                for (int k = 0; k < colcount; k++)
                {
                    TableCell tc = new TableCell(new Paragraph(new Run(new Text(txtPatentlist[begintag][0]))));
                    tr.Append(tc);
                    begintag++;
                }
                tb.AppendChild(tr);
            }
            body.AppendChild(tb);
            wordprocessingDocument.MainDocumentPart.Document.Save();
            wordprocessingDocument.Close();
        }

        public static void AppendListItem(Body body, string p, int r1, int ni, int i)
        {
            Paragraph paragraph1 = new Paragraph();
            ParagraphProperties pp1 = new ParagraphProperties();
            ParagraphStyleId psi = new ParagraphStyleId() { Val = "ListParagraph" };

            NumberingProperties numberingProperties1 = new NumberingProperties();
            NumberingLevelReference numberingLevelReference1 = new NumberingLevelReference() { Val = r1 };
            NumberingId numberingId1 = new NumberingId() { Val = ni };

            numberingProperties1.Append(numberingLevelReference1);
            numberingProperties1.Append(numberingId1);
            Indentation indentation1 = new Indentation() { FirstLineChars = i };

            pp1.Append(psi);
            pp1.Append(numberingProperties1);
            pp1.Append(indentation1);

            Run run = new Run();
            Text text = new Text();
            text.Text = p;
            run.Append(text);
            paragraph1.Append(pp1);
            paragraph1.Append(run);
            body.Append(paragraph1);


        }
    }
}
