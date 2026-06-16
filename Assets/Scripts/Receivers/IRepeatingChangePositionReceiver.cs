using UnityEngine;
using System.Collections.Generic;

public class IRepeatingChangePositionReceiver : MonoBehaviour, IReceiver
{
    [TextArea]
    [Header("Developer message:")]
    public string Notes = "Attach this script to any game object which you wish to have move. Set up at least two positions for the object to cycle between by adding new Vector3 items to the 'positions' list, and entering the world coordinates which you want the object to move to. \n When receiving a positive signal, the object will head towards the next item in the list, and loop back around to the first item if it reaches the last item. If receiving a negative signal, it will head towards the previous item in the list or loop around to the first item if it has reached the end of the list. \n Usage examples: Moving horizontal platform, moving vertical platform (elevator), sliding door (vertical or horizontal)";


    [Tooltip("all possible positions that the platform can switch between")]
    [SerializeField] List<Vector3> positions = new List<Vector3>();

    [Tooltip("the position that the platform is currently trying to get to / at")]
    private int currentPosition;

    [Tooltip("the speed that the platform moves at")]
    [SerializeField] float objectSlideSpeed;

    [Tooltip("stopping distance for the platform / distance at which it 'snaps' into place")]
    float stoppingDistance = .2f;

    Rigidbody rBody;

    private void Start()
    {
        currentPosition = 0;

        rBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlatformMove();
    }

    public void OnSignal(bool polarity)
    {
        UpdateTargetPlatformPosition(polarity);
    }

    private void UpdateTargetPlatformPosition(bool polarity)
    {
        if (polarity == true)
        {
            if (currentPosition < positions.Count - 1)
            {
                currentPosition++;
            }
            else
            {
                currentPosition = 0;
            }
        }
        else
        {
            if(currentPosition > 0)
            {
                currentPosition--;
            }
            else
            {
                currentPosition = positions.Count - 1;
            }
        }
    }

    private void PlatformMove()
    {
        Vector3 currPosition = positions[currentPosition];

        if (rBody.position != currPosition)
        {
            if(Vector3.Distance(rBody.position, currPosition) < stoppingDistance)
            {
                transform.position = currPosition;
            }
            else
            {
                Vector3 moveDirection = currPosition - transform.position;
                moveDirection = moveDirection.normalized;
                rBody.MovePosition(transform.position + moveDirection * Time.deltaTime * objectSlideSpeed);
            }
        }
    }

}
