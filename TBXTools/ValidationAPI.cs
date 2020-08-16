using System;
using System.Collections.Generic;
using TBXTools.Data;
using TBXTools.Models;

namespace TBXTools
{
    public static class ValidationAPI
    {
        static ValidationDatabase _validationDatabase;

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
            return ValidationDatabase.GetDialectsAsync().Result;
        }

        public static Dialect GetDialect(string name)
        {
            return ValidationDatabase.GetDialectAsync(name).Result;
        }

        public static List<Module> GetModules()
        {
            return ValidationDatabase.GetModulesAsync().Result;
        }

        public static Module GetModule(string name)
        {
            return ValidationDatabase.GetModuleAsync(name).Result;
        }
    }
}
