using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Spestqnko.Core.Models;

namespace Spestqnko.Data
{
    public class SpestqnkoDbContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<UserWallet> UserWallets { get; set; }
        public DbSet<UserWalletCategory> UserWalletCategories { get; set; }
        public DbSet<ExpenseNotificationTreshold> ExpenseNotificationTresholds { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public SpestqnkoDbContext(DbContextOptions<SpestqnkoDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tell EF that Expense replaces the old Spending entity
            modelBuilder.Entity<Expense>().ToTable("Expenses");
            
            // Tell EF that ExpenseNotificationTreshold replaces the old SpendingNotificationTreshold entity
            modelBuilder.Entity<ExpenseNotificationTreshold>().ToTable("ExpenseNotificationTresholds");
        }
    }
}