using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitAddin.CodeCompileTest.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Command<T> : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            var t = typeof(T);

            System.Windows.MessageBox.Show(t.Name);

            return Result.Succeeded;
        }
    }
}