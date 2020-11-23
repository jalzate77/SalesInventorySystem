using Microsoft.EntityFrameworkCore;
using SalesAndInventorySystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SalesAndInventorySystem
{
    [Export(typeof(InventoryDbContext))]
    public class InventoryDbContext : DbContext
    {
        private readonly string connectionString;

        public InventoryDbContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        public InventoryDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public InventoryDbContext()
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (string.IsNullOrEmpty(connectionString))
                optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = InventorySystemDB; Trusted_Connection = True; ");
            else
                optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Item> Items { get; set; }

        public DbSet<ItemPrice> ItemPrices { get; set; }

        public DbSet<SourcingTransaction> SourcingTransactions { get; set; }

        public DbSet<SourcingTransactionItem> SourcingTransactionItems { get; set; }

        public DbSet<SaleTransaction> SaleTransactions { get; set; }

        public DbSet<SaleTransactionItem> SaleTransactionItems { get; set; }

        public DbSet<Settings> Settings { get; set; }
    }
}
