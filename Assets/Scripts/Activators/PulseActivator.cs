using UnityEngine;

public class PulseActivator : MonoBehaviour, IReceiver
{
    [Header("Developer message:")]
    public string Notes = "This Activator will cause the linked Transmitter to send out a signal on a time delay. \n " +
        "It also acts as a Receiver, allowing other Activators to determine whether it is active or not. If it receives a negative signal, the 'pulse' behaviour will stop and it will become idle. If it receives a positive signal, the 'pulse' behaviour will resume. " +
        "\nBy default the pulse is enabled when the scene loads." +
        "\n" +
        "\nUse case examples: Combined with a positive transmitter and link it to a ProjectileHazardSpawner to send out a projectile on a timed interval. Include a SinglePressColliderActivator pressure switch elsewhere in the level to allow the player to deactivate the spawner." +
        "\nOther use case example: Combine with a ToggleTransmitter and link to a ToggleChangePositionReceiver to have a moving platform or elevator which oscillates between positions.";

    ITransmitter transmitter;

    bool pulseActive = true;

    [Tooltip("The time delay between 'pulses', measured in seconds.")]
    [SerializeField] float coolDown;

    private void Awake()
    {
        if (gameObject.TryGetComponent<ITransmitter>(out ITransmitter i))
        {
            transmitter = i;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {      
        InvokeRepeating("PulseTransmit", coolDown, coolDown);
    }


    private void PulseTransmit()
    {
        if (pulseActive)
        {
            transmitter.SignalOut();
        }
    }

    public void OnSignal(bool polarity)
    {
        pulseActive = polarity;
    }
}
