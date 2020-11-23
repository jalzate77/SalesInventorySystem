using Caliburn.Micro;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SalesAndInventorySystem.Common;
using SalesAndInventorySystem.Models;
using SalesAndInventorySystem.Services;
using SalesAndInventorySystem.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;

namespace SalesAndInventorySystem.ViewModels
{
    [Export(typeof(ShellViewModel))]
    public class ShellViewModel : Conductor<Screen>
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();

            SplashScreen.ShowAsync();

            Task.Run(() =>
            {
                SplashScreen.Message = "Launching application...";

                System.Threading.Thread.Sleep(500);

                SplashScreen.Message = "Creating/Updating database...";

                Context.Database.Migrate();

                SplashScreen.Message = "Loading complete! Launching application...";

                System.Threading.Thread.Sleep(600);

                App.Current.Dispatcher.Invoke(() =>
                {
                    SplashScreen.Visibility = System.Windows.Visibility.Hidden;

                    ShowHomePage();
                });
            });
        }

        public void ShowHomePage()
        {
            ActivateItem(HomeViewModel);
        }

        public void ShowPOSPage()
        {
            ActivateItem(POSViewModel);
        }

        public void ShowItemsPage()
        {
            ActivateItem(ItemViewModel);
        }

        public void ShowSourcingPage()
        {
            ActivateItem(SourcingViewModel);
        }

        public void ShowSalesPage()
        {
            ActivateItem(SaleViewModel);
        }

        public void ShowSettingsPage()
        {
            ActivateItem(SettingsViewModel);
        }

        [Import(typeof(SplashScreen))]
        public SplashScreen SplashScreen { get; set; }

        [Import(typeof(SettingsViewModel))]
        public SettingsViewModel SettingsViewModel { get; set; }

        [Import(typeof(GenericContentDialog))]
        public GenericContentDialog Dialog { get; set; }

        [Import(typeof(InventoryDbContext))]
        public InventoryDbContext Context { get; set; }

        [Import(typeof(HomeViewModel))]
        public HomeViewModel HomeViewModel { get; set; }

        [Import(typeof(POSViewModel))]
        public POSViewModel POSViewModel { get; set; }

        [Import(typeof(ItemViewModel))]
        public ItemViewModel ItemViewModel { get; set; }

        [Import(typeof(SourcingViewModel))]
        public SourcingViewModel SourcingViewModel { get; set; }

        [Import(typeof(SaleViewModel))]
        public SaleViewModel SaleViewModel { get; set; }
    }
}
