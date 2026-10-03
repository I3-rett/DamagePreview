namespace DamagePreview.Core;

/// <summary>Everything about the attacker that the damage depends on, before status effects.</summary>
public sealed class AttackInput
{
    public AttackInput(DamageTypes weapon, DamageTypes ammo, float skillFactor, float drawPercent,
        float attackDamageMultiplier, float levelDamageFactor)
    {
        Weapon = weapon; Ammo = ammo; SkillFactor = skillFactor; DrawPercent = drawPercent;
        AttackDamageMultiplier = attackDamageMultiplier; LevelDamageFactor = levelDamageFactor;
    }

    public DamageTypes Weapon { get; }
    public DamageTypes Ammo { get; }
    /// <summary>Skill level / 100, clamped 0..1.</summary>
    public float SkillFactor { get; }
    /// <summary>Bow draw 0..1; 1 for anything that is not a draw weapon.</summary>
    public float DrawPercent { get; }
    /// <summary>Attack.m_damageMultiplier of the attack being previewed.</summary>
    public float AttackDamageMultiplier { get; }
    /// <summary>See AttackSide.LevelDamageFactor.</summary>
    public float LevelDamageFactor { get; }
}
