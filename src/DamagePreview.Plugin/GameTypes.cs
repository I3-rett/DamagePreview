using DamagePreview.Core;

namespace DamagePreview;

/// <summary>Field-by-field copies between the game structs and the Core mirrors.</summary>
internal static class GameTypes
{
    public static DamageTypes ToCore(HitData.DamageTypes d) => new(
        d.m_damage, d.m_blunt, d.m_slash, d.m_pierce, d.m_chop, d.m_pickaxe,
        d.m_fire, d.m_frost, d.m_lightning, d.m_poison, d.m_spirit, d.m_nonPlayer);

    public static HitData.DamageTypes ToGame(DamageTypes d) => new()
    {
        m_damage = d.Damage, m_blunt = d.Blunt, m_slash = d.Slash, m_pierce = d.Pierce,
        m_chop = d.Chop, m_pickaxe = d.Pickaxe, m_fire = d.Fire, m_frost = d.Frost,
        m_lightning = d.Lightning, m_poison = d.Poison, m_spirit = d.Spirit, m_nonPlayer = d.NonPlayer,
    };

    public static DamageModifiers ToCore(HitData.DamageModifiers m) => new(
        (DamageModifier)m.m_blunt, (DamageModifier)m.m_slash, (DamageModifier)m.m_pierce,
        (DamageModifier)m.m_chop, (DamageModifier)m.m_pickaxe, (DamageModifier)m.m_fire,
        (DamageModifier)m.m_frost, (DamageModifier)m.m_lightning, (DamageModifier)m.m_poison,
        (DamageModifier)m.m_spirit, (DamageModifier)m.m_nonPlayer);
}
