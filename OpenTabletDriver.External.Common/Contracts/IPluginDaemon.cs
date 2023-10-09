using System.Collections.Generic;
using System.Threading.Tasks;
using OpenTabletDriver.External.Common.Serializables;

namespace OpenTabletDriver.External.Common.Contracts
{
    public interface IPluginDaemon
    {
        public Task<List<SerializablePlugin>> GetPlugins();
    }
}