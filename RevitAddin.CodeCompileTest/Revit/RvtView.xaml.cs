using Autodesk.Windows;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RevitAddin.CodeCompileTest.Revit
{
    public partial class RvtView : Window
    {
        public List<string> Items { get; set; } = new List<string>() { "hi" };
        public RvtView()
        {
            InitializeComponent();
            InitializeWindow();
            //RibbonCombo ribbonCombo = new RibbonCombo();
            //this.Content = ribbonCombo;
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