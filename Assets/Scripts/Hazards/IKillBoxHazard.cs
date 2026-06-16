using UnityEngine;

public class IKillBoxHazard : MonoBehaviour, IHazard
{
    [TextArea]
    [Header("Developer message:")]
    public string Notes = "This hazard will instantly kill the player when they touch it. \n Use this to make instant kill traps such as bottomless pits or lava, and to create a fail-safe for if the player falls out of the world.";

    [Tooltip("Determines whether this hazard is active or inactive when the scene starts.")]
    [SerializeField] bool hazardActive;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hazardActive)
        {
            if (other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth pHealth))
            {
                pHealth.AdjustHealth(pHealth.GetPlayerHealth());
            }
        }
    }
    public void ToggleHazardActive(bool active)
    {

        hazardActive = active;
    }

}
