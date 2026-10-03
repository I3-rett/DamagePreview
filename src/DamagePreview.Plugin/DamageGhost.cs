using DamagePreview.Core;
using UnityEngine;
using UnityEngine.UI;

namespace DamagePreview;

/// <summary>Four images inside one enemy health bar: two segments for the primary attack,
/// two tick marks for the secondary. Lives on the hud GameObject and dies with it.
/// Never touches the vanilla GuiBars or the Name text (HealthBar Plus owns that).</summary>
internal sealed class DamageGhost : MonoBehaviour
{
    private const float TickWidth = 2f;

    private RectTransform _reference = null!;
    private GuiBar _slow = null!;
    private RectTransform _ghostMax = null!;
    private RectTransform _ghostMin = null!;
    private RectTransform _tickMax = null!;
    private RectTransform _tickMin = null!;
    private bool _built;

    /// <summary>Returns the ghost for this hud, building it on first sight. Null when the
    /// hud is gone or does not have the vanilla bar hierarchy (another mod replaced it).</summary>
    public static DamageGhost? Attach(EnemyHud.HudData hud)
    {
        GameObject? gui = hud.m_gui;
        if (gui == null || hud.m_healthFast == null || hud.m_healthSlow == null) return null;
        if (hud.m_healthFast.m_bar == null || hud.m_healthSlow.m_bar == null) return null;

        DamageGhost ghost = gui.GetComponent<DamageGhost>() ?? gui.AddComponent<DamageGhost>();
        if (!ghost._built)
        {
            ghost.Build(hud);
        }
        return ghost;
    }

    public void Show(float healthFraction, float maxHealth, DamageRange primary, DamageRange? secondary)
    {
        if (_reference == null || _slow == null) { Hide(); return; }
        float width = _slow.m_width > 0f ? _slow.m_width : 0f;
        if (width <= 0f) { Hide(); return; }
        float x = _reference.anchoredPosition.x;
        float y = _reference.anchoredPosition.y;

        if (primary.IsNothing)
        {
            _ghostMax.gameObject.SetActive(false);
            _ghostMin.gameObject.SetActive(false);
        }
        else
        {
            (float fromMax, float to) = GhostLayout.Segment(healthFraction, primary.Max, maxHealth);
            (float fromMin, _) = GhostLayout.Segment(healthFraction, primary.Min, maxHealth);
            Place(_ghostMax, x + fromMax * width, y, (to - fromMax) * width, true);
            Place(_ghostMin, x + fromMin * width, y, (to - fromMin) * width, true);
        }

        bool ticks = secondary.HasValue && !secondary.Value.IsNothing && Settings.ShowSecondary.Value;
        if (!ticks)
        {
            _tickMax.gameObject.SetActive(false);
            _tickMin.gameObject.SetActive(false);
        }
        else
        {
            (float fromMax, _) = GhostLayout.Segment(healthFraction, secondary!.Value.Max, maxHealth);
            (float fromMin, _) = GhostLayout.Segment(healthFraction, secondary.Value.Min, maxHealth);
            Place(_tickMax, x + fromMax * width, y, TickWidth, true);
            Place(_tickMin, x + fromMin * width, y, TickWidth, true);
        }

        _ghostMax.GetComponent<Image>().color = Settings.PrimaryMax.Value;
        _ghostMin.GetComponent<Image>().color = Settings.PrimaryMin.Value;
        _tickMax.GetComponent<Image>().color = Settings.Secondary.Value;
        _tickMin.GetComponent<Image>().color = Settings.Secondary.Value;
    }

    public void Hide()
    {
        if (!_built) return;
        _ghostMax.gameObject.SetActive(false);
        _ghostMin.gameObject.SetActive(false);
        _tickMax.gameObject.SetActive(false);
        _tickMin.gameObject.SetActive(false);
    }

    private void Build(EnemyHud.HudData hud)
    {
        _slow = hud.m_healthSlow;
        _reference = hud.m_healthFast.m_bar;
        Image? source = _reference.GetComponent<Image>();
        Transform track = _reference.parent;

        _ghostMax = MakeImage("dp_ghost_max", track, source);
        _ghostMin = MakeImage("dp_ghost_min", track, source);
        _tickMax = MakeImage("dp_tick_max", track, source);
        _tickMin = MakeImage("dp_tick_min", track, source);
        _built = true;
        Hide();
    }

    private RectTransform MakeImage(string name, Transform parent, Image? source)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);
        var rt = (RectTransform)go.transform;
        rt.anchorMin = _reference.anchorMin;
        rt.anchorMax = _reference.anchorMax;
        rt.pivot = _reference.pivot;
        rt.anchoredPosition = _reference.anchoredPosition;
        rt.sizeDelta = _reference.sizeDelta;
        var image = go.GetComponent<Image>();
        if (source != null)
        {
            image.sprite = source.sprite;
            image.material = source.material;
            image.type = source.type;
        }
        image.raycastTarget = false;
        go.transform.SetAsLastSibling();
        return rt;
    }

    private static void Place(RectTransform rt, float x, float y, float width, bool active)
    {
        if (width <= 0f)
        {
            rt.gameObject.SetActive(false);
            return;
        }
        rt.gameObject.SetActive(active);
        rt.anchoredPosition = new Vector2(x, y);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
}
