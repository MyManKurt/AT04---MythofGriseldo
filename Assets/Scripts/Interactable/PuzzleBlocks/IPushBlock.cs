using UnityEngine;

public class IPushBlock : CanTriggerCollisionActivator, IBlock
{
    // player can walk up to this, and while facing it if they hold interact + walk forward the cube will move in one of the 4 cardinal directions.

    private Transform playerPos;

    private Vector3 pushDir;

    bool playerLookingAtAndInProximity;

    [Tooltip("Speed the block will slide at when the player starts walking into it.")]
    [SerializeField] float pushSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
                transform.position += pushDir * pushSpeed * Time.deltaTime;
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

        pushDir = new Vector3 (playerDir.x, 0, playerDir.z);

        // flip the direction that the player is coming from
        pushDir *= -1;        
    }

    public void ThrowDrop()
    {
        //what happens when the player releases the object
    }

    public bool IsEngagedWithPlayer()
    {
        //a check to see if the player is currently carrying or pushing this object
        return playerLookingAtAndInProximity;
    }
}
