using Caliburn.Micro;
using SalesAndInventorySystem.Services;
using SalesAndInventorySystem.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Data;

namespace SalesAndInventorySystem.Models
{
    public class ViewModel<TEntity> : Conductor<TEntity>
        where TEntity : class
    {
        [Import(typeof(InventoryDbContext))]
        public InventoryDbContext Context { get; set; }

        [Import(typeof(GenericContentDialog))]
        public GenericContentDialog Dialog { get; set; }
    }

    public class EntityViewModel<TEntity> : Conductor<TEntity>.Collection.OneActive
        where TEntity : class
    {
        private bool isDeletedShown;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            View = CollectionViewSource.GetDefaultView(Items);
        }

        public void NotifyAll()
        {
            NotifyOfPropertyChange("");
        }

        public void Notify([CallerMemberName]string source = "")
        {
            NotifyOfPropertyChange(source);
        }

        public ICollectionView View { get; set; }

        [Import(typeof(InventoryDbContext))]
        public InventoryDbContext Context { get; set; }

        [Import(typeof(AutoNumberService))]
        public AutoNumberService AutoNumberService { get; set; }

        [Import(typeof(GenericContentDialog))]
        public GenericContentDialog Dialog { get; set; }

        public bool IsDeletedShown
        {
            get => isDeletedShown;
            set
            {
                isDeletedShown = value;
                NotifyOfPropertyChange();
            }
        }

    }
}
