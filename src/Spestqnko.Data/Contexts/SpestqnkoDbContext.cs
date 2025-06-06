﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        public required DbSet<Notification> Notifications { get; set; }
        public required DbSet<Currency> Currencies { get; set; }
        public required DbSet<WalletInvitation> WalletInvitations { get; set; }

        public SpestqnkoDbContext(DbContextOptions<SpestqnkoDbContext> options)
            : base(options)
        {
            
        }
    }
}