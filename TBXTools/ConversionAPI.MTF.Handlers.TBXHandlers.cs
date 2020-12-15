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
                    if (DescentParseElements(child as XElement, out XNode outChild)) outElement.Add(outChild);
                }
                else outElement.Add(child);
            }
        }

        private static bool DescentParseElements(XElement currentElement, out XNode outNode)
        {
            outNode = null;
            switch (currentElement.Name.LocalName)
            {
                case "admin":
                    if (ShouldGroupifyTBXElement(currentElement))
                    {
                        GroupifyTBXElementInSourceXDocument(ref currentElement);
                        outNode = HandleAdminGrp(currentElement);
                    }
                    else outNode = HandleAdmin(currentElement);
                    break;
                case "adminGrp":
                    outNode = HandleAdminGrp(currentElement);
                    break;
                case "adminNote":
                    if (ShouldGroupifyTBXElement(currentElement))
                    {
                        GroupifyTBXElementInSourceXDocument(ref currentElement);
                        outNode = HandleAdminGrp(currentElement);
                    }
                    else outNode = HandleAdminNote(currentElement);
                    break;
                case "back":
                    break;
                case "body":
                    outNode = HandleBody(currentElement);
                    break;
                case "change":
                    break;
                case "conceptEntry":
                    outNode = HandleConceptEntry(currentElement);
                    break;
                case "date":
                    outNode = HandleDate(currentElement);
                    break;
                case "descrip":
                    if (ShouldGroupifyTBXElement(currentElement))
                    {
                        GroupifyTBXElementInSourceXDocument(ref currentElement);
                        outNode = HandleDescripGrp(currentElement);
                    } else outNode = HandleDescrip(currentElement);
                    break;
                case "descripGrp":
                    outNode = HandleDescripGrp(currentElement);
                    break;
                case "descripNote":
                    if (ShouldGroupifyTBXElement(currentElement))
                    {
                        GroupifyTBXElementInSourceXDocument(ref currentElement);
                        outNode = HandleDescripGrp(currentElement);
                    }
                    else outNode = HandleDescripNote(currentElement);
                    break;
                case "ec":
                    outNode = new XText(currentElement.Value);
                    break;
                case "encodingDesc":
                    break;
                case "fileDesc":
                    break;
                case "foreign":
                    outNode = new XText(currentElement.Value);
                    break;
                case "hi":
                    outNode = new XText(currentElement.Value);
                    break;
                case "item":
                    break;
                case "itemGrp":
                    break;
                case "itemSet":
                    break;
                case "langSec":
                    outNode = HandleLangSec(currentElement);
                    break;
                case "note":
                    if (ShouldGroupifyTBXNote(currentElement))
                    {
                        GroupifyTBXElementInSourceXDocument(ref currentElement);
                        outNode = HandleDescripGrp(currentElement);
                    } else outNode = HandleNote(currentElement);
                    break;
                case "p":
                    break;
                case "ph":
                    outNode = new XText(currentElement.Value);
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
                    outNode = new XText(currentElement.Value);
                    break;
                case "sourceDesc":
                    break;
                case "tbx":
                    break;
                case "tbxHeader":
                    break;
                case "term":
                    outNode = HandleTerm(currentElement);
                    break;
                case "termNote":
                    if (ShouldGroupifyTBXElement(currentElement))
                    {
                        GroupifyTBXElementInSourceXDocument(ref currentElement);
                        outNode = HandleDescripGrp(currentElement);
                    } else outNode = HandleTermNote(currentElement);
                    break;
                case "termNoteGrp":
                    outNode = HandleTermNoteGrp(currentElement);
                    break;
                case "termSec":
                    outNode = HandleTermSec(currentElement);
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
                        outNode = HandleTransacGrp(currentElement);
                    } else outNode = HandleTransac(currentElement);
                    break;
                case "transacGrp":
                    outNode = HandleTransacGrp(currentElement);
                    break;
                case "transacNote":
                    if (ShouldGroupifyTBXElement(currentElement))
                    {
                        GroupifyTBXElementInSourceXDocument(ref currentElement);
                        outNode = HandleDescripGrp(currentElement);
                    }
                    else outNode = HandleTransacNote(currentElement);
                    break;
                case "xref":
                    if (ShouldGroupifyTBXElement(currentElement))
                    {
                        GroupifyTBXElementInSourceXDocument(ref currentElement);
                        outNode = HandleXrefGrp(currentElement);
                    }
                    else outNode = HandleXref(currentElement);
                    break;
                default:
                    break;
            }

            return outNode != null;
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

        public static XElement HandleAdmin(XElement elt)
        {
            string type = elt.Attribute("type").Value;
            switch (type)
            {
                case "customerSubset":
                    type = "Customer";
                    break;
                case "projectSubset":
                    type = "Project";
                    break;
                case "source":
                    type = "Source";
                    break;
                default:
                    break;
            }

            XElement newElt = new XElement("descrip", new XAttribute("type", type));
            ParseChildNodes(elt, newElt);
            return newElt;
        }

        public static XElement HandleAdminGrp(XElement elt)
        {
            return HandleDescripGrp(elt);
        }

        public static XElement HandleAdminNote(XElement elt)
        {
            XElement newElt = new XElement("descrip");
            ParseChildNodes(elt, newElt);
            return newElt;
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
                case "context":
                    type = "Context";
                    break;
                case "definition":
                    type = "Definition";
                    break;
                case "subjectField":
                    type = "Subject field";
                    break;
                default:
                    break;
            }

            XElement newElt = new XElement("descrip", new XAttribute("type", type));
            ParseChildNodes(elt, newElt);
            return newElt;
        }

        public static XElement HandleDescripGrp(XElement elt)
        {
            XElement newElt = new XElement("descripGrp");
            ParseChildNodes(elt, newElt);
            return newElt;
        }

        public static XElement HandleDescripNote(XElement elt)
        {
            XElement newElt = new XElement("descrip", new XAttribute("type", elt.Attribute("type")));
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
            XElement newElt = new XElement("descrip", new XAttribute("type", "Note"));
            ParseChildNodes(elt, newElt);
            return newElt;
        }

        public static XElement HandleTerm(XElement elt)
        {
            XElement newElt = new XElement("term");
            ParseChildNodes(elt, newElt);
            return newElt;
        }

        public static XElement HandleTermNote(XElement elt)
        {
            string type = elt.Attribute("type").Value;
            string value = elt.Value;
            switch (type)
            {
                case "administrativeStatus":
                    type = "Usage status";
                    switch(value)
                    {
                        case "admittedTerm-admn-sts":
                            value = "admitted";
                            break;
                        case "preferredTerm-admn-sts":
                            value = "preferred";
                            break; 
                        case "deprecatedTerm-admn-sts":
                            value = "not recommended";
                            break;
                        case "supersededTerm-admn-sts":
                            value = "obsolete";
                            break;
                        default:
                            break;
                    }
                    break;
                case "grammaticalGender":
                    type = "Gender";
                    switch (value)
                    {
                        case "masculine":
                        case "feminine":
                        case "neuter":
                        case "other":
                            break;
                        default:
                            break;
                    }
                    break;
                case "partOfSpeech":
                    type = "Part of Speech";
                    switch(value)
                    {
                        case "adjective":
                        case "noun":
                        case "other":
                        case "verb":
                        case "adverb":
                            break;
                        default:
                            break;
                    }
                    break;
                case "termLocation":
                    type = "Term location";
                    switch(value)
                    {
                        case "checkBox":
                            value = "check box";
                            break;
                        case "comboBox":
                            value = "combo box";
                            break;
                        case "comboBoxElement":
                            value = "combo box element";
                            break;
                        case "dialogBox":
                            value = "dialogue box";
                            break;
                        case "groupBox":
                            value = "group box";
                            break;
                        case "informativeMessage":
                            value = "informative message";
                            break;
                        case "interactiveMessage":
                            value = "interactive message";
                            break;
                        case "menuItem":
                            value = "menu item";
                            break;
                        case "progressBar":
                            value = "progress bar";
                            break;
                        case "pushButton":
                            value = "push button";
                            break;
                        case "radioButton":
                            value = "radio button";
                            break;
                        case "slider":
                            break;
                        case "spinBox":
                            value = "spin box";
                            break;
                        case "tab":
                            break;
                        case "tableText":
                            value = "table text";
                            break;
                        case "textBox":
                            value = "text box";
                            break;
                        case "toolTip":
                            value = "tool tip";
                            break;
                        case "user-definedType":
                            break;
                        default:
                            break;
                    }
                    break;
                case "termType":
                    type = "Term type";
                    switch(value)
                    {
                        case "fullForm":
                            value = "full form";
                            break;
                        case "shortForm":
                            value = "short form";
                            break;
                        case "acronym":
                        case "abbreviation":
                        case "variant":
                        case "phrase":
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            elt.Value = value;
            XElement newElt = new XElement("descrip", new XAttribute("type", type));
            ParseChildNodes(elt, newElt);
            return newElt;
        }

        public static XElement HandleTermNoteGrp(XElement elt)
        {
            return HandleDescripGrp(elt);
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

            XElement newElt = new XElement("transac", new XAttribute("type", type), value);
            return newElt;
        }

        public static XElement HandleTransacNote(XElement elt)
        {
            XElement newElt = new XElement("descrip", elt.Attribute("type"));
            ParseChildNodes(elt, newElt);
            return newElt;
        }

        public static XElement HandleXref(XElement elt)
        {
            string type = elt.Attribute("type")?.Value;
            switch(type)
            {
                case "xGraphic":
                    type = "Image";
                    break;
                default:
                    break;
            }

            XElement newElt = new XElement("descrip", new XAttribute("type", type));
            ParseChildNodes(elt, newElt);
            return newElt;
        }

        private static XElement HandleXrefTargetAttributeAsAdmin(XElement elt)
        {
            XElement newElt = null;
            string target = elt.Attribute("target")?.Value;
            if (!string.IsNullOrWhiteSpace(target))
            {
                newElt = new XElement(elt.Name.Namespace + "adminGrp",
                    new XElement(elt.Name.Namespace + "admin", new XAttribute("type", "source"), target)
                    );
            }

            return HandleAdminGrp(newElt);
        }

        /// <summary>
        /// Although there is no such element as &lt;xrefGrp&gt;, this handler is here to handle a "groupified"
        /// xref.
        /// </summary>
        /// <param name="elt">A "groupified" xref, aka &lt;xrefGrp&gt;</param>
        /// <returns>Fully parsed MTF equivalent of xref.</returns>
        public static XElement HandleXrefGrp(XElement elt)
        {
            XElement newElt = HandleDescripGrp(elt);
            XElement xrefTarget = HandleXrefTargetAttributeAsAdmin(elt.Element(elt.Name.Namespace + "xref"));
            newElt.Add(xrefTarget);
            return newElt;
        }
    }
}
