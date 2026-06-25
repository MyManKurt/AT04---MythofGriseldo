using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    // The name of the scene that will load when the player wins.
    // This must match the scene name in Build Settings exactly.
    [SerializeField] private string winSceneName = "WinScene";

    // Stops the trigger from loading the win scene more than once.
    private bool hasWon = false;

    private void OnTriggerEnter(Collider other)
    {
        // If the player has already won, stop this code from running again.
        if (hasWon) return;

        // Checks if the object entering the trigger is the player,
        // or part of the player object.
        if (other.GetComponentInParent<PlayerHealth>() != null)
        {
            // Mark the win as triggered.
            hasWon = true;

            // Load the win scene.
            SceneManager.LoadScene(winSceneName);
        }
    }
}