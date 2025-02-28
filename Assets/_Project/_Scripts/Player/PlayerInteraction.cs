using System.Collections.Generic;
using Project.InteractableSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.PlayerCharacter
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInteraction : MonoBehaviour
    { 
        [SerializeField] private Transform interactOrigin;
        
        // Angle from center of viewport to the left and right
        [SerializeField] private float interactionAngle = 25.0f;
        [SerializeField] private float interactionRange = 10.0f;

        private Dictionary<Collider, Interactable> _cachedInteractables = new Dictionary<Collider, Interactable>();
        
        private bool _isInteracting;
        private bool _canInteract = true;

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
            
            List<Interactable> interactablesInRange = FindInteractablesInRange();
            
            if (interactablesInRange.Count == 0)
                return;
            
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

        private List<Interactable> FindInteractablesInRange()
        {
            print(_cachedInteractables.Count);
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange);
            List<Interactable> interactablesInRange = new List<Interactable>();
            
            foreach (var hitCollider in hitColliders)
            {
                if (_cachedInteractables.TryGetValue(hitCollider, out var cachedInteractable))
                {
                    interactablesInRange.Add(cachedInteractable);
                    continue;
                }
                
                if (hitCollider.gameObject.TryGetComponent<Interactable>(out var interactableComponent))
                    interactablesInRange.Add(interactableComponent);
                
                _cachedInteractables.Add(hitCollider, interactableComponent);
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
