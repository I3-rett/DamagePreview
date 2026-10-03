using DamagePreview.Core;
using Xunit;

namespace DamagePreview.Tests;

public class ArmorFormulaTests
{
    // HitData.DamageTypes.ApplyArmor(float dmg, float ac):
    //   result = Clamp01(dmg / (ac * 4)) * dmg;  if (ac < dmg / 2) result = dmg - ac;
    [Fact]
    public void Low_armor_subtracts_flat()
    {
        Assert.Equal(90f, ArmorFormula.Apply(100f, 10f), 4);   // 10 < 50
    }

    [Fact]
    public void High_armor_scales_quadratically()
    {
        // ac 60 >= 50: Clamp01(100 / 240) * 100 = 41.666
        Assert.Equal(100f / 240f * 100f, ArmorFormula.Apply(100f, 60f), 3);
    }

    [Fact]
    public void Boundary_ac_equal_to_half_damage_uses_the_scaled_branch()
    {
        // ac 50 is not < 50: Clamp01(100/200) * 100 = 50
        Assert.Equal(50f, ArmorFormula.Apply(100f, 50f), 4);
    }

    [Fact]
    public void ApplyTo_scales_the_nine_armored_channels_proportionally_and_leaves_the_rest()
    {
        var d = new DamageTypes(damage: 7, blunt: 50, pierce: 50, chop: 9, pickaxe: 11, fire: 0);
        // armored sum = 100, ac 10 -> 90 -> factor 0.9
        DamageTypes r = ArmorFormula.ApplyTo(d, 10f);

        Assert.Equal(45f, r.Blunt, 4);
        Assert.Equal(45f, r.Pierce, 4);
        Assert.Equal(7f, r.Damage);
        Assert.Equal(9f, r.Chop);
        Assert.Equal(11f, r.Pickaxe);
    }

    [Fact]
    public void ApplyTo_with_zero_armor_is_identity()
    {
        var d = new DamageTypes(blunt: 50);
        DamageTypes r = ArmorFormula.ApplyTo(d, 0f);
        Assert.Equal(50f, r.Blunt);
    }

    [Fact]
    public void ApplyTo_with_zero_armored_damage_does_not_divide_by_zero()
    {
        var d = new DamageTypes(damage: 5, chop: 3);
        DamageTypes r = ArmorFormula.ApplyTo(d, 100f);
        Assert.Equal(5f, r.Damage);
        Assert.Equal(3f, r.Chop);
    }
}
