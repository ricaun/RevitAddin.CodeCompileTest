using System;
using System.Windows;

namespace RevitAddin.CodeCompileTest.Revit
{
    public partial class View1 : Window
    {
        public View1()
        {
            InitializeComponent();
            InitializeWindow();
        }

        #region InitializeWindow
        private void InitializeWindow()
        {
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.ShowInTaskbar = false;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            new System.Windows.Interop.WindowInteropHelper(this) { Owner = Autodesk.Windows.ComponentManager.ApplicationWindow };
        }
        #endregion
    }
}