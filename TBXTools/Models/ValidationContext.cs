﻿using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TBXTools.Models
{
    [Table("dialects")]
    public class Dialect
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        [Unique, Collation("NOCASE")]
        public string name { get; set; }
        public string definition { get; set; }
        public string dca_rng { get; set; }
        public string dca_sch { get; set; }
        public string dct_nvdl { get; set; }
        public string dct_sch { get; set; }
        [ManyToMany(typeof(DialectModule))]
        public List<Module> modules { get; set; }

        public override string ToString()
        {
            return 
$@"Name: {name}
    Definition: {definition}
    DCA:
        RNG: {dca_rng}
        SCH: {dca_sch}
    DCT:
        NVDL: {dct_nvdl}
        SCH: {dct_sch}";
        }
    }

    [Table("modules")]
    public class Module
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        [Unique, Collation("NOCASE")]
        public string name { get; set; }
        public string definition { get; set; }
        public string rng { get; set; }
        public string sch { get; set; }
        public string tbxmd { get; set; }

        //[ManyToMany(typeof(DialectModule))] // we don't really need this.
        public List<Dialect> dialects { get; }

        public override string ToString()
        {
            return 
$@"Name: {name}
    Definition: {definition}
    RNG: {rng}
    SCH: {sch}
    SCH: {tbxmd}";
        }
    }

    [Table("dialects_modules")]
    public class DialectModule
    {
        [ForeignKey(typeof(Dialect)), PrimaryKey]
        public int dialects_id { get; set; }
        [ForeignKey(typeof(Module)), PrimaryKey]
        public int modules_id { get; set; }
    }
}
