namespace DamagePreview.Core;

/// <summary>Everything about the target and the world that Character.RPC_Damage and
/// Character.ApplyDamage read before subtracting health.</summary>
public sealed class TargetInput
{
    public TargetInput(DamageModifiers modifiers, bool backstabApplies, float backstabBonus,
        bool staggering, float worldLevelArmor, float difficultyScaleEnemy, float playerDamageRate)
    {
        Modifiers = modifiers; BackstabApplies = backstabApplies; BackstabBonus = backstabBonus;
        Staggering = staggering; WorldLevelArmor = worldLevelArmor;
        DifficultyScaleEnemy = difficultyScaleEnemy; PlayerDamageRate = playerDamageRate;
    }

    public DamageModifiers Modifiers { get; }
    /// <summary>Target not alerted, backstab cooldown elapsed, option enabled.</summary>
    public bool BackstabApplies { get; }
    /// <summary>Weapon m_backstabBonus.</summary>
    public float BackstabBonus { get; }
    public bool Staggering { get; }
    /// <summary>Game.m_worldLevel * Game.instance.m_worldLevelEnemyBaseAC.</summary>
    public float WorldLevelArmor { get; }
    /// <summary>Game.GetDifficultyDamageScaleEnemy(position).</summary>
    public float DifficultyScaleEnemy { get; }
    /// <summary>Game.m_playerDamageRate.</summary>
    public float PlayerDamageRate { get; }
}
