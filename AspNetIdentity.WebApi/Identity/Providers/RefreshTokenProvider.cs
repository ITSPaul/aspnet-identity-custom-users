namespace WebApi.Identity.Providers
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Infrastructure;

    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private static readonly ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            await Task.Run(() => this.Create(context));
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            await Task.Run(() => this.Receive(context));
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["audience"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            // maybe only create a handle the first time, then re-use for same client
            // copy properties and set the desired lifetime of refresh token
            var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
            {
                IssuedUtc = context.Ticket.Properties.IssuedUtc,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(120) // TODO: change for production, read from web.config
            };

            context.Ticket.Properties.IssuedUtc = refreshTokenProperties.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = refreshTokenProperties.ExpiresUtc;

            var refreshTokenId = Guid.NewGuid().ToString("n");
            var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);

            //_refreshTokens.TryAdd(refreshTokenId, context.Ticket);
            _refreshTokens.TryAdd(refreshTokenId, refreshTokenTicket);

            // consider storing only the hash of the handle
            context.SetToken(refreshTokenId);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            AuthenticationTicket ticket;
            if (_refreshTokens.TryRemove(context.Token, out ticket))
            {
                context.SetTicket(ticket);
            }
        }
    }
}
