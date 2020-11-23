using Caliburn.Micro;
using Microsoft.EntityFrameworkCore;
using SalesAndInventorySystem.Common;
using SalesAndInventorySystem.Models;
using SalesAndInventorySystem.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SalesAndInventorySystem.ViewModels
{
    [Export(typeof(ItemViewModel))]
    public class ItemViewModel : EntityViewModel<Item>
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();

            DisplayName = "Item Management";

            PropertyChanged += ItemViewModel_PropertyChanged;

            Populate();
        }

        private void ItemViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveItem")
            {
                IsStockReadOnly = true;
                NotifyAll();
            }
        }

        public void Populate()
        {
            IsCreating = false;

            Items.Clear();
            Items.AddRange(Context.Items.Include(i => i.ItemPrices).ToList());

            if (Items.Count == 0)
            {
                New();
            }
            else
            {
                ActivateItem(Items.First());
            }

            NotifyAll();
        }

        public void New()
        {
            ChangeActiveItem(new Item()
            {
                Id = Guid.NewGuid(),
                ItemId = AutoNumberService.GetNextItemSequence(Context),
                Deleted = false
            }, false); ;

            IsCreating = true;

            NotifyAll();
        }

        public void Save()
        {
            if (Validate())
            {
                if (Context.Items.Any(i => i.Id == ActiveItem.Id))
                {
                    foreach (var item in ActiveItem.ItemPrices)
                    {
                        if (!Context.ItemPrices.Any(i => i.Id == item.Id))
                        {
                            Context.ItemPrices.Add(item);
                        }
                    }
                }
                else
                {
                    Context.Items.Add(ActiveItem);
                }

                if (Context.SaveChanges() > 0)
                {
                    IsCreating = false;

                    NotifyAll();

                    ActiveItem.NotifyAll();
                }
                else
                {
                    Dialog.ShowAsync("Error", "An error has occurred upon attempting to save the item.");
                }
            }
        }

        private bool Validate()
        {
            var errorMesage = "";

            if (string.IsNullOrEmpty(ActiveItem.ItemId))
                errorMesage = errorMesage.AppendCommaDelimited("Item Id");

            if (string.IsNullOrEmpty(ActiveItem.Name))
                errorMesage = errorMesage.AppendCommaDelimited("Name");

            if (!string.IsNullOrEmpty(errorMesage))
            {
                errorMesage += " cannot be empty.";

                Dialog.ShowAsync("Error", errorMesage);

                return false;
            }

            return true;
        }

        public void EditInv()
        {
            IsStockReadOnly = false;
            Notify("IsStockReadOnly");
            Notify("CanEditInv");

            Dialog.ShowAsync("Info", "Item stock quantity can now be modified for this item.");
        }

        public async void Delete()
        {
            var result = await Dialog.ShowAsync("Warning", "Are you sure you want to delete this item?", "Yes", "No");

            if (result == ModernWpf.Controls.ContentDialogResult.Primary)
            {
                ActiveItem.Deleted = true;

                if (Context.SaveChanges() > 0)
                {
                    ActiveItem.NotifyAll();

                    await Dialog.ShowAsync("Delete Successful", "This item has been marked as deleted. The item will remain in the system as a reference for past transactions.");
                }
                else
                {
                    await Dialog.ShowAsync("Error", "An error has occurred upon attempting to delete the item.");
                }
            }

            NotifyAll();
        }

        public void Restore()
        {
            ActiveItem.Deleted = false;

            if (Context.SaveChanges() > 0)
            {
                ActiveItem.NotifyAll();

                Dialog.ShowAsync("Restore Successful", "This item has been marked as restored.");
            }
            else
            {
                Dialog.ShowAsync("Error", "An error has occurred upon attempting to restore the item.");
            }

            NotifyAll();
        }

        public bool IsStockReadOnly { get; set; }

        public bool IsCreating { get; set; }
        public bool CanSave { get => ActiveItem != null && !ActiveItem.Deleted; }
        public bool CanDelete { get => !IsCreating || (ActiveItem != null && !ActiveItem.Deleted); }
        public bool CanRestore { get => !IsCreating || (ActiveItem != null && ActiveItem.Deleted); }
        public bool CanEditInv { get => !IsCreating || (ActiveItem != null && !ActiveItem.Deleted) && IsStockReadOnly; }
    }
}
