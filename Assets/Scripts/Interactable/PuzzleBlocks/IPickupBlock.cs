using UnityEngine;
using StarterAssets;

public class IPickupBlock : MonoBehaviour, IBlock
{
    [TextArea]
    [Header("Developer message:")]
    public string Notes = "This object can be carried around by the player, if they walk up to it and press the interact key." +
        "\n Pressing the interact key again will 'throw' the object." +
        "\n" +
        "\nThis object will cause CollisionActivator objects to activate their transmitters as well.";

    private Transform playerCarryPos;
    bool playerLookingAtAndInProximity = false;

    bool playerIsCarrying = false;

    Rigidbody rb;

    [Tooltip("The force which will be applied when the player drops/throws this objet. Higher number = further")]
    [SerializeField] float throwForce;

    [SerializeField] GameObject interactionIndicator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        interactionIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsCarrying) 
        {
            transform.position = playerCarryPos.position;
        }

        if(interactionIndicator.activeSelf && playerIsCarrying)
        {
            interactionIndicator.SetActive(false);
        }
    }

    public void DetectPlayer(bool detected, Transform playerTransform)
    {
        playerLookingAtAndInProximity = detected;
        playerCarryPos = playerTransform;
        interactionIndicator.SetActive(detected);
    }

    public void Interact()
    {
        if (!playerIsCarrying) 
        {
            playerIsCarrying = true;
            rb.isKinematic = true;           
        }
    }

    public void ThrowDrop()
    {
        Debug.Log("Tried to throw the ball away");
        playerIsCarrying = false;
        rb.isKinematic = false;
        rb.AddForce(playerCarryPos.forward * throwForce, ForceMode.VelocityChange);
        
    }

    public bool IsEngagedWithPlayer()
    {
        //a check to see if the player is currently carrying or pushing this object
        return playerIsCarrying;
    }
}
