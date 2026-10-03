using DamagePreview.Core;
using Xunit;

namespace DamagePreview.Tests;

public class SkillRollTests
{
    // Skills.GetRandomSkillFactor: n = Lerp(0.4, 1, f); a = Clamp01(n - 0.15); b = Clamp01(n + 0.15).
    [Theory]
    [InlineData(0f, 0.25f, 0.55f)]
    [InlineData(0.5f, 0.55f, 0.85f)]
    [InlineData(1f, 0.85f, 1f)]      // upper bound clamps at 1
    public void Range_matches_GetRandomSkillFactor_bounds(float factor, float min, float max)
    {
        (float lo, float hi) = SkillRoll.Range(factor);
        Assert.Equal(min, lo, 5);
        Assert.Equal(max, hi, 5);
    }

    [Fact]
    public void Range_clamps_a_factor_above_one()
    {
        (float lo, float hi) = SkillRoll.Range(1.5f);
        Assert.Equal(0.85f, lo, 5);
        Assert.Equal(1f, hi, 5);
    }
}
