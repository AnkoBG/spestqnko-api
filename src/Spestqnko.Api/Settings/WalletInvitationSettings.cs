namespace Spestqnko.Api.Settings
{
    public class WalletInvitationSettings
    {
        public int ExpirationTimeInHours { get; set; } = 24;
        /// <summary>
        /// The interval in minutes for cleaning up expired or used wallet invitations
        /// </summary>
        public int CleanupIntervalMinutes { get; set; } = 60;
    }
} 