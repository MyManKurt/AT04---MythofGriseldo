using UnityEngine;

public class ITimedPositiveTransmitter : MonoBehaviour, ITransmitter
{
    [SerializeField] GameObject receiverObject;
    IReceiver receiver;

    bool polarity = true;

    //[SerializeField] GameObject polarityIndicatorPositive;

  //  [SerializeField] GameObject polarityIndicatorNegative;

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
            if (countDownTimer <= 0)
            {
                polarity = false;
             //   polarityIndicatorPositive.SetActive(false);
             //   polarityIndicatorNegative.SetActive(true);
                countDownTimer = toggleTime;
            }
        }
        else
        {
            polarity = true;
           // polarityIndicatorPositive.SetActive(true);
          //  polarityIndicatorNegative.SetActive(false);
        }
    }

    private void Countdown()
    {
        if(countDownTimer > 0)
        {
            countDownTimer -= Time.deltaTime;
            if(countDownTimer < 0)
            {
                SignalOut();
            }
        }
    }

}
