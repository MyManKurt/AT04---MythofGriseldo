using UnityEngine;

public class IPositiveTransmitter : MonoBehaviour, ITransmitter
{
    [SerializeField] GameObject receiverObject;
    private IReceiver receiver;

    bool polarity = true;

    private void Start()
    {
        receiver = receiverObject.GetComponent<IReceiver>();
    }

    public void SignalOut()
    {
        receiver.OnSignal(polarity);
    }

}
