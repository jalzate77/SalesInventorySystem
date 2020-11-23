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
    /// Interaction logic for ItemView.xaml
    /// </summary>
    public partial class ItemView : ModernWpf.Controls.Page
    {
        public ItemView()
        {
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextboxEvents.TextBox_KeyDown_IntOnly(sender, e);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //TextboxEvents.TextBox_TextChanged_IntOnly(sender, e);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextboxEvents.TextBox_Validate_IntOnly(sender);
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            GenericEvents.ScrollViewer_PreviewMouseWheel(sender, e);
        }
    }
}
