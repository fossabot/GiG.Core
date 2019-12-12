using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace GiG.Core.Messaging.Avro.Schema.Generator.MSBuild.Tasks
{
    /// <summary>
    /// Gets the Dot Net Execution Path.
    /// </summary>
    public class GetDotNetHost : Task
    {
        /// <summary>
        /// Dot Net Execution Path.
        /// </summary>
        [Output]
        // ReSharper disable once MemberCanBePrivate.Global
        public string DotNetHost { get; set; }

        /// <summary>
        /// Execute the retrieval of the Dot Net Execution Path. 
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            DotNetHost = TryFindDotNetExePath();

            return true;
        }

        private static string TryFindDotNetExePath() => DotNetMuxer.MuxerPathOrDefault();
    }
}