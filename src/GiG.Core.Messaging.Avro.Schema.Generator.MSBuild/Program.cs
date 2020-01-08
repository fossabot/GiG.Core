using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace GiG.Core.Messaging.Avro.Schema.Generator.MSBuild
{
    /// <summary>
    /// The Application Program. 
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        public static void Main(string[] args)
        {
            // Un-comment below lines to Debug this project.
            //var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent?.Parent?.FullName;
            //args = new[] {"SourceToSource", projectDirectory + "\\sample.g.args.txt"};

            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            Console.WriteLine("Avro.CodeGenerator - command-line = {0}", string.Join(" ", args));

            try
            {
                SourceToSource(args.Skip(1).ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine("-- Code Generation FAILED -- \n{0}", ex.Message);
            }
        }
        
        /// <summary>
        /// Entry point for code generation process.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void SourceToSource(string[] args)
        {
            var logger = new LoggerFactory()
                // TODO Handle Logging 
                // .AddConsole()
                .CreateLogger<CodeGenerator>();

            using (new AssemblyResolver())
            {
                var cmd = new CodeGeneratorCommand {Log = logger};

                var argsFile = args[0].Trim('"');
                if (!File.Exists(argsFile)) 
                    throw new ArgumentException($"Arguments file \"{argsFile}\" does not exist.");
                
                var fileArgs = File.ReadAllLines(argsFile);

                foreach (var arg in fileArgs)
                {
                    var parts = arg.Split(new[] {':'}, 2);
                    if (parts.Length > 2) throw new ArgumentException($"Argument \"{arg}\" cannot be parsed.");
                    var key = parts[0];
                    var value = parts.Skip(1).SingleOrDefault();

                    switch (key)
                    {
                        case "WaitForDebugger":
                            var i = 0;
                            while (!Debugger.IsAttached)
                            {
                                if (i++ % 50 == 0)
                                {
                                    Console.WriteLine("Waiting for debugger to attach.");
                                }

                                Thread.Sleep(100);
                            }

                            break;
                        case nameof(cmd.ProjectGuid):
                            cmd.ProjectGuid = value;
                            break;
                        case nameof(cmd.ProjectPath):
                            cmd.ProjectPath = value;
                            break;
                        case nameof(cmd.OutputType):
                            cmd.OutputType = value;
                            break;
                        case nameof(cmd.TargetPath):
                            cmd.TargetPath = value;
                            break;
                        case nameof(cmd.AssemblyName):
                            cmd.AssemblyName = value;
                            break;
                        case nameof(cmd.Compile):
                            cmd.Compile.Add(value);
                            break;
                        case nameof(cmd.Reference):
                            cmd.Reference.Add(value);
                            break;
                        case nameof(cmd.DefineConstants):
                            cmd.DefineConstants.AddRange(value?.Split(',', StringSplitOptions.RemoveEmptyEntries));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException($"Key \"{key}\" in argument file is unknown.");
                    }
                }

                var stopwatch = Stopwatch.StartNew();
                cmd.Execute(CancellationToken.None).GetAwaiter().GetResult();
                cmd.Log.LogInformation($"Total code generation time: {stopwatch.ElapsedMilliseconds}ms.");
            }
        }
    }
}