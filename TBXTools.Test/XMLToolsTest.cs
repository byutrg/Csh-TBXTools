using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Xml.Linq;

namespace TBXTools.Test
{
    [TestClass]
    public class XMLToolsTest
    {
        [TestMethod]
        public void GetNodeLocation_ValidInput_Test()
        {
            XElement context = new XElement("root",
                new XElement("hello"), new XElement("hello",
                    new XElement("world")));
            string expected = "/root/hello[2]/world[1]";

            Assert.AreEqual(XmlTools.GetNodeLocation(context.Descendants("world").First()), expected);
        }
    }
}
