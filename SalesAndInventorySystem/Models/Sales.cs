using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SalesAndInventorySystem.Models
{
    public class SaleTransaction : TransactionEntityModel
    {
        private DateTime dateTime;

        public SaleTransaction()
        {
            SaleTransactionItems = new BindableCollection<SaleTransactionItem>();

            SaleTransactionItems.CollectionChanged += SourcingTranasctionItems_CollectionChanged;
        }

        private void SourcingTranasctionItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (SaleTransactionItem item in e.NewItems)
                {
                    item.UpdateParentPrice = () => ComputeTotals();
                }
            }

            ComputeTotals();
        }

        public void ComputeTotals()
        {
            TotalAmount = 0;

            foreach (SaleTransactionItem item in SaleTransactionItems)
            {
                TotalAmount += item.SubTotal;
            }

            Notify("TotalAmount");
        }

        public override DateTime DateTime
        {
            get => dateTime;
            set
            {
                dateTime = value;
                TransactionId = value.ToString("YYYYmmdd-") + TransactionId;
                Notify("TransactionId");
            }
        }

        public double TotalAmount { get; set; }

        public BindableCollection<SaleTransactionItem> SaleTransactionItems { get; set; }
    }

    public class SaleTransactionItem : EntityModel
    {
        private int quantity;
        private Item item;

        public Item Item
        {
            get => item;
            set
            {
                item = value;

                if (value != null)
                {
                    Price = value.CurrentPrice;
                }

                Notify("Price");
                Notify("SubTotal");

                UpdateParentPrice?.Invoke();
            }
        }

        public double Price { get; set; }

        public int Quantity
        {
            get => quantity;
            set
            {
                quantity = value;

                Notify("SubTotal");

                UpdateParentPrice?.Invoke();
            }
        }

        public double SubTotal
        {
            get
            {
                if (Item == null)
                    return 0;

                return Quantity * Item.CurrentPrice;
            }
        }

        [NotMapped]
        public System.Action UpdateParentPrice { get; set; }
    }
}
