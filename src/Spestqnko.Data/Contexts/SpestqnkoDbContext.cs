using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Spestqnko.Core.Models;

namespace Spestqnko.Data
{
    public class SpestqnkoDbContext : IdentityDbContext<User, Role, Guid>
    {
        public required DbSet<Expense> Expenses { get; set; }
        public required DbSet<Category> Categories { get; set; }
        public required DbSet<Wallet> Wallets { get; set; }
        public required DbSet<UserWallet> UserWallets { get; set; }
        public required DbSet<UserWalletCategory> UserWalletCategories { get; set; }
        public required DbSet<ExpenseNotificationTreshold> ExpenseNotificationTresholds { get; set; }
        public required DbSet<Notification> Notifications { get; set; }

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