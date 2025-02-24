﻿using SalesAndInventorySystem.Common;
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
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : ModernWpf.Controls.Page
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            GenericEvents.ScrollViewer_PreviewMouseWheel(sender, e);
        }
    }
}
