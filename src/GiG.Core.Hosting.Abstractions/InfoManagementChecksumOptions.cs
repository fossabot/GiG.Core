using System;

namespace GiG.Core.Hosting.Abstractions
{
    /// <summary>
    /// Checksum options for Info Management.
    /// </summary>
    public class InfoManagementChecksumOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "InfoManagement:Checksum";
        
        /// <summary>
        /// The root directory.
        /// </summary>
        public string Root { get; set; } = AppContext.BaseDirectory;

        /// <summary>
        /// The path of the file which will contain the checksum.
        /// </summary>
        public string FilePath { get; set; } = "checksum.app.txt";
    }
}