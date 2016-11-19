using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.FindSymbols;

namespace Slipstream.CommonDotNet.Commands.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SlipstreamCommonDotNetCommandsAnalyzer : DiagnosticAnalyzer
    {
        //public const string MissingInterfaceDiagnosticId = "SlipstreamCommonDotNetCommandsAnalyzerMissingInterface";
        //public const string MissingReturnDiagnosticId = "SlipstreamCommonDotNetCommandsAnalyzerMissingReturn";
        public const string MissingInterfaceDiagnosticId = "SCDNCMD001";
        public const string MissingReturnDiagnosticId = "SCDNCMD002";
        //private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        //private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        //private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));

        private const string Category = "Usage";

        private static DiagnosticDescriptor MissingInterfaceRule = new DiagnosticDescriptor(MissingInterfaceDiagnosticId, 
            "Commands must extend interfaces of all possible return types",
            "'{0}' cannot return '{1}'",
            Category,
            DiagnosticSeverity.Error,
            true,
            "The interfaces on the command is used to determine all possible return types");

        private static DiagnosticDescriptor MissingReturnRule = new DiagnosticDescriptor(MissingReturnDiagnosticId,
            "Command handlers must return all types described on the command",
            "'{0}' must return '{1}'",
            Category,
            DiagnosticSeverity.Error,
            true,
            "The interfaces on the command is used to determine all possible return types");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(MissingInterfaceRule, MissingReturnRule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationAction(AnalyzeCompilation);
        }

        private static readonly SymbolDisplayFormat SymbolDisplayFormat = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

        private void AnalyzeCompilation(CompilationAnalysisContext context)
        {
            var iAsyncCommandHandlers = context.Compilation.SyntaxTrees
                .SelectMany(s => s.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())
                .Where(c => context.Compilation.GetSemanticModel(c.SyntaxTree).GetDeclaredSymbol(c).AllInterfaces.Any(i => i.ToDisplayString(SymbolDisplayFormat) == "Slipstream.CommonDotNet.Commands.IAsyncCommandHandler"))
                .ToList();

            foreach (var iAsyncCommandHandler in iAsyncCommandHandlers)
            {
                var methodDeclarationSyntax = iAsyncCommandHandler.DescendantNodes().OfType<MethodDeclarationSyntax>()
                    .Single(m => m.Identifier.Text == "ExecuteAsync");

                var commandSymbol = (INamedTypeSymbol)context.Compilation.GetSemanticModel(methodDeclarationSyntax.SyntaxTree)
                    .GetSymbolInfo(methodDeclarationSyntax.ParameterList.Parameters.Single().DescendantNodes().Single(n => n is IdentifierNameSyntax)).Symbol;

                var interfaceSymbols = GetInterfaceSymbols(commandSymbol);

                var returnSymbols = GetReturnSymbolInfo(context.Compilation, methodDeclarationSyntax);

                foreach (var returnSymbol in returnSymbols)
                {
                    if (interfaceSymbols.All(i => !returnSymbol.Equals(i)))
                    {
                        var builder = ImmutableDictionary.CreateBuilder<string, string>();
                        builder.Add("Return", returnSymbol.ToDisplayString(SymbolDisplayFormat));

                        var diagnostic = Diagnostic.Create(MissingInterfaceRule, commandSymbol.Locations.Single(), builder.ToImmutable(), iAsyncCommandHandler.Identifier.Text, returnSymbol.Name);
                        context.ReportDiagnostic(diagnostic);
                    }
                }

                foreach (var interfaceSymbol in interfaceSymbols)
                {
                    if (returnSymbols.All(i => !interfaceSymbol.Equals(i)))
                    {
                        var diagnostic = Diagnostic.Create(MissingReturnRule, methodDeclarationSyntax.ReturnType.GetLocation(), iAsyncCommandHandler.Identifier.Text, interfaceSymbol.Name);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }

        private ICollection<ITypeSymbol> GetInterfaceSymbols(INamedTypeSymbol commandSymbol)
        {
            var commandInterfaces = commandSymbol.Interfaces;

            // get the success
            var successResult = commandInterfaces.Single(i => i.Name.Contains("ISuccessResult")).TypeArguments.Last();

            // get the errors
            var errorResults = commandInterfaces.SelectMany(i => i.AllInterfaces.Where(ai => ai.Name.Contains("IErrorResult"))).Select(i => i.TypeArguments.Single());

            var typeSymbols = new List<ITypeSymbol> { successResult };
            typeSymbols.AddRange(errorResults);
            return typeSymbols;
        }

        private ICollection<ITypeSymbol> GetReturnSymbolInfo(Compilation compilation, MethodDeclarationSyntax methodDeclarationSyntax)
        {
            var typeSymbols = new List<ITypeSymbol>();

            var returnStatements = methodDeclarationSyntax.DescendantNodes().OfType<ReturnStatementSyntax>();

            foreach (var returnStatementSyntax in returnStatements)
            {
                var semanticModel = compilation.GetSemanticModel(returnStatementSyntax.SyntaxTree);

                if (returnStatementSyntax.DescendantNodes().OfType<InvocationExpressionSyntax>().Any())
                {
                    // then find return statements inside there;
                    var test = semanticModel.GetSymbolInfo(returnStatementSyntax.DescendantNodes().OfType<InvocationExpressionSyntax>().First());
                    var check = test.Symbol.DeclaringSyntaxReferences.Single();
                    typeSymbols.AddRange(GetReturnSymbolInfo(compilation, check.GetSyntaxAsync().Result as MethodDeclarationSyntax));
                }

                typeSymbols.AddRange(returnStatementSyntax.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().SelectMany(c => c.DescendantNodes()
                    .Where(n => n is IdentifierNameSyntax))
                    .Select(i => semanticModel.GetSymbolInfo(i).Symbol as INamedTypeSymbol).ToList());

            }

            return typeSymbols;
        }
    }

}
