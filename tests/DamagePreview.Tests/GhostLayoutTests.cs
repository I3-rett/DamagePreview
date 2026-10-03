using DamagePreview.Core;
using Xunit;

namespace DamagePreview.Tests;

public class GhostLayoutTests
{
    [Fact]
    public void Segment_ends_at_current_health_and_starts_damage_earlier()
    {
        (float from, float to) = GhostLayout.Segment(healthFraction: 0.8f, damage: 20f, maxHealth: 100f);
        Assert.Equal(0.6f, from, 5);
        Assert.Equal(0.8f, to, 5);
    }

    [Fact]
    public void Segment_clamps_at_zero_when_damage_exceeds_health()
    {
        (float from, float to) = GhostLayout.Segment(0.3f, 50f, 100f);
        Assert.Equal(0f, from);
        Assert.Equal(0.3f, to, 5);
    }

    [Fact]
    public void Segment_is_empty_when_damage_is_zero_or_max_health_is_zero()
    {
        Assert.Equal(GhostLayout.Segment(0.5f, 0f, 100f).From, GhostLayout.Segment(0.5f, 0f, 100f).To);
        Assert.Equal(GhostLayout.Segment(0.5f, 10f, 0f).From, GhostLayout.Segment(0.5f, 10f, 0f).To);
    }

    [Fact]
    public void DamageRange_IsNothing_when_max_is_zero()
    {
        Assert.True(new DamageRange(0f, 0f).IsNothing);
        Assert.False(new DamageRange(0f, 1f).IsNothing);
    }
}
