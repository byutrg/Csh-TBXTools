using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using TBXTools.Models;

namespace TBXTools.Data
{
    public class ValidationDatabase : IDisposable
    {
        readonly SQLiteAsyncConnection _database;

        private const string DATABASE_FILE = "tbx_validation_api.db";
        private static string _environmentPath = null;
        private bool disposedValue;

        private static string EnvironmentPath
        {
            get
            {
                if (!string.IsNullOrEmpty(_environmentPath)) return _environmentPath;

                string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                _environmentPath = assemblyFolder;
                return _environmentPath;
            }
        }
        private static string DatabasePath { get => Path.Combine(EnvironmentPath, DATABASE_FILE); }

        public static bool IsReady { get; set; } = false;

        public ValidationDatabase()
        {
            var dbBytes = Resources.tbx_validation_api;

            bool rewriteDB = true;
            
            if (File.Exists(DatabasePath))
            {
                using (var md5 = MD5.Create())
                {
                    byte[] existingMD5;
                    byte[] inMemoryMD5;

                    var existingDBBytes = File.ReadAllBytes(DatabasePath);
                    existingMD5 = md5.ComputeHash(existingDBBytes);
                    inMemoryMD5 = md5.ComputeHash(dbBytes);

                    if (existingMD5.Equals(inMemoryMD5)) rewriteDB = false;
                }
            }

            if (rewriteDB)
            {
                using (var dbFileWriter = File.Create(DatabasePath))
                {
                    dbFileWriter.Write(dbBytes, 0, dbBytes.Length);
                }
            }

            _database = new SQLiteAsyncConnection(DatabasePath);
        }

        

        public Task<List<Dialect>> GetDialectsAsync()
        {
            return _database.GetAllWithChildrenAsync<Dialect>();
        }

        public Task<Dialect> GetDialectAsync(string name)
        {
            var dialect = _database.Table<Dialect>().Where(d => d.name.ToLower().Equals(name.ToLower())).FirstOrDefaultAsync().Result;
            if (dialect == default(Dialect)) return null;
            return _database.GetWithChildrenAsync<Dialect>(dialect.id);
        }

        public Task<List<Models.Module>> GetModulesAsync()
        {
            return _database.GetAllWithChildrenAsync<Models.Module>();
        }

        public Task<Models.Module> GetModuleAsync(string name)
        {
            var module = _database.Table<Models.Module>().Where(m => m.name.ToLower().Equals(name.ToLower())).FirstOrDefaultAsync().Result;
            return _database.GetWithChildrenAsync<Models.Module>(module.id);
        }

        #region IDisposable implementation
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                if (File.Exists(DatabasePath))
                {
                    try
                    {
                        File.Delete(DatabasePath);
                    } catch (IOException)
                    {
                        disposedValue = false;
                        return;
                    }
                }
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~ValidationDatabase()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
