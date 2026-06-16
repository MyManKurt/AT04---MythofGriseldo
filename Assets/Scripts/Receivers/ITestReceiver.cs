using UnityEngine;

public class ITestReceiver : MonoBehaviour, IReceiver
{
    [SerializeField] MeshRenderer testIndicator;

    [SerializeField] Material positiveIndicator;
    [SerializeField] Material negativeIndicator;

    public void OnSignal(bool polarity)
    {
        if (polarity)
        {
            testIndicator.material = positiveIndicator;
            Debug.Log("Positive signal received");
        }
        else
        {
            testIndicator.material = negativeIndicator;
           // Debug.Log("Negative signal received");
        }
    }
}
