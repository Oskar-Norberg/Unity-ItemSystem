using System;
using System.Collections.Generic;
using Project.InteractableSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.PlayerCharacter
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInteraction : MonoBehaviour
    {
        public Interactable CurrentlyClosestInteractable => _closestInteractable;
        public bool IsInteracting => _currentlyInteractingInteractable != null;

        [SerializeField] private Transform interactOrigin;
        
        // Angle from center of viewport to the left and right
        [SerializeField] private float interactionAngle = 17.5f;
        [SerializeField] private float interactionRange = 10.0f;

        private Dictionary<GameObject, Interactable> _cachedInteractables = new Dictionary<GameObject, Interactable>();
        
        private Interactable _closestInteractable;

        private Interactable _currentlyInteractingInteractable;
        private bool _canInteract = true;

        private void Awake()
        {
            _canInteract = true;
        }

        private void OnSubmit(InputValue submitValue)
        {
            if (!_canInteract)
                return;
            
            if (!submitValue.isPressed) 
                return;
            
            if (!_closestInteractable)
                return;
            
            Interact(_closestInteractable);
        }

        private void Update()
        {
            FindInteractables();
        }

        // TODO: Isn't this really expensive to run every frame?
        // Has a negligible performance impact
        // Could be optimized by making it a coroutine or a timer that runs every x amounts of seconds
        // However it's fine for now
        private void FindInteractables()
        {
            _closestInteractable = null;
            
            List<Interactable> interactablesInRange = GetInteractablesInRange();
            List<Interactable> nonObscuredInteractables = GetNonObscuredInteractables(interactablesInRange);
            Interactable closestInteractable = GetClosestInteractableByAngle(nonObscuredInteractables);

            _closestInteractable = closestInteractable;
        }

        private void Interact(Interactable interactable)
        {
            _canInteract = false;
            _currentlyInteractingInteractable = interactable;
            _currentlyInteractingInteractable.OnInteractionFinished += OnInteractionFinished;
            interactable.Interact(transform);
        }
        
        private void OnInteractionFinished()
        {
            _currentlyInteractingInteractable.OnInteractionFinished -= OnInteractionFinished;
            _canInteract = true;
        }

        #region Finding Interactables

        private List<Interactable> GetInteractablesInRange()
        {
            Tuple<int, Collider[]> collidersInRange = GetCollidersInRange();
            List<Interactable> interactablesInRange = new List<Interactable>();

            for (int i = 0; i < collidersInRange.Item1; i++)
            {
                var colliderGameObject = collidersInRange.Item2[i].gameObject;
                
                if (_cachedInteractables.TryGetValue(colliderGameObject, out var cachedInteractable))
                {
                    if (cachedInteractable == null)
                        continue;
                    
                    interactablesInRange.Add(cachedInteractable);
                    continue;
                }
                
                if (colliderGameObject.gameObject.TryGetComponent<Interactable>(out var interactableComponent))
                    interactablesInRange.Add(interactableComponent);
                
                _cachedInteractables.Add(colliderGameObject, interactableComponent);
            }

            return interactablesInRange;
        }

        private List<Interactable> GetNonObscuredInteractables(List<Interactable> interactables)
        {
            List<Interactable> nonObscuredInteractables = new List<Interactable>();

            foreach (var interactable in interactables)
            {
                Vector3 directionToInteractable = interactable.transform.position - interactOrigin.position;
                float distanceToInteractable = directionToInteractable.magnitude;

                if (!Physics.Raycast(interactOrigin.position, directionToInteractable, out var hit, distanceToInteractable)) 
                    continue;
                
                if (hit.collider.gameObject != interactable.gameObject)
                    continue;
                
                nonObscuredInteractables.Add(interactable);
            }

            return nonObscuredInteractables;
        }

        private Interactable GetClosestInteractableByAngle(List<Interactable> interactables)
        {
            List<Tuple<Interactable, float>> interactablesByAngle = new ();
            
            foreach (var interactable in interactables)
            {
                Vector3 directionToInteractable = interactable.transform.position - interactOrigin.position;
                float angleToInteractable = Vector3.Angle(interactOrigin.forward, directionToInteractable);
                
                interactablesByAngle.Add(new Tuple<Interactable, float>(interactable, angleToInteractable));
            }
            
            interactablesByAngle.Sort((a, b) => a.Item2.CompareTo(b.Item2));
            
            if (interactablesByAngle.Count == 0)
                return null;
            
            if (interactablesByAngle[0].Item2 > interactionAngle)
                return null;
            
            return interactablesByAngle[0].Item1;
        }
        
        #endregion

        /**
         * <returns>Tuple with first element as size, second as array of colliders</returns>
         */
        private Tuple<int, Collider[]> GetCollidersInRange()
        {
            Collider[] hitCollider = new Collider[64];
            int hitCount;
            
            while (true)
            {
                hitCount = Physics.OverlapSphereNonAlloc(transform.position, interactionRange, hitCollider);

                if (hitCount >= hitCollider.Length)
                {
                    hitCollider = new Collider[hitCollider.Length * 2];
                    continue;
                }

                break;
            }

            return Tuple.Create(hitCount, hitCollider);
        }
        
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
