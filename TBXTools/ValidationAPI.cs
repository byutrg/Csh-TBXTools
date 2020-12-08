using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using TBXTools.Data;
using TBXTools.Models;

namespace TBXTools
{
    public static class ValidationAPI
    {
        static ValidationDatabase _validationDatabase;

        public static XNamespace Namespace { get => "urn:iso:std:iso:30042:ed-2"; }

        public enum Style
        {
            dca,
            dct
        }

        public static ValidationDatabase ValidationDatabase
        {
            get
            {
                if (_validationDatabase == null)
                {
                    _validationDatabase = new ValidationDatabase();
                }
                return _validationDatabase;
            }
        }

        public static List<Dialect> GetDialects()
        {
            return ValidationDatabase.GetDialectsAsync()?.Result;
        }

        public static Dialect GetDialect(string name)
        {
            return ValidationDatabase.GetDialectAsync(name)?.Result;
        }

        public static List<Module> GetModules()
        {
            return ValidationDatabase.GetModulesAsync()?.Result;
        }

        public static Module GetModule(string name)
        {
            return ValidationDatabase.GetModuleAsync(name)?.Result;
        }


        public static class Dialects
        {
            public static class DCA
            {
                public static string GetDefinition(string name) => GetDefinition(GetDialect(name));
                public static string GetDefinition(Dialect dialect)
                {
                    return Resources.ResourceManager.GetString(Path.GetFileName(dialect.definition));
                }

                public static string GetRNGContents(string name) => GetRNGContents(GetDialect(name));
                public static string GetRNGContents(Dialect dialect)
                {
                    return Resources.ResourceManager.GetString(Path.GetFileName(dialect.dca_rng));
                }

                public static string GetSCHContents(string name) => GetSCHContents(GetDialect(name));
                public static string GetSCHContents(Dialect dialect)
                {
                    return Resources.ResourceManager.GetString(Path.GetFileName(dialect.dca_sch));
                }
            }

            public static class DCT
            {
                public static string GetNVDLContents(string name) => GetNVDLContents(GetDialect(name));
                public static string GetNVDLContents(Dialect dialect)
                {
                    return Resources.ResourceManager.GetString(Path.GetFileName(dialect.dct_nvdl));
                }
                public static string GetSCHContents(string name) => GetSCHContents(GetDialect(name));
                public static string GetSCHContents(Dialect dialect)
                {
                    return Resources.ResourceManager.GetString(Path.GetFileName(dialect.dct_sch));
                }
            }
        }


        public static class Modules
        {
            public static string GetDefinition(string name) => GetDefinition(GetModule(name));
            public static string GetDefinition(Module module)
            {
                return Resources.ResourceManager.GetString(Path.GetFileName(module.definition));
            }

            public static string GetRNGContents(string name) => GetRNGContents(GetModule(name));
            public static string GetRNGContents(Module module)
            {
                return Resources.ResourceManager.GetString(Path.GetFileName(module.rng));
            }
        }

    }
}
