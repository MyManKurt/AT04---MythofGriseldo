using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class PlayerInteract : MonoBehaviour
{
    IInteractable interactable;
    IBlock block;

   // [SerializeField] float interactRange;

    //[SerializeField] Transform blockInteractDetectionStart;
    //[SerializeField] float blockInteractRange;

    [SerializeField] Transform blockCarryingPoint;

    private PlayerInput _playerInput;
    private StarterAssetsInputs _input;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _input = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        //FindInteractable();
        //FindBlock();

        if (_input.interact)
        {
            Debug.Log("Interact input working!");
            TryInteracting();            
        }
    }

    private void TryInteracting()
    {
        if (interactable != null)
        {
            interactable.Activate();
        }
        else if (block != null)
        {
            if (!block.IsEngagedWithPlayer())
            {
                block.Interact();
            }
            else
            {
                block.ThrowDrop();
            }
        }
        
        _input.interact = false;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable _interactable;
        if (other.TryGetComponent<IInteractable>(out _interactable))
        {
            interactable = _interactable;
            interactable.ToggleInteractPrompt(true);
        }

        IBlock _block;
        if(other.TryGetComponent<IBlock>(out _block))
        {
            if (block != null)
            {
                if (_block != block) { block.DetectPlayer(false, blockCarryingPoint); }
                //Debug.Log("Assigning new block...");
                block = _block;
            }
            else
            {
                //Debug.Log("No Block assigned, assigning...");
                block = _block;
            }
            _block.DetectPlayer(true, blockCarryingPoint);
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        IInteractable _interactable;
        if (other.TryGetComponent<IInteractable>(out _interactable))
        {
            if (interactable == _interactable)
            {
                interactable.ToggleInteractPrompt(false);
                interactable = null;
            }
        }

        IBlock _block;
        if (other.TryGetComponent<IBlock>(out _block))
        {
                //Debug.Log("No block detected, clearing old Block...");
                _block.DetectPlayer(false, blockCarryingPoint);
                block = null;
        }
    }
}


