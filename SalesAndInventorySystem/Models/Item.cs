using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Xps.Serialization;

namespace SalesAndInventorySystem.Models
{

    public class Item : ValidatingEntityModel
    {
        private int stock;
        private int? stockCriticalLevel;

        public Item()
        {
            Deleted = false;

            ItemPrices = new BindableCollection<ItemPrice>();

            ItemPrices.CollectionChanged += ItemPrices_CollectionChanged;

            ItemPricesView = CollectionViewSource.GetDefaultView(ItemPrices);

            ItemPricesView.SortDescriptions.Add(new SortDescription("ScheduleDate", ListSortDirection.Descending));
        }

        private void ItemPrices_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (ItemPrice item in e.NewItems)
                {
                    item.UpdateParentPrice = () => NotifyOfPropertyChange("CurrentPrice");
                }

            }
            NotifyOfPropertyChange("CurrentPrice");
        }

        public double GetPrice(DateTime dateTime)
        {
            if (ItemPrices.Count > 0)
            {
                var maxDate = ItemPrices.Where(i => i.ScheduleDate <= dateTime).Max(i => i.ScheduleDate);

                return ItemPrices.FirstOrDefault(i => i.ScheduleDate == maxDate)?.Amount ?? 0;
            }

            return 0;
        }

        public override string ToString()
        {
            return Name + (string.IsNullOrEmpty(SKU) ? "" : $" ({SKU})");
        }

        [MaxLength(100)]
        [Required]
        public string ItemId { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        public string SKU { get; set; }

        [NotMapped]
        public double CurrentPrice { get => GetPrice(DateTime.Now); }

        public BindableCollection<ItemPrice> ItemPrices { get; set; }

        public ICollectionView ItemPricesView { get; }

        public int Stock
        {
            get => stock;
            set
            {
                stock = value;
                Notify("IsStockInCriticalLevel");
                Notify("TotalValuation");
            }
        }

        public int? StockCriticalLevel
        {
            get => stockCriticalLevel;
            set
            {
                stockCriticalLevel = value;
                Notify("IsStockInCriticalLevel");
            }
        }

        [NotMapped]
        public bool IsStockInCriticalLevel { get => StockCriticalLevel!= null && Stock < StockCriticalLevel; }

        [NotMapped]
        public double TotalValuation { get => Stock * CurrentPrice; }
    }

    public class ItemPrice : EntityModel
    {
        private DateTime scheduleDate;
        private double amount;

        public ItemPrice()
        {
            ScheduleDate = DateTime.Now;
        }

        public DateTime ScheduleDate
        {
            get => scheduleDate;
            set
            {
                scheduleDate = value;
                UpdateParentPrice?.Invoke();
            }
        }

        public double Amount
        {
            get => amount;
            set
            {
                amount = value;
                UpdateParentPrice?.Invoke();
            }
        }

        [NotMapped]
        public System.Action UpdateParentPrice;
    }
}
