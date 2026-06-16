using StarterAssets;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.TryGetComponent<ThirdPersonController>(out ThirdPersonController tPersonCtrl))
        {
            Debug.Log("Captured " + other.name);
            tPersonCtrl.SetPlatform(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<ThirdPersonController>(out ThirdPersonController tPersonCtrl))
        {
            Debug.Log("Releasing " + other.name);
            tPersonCtrl.SetPlatform(null);
        }
    }*/

}
