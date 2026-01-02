using UnityEngine;

public class GloveboxSystem : MonoBehaviour
{
    [Header("System State")]
    [SerializeField] private float pressureKPa = 101.3f;

    [Header("Models")]
    public PressureModel pressureModel = new PressureModel();
    public AirflowModel airflowModel = new AirflowModel();

    [Header("Limits")]
    public float minPressure = 95f;
    public float maxPressure = 110f;

    public float PressureKPa => pressureKPa;
    public float Inflow => airflowModel.GetInflow();
    public float Outflow => airflowModel.GetOutflow();


    public void AddPressure(float deltaKPa)
    {
        pressureKPa = Mathf.Clamp(pressureKPa + deltaKPa, minPressure, maxPressure);
    }
    public void ResetToNominal()
    {
        pressureKPa = 101.3f;

        // Put flows back to default “nominal”
        airflowModel.SetInflow(1f);
        airflowModel.SetOutflow(1f);
    }

    private void Update()
    {
        airflowModel.TickInput(Time.deltaTime);

        float inflow = airflowModel.GetInflow();
        float outflow = airflowModel.GetOutflow();

        pressureKPa = pressureModel.Step(pressureKPa, inflow, outflow, Time.deltaTime);
        pressureKPa = Mathf.Clamp(pressureKPa, minPressure, maxPressure);
    }
}
