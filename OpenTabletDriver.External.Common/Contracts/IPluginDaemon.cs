using System.Collections.Generic;
using System.Threading.Tasks;
using OpenTabletDriver.External.Common.Serializables;

namespace OpenTabletDriver.External.Common.Contracts
{
    public interface IPluginDaemon
    {
        /// <summary>
        ///   Gets the plugins that are available for use.
        /// </summary>
        /// <returns>The available plugins.</returns>
        public Task<List<SerializablePlugin>> GetPlugins();
    }
}