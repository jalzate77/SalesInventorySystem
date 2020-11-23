using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
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
    [Export(typeof(SplashScreen))]
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : ModernWpf.Controls.ContentDialog, INotifyPropertyChanged
    {
        private string message;

        public SplashScreen()
        {
            InitializeComponent();
            DataContext = this;
            Message = "Loading...";
        }

        public string Message
        {
            get => message;
            set
            {
                message = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
