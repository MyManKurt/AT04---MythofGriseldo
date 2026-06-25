using UnityEngine;

public class IContactDamageHazard : MonoBehaviour, IHazard
{

    [TextArea]
    [Header("Developer message:")]
    public string Notes = "This hazard script can be attached to an object with a collider set to 'trigger'. When the player touches it, they will take damage equal to 'DamageAmount' \n Good for ideas like spike traps and lava floors.";

    [Tooltip("The amount of damage that this object will do to the player when they touch it or it touches them.")]
    [SerializeField] int damageAmount;

    AudioSource audioSource;
    [Tooltip("The sound that will play when this object touches the player (note: different to the 'pain' sound from the player!")]
    [SerializeField] AudioClip hitPlayerClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ToggleHazardActive(bool active)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth pHealth = other.GetComponentInParent<PlayerHealth>();

        if (pHealth != null)
        {
            pHealth.AdjustHealth(damageAmount);

            if (audioSource != null && hitPlayerClip != null)
            {
                audioSource.PlayOneShot(hitPlayerClip);
            }

            Debug.Log("Player hit by hazard");
        }
    }
}
