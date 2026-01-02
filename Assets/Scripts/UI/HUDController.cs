using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [Header("Status Override")]
    public bool statusOverrideEnabled = false;
    public string statusOverrideText = "PASSED";
    public Color statusOverrideColor = Color.green;

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

    [Header("Colors")]
    public Color okColor = Color.white;
    public Color warningColor = Color.yellow;

    private void Reset()
    {
        system = FindFirstObjectByType<GloveboxSystem>();
    }

    private void Update()
    {
        if (system == null) return;

        float p = system.PressureKPa;
        float inflow = system.Inflow;
        float outflow = system.Outflow;

        if (pressureText != null)
            pressureText.text = $"Pressure: {p:F1} kPa";

        if (inflowText != null)
            inflowText.text = $"Inflow: {inflow:F2}";

        if (outflowText != null)
            outflowText.text = $"Outflow: {outflow:F2}";

        // ✅ STATUS OVERRIDE (PASSED sticks)
        if (statusOverrideEnabled && statusText != null)
        {
            statusText.text = $"Status: {statusOverrideText}";
            statusText.color = statusOverrideColor;
            return;
        }

        // Normal status behavior
        bool warning = (p <= warningPressureLow || p >= warningPressureHigh);
        if (statusText != null)
        {
            statusText.text = $"Status: {(warning ? "WARNING" : "OK")}";
            statusText.color = warning ? warningColor : okColor;
        }
    }
}
