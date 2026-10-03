using System;
using HarmonyLib;

namespace DamagePreview;

/// <summary>Runs after the game has positioned and sized every enemy health bar for this
/// frame. Ordered after Creature Level and Loot Control, which may swap hud prefabs.
/// HealthBar Plus patches the same method but only edits the Name text; we never touch it.</summary>
[HarmonyPatch(typeof(EnemyHud), nameof(EnemyHud.UpdateHuds))]
[HarmonyAfter("org.bepinex.plugins.creaturelevelcontrol")]
internal static class EnemyHudPatch
{
    [HarmonyPostfix]
    private static void Postfix(EnemyHud __instance, Player player)
    {
        if (!Plugin.Active) return;
        try
        {
            PreviewDriver.Update(__instance, player);
        }
        catch (Exception e)
        {
            Plugin.Fail(e);
        }
    }
}
