using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [Header("References")]
    public GloveboxSystem system;

    [Header("Text")]
    public TMP_Text pressureText;
    public TMP_Text inflowText;
    public TMP_Text outflowText;
    public TMP_Text statusText;

    [Header("Thresholds")]
    public float warningPressureLow = 97f;
    public float warningPressureHigh = 108f;

    private void Reset()
    {
        // Auto-find GloveboxSystem if possible
        system = FindFirstObjectByType<GloveboxSystem>();
    }

    private void Update()
    {
        if (system == null) return;

        float p = system.PressureKPa;
        float inflow = system.Inflow;
        float outflow = system.Outflow;

        if (pressureText != null) pressureText.text = $"Pressure: {p:F1} kPa";
        if (inflowText != null) inflowText.text = $"Inflow: {inflow:F2}";
        if (outflowText != null) outflowText.text = $"Outflow: {outflow:F2}";

        string status = ComputeStatus(p);
        if (statusText != null) statusText.text = $"Status: {status}";
    }

    private string ComputeStatus(float pressureKPa)
    {
        // Simple logic for now; later we’ll move this into Core.
        if (pressureKPa <= warningPressureLow || pressureKPa >= warningPressureHigh)
            return "WARNING";

        return "OK";
    }
}
