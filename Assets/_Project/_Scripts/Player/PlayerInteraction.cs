using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private Transform interactOrigin;
        
        // Angle from center of viewport to the left and right
        [SerializeField] private float interactionAngle = 25.0f;
        [SerializeField] private float interactionRange = 10.0f;

        private void OnSubmit(InputValue submitValue)
        {
            if (!submitValue.isPressed) 
                return;
            
            List<Interactable> interactables = FindAllInteractables();

            List<Interactable> interactablesInRange = FindInteractablesInRange(interactables);
            List<Interactable> interactablesInAngle = InteractablesInAngle(interactablesInRange);
            Interactable highestPriorityInteractable = GetHighestPriorityInteractable(interactablesInAngle);
                
            highestPriorityInteractable?.Interact(transform);
        }

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
    }
}
