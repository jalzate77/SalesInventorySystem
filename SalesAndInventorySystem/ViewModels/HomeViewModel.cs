using SalesAndInventorySystem.Common;
using SalesAndInventorySystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SalesAndInventorySystem.ViewModels
{
    [Export(typeof(HomeViewModel))]
    public class HomeViewModel : EntityViewModel<DummyTransactionModel>
    {
        private DateTime transactionRangeFrom;
        private DateTime transactionRangeTo;
        private bool isProcessing;

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            DisplayName = "Dashboard";

            PropertyChanged += HomeViewModel_PropertyChanged;

            View.SortDescriptions.Add(new System.ComponentModel.SortDescription("TransactionDate", System.ComponentModel.ListSortDirection.Descending));

            TransactionRangeFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            TransactionRangeTo = TransactionRangeFrom.AddMonths(1).AddDays(-1);

            Populate();
        }

        private void HomeViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.StartsWith("TransactionRange"))
            {
                if (transactionRangeTo < transactionRangeFrom)
                {
                    TransactionRangeTo = transactionRangeFrom;
                }
            }
        }

        public void Populate()
        {
            IsProcessing = true;

            Task.Run(() =>
            {
                Items.Clear();

                Items.AddRange(Context.SourcingTransactions.Where(i => i.IsPosted && i.DateTime > transactionRangeFrom.Date.AddDays(-1) && i.DateTime <= transactionRangeTo.Date.AddDays(1)).Select(i => new DummyTransactionModel()
                {
                    Amount = i.TotalAmount,
                    TransactionDate = i.DateTime,
                    Type = "Sourcing"
                }).ToList());

                Items.AddRange(Context.SaleTransactions.Where(i => i.IsPosted && i.DateTime > transactionRangeFrom.Date.AddDays(-1) && i.DateTime <= transactionRangeTo.Date.AddDays(1)).Select(i => new DummyTransactionModel()
                {
                    Amount = i.TotalAmount,
                    TransactionDate = i.DateTime,
                    Type = "Sale"
                }).ToList());

                TotalSalesCount = Items.Count(i => i.Type == "Sale");
                TotalSalesAmount = Items.Where(i => i.Type == "Sale").Sum(i => i.Amount);
                TotalSourcingCount = Items.Count(i => i.Type == "Sourcing");
                TotalSourcingAmount = Items.Where(i => i.Type == "Sourcing").Sum(i => i.Amount);
                TotalDifference = TotalSalesAmount - TotalSourcingAmount;

                TotalCriticalItemCount = Context.Items.ToList().Count(i => i.IsStockInCriticalLevel);

                NotifyAll();

                System.Threading.Thread.Sleep(5000);

                IsProcessing = false;
            });
        }

        public bool IsProcessing
        {
            get => isProcessing;
            set
            {
                isProcessing = value;
                Notify();
            }
        }

        public DateTime TransactionRangeFrom
        {
            get => transactionRangeFrom;
            set
            {
                transactionRangeFrom = value;
                Notify();
            }
        }

        public DateTime TransactionRangeTo
        {
            get => transactionRangeTo;
            set
            {
                transactionRangeTo = value;
                Notify();
            }
        }

        public int TotalCriticalItemCount { get; set; }
        public int TotalTransactionCountToday { get; set; }

        public int TotalSalesCount { get; set; }
        public double TotalSalesAmount { get; set; }
        public int TotalSourcingCount { get; set; }
        public double TotalSourcingAmount { get; set; }
        public double TotalDifference { get; set; }
    }
}
