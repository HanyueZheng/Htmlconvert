using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;


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
            string fileName = @"C:\Users\12773\Desktop\aa.png";
            //string strTxt = "Append text in body - OpenAndAddTextToWordDocument";
            //CreateTable(document);
            //OpenAndAddTextToWordDocument(document, strTxt);
            //OpenAndAddTextToWordDocument(document, strTxt);
            //OpenAndAddTextToWordDocument(document, strTxt);
            
            //BookMark(document);
            //SetTitle(document);

            //using (WordprocessingDocument doc =
            //    WordprocessingDocument.Open(document, true))
            //{
            //    // Get the Styles part for this document.
            //    StyleDefinitionsPart part =
            //        doc.MainDocumentPart.StyleDefinitionsPart;

            //    // If the Styles part does not exist, add it and then add the style.
            //    if (part == null)
            //    {
            //        part = AddStylesPartToPackage(doc);
            //    }

            //    // Set up a variable to hold the style ID.
            //    string parastyleid = "OverdueAmountPara";

            //    // Create and add a paragraph style to the specified styles part 
            //    // with the specified style ID, style name and aliases.
            //    CreateAndAddParagraphStyle(part,
            //        parastyleid,
            //        "Overdue Amount Para",
            //        "Late Due, Late Amount");

            //    // Add a paragraph with a run and some text.
            //    Paragraph p =
            //        new Paragraph(
            //            new Run(
            //                new Text("This is some text in a run in a paragraph.")));

            //    // Add the paragraph as a child element of the w:body element.
            //    doc.MainDocumentPart.Document.Body.AppendChild(p);
            //    // If the paragraph has no ParagraphProperties object, create one.
            //    if (p.Elements<ParagraphProperties>().Count() == 0)
            //    {
            //        p.PrependChild<ParagraphProperties>(new ParagraphProperties());
            //    }

            //    // Get a reference to the ParagraphProperties object.
            //    ParagraphProperties pPr = p.ParagraphProperties;

            //    // If a ParagraphStyleId object doesn't exist, create one.
            //    if (pPr.ParagraphStyleId == null)
            //        pPr.ParagraphStyleId = new ParagraphStyleId();

            //    // Set the style of the paragraph.
            //    pPr.ParagraphStyleId.Val = parastyleid;
            //}


        }


        // Insert a table into a word processing document.
        public static void CreateTable(string fileName)
        {
            // Use the file name and path passed in as an argument 
            // to open an existing Word 2007 document.

            using (WordprocessingDocument doc
                = WordprocessingDocument.Open(fileName, true))
            {
                // Create an empty table.
                Table table = new Table();

                // Create a TableProperties object and specify its border information.
                TableProperties tblProp = new TableProperties(
                    new TableBorders(
                        new TopBorder()
                        {
                            Val =
                            new EnumValue<BorderValues>(BorderValues.BasicThinLines),
                            Size = 10
                        },
                        new BottomBorder()
                        {
                            Val =
                            new EnumValue<BorderValues>(BorderValues.BasicThinLines),
                            Size = 10
                        },
                        new LeftBorder()
                        {
                            Val =
                            new EnumValue<BorderValues>(BorderValues.BasicThinLines),
                            Size = 10
                        },
                        new RightBorder()
                        {
                            Val =
                            new EnumValue<BorderValues>(BorderValues.BasicThinLines),
                            Size = 10
                        },
                        new InsideHorizontalBorder()
                        {
                            Val =
                            new EnumValue<BorderValues>(BorderValues.BasicThinLines),
                            Size = 10
                        },
                        new InsideVerticalBorder()
                        {
                            Val =
                            new EnumValue<BorderValues>(BorderValues.BasicThinLines),
                            Size = 10
                        }
                    )
                );

                // Append the TableProperties object to the empty table.
                table.AppendChild<TableProperties>(tblProp);

                // Create a row.
                TableRow tr = new TableRow();

                // Create a cell.
                TableCell tc1 = new TableCell();

                // Specify the width property of the table cell.
                tc1.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));

                // Specify the table cell content.
                tc1.Append(new Paragraph(new Run(new Text("some text1"))));

                // Append the table cell to the table row.
                tr.Append(tc1);

                // Create a second table cell by copying the OuterXml value of the first table cell.
                TableCell tc2 = new TableCell(tc1.OuterXml);

                // Append the table cell to the table row.
                tr.Append(tc2);

                // Append the table row to the table.
                table.Append(tr);

                // Append the table to the document.
                doc.MainDocumentPart.Document.Body.Append(table);
            }
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
            var html =
        @"<body><h1><b>bold</b> heading</h1><p>This is <u>italic</u> paragraph</p><img src='C:\Users\12773\Desktop\aa.png'/></body>";

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
                if (nodename == "h1" || nodename == "p" || nodename == "img")
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
                n = n.getParent();
            }
            txtPatentlist.Add(plist);
        }

        public static void ProcessTranslate()
        {
            string document = @"C:\Users\12773\Desktop\demo.docx";
            string fileName = @"C:\Users\12773\Desktop\aa.png";
            int begintag = 0;
            int endtag = 0;
            while (begintag < txtPatentlist.Count())
            {
                endtag = GetInterval(begintag);

                WordprocessingDocument wordprocessingDocument =
                    WordprocessingDocument.Open(document, true);
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
                            case "img":
                                InsertAPicture(document, fileName);
                                break;
                            default:
                                break;
                        }
                    }
                }
                wordprocessingDocument.Close();
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
                                             new A.Extents() { Cx = 990000L, Cy = 792000L }),
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

    }
}
