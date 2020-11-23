using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;
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
    [Export(typeof(GenericContentDialog))]
    /// <summary>
    /// Interaction logic for GenericContentDialog.xaml
    /// </summary>
    public partial class GenericContentDialog : ModernWpf.Controls.ContentDialog, INotifyPropertyChanged
    {
        public GenericContentDialog()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void Reset()
        {
            Title = "";
            Message = "";
            PrimaryButtonText = "OK";
            SecondaryButtonText= null;
            CloseButtonText = null;

            NotifyAll();
        }

        public Task<ContentDialogResult> ShowAsync(string title, string message)
        {
            Reset();

            Title = title;
            Message = message;
            PrimaryButtonText = "OK";

            NotifyAll();

            return ShowAsync();
        }

        public Task<ContentDialogResult> ShowAsync(string title, string message, string primary)
        {
            Reset();

            Title = title;
            Message = message;
            PrimaryButtonText = primary;

            NotifyAll();

            return ShowAsync();
        }

        public Task<ContentDialogResult> ShowAsync(string title, string message, string primary, string secondary)
        {
            Reset();

            Title = title;
            Message = message;
            PrimaryButtonText = primary;
            SecondaryButtonText = secondary;

            NotifyAll();

            return ShowAsync();
        }

        public Task<ContentDialogResult> ShowAsync(string title, string message, string primary, string secondary, string close)
        {
            Reset();

            Title = title;
            Message = message;
            PrimaryButtonText = primary;
            SecondaryButtonText = secondary;
            CloseButtonText = close;

            NotifyAll();

            return ShowAsync();
        }

        private void NotifyAll()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Message { get; set; }
    }
}
