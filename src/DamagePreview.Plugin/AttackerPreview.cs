using DamagePreview.Core;

namespace DamagePreview;

/// <summary>Attacker-side damage after status effects, for both visual slots. One per frame.</summary>
internal sealed class AttackerPreview
{
    public DamageTypes PrimaryMin;
    public DamageTypes PrimaryMax;
    public bool HasSecondary;
    public DamageTypes SecondaryMin;
    public DamageTypes SecondaryMax;
    public float BackstabBonus;
}
