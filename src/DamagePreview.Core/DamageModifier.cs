namespace DamagePreview.Core;

/// <summary>Mirror of HitData.DamageModifier, same member order.</summary>
public enum DamageModifier
{
    Normal,
    Resistant,
    Weak,
    Immune,
    Ignore,
    VeryResistant,
    VeryWeak,
    SlightlyResistant,
    SlightlyWeak,
}

public static class DamageModifierMath
{
    /// <summary>Factor applied to one damage type, as in HitData.ApplyModifier.</summary>
    public static float Multiplier(DamageModifier modifier) => modifier switch
    {
        DamageModifier.Resistant => 0.5f,
        DamageModifier.Weak => 1.5f,
        DamageModifier.Immune => 0f,
        DamageModifier.Ignore => 0f,
        DamageModifier.VeryResistant => 0.25f,
        DamageModifier.VeryWeak => 2f,
        DamageModifier.SlightlyResistant => 0.75f,
        DamageModifier.SlightlyWeak => 1.25f,
        _ => 1f,
    };
}
