using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLoader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!SceneManager.GetSceneByName("PlayerAndCameraScene").isLoaded) 
        {
            SceneManager.LoadSceneAsync("PlayerAndCameraScene", LoadSceneMode.Additive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
