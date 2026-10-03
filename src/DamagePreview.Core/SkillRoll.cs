namespace DamagePreview.Core;

/// <summary>Bounds of Skills.GetRandomSkillFactor for a given skill factor (level / 100).</summary>
public static class SkillRoll
{
    public static (float Min, float Max) Range(float skillFactor)
    {
        float n = Mathx.Lerp(0.4f, 1f, Mathx.Clamp01(skillFactor));
        return (Mathx.Clamp01(n - 0.15f), Mathx.Clamp01(n + 0.15f));
    }
}
