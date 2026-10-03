using BepInEx.Configuration;
using UnityEngine;

namespace DamagePreview;

internal static class Settings
{
    public static ConfigEntry<bool> Enabled = null!;
    public static ConfigEntry<bool> IncludeBackstab = null!;
    public static ConfigEntry<bool> UndrawnBowIsFullDraw = null!;
    public static ConfigEntry<bool> ShowSecondary = null!;
    public static ConfigEntry<Color> PrimaryMin = null!;
    public static ConfigEntry<Color> PrimaryMax = null!;
    public static ConfigEntry<Color> Secondary = null!;

    public static void Bind(ConfigFile config)
    {
        Enabled = config.Bind("General", "Enabled", true, new ConfigDescription(
            "Master switch. Off hides every preview immediately.", null,
            new ConfigurationManagerAttributes { Order = 100 }));
        IncludeBackstab = config.Bind("General", "IncludeBackstab", true, new ConfigDescription(
            "Apply the weapon's backstab bonus when the target has not noticed you and its backstab cooldown has elapsed.", null,
            new ConfigurationManagerAttributes { Order = 90 }));
        UndrawnBowIsFullDraw = config.Bind("General", "UndrawnBowIsFullDraw", true, new ConfigDescription(
            "With a bow that is not drawn, preview a fully drawn shot instead of nothing.", null,
            new ConfigurationManagerAttributes { Order = 80 }));
        ShowSecondary = config.Bind("General", "ShowSecondary", true, new ConfigDescription(
            "Show the two tick marks for the secondary attack (or the fully drawn shot of a bow).", null,
            new ConfigurationManagerAttributes { Order = 70 }));

        PrimaryMin = config.Bind("Colors", "PrimaryMin", new Color(1f, 0.55f, 0f, 1f), new ConfigDescription(
            "Guaranteed damage of the primary attack (lowest skill roll).", null,
            new ConfigurationManagerAttributes { Order = 30 }));
        PrimaryMax = config.Bind("Colors", "PrimaryMax", new Color(1f, 0.55f, 0f, 0.5f), new ConfigDescription(
            "Possible extra damage of the primary attack (up to the highest skill roll).", null,
            new ConfigurationManagerAttributes { Order = 20 }));
        Secondary = config.Bind("Colors", "Secondary", new Color(0.7f, 0.4f, 1f, 1f), new ConfigDescription(
            "Tick marks of the secondary attack / fully drawn bow.", null,
            new ConfigurationManagerAttributes { Order = 10 }));
    }
}
