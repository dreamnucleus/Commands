using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace Slipstream.CommonDotNet.Commands.Analyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SlipstreamCommonDotNetCommandsAnalyzerCodeFixProvider)), Shared]
    public class SlipstreamCommonDotNetCommandsAnalyzerCodeFixProvider : CodeFixProvider
    {
        private const string Title = "Add missing interface to command";

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(SlipstreamCommonDotNetCommandsAnalyzer.MissingInterfaceDiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            // TODO: Replace the following code with your own analysis, generating a CodeAction for each fix to suggest
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: Title,
                    createChangedSolution: c => AddInterface(context.Document, declaration, diagnostic.Properties["Return"], c),
                    equivalenceKey: Title),
                diagnostic);
        }

        private async Task<Solution> AddInterface(Document document, ClassDeclarationSyntax declaration, string diagnosticProperty, CancellationToken cancellationToken)
        {
            var toAdd = (await document.Project.GetCompilationAsync(cancellationToken)).GlobalNamespace
                .GetNamespaceMembers().SelectMany(GetAllTypeMembers)
                .FirstOrDefault(t => t.CanBeReferencedByName && t.Interfaces
                    .Any(i => i.ToDisplayString() == $"Slipstream.CommonDotNet.Commands.Results.IErrorResult<{diagnosticProperty}>"));

            if (toAdd != null)
            {
                var newClass = declaration.AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(toAdd.Name)));

                var root = await document.GetSyntaxRootAsync(cancellationToken);
                var newRoot = root.ReplaceNode(declaration, newClass);

                return document.WithSyntaxRoot(newRoot).Project.Solution;
            }
            else
            {
                return document.Project.Solution;
            }
        }

        private IEnumerable<INamedTypeSymbol> GetAllTypeMembers(INamespaceSymbol namespaceSymbol)
        {
            var namedTypeSymbols = new List<INamedTypeSymbol>();

            namedTypeSymbols.AddRange(namespaceSymbol.GetTypeMembers().Where(t => t.CanBeReferencedByName));

            foreach (var namespaceMember in namespaceSymbol.GetNamespaceMembers())
            {
                namedTypeSymbols.AddRange(GetAllTypeMembers(namespaceMember));
            }

            return namedTypeSymbols;
        }
    }
}