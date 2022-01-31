using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.CodeDom;
using System.Reflection;

namespace RevitAddin.CodeCompileTest.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            NewCodeDomClass.BuildHelloWorldGraph();

            foreach (var item in Assembly.GetExecutingAssembly().GetTypes())
            {
                Console.WriteLine(item);
            }

            return Result.Succeeded;
        }
    }
    class NewCodeDomClass
    {
        CodeCompileUnit targetUnit;
        CodeTypeDeclaration targetClass;
        public NewCodeDomClass(string nameassembly)
        {
            targetUnit = new CodeCompileUnit();
            CodeNamespace samples = new CodeNamespace(nameassembly);
            samples.Imports.Add(new CodeNamespaceImport("Methods.ClassWithIExternalCommand"));
            targetClass = new CodeTypeDeclaration(nameassembly);
            targetClass.IsClass = true;
            targetClass.TypeAttributes = TypeAttributes.Public;
            targetClass.BaseTypes.Add(typeof(Autodesk.Revit.UI.IExternalCommand));
            samples.Types.Add(targetClass);
            targetUnit.Namespaces.Add(samples);
        }

        // Build a Hello World program graph using
        // System.CodeDom types.
        public static CodeCompileUnit BuildHelloWorldGraph()
        {
            // Create a new CodeCompileUnit to contain
            // the program graph.
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            // Declare a new namespace called Samples.
            CodeNamespace samples = new CodeNamespace("Samples");
            // Add the new namespace to the compile unit.
            compileUnit.Namespaces.Add(samples);

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

            return compileUnit;
        }
    }
}