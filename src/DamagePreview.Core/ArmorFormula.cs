namespace DamagePreview.Core;

/// <summary>HitData.DamageTypes.ApplyArmor, both overloads.</summary>
public static class ArmorFormula
{
    public static float Apply(float damage, float armor)
    {
        float result = Mathx.Clamp01(damage / (armor * 4f)) * damage;
        if (armor < damage / 2f)
        {
            result = damage - armor;
        }
        return result;
    }

    /// <summary>Scales the nine armored channels by one common factor. `Damage`, `Chop`
    /// and `Pickaxe` are not armored in the game.</summary>
    public static DamageTypes ApplyTo(DamageTypes d, float armor)
    {
        if (armor <= 0f)
        {
            return d;
        }
        float sum = d.Blunt + d.Slash + d.Pierce + d.Fire + d.Frost + d.Lightning + d.Poison + d.Spirit + d.NonPlayer;
        if (sum <= 0f)
        {
            return d;
        }
        float f = Apply(sum, armor) / sum;
        return new DamageTypes(
            d.Damage, d.Blunt * f, d.Slash * f, d.Pierce * f, d.Chop, d.Pickaxe,
            d.Fire * f, d.Frost * f, d.Lightning * f, d.Poison * f, d.Spirit * f, d.NonPlayer * f);
    }
}
