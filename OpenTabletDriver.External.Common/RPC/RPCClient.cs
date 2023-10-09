using System;
using System.IO.Pipes;
using System.Threading.Tasks;
using StreamJsonRpc;

namespace OpenTabletDriver.External.Common.RPC
{
    public class RpcClient<T> : IDisposable
        where T : class
    {
        private readonly string pipeName;
        private NamedPipeClientStream stream;
        private JsonRpc rpc;

        public T Instance { private set; get; }

        public event EventHandler Disconnected;
        public event EventHandler Connected;
        public event EventHandler Attached;
        public event EventHandler Connecting;
        
        public bool IsConnected { get; private set; } = false;
        public bool IsConnecting { get; private set; } = false;

        public RpcClient(string pipeName)
        {
            this.pipeName = pipeName;
            this.stream = null!;
            this.rpc = null!;
            this.Instance = null!;

            this.Disconnected += (_, _) => { 
                IsConnected = false;
                IsConnecting = false;
            };
            this.Connected += (_, _) => { 
                IsConnected = true;
                IsConnecting = false;
            };

            this.Attached = null!;

            this.Connecting += (_, _) => { 
                IsConnecting = true;
            };
        }

        public async Task ConnectAsync()
        {
            this.stream = GetStream();

            Connecting?.Invoke(this, null!);

            await this.stream.ConnectAsync();

            Connected?.Invoke(this, null!);

            rpc = new JsonRpc(this.stream);
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

        public void Dispose()
        {
            this.stream?.Dispose();
            this.rpc?.Dispose();
            this.Disconnected.Invoke(this, null!);
        }
    }
}