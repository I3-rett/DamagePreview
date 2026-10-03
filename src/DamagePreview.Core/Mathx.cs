namespace DamagePreview.Core;

/// <summary>The two Mathf helpers the game formulas use, for netstandard2.0.</summary>
public static class Mathx
{
    public static float Clamp01(float v) => v < 0f ? 0f : v > 1f ? 1f : v;

    public static float Lerp(float a, float b, float t) => a + (b - a) * Clamp01(t);
}
