using Caliburn.Micro;
using Microsoft.EntityFrameworkCore;
using SalesAndInventorySystem.Models;
using SalesAndInventorySystem.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Transactions;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace SalesAndInventorySystem.ViewModels
{
    [Export(typeof(POSViewModel))]
    public class POSViewModel : Conductor<SaleTransaction>
    {
        private BindableCollection<Item> inventoryItemList;
        private string searchString;
        private bool searchPrimed = false;
        private bool transactionIsDirty = false;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            DisplayName = "POS";

            inventoryItemList = new BindableCollection<Item>();
            InventoryItemListView = CollectionViewSource.GetDefaultView(inventoryItemList);
            InventoryItemListView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            Populate();

            New();
        }
        private void Populate()
        {
            inventoryItemList.Clear();
            inventoryItemList.AddRange(Context.Items.Include(i => i.ItemPrices).Where(i => i.Stock > 0 && !i.Deleted));
            InventoryItemListView = CollectionViewSource.GetDefaultView(inventoryItemList);
        }

        private void Filter(string value)
        {
            InventoryItemListView.Filter += (o) =>
            {
                if (string.IsNullOrEmpty(value))
                    return true;

                return (o as Item).ToString().IndexOf(value, StringComparison.InvariantCultureIgnoreCase) > -1;
            };
        }

        public async void New()
        {
            ModernWpf.Controls.ContentDialogResult result = ModernWpf.Controls.ContentDialogResult.None;

            if (transactionIsDirty)
            {
                result = await Dialog.ShowAsync(
                  "Confirmation",
                  "Are you sure you want to create a new transaction? The data of the current transction will be cleared.",
                  "Yes",
                  "No"
              );
            }

            if (!transactionIsDirty || result == ModernWpf.Controls.ContentDialogResult.Primary)
            {
                ActiveItem = new SaleTransaction()
                {
                    Id = Guid.NewGuid(),
                    TransactionId = (Context.SaleTransactions.Count() + 1) + "",
                    DateTime = DateTime.Now
                };

                ActiveItem.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == "TotalAmount")
                    {
                        NotifyOfPropertyChange("CanSave");
                    }

                    transactionIsDirty = true;
                };
            }

            Populate();
        }

        public async void Save()
        {
            var result = await Dialog.ShowAsync(
               "Confirmation",
               "Post transaction?",
               "Yes",
               "No"
           );

            if (result == ModernWpf.Controls.ContentDialogResult.Primary)
            {
                bool canPost = true;

                foreach (SaleTransactionItem i in ActiveItem.SaleTransactionItems)
                {
                    if ((i.Item.Stock - i.Quantity) < 0)
                    {
                        canPost = false;

                        await Dialog.ShowAsync(
                          "Posting Error!",
                          $"Cannot post transaction. {i.Item} stock is less than this transaction's sold quantity.");

                        break;
                    }
                }

                if (canPost)
                {
                    foreach (SaleTransactionItem i in ActiveItem.SaleTransactionItems)
                    {
                        i.Item.Stock -= i.Quantity;
                    }

                    ActiveItem.Post();

                    Context.SaleTransactions.Add(ActiveItem);

                    Context.SaveChanges();

                    transactionIsDirty = false;

                    result = await Dialog.ShowAsync(
                        "Success!",
                        "Transaction has been posted. ",
                        "Ok",
                        "New Transaction"
                    );

                    if (result == ModernWpf.Controls.ContentDialogResult.Secondary)
                    {
                        New();
                    }
                }
            }
        }

        public void OnItemMouseDoubleClick(Item item)
        {
            SaleTransactionItem existing = ActiveItem.SaleTransactionItems.FirstOrDefault(i => i.Item.Id == item.Id);

            if (existing == null)
            {
                ActiveItem.SaleTransactionItems.Add(new SaleTransactionItem()
                {
                    Id = Guid.NewGuid(),
                    Item = item,
                    Quantity = 1
                });
            }
            else
            {
                existing.Quantity++;
                existing.NotifyAll();
            }
        }

        public void OnSearchPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (searchPrimed)
                {
                    OnItemMouseDoubleClick(InventoryItemListView.Cast<Item>().First());
                }
                else
                {
                    searchPrimed = true;
                }
            }
            else
            {
                searchPrimed = false;
            }
        }

        [Import(typeof(InventoryDbContext))]
        public InventoryDbContext Context { get; set; }

        [Import(typeof(GenericContentDialog))]
        public GenericContentDialog Dialog { get; set; }

        public ICollectionView InventoryItemListView { get; set; }

        public string SearchString
        {
            get => searchString;
            set
            {
                searchString = value;
                Filter(value);
            }
        }

        public bool CanSave { get => ActiveItem.SaleTransactionItems.Count() > 0 && ActiveItem.TotalAmount > 0; }
    }
}
