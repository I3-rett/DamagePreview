using BepInEx;
using BepInEx.Logging;

namespace DamagePreview;

/// <summary>Entry point. Client-side only: reads local game state and draws UI.
/// No RPC, no ZDO write, no version handshake.</summary>
[BepInPlugin(PluginInfo.Guid, PluginInfo.Name, PluginInfo.Version)]
public sealed class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log = null!;

    private void Awake()
    {
        Log = Logger;
        Log.LogInfo($"{PluginInfo.Name} {PluginInfo.Version} loaded");
    }
}
