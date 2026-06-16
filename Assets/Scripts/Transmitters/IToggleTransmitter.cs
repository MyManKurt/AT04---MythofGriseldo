using UnityEngine;

public class IToggleTransmitter : MonoBehaviour, ITransmitter
{
    [SerializeField] GameObject receiverObject;
    [SerializeField] IReceiver receiver;

    bool polarity = true;

   // [SerializeField] GameObject polarityIndicatorPositive;

   // [SerializeField] GameObject polarityIndicatorNegative;

    private void Start()
    {
        receiver = receiverObject.GetComponent<IReceiver>();
    }


    public void SignalOut()
    {
        // send current value of polarity to 
        receiver.OnSignal(polarity);

        // flip the polarity and change the visual indicator for polarity
        
        if (polarity == true)
        {
            polarity = false;
            //polarityIndicatorPositive.SetActive(false);
            //polarityIndicatorNegative.SetActive(true);
        }
        else
        {
            polarity = true;
           //polarityIndicatorPositive.SetActive(true);
           // polarityIndicatorNegative.SetActive(false);
        }
    }
}
