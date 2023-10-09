using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using OpenTabletDriver.Plugin;
using StreamJsonRpc;

namespace OpenTabletDriver.External.Common.RPC
{
    public class RpcServer<T> : IDisposable
        where T : class, new()
    {
        private JsonRpc rpc;
        private readonly string pipeName;

        private bool hasStarted = false;

        public event EventHandler<bool> ConnectionStateChanged;
        public T Instance { protected set; get; } = new T();

        public RpcServer(string pipeName)
        {
            this.pipeName = pipeName;
            this.rpc = null!;
            this.ConnectionStateChanged = null!;
        }

        public async Task MainAsync()
        {
            if (hasStarted)
                return;
            else
                hasStarted = true;

            while (hasStarted)
            {
                var stream = CreateStream();
                await stream.WaitForConnectionAsync();
                
                _ = Task.Run(async () =>
                {
                    try
                    {
                        ConnectionStateChanged?.Invoke(this, true);
                        this.rpc = JsonRpc.Attach(stream, Instance);
                        await this.rpc.Completion;
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                    catch (IOException)
                    {
                    }
                    catch (Exception ex)
                    {
                        Log.Exception(ex);
                    }

                    ConnectionStateChanged?.Invoke(this, false);
                    this.rpc.Dispose();
                    await stream.DisposeAsync();
                });
            }
        }

        private NamedPipeServerStream CreateStream()
        {
            return new NamedPipeServerStream(
                this.pipeName,
                PipeDirection.InOut,
                NamedPipeServerStream.MaxAllowedServerInstances,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous | PipeOptions.WriteThrough | PipeOptions.CurrentUserOnly
            );
        }

        public void Dispose()
        {
            hasStarted = false;
            rpc?.Dispose();
        }
    }
}