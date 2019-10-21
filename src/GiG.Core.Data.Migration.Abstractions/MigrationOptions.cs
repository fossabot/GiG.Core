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
        /// <summary>
        /// A value to indicate if the Migration is enabled or not.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// The Locations of the Scripts to be executed.
        /// </summary>
        public List<string> Locations { get; set; } = new List<string>();

        /// <summary>
        /// The Database Connection to be used for the Migration.
        /// </summary>
        public IDbConnection Connection { get; set; }

        /// <summary>
        /// The Schema Name for the Migration Change Log table.
        /// </summary>
        public string MetadataTableSchema { get; set; } = "";

        /// <summary>
        /// The table name for the Migration Change Log.
        /// </summary>
        public string MetadataTableName { get; set; } = "changelog";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dbConnection">The <see cref="T:System.Data.IDbConnection" /> to be used for the Database Migration.</param>
        public MigrationOptions([NotNull] IDbConnection dbConnection)
        {
            if (dbConnection == null) throw new ArgumentNullException(nameof(dbConnection));

            Connection = dbConnection;
        }
    }
}
