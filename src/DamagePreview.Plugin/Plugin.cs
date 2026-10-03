using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace DamagePreview;

/// <summary>Entry point. Client-side only: reads local game state and draws UI inside the
/// vanilla enemy hud. No RPC, no ZDO write, no version handshake.</summary>
[BepInPlugin(PluginInfo.Guid, PluginInfo.Name, PluginInfo.Version)]
public sealed class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log = null!;

    private static bool _failed;
    private Harmony? _harmony;

    /// <summary>False when the user turned the mod off or a fatal error disabled it.</summary>
    internal static bool Active => !_failed && Settings.Enabled != null && Settings.Enabled.Value;

    /// <summary>Log once and stop touching the hud for the rest of the session.</summary>
    internal static void Fail(Exception e)
    {
        if (_failed) return;
        _failed = true;
        Log.LogError($"{PluginInfo.Name} disabled itself after an error: {e}");
        try
        {
            if (EnemyHud.instance != null)
            {
                PreviewDriver.HideAllPublic(EnemyHud.instance);
            }
        }
        catch
        {
            // Nothing more we can do; the hud keeps working without us.
        }
    }

    private void Awake()
    {
        Log = Logger;
        Settings.Bind(Config);
        Settings.Enabled.SettingChanged += (_, _) =>
        {
            try
            {
                if (!Settings.Enabled.Value && EnemyHud.instance != null)
                {
                    PreviewDriver.HideAllPublic(EnemyHud.instance);
                }
            }
            catch (Exception e)
            {
                Fail(e);
            }
        };
        _harmony = new Harmony(PluginInfo.Guid);
        _harmony.PatchAll(typeof(Plugin).Assembly);
        Log.LogInfo($"{PluginInfo.Name} {PluginInfo.Version} loaded");
    }

    private void OnDestroy()
    {
        _harmony?.UnpatchSelf();
    }
}
