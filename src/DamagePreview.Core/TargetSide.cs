namespace DamagePreview.Core;

/// <summary>Character.RPC_Damage from the backstab check down to Character.ApplyDamage's
/// health subtraction. Returns the instant damage the target actually loses.</summary>
public static class TargetSide
{
    public static float Apply(DamageTypes hit, TargetInput t)
    {
        DamageTypes d = hit;
        if (t.BackstabApplies && t.BackstabBonus > 1f)
        {
            d = d.Scale(t.BackstabBonus);
        }
        if (t.Staggering)
        {
            d = d.Scale(2f);
        }
        d = Resistance.Apply(d, t.Modifiers);
        d = ArmorFormula.ApplyTo(d, t.WorldLevelArmor);

        // RPC_Damage zeroes poison/fire/spirit, then ApplyDamage scales and totals.
        float instant = d.InstantTotal() * t.DifficultyScaleEnemy * t.PlayerDamageRate;
        return instant <= 0.1f ? 0f : instant;
    }
}
