using DamagePreview.Core;
using UnityEngine;

namespace DamagePreview;

/// <summary>Reads what Character.RPC_Damage / ApplyDamage would read for this target. Read-only.</summary>
internal static class TargetReader
{
    private const float BackstabCooldownSeconds = 300f;

    public static bool IsPreviewable(Character c, EnemyHud.HudData hud) =>
        c != null && !c.IsPlayer() && !hud.m_isMount;

    public static TargetInput Read(Character c, float backstabBonus)
    {
        bool backstab = Settings.IncludeBackstab.Value
            && c.m_baseAI != null
            && !c.m_baseAI.IsAlerted()
            && backstabBonus > 1f
            && Time.time - c.m_backstabTime > BackstabCooldownSeconds;

        return new TargetInput(
            GameTypes.ToCore(c.GetDamageModifiers()),
            backstab, backstabBonus,
            c.IsStaggering(),
            Game.m_worldLevel * Game.instance.m_worldLevelEnemyBaseAC,
            Game.instance.GetDifficultyDamageScaleEnemy(c.transform.position),
            Game.m_playerDamageRate);
    }
}
