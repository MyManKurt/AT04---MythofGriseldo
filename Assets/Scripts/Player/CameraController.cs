using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Camera object
    [SerializeField] GameObject camObject;

    // object which the camera is interpolating towards
    [SerializeField] private Transform followTarget;

    [SerializeField] private Transform cameraOffsetObject;
    private Vector3 cameraOffsetPosition;

    //height that the camera should try to maintain
    [SerializeField] private float followCamHeight;

    // camera interpolation speed
    [SerializeField] private float cameraSpeed;

    // stopping distance
    [SerializeField] private float stoppingDistance;

    //camera look target
    [SerializeField] private Transform lookTarget;

    //camera rotation speed
    [SerializeField] private float rotationSpeed;

    public bool avoidWall = false;

    SphereCollider sCollider;
    //BoxCollider bCollider;

    Vector3 cameraCollisionPoint;
    private GameObject cameraCollisionObject;

    private void Start()
    {                
        sCollider = GetComponent<SphereCollider>();
       // bCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        cameraOffsetPosition = cameraOffsetObject.position;

        CameraFollow();
        camObject.transform.LookAt(lookTarget);
        
        int rotDir = (int)Input.GetAxis("RotateCam");
        RotateCameraVertical(rotDir);

        /*if (Input.GetButton("SnapCamera"))
        {
            SnapCameraBehindPlayer();
        }*/
        
    }

    private void CameraFollow()
    {
     
        transform.position = Vector3.Slerp(transform.position, followTarget.position, cameraSpeed * Time.deltaTime);

        if (avoidWall)
        {
            if (cameraCollisionPoint != null) 
            {
                if (Vector3.Distance(cameraCollisionPoint, cameraOffsetObject.position) > 1.5f)
                {
                    Debug.Log("camera offset object has gotten away from wall");
                    Vector3 dir = followTarget.transform.position - cameraOffsetObject.transform.position;
                    RaycastHit hit;
                    if (Physics.Raycast(cameraOffsetObject.transform.position, dir, out hit, float.MaxValue))
                    {
                       // Debug.Log($"Raycast has hit: {hit.collider.name}");
                        if (hit.collider.gameObject != cameraCollisionObject)
                        {
                            avoidWall = false;
                            camObject.transform.position = Vector3.Lerp(camObject.transform.position, transform.position + new Vector3(0, followCamHeight, 0), 0.5f * Time.deltaTime);
                        }
                    }
                }
                if (Vector3.Distance(camObject.transform.position, cameraCollisionPoint) < 1.5f)
                {
                    camObject.transform.position = Vector3.Lerp(camObject.transform.position, transform.position + new Vector3(0, followCamHeight, 0), 0.5f * Time.deltaTime);
                    //sCollider.center = camObject.transform.localPosition;
                }  
            }
            else
            {
                camObject.transform.position = Vector3.Lerp(camObject.transform.position, transform.position + new Vector3(0, followCamHeight, 0), 0.5f * Time.deltaTime);
                Vector3 dir = followTarget.transform.position - cameraOffsetObject.transform.position;
                RaycastHit hit;
                if (Physics.Raycast(cameraOffsetObject.transform.position, dir, out hit, float.MaxValue))
                {
                   // Debug.Log($"Raycast has hit: {hit.collider.name}");
                    if (hit.collider.gameObject == followTarget.gameObject)
                    {
                        avoidWall = false;
                    }
                }
            }
            
        }
        else
        {
            camObject.transform.position = Vector3.Lerp(camObject.transform.position, cameraOffsetPosition, 0.5f * Time.deltaTime);
            //  sCollider.center = camObject.transform.localPosition;

            Vector3 dir = followTarget.transform.position - cameraOffsetObject.transform.position;
            RaycastHit hit;
            if (Physics.Raycast(cameraOffsetObject.transform.position, dir, out hit, float.MaxValue))
            {
               // Debug.Log($"Raycast has hit: {hit.collider.name}");
                if (hit.collider.gameObject != followTarget.gameObject)
                {
                    avoidWall = true;
                }
            }
        }       
    }

    private void RotateCameraVertical(int rotDir)
    {
        transform.Rotate(Vector3.up, 5 * rotDir * rotationSpeed * Time.deltaTime);        
    }

    private void SnapCameraBehindPlayer()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, followTarget.rotation, rotationSpeed * Time.deltaTime);
    }

    private void ResetAvoidWallCountdown()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        cameraCollisionPoint = sCollider.ClosestPoint(other.transform.position);
        cameraCollisionObject = other.gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        avoidWall = true;
        cameraCollisionPoint = sCollider.ClosestPoint(other.transform.position);
    }

    private void OnTriggerExit(Collider other)
    {
        RaycastHit hit;
        Vector3 dir = cameraOffsetObject.transform.position - followTarget.transform.position;
        if(Physics.Raycast(cameraOffsetObject.transform.position, dir, out hit, float.MaxValue))
        {
            if(!hit.collider.gameObject == cameraCollisionObject)
            {
                avoidWall = false;
            }
        }        
    }

}
