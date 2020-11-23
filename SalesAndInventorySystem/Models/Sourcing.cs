using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;

namespace SalesAndInventorySystem.Models
{
    public class SourcingTransaction : TransactionEntityModel
    {
        private DateTime dateTime;

        public SourcingTransaction()
        {
            SourcingTransactionItems = new BindableCollection<SourcingTransactionItem>();

            SourcingTransactionItems.CollectionChanged += SourcingTranasctionItems_CollectionChanged;
        }

        private void SourcingTranasctionItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (SourcingTransactionItem item in e.NewItems)
                {
                    item.UpdateParentPrice = () => ComputeTotals();
                }
            }

            ComputeTotals();
        }

        public void ComputeTotals()
        {
            TotalAmount = 0;

            foreach (SourcingTransactionItem item in SourcingTransactionItems)
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
                TransactionId = TransactionId;
                Notify("TransactionId");
            }
        }

        [MaxLength(200)]
        public string Remarks { get; set; }

        public double TotalAmount { get; set; }

        public BindableCollection<SourcingTransactionItem> SourcingTransactionItems { get; set; }
    }

    public class SourcingTransactionItem : EntityModel
    {
        private double purchasePrice;
        private int quantity;

        public Item Item { get; set; }

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

        public double PurchasePrice
        {
            get => purchasePrice;
            set
            {
                purchasePrice = value;
                Notify("SubTotal");
                UpdateParentPrice?.Invoke();
            }
        }

        public double SubTotal { get => Quantity * PurchasePrice; }

        [NotMapped]
        public System.Action UpdateParentPrice { get; set; }
    }
}
