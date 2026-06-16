using UnityEngine;

public class ISwingingContactHazard : MonoBehaviour, IHazard
{

    [TextArea]
    [Header("Developer message:")]
    public string Notes = "This hazard script can be attached to an object which will then oscillate between the 'RotationTargets' in the list. Only two rotation targets can be put in the list. A collider set to 'trigger' should be attached to the game object at the 'pointy' part of the model -- alternatively, multiple child objects with IContactDamageHazard can be added to add multiple damage contact points. \n Good for making pendulum blade hazards or spiked gates.";

    [Tooltip("Amount of damage to do to the player if they get hit")]
    [SerializeField] int damageAmount;

    [Tooltip("Positions that the swinging trap will oscillate between (Two maxi")]
    //the two rotations that the swinging trap will oscillate between.]
    [SerializeField] Vector3[] rotationTargets;

    private Vector3 targetRotation;

    // states: active and not active
    [Tooltip("Determines if the hazard is active. Tick this on to have it active as the scene starts, and send a negative signal to stop it.")]
    [SerializeField] bool hazardActive;

    [Tooltip("Delay in seconds before swinging again after one swing of the pendulum is completed.")]
    [SerializeField] float swingTimer;
    float timerMax;

    [Tooltip("Speed at which the pendulum object will swing.")]
    [SerializeField] float swingSpeed;
    float currentSwingSpeed;

    AudioSource audioSource;
    [Tooltip("Audio to play each time the object begins a swing.")]
    [SerializeField] AudioClip swingClip;
    [Tooltip("Audio to play each time the object strikes the player")]
    [SerializeField] AudioClip hitPlayerClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timerMax = swingTimer;
        currentSwingSpeed = swingSpeed;
        targetRotation = rotationTargets[0];
    }

    private void Update()
    {
        // oscillate between rotations
        Oscillate();

    }

    private void Oscillate()
    {
        if (transform.rotation != Quaternion.Euler(targetRotation))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(targetRotation), currentSwingSpeed * Time.deltaTime);
        }
        else
        {
            if (targetRotation == rotationTargets[0])
            {
                targetRotation = rotationTargets[1];
                currentSwingSpeed = 0;
                swingTimer = timerMax;
            }
            else
            {
                targetRotation = rotationTargets[0];
                currentSwingSpeed = 0;
                swingTimer = timerMax;
            }
            audioSource.PlayOneShot(swingClip);

        }

        if (swingTimer > 0)
        {
            if (hazardActive)
            {
                swingTimer -= Time.deltaTime;
            }
        }
        else
        {
            currentSwingSpeed = swingSpeed;
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth pHealth))
        {
            pHealth.AdjustHealth(damageAmount);
            audioSource.PlayOneShot(hitPlayerClip);
        }
    }

    public void ToggleHazardActive(bool active)
    {
        hazardActive = active;
    }
}
