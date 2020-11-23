using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SalesAndInventorySystem.Models
{
    public class Settings : EntityModel
    {
        private string autoNumberDelimiter;
        private string autoNumberItem;
        private string autoNumberSourcing;
        private bool autoNumberSaleReset;

        public Settings()
        {
            PropertyChanged += Settings_PropertyChanged;
        }

        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AutoNumberDelimiter")
            {
                AutoNumberItemReset = true;
                AutoNumberSourcingReset = true;
                AutoNumberSaleReset = true;
            }
            else if (e.PropertyName == "AutoNumberItem")
            {
                AutoNumberItemReset = true;
            }
            else if (e.PropertyName == "AutoNumberSourcing")
            {
                AutoNumberSourcingReset = true;
            }
            else if (e.PropertyName == "AutoNumberSale")
            {
                AutoNumberSaleReset = true;
            }
        }

        [MaxLength(1)]
        [DefaultValue("-")]
        public string AutoNumberDelimiter
        {
            get => autoNumberDelimiter;
            set
            {
                PreviousAutoNumberDelimiter = autoNumberDelimiter;

                autoNumberDelimiter = value;

                Notify();
            }
        }

        [MaxLength(1)]
        [DefaultValue("-")]
        public string PreviousAutoNumberDelimiter { get; set; }

        [MaxLength(50)]
        public string AutoNumberItem
        {
            get => autoNumberItem;
            set
            {
                autoNumberItem = value;
                Notify();
            }
        }

        public bool AutoNumberItemReset { get; set; }

        [MaxLength(50)]
        public string AutoNumberSourcing
        {
            get => autoNumberSourcing;
            set
            {
                autoNumberSourcing = value;
                Notify();
            }
        }

        public bool AutoNumberSourcingReset { get; set; }

        [MaxLength(50)]
        public string AutoNumberSale { get; set; }

        public bool AutoNumberSaleReset
        {
            get => autoNumberSaleReset;
            set
            {
                autoNumberSaleReset = value;
                Notify();
            }
        }
    }
}
