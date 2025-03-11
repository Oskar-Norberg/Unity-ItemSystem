using Project.InteractableSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.PlayerCharacter
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInteractor : Interactor
    { 
        private void OnSubmit(InputValue submitValue)
        {
            if (!CanInteract)
                return;
            
            if (!submitValue.isPressed) 
                return;
            
            if (!CurrentlyClosestInteractable)
                return;
            
            Interact(CurrentlyClosestInteractable);
        }

        // TODO: Isn't this really expensive to run every frame?
        // Has a negligible performance impact
        // Could be optimized by making it a coroutine or a timer that runs every x amounts of seconds
        // However it's fine for now
        private void Update()
        {
            FindInteractables();
        }
    }
}
