using UnityEngine;

public class GloveboxController : MonoBehaviour
{
    [Header("References")]
    public GloveboxSystem system;

    [Header("Fault Injection")]
    public bool leakEnabled = false;
    public float leakRateKPaPerSec = 0.3f;

    [Header("Controls")]
    public KeyCode toggleLeakKey = KeyCode.L;

    private void Reset()
    {
        system = FindFirstObjectByType<GloveboxSystem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleLeakKey))
            leakEnabled = !leakEnabled;

        if (!leakEnabled || system == null) return;

        system.AddPressure(-leakRateKPaPerSec * Time.deltaTime);
    }
}
