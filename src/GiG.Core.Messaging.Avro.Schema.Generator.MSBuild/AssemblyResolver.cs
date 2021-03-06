﻿using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyModel.Resolution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

[assembly: InternalsVisibleTo("GiG.Core.Messaging.Avro.Schema.Generator.Tests.Integration")]
namespace GiG.Core.Messaging.Avro.Schema.Generator.MSBuild
{
    /// <inheritdoc />
    /// <summary>
    /// Simple class that loads the reference assemblies upon the AppDomain.AssemblyResolve.
    /// </summary>
    internal class AssemblyResolver : IDisposable
    {
        private readonly ICompilationAssemblyResolver _assemblyResolver;
        private readonly DependencyContext _resolverDependencyContext;
        private readonly AssemblyLoadContext _loadContext;

        public AssemblyResolver()
        {
            _resolverDependencyContext = DependencyContext.Load(typeof(AssemblyResolver).Assembly);
            var codeGenPath = Path.GetDirectoryName(new Uri(typeof(AssemblyResolver).Assembly.CodeBase).LocalPath);
            _assemblyResolver = new CompositeCompilationAssemblyResolver(
                new ICompilationAssemblyResolver[]
                {
                    new AppBaseCompilationAssemblyResolver(codeGenPath),
                    new ReferenceAssemblyPathResolver(),
                    new PackageCompilationAssemblyResolver()
                });

            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;

            _loadContext = AssemblyLoadContext.GetLoadContext(typeof(AssemblyResolver).Assembly);
            _loadContext.Resolving += AssemblyLoadContextResolving;
            if (_loadContext != AssemblyLoadContext.Default)
            {
                AssemblyLoadContext.Default.Resolving += AssemblyLoadContextResolving;
            }
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= ResolveAssembly;

            _loadContext.Resolving -= AssemblyLoadContextResolving;
            if (_loadContext != AssemblyLoadContext.Default)
            {
                AssemblyLoadContext.Default.Resolving -= AssemblyLoadContextResolving;
            }
        }

        /// <summary>
        /// Handles System.AppDomain.AssemblyResolve event of an System.AppDomain.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The event data.</param>
        /// <returns>The assembly that resolves the type, assembly, or resource or null if the assembly cannot be resolved.
        /// </returns>
        private Assembly ResolveAssembly(object sender, ResolveEventArgs args) => AssemblyLoadContextResolving(null, new AssemblyName(args.Name));

        private Assembly AssemblyLoadContextResolving(AssemblyLoadContext context, AssemblyName name)
        {
            // Attempt to resolve the library from one of the dependency contexts.
            var library = _resolverDependencyContext?.RuntimeLibraries?.FirstOrDefault(NamesMatch);
            if (library == null) return null;

            var wrapper = new CompilationLibrary(
                library.Type,
                library.Name,
                library.Version,
                library.Hash,
                library.RuntimeAssemblyGroups.SelectMany(g => g.AssetPaths),
                library.Dependencies,
                library.Serviceable);

            var assemblies = new List<string>();
            if (_assemblyResolver.TryResolveAssemblyPaths(wrapper, assemblies))
            {
                foreach (var assembly in assemblies.Select(TryLoadAssemblyFromPath).Where(assembly => assembly != null))
                {
                    return assembly;
                }
            }

            return null;

            bool NamesMatch(RuntimeLibrary runtime) => string.Equals(runtime.Name, name.Name, StringComparison.OrdinalIgnoreCase);
        }

        private Assembly TryLoadAssemblyFromPath(string path)
        {
            try
            {
                return _loadContext.LoadFromAssemblyPath(path);
            }
            catch
            {
                return null;
            }
        }
    }
}
