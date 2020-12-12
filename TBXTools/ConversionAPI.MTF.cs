using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using TBXTools.ConversionAPI.MTF.Handlers;

namespace TBXTools.ConversionAPI.MTF
{
    public static class Convert
    {
        public static void Convert_MTF_TBX() { }

        public static bool Convert_TBX_MTF(Stream tbxFile, Stream outputFile)
        {
            try
            {
                XDocument tbxDoc = XDocument.Load(tbxFile);
                XElement body = tbxDoc.Descendants(TBXTools.ValidationAPI.Namespace + "body").First();
                XDocument mtfDoc = new XDocument(TBXHandlers.HandleBody(body));
                mtfDoc.Save(outputFile);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        
    }
}
