using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StreamJsonRpc;

namespace OpenTabletDriver.External.Common.RPC
{
    public class RpcClient<T> : IDisposable
        where T : class
    {
        private readonly string pipeName;
        private NamedPipeClientStream stream;
        private JsonRpc rpc;

        public RpcClient(string pipeName)
        {
            this.pipeName = pipeName;
            this.stream = null!;
            this.rpc = null!;
            this.Instance = null!;

            this.Disconnected += (_, _) => { 
                IsAttached = false;
                IsConnected = false;
                IsConnecting = false;
            };

            this.Connected += (_, _) => { 
                IsConnected = true;
                IsConnecting = false;
            };

            this.Attached = (_, _) => {
                IsAttached = true;
            };

            this.Connecting += (_, _) => { 
                IsConnecting = true;
            };
        }

        public event EventHandler Disconnected;
        public event EventHandler Connected;
        public event EventHandler Attached;
        public event EventHandler Connecting;

        public T Instance { private set; get; }
        public List<JsonConverter> Converters { get; } = new List<JsonConverter>();
        public bool IsConnected { get; private set; } = false;
        public bool IsConnecting { get; private set; } = false;
        public bool IsAttached { get; private set; } = false;

        public async Task ConnectAsync()
        {
            this.stream = GetStream();

            Connecting?.Invoke(this, null!);

            await this.stream.ConnectAsync();

            Connected?.Invoke(this, null!);

            rpc = GetRpc(this.stream);
            this.Instance = this.rpc.Attach<T>();
            rpc.StartListening();

            Attached?.Invoke(this, null!);

            rpc.Disconnected += (_, _) =>
            {
                this.stream.Dispose();
                Disconnected?.Invoke(this, null!);
                rpc.Dispose();
            };
        }

        private NamedPipeClientStream GetStream()
        {
            return new NamedPipeClientStream(
                ".",
                this.pipeName,
                PipeDirection.InOut,
                PipeOptions.Asynchronous | PipeOptions.WriteThrough | PipeOptions.CurrentUserOnly
            );
        }

        private JsonRpc GetRpc(NamedPipeClientStream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            // If no converters are provided, use the default JsonRpc
            if (Converters.Count == 0)
                return new JsonRpc(stream);

            // Otherwise, use a custom JsonRpc with the provided converters
            var messageFormatter = new JsonMessageFormatter();

            foreach (var converter in Converters)
                messageFormatter.JsonSerializer.Converters.Add(converter);

            return new JsonRpc(new HeaderDelimitedMessageHandler(stream, stream, messageFormatter));
        }

        public void Dispose()
        {
            this.stream?.Dispose();
            this.rpc?.Dispose();
            this.Disconnected.Invoke(this, null!);
        }
    }
}