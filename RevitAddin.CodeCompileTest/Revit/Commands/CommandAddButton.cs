using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ricaun.Revit.UI;

using RevitAddin.CodeCompileTest.Services;
using System;
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
                var ribbonPanel = App.ribbonPanel;
                ribbonPanel.AddItem(ribbonPanel.NewPushButtonData(command))
                    .SetLargeImage(GetLargeImageUri().GetBitmapSource());
            }

            return Result.Succeeded;
        }

        public string GetLargeImageUri()
        {
            var baseImage = @"https://img.icons8.com/small/32/000000/circled-{0}.png";
            var imageLarge = string.Format(baseImage, GetLetter());
            return imageLarge;
        }

        public char GetLetter()
        {
            var num = DateTime.Now.Ticks % 26;
            char let = (char)('a' + num);
            return let;
        }
    }
}