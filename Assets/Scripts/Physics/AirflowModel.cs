using UnityEngine;

[System.Serializable]
public class AirflowModel
{
    [Header("Base Flow Rates")]
    public float inflow = 1f;
    public float outflow = 1f;

    [Header("Limits")]
    public float minFlow = 0f;
    public float maxFlow = 5f;

    [Header("Keyboard Controls")]
    [Tooltip("How fast flows change when you hold keys.")]
    public float adjustRate = 1.5f;

    [Tooltip("Hold to increase/decrease inflow/outflow. Defaults: W/S inflow, D/A outflow.")]
    public KeyCode inflowUpKey = KeyCode.W;
    public KeyCode inflowDownKey = KeyCode.S;
    public KeyCode outflowUpKey = KeyCode.D;
    public KeyCode outflowDownKey = KeyCode.A;

    public void TickInput(float dt)
    {
        float inflowDelta = 0f;
        float outflowDelta = 0f;

        if (Input.GetKey(inflowUpKey)) inflowDelta += adjustRate * dt;
        if (Input.GetKey(inflowDownKey)) inflowDelta -= adjustRate * dt;

        if (Input.GetKey(outflowUpKey)) outflowDelta += adjustRate * dt;
        if (Input.GetKey(outflowDownKey)) outflowDelta -= adjustRate * dt;

        if (inflowDelta != 0f) SetInflow(inflow + inflowDelta);
        if (outflowDelta != 0f) SetOutflow(outflow + outflowDelta);
    }

    public void SetInflow(float value)
    {
        inflow = Mathf.Clamp(value, minFlow, maxFlow);
    }

    public void SetOutflow(float value)
    {
        outflow = Mathf.Clamp(value, minFlow, maxFlow);
    }

    public float GetInflow() => inflow;
    public float GetOutflow() => outflow;
}
