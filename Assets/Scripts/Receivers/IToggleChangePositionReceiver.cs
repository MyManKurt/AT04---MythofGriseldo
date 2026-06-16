using UnityEngine;

public class IToggleChangePositionReceiver : MonoBehaviour, IReceiver
{
    [TextArea]
    [Header("Developer message:")]
    public string Notes = "Attach this script to any game object which you wish to have move that can be manually opened and closed." +
        "\nWhen receiving a positive signal, it will move to openPosition." +
        "\nWhen receiving a negatibe signal, it will move to the closedPosition. " +
        "\nUsage examples: Moving horizontal platform, moving vertical platform (elevator), sliding door (vertical or horizontal)";


    [SerializeField] Vector3 positionA;

    [SerializeField] Vector3 positionB;

    [SerializeField] bool startAtPositionA;

    bool isAtPositionA;

    [SerializeField] float objectSlideSpeed;

    float stoppingDistance = 0.2f;

    //Rigidbody rBody;

    bool isStopped = false;

    private void Start()
    {
        isAtPositionA = startAtPositionA;

        //rBody = GetComponent<Rigidbody>();
    }

    public void OnSignal(bool polarity)
    {
        if (polarity != isAtPositionA)
        {
            isAtPositionA = polarity;
            isStopped = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isStopped == false)
        {
            DoorSlide();
        }
    }


    public void DoorSlide()
    {

        if (isAtPositionA == true)
        {
            if (transform.localPosition != positionA)
            {
                if (Vector3.Distance(transform.localPosition, positionA) < stoppingDistance)
                {
                    transform.localPosition = positionA;
                    isStopped = true;
                }
                else
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, positionA, objectSlideSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            if (transform.localPosition != positionB)
            {
                if (Vector3.Distance(transform.localPosition, positionB) < stoppingDistance)
                {
                    transform.localPosition = positionB;
                    isStopped = true;
                }
                else
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, positionB, objectSlideSpeed * Time.deltaTime);
                }
            }
        }

        /*

        if(isAtPositionA == true)
        {
            if(transform.position != positionA)
            {
                if(Vector3.Distance(rBody.position, positionA) < stoppingDistance)
                {
                    rBody.position = positionA;
                    _currentSlideSpeed = 0;
                }
                else
                {
                    Vector3 moveDirection = positionA - transform.position;
                    moveDirection = moveDirection.normalized;
                    rBody.MovePosition(transform.position + moveDirection  * Time.fixedDeltaTime * _currentSlideSpeed);    
                }         
             }

         }
         else
         {
             if (transform.position != positionB)
             {
                 if (Vector3.Distance(rBody.position, positionB) < stoppingDistance)
                 {
                    //Debug.Log("Stopping at B");
                    rBody.position = positionB;                    
                 }
                 else
                 {
                    //Debug.Log("Trying to get to PositionB");
                    Vector3 moveDirection = positionB - transform.position;
                    moveDirection = moveDirection.normalized;
                    rBody.MovePosition(transform.position + moveDirection * Time.fixedDeltaTime * _currentSlideSpeed);   
                }
            }            
        }*/
    }

    /*
        private void OnTriggerEnter(Collider other)
        {
            //set the parent of the player to this object
            if(other.TryGetComponent<CharacterController>(out CharacterController _cTroller))
            {
               cTroller = _cTroller;
            }


            //set the parent of the player to this object
            other.transform.parent = transform;
            Debug.Log($"Capturing {other.name} on the moving platform");

        }

        private void OnTriggerExit(Collider other)
        {
            //remove this object as the player object's parent
            //set the parent of the player to this object
            if (other.TryGetComponent<CharacterController>(out CharacterController _cTroller))
            {
                if(_cTroller == cTroller)
                {
                    cTroller = null;
                }
            }
            //remove this object as the player object's parent
            other.transform.parent = null;
            Debug.Log($"Releasing {other.name} on the moving platform");

        }*/

}
