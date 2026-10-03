namespace DamagePreview.Core;

/// <summary>Lowest and highest instant damage of one attack against one target.</summary>
public readonly struct DamageRange
{
    public readonly float Min;
    public readonly float Max;

    public DamageRange(float min, float max)
    {
        Min = min; Max = max;
    }

    public bool IsNothing => Max <= 0f;
}
