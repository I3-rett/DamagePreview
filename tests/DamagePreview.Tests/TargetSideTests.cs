using DamagePreview.Core;
using Xunit;

namespace DamagePreview.Tests;

public class TargetSideTests
{
    private static TargetInput Plain(
        DamageModifiers? modifiers = null, bool backstab = false, float backstabBonus = 3f,
        bool staggering = false, float armor = 0f, float difficulty = 1f, float rate = 1f) =>
        new(modifiers ?? new DamageModifiers(), backstab, backstabBonus, staggering, armor, difficulty, rate);

    [Fact]
    public void Plain_hit_returns_instant_total()
    {
        float r = TargetSide.Apply(new DamageTypes(pierce: 40, poison: 30), Plain());
        Assert.Equal(40f, r, 4);
    }

    [Fact]
    public void Backstab_multiplies_when_it_applies_and_bonus_above_one()
    {
        Assert.Equal(120f, TargetSide.Apply(new DamageTypes(pierce: 40), Plain(backstab: true, backstabBonus: 3f)), 4);
        Assert.Equal(40f, TargetSide.Apply(new DamageTypes(pierce: 40), Plain(backstab: true, backstabBonus: 1f)), 4);
        Assert.Equal(40f, TargetSide.Apply(new DamageTypes(pierce: 40), Plain(backstab: false, backstabBonus: 3f)), 4);
    }

    [Fact]
    public void Staggering_target_takes_double()
    {
        Assert.Equal(80f, TargetSide.Apply(new DamageTypes(pierce: 40), Plain(staggering: true)), 4);
    }

    [Fact]
    public void Order_is_backstab_then_stagger_then_resistance_then_armor_then_difficulty()
    {
        // 40 pierce, backstab x3 = 120, stagger x2 = 240, Resistant = 120,
        // armor 10 (10 < 60) = 110, difficulty 0.5, rate 2 = 110.
        var t = Plain(new DamageModifiers(pierce: DamageModifier.Resistant),
            backstab: true, backstabBonus: 3f, staggering: true, armor: 10f, difficulty: 0.5f, rate: 2f);

        Assert.Equal(110f, TargetSide.Apply(new DamageTypes(pierce: 40), t), 3);
    }

    [Fact]
    public void Dot_channels_never_reach_health()
    {
        float r = TargetSide.Apply(new DamageTypes(fire: 50, poison: 50, spirit: 50), Plain());
        Assert.Equal(0f, r);
    }

    [Fact]
    public void Totals_at_or_below_point_one_count_as_nothing()
    {
        Assert.Equal(0f, TargetSide.Apply(new DamageTypes(pierce: 0.1f), Plain()));
        Assert.Equal(0f, TargetSide.Apply(new DamageTypes(pierce: 0.05f), Plain()));
        Assert.Equal(0.2f, TargetSide.Apply(new DamageTypes(pierce: 0.2f), Plain()), 5);
    }

    [Fact]
    public void Generic_damage_channel_skips_resistance_and_armor_but_counts()
    {
        var t = Plain(new DamageModifiers(pierce: DamageModifier.Immune), armor: 1000f);
        Assert.Equal(10f, TargetSide.Apply(new DamageTypes(damage: 10, pierce: 100), t), 4);
    }
}
