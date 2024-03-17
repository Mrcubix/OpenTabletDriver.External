

using Avalonia.Controls;

namespace OpenTabletDriver.External.Avalonia.Extensions;

public static class IResourceHostExtensions
{
    public static bool TryFindResource<T>(this IResourceHost host, string key, out T? value) where T : class
    {
        if (host.TryFindResource(key, out var obj) && obj is T t)
        {
            value = t;
            return true;
        }

        value = default;
        return false;
    }
}