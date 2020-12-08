using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TBXTools.ConversionAPI.MTF.Handlers;

namespace TBXTools.ConversionAPI.MTF
{
    public static class Convert
    {
        public static void ToTBX() { }

        public static void ToMTF(FileStream tbxFile)
        {
            XDocument tbxDoc = XDocument.Load(tbxFile);
            IEnumerable<XElement> conceptEntries = tbxDoc.Descendants(TBXTools.ValidationAPI.Namespace + "conceptEntry");
            int conceptEntriesCount = conceptEntries.Count();
            XDocument mtfDoc = new XDocument(new XElement("mtf"));
            for (int i = 0; i < conceptEntriesCount; i++)
            {
                XElement conceptEntry = conceptEntries.ElementAt(i);
                XElement conceptGrp = new XElement("conceptGrp", new XElement("concept", i));


                    
            }
        }

        
    }
}
