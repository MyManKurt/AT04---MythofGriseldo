using UnityEngine;

public interface IInteractable
{
    public void Activate();

    public void ToggleInteractPrompt(bool toggle);
}
