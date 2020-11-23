using SalesAndInventorySystem.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SalesAndInventorySystem.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : ModernWpf.Controls.Page
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox t = sender as TextBox;

            t.Text = t.Text.Trim();

            if (t.Text.Trim().Length == 1)
            {
                e.Handled = true;
            }
        }
    }
}
