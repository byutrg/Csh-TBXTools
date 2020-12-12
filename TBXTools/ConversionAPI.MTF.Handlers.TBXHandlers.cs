using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace TBXTools.ConversionAPI.MTF.Handlers
{
    public static class TBXHandlers
    {
        private static void ParseChildNodes(XElement currentElement, XElement outElement)
        {
            foreach (var child in currentElement.Nodes().Where(n => n.NodeType == XmlNodeType.Element || n.NodeType == XmlNodeType.Text))
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    if (DescentParseElements(child as XElement, out XElement outChild)) outElement.Add(outChild);
                }
                else outElement.Add(child);
            }
        }

        private static bool DescentParseElements(XElement currentElement, out XElement outElement)
        {
            outElement = null;
            switch (currentElement.Name.LocalName)
            {
                case "admin":
                    break;
                case "adminGrp":
                    break;
                case "adminNote":
                    break;
                case "back":
                    break;
                case "body":
                    outElement = HandleBody(currentElement);
                    break;
                case "change":
                    break;
                case "conceptEntry":
                    outElement = HandleConceptEntry(currentElement);
                    break;
                case "date":
                    outElement = HandleDate(currentElement);
                    break;
                case "descrip":
                    if (ShouldGroupifyTBXElement(currentElement))
                    {
                        GroupifyTBXElementInSourceXDocument(ref currentElement);
                        outElement = HandleDescripGrp(currentElement);
                    }
                    outElement = HandleDescrip(currentElement);
                    break;
                case "descripGrp":
                    break;
                case "descripNote":
                    break;
                case "ec":
                    break;
                case "encodingDesc":
                    break;
                case "fileDesc":
                    break;
                case "foreign":
                    break;
                case "hi":
                    break;
                case "item":
                    break;
                case "itemGrp":
                    break;
                case "itemSet":
                    break;
                case "langSec":
                    outElement = HandleLangSec(currentElement);
                    break;
                case "note":
                    if (ShouldGroupifyTBXNote(currentElement))
                    {
                        GroupifyTBXElementInSourceXDocument(ref currentElement);
                        outElement = HandleDescripGrp(currentElement);
                    } else outElement = HandleNote(currentElement);
                    break;
                case "p":
                    break;
                case "ph":
                    break;
                case "publicationStmt":
                    break;
                case "ref":
                    break;
                case "refObject":
                    break;
                case "refObjectSec":
                    break;
                case "revisionDesc":
                    break;
                case "sc":
                    break;
                case "sourceDesc":
                    break;
                case "tbx":
                    break;
                case "tbxHeader":
                    break;
                case "term":
                    outElement = HandleTerm(currentElement);
                    break;
                case "termNote":
                    if (ShouldGroupifyTBXElement(currentElement))
                    {
                        GroupifyTBXElementInSourceXDocument(ref currentElement);
                        outElement = HandleDescripGrp(currentElement);
                    } else outElement = HandleTermNote(currentElement);
                    break;
                case "termNoteGrp":
                    break;
                case "termSec":
                    outElement = HandleTermSec(currentElement);
                    break;
                case "text":
                    break;
                case "title":
                    break;
                case "titleStmt":
                    break;
                case "transac":
                    if (ShouldGroupifyTBXElement(currentElement))
                    {
                        GroupifyTBXElementInSourceXDocument(ref currentElement);
                        outElement = HandleTransacGrp(currentElement);
                    } else outElement = HandleTransac(currentElement);
                    break;
                case "transacGrp":
                    outElement = HandleTransacGrp(currentElement);
                    break;
                case "transacNote":
                    break;
                case "xref":
                    break;
                default:
                    break;
            }

            return outElement != null;
        }

        public static bool ShouldGroupifyTBXElement(XElement elt)
        {
            return elt.Parent.Name != elt.Name + "Grp";
        }

        public static bool ShouldGroupifyTBXNote(XElement elt)
        {
            return !elt.Parent.Name.LocalName.EndsWith("Grp");
        }

        public static void GroupifyTBXElementInSourceXDocument(ref XElement elt)
        {
            XElement grpElt = new XElement(elt.Name + "Grp", new XElement(elt));
            elt = grpElt;
        }

        public static void OrderTermSecChildren(XElement elt)
        {
            List<XElement> newlySortedChildren = elt.Elements().OrderBy(e => e.Name.LocalName.Equals("descripGrp"))
                .ThenBy(e => e.Name.LocalName.Equals("transacGrp"))
                .ToList();

            elt.Elements().Remove();
            elt.Add(newlySortedChildren);
        }

        public static XElement HandleBody(XElement elt)
        {
            XElement newElt = new XElement("mtf");
            ParseChildNodes(elt, newElt);
            return newElt;
        }

        public static XElement HandleConceptEntry(XElement elt)
        {
            int id = elt.ElementsBeforeSelf(elt.Name).Count() + 1;
            XElement newElt = new XElement("conceptGrp", new XElement("concept", id));
            ParseChildNodes(elt, newElt);
            return newElt;
        }

        public static XElement HandleDate(XElement elt)
        {
            return new XElement("date", DateTime.Parse(elt.Value).ToString("yyyy-MM-ddT00:00:00"));
        }

        public static XElement HandleDescrip(XElement elt)
        {
            string type = elt.Attribute("type").Value;
            switch (type)
            {
                case "subjectField":
                    type = "Subject field";
                    break;
                default:
                    break;
            }

            return new XElement("descrip", new XAttribute("type", type), elt.Value);
        }

        public static XElement HandleDescripGrp(XElement elt)
        {
            XElement newElt = new XElement("descripGrp");
            ParseChildNodes(elt, newElt);
            return newElt;
        }

        public static XElement HandleLangSec(XElement elt)
        {
            string langCode = elt.Attribute(XNamespace.Xml + "lang")?.Value;
            string langName = null;
            if (!string.IsNullOrWhiteSpace(langCode))
            {
                langName = new CultureInfo(langCode).DisplayName;
            }
            XElement newElt = new XElement("languageGrp", 
                new XElement("language",
                    new XAttribute("type", langName),
                    new XAttribute("lang", langCode)
                ));

            ParseChildNodes(elt, newElt);
            return newElt;
        }

        public static XElement HandleNote(XElement elt)
        {
            return new XElement("descrip", new XAttribute("type", "Note"), elt.Value);
        }

        public static XElement HandleTerm(XElement elt)
        {
            return new XElement("term", elt.Value);
        }

        public static XElement HandleTermNote(XElement elt)
        {
            string type = elt.Attribute("type").Value;
            switch (type)
            {
                case "partOfSpeech":
                    type = "Part of Speech";
                    break;
                default:
                    break;
            }

            return new XElement("descrip", new XAttribute("type", type), elt.Value);
        }

        public static XElement HandleTermSec(XElement elt)
        {
            XElement newElt = new XElement("termGrp");
            ParseChildNodes(elt, newElt);
            OrderTermSecChildren(newElt);
            return newElt;
        }

        public static XElement HandleTransacGrp(XElement elt)
        {
            XElement newElt = new XElement("transacGrp");
            ParseChildNodes(elt, newElt);
            return newElt;
        }

        public static XElement HandleTransac(XElement elt)
        {
            string type = elt.Attribute("type")?.Value;
            string value = elt.Value;
            switch (type)
            {
                case "transactionType":
                    type = elt.Value;
                    XElement responsibilityElt = elt.ElementsAfterSelf(elt.Name.Namespace + "transacNote")?.First(e => e.Attribute("type")?.Value == "responsibility");
                    if (responsibilityElt != null)
                    {
                        responsibilityElt.Remove();
                        value = responsibilityElt.Value;
                    }
                    break;
                default:
                    break;
            }

            return new XElement("transac", new XAttribute("type", type), value);
        }
    }
}
