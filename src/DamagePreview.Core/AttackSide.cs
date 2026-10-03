namespace DamagePreview.Core;

/// <summary>Attack.FireProjectileBurst / DoMeleeAttack up to Attack.ModifyDamage, for the
/// lowest and highest skill roll. Status effects are applied by the caller afterwards.</summary>
public static class AttackSide
{
    public static (DamageTypes Min, DamageTypes Max) Compute(AttackInput a)
    {
        (float lo, float hi) = SkillRoll.Range(a.SkillFactor);
        DamageTypes baseDamage = a.Weapon.Add(a.Ammo);
        return (Modify(baseDamage, a, lo), Modify(baseDamage, a, hi));
    }

    /// <summary>Attack.GetLevelDamageFactor: 1 + max(0, level - 1) * 0.5.</summary>
    public static float LevelDamageFactor(int level) => 1f + System.Math.Max(0, level - 1) * 0.5f;

    // Attack.ModifyDamage(hitData, damageFactor) where damageFactor = roll * draw.
    private static DamageTypes Modify(DamageTypes d, AttackInput a, float roll) =>
        d.Scale(a.AttackDamageMultiplier)
         .Scale(roll * a.DrawPercent)
         .Scale(a.LevelDamageFactor);
}
