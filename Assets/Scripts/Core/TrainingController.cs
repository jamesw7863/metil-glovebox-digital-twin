using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainingController : MonoBehaviour
{
    public enum State { Menu, Running, Passed }

    [Header("State")]
    public State state = State.Menu;

    [Header("References")]
    public GloveboxSystem system;
    public GloveboxController faults;
    public HUDController hud;
    public GloveboxView view;

    [Header("Panels")]
    public GameObject startPanel;        // Canvas/StartPanel
    public GameObject objectivePanel;    // Canvas/ObjectivePanel (optional, can stay on)

    [Header("Start UI")]
    public TMP_Text startTitleText;      // StartTitleText
    public TMP_Text startDescText;       // StartDescriptionText (your StartModeText)
    public Button startButton;           // StartButton

    [Header("Objective UI")]
    public TMP_Text objectiveTitleText;  // ObjectiveTitleText
    public TMP_Text objectiveDetailText; // ObjectiveDetailText

    [Header("Training Objective")]
    public float targetLow = 98f;
    public float targetHigh = 105f;
    public float holdSecondsToPass = 30f;   // HARD default

    [Header("Scenario")]
    public float startDelaySeconds = 1.0f;
    public float faultInjectTime = 5.0f;
    public float leakRateDuringScenario = 0.3f;

    [Header("Controls")]
    public KeyCode startKey = KeyCode.Space;
    public KeyCode restartKey = KeyCode.R;

    [Header("Pass Visuals")]
    public Color passColor = Color.green;

    private float timeSinceStart;
    private float timeInBand;
    private bool faultInjected;

    private void Reset()
    {
        system = FindFirstObjectByType<GloveboxSystem>();
        faults = FindFirstObjectByType<GloveboxController>();
        hud = FindFirstObjectByType<HUDController>();
        view = FindFirstObjectByType<GloveboxView>();
    }

    private void Start()
    {
        // Wire button if assigned
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(StartScenarioFromUI);
        }

        ShowMenu();
    }

    private void Update()
    {
        if (state == State.Menu)
        {
            if (Input.GetKeyDown(startKey))
                StartScenario();
            return;
        }

        if (Input.GetKeyDown(restartKey))
        {
            ShowMenu();
            return;
        }

        if (state != State.Running || system == null)
            return;

        timeSinceStart += Time.deltaTime;

        if (!faultInjected && timeSinceStart >= faultInjectTime)
            InjectLeak();

        float p = system.PressureKPa;
        bool inBand = (p >= targetLow && p <= targetHigh);

        if (inBand) timeInBand += Time.deltaTime;
        else timeInBand = Mathf.Max(0f, timeInBand - Time.deltaTime * 0.5f);

        UpdateObjectiveUI(p, inBand);

        if (timeInBand >= holdSecondsToPass)
            Pass();
    }

    private void StartScenarioFromUI()
    {
        StartScenario();
    }

    private void ShowMenu()
    {
        state = State.Menu;

        // Panels
        if (startPanel != null) startPanel.SetActive(true);
        if (objectivePanel != null) objectivePanel.SetActive(false);

        // Reset sim to nominal + stop fault
        if (system != null)
        {
            system.enabled = true;
            system.AddPressure(101.3f - system.PressureKPa);
        }

        if (faults != null)
        {
            faults.enabled = true;
            faults.leakEnabled = false;
            faults.leakRateKPaPerSec = leakRateDuringScenario;
        }

        // Clear view override
        if (view != null)
        {
            view.forceColor = false;
        }

        // Clear HUD override
        if (hud != null)
        {
            hud.statusOverrideEnabled = false;
        }

        // Start screen text
        if (startTitleText != null)
            startTitleText.text = "Glovebox Pressure Training";

        if (startDescText != null)
            startDescText.text =
                $"Goal: Hold {targetLow:F0}-{targetHigh:F0} kPa for {holdSecondsToPass:F0}s during a leak.\n" +
                $"Press SPACE or click Start.";

        // Reset timers
        timeSinceStart = 0f;
        timeInBand = 0f;
        faultInjected = false;
    }

    private void StartScenario()
    {
        state = State.Running;

        if (startPanel != null) startPanel.SetActive(false);
        if (objectivePanel != null) objectivePanel.SetActive(true);

        timeSinceStart = 0f;
        timeInBand = 0f;
        faultInjected = false;

        // Ensure sim is running and leak starts OFF
        if (system != null)
        {
            system.enabled = true;
            system.AddPressure(101.3f - system.PressureKPa);
        }

        if (faults != null)
        {
            faults.enabled = true;
            faults.leakEnabled = false;
            faults.leakRateKPaPerSec = leakRateDuringScenario;
        }

        if (objectiveTitleText != null)
            objectiveTitleText.text = $"Objective: Hold {targetLow:F0}-{targetHigh:F0} kPa";

        if (objectiveDetailText != null)
            objectiveDetailText.text =
                $"Leak will inject at {faultInjectTime:F0}s.\n" +
                $"Hold in range for {holdSecondsToPass:F0}s.\n" +
                $"Press R to return to menu.";

        // Optional: small delay before leak logic starts feeling “live”
        if (startDelaySeconds > 0f)
            Invoke(nameof(BeginScenario), startDelaySeconds);
    }

    private void BeginScenario()
    {
        // Nothing required right now, but this gives you an easy hook later
    }

    private void InjectLeak()
    {
        faultInjected = true;
        if (faults != null)
        {
            faults.leakRateKPaPerSec = leakRateDuringScenario;
            faults.leakEnabled = true;
        }
    }

    private void Pass()
    {
        state = State.Passed;

        // Freeze simulation
        if (system != null) system.enabled = false;
        if (faults != null) faults.enabled = false;

        // Force glovebox GREEN
        if (view != null)
        {
            view.forceColor = true;
            view.forcedColor = passColor;
        }

        // Override HUD status to PASSED
        if (hud != null)
        {
            hud.statusOverrideEnabled = true;
            hud.statusOverrideText = "PASSED";
            hud.statusOverrideColor = passColor;
        }

        // Objective text
        if (objectiveTitleText != null)
            objectiveTitleText.text = "Objective Complete";

        if (objectiveDetailText != null)
            objectiveDetailText.text =
                $"PASSED. Held {targetLow:F0}-{targetHigh:F0} kPa for {holdSecondsToPass:F0}s.\n" +
                $"Press R to return to menu.";
    }

    private void UpdateObjectiveUI(float pressure, bool inBand)
    {
        if (objectiveTitleText == null || objectiveDetailText == null)
            return;

        string bandStatus = inBand ? "IN RANGE" : "OUT OF RANGE";
        string leakStatus = faultInjected ? "Leak: ON" : "Leak: pending";
        float pct = Mathf.Clamp01(timeInBand / holdSecondsToPass) * 100f;

        objectiveTitleText.text = $"Objective: Hold {targetLow:F0}-{targetHigh:F0} kPa";

        objectiveDetailText.text =
            $"Pressure: {pressure:F1} kPa ({bandStatus})\n" +
            $"Progress: {pct:F0}% | Time in range: {timeInBand:F1}s / {holdSecondsToPass:F0}s\n" +
            $"{leakStatus}\n" +
            $"Press R to return to menu | Press H for help";
    }
}
