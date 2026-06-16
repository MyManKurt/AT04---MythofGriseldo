using UnityEngine;

public class IHazardReceiver : MonoBehaviour, IReceiver
{
    [TextArea]
    [Header("Developer message:")]
    public string Notes = "This script must be attached to an object which has a hazard script attached to it as well.";

    private IHazard hazard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hazard = GetComponent<IHazard>();
    }


    public void OnSignal(bool polarity)
    {        
        hazard.ToggleHazardActive(polarity);
    }
}
