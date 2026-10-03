using DamagePreview.Core;
using Xunit;

namespace DamagePreview.Tests;

public class AttackSideTests
{
    [Fact]
    public void Compute_adds_ammo_then_scales_by_roll_draw_multiplier_and_level()
    {
        // Bow 32 pierce + arrow 20 pierce, skill 50 (roll 0.55..0.85), draw 0.5,
        // attack multiplier 1, level 1.
        var input = new AttackInput(
            weapon: new DamageTypes(pierce: 32),
            ammo: new DamageTypes(pierce: 20),
            skillFactor: 0.5f, drawPercent: 0.5f,
            attackDamageMultiplier: 1f, levelDamageFactor: 1f);

        (DamageTypes min, DamageTypes max) = AttackSide.Compute(input);

        Assert.Equal(52f * 0.55f * 0.5f, min.Pierce, 4);
        Assert.Equal(52f * 0.85f * 0.5f, max.Pierce, 4);
    }

    [Fact]
    public void Compute_applies_attack_multiplier_and_level_factor_to_every_channel()
    {
        var input = new AttackInput(
            weapon: new DamageTypes(slash: 10, fire: 10),
            ammo: default,
            skillFactor: 1f, drawPercent: 1f,
            attackDamageMultiplier: 2f, levelDamageFactor: 1.5f);

        (DamageTypes min, DamageTypes max) = AttackSide.Compute(input);

        Assert.Equal(10f * 0.85f * 2f * 1.5f, min.Slash, 4);
        Assert.Equal(10f * 1f * 2f * 1.5f, max.Fire, 4);
    }

    [Theory]
    [InlineData(1, 1f)]
    [InlineData(2, 1.5f)]
    [InlineData(3, 2f)]
    [InlineData(0, 1f)]
    public void LevelDamageFactor_matches_Attack_GetLevelDamageFactor(int level, float expected)
    {
        Assert.Equal(expected, AttackSide.LevelDamageFactor(level));
    }
}
