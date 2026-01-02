using UnityEngine;

public class GloveboxView : MonoBehaviour
{
    [Header("References")]
    public GloveboxSystem system;

    [Header("Renderers")]
    public MeshRenderer chamberRenderer;

    [Header("Visual Settings")]
    public Color normalColor = Color.gray;
    public Color warningColor = Color.yellow;
    public Color dangerColor = Color.red;

    [Header("Overrides")]
    public bool forceColor = false;
    public Color forcedColor = Color.green;

    private void Reset()
    {
        if (system == null)
            system = FindFirstObjectByType<GloveboxSystem>();

        if (chamberRenderer == null)
            chamberRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Update()
    {
        if (chamberRenderer == null)
            return;

        if (forceColor)
        {
            chamberRenderer.material.color = forcedColor;
            return;
        }

        if (system == null)
            return;

        float p = system.PressureKPa;

        if (p < system.minPressure || p > system.maxPressure)
            chamberRenderer.material.color = dangerColor;
        else if (p < system.minPressure + 2f || p > system.maxPressure - 2f)
            chamberRenderer.material.color = warningColor;
        else
            chamberRenderer.material.color = normalColor;
    }
}
