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
        healthBar.fillAmount = (float)playerHealth / healthMax;
    }

    public void AdjustHealth(int amount)
    {
        playerHealth += amount;
        playerHealth = Mathf.Clamp(playerHealth, 0, healthMax);

        healthBar.fillAmount = (float)playerHealth / healthMax;

        if (amount < 0)
        {
            audioSource.PlayOneShot(painClip);
        }
        else
        {
            audioSource.PlayOneShot(healClip);
        }

        if (playerHealth <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }
}
