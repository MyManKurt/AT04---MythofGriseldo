using UnityEngine;

public class IProjectileHazardSpawner : MonoBehaviour, IHazard
{
    [TextArea]
    [Header("Developer message:")]
    public string Notes = "This hazard will send out a projectile which causes damage to the player on contact. \n The damage value should be a negative number so as to actually subtract health from the player. \n To set up in the scene, you need: \n 1. A game object with this script attached, placed where you want the spawner. \n 2. A child object to be assigned as the spawnpoint, pointing in the direction that you want the projectile to go. \n 3. The projectile prefab component assigned to the appropriate slot.";

    [Header("Projectile characteristics")]
    [SerializeField] int projectileDamage;
    [SerializeField] Vector3 projectileVelocity;
    [SerializeField] float projectileSpeed;

    private bool hazardActive;

    [Tooltip("The object which will 'spawn' when this Hazard received a positive signal. Must be assigned from the Project/Assets folder, not from the scene Hierarchy.")]
    [SerializeField] Projectile projectilePrefab;

    [Tooltip("Determines the position and rotation of where the projectile prefab will appear. Must be an empty game object which is a child of the object this script is attached to.")]
    [SerializeField] Transform projectileSpawnPoint;

    AudioSource audioSource;
    [Tooltip("Audio clip which will play when the projectile is launched.")]
    [SerializeField] AudioClip shootProjectileClip;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShootProjectile()
    {
        // instantiate a projectile hazard 
        Projectile projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        projectileVelocity = projectileSpawnPoint.rotation.eulerAngles;

        projectile.ProjectileSetup(projectileDamage, projectileVelocity, projectileSpeed);

        audioSource.PlayOneShot(shootProjectileClip);
    }

    public void ToggleHazardActive(bool active)
    {
        if (active) 
        {
            ShootProjectile();
        }
    }
}
