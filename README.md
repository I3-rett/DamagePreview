# DamagePreview

A client-side Valheim mod that previews how much damage your weapon will do, right on the enemy health bar. The preview shows the minimum and maximum hit based on your current skill level, and for secondary attacks that land in several ticks it shows each tick. Nothing is sent over the network and nothing is written to the world, so it works on any server without the server needing the mod.

## Install

- **r2modman / Thunderstore:** install DamagePreview from the mod manager.
- **Manual:** install BepInEx for Valheim, then unzip the release so that `DamagePreview.dll` and `DamagePreview.Core.dll` sit in `BepInEx/plugins/DamagePreview/`.

## Build

You need the .NET SDK and a Valheim install with BepInEx.

```
dotnet build DamagePreview.sln -c Debug
```

To copy the result into a mod profile as part of the build:

```
dotnet build -p:Deploy=true -p:BepInExPath="%APPDATA%\r2modmanPlus-local\Valheim\profiles\dev\BepInEx"
```

## Test

```
dotnet test tests/DamagePreview.Tests/DamagePreview.Tests.csproj
```

The tests read the installed game assemblies to check that the game members the mod depends on still exist. Set `VALHEIM_MANAGED` to your `valheim_Data\Managed` folder if Valheim is not in a standard Steam location. To run without Valheim installed, set `DAMAGEPREVIEW_ALLOW_MISSING_GAME=1`; those runs do not check API compatibility.

## Configuration

| Section | Setting | Default | Description |
| --- | --- | --- | --- |
| General | Enabled | true | Master switch. Off hides every preview immediately. |
| General | IncludeBackstab | true | Apply the weapon's backstab bonus when the target has not noticed you and its backstab cooldown has elapsed. |
| General | UndrawnBowIsFullDraw | true | With a bow that is not drawn, preview a fully drawn shot instead of nothing. |
| General | ShowSecondary | true | Show the two tick marks for the secondary attack (or the fully drawn shot of a bow). |
| Colors | PrimaryMin | orange (1, 0.55, 0, 1) | Guaranteed damage of the primary attack (lowest skill roll). |
| Colors | PrimaryMax | orange, half transparent (1, 0.55, 0, 0.5) | Possible extra damage of the primary attack (up to the highest skill roll). |
| Colors | Secondary | purple (0.7, 0.4, 1, 1) | Tick marks of the secondary attack / fully drawn bow. |

## Compatibility

- **HealthBar Plus:** works alongside it. Verified by reading its code: it only edits the Name text of the health bar.
- **Creature Level & Loot Control:** works alongside it. The preview patch is ordered after it.

## Known limitations

- Damage over time (poison, fire, spirit) is not shown.
- Creatures that are blocking with a shield show the unblocked hit.
- Weak spots are not modelled.
- The extra backstab condition of the PassiveMobs world modifier is not modelled.
- No preview is shown on the creature you are riding.

## Package

```
powershell -File build/Package.ps1
```

Builds Release and writes `build/out/DamagePreview-<version>.zip`, checking that `PluginInfo.cs` and `package/manifest.json` agree on the version.

## Release

1. Bump the version in `src/DamagePreview.Plugin/PluginInfo.cs`, `package/manifest.json` and `thunderstore.toml`, and add a section to `package/CHANGELOG.md`.
2. Commit and merge to `main`.
3. Tag the commit `vX.Y.Z` and push the tag. The Release workflow checks that the tag matches the source, builds, attaches the zip to a GitHub release and, if the `THUNDERSTORE_TOKEN` secret is set, publishes to Thunderstore.

## License

MIT
