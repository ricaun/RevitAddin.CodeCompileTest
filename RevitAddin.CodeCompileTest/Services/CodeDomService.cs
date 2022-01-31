using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RevitAddin.CodeCompileTest.Services
{
    public class CodeDomService
    {
        public Assembly CreateCommand()
        {
            string source = @"
            using Autodesk.Revit.Attributes;
            using Autodesk.Revit.DB;
            using Autodesk.Revit.UI;
            namespace CodeNamespace 
            {
              [Transaction(TransactionMode.Manual)]
              public class Command : IExternalCommand
              {     
                public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
                {
                    UIApplication uiapp = commandData.Application;

                    System.Windows.MessageBox.Show(this.GetType().Name + '\n' + this.GetType().Assembly.GetName().Name);

                    return Result.Succeeded;
                }
              }
              public class Teste {}
            }";

            var targetUnit = new CodeSnippetCompileUnit(source);

            return GenerateCode(targetUnit);
        }

        public CodeCompileUnit BuildHelloWorldGraph()
        {

            CodeCompileUnit targetUnit = new CodeCompileUnit();

            // Declare a new namespace called Samples.
            CodeNamespace samples = new CodeNamespace("Samples");
            // Add the new namespace to the compile unit.
            targetUnit.Namespaces.Add(samples);

            // Add the new namespace import for the System namespace.
            samples.Imports.Add(new CodeNamespaceImport("System"));

            // Declare a new type called Class1.
            CodeTypeDeclaration class1 = new CodeTypeDeclaration("Class1");
            // Add the new type to the namespace type collection.
            samples.Types.Add(class1);

            // Declare a new code entry point method.
            CodeEntryPointMethod start = new CodeEntryPointMethod();

            // Create a type reference for the System.Console class.
            CodeTypeReferenceExpression csSystemConsoleType = new CodeTypeReferenceExpression("System.Console");

            // Build a Console.WriteLine statement.
            CodeMethodInvokeExpression cs1 = new CodeMethodInvokeExpression(
                csSystemConsoleType, "WriteLine",
                new CodePrimitiveExpression("Hello World!"));

            // Add the WriteLine call to the statement collection.
            start.Statements.Add(cs1);

            // Add the code entry point method to
            // the Members collection of the type.
            class1.Members.Add(start);

            return targetUnit;
        }

        public Assembly GenerateCode(params CodeCompileUnit[] compilationUnits)
        {
            CodeDomProvider provider = new CSharpCodeProvider();
            CompilerParameters compilerParametes = new CompilerParameters();

            compilerParametes.GenerateExecutable = false;
            compilerParametes.IncludeDebugInformation = false;
            compilerParametes.GenerateInMemory = false;

            #region Add GetReferencedAssemblies
            var assemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            var nameAssemblies = new Dictionary<string, Assembly>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assemblyNames.Any(e => e.Name == assembly.GetName().Name))
                {
                    nameAssemblies[assembly.GetName().Name] = assembly;
                }
            }
            foreach (var keyAssmbley in nameAssemblies)
            {
                //Console.WriteLine($"Assmbley: {keyAssmbley.Key}");
                compilerParametes.ReferencedAssemblies.Add(keyAssmbley.Value.Location);
            }
            #endregion

            CompilerResults results = provider.CompileAssemblyFromDom(compilerParametes, compilationUnits);
            return results.CompiledAssembly;
        }
    }
}