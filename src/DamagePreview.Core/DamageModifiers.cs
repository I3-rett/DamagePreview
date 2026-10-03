namespace DamagePreview.Core;

/// <summary>Mirror of HitData.DamageModifiers: one modifier per resisted channel.
/// The generic `Damage` channel has no modifier in the game.</summary>
public readonly struct DamageModifiers
{
    public readonly DamageModifier Blunt, Slash, Pierce, Chop, Pickaxe,
        Fire, Frost, Lightning, Poison, Spirit, NonPlayer;

    public DamageModifiers(
        DamageModifier blunt = DamageModifier.Normal, DamageModifier slash = DamageModifier.Normal,
        DamageModifier pierce = DamageModifier.Normal, DamageModifier chop = DamageModifier.Normal,
        DamageModifier pickaxe = DamageModifier.Normal, DamageModifier fire = DamageModifier.Normal,
        DamageModifier frost = DamageModifier.Normal, DamageModifier lightning = DamageModifier.Normal,
        DamageModifier poison = DamageModifier.Normal, DamageModifier spirit = DamageModifier.Normal,
        DamageModifier nonPlayer = DamageModifier.Normal)
    {
        Blunt = blunt; Slash = slash; Pierce = pierce; Chop = chop; Pickaxe = pickaxe;
        Fire = fire; Frost = frost; Lightning = lightning; Poison = poison; Spirit = spirit;
        NonPlayer = nonPlayer;
    }
}
