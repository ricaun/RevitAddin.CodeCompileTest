using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using RevitAddin.CodeCompileTest.Services;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RevitAddin.CodeCompileTest.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class CommandAddButton : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            CodeDomService newCodeDomClass = new CodeDomService();
            Assembly assembly = newCodeDomClass.CreateCommand();

            foreach (var type in assembly.GetTypes())
            {
                Console.WriteLine($"Type: {type}");
            }

            var commands = assembly.GetTypes().Where(e => typeof(IExternalCommand).IsAssignableFrom(e));
            foreach (var command in commands)
            {
                Console.WriteLine($"Command: {command}");
                var name = $"{command} {DateTime.Now}";
                var text = $"{command.Name}";
                PushButtonData currentBtn = NewPushButton(command, name, text);

                App.ribbonPanel.AddItem(currentBtn);
            }

            return Result.Succeeded;
        }

        private PushButtonData NewPushButton(Type command, string name = null, string text = null)
        {
            var commandType = command;
            var currentDll = commandType.Assembly.Location;
            string fullname = commandType.FullName;
            string targetName = commandType.Name;
            if (name != null) targetName = name;
            PushButtonData currentBtn = new PushButtonData(targetName, targetName, currentDll, fullname);
            if (text != null) currentBtn.Text = text;
            return currentBtn;
        }
    }
}