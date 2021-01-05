using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TBXTools
{
    public static class XmlTools
    {
        /// <summary>
        /// Get's the XPath location of an element. Uses one-based indexing.
        /// </summary>
        /// <param name="elt"></param>
        /// <returns>XPath location as string</returns>
        public static string GetNodeLocation(XElement elt)
        {
            if (elt.Parent == null) return $"/{elt.Name.LocalName}";

            int index = elt.Parent.Elements(elt.Name).ToList().IndexOf(elt) + 1;
            return $@"{GetNodeLocation(elt.Parent)}/{elt.Name.LocalName}[{index}]";
        } 
    }
}
