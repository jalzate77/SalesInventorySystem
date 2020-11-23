using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SalesAndInventorySystem.Common
{
    public static class TextboxEvents
    {
        public static void TextBox_KeyDown_IntOnly(object sender, KeyEventArgs e)
        {
            var source = sender as TextBox;

            bool isNumber = (e.Key >= Key.D0 && e.Key <= Key.D9);
            bool isLR = (e.Key == Key.Left || e.Key == Key.Right);

            if(!isNumber && !isLR)
            {
                e.Handled = true;
            }
        }

        public static void TextBox_TextChanged_IntOnly(object sender, TextChangedEventArgs e)
        {
            var source = sender as TextBox;
            var text = source.Text;

            if(int.TryParse(text.Trim(), out int result ))
            {
                source.Text = result + "";
            }
            else
            {
                // sanitize here if neccessary
            }
        }

        public static void TextBox_Validate_IntOnly(object sender)
        {
            var source = sender as TextBox;
            var text = source.Text;

            if (int.TryParse(text.Trim(), out int result))
            {
                source.Text = result + "";
            }
            else
            {
                // sanitize here if neccessary
            }
        }
    }
}
