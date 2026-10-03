using DamagePreview.Core;
using Xunit;

namespace DamagePreview.Tests;

public class ResistanceTests
{
    [Fact]
    public void Apply_multiplies_each_channel_by_its_modifier_and_leaves_damage_untouched()
    {
        var d = new DamageTypes(damage: 10, pierce: 10, fire: 10, poison: 10);
        var m = new DamageModifiers(pierce: DamageModifier.Resistant, fire: DamageModifier.VeryWeak,
            poison: DamageModifier.Immune);

        DamageTypes r = Resistance.Apply(d, m);

        Assert.Equal(10f, r.Damage);
        Assert.Equal(5f, r.Pierce);
        Assert.Equal(20f, r.Fire);
        Assert.Equal(0f, r.Poison);
    }
}
