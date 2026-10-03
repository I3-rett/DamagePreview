namespace DamagePreview.Core;

/// <summary>HitData.ApplyResistance: each resisted channel times its modifier.</summary>
public static class Resistance
{
    public static DamageTypes Apply(DamageTypes d, DamageModifiers m) => new(
        d.Damage,
        d.Blunt * DamageModifierMath.Multiplier(m.Blunt),
        d.Slash * DamageModifierMath.Multiplier(m.Slash),
        d.Pierce * DamageModifierMath.Multiplier(m.Pierce),
        d.Chop * DamageModifierMath.Multiplier(m.Chop),
        d.Pickaxe * DamageModifierMath.Multiplier(m.Pickaxe),
        d.Fire * DamageModifierMath.Multiplier(m.Fire),
        d.Frost * DamageModifierMath.Multiplier(m.Frost),
        d.Lightning * DamageModifierMath.Multiplier(m.Lightning),
        d.Poison * DamageModifierMath.Multiplier(m.Poison),
        d.Spirit * DamageModifierMath.Multiplier(m.Spirit),
        d.NonPlayer * DamageModifierMath.Multiplier(m.NonPlayer));
}
