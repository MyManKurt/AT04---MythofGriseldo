using UnityEngine;

public class TimedResetColliderActivator : MonoBehaviour
{
    [TextArea]
    public string DeveloperNote = "This Activator will detect any object with a 'CanTriggerCollisionActivator' component that moves into it's trigger collider. " +
        "\nOnce it has activated the attached transmitter, a timer will start, and once the timer has finished this Activator can be used again." +
        "\nUse case: Combine with 'Timed' Transmitters to create behaviour where an object will perform an action, and then reset after a delay then allowing the player to try again. " +
        "\nEG: A door which opens when a pressure plate is stepped on, but will close after 10 seconds. The player can activate it again if they failed to get through the door the first time.";

    //detect when the player steps on it, and reset after a delay

    //[SerializeField] GameObject transmitterObject;
    ITransmitter transmitter;

    bool pressed = false;

    [Tooltip("The number of seconds to wait before the Activator is available again.")]
    [SerializeField] float resetTimer;
    float timer;

    [Tooltip("Optional use: The default position of the Activator object. It will return to this position when the timer runs out.")]
    [SerializeField] Vector3 positionA;
    [Tooltip("Optional use: The position that the Activator object will move to the Activation has happened. EG, a pressure pad that disappears into the ground when stepped on.")]
    [SerializeField] Vector3 positionB;
    [Tooltip("Optional use: The speed that the Activator object will move between PositionA and PositionB")]
    [SerializeField] float lerpSpeed;
    [Tooltip("The distance which the Activator object will stop smoothly transitioning between positions when moving and will just snap to it's target position.")]
    [SerializeField] float snapDistance;

    AudioSource audioSource;
    [Tooltip("The audio clip which will play when this Activator is interacted with.")]
    [SerializeField] AudioClip activatedClip;
    [Tooltip("The audio clip which will play when this Activator resets.")]
    [SerializeField] AudioClip resetClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (gameObject.TryGetComponent<ITransmitter>(out ITransmitter i))
        {
            transmitter = i;
        }
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            pressed = false;
            audioSource.PlayOneShot(resetClip);
        }

        if (pressed)
        {
            if (Vector3.Distance(transform.position, positionB) > snapDistance)
            {
                transform.position = Vector3.Slerp(transform.position, positionB, lerpSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = positionB;
            }

        }
        else
        {
            if (Vector3.Distance(transform.position, positionA) > snapDistance)
            {
                transform.position = Vector3.Slerp(transform.position, positionA, lerpSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = positionA;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!pressed)
        {
            if (other.gameObject.TryGetComponent<CanTriggerCollisionActivator>
                (out CanTriggerCollisionActivator t))
            {
                timer = resetTimer;
                transmitter.SignalOut();
                pressed = true;
                audioSource.PlayOneShot(activatedClip);
            }
        }
    }
}
