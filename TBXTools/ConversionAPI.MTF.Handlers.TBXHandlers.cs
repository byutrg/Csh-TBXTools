using System;
using System.Collections.Generic;
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
                    break;
                case "change":
                    break;
                case "conceptEntry":
                    break;
                case "date":
                    outElement = HandleDate(currentElement);
                    break;
                case "descrip":
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
                    break;
                case "note":
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
                    break;
                case "termNote":
                    break;
                case "termNoteGrp":
                    break;
                case "termSec":
                    break;
                case "text":
                    break;
                case "title":
                    break;
                case "titleStmt":
                    break;
                case "transac":
                    outElement = HandleTransac(currentElement);
                    break;
                case "transacGrp":
                    outElement = TBXHandlers.HandleTransacGrp(currentElement);
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

        public static XElement HandleDate(XElement elt)
        {
            return new XElement("date", DateTime.Parse(elt.Value).ToString("yyyy-MM-ddT00:00:00"));
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
            switch(type) {
                case "transactionType":
                    type = elt.Value;
                    XElement responsibilityElt = elt.ElementsAfterSelf("transacNote").First(e => e.Attribute("type")?.Value == "responsibility");
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
