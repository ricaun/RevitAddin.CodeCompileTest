using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ricaun.Revit.UI;
using System;
using System.Linq;

namespace RevitAddin.CodeCompileTest.Revit
{
    [Console]
    public class App : IExternalApplication
    {
        public static RibbonPanel ribbonPanel;
        public Result OnStartup(UIControlledApplication application)
        {
            ribbonPanel = application.CreatePanel("CodeCompile");
            _ = ribbonPanel.GetRibbonPanel().RibbonControl;

            var solidColorBrush = new System.Windows.Media.SolidColorBrush(
                new System.Windows.Media.Color()
                {
                    R = 255,
                    G = 155,
                    B = 255,
                    A = 100
                });
            ribbonPanel.GetRibbonPanel().CustomPanelTitleBarBackground = solidColorBrush;
            ribbonPanel.GetRibbonPanel().CustomPanelBackground = solidColorBrush;
            ribbonPanel.GetRibbonPanel().CustomSlideOutPanelBackground = System.Windows.Media.Brushes.Red;



            var b = ribbonPanel.AddPushButton<Commands.Command>();
            ribbonPanel.AddPushButton<Commands.Command<string>>("string");
            ribbonPanel.AddPushButton<Commands.Command<int>>("int");

            ribbonPanel.AddItem(NewPushButtonData<Commands.Command<double>>("double"));
            ribbonPanel.AddItem(NewPushButtonData<Commands.Command<App>>("App"));

            foreach (var item in ribbonPanel.GetRibbonItems())
            {
                switch (item)
                {
                    case RibbonButton button:
                        button.SetLargeImage(BitmapExtension.GetBitmapSource("iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAACQAAAAkABgV+WigAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAPaSURBVFiFvZddbBRVGIaf+dnd2bp/dQu0paStho2kQVpNL0RDQ/WiCVZEozHGLImpvS0iBIxKkTRGEU29gVTlRgJBuRQ1xliLEGmDSa1EbWJtKy3VdmmVsmt3Z3/Gi+ngst2fmSHxTc7Fzvm+931nvzPnfEfAPHzANqAV2ATUAYHlub+BSeAHoB/4DLhhgbsoQsBxIAZoJkcM+BBYfzvCbuCI4FCSFoRzhwocBhSr4uuBy4BW0dGnBcPvaWKZ364JDbgIVJkVbwLmjOSKzuNa/SlNq33/muZr69IQJbsmpoB7c8XEPG/+JbBqRaAnSDDcy9qeSyj3bDH7MtmoQV+clYUMKMCZfOLZcNY1UXXgHGv2foq8qt6OibPo6wsAKWvyTWBHbkbZ/dtx1TWtYHJUhfA93Ino9pIYG0JLqWZNVANpYAD++wdCQJdZBgOC042/fR8174zibe0EIbeiBbGb5VIYGfsA2aoBA1J5NRUdfVQfGkQJbTaT4gEOGAZ8wDN2xbPhuruZqu4LrO76BDm4rlR4GPBKwBPFDDjXbcTd0GrehSDgrGnA29oBmQzqxPeQSeelBkYk9NrfV4gvPnqe+M8DuOoakQKVhcJW+pBduDc+gmfLTjLRedQrI/nCFiTgNWBtMbLUtd+50f8BqblxlNBmRMVj2ohY5ueO5h24N7SgTg6Tvj6bPZ0UgAhQYZpQ8eDftgf/Y/sRHC7TRgDQMkQvnGTh5B7Si3MAcxLQw637QXGOlEr8lwFigx8jBSpx1jSYNyAIOGs34d3aAYA6fkk2/eHm5XO6SweVgAwsYqUEZQEC2/fja9t1GyV4ifRiBOC6DEyYMiCIeB56jjufPYzkX2NNGFj6qZ+FEy+iXvkx+/G4jN5GNRdLVja0EAz34qxttCycnP2Nv06/TGzoTL7pERm9h3uhEEHg8Vcof7rHsjDA0uWvmD3SjpZMFAr5WkQ/HmOFIuTVd9kSB0jNTxUTjwFfiEAUOG1bxT5OAVHjM3wLSP6P4ip6/3HzOP4V6LXKoqlLaOqSHQPvAuPZBkA/nwfNMvwzfJbpvQ1M7w4RPX8CNM1s6kXgoPEj20AcvSWbKpatTg7zx6EWZt9uJxWZILUwTeRYmJnuB0iMlfQ/AzwF3FyZuVvxn8CjwHRuZiY6z/xHu7j6ajPx0W9XMCfGhpjpfpDIsZ25J56BKaANuFrKJeid8TlsXkxExaOVP3lQCz5/1Hj2HTntuBm4gNcFhxI1K5w7BIeSAN5Y5rKNSuAo+n5hVjwK9AEldzHBghEP+vV8K9AI1HPr9XwCGAa+AT5fNlES/wLKWXKx84783QAAAABJRU5ErkJggg=="));
                        break;
                }
            }

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            ribbonPanel.Remove();

            return Result.Succeeded;
        }

        public PushButtonData NewPushButtonData<TExternalCommand>(string name = null, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            var commandType = typeof(TExternalCommand);
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