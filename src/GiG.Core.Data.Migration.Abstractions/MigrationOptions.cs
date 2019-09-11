using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Data;

namespace GiG.Core.Data.Migration.Abstractions
{
    /// <summary>
    /// Migration Options.
    /// </summary>
    public class MigrationOptions
    {
        public bool Enabled { get; set; } = true;
        public List<string> Locations { get; set; } = new List<string>();
        public IDbConnection Connection { get; set; }
        public string MetadataTableSchema { get; set; } = "";
        public string MetadataTableName { get; set; } = "changelog";

        public MigrationOptions([NotNull] IDbConnection dbConnection)
        {
            if (dbConnection == null) throw new ArgumentNullException(nameof(dbConnection));

            Connection = dbConnection;
        }
    }
}
