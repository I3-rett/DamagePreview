namespace DamagePreview.Core;

/// <summary>Where a damage amount lands on a 0..1 health bar.</summary>
public static class GhostLayout
{
    /// <summary>Segment from the post-hit health fraction to the current one, clamped to the bar.</summary>
    public static (float From, float To) Segment(float healthFraction, float damage, float maxHealth)
    {
        float to = Mathx.Clamp01(healthFraction);
        if (damage <= 0f || maxHealth <= 0f)
        {
            return (to, to);
        }
        float from = Mathx.Clamp01(healthFraction - damage / maxHealth);
        return (from, to);
    }
}
