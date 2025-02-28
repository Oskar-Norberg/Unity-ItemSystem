using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.PlayerCharacter
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Transform cameraPivot;

        [SerializeField] private float cameraSensitivity = 1.0f;
        [SerializeField] private float movementSpeed = 5.0f;
        
        private Rigidbody _rigidbody;
        private Vector2 wishDirection;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            // TODO: This is very temporary, should happen in a GameManager
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        private void OnMove(InputValue movementValue) {
            Vector2 movementVector = movementValue.Get<Vector2>() ;
            wishDirection.x = movementVector.x;
            wishDirection.y = movementVector.y;
        }
        
        private void OnLook(InputValue lookValue) {
            Vector2 lookVector = Time.deltaTime * cameraSensitivity * lookValue.Get<Vector2>();

            Look(lookVector);
        }
        
        private void FixedUpdate() {
            _rigidbody.angularVelocity = Vector3.zero;
            
            Move();
        }

        private void Move()
        {
            if (wishDirection == Vector2.zero)
                return;
            
            Vector3 moveDirection = transform.rotation * new Vector3(wishDirection.x, 0, wishDirection.y);
            Vector3 force = moveDirection.normalized * movementSpeed;
            
            if (_rigidbody.linearVelocity.magnitude > movementSpeed)
                return;
            
            _rigidbody.AddForce(force, ForceMode.Acceleration);
        }

        private void Look(Vector2 direction)
        {
            cameraPivot.Rotate(Vector3.right, -direction.y);
            transform.Rotate(Vector3.up, direction.x);
        }
    }
}
