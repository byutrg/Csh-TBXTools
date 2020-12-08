using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using TBXTools.ConversionAPI.MTF.Handlers;
using System.Text.RegularExpressions;

namespace TBXTools.Test
{
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
            string xElementStr = Regex.Replace(xElement.ToString(), @"\s+", " ");
            return xElementStr.Equals(Regex.Replace(expected, @"\s+", " "));
        }

        [TestMethod]
        public void HandleTransacGrp_ValidInput_Test()
        {
            (XElement, string) testingConstants = GetResourceSourceAndExpected("transacGrp");

            XElement outElement = TBXHandlers.HandleTransacGrp(testingConstants.Item1);
            Assert.IsTrue(SerializedElementMatchesString(outElement, testingConstants.Item2));
        }
    }
}
