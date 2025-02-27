using System.Collections.Generic;
using Project.InteractableSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.PlayerCharacter
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInteraction : MonoBehaviour
    {
        private bool _isInteracting;
        private bool _canInteract = true;
        
        [SerializeField] private Transform interactOrigin;
        
        // Angle from center of viewport to the left and right
        [SerializeField] private float interactionAngle = 25.0f;
        [SerializeField] private float interactionRange = 10.0f;

        private void Awake()
        {
            _isInteracting = false;
            _canInteract = true;
        }

        private void OnSubmit(InputValue submitValue)
        {
            if (!_canInteract)
                return;
            
            if (!submitValue.isPressed) 
                return;
            
            List<Interactable> interactables = FindAllInteractables();
            
            List<Interactable> interactablesInRange = FindInteractablesInRange(interactables);
            List<Interactable> interactablesInAngle = InteractablesInAngle(interactablesInRange);
            
            // TODO: This should find all interactibles of the hightest prio group.
            // Then filter based on angle from viewport center
            Interactable highestPriorityInteractable = GetHighestPriorityInteractable(interactablesInAngle);
                
            if (highestPriorityInteractable != null)
                Interact(highestPriorityInteractable);
        }
        
        private void Interact(Interactable interactable)
        {
            _canInteract = false;
            interactable.Interact(transform);
        }

        #region Event Subscription

        private void OnEnable()
        {
            Interactable.OnInteractionFinished += OnInteractionFinished;
        }
        
        private void OnDisable()
        {
            Interactable.OnInteractionFinished -= OnInteractionFinished;
        }

        #endregion

        #region Event Handlers

        private void OnInteractionFinished()
        {
            _canInteract = true;
        }

        #endregion

        #region Finding Interactables
        // TODO: This is pretty expensive, but it's fine for now
        // TODO: Why the fuck am i not doing a sphere cast???
        private List<Interactable> FindAllInteractables()
        {
            Interactable[] interactables =
                FindObjectsByType<Interactable>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            
            return new List<Interactable>(interactables);
        }

        private List<Interactable> FindInteractablesInRange(List<Interactable> interactables)
        {
            List<Interactable> interactablesInRange = new List<Interactable>();
            
            foreach (var interactable in interactables)
            {
                float distanceToPlayer = Vector3.Distance(interactable.transform.position, transform.position);
                if (distanceToPlayer <= interactionRange)
                    interactablesInRange.Add(interactable);
            }
            
            return interactablesInRange;
        }
        
        private List<Interactable> InteractablesInAngle(List<Interactable> interactables)
        {
            List<Interactable> interactablesInAngle = new List<Interactable>();
            
            foreach (var interactable in interactables)
            {
                Vector3 directionToInteractable = interactable.transform.position - interactOrigin.position;
                float angleToInteractable = Vector3.Angle(interactOrigin.forward, directionToInteractable);
                
                if (angleToInteractable <= interactionAngle)
                    interactablesInAngle.Add(interactable);
            }
            
            return interactablesInAngle;
        }

        private Interactable GetHighestPriorityInteractable(List<Interactable> interactables)
        {
            if (interactables.Count == 0)
                return null;
            
            Interactable highestPriorityInteractable = interactables[0];
            
            foreach (var interactable in interactables)
            {
                if (interactable.Priority > highestPriorityInteractable.Priority)
                    highestPriorityInteractable = interactable;
            }

            return highestPriorityInteractable;
        }
        
        #endregion

        #region Debug Draw
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, interactionRange);

            Gizmos.color = Color.yellow;
            Matrix4x4 temp = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(interactOrigin.position, interactOrigin.rotation, Vector3.one);
            Gizmos.DrawFrustum(Vector3.zero, interactionAngle * 2, interactionRange, 0.1f, 1.0f);
            Gizmos.matrix = temp;
        }
        #endregion
    }
}
