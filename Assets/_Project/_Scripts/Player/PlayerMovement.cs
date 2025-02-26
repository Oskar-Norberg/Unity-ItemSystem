using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Transform cameraPivot;
        
        private Rigidbody _rigidbody;
        private Vector2 wishDirection;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        private void OnMove(InputValue movementValue) {
            Vector2 movementVector = movementValue.Get<Vector2>();
            wishDirection.x = movementVector.x;
            wishDirection.y = movementVector.y;
        }
        
        private void OnLook(InputValue lookValue) {
            Vector2 lookVector = lookValue.Get<Vector2>();
            cameraPivot.Rotate(Vector3.right, lookVector.y);
            transform.Rotate(Vector3.up, lookVector.x);
        }
        
        private void FixedUpdate() {
            Move();
        }

        private void Move()
        {
            if (wishDirection == Vector2.zero)
                return;
            
            Vector3 moveDirection = new Vector3(wishDirection.x, 0, wishDirection.y);
            
            _rigidbody.linearVelocity = moveDirection;
        }
    }
}
