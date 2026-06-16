using UnityEngine;

public class INegativeTransmitter : MonoBehaviour, ITransmitter
{
    [SerializeField] GameObject receiverObject;
    private IReceiver receiver;

    bool polarity = false;

    private void Start()
    {
        receiver = receiverObject.GetComponent<IReceiver>();
    }

    public void SignalOut()
    {
        receiver.OnSignal(polarity);
    }
}
