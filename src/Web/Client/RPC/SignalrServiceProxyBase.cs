using MessagePack;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.SignalR.Client;

namespace Web.Client.RPC
{
    internal abstract class SignalrServiceProxyBase : IAsyncDisposable
    {
        protected readonly HubConnection _hubConnection;

        public SignalrServiceProxyBase(Uri serviceBaseUri)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(serviceBaseUri)
                .WithAutomaticReconnect()
                .AddMessagePackProtocol(options =>
                {
                    options.SerializerOptions = MessagePackSerializerOptions.Standard
                        .WithResolver(CompositeResolver.Create(TypelessObjectResolver.Instance, TypelessContractlessStandardResolver.Instance))
                        .WithSecurity(MessagePackSecurity.UntrustedData);
                })
                .Build();
            this.RegisterMethodHandlers(_hubConnection);
        }

        public async virtual ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
            }
        }

        protected virtual void RegisterMethodHandlers(HubConnection hubConnection) { }

        protected async Task<TResult> InvokeAsync<TResult>(string methodName, CancellationToken cancellationToken, params object?[] args)
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
                await _hubConnection.StartAsync();

            return await _hubConnection.InvokeCoreAsync<TResult>(methodName, args, cancellationToken);
        }

        protected async Task SendAsync(string methodName, CancellationToken cancellationToken, params object?[] args)
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
                await _hubConnection.StartAsync();

            await _hubConnection.SendCoreAsync(methodName, args, cancellationToken);
        }
    }
}
