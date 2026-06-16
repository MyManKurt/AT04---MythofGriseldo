using UnityEngine;

public class IToggleChangeRotationReceiver : MonoBehaviour, IReceiver
{

    [TextArea]
    [Header("Developer message:")]
    public string Notes = "This receiver script will cause the attached object to pivot between two different rotations. \n Good for doors and certain types of platform.";

    [Tooltip("The value for the rotation that the object will start at (open)")]
    [SerializeField] Vector3 rotationA;
    [Tooltip("the value it will transition to when receiving a negative signal(closed)")]
    [SerializeField] Vector3 rotationB;

    [SerializeField] bool startAtRotationA;

    private bool isOpen;

    private bool isRotating;

    [Tooltip("Speed at which the object will rotate at")]
    [SerializeField] float rotationSpeed;
 
    private enum RotationAxis { xAxis, yAxis, zAxis}
    [SerializeField] RotationAxis rotationAxis;

    //Rigidbody rBody;
    //Vector3 eulerAngleVelocity;

    private void Start()
    {
        isOpen = startAtRotationA;
        
    }

    private void Update()
    {
        if (isRotating)
        {
            ObjectRotate();
        }
    }


    public void OnSignal(bool polarity)
    {
        if (polarity != isOpen)
        {
            isOpen = polarity;
            isRotating = true;
        }
    }


    public void ObjectRotate()
    {
        //play opening animation
        if (isOpen == true)
        {
            if (transform.rotation != Quaternion.Euler(rotationA))
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotationA), rotationSpeed * Time.deltaTime);

            }
            else
            {
                isRotating = false;
            }
        }
        else
        {
            if (transform.rotation != Quaternion.Euler(rotationB))
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotationB), rotationSpeed * Time.deltaTime);
                                
            }
            else
            {
                isRotating = false;
            }
        }
    }

}
