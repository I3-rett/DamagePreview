using DamagePreview.Core;

namespace DamagePreview;

/// <summary>Reads the local player's weapon, ammo, skill, draw and status effects.
/// Read-only: the scratch HitData never gets an attacker, a target, or leaves this class.</summary>
internal static class AttackerReader
{
    public static bool TryRead(Player player, out AttackerPreview preview)
    {
        preview = null!;
        ItemDrop.ItemData? weapon = player.GetCurrentWeapon();
        if (weapon == null) return false;

        ItemDrop.ItemData.SharedData shared = weapon.m_shared;
        Attack primary = shared.m_attack;
        bool isDraw = primary.m_bowDraw;

        DamageTypes weaponDamage = GameTypes.ToCore(weapon.GetDamage());
        DamageTypes ammoDamage = AmmoDamage(player, shared);
        float skill = player.GetSkillFactor(shared.m_skillType);
        float level = AttackSide.LevelDamageFactor(player.GetLevel());

        float draw = AttackSlots.EffectiveDraw(isDraw, player.GetAttackDrawPercentage(), Settings.UndrawnBowIsFullDraw.Value);
        (DamageTypes pMin, DamageTypes pMax) = AttackSide.Compute(
            new AttackInput(weaponDamage, ammoDamage, skill, draw, primary.m_damageMultiplier, level));

        preview = new AttackerPreview
        {
            PrimaryMin = WithStatusEffects(player, shared.m_skillType, pMin),
            PrimaryMax = WithStatusEffects(player, shared.m_skillType, pMax),
            BackstabBonus = shared.m_backstabBonus,
        };

        bool hasSecondaryAttack = weapon.HaveSecondaryAttack();
        if (AttackSlots.HasSecondarySlot(isDraw, hasSecondaryAttack))
        {
            // A bow's ticks are the primary attack at full draw; a melee weapon's are its secondary.
            float multiplier = isDraw ? primary.m_damageMultiplier : shared.m_secondaryAttack.m_damageMultiplier;
            (DamageTypes sMin, DamageTypes sMax) = AttackSide.Compute(
                new AttackInput(weaponDamage, ammoDamage, skill, AttackSlots.SecondaryDraw(isDraw), multiplier, level));
            preview.HasSecondary = true;
            preview.SecondaryMin = WithStatusEffects(player, shared.m_skillType, sMin);
            preview.SecondaryMax = WithStatusEffects(player, shared.m_skillType, sMax);
        }
        return true;
    }

    // Mirrors Attack.FindAmmo: equipped ammo if it is still in the inventory and of the right
    // type, otherwise the first matching stack.
    private static DamageTypes AmmoDamage(Player player, ItemDrop.ItemData.SharedData weapon)
    {
        if (string.IsNullOrEmpty(weapon.m_ammoType)) return default;
        ItemDrop.ItemData? ammo = player.GetAmmoItem();
        if (ammo != null && (!player.GetInventory().ContainsItem(ammo) || ammo.m_shared.m_ammoType != weapon.m_ammoType))
        {
            ammo = null;
        }
        ammo ??= player.GetInventory().GetAmmoItem(weapon.m_ammoType);
        return ammo == null ? default : GameTypes.ToCore(ammo.GetDamage());
    }

    // Lets the game apply every active status effect's attack modifier (SE_Stats.ModifyAttack).
    private static DamageTypes WithStatusEffects(Player player, Skills.SkillType skill, DamageTypes d)
    {
        var hit = new HitData { m_damage = GameTypes.ToGame(d) };
        player.GetSEMan().ModifyAttack(skill, ref hit);
        return GameTypes.ToCore(hit.m_damage);
    }
}
