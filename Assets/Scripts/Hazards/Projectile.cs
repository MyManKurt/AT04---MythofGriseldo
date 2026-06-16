using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;

    private Vector3 velocity;

    private float speed;

    AudioSource audioSource;
    [Tooltip("Audio to be played when the projectile hits the player.")]
    [SerializeField] AudioClip hitPlayerClip;
    [Tooltip("Audio to be played when the projectile hits the environment.")]
    [SerializeField] AudioClip hitEnvironmentClip;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void ProjectileSetup(int dmg, Vector3 vel, float spd)
    {
        damage = dmg;
        velocity = vel;
        speed = spd;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Hit an object");

        if (collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            //Debug.Log("The object is the player character");
            playerHealth.AdjustHealth(damage);
            audioSource.PlayOneShot(hitPlayerClip);
            gameObject.SetActive(false);
        }
        audioSource.PlayOneShot(hitEnvironmentClip);
        gameObject.SetActive(false);
    }
}
