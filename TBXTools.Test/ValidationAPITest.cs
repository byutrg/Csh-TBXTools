using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TBXTools.Models;

namespace TBXTools.Test
{
    [TestClass]
    public class ValidationAPITest
    {
        [TestMethod]
        public void GetDialects_DialectListNotNullOrEmpty_Test()
        {
            List<Dialect> dialects = ValidationAPI.GetDialects();
            Assert.IsNotNull(dialects);
            Assert.IsTrue(dialects.Count > 0);
        }

        [TestMethod]
        public void GetDialect_TBXFake_Null_Test()
        {
            Dialect dialect = ValidationAPI.GetDialect("TBX-Fake");
            Assert.IsNull(dialect);
        }

        [TestMethod]
        public void GetDialect_TBXBasicLowerCase_TBXBasicDialectObject_Test()
        {
            Dialect dialect = ValidationAPI.GetDialect("tbx-basic");
            Assert.IsNotNull(dialect);
        }

        [TestMethod]
        public void GetModules_ModuleListNotNullOrEmpty_Test()
        {
            List<Module> modules = ValidationAPI.GetModules();
            Assert.IsNotNull(modules);
            Assert.IsTrue(modules.Count > 0);
        }

        [TestMethod]
        public void GetModule_BasicUpperCase_BasicModuleObject_Test()
        {
            Module module = ValidationAPI.GetModule("BASIC");
            Assert.IsNotNull(module);
        }
    }
}
