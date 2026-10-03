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

## Compatibility

Works alongside HealthBar Plus and CLLC.

## Known limitations

- Damage over time (fire, poison, spirit) is not shown.
- Creatures that are blocking are not accounted for.
- PassiveMobs is not supported.

## License

MIT
