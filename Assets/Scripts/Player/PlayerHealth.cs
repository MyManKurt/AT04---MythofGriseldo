using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{   

    int playerHealth;

    [SerializeField] private int healthMax;

    AudioSource audioSource;
    [SerializeField] AudioClip painClip;
    [SerializeField] AudioClip healClip;

    [SerializeField] Image healthBar;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerHealth = healthMax;
        healthBar.fillAmount = playerHealth / 100;
    }

    public void AdjustHealth(int amount)
    {
        if(amount < 0)
        {
            audioSource.PlayOneShot(painClip);
        }
        else
        {
            audioSource.PlayOneShot(healClip);
        }
        playerHealth += amount;
        healthBar.fillAmount = playerHealth / 100;
        //Debug.Log("Health adjusted to " + playerHealth);

        if(playerHealth < 0)
        {
            //game over
            SceneManager.LoadScene("GameOverScene");
        }
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }
}
