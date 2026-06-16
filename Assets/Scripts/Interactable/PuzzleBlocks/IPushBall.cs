using UnityEngine;

public class IPushBall : CanTriggerCollisionActivator, IBlock
{
    // player can walk up to this, and while facing it if they hold interact + walk forward the cube will move in one of the 4 cardinal directions.

    private Transform playerPos;

    private Vector3 pushDir;

    bool playerLookingAtAndInProximity;

    [Tooltip("Speed that the ball will roll at if the player starts walking into it.")]
    [SerializeField] float pushSpeed;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Push();
    }

    private void Push()
    {
        if (playerLookingAtAndInProximity)
        {
            float verticalInput = Input.GetAxis("Vertical");
            if (verticalInput > 0)
            {
                Vector3 pushForce = pushDir * pushSpeed * Time.deltaTime;

                rb.AddForce(pushForce, ForceMode.VelocityChange);
            }
        }
    }

    public void Interact()
    {
    }

    public void DetectPlayer(bool detected, Transform playerTransform)
    {
        playerPos = playerTransform;
        playerLookingAtAndInProximity = detected;

        // find the direction to the player
        Vector3 playerDir = playerPos.position - transform.position;

        pushDir = new Vector3(playerDir.x, 0, playerDir.z);

        // flip the direction that the player is coming from
        pushDir *= -1;
    }

    public void ThrowDrop()
    {
        // ability to throw the ball if it can be picked up
    }

    public bool IsEngagedWithPlayer()
    {
        //a check to see if the player is currently carrying or pushing this object
        return playerLookingAtAndInProximity;
    }
}
