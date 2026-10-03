namespace DamagePreview.Core;

/// <summary>Mirror of HitData.DamageTypes: the twelve damage channels. Immutable.</summary>
public readonly struct DamageTypes
{
    public readonly float Damage, Blunt, Slash, Pierce, Chop, Pickaxe,
        Fire, Frost, Lightning, Poison, Spirit, NonPlayer;

    public DamageTypes(
        float damage = 0, float blunt = 0, float slash = 0, float pierce = 0,
        float chop = 0, float pickaxe = 0, float fire = 0, float frost = 0,
        float lightning = 0, float poison = 0, float spirit = 0, float nonPlayer = 0)
    {
        Damage = damage; Blunt = blunt; Slash = slash; Pierce = pierce;
        Chop = chop; Pickaxe = pickaxe; Fire = fire; Frost = frost;
        Lightning = lightning; Poison = poison; Spirit = spirit; NonPlayer = nonPlayer;
    }

    public static DamageTypes Uniform(float v) => new(v, v, v, v, v, v, v, v, v, v, v, v);

    public DamageTypes Add(DamageTypes o) => new(
        Damage + o.Damage, Blunt + o.Blunt, Slash + o.Slash, Pierce + o.Pierce,
        Chop + o.Chop, Pickaxe + o.Pickaxe, Fire + o.Fire, Frost + o.Frost,
        Lightning + o.Lightning, Poison + o.Poison, Spirit + o.Spirit, NonPlayer + o.NonPlayer);

    public DamageTypes Scale(float f) => new(
        Damage * f, Blunt * f, Slash * f, Pierce * f, Chop * f, Pickaxe * f,
        Fire * f, Frost * f, Lightning * f, Poison * f, Spirit * f, NonPlayer * f);

    public DamageTypes Scale(DamageTypes m) => new(
        Damage * m.Damage, Blunt * m.Blunt, Slash * m.Slash, Pierce * m.Pierce,
        Chop * m.Chop, Pickaxe * m.Pickaxe, Fire * m.Fire, Frost * m.Frost,
        Lightning * m.Lightning, Poison * m.Poison, Spirit * m.Spirit, NonPlayer * m.NonPlayer);

    /// <summary>HitData.DamageTypes.GetTotalDamage: all twelve channels.</summary>
    public float Total() =>
        Damage + Blunt + Slash + Pierce + Chop + Pickaxe + Fire + Frost + Lightning + Poison + Spirit + NonPlayer;

    /// <summary>What Character.ApplyDamage subtracts from health: everything except the three
    /// channels RPC_Damage zeroes beforehand (poison, fire, spirit are damage over time).</summary>
    public float InstantTotal() =>
        Damage + Blunt + Slash + Pierce + Chop + Pickaxe + Frost + Lightning + NonPlayer;
}
