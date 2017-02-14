using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.DotNet.ProjectModel.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.FindSymbols;

namespace Slipstream.CommonDotNet.Commands.Analyzer.Playground
{
    class Program
    {
        private static readonly List<ITypeSymbol> InterfaceSymbols = new List<ITypeSymbol>();
        private static readonly List<ITypeSymbol> ReturnSymbols = new List<ITypeSymbol>();

        private static readonly ProjectJsonWorkspace Workspace = new ProjectJsonWorkspace(@"D:\Source\GitHub\DreamNucleus\Commands\Slipstream.CommonDotNet.Commands.Playground\");
        private static readonly Solution Solution = Workspace.CurrentSolution;
        private static readonly Document Document = Solution.Projects.SelectMany(p => p.Documents.Where(d => d.Name.EndsWith("FakeCommand.cs"))).Single();


        static void Main(string[] args)
        {
            //var file = @"D:\Source\GitHub\DreamNucleus\Commands\Commands.sln";

            //var workspace = MSBuildWorkspace.Create();
            //var solution = workspace.OpenSolutionAsync(file).Result;

            //var test = Solution.Projects.First().GetCompilationAsync().Result.ReferencedAssemblyNames.Where(t => t.Name.Contains("Autofac"));
            var test = Solution.Projects.First().GetCompilationAsync().Result
                .GetSemanticModel(Solution.Projects.First().GetCompilationAsync().Result.SyntaxTrees.First());

            var methodSyntaxes = Document.GetSyntaxRootAsync().Result.DescendantNodes().OfType<MethodDeclarationSyntax>();


            foreach (var methodDeclarationSyntax in methodSyntaxes)
            {
                if (methodDeclarationSyntax.Identifier.Text == "ExecuteAsync")
                {
                    AddInterfaceSymbols(methodDeclarationSyntax);
                    AddReturnSymbolInfo(methodDeclarationSyntax);
                }
            }

            Console.ReadKey();

            var testas = SymbolFinder.FindReferencesAsync(InterfaceSymbols[1], Solution).Result;


            Console.WriteLine();
        }

        private static void AddInterfaceSymbols(MethodDeclarationSyntax methodDeclarationSyntax)
        {

            var methodSymbol = Document.GetSemanticModelAsync().Result.GetSymbolInfo(methodDeclarationSyntax.ParameterList.Parameters.Single().DescendantNodes().Single(n => n is IdentifierNameSyntax));

            var commandInterfaces = ((INamedTypeSymbol)methodSymbol.Symbol).Interfaces;

            // get the success
            var successResult = commandInterfaces.Single(i => i.Name.Contains("ISuccessResult")).TypeArguments.Last();

            // get the errors
            var errorResults = commandInterfaces.SelectMany(i => i.AllInterfaces.Where(ai => ai.Name.Contains("IErrorResult"))).Select(i => i.TypeArguments.Single());

            InterfaceSymbols.Add(successResult);
            InterfaceSymbols.AddRange(errorResults);
        }

        private static void AddReturnSymbolInfo(MethodDeclarationSyntax methodDeclarationSyntax)
        {
            var returnStatements = methodDeclarationSyntax.DescendantNodes().OfType<ReturnStatementSyntax>();

            foreach (var returnStatementSyntax in returnStatements)
            {
                var semanticModel = Solution.GetDocument(methodDeclarationSyntax.SyntaxTree).GetSemanticModelAsync().Result;
                var nodes = returnStatementSyntax.DescendantNodes().ToList();
                var firstNode = nodes.First();


                Console.Write(firstNode.Kind() + " : " + firstNode.GetType().Name +  " : " + firstNode + " : " + nodes.Count());

                // this works quite well. if we used another command returned its result... we would need to check that?
                var typeInfo = semanticModel.GetTypeInfo(returnStatementSyntax.Expression);

                if (typeInfo.Type != null)
                {
                    Console.Write("  ||  " + typeInfo.Type.Name);
                }
                Console.WriteLine();


                //if (returnStatementSyntax.DescendantNodes().OfType<InvocationExpressionSyntax>().Any())
                //{
                //    // then find return statements inside there;
                //    var test = semanticModel.GetSymbolInfo(returnStatementSyntax.DescendantNodes().OfType<InvocationExpressionSyntax>().First());
                //    var check = test.Symbol.DeclaringSyntaxReferences.Single();
                //    AddReturnSymbolInfo(check.GetSyntaxAsync().Result as MethodDeclarationSyntax);
                //}

                

                ReturnSymbols.AddRange(returnStatementSyntax.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().SelectMany(c => c.DescendantNodes()
                    .Where(n => n is IdentifierNameSyntax))
                    .Select(i => semanticModel.GetSymbolInfo(i).Symbol as INamedTypeSymbol).ToList());

            }

            //Console.ReadKey();
        }
    }
}
