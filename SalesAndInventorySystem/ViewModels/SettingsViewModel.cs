using SalesAndInventorySystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace SalesAndInventorySystem.ViewModels
{
    [Export(typeof(SettingsViewModel))]
    public class SettingsViewModel : ViewModel<Settings>
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();

            DisplayName = "Settings";

            Reload();
        }

        public void Reload()
        {
            var settings = Context.Settings.FirstOrDefault();

            if (settings == null)
            {
                settings = new Settings()
                {
                    Id = Guid.NewGuid(),
                    AutoNumberDelimiter = "-",
                    AutoNumberItemReset = true,
                    AutoNumberSaleReset = true,
                    AutoNumberSourcingReset = true
                    
                };

                Context.Settings.Add(settings);

                Context.SaveChanges();
            }

            ActivateItem(settings);
        }

        public void Save()
        {
            if (Context.SaveChanges() > 0)
            {
                Dialog.ShowAsync("Info", "Settings have been saved.");
            }
            else
            {
                Dialog.ShowAsync("", "An error has occurred upon attempting to save settings.");
            }
        }
    }
}
