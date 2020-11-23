using Caliburn.Micro;
using Microsoft.EntityFrameworkCore;
using SalesAndInventorySystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SalesAndInventorySystem.ViewModels
{
    [Export(typeof(SaleViewModel))]
    public class SaleViewModel : EntityViewModel<SaleTransaction>
    {
        private BindableCollection<Item> inventoryItemList;
        bool isCreating = false;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            DisplayName = "Sales Transactions";

            PropertyChanged += SaleViewModel_PropertyChanged;

            inventoryItemList = new BindableCollection<Item>();
            InventoryItemListView = CollectionViewSource.GetDefaultView(inventoryItemList);

            View.SortDescriptions.Add(new SortDescription("DateTime", ListSortDirection.Descending));

            Populate();
        }

        private void SaleViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveItem")
            {
                isCreating = false;

                NotifyAll();
            }
        }

        public void Populate()
        {
            inventoryItemList.Clear();
            inventoryItemList.AddRange(Context.Items.Include(i => i.ItemPrices).Where(i=>i.Stock > 0 && !i.Deleted).ToList());

            Items.Clear();
            Items.AddRange(Context.SaleTransactions.Include(i => i.SaleTransactionItems)
                                                       .OrderByDescending(i => i.DateTime)
                                                       .ToList());

            if (Items.Count == 0)
            {
                New();
            }
            else
            {
                ActivateItem(Items.First());
            }

            isCreating = false;

            NotifyAll();
        }

        public void New()
        {
            ActiveItem = new SaleTransaction()
            {
                Id = Guid.NewGuid(),
                TransactionId = AutoNumberService.GetNextSalesSequence(Context),
                DateTime = DateTime.Now
            };

            ActiveItem.PropertyChanged += (o, e) =>
             {
                 if (e.PropertyName == "TotalAmount")
                     Notify("CanSave");
             };

            isCreating = true;

            NotifyAll();
        }

        public void Save()
        {
            if (Validate())
            {
                if (Context.SaleTransactions.Any(i => i.Id == ActiveItem.Id))
                {
                    foreach (var item in ActiveItem.SaleTransactionItems)
                    {
                        if (!Context.SaleTransactionItems.Any(i => i.Id == item.Id))
                        {
                            Context.SaleTransactionItems.Add(item);
                        }
                    }
                }
                else
                {
                    Context.SaleTransactions.Add(ActiveItem);
                }

                if (Context.SaveChanges() > 0)
                {
                    isCreating = false;

                    ActiveItem.NotifyAll();

                    NotifyAll();
                }
                else
                {
                    Dialog.ShowAsync("Error", "An error has occurred upon attempting to save the transaction.");
                }
            }
        }

        private bool Validate()
        {
            var errorMesage = "";

            if (ActiveItem.SaleTransactionItems.Count() == 0)
                errorMesage = "Transaction is still empty. Please add some items before saving.";

            if (!string.IsNullOrEmpty(errorMesage))
            {
                Dialog.ShowAsync("Error", errorMesage);

                return false;
            }

            return true;
        }

        public void Post()
        {
            bool canPost = true;

            foreach (SaleTransactionItem i in ActiveItem.SaleTransactionItems)
            {
                if ((i.Item.Stock - i.Quantity) < 0)
                {
                    canPost = false;

                    Dialog.ShowAsync(
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
                Context.SaveChanges();
            }

            NotifyAll();
        }

        public void Unpost()
        {
            foreach (SaleTransactionItem i in ActiveItem.SaleTransactionItems)
            {
                i.Item.Stock += i.Quantity;
            }

            ActiveItem.Unpost();
            Context.SaveChanges();

            NotifyAll();
        }

        public void ShowPosted()
        {
            View.Filter = (o) =>
            {
                if ((o as SourcingTransaction).IsPosted)
                    return true;

                return false;
            };
        }

        public void ShowUnposted()
        {
            View.Filter = (o) =>
            {
                if ((o as SourcingTransaction).IsPosted)
                    return false;

                return true;
            };
        }

        public void ShowAll()
        {
            View.Filter = null;
        }

        public ICollectionView InventoryItemListView { get; set; }

        public bool CanSave { get => ActiveItem != null && ActiveItem.TotalAmount > 0; }
        public bool CanPost { get => !isCreating && !(ActiveItem?.IsPosted ?? false); }
        public bool CanUnpost { get => !isCreating && (ActiveItem?.IsPosted ?? false); }
        public bool CanShowPosted { get => !isCreating; }
        public bool CanShowUnposted { get => !isCreating; }
        public bool CanShowAll { get => !isCreating; }
    }
}
