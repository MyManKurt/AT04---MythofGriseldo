using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class ISpringLoadedHazard : MonoBehaviour, IHazard
{

    [TextArea]
    [Header("Developer message:")]
    public string Notes = "This hazard script will 'snap' to the 'closed' position at a speed determined by 'HatchSpeed'. \n Could be used for making a trapdoor, or combined with IContactDamageHazard to make some kind of 'spring loaded' trap which fires once.";

    //snaps open and drop the player

    // the open and closed rotation values
    [Tooltip("The rotation value of the object when it is in the closed position.")]
    [SerializeField] Vector3 closedRotation;
    [Tooltip("The rotation value of the object when it is in the open position.")]
    [SerializeField] Vector3 openRotation;

    Vector3 targetRotation;

    [Tooltip("The speed that the object will rotate at.")]
    [SerializeField] float hatchSpeed;

    bool hatchMoving;

    AudioSource audioSource;
    [Tooltip("The audio which will play when the hazard is activated.")]
    [SerializeField] AudioClip activatedClip;
    [Tooltip("The audio which will play when the hazard is reset.")]
    [SerializeField] AudioClip resetClip;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        hatchMoving = false;
        targetRotation = openRotation;
    }

    private void Update()
    {
        HatchMoving();
    }

    private void HatchMoving()
    {
        if (hatchMoving)
        {
            if (transform.rotation != Quaternion.Euler(targetRotation))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation), hatchSpeed * Time.deltaTime);
            }
            else
            {
                hatchMoving = false;
            }
        }
    }

    public void ToggleHazardActive(bool active)
    {
        hatchMoving = true;

        if(active)
        {
            targetRotation = openRotation;
            audioSource.PlayOneShot(activatedClip);
        }
        else
        {
            targetRotation = closedRotation;
            audioSource.PlayOneShot(resetClip);
        }
    }
}
