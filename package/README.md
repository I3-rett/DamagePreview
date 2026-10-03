# DamagePreview

Shows how much damage your equipped weapon will do, directly on the enemy health bar. Client side only: it works on any server without the server having the mod.

## What you see

A highlighted segment at the end of the enemy's health bar. The solid part is the damage you are guaranteed to deal (lowest skill roll); the faded part is the extra you might deal (up to the highest roll). If the segment reaches the start of the bar, the hit can kill. Purple tick marks show the secondary attack, or the fully drawn shot with a bow.

## What it accounts for

- Your weapon skill level
- Bow draw (an undrawn bow can preview a full draw)
- Backstab bonus when the target has not noticed you
- Stagger bonus
- The target's damage resistances and armor

## Configuration

Settings are in `BepInEx/config/` (or the mod manager config editor).

| Section | Setting | Default | Effect |
| --- | --- | --- | --- |
| General | Enabled | true | Master switch |
| General | IncludeBackstab | true | Apply the backstab bonus when it would apply |
| General | UndrawnBowIsFullDraw | true | Preview a full draw while the bow is not drawn |
| General | ShowSecondary | true | Show the secondary attack tick marks |
| Colors | PrimaryMin | orange | Guaranteed damage |
| Colors | PrimaryMax | orange, half transparent | Possible extra damage |
| Colors | Secondary | purple | Secondary attack tick marks |

## Compatibility

- HealthBar Plus: works alongside it.
- Creature Level & Loot Control: works alongside it.

## Known limitations

- Damage over time (poison, fire, spirit) is not shown.
- Creatures that block show the unblocked hit.
- Weak spots are not modelled.
- The extra backstab condition of the PassiveMobs world modifier is not modelled.
- No preview is shown on the creature you are riding.

Source and issues: https://github.com/I3-rett/DamagePreview
