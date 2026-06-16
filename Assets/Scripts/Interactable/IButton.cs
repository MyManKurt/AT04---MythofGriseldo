using UnityEngine;

public class IButton : MonoBehaviour, IInteractable
{
    [Tooltip("The object which will appear if the player is within interaction range and facing this object.")]
    [SerializeField] GameObject interactPrompt;
    
    [SerializeField] ITransmitter transmitter;

    AudioSource audioSource;
    [Tooltip("Audio to play when the player presses this button.")]
    [SerializeField] AudioClip buttonPressedClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        interactPrompt.SetActive(false);
        transmitter = GetComponent<ITransmitter>();        
    }

    public void Activate()
    {
        transmitter.SignalOut();
        audioSource.PlayOneShot(buttonPressedClip);
    }

    public void ToggleInteractPrompt(bool toggle)
    {
        interactPrompt.SetActive(toggle);
    }
}
