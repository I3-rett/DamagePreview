using System.Collections.Generic;
using DamagePreview.Core;

namespace DamagePreview;

/// <summary>Once per EnemyHud.UpdateHuds: attacker side once, target side per visible hud.</summary>
internal static class PreviewDriver
{
    public static void Update(EnemyHud hud, Player player)
    {
        Dictionary<Character, EnemyHud.HudData> huds = hud.m_huds;
        if (huds == null || huds.Count == 0) return;

        AttackerPreview? found = null;
        if (player == null || !AttackerReader.TryRead(player, out found) || found == null)
        {
            HideAll(huds);
            return;
        }
        AttackerPreview attacker = found;

        foreach (KeyValuePair<Character, EnemyHud.HudData> pair in huds)
        {
            Character c = pair.Key;
            EnemyHud.HudData data = pair.Value;
            if (data.m_gui == null || !data.m_gui.activeSelf) continue;

            DamageGhost? ghost = DamageGhost.Attach(data);
            if (ghost == null) continue;

            if (!TargetReader.IsPreviewable(c, data))
            {
                ghost.Hide();
                continue;
            }

            TargetInput target = TargetReader.Read(c, attacker.BackstabBonus);
            var primary = new DamageRange(
                TargetSide.Apply(attacker.PrimaryMin, target),
                TargetSide.Apply(attacker.PrimaryMax, target));
            DamageRange? secondary = attacker.HasSecondary
                ? new DamageRange(TargetSide.Apply(attacker.SecondaryMin, target), TargetSide.Apply(attacker.SecondaryMax, target))
                : null;

            float maxHealth = c.GetMaxHealth();
            float fraction = maxHealth > 0f ? c.GetHealth() / maxHealth : 0f;
            ghost.Show(fraction, maxHealth, primary, secondary);
        }
    }

    public static void HideAllPublic(EnemyHud hud)
    {
        if (hud.m_huds != null) HideAll(hud.m_huds);
    }

    private static void HideAll(Dictionary<Character, EnemyHud.HudData> huds)
    {
        foreach (EnemyHud.HudData data in huds.Values)
        {
            if (data.m_gui == null) continue;
            data.m_gui.GetComponent<DamageGhost>()?.Hide();
        }
    }
}
