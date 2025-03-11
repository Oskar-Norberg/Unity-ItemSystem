using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.InteractableSystem
{
    public class Interactor : MonoBehaviour
    {
        private const int InitialMaxCollidersSize = 64;
        
        public Interactable CurrentlyClosestInteractable => _closestInteractable;
        public bool IsInteracting => _currentlyInteractingInteractable != null;
        
        protected bool CanInteract => _canInteract;
        protected Interactable CurrentlyInteractingInteractable => _currentlyInteractingInteractable ;

        [SerializeField] private Transform interactOrigin;
        
        // Angle from center of viewport to the left and right
        [SerializeField] private float interactionAngle = 17.5f;
        [SerializeField] private float interactionRange = 10.0f;

        private readonly Dictionary<GameObject, Interactable> _cachedInteractables = new();
        private Collider[] _collidersInRange;
        
        private Interactable _closestInteractable;

        private Interactable _currentlyInteractingInteractable;
        private bool _canInteract = true;

        private void Awake()
        {
            _collidersInRange = new Collider[InitialMaxCollidersSize];
            _canInteract = true;
        }
        
        protected void FindInteractables()
        {
            _closestInteractable = null;
            
            List<Interactable> interactablesInRange = GetInteractablesInRange();
            List<Interactable> nonObscuredInteractables = GetNonObscuredInteractables(interactablesInRange);
            Interactable closestInteractable = GetClosestInteractableByAngle(nonObscuredInteractables);

            _closestInteractable = closestInteractable;
        }

        protected void Interact(Interactable interactable)
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
            Tuple<Collider[], int> collidersInRange = GetCollidersInRange();
            List<Interactable> interactablesInRange = new List<Interactable>();

            for (int i = 0; i < collidersInRange.Item2; i++)
            {
                var colliderGameObject = collidersInRange.Item1[i].gameObject;
                
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
                
                if (angleToInteractable > interactionAngle)
                    continue;
                
                interactablesByAngle.Add(new Tuple<Interactable, float>(interactable, angleToInteractable));
            }
            
            if (interactablesByAngle.Count == 0)
                return null;
            
            interactablesByAngle.Sort((a, b) => a.Item2.CompareTo(b.Item2));
            
            return interactablesByAngle[0].Item1;
        }
        
        #endregion

        /**
         * <returns>Tuple with first element as size, second as array of colliders</returns>
         */
        private Tuple<Collider[], int> GetCollidersInRange()
        {
            int hitCount;
            
            while (true)
            {
                hitCount = Physics.OverlapSphereNonAlloc(transform.position, interactionRange, _collidersInRange);

                if (hitCount >= _collidersInRange.Length)
                {
                    _collidersInRange = new Collider[_collidersInRange.Length * 2];
                    continue;
                }

                break;
            }

            return Tuple.Create(_collidersInRange, hitCount);
        }
        
        #region Debug Draw
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, interactionRange);

            Gizmos.color = Color.yellow;
            Matrix4x4 temp = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(interactOrigin.position, interactOrigin.rotation, Vector3.one);
            Gizmos.DrawFrustum(Vector3.zero, interactionAngle, interactionRange, 0.1f, 1.0f);
            Gizmos.matrix = temp;

            if (_closestInteractable)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(interactOrigin.position, _closestInteractable.transform.position);
            }
        }
        #endregion
    }
}
