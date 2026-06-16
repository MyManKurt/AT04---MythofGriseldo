using UnityEngine;

public interface IBlock
{
    public void Interact();

    public void DetectPlayer(bool detected, Transform playerTransform);

    public void ThrowDrop();

    public bool IsEngagedWithPlayer();
}
