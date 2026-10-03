namespace DamagePreview;

/// <summary>Single source of truth for the plugin identity. package/manifest.json and
/// thunderstore.toml must carry the same version; the release workflow checks.</summary>
internal static class PluginInfo
{
    public const string Guid = "I3_rett.DamagePreview";
    public const string Name = "DamagePreview";
    public const string Version = "0.1.0";
}
