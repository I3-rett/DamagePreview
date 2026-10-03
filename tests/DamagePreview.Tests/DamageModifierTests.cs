using DamagePreview.Core;
using Xunit;

namespace DamagePreview.Tests;

public class DamageModifierTests
{
    // Values quoted from HitData.ApplyModifier(float, DamageModifier, ...) in IL, 2026-10-03.
    [Theory]
    [InlineData(DamageModifier.Normal, 1f)]
    [InlineData(DamageModifier.Resistant, 0.5f)]
    [InlineData(DamageModifier.Weak, 1.5f)]
    [InlineData(DamageModifier.Immune, 0f)]
    [InlineData(DamageModifier.Ignore, 0f)]
    [InlineData(DamageModifier.VeryResistant, 0.25f)]
    [InlineData(DamageModifier.VeryWeak, 2f)]
    [InlineData(DamageModifier.SlightlyResistant, 0.75f)]
    [InlineData(DamageModifier.SlightlyWeak, 1.25f)]
    public void Multiplier_matches_the_game_table(DamageModifier modifier, float expected)
    {
        Assert.Equal(expected, DamageModifierMath.Multiplier(modifier));
    }

    [Fact]
    public void Enum_order_matches_the_game_so_casts_are_safe()
    {
        Assert.Equal(0, (int)DamageModifier.Normal);
        Assert.Equal(1, (int)DamageModifier.Resistant);
        Assert.Equal(2, (int)DamageModifier.Weak);
        Assert.Equal(3, (int)DamageModifier.Immune);
        Assert.Equal(4, (int)DamageModifier.Ignore);
        Assert.Equal(5, (int)DamageModifier.VeryResistant);
        Assert.Equal(6, (int)DamageModifier.VeryWeak);
        Assert.Equal(7, (int)DamageModifier.SlightlyResistant);
        Assert.Equal(8, (int)DamageModifier.SlightlyWeak);
    }
}
