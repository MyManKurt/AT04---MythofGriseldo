using UnityEngine;

public class ToggleColliderActivator : MonoBehaviour
{
    [TextArea]
    public string DeveloperNote = "This Activator will detect any object with a 'CanTriggerCollisionActivator' component that moves into it's trigger collider. " +
        "\nOnce it has activated the attached transmitter, it will activate it again if the player / other object that is 'pressing' it moves away. " +
        "\nUse case example: A door which requires a moveable box to be placed on this Activator to keep the door open.";

    //[SerializeField] GameObject transmitterObject;
    [SerializeField] ITransmitter transmitter;

    bool pressed = false;

    [Tooltip("Optional use: The default position of the Activator object. It will return to this position when reset.")]
    [SerializeField] Vector3 activatorReadyPosition;
    [Tooltip("Optional use: The position that the Activator object will move to the Activation has happened. EG, a pressure pad that disappears into the ground when stepped on.")]
    [SerializeField] Vector3 activatorUsedPosition;
    [Tooltip("Optional use: The speed that the Activator object will move between PositionA and PositionB \n Leave this at 0 if you don't want it to move.")]
    [SerializeField] float activatorMoveSpeed;
    [Tooltip("The distance which the Activator object will stop smoothly transitioning between positions when moving and will just snap to it's target position.")]
    [SerializeField] float snapDistance;

    AudioSource audioSource;
    [Tooltip("The audio clip which will play when this Activator is interacted with.")]
    [SerializeField] AudioClip activatedClip;
    [Tooltip("The audio clip which will play when this Activator resets.")]
    [SerializeField] AudioClip resetClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (gameObject.TryGetComponent<ITransmitter>(out ITransmitter i))
        {
            transmitter = i;
        }
    }

    private void Update()
    {
        if (pressed) 
        {
            if(Vector3.Distance(transform.position, activatorUsedPosition) > snapDistance) 
            {
                transform.position = Vector3.Slerp(transform.position, activatorUsedPosition, activatorMoveSpeed * Time.deltaTime);  
            }
            else
            {
                transform.position = activatorUsedPosition;
            }

        }
        else
        {
            if (Vector3.Distance(transform.position, activatorReadyPosition) > snapDistance)
            {
                transform.position = Vector3.Slerp(transform.position, activatorReadyPosition, activatorMoveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = activatorReadyPosition;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!pressed)
        {
            if (other.gameObject.TryGetComponent<CanTriggerCollisionActivator>
                (out CanTriggerCollisionActivator t))
            {
                transmitter.SignalOut();
                pressed = true;
                audioSource.PlayOneShot(activatedClip);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (pressed)
        {
            if (other.gameObject.TryGetComponent<CanTriggerCollisionActivator>
                (out CanTriggerCollisionActivator t))
            {
                transmitter.SignalOut();
                pressed = false;
                audioSource.PlayOneShot(resetClip);
            }
        }
    }
}
