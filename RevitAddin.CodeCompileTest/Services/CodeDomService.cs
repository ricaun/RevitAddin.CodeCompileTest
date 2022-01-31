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
            foreach (var keyAssembly in nameAssemblies)
            {
                //Console.WriteLine($"Assembly: {keyAssembly.Key}");
                compilerParametes.ReferencedAssemblies.Add(keyAssembly.Value.Location);
            }
            #endregion

            CompilerResults results = provider.CompileAssemblyFromDom(compilerParametes, compilationUnits);
            return results.CompiledAssembly;
        }
    }
}