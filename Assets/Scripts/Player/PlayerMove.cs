using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Cinemachine;

public class PlayerMove : CanTriggerCollisionActivator
{
    //references to the camera, cinemachine brain and cinemachine freelook component
    [SerializeField] private Transform cam;
    [SerializeField]  private CinemachineBrain brain;    

    //variables referencing the audio source and audio prefab for footstep and jump
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip footClip;
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip oofClip;
    [SerializeField] AudioClip crunchClip;

    //variable for changing parameters on the rigidbody
    private Rigidbody rbody;

    //variable for adjusting the rate at which the character falls
    public float gravityModifier = 2.5f;
    public float lowJumpMultipier = 2f;

    //The base move speed of the character and turn speed of the camera
    public float moveSpeed = 1f;
    public float turnSpeed = 45f;
    public float jumpHeight = 2f;

    //varaibes for handling the character turning relative to the direction the camera is pointing
    public float turnSmoothTime = 1f;
    private float turnSmoothVelocity;

    //The move speed, turn speed and jump height with the buffs you have from leveling up.   
    public float currentMoveSpeed;
    public float currentTurnSpeed;
    public float currentJumpHeight;

    //position which the player will be teleported to when respawning
    private Vector3 spawn;

    //reference to the animation manager script
    // TEMP_REMOVE private AnimationManager animManager;

    //variables for handling the movement of the player
    private Vector3 motion;
    private Vector2 input;

    private void Awake()
    {
        //set the cam variable
        if (cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
    }

    private void Start()
    {
        //set the spawn point of the player to wherever the character model is placed in the Unity inspector
        SetRespawn();


        // set the CinemachineBrain variable
        if (!cam.TryGetComponent<CinemachineBrain>(out brain))
        {
            Debug.LogError("You need a Cinemachine brain component on the main camera!");
        }
        //set the rigidbody variable
        if (!TryGetComponent<Rigidbody>(out rbody))
        {
            Debug.LogError("You need a Rigidbody component on this game object!");
        }
    }

    private void Update()
    {
        // Check spacebar to trigger jumping. Checks if vertical velocity (eg velocity.y) is near to zero.

        if (Input.GetButtonDown("Jump") == true)
        {
            Jump(1, 1);
        }

        //checks if the player is current descending and increases their fall rate by the gravityModifier
        if (rbody.linearVelocity.y < 0)
        {
            rbody.linearVelocity += Vector3.up * Physics.gravity.y * (gravityModifier - 1) * Time.deltaTime;
        }
        //checks if tthe player is still holding down jump button; if not, the effect of gravity is increased so they perform a smaller 'hop'
        else if (Mathf.Abs(rbody.linearVelocity.y) > 0.01f && !Input.GetButton("Jump"))
        {
            rbody.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultipier - 1) * Time.deltaTime;
        }

        // Rotation and movement speed is modifed by the level (currentMoveSpeed) of the player and by the time between update frames (Time.deltaTime). 


        // Move player via horizontal / vertical inputs             
        //take the axis of the inputs and assign them to part of 'input' vector 2
        input.x = Input.GetAxis("Vertical");
        input.y = Input.GetAxis("Horizontal");
        motion = new Vector3(input.y, 0, input.x).normalized; //combine the inputs into a single vector 3

        //check if any inputs are active
        if (motion.magnitude >= 0.1)
        {
            //turn the character towards the directions dicated by the inputs (the combined average of the two directions) modified by the current direction of the third person camera
            float targetAngle = Mathf.Atan2(motion.x, motion.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); //smooth the rotation angle for the player to face in the direction they are moving
            transform.rotation = Quaternion.Euler(0f, angle, 0f); //rotate the character to face the direction they are moving relative to the direction of the camera
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // calculate the direction to move the character

            //update the character's position based on the inputs received modified by the current movement speed
            transform.position += new Vector3(moveDir.x, 0, moveDir.z) * currentMoveSpeed * Time.deltaTime;

            //update the rigidbody's velocity according to the inputs received

            //check if the player is currently falling; if not, switch the animation from idle to running           

        }        
    }

    //self contained method for jumping which other scripts can call with applied modifiers
    public void Jump(float jumpMod, int jumpType)
    {
        rbody.linearVelocity += Vector3.up * this.currentJumpHeight * jumpMod;
        if (jumpType == 1)
        {
            //animManager.JumpAnim();
            source.clip = null;
            source.PlayOneShot(jumpClip);
        }
        else if (jumpType == 2)
        {
            //animManager.Ouchie();
            source.clip = null;
            source.PlayOneShot(oofClip);
        }
    }

    //method to update the player's respawn position to their current transform position
    public void SetRespawn()
    {
        spawn = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    }

    //method for handling different death 'types' 
    public void Death(int deathType)
    {
        //deathType 1 = falling with lives left
        //deathType 2 = falling no lives left
        //deathType 3 = died to 


        if (deathType == 1)
        {
            StartCoroutine(FallRespawn());
        }
        else if (deathType == 2)
        {
            StartCoroutine(FallDeath());
        }
        else
        {
            StartCoroutine(DamageDeath());
        }
    }

    //enumerator method for respawning the player when they fall, including stopping the camera from chasing them down and activating a 'death screen' overlay
    IEnumerator FallRespawn()
    {
        //decouple the camera from the player
        brain.enabled = false;

        //show the 'YOU DIED' ui element

        //wait a few seconds
        yield return new WaitForSeconds(3);

        //clear 'YOU DIED' ui element

        //move the player 
        rbody.linearVelocity = Vector3.zero; //prevents the player clipping through the ground when they respawn
        this.transform.position = spawn;


        //recouple the camera to the player
        brain.enabled = true;
    }

    //enumerator for when the player falls to their death without any health/lives remaining
    IEnumerator FallDeath()
    {
        //disable the camera's cinemachine brain so it doesn't follow them through the fog
        brain.enabled = false;

        //show the 'YOU DIED' ui element
        // deathScreen.SetActive(true); TEMP_DISABLE

        //wait a few seconds
        yield return new WaitForSeconds(3);

        //clear 'YOU DIED' ui element

        //recouple the camera to the player
        brain.enabled = true;

        //reload the entire scene
        SceneManager.LoadScene("GameScene");
    }

    //enumerator for when the player dies due to damage from enemies or traps
    IEnumerator DamageDeath()
    {
        //disable the camera brain so it doesn't follow the player (if they end up falling off the edge)
        brain.enabled = false;

        //spawn the ragdoll        
        // RagDoll();  TEMP_DISABLE

        //show the 'YOU DIED' ui element
        // deathScreen.SetActive(true);  TEMP_DISABLE

        //wait a few seconds
        yield return new WaitForSeconds(3);

        //clear 'YOU DIED' ui element

        //recouple the camera to the player
        brain.enabled = true;

        //reload the game scene to start over
        SceneManager.LoadScene("GameScene");
    }

    //ontriggerenter method for performing a 'goomba stomp' on the enemies.
    private void OnTriggerEnter(Collider other)
    {
        /* all this is the logic for goomba stomping
        // Enemy enemy;  TEMP_DISABLE
        //execute the 'death' method on the enemy.
        if (other.TryGetComponent<Enemy>(out enemy))
        {
            if (animManager.animator.GetBool("flinching") == false)
            {
                //call the death function on the enemy being stomped
                enemy.Death();
                source.clip = null;
                source.PlayOneShot(crunchClip);
                Jump(1.5f, 1);
            }

        } */
    }
}
    /*

    //reference to character controller
    private CharacterController controller;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    [SerializeField] private float turnSpeed = 10.0f;

    [SerializeField] Transform camContainerTransform;

    private Vector3 camContainerRight;
    private Vector3 camContainerForward;

    AudioSource audioSource;
    [SerializeField] AudioClip jumpClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();        
    }

    private void Update()
    {
        Movement();    
        //StickPlayerToMovingGround();
    }

    // player presses left arrow or right arrow / right thumbstick input
    // character steers / turns in the direction of the arrow input


    private void Movement()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, 2f))
        {
            GameObject hitObj = hit.collider.gameObject;
            Debug.Log($"Buttray detected {hitObj.name}");

            if (hitObj.layer == 3)
            {
                groundedPlayer = true;
            }            
        }
        else
        {
            groundedPlayer = false;
        }

       // groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        

        // Horizontal input
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            if (Input.GetAxisRaw("Vertical") == 0)
            {
                //Debug.Log("No input from player, updating camera direction...");
                camContainerRight = camContainerTransform.right;
                camContainerForward = camContainerTransform.forward;
            }
        }

        Vector3 move = camContainerRight * xInput + camContainerForward * zInput;

        // Jump
        if (Input.GetButton("Jump") && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            audioSource.PlayOneShot(jumpClip);
        }

        //turn in direction of movement input
        if(move != Vector3.zero && groundedPlayer)
        {
            transform.forward = Vector3.Slerp(transform.forward, move, turnSpeed * Time.deltaTime);
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Combine horizontal and vertical movement
        Vector3 finalMove = (move * playerSpeed) + (playerVelocity.y * Vector3.up);
        controller.Move(finalMove * Time.deltaTime);
    }


    private void StickPlayerToMovingGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, 2f))
        {            
            GameObject hitObj = hit.collider.gameObject;
            Debug.Log($"Buttray detected {hitObj.name}");

            if (hitObj.TryGetComponent<MagnetizePlayerToSurface>(out MagnetizePlayerToSurface mag))
            {
                        Debug.Log($"{hitObj.name} is now the parent");
                        transform.SetParent(hitObj.transform);
               
            }
            else
            {
                Debug.Log("Ground is not dynamic, clearing parent.");
                transform.parent = null;
                //transform.SetParent();
            }
        }
        else
        {
            Debug.Log("We're airborne, clearing parent.");
            transform.parent = null;
        }
    }
}
    */
