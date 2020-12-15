using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using TBXTools.ConversionAPI.MTF.Handlers;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

namespace TBXTools.Test
{
    public static class ExtensionMethods
    {
        public static string RemoveGeminateWhitespaceAndNewlines(this String str)
        {
            return Regex.Replace(str, @"\s+", " ");
        }
    }

    [TestClass]
    public class TBXHandlersTest
    {


        private (XElement, string) GetResourceSourceAndExpected(string key)
        {
            string source = ConversionAPI_MTF_TBXHandlersResourcesSources.ResourceManager.GetString(key);
            string expected = ConversionAPI_MTF_TBXHandlersResourcesExpected.ResourceManager.GetString(key);

            return (
                XElement.Parse(source),
                expected);
        }

        private bool SerializedElementMatchesString(XElement xElement, string expected)
        {
            string xElementStr = xElement.ToString().RemoveGeminateWhitespaceAndNewlines();
            return xElementStr.Equals(Regex.Replace(expected, @"\s+", " "));
        }

        [TestMethod]
        public void Internal_RemoveGeminateWhitespaceAndNewlines_Valid_Test()
        {
            string source = "This \r \r\n is   a test\n";
            string expected = "This is a test ";
            Assert.AreEqual(source.RemoveGeminateWhitespaceAndNewlines(), expected);
        }

        [TestMethod]
        public void Internal_SerializedElementMatchesString_Valid_Test()
        {
            XElement source = new XElement("test", "hello");
            string expected = @"<test>hello</test>";
            Assert.IsTrue(SerializedElementMatchesString(source, expected));
        }

        [TestMethod]
        public void Internal_SerializedElementMatchesString_Invalid_Test()
        {
            XElement source = new XElement("test", "world");
            string expected = @"<test>hello</test>";
            Assert.IsFalse(SerializedElementMatchesString(source, expected));
        }

        [TestMethod]
        public void ToMTF_ValidInput_Test()
        {
            string tbxFileContent = TestFiles.TBX_Valid;
            Stream streamReader = new MemoryStream(Encoding.UTF8.GetBytes(tbxFileContent));
            Stream outputStream = new MemoryStream();
            ConversionAPI.MTF.Convert.Convert_TBX_MTF(streamReader, outputStream);

            outputStream.Position = 0;
            using (StreamReader outputReader = new StreamReader(outputStream))
            {
                string mtfOuput = outputReader.ReadToEnd();
                Assert.AreEqual(
                    TestFiles.MTF_Valid.RemoveGeminateWhitespaceAndNewlines(),
                    mtfOuput.RemoveGeminateWhitespaceAndNewlines());
            }
        }

        [TestMethod]
        public void GroupifyTBXElementInSourceXDocument_ValidAdminInput_Test()
        {
            (XElement, string) testingConstants = GetResourceSourceAndExpected("admin_toGroupify");
            XElement eltToGroupify = testingConstants.Item1;
            TBXHandlers.GroupifyTBXElementInSourceXDocument(ref eltToGroupify);
            Assert.IsTrue(SerializedElementMatchesString(eltToGroupify, testingConstants.Item2));
        }

        [TestMethod]
        public void GroupifyTBXElementInSourceXDocument_ValidDescripInput_Test()
        {
            (XElement, string) testingConstants = GetResourceSourceAndExpected("descrip_toGroupify");
            XElement eltToGroupify = testingConstants.Item1;
            TBXHandlers.GroupifyTBXElementInSourceXDocument(ref eltToGroupify);
            Assert.IsTrue(SerializedElementMatchesString(eltToGroupify, testingConstants.Item2));
        }

        [TestMethod]
        public void GroupifyTBXElementInSourceXDocument_ValidTransacInput_Test()
        {
            (XElement, string) testingConstants = GetResourceSourceAndExpected("transac_toGroupify");
            XElement eltToGroupify = testingConstants.Item1;
            TBXHandlers.GroupifyTBXElementInSourceXDocument(ref eltToGroupify);
            Assert.IsTrue(SerializedElementMatchesString(eltToGroupify, testingConstants.Item2));
        }

        [TestMethod]
        public void GroupifyTBXElementInSourceXDocument_ValidTermNoteInput_Test()
        {
            (XElement, string) testingConstants = GetResourceSourceAndExpected("termNote_toGroupify");
            XElement eltToGroupify = testingConstants.Item1;
            TBXHandlers.GroupifyTBXElementInSourceXDocument(ref eltToGroupify);
            Assert.IsTrue(SerializedElementMatchesString(eltToGroupify, testingConstants.Item2));
        }

        [TestMethod]
        public void GroupifyTBXElementInSourceXDocument_ValidXrefInput_Test()
        {
            (XElement, string) testingConstants = GetResourceSourceAndExpected("xref_toGroupify");
            XElement eltToGroupify = testingConstants.Item1;
            TBXHandlers.GroupifyTBXElementInSourceXDocument(ref eltToGroupify);
            Assert.IsTrue(SerializedElementMatchesString(eltToGroupify, testingConstants.Item2));
        }

        [TestMethod]
        public void HandleXrefGrp_MockedXrefGrp_Test()
        {
            (XElement, string) testingConstants = GetResourceSourceAndExpected("xref_parsed");
            XElement outElement = TBXHandlers.HandleXrefGrp(testingConstants.Item1);
            Assert.IsTrue(SerializedElementMatchesString(outElement, testingConstants.Item2));
        }

        [TestMethod]
        public void HandleAdmin_ValidInput_Test()
        {
            (XElement, string) testingConstants = GetResourceSourceAndExpected("admin");
            XElement outElement = TBXHandlers.HandleAdmin(testingConstants.Item1);
            Assert.IsTrue(SerializedElementMatchesString(outElement, testingConstants.Item2));
        }

        [TestMethod]
        public void HandleConceptEntry_ValidInput_Test()
        {
            (XElement, string) testingConstants = GetResourceSourceAndExpected("conceptEntry");
            XElement outElement = TBXHandlers.HandleConceptEntry(testingConstants.Item1);
            Assert.IsTrue(SerializedElementMatchesString(outElement, testingConstants.Item2));
        }

        [TestMethod]
        public void HandleDate_ValidInput_Test()
        {
            (XElement, string) testingConstants = GetResourceSourceAndExpected("date");
            XElement outElement = TBXHandlers.HandleDate(testingConstants.Item1);
            Assert.IsTrue(SerializedElementMatchesString(outElement, testingConstants.Item2));
        }

        [TestMethod]
        public void HandleDescrip_ValidInput_Test()
        {
            (XElement, string) testingConstants = GetResourceSourceAndExpected("descrip");
            XElement outElement = TBXHandlers.HandleDescrip(testingConstants.Item1);
            Assert.IsTrue(SerializedElementMatchesString(outElement, testingConstants.Item2));
        }

        [TestMethod]
        public void HandleLangSec_ValidInput_Test()
        {
            (XElement, string) testingConstants = GetResourceSourceAndExpected("langSec");
            XElement outElement = TBXHandlers.HandleLangSec(testingConstants.Item1);
            Assert.IsTrue(SerializedElementMatchesString(outElement, testingConstants.Item2));
        }

        [TestMethod]
        public void HandleNote_ValidInput_Test()
        {
            (XElement, string) testingConstants = GetResourceSourceAndExpected("note");
            XElement outElement = TBXHandlers.HandleNote(testingConstants.Item1);
            Assert.IsTrue(SerializedElementMatchesString(outElement, testingConstants.Item2));
        }

        [TestMethod]
        public void HandleTerm_ValidInput_Test()
        {
            (XElement, string) testingConstants = GetResourceSourceAndExpected("term");
            XElement outElement = TBXHandlers.HandleTerm(testingConstants.Item1);
            Assert.IsTrue(SerializedElementMatchesString(outElement, testingConstants.Item2));
        }

        public void HandleTermNote_ValidInput_Test()
        {
            (XElement, string) testingConstants = GetResourceSourceAndExpected("termNote");
            XElement outElement = TBXHandlers.HandleTermNote(testingConstants.Item1);
            Assert.IsTrue(SerializedElementMatchesString(outElement, testingConstants.Item2));
        }

        [TestMethod]
        public void HandleTermSec_ValidInput_Test()
        {
            (XElement, string) testingConstants = GetResourceSourceAndExpected("termSec");
            XElement outElement = TBXHandlers.HandleTermSec(testingConstants.Item1);
            Assert.IsTrue(SerializedElementMatchesString(outElement, testingConstants.Item2));
        }

        [TestMethod]
        public void HandleTransacGrp_ValidInput_Test()
        {
            (XElement, string) testingConstants = GetResourceSourceAndExpected("transacGrp");
            XElement outElement = TBXHandlers.HandleTransacGrp(testingConstants.Item1);
            Assert.IsTrue(SerializedElementMatchesString(outElement, testingConstants.Item2));
        }

        [TestMethod]
        public void HandleXref_ValidInput_Test()
        {
            (XElement, string) testingConstants = GetResourceSourceAndExpected("xref");
            XElement outElement = TBXHandlers.HandleXref(testingConstants.Item1);
            Assert.IsTrue(SerializedElementMatchesString(outElement, testingConstants.Item2));
        }
    }
}
