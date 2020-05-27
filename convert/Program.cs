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
using System;
using System.Text;

namespace convert
{
    class Program
    {
        static List<List<string>> txtPatentlist = new List<List<string>>();
        static WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open("test.docx", true);
        //static WordprocessingDocument wordprocessingDocument = new WordprocessingDocument();
        static MainDocumentPart m = wordprocessingDocument.MainDocumentPart;
        static Body body = new Body();

        static void Main(string[] args)
        {
            //string path = "case.html";
            string dir = "./toDelete/toDelete/FUNCTIONAL REQUIREMENTS/";
            List<string> filearr = new List<string>();
            List<string> htmlarr = new List<string>();
            List<string> htmlarr1 = getPath(dir, filearr, htmlarr);
            List<string> headinglist = new List<string>();
            
            foreach (string html in htmlarr1)
            {
                int count = SubstringCount("\\", html);
                string htmlstring = html + count.ToString();
                headinglist.Add(htmlstring);
            }
            string path = "demo.html";
            Program.ParseHtml(path);
            string savepath = "test.docx";
            ProcessTranslate(savepath);
            string document = @" demo.docx";
        }

        public static string ToHexString(string str)
        {
            byte[] btArr = Encoding.Default.GetBytes(str);
            string strBuffer = Encoding.UTF8.GetString(btArr);
            return strBuffer;
        }

        static int SubstringCount(string a, string b)
        {
            int count = 0;
            int index = 0;

            while ((index = b.IndexOf(a, index)) != -1)
            {
                count++;
                index = index + a.Length;
            }
            return count;
        }

        public static List<string> getPath(string dir, List<string> filearr, List<string> htmlarr)
        {
            DirectoryInfo Dir = new DirectoryInfo(dir);
            foreach (DirectoryInfo d in Dir.GetDirectories()){
                getPath(dir + d.ToString() + "\\", filearr, htmlarr);
                filearr.Add(dir + d.ToString() + "\\");
            }
            foreach (FileInfo f in Dir.GetFiles("*.html*"))
            {
                htmlarr.Add(Dir + f.ToString());
            }
                return htmlarr;
        }

        public static void BookMark(string filepath)
        {
            //WordprocessingDocument doc =
            //WordprocessingDocument.Open(filepath, true);
            //Body body = doc.MainDocumentPart.Document.Body;
            //Paragraph paragraph1 = doc.MainDocumentPart.Document.Body.Elements<Paragraph>().First();
            Paragraph paragraph1 = body.Elements<Paragraph>().First();

            BookmarkStart bookmarkStart = new BookmarkStart() { Name = "p1", Id = "1" };
            BookmarkEnd bookmarkEnd = new BookmarkEnd() { Id = "1" };
            body.InsertBefore<BookmarkStart>(bookmarkStart, paragraph1);
            body.InsertAfter<BookmarkEnd>(bookmarkEnd, paragraph1);

            //Paragraph paragraph3 = doc.MainDocumentPart.Document.Body.Elements<Paragraph>().ElementAt(2);
            Paragraph paragraph3 = body.Elements<Paragraph>().ElementAt(2);
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
            //doc.Close();
        }

        public static void ParseHtml(string filepath)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(filepath);
            string document = @"demo.docx";

            //doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//body");
            Node n1 = new Node();
            n1.setNodename("body");
            traverse(node, n1);
            HtmlConvert(n1);
        }

        public static void traverse(HtmlNode node, Node pnode)
        {
            for (int i = 0; i < node.ChildNodes.Count(); i++)
            {
                HtmlNode n = node.ChildNodes[i];
                if (n.InnerHtml != "\r\n" && n.InnerHtml != "\r\n\t")
                {
                    Node tnode = new Node();
                    var nodename = n.Name;
                    tnode.setNodename(nodename);
                    tnode.setParent(pnode);
                    pnode.AddChild(tnode);
                    if (nodename == "h1"|| nodename == "h2"|| nodename == "h3" || nodename == "p" || nodename == "img" || nodename == "table" || (nodename == "ul" && tnode.getParent().getNodename() != "li" || nodename == "pre"))
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
                    {
                        tnode.setText(n.InnerHtml);                        
                    }
                    else { traverse(n, tnode); }
                }
                
            }
        }

        public static void HtmlConvert(Node n1)
        {
            string document = @"demo.docx";
            string nodename = n1.getNodename();
            List<Node> nlist = n1.getChilds();
            int count = nlist.Count;
            switch (nodename)
            {
                case "br":
                    List<string> brlist = new List<string>();
                    brlist.Add("br");
                    txtPatentlist.Add(brlist);
                    break;
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
                    string txt = ToHexString(n1.getText());
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

        public static void ProcessTranslate(string document)
        {
            if (!File.Exists(document))
            {
                FileStream f = File.Create(document);
                StreamWriter sw = new StreamWriter(f);
                sw.Write("\n");
                f.Close();
                f.Dispose();
            }
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

                //else if (txtPatentlist[begintag][0] == "br")
                //{
                //    WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(document, true);
                //    Body body = wordprocessingDocument.MainDocumentPart.Document.Body;
                //    Paragraph para = new Paragraph();
                //    Run run = para.AppendChild(new Run());
                //    run.Append(new Break());
                //    body.AppendChild(para);
                //    wordprocessingDocument.MainDocumentPart.Document.Save();

                //    wordprocessingDocument.Close();
                //}

                else if (txtPatentlist[begintag].Contains("table"))
                {
                    int trindex = txtPatentlist[begintag].IndexOf("tr") + 1;
                    int colcount = Int32.Parse(txtPatentlist[begintag][trindex]);
                    int rowcount = (endtag - begintag) / colcount;
                    CreateTable(document, colcount, rowcount, begintag);
                }

                else if (txtPatentlist[begintag].Contains("ul"))
                {
                    //WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(document, true);
                    //MainDocumentPart m = wordprocessingDocument.MainDocumentPart;
                    //Body body = m.Document.Body;

                    NumberingDefinitionsPart numberingPart = m.NumberingDefinitionsPart;
                    if (numberingPart == null)
                    {
                        numberingPart = m.AddNewPart<NumberingDefinitionsPart>();
                    }

                        Numbering element =
                      new Numbering(
                        new AbstractNum(
                          new MultiLevelType() { Val = MultiLevelValues.Multilevel },
                          new Level(
                            
                            new NumberingFormat() { Val = NumberFormatValues.Bullet },
                            new LevelText() { Val = "●" }
                          )
                          { LevelIndex = 0 },

                          new Level(

                            new NumberingFormat() { Val = NumberFormatValues.Bullet },
                            new LevelText() { Val = "o" }
                          )
                          { LevelIndex = 1 },

                          new Level(

                            new NumberingFormat() { Val = NumberFormatValues.Bullet },
                            new LevelText() { Val = "■" }
                          )
                          { LevelIndex = 2}
                        )
                        { AbstractNumberId = 1 },
                        new NumberingInstance(
                          new AbstractNumId() { Val = 1 }
                        )
                        { NumberID = 1 }
                        );

                    element.Save(numberingPart);




                    bool isenter = false;
                    
                    for (; begintag < endtag; begintag++)
                    {
                        if (txtPatentlist[begintag][0] == "br")
                        {
                            begintag++;
                            isenter = true;
                        }
                        int level = -1;
                        string content = txtPatentlist[begintag][0];
                        foreach (string s in txtPatentlist[begintag])
                        {
                            if (s == "ul")
                                level += 1;
                        }
                        AppendListItem(wordprocessingDocument, content, level, listcount, 0, isenter);
                        wordprocessingDocument.MainDocumentPart.Document.Save();
                        isenter = false;
                    }
                    listcount++;
                    //wordprocessingDocument.Close();                                 
                }

                else if (txtPatentlist[begintag].Contains("pre"))
                {
                    //WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(document, true);
                    
                    //Body body = wordprocessingDocument.MainDocumentPart.Document.Body;
                    Paragraph para = new Paragraph();

                    ParagraphProperties paraProperties = new ParagraphProperties();
                    ParagraphBorders paraBorders = new ParagraphBorders();
                    TopBorder top = new TopBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)9U, Space = (UInt32Value)5U };
                    BottomBorder bottom = new BottomBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)9U, Space = (UInt32Value)5U };
                    LeftBorder left = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)9U, Space = (UInt32Value)5U };
                    RightBorder right = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)9U, Space = (UInt32Value)5U };
                    paraBorders.Append(bottom);
                    paraBorders.Append(top);
                    paraBorders.Append(left);
                    paraBorders.Append(right);
                    paraProperties.Append(paraBorders);
                    para.Append(paraProperties);

                    Run run = para.AppendChild(new Run());
                    string t = txtPatentlist[begintag][0] ;
                    string[] s = t.Split('\n');
                    foreach (string str in s)
                    {
                        if (str.Contains("  "))
                        {
                            run.Append(new TabChar());
                        }
                        run.AppendChild(new Text(str));
                        run.Append(new Break());
                    }

                    
                    body.AppendChild(para);
                    wordprocessingDocument.MainDocumentPart.Document.Save();

                    //wordprocessingDocument.Close();
                }

                else if (txtPatentlist[begintag].Contains("h1"))
                {
                    SetTitle(begintag, endtag, 1.ToString(), document);
                    //string path = @"demo.docx";
                    //CreateWordDocumentUsingMSWordStyles(begintag, endtag, path, "E:\\Microsoft Office\\Office16\\2052\\QuickStyles\\Default.dotx");

                }

                else if (txtPatentlist[begintag].Contains("h2"))
                {
                    SetTitle2(begintag, endtag, 2.ToString(), document);
                }

                else if (txtPatentlist[begintag].Contains("h3"))
                {
                    SetTitle3(begintag, endtag, 3.ToString(), document);
                }

                else
                {
                    //WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(document, true);
                    //Body body = wordprocessingDocument.MainDocumentPart.Document.Body;
                    Paragraph para = body.AppendChild(new Paragraph());

                    for (; begintag < endtag; begintag++)
                    {
                        Run run = para.AppendChild(new Run());
                        if (txtPatentlist[begintag][0] == "br")
                        {
                            run.Append(new Break());
                            begintag++;
                        }
                        if (txtPatentlist[begintag][0] != "endtag")
                        {
                            Text t = new Text(ToHexString(txtPatentlist[begintag][0]));
                            t.Space = t.Space = new EnumValue<SpaceProcessingModeValues>(SpaceProcessingModeValues.Preserve);
                            
                            run.AppendChild(t);
                            
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
                    //wordprocessingDocument.Close();
                }
                                
                begintag = endtag + 1;
            }
            m.Document.Body = body;
            wordprocessingDocument.MainDocumentPart.Document.Save();
            wordprocessingDocument.Close();

        }

        public static void CreateWordDocumentUsingMSWordStyles(int begintag, int endtag, string outputPath, string templatePath)
        {
            // create a copy of the template and open the copy
            File.Copy(templatePath, outputPath, true);

            using (var document = WordprocessingDocument.Open(outputPath, true))
            {
                document.ChangeDocumentType(WordprocessingDocumentType.Document);

                var mainPart = document.MainDocumentPart;
                var settings = mainPart.DocumentSettingsPart;

                var templateRelationship = new AttachedTemplate { Id = "relationId1" };
                settings.Settings.Append(templateRelationship);

                var templateUri = new Uri("c:\\anything.dotx", UriKind.Absolute); // you can put any path you like and the document styles still work
                settings.AddExternalRelationship("http://schemas.openxmlformats.org/officeDocument/2006/relationships/attachedTemplate", templateUri, templateRelationship.Id);

                // using Title as it would appear in Microsoft Word
                var paragraphProps = new ParagraphProperties();
                paragraphProps.ParagraphStyleId = new ParagraphStyleId { Val = "Heading2" };

                // add some text with the "Title" style from the "Default" style set supplied by Microsoft Word
                var run = new Run();
                run.Append(new Text("1.1.3"));

                var paragraph = new Paragraph();
                paragraph.Append(paragraphProps);
                paragraph.Append(run);

                mainPart.Document.Body.Append(paragraph);

                mainPart.Document.Save();
            }
        }


        public static void SetTitle2(int begintag, int endtag, string val, string document)
        {
            //WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(document, true);
            //Body body = wordprocessingDocument.MainDocumentPart.Document.Body;

            StyleDefinitionsPart part = wordprocessingDocument.MainDocumentPart.StyleDefinitionsPart;

            if (part != null)
            {
                Style style = new Style()
                {
                    Type = StyleValues.Paragraph,
                    StyleId = "Heading2",
                    BasedOn = new BasedOn() { Val = "Normal" },

                };
                StyleName styleName1 = new StyleName() { Val = "heading 2" };
                style.Append(styleName1);
                style.Append(new PrimaryStyle());
                StyleRunProperties styleRunProperties1 = new StyleRunProperties();

                styleRunProperties1.Append(new RunFonts()
                {
                    ComplexScriptTheme = ThemeFontValues.MajorBidi,
                    HighAnsiTheme = ThemeFontValues.MajorHighAnsi,
                    EastAsiaTheme = ThemeFontValues.MajorEastAsia,
                    AsciiTheme = ThemeFontValues.MajorAscii
                });
                styleRunProperties1.Append(new FontSize() { Val = "36" });
                style.Append(styleRunProperties1);
                part.Styles.Append(style);

                Paragraph p = new Paragraph();
                ParagraphProperties ppr = new ParagraphProperties();
                ParagraphStyleId stid = new ParagraphStyleId() { Val = "Heading2" };
                ppr.Append(stid);
                p.Append(ppr);

                for (; begintag < endtag; begintag++)
                {
                    Run run = p.AppendChild(new Run(new Text(ToHexString(txtPatentlist[begintag][0]))));
                }

                body.Append(p);
                wordprocessingDocument.MainDocumentPart.Document.Save();

                //wordprocessingDocument.Close();
            }
        }

        public static void SetTitle3(int begintag, int endtag, string val, string document)
        {
            //WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(document, true);
            //Body body = wordprocessingDocument.MainDocumentPart.Document.Body;

            StyleDefinitionsPart part = wordprocessingDocument.MainDocumentPart.StyleDefinitionsPart;

            if (part != null)
            {
                Style style = new Style()
                {
                    Type = StyleValues.Paragraph,
                    StyleId = "Heading3",
                    BasedOn = new BasedOn() { Val = "Normal" },

                };
                StyleName styleName1 = new StyleName() { Val = "heading 3" };
                style.Append(styleName1);
                style.Append(new PrimaryStyle());
                StyleRunProperties styleRunProperties1 = new StyleRunProperties();

                styleRunProperties1.Append(new RunFonts()
                {
                    ComplexScriptTheme = ThemeFontValues.MajorBidi,
                    HighAnsiTheme = ThemeFontValues.MajorHighAnsi,
                    EastAsiaTheme = ThemeFontValues.MajorEastAsia,
                    AsciiTheme = ThemeFontValues.MajorAscii
                });
                styleRunProperties1.Append(new FontSize() { Val = "28" });
                style.Append(styleRunProperties1);
                part.Styles.Append(style);

                Paragraph p = new Paragraph();
                ParagraphProperties ppr = new ParagraphProperties();
                ParagraphStyleId stid = new ParagraphStyleId() { Val = "Heading3" };
                ppr.Append(stid);
                p.Append(ppr);

                for (; begintag < endtag; begintag++)
                {
                    Run run = p.AppendChild(new Run(new Text(ToHexString(txtPatentlist[begintag][0]))));
                }

                body.Append(p);
                wordprocessingDocument.MainDocumentPart.Document.Save();

                //wordprocessingDocument.Close();
            }
        }

        public static void SetTitle(int begintag, int endtag, string val, string document)
        {
            //WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(document, true);
            //Body body = wordprocessingDocument.MainDocumentPart.Document.Body;

            StyleDefinitionsPart part = wordprocessingDocument.MainDocumentPart.StyleDefinitionsPart;
            if (part != null)
            {
                Style style = new Style()
                {
                    Type = StyleValues.Paragraph,
                    StyleId = "Heading1",
                    BasedOn = new BasedOn() { Val = "Normal" },
                    
                };
                StyleName styleName1 = new StyleName() { Val = "heading 1" };
                style.Append(styleName1);
                style.Append(new PrimaryStyle());
                StyleRunProperties styleRunProperties1 = new StyleRunProperties();
                styleRunProperties1.Append(new Bold());

                styleRunProperties1.Append(new RunFonts()
                {
                    ComplexScriptTheme = ThemeFontValues.MajorBidi,
                    HighAnsiTheme = ThemeFontValues.MajorHighAnsi,
                    EastAsiaTheme = ThemeFontValues.MajorEastAsia,
                    AsciiTheme = ThemeFontValues.MajorAscii
                });
                styleRunProperties1.Append(new FontSize() { Val = "48" });
                style.Append(styleRunProperties1);
                part.Styles.Append(style);

                Paragraph p = new Paragraph();
                ParagraphProperties ppr = new ParagraphProperties();
                ParagraphStyleId stid = new ParagraphStyleId() { Val = "Heading1" };
                ppr.Append(stid);
                p.Append(ppr);

                for (; begintag < endtag; begintag++)
                {
                    Run run = p.AppendChild(new Run(new Text(ToHexString(txtPatentlist[begintag][0]))));
                }

                body.Append(p);
                wordprocessingDocument.MainDocumentPart.Document.Save();

                //wordprocessingDocument.Close();
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

        // Add a StylesDefinitionsPart to the document.  Returns a reference to it.
        public static StyleDefinitionsPart AddStylesPartToPackage(WordprocessingDocument doc)
        {
            StyleDefinitionsPart part;
            part = doc.MainDocumentPart.AddNewPart<StyleDefinitionsPart>();
            Styles root = new Styles();
            root.Save(part);
            return part;
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
                                             new A.Extents() { Cx = 99000000L, Cy = 79200000L }),
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
            //WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(fileName, true);
            //Body body = wordprocessingDocument.MainDocumentPart.Document.Body;

            Table tb = new Table();

            //Table properties
            TableProperties tableProp = new TableProperties();
            TableBorders tbb = new TableBorders( 
                new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 7},
                new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 7},
                new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 7},
                new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 7},
                new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 7},
                new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 7}
            );
            TableWidth tableWidth = new TableWidth() { Width = "2800", Type = TableWidthUnitValues.Pct };
            
            tableProp.Append( tableWidth, tbb);
            tb.AppendChild(tableProp);

            //Table column
            TableGrid tg = new TableGrid();
            
            for (int i = 1; i < colcount; i++)
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
                    TableCell tc = new TableCell(new Paragraph(new Run(new Text(ToHexString(txtPatentlist[begintag][0])))));
                    tr.Append(tc);
                    begintag++;
                }
               
                tb.AppendChild(tr);
            }
            body.AppendChild(tb);
            wordprocessingDocument.MainDocumentPart.Document.Save();
            //wordprocessingDocument.Close();
        }

        public static void AppendListItem(WordprocessingDocument wordprocessingDocument, string p, int level, int listcount, int i, bool isenter)
        {
            
            //MainDocumentPart m = wordprocessingDocument.MainDocumentPart;
            //Body body = m.Document.Body;


            Paragraph paragraph1 = new Paragraph();
            Paragraph paragraph2 = new Paragraph();
            ParagraphProperties pp1 = new ParagraphProperties();
            ParagraphProperties pp2 = new ParagraphProperties();
            
            int indent = 420 * (level + 1);
            Indentation indentation2 = new Indentation() { Left = indent .ToString() };

            NumberingProperties numberingProperties1 = new NumberingProperties();
            NumberingLevelReference numberingLevelReference1 = new NumberingLevelReference() { Val = level };
            
            NumberingId numberingId1 = new NumberingId() { Val = 1 };
           
            numberingProperties1.Append(numberingLevelReference1);
            numberingProperties1.Append(numberingId1);
            int indent1 = 420 * level;
            Indentation indentation1 = new Indentation() { Left = indent1.ToString() };

            
            pp1.Append(numberingProperties1);
            pp1.Append(indentation1);

            pp2.Append(indentation2);

            Run run = new Run();
            Text text = new Text();
            text.Text = p;

            run.Append(text);
             
            if (isenter)
            {
                paragraph2.Append(pp2);
                paragraph2.Append(run);
                body.Append(paragraph2);
            }
            else
            {
                paragraph1.Append(pp1);
                paragraph1.Append(run);
                body.Append(paragraph1);
            }
            
            
        }
    }
}
