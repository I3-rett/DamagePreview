using DamagePreview.Core;
using Xunit;

namespace DamagePreview.Tests;

public class AttackSlotsTests
{
    [Theory]
    [InlineData(false, 0f, true, 1f)]     // melee: draw is always 1
    [InlineData(false, 0.3f, true, 1f)]
    [InlineData(true, 0.3f, true, 0.3f)]  // bow drawing: live value
    [InlineData(true, 0f, true, 1f)]      // bow undrawn, option on: full
    [InlineData(true, 0f, false, 0f)]     // bow undrawn, option off: nothing
    public void EffectiveDraw(bool isDraw, float live, bool undrawnIsFull, float expected)
    {
        Assert.Equal(expected, AttackSlots.EffectiveDraw(isDraw, live, undrawnIsFull));
    }

    [Theory]
    [InlineData(true, false, true)]   // bow: ticks are the full draw
    [InlineData(false, true, true)]   // melee with secondary
    [InlineData(false, false, false)] // crossbow / plain weapon
    public void HasSecondarySlot(bool isDraw, bool hasSecondary, bool expected)
    {
        Assert.Equal(expected, AttackSlots.HasSecondarySlot(isDraw, hasSecondary));
    }

    [Fact]
    public void SecondaryDraw_is_always_full()
    {
        Assert.Equal(1f, AttackSlots.SecondaryDraw(true));
        Assert.Equal(1f, AttackSlots.SecondaryDraw(false));
    }
}
