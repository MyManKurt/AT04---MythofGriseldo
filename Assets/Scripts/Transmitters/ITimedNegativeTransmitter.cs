using UnityEngine;

public class ITimedNegativeTransmitter : MonoBehaviour, ITransmitter
{
    [SerializeField] GameObject receiverObject;
    IReceiver receiver;

    bool polarity = false;

    [SerializeField] float toggleTime;

    float countDownTimer;

    private void Start()
    {
        receiver = receiverObject.GetComponent<IReceiver>();
       // polarityIndicatorNegative.SetActive(false);
       // polarityIndicatorPositive.SetActive(true);
    }

    private void Update()
    {
        Countdown();
    }

    public void SignalOut()
    {
        // send current value of polarity to 
        receiver.OnSignal(polarity);

        // flip the polarity and change the visual indicator for polarity
        if (polarity == true)
        {
            polarity = false;
           // polarityIndicatorPositive.SetActive(false);
           // polarityIndicatorNegative.SetActive(true);
            
        }
        else
        {
            if (countDownTimer <= 0)
            {
                polarity = true;
               // polarityIndicatorPositive.SetActive(true);
               // polarityIndicatorNegative.SetActive(false);
                countDownTimer = toggleTime;
            }
        }
    }

    private void Countdown()
    {
        if (countDownTimer > 0)
        {
            countDownTimer -= Time.deltaTime;
            if (countDownTimer < 0)
            {
                SignalOut();
            }
        }
    }
}
