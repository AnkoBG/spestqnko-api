using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spestqnko.Core.Repositories;
using Spestqnko.Api.Settings;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Spestqnko.Api
{
    public class WalletInvitationCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WalletInvitationCleanupService> _logger;
        private readonly WalletInvitationSettings _settings;

        public WalletInvitationCleanupService(
            IServiceProvider serviceProvider,
            IOptions<WalletInvitationSettings> options,
            ILogger<WalletInvitationCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _settings = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var repositoryManager = scope.ServiceProvider.GetRequiredService<IRepositoryManager>();
                        var now = DateTime.UtcNow;
                        var invitations = repositoryManager.WalletInvitations
                            .Find(i => i.IsUsed || i.IsExpired)
                            .ToList();
                            
                        if (invitations.Any())
                        {
                            repositoryManager.WalletInvitations.RemoveRange(invitations);
                            await repositoryManager.SaveChangesAsync();
                            _logger.LogInformation($"Removed {invitations.Count} expired/used wallet invitations.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while cleaning up wallet invitations.");
                }
                await Task.Delay(TimeSpan.FromMinutes(_settings.CleanupIntervalMinutes), stoppingToken);
            }
        }
    }
} 