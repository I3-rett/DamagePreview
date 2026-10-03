namespace DamagePreview.Core;

/// <summary>Which attack each visual slot shows. Segments = "primary" slot, ticks = "secondary" slot.
/// For a bow the ticks reuse the primary attack at full draw.</summary>
public static class AttackSlots
{
    public static float EffectiveDraw(bool isDrawWeapon, float liveDraw, bool undrawnIsFull)
    {
        if (!isDrawWeapon) return 1f;
        if (liveDraw > 0f) return Mathx.Clamp01(liveDraw);
        return undrawnIsFull ? 1f : 0f;
    }

    public static bool HasSecondarySlot(bool isDrawWeapon, bool hasSecondaryAttack) =>
        isDrawWeapon || hasSecondaryAttack;

    public static float SecondaryDraw(bool isDrawWeapon) => 1f;
}
