using UnityEngine;

[System.Serializable]
public class PressureModel
{
    [Tooltip("How strongly flow imbalance changes pressure.")]
    public float gain = 2f;

    [Tooltip("If true, pressure will slowly drift back toward ambient pressure.")]
    public bool enableAmbientRelaxation = false;

    [Tooltip("Ambient pressure (kPa). Used if ambient relaxation is enabled.")]
    public float ambientPressureKPa = 101.3f;

    [Tooltip("How fast pressure returns toward ambient if enabled.")]
    public float ambientRelaxationRate = 0.2f;

    public float Step(float currentPressureKPa, float inflow, float outflow, float dt)
    {
        // Base behavior: pressure rises if inflow > outflow, falls if outflow > inflow.
        float deltaFlow = inflow - outflow;
        float nextPressure = currentPressureKPa + (deltaFlow * gain * dt);

        // Slight drift toward ambient to keep the system stable and realistic.
        if (enableAmbientRelaxation)
        {
            float toAmbient = ambientPressureKPa - nextPressure;
            nextPressure += toAmbient * ambientRelaxationRate * dt;
        }

        return nextPressure;
    }
}
