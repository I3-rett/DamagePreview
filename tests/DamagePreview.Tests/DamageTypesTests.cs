using DamagePreview.Core;
using Xunit;

namespace DamagePreview.Tests;

public class DamageTypesTests
{
    [Fact]
    public void Add_sums_every_field()
    {
        var a = new DamageTypes(damage: 1, blunt: 2, slash: 3, pierce: 4, chop: 5, pickaxe: 6,
            fire: 7, frost: 8, lightning: 9, poison: 10, spirit: 11, nonPlayer: 12);
        var b = new DamageTypes(pierce: 20, poison: 5);

        DamageTypes sum = a.Add(b);

        Assert.Equal(24f, sum.Pierce);
        Assert.Equal(15f, sum.Poison);
        Assert.Equal(1f, sum.Damage);
        Assert.Equal(12f, sum.NonPlayer);
    }

    [Fact]
    public void Scale_by_float_multiplies_every_field()
    {
        var d = new DamageTypes(damage: 1, blunt: 2, fire: 4, nonPlayer: 8);

        DamageTypes s = d.Scale(0.5f);

        Assert.Equal(0.5f, s.Damage);
        Assert.Equal(1f, s.Blunt);
        Assert.Equal(2f, s.Fire);
        Assert.Equal(4f, s.NonPlayer);
    }

    [Fact]
    public void Scale_per_type_multiplies_field_by_field()
    {
        var d = new DamageTypes(pierce: 10, fire: 10);
        var m = new DamageTypes(damage: 1, blunt: 1, slash: 1, pierce: 2, chop: 1, pickaxe: 1,
            fire: 0.5f, frost: 1, lightning: 1, poison: 1, spirit: 1, nonPlayer: 1);

        DamageTypes s = d.Scale(m);

        Assert.Equal(20f, s.Pierce);
        Assert.Equal(5f, s.Fire);
    }

    [Fact]
    public void Total_includes_all_twelve_fields_like_the_game()
    {
        var d = new DamageTypes(damage: 1, blunt: 1, slash: 1, pierce: 1, chop: 1, pickaxe: 1,
            fire: 1, frost: 1, lightning: 1, poison: 1, spirit: 1, nonPlayer: 1);

        Assert.Equal(12f, d.Total());
    }

    [Fact]
    public void InstantTotal_excludes_poison_fire_and_spirit()
    {
        var d = new DamageTypes(damage: 1, blunt: 1, slash: 1, pierce: 1, chop: 1, pickaxe: 1,
            fire: 100, frost: 1, lightning: 1, poison: 100, spirit: 100, nonPlayer: 1);

        Assert.Equal(9f, d.InstantTotal());
    }

    [Fact]
    public void Uniform_sets_every_field()
    {
        DamageTypes u = DamageTypes.Uniform(1f);
        Assert.Equal(12f, u.Total());
    }
}
