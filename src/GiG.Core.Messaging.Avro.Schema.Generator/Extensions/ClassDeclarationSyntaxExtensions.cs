using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace GiG.Core.Messaging.Avro.Schema.Generator.Extensions
{
    internal static class ClassDeclarationSyntaxExtensions
    {
        private const string PublicModifier = "public";
        private const string PartialModifier = "partial";
        
        internal static bool IsPublic(this ClassDeclarationSyntax classDeclaration) => HasModifier(classDeclaration, PublicModifier);

        internal static bool IsPartial(this ClassDeclarationSyntax classDeclaration) => HasModifier(classDeclaration, PartialModifier);

        private static bool HasModifier(BaseTypeDeclarationSyntax classDeclaration, string modifier) => classDeclaration != null && classDeclaration.Modifiers.Any(s => s.Text.Equals(modifier));
    }
}